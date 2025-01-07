using System;
using System.Linq;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Domain.Uow;
using Abp.Runtime.Session;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using Azure.Data.Tables;
using Microsoft.Extensions.Configuration;
using Icon.Configuration;
using System.Collections.Generic;
using Icon.Matrix.Models;
using Icon.Matrix.Coingecko;


namespace Icon.Matrix
{
    public interface ITokenPoolManager : IDomainService
    {
        Task ImportRaydiumPoolUpdates();
    }
    public class TokenPoolManager : IconServiceBase, ITokenPoolManager
    {
        private readonly ICoingeckoService _coingeckoService;
        private readonly IRepository<CoingeckoPoolUpdate, Guid> _coingeckoPoolUpdateRepository;
        private readonly IRepository<RaydiumPair, Guid> _raydiumPairRepository;

        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private IConfigurationRoot _configuration;

        public TokenPoolManager(
            ICoingeckoService coingeckoService,
            IRepository<CoingeckoPoolUpdate, Guid> coingeckoPoolUpdateRepository,
            IRepository<RaydiumPair, Guid> raydiumPairRepository,

            IUnitOfWorkManager unitOfWorkManager,
            IAppConfigurationAccessor appConfigurationAccessor)
        {
            _coingeckoService = coingeckoService;
            _coingeckoPoolUpdateRepository = coingeckoPoolUpdateRepository;
            _raydiumPairRepository = raydiumPairRepository;

            _unitOfWorkManager = unitOfWorkManager;
            _configuration = appConfigurationAccessor.Configuration;
        }

        public async Task ImportRaydiumPoolUpdates()
        {
            var raydiumPairs = await _raydiumPairRepository
                .GetAll()
                .Where(x => x.Slot > 0 && x.LastPoolUpdate == null || x.LastPoolUpdate < DateTime.UtcNow.AddHours(-6))
                .ToListAsync();

            foreach (var raydiumPair in raydiumPairs)
            {
                if (string.IsNullOrEmpty(raydiumPair.BaseTokenAccount))
                {
                    continue;
                }

                CoingeckoPoolsResponse coingeckoPoolsResponse = null;
                try
                {
                    coingeckoPoolsResponse = await _coingeckoService.GetPoolsAsync(raydiumPair.BaseTokenAccount);
                }
                catch (Exception ex)
                {
                    Logger.Error("Error getting coingecko pools for " + raydiumPair.BaseTokenAccount, ex);
                    continue;
                }


                if (coingeckoPoolsResponse == null || coingeckoPoolsResponse.Data == null)
                {
                    continue;
                }

                foreach (var coingeckoPoolUpdate in coingeckoPoolsResponse.Data)
                {
                    if (coingeckoPoolUpdate.Attributes == null)
                    {
                        continue;
                    }

                    var entity = CoingeckoPoolConverter.ToCoingeckoPoolEntity(coingeckoPoolUpdate, raydiumPair.Id);

                    if (entity == null)
                    {
                        continue;
                    }

                    using (var uow = _unitOfWorkManager.Begin())
                    {
                        raydiumPair.LastPoolUpdate = DateTime.UtcNow;

                        await _raydiumPairRepository.UpdateAsync(raydiumPair);
                        await _coingeckoPoolUpdateRepository.InsertAsync(entity);
                        await _unitOfWorkManager.Current.SaveChangesAsync();
                        uow.Complete();
                    }
                }
            }
        }
    }

    public static class CoingeckoPoolConverter
    {
        public static CoingeckoPoolUpdate ToCoingeckoPoolEntity(
            CoingeckoPoolData data,
            Guid? raydiumPairId = null
        )
        {
            if (data == null) return null;

            var attr = data.Attributes;

            var entity = new CoingeckoPoolUpdate
            {
                RaydiumPairId = raydiumPairId,
                PoolId = data.Id,
                PoolType = data.Type,

                BaseTokenPriceUsd = attr?.BaseTokenPriceUsd,
                BaseTokenPriceNativeCurrency = attr?.BaseTokenPriceNativeCurrency,
                QuoteTokenPriceUsd = attr?.QuoteTokenPriceUsd,
                QuoteTokenPriceNativeCurrency = attr?.QuoteTokenPriceNativeCurrency,
                BaseTokenPriceQuoteToken = attr?.BaseTokenPriceQuoteToken,
                QuoteTokenPriceBaseToken = attr?.QuoteTokenPriceBaseToken,

                Address = attr?.Address,
                Name = attr?.Name,
                PoolCreatedAt = attr?.PoolCreatedAt,
                TokenPriceUsd = attr?.TokenPriceUsd,
                FdvUsd = attr?.FdvUsd,
                MarketCapUsd = attr?.MarketCapUsd,

                PriceChangeM5 = attr?.PriceChangePercentage?.M5,
                PriceChangeH1 = attr?.PriceChangePercentage?.H1,
                PriceChangeH6 = attr?.PriceChangePercentage?.H6,
                PriceChangeH24 = attr?.PriceChangePercentage?.H24,

                M5Buys = attr?.Transactions?.M5?.Buys,
                M5Sells = attr?.Transactions?.M5?.Sells,
                M5Buyers = attr?.Transactions?.M5?.Buyers,
                M5Sellers = attr?.Transactions?.M5?.Sellers,

                M15Buys = attr?.Transactions?.M15?.Buys,
                M15Sells = attr?.Transactions?.M15?.Sells,
                M15Buyers = attr?.Transactions?.M15?.Buyers,
                M15Sellers = attr?.Transactions?.M15?.Sellers,

                M30Buys = attr?.Transactions?.M30?.Buys,
                M30Sells = attr?.Transactions?.M30?.Sells,
                M30Buyers = attr?.Transactions?.M30?.Buyers,
                M30Sellers = attr?.Transactions?.M30?.Sellers,

                H1Buys = attr?.Transactions?.H1?.Buys,
                H1Sells = attr?.Transactions?.H1?.Sells,
                H1Buyers = attr?.Transactions?.H1?.Buyers,
                H1Sellers = attr?.Transactions?.H1?.Sellers,

                H24Buys = attr?.Transactions?.H24?.Buys,
                H24Sells = attr?.Transactions?.H24?.Sells,
                H24Buyers = attr?.Transactions?.H24?.Buyers,
                H24Sellers = attr?.Transactions?.H24?.Sellers,

                VolumeM5 = attr?.VolumeUsd?.M5,
                VolumeH1 = attr?.VolumeUsd?.H1,
                VolumeH6 = attr?.VolumeUsd?.H6,
                VolumeH24 = attr?.VolumeUsd?.H24,

                ReserveInUsd = attr?.ReserveInUsd,

                BaseTokenId = data.Relationships?.BaseToken?.Data?.Id,
                QuoteTokenId = data.Relationships?.QuoteToken?.Data?.Id,
                DexId = data.Relationships?.Dex?.Data?.Id
            };

            return entity;
        }
    }
}