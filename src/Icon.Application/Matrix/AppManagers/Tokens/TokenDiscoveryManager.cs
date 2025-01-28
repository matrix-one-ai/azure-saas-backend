using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Domain.Uow;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Icon.Configuration;
using Icon.Matrix.Coingecko;
using Icon.Matrix.Models;
using Icon.Matrix.Twitter;
using Icon.EntityFrameworkCore.Matrix;
using Abp.Json;
using System.Net.WebSockets;
using Tweetinvi.Core.Iterators;
using System.Linq.Dynamic.Core.Tokenizer;
using System.Runtime.InteropServices;
using System.Transactions;

namespace Icon.Matrix.TokenDiscovery
{
    public interface ITokenDiscoveryManager : IDomainService
    {
        Task FullDiscovery();
    }

    public class TokenDiscoveryManager : IconServiceBase, ITokenDiscoveryManager
    {
        private readonly ICoingeckoService _coingeckoService;
        private readonly ITwitterAPICommunicationService _twitterAPICommunicationService;

        private readonly IRepository<CoingeckoPoolUpdate, Guid> _coingeckoPoolUpdateRepository;
        private readonly IRepository<CoingeckoAggregatedUpdate, Guid> _coingeckoAggregatedUpdateRepository;
        private readonly IRepository<TwitterImportTweetEngagement, Guid> _twitterImportTweetEngagementRepository;
        private readonly IRepository<TwitterImportTweetCount, Guid> _twitterImportTweetCountRepository;
        private readonly IRepository<RaydiumPair, Guid> _raydiumPairRepository;

        private readonly IMatrixBulkRepository<CoingeckoPoolUpdate> _coingeckoPoolUpdateBulkRepository;
        private readonly IMatrixBulkRepository<CoingeckoAggregatedUpdate> _coingeckoAggregatedUpdateBulkRepository;
        private readonly IMatrixBulkRepository<RaydiumPair> _raydiumPairBulkRepository;
        private readonly IMatrixBulkRepository<TwitterImportTweetEngagement> _twitterImportTweetEngagementBulkRepository;
        private readonly IMatrixBulkRepository<TwitterImportTweetCount> _twitterImportTweetCountBulkRepository;

        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IConfigurationRoot _configuration;

        private static readonly List<string> _raydiumPairPropsToUpdate = new List<string>
        {
            nameof(RaydiumPair.DiscoveryStageName),
            nameof(RaydiumPair.DiscoveryStageLastUpdated),
            nameof(RaydiumPair.TotalLiquidityUsd),
            nameof(RaydiumPair.TweetsCATweetCount),
            nameof(RaydiumPair.TweetsCATweetCount1H),
            nameof(RaydiumPair.TweetsCATweetCount3H),
            nameof(RaydiumPair.TweetsCATweetCount6H),
            nameof(RaydiumPair.TweetsCATweetCount12H),
            nameof(RaydiumPair.TweetsCATweetCount24H),
            nameof(RaydiumPair.TweetsCATweetCount1WK),
            nameof(RaydiumPair.TwitterCAQueryCount),
            nameof(RaydiumPair.TwitterCAFirstMentionTime),
            nameof(RaydiumPair.TwitterCAFound),
            nameof(RaydiumPair.TwitterCAFoundAtTime),
            nameof(RaydiumPair.TwitterCAFirstMentionTweetId),
            nameof(RaydiumPair.TwitterCAFirstMentionText),
            nameof(RaydiumPair.TwitterCAFirstMentionHandle),
            nameof(RaydiumPair.TwitterCAMostLikesOnSingleTweet),
            nameof(RaydiumPair.TwitterCAMostRepliesOnSingleTweet),
            nameof(RaydiumPair.TweetsCAEngagementTweetsImported),
            nameof(RaydiumPair.TweetsCAEngagementTotalLikes),
            nameof(RaydiumPair.TweetsCAEngagementTotalReplies),
            nameof(RaydiumPair.TweetsCAEngagementTotalRetweets),
            nameof(RaydiumPair.TweetsCAEngagementTotalQuotes),
            nameof(RaydiumPair.PriceRefreshLastUpdateTime),

            // Extended metric fields:
            nameof(RaydiumPair.VlrValue),
            nameof(RaydiumPair.VlrQualitativeAnalysis),
            nameof(RaydiumPair.VlrRecommendation),

            nameof(RaydiumPair.BuySellRatioValue),
            nameof(RaydiumPair.BuySellRatioQualitativeAnalysis),
            nameof(RaydiumPair.BuySellRatioRecommendation),

            nameof(RaydiumPair.VolumeToFdv1HValue),
            nameof(RaydiumPair.VolumeToFdv1HQualitativeAnalysis),
            nameof(RaydiumPair.VolumeToFdv1HRecommendation),

            nameof(RaydiumPair.CombinedMetricScore),
            nameof(RaydiumPair.CombinedQualitativeAnalysis),
            nameof(RaydiumPair.CombinedRecommendation),

            nameof(RaydiumPair.EngagementCorrelationAnalysis),
            nameof(RaydiumPair.EngagementCorrelationScore),
            nameof(RaydiumPair.PlantFinalSummary),
            nameof(RaydiumPair.PlantFinalPrediction),

            // Link to aggregated update
            nameof(RaydiumPair.CoingeckoLastAggregatedUpdateId)
        };

        public TokenDiscoveryManager(
            ICoingeckoService coingeckoService,
            ITwitterAPICommunicationService twitterAPICommunicationService,

            IRepository<CoingeckoPoolUpdate, Guid> coingeckoPoolUpdateRepository,
            IRepository<CoingeckoAggregatedUpdate, Guid> coingeckoAggregatedUpdateRepository,
            IRepository<TwitterImportTweetEngagement, Guid> twitterImportTweetEngagementRepository,
            IRepository<TwitterImportTweetCount, Guid> twitterImportTweetCountRepository,
            IRepository<RaydiumPair, Guid> raydiumPairRepository,

            IMatrixBulkRepository<CoingeckoPoolUpdate> coingeckoPoolUpdateBulkRepository,
            IMatrixBulkRepository<CoingeckoAggregatedUpdate> coingeckoAggregatedUpdateBulkRepository,
            IMatrixBulkRepository<RaydiumPair> raydiumPairBulkRepository,
            IMatrixBulkRepository<TwitterImportTweetEngagement> twitterImportTweetEngagementBulkRepository,
            IMatrixBulkRepository<TwitterImportTweetCount> twitterImportTweetCountBulkRepository,

            IUnitOfWorkManager unitOfWorkManager,
            IAppConfigurationAccessor appConfigurationAccessor
        )
        {
            _coingeckoService = coingeckoService;
            _twitterAPICommunicationService = twitterAPICommunicationService;

            _coingeckoPoolUpdateRepository = coingeckoPoolUpdateRepository;
            _coingeckoAggregatedUpdateRepository = coingeckoAggregatedUpdateRepository;
            _twitterImportTweetEngagementRepository = twitterImportTweetEngagementRepository;
            _twitterImportTweetCountRepository = twitterImportTweetCountRepository;
            _raydiumPairRepository = raydiumPairRepository;

            _coingeckoPoolUpdateBulkRepository = coingeckoPoolUpdateBulkRepository;
            _coingeckoAggregatedUpdateBulkRepository = coingeckoAggregatedUpdateBulkRepository;
            _raydiumPairBulkRepository = raydiumPairBulkRepository;
            _twitterImportTweetEngagementBulkRepository = twitterImportTweetEngagementBulkRepository;
            _twitterImportTweetCountBulkRepository = twitterImportTweetCountBulkRepository;

            _unitOfWorkManager = unitOfWorkManager;
            _configuration = appConfigurationAccessor.Configuration;
        }

        public async Task FullDiscovery()
        {
            Logger.Info("Starting FullDiscovery...");
            var inactiveStages = TokenStageDefinitions.InactiveStages;

            // int batchSize = 10;
            // int skip = 0;
            // while (true)
            // {
            //     var activePairs = await _raydiumPairRepository
            //         .GetAll()
            //         .Where(x => !inactiveStages.Contains(x.DiscoveryStageName))
            //         .OrderBy(x => x.Id)
            //         .Skip(skip)
            //         .Take(batchSize)
            //         .ToListAsync();

            //     if (!activePairs.Any()) break;

            //     foreach (var pair in activePairs)
            //     {
            //         await ProcessSinglePairAsync(pair);
            //     }

            //     skip += batchSize;
            // }

            var activePairs = await _raydiumPairRepository
                .GetAll()
                .Where(x => !inactiveStages.Contains(x.DiscoveryStageName))
                .ToListAsync();

            foreach (var pair in activePairs)
            {
                var unitOfWorkOptions = new UnitOfWorkOptions
                {
                    Scope = TransactionScopeOption.RequiresNew,
                    IsTransactional = true
                };

                using (var uow = _unitOfWorkManager.Begin(unitOfWorkOptions))
                {

                    await ProcessSinglePairAsync(pair);
                    await uow.CompleteAsync();
                }

                Logger.Info("FullDiscovery completed.");
            }
        }

        /// <summary>
        /// Process a single RaydiumPair in its own UoW scope so we can commit
        /// changes and keep memory usage & transaction duration small.
        /// </summary>
        private async Task ProcessSinglePairAsync(RaydiumPair pair)
        {

            try
            {
                if (string.IsNullOrWhiteSpace(pair.DiscoveryStageName))
                {
                    SetStage(pair, TokenStageDefinitions.Stages.Inception);
                }

                // If not "death," do price update
                if (pair.DiscoveryStageName != TokenStageDefinitions.Stages.Death)
                {
                    await UpdatePriceForPair(pair, TokenStageDefinitions.Definitions[TokenStageDefinitions.Stages.PriceTracking]);
                    CheckAndPerformStageTransition(pair);
                }

                // Stage logic loop
                while (true)
                {
                    var oldStage = pair.DiscoveryStageName;
                    await ApplyStageLogic(pair);

                    CheckAndPerformStageTransition(pair);
                    if (pair.DiscoveryStageName == oldStage ||
                        pair.DiscoveryStageName == TokenStageDefinitions.Stages.Death)
                    {
                        break;
                    }
                }

                await _raydiumPairBulkRepository.BulkInsertOrUpdateIncludeAsync(new List<RaydiumPair> { pair }, _raydiumPairPropsToUpdate);

            }
            catch (Exception ex)
            {
                Logger.Error($"Error processing pair {pair.Id}", ex);
                // Optionally handle or rethrow. UoW automatically won't commit
                // if we don't call CompleteAsync().
            }
        }


        #region Stage Logic
        private async Task ApplyStageLogic(RaydiumPair pair)
        {
            switch (pair.DiscoveryStageName)
            {
                case var s when s == TokenStageDefinitions.Stages.Death:
                    Logger.Debug($"Pair {pair.Id} is in Death stage, skipping logic.");
                    break;

                case var s when s == TokenStageDefinitions.Stages.Inception:
                    Logger.Debug($"Pair {pair.Id} is in Inception stage, no direct logic here.");
                    break;

                case var s when s == TokenStageDefinitions.Stages.PriceTracking:
                    Logger.Debug($"Pair {pair.Id} is in PriceTracking stage, no extra logic after price update.");
                    break;

                case var s when s == TokenStageDefinitions.Stages.EngagementPostTracking:
                    Logger.Debug($"Pair {pair.Id} in EngagementPostTracking => updating tweet counts...");
                    await UpdateTweetCountsForPair(pair, TokenStageDefinitions.Definitions[TokenStageDefinitions.Stages.EngagementPostTracking]);
                    break;

                case var s when s == TokenStageDefinitions.Stages.EngagementDetailTracking:
                    Logger.Debug($"Pair {pair.Id} in EngagementDetailTracking => updating tweet counts & engagement...");
                    await UpdateTweetCountsForPair(pair, TokenStageDefinitions.Definitions[TokenStageDefinitions.Stages.EngagementPostTracking]);
                    await UpdateTweetEngagementForPair(pair, TokenStageDefinitions.Definitions[TokenStageDefinitions.Stages.EngagementDetailTracking]);
                    break;

                default:
                    Logger.Warn($"Pair {pair.Id} has unknown stage {pair.DiscoveryStageName}. Doing nothing.");
                    break;
            }
        }

        private void CheckAndPerformStageTransition(RaydiumPair pair)
        {
            var currentStageName = pair.DiscoveryStageName;
            var minRequirements = new TokenTrackingRequirements(minLiquidity: 20000, maxTokenAgeHours: 3);
            var liquidityUsd = pair.TotalLiquidityUsd ?? 0;

            if (liquidityUsd < minRequirements.MinLiquidity)
            {
                SetStage(pair, TokenStageDefinitions.Stages.Death);
                return;
            }

            if (DateTimeOffset.UtcNow > pair.CreationTime.AddHours(minRequirements.MaxTokenAgeHours))
            {
                SetStage(pair, TokenStageDefinitions.Stages.TrackingEndedTokenAge);
                return;
            }

            if (currentStageName == TokenStageDefinitions.Stages.Death || currentStageName == TokenStageDefinitions.Stages.TrackingEndedTokenAge)
            {
                return;
            }

            if (currentStageName == TokenStageDefinitions.Stages.Inception)
            {
                SetStage(pair, TokenStageDefinitions.Stages.PriceTracking);
            }
            else if (currentStageName == TokenStageDefinitions.Stages.PriceTracking)
            {
                SetStage(pair, TokenStageDefinitions.Stages.EngagementPostTracking);
            }
            else if (currentStageName == TokenStageDefinitions.Stages.EngagementPostTracking)
            {
                if (pair.TweetsCATweetCount > 0)
                {
                    SetStage(pair, TokenStageDefinitions.Stages.EngagementDetailTracking);
                }
            }
            else if (currentStageName == TokenStageDefinitions.Stages.EngagementDetailTracking)
            {
                // No transition out of this stage
            }
        }

        private void SetStage(RaydiumPair pair, string newStageName)
        {
            if (pair.DiscoveryStageName == newStageName)
                return; // No change

            pair.DiscoveryStageName = newStageName;
            pair.DiscoveryStageLastUpdated = DateTimeOffset.UtcNow;

            Logger.Debug($"Pair {pair.Id} moved to stage: {newStageName}");
        }

        #endregion

        #region Price Update / Aggregation

        private async Task UpdatePriceForPair(RaydiumPair pair, TokenStageDefinition stageDefinition)
        {
            if (string.IsNullOrWhiteSpace(pair.BaseTokenAccount))
                return;

            CoingeckoPoolsResponse coingeckoPoolsResponse;
            try
            {
                coingeckoPoolsResponse = await _coingeckoService.GetPoolsAsync(pair.BaseTokenAccount);
            }
            catch (Exception ex)
            {
                Logger.Error($"Error getting coingecko pools for {pair.BaseTokenAccount}", ex);
                return;
            }

            if (coingeckoPoolsResponse?.Data == null || !coingeckoPoolsResponse.Data.Any())
                return;

            // 1) Convert JSON -> List<CoingeckoPoolUpdate>
            var poolUpdates = new List<CoingeckoPoolUpdate>();
            var priceGroupId = Guid.NewGuid(); // optional grouping
            var priceGroupTime = DateTimeOffset.UtcNow;


            foreach (var poolData in coingeckoPoolsResponse.Data)
            {
                if (poolData.Attributes == null)
                    continue;

                var entity = ToCoingeckoPoolEntity(poolData, pair.Id);
                if (entity == null)
                    continue;

                // if(pair.ContractAddress == null)
                // {
                //     pair.ContractAddress = poolData.Attributes.Address;
                // }

                entity.CoingeckoAggregatedUpdateId = priceGroupId;
                entity.CreationTime = priceGroupTime;
                poolUpdates.Add(entity);
            }

            // 2) Bulk insert them
            if (poolUpdates.Any())
            {
                await _coingeckoPoolUpdateBulkRepository.BulkInsertAsync(poolUpdates);
            }

            // 3) Aggregate them into a single CoingeckoAggregatedUpdate
            var aggregated = BuildAggregatedUpdate(poolUpdates);
            aggregated.Id = priceGroupId;
            aggregated.RaydiumPairId = pair.Id;
            aggregated.CreationTime = priceGroupTime;

            // 4) Save the aggregated update in DB
            await _coingeckoAggregatedUpdateBulkRepository.BulkInsertAsync(new List<CoingeckoAggregatedUpdate> { aggregated });

            // 5) Link the aggregated update to the pair
            pair.CoingeckoLastAggregatedUpdateId = aggregated.Id;

            // 6) Calculate extended metrics from the aggregator
            CalculateExtendedMetricsFromAggregate(pair, aggregated);

            // 7) Mark last price refresh time
            pair.PriceRefreshLastUpdateTime = DateTimeOffset.UtcNow;

            if (pair.PriceRefreshNextUpdateTime.HasValue && pair.PriceRefreshNextUpdateTime.Value > DateTimeOffset.UtcNow)
            {
                return; // Not time yet
            }
            else if (pair.TotalLiquidityUsd >= stageDefinition.MinLiquidity01MinuteRefresh)
            {
                pair.PriceRefreshNextUpdateTime = DateTimeOffset.UtcNow.AddMinutes(1);
            }
            else if (pair.TotalLiquidityUsd >= stageDefinition.MinLiquidity05MinuteRefresh)
            {
                pair.PriceRefreshNextUpdateTime = DateTimeOffset.UtcNow.AddMinutes(5);
            }
            else if (pair.TotalLiquidityUsd >= stageDefinition.MinLiquidity15MinuteRefresh)
            {
                pair.PriceRefreshNextUpdateTime = DateTimeOffset.UtcNow.AddMinutes(15);
            }
            else if (pair.TotalLiquidityUsd >= stageDefinition.MinLiquidity30MinuteRefresh)
            {
                pair.PriceRefreshNextUpdateTime = DateTimeOffset.UtcNow.AddMinutes(30);
            }
            else if (pair.TotalLiquidityUsd >= stageDefinition.MinLiquidity1HourRefresh)
            {
                pair.PriceRefreshNextUpdateTime = DateTimeOffset.UtcNow.AddHours(1);
            }
            else if (pair.TotalLiquidityUsd >= stageDefinition.MinLiquidity6HourRefresh)
            {
                pair.PriceRefreshNextUpdateTime = DateTimeOffset.UtcNow.AddHours(6);
            }
            else if (pair.TotalLiquidityUsd >= stageDefinition.MinLiquidity24HourRefresh)
            {
                pair.PriceRefreshNextUpdateTime = DateTimeOffset.UtcNow.AddHours(24);
            }
            else
            {
                pair.PriceRefreshNextUpdateTime = DateTimeOffset.UtcNow.AddHours(24);
            }
        }

        /// <summary>
        /// Summarizes a list of CoingeckoPoolUpdate rows (for the same token)
        /// into a single CoingeckoAggregatedUpdate entity.
        /// </summary>
        private CoingeckoAggregatedUpdate BuildAggregatedUpdate(IEnumerable<CoingeckoPoolUpdate> updates)
        {
            var agg = new CoingeckoAggregatedUpdate();
            int poolCount = 0;

            // Weighted average price
            double totalVolForWeighting = 0.0;
            double priceVolumeAccumulator = 0.0;

            bool fdvSet = false;
            bool mcapSet = false;

            foreach (var u in updates)
            {
                poolCount++;

                // 1) Liquidity
                if (u.ReserveInUsd.HasValue)
                {
                    agg.TotalLiquidityUsd += u.ReserveInUsd.Value;
                }

                // 2) FDV / MarketCap (take max as an example)
                if (u.FdvUsd.HasValue)
                {
                    if (!fdvSet || u.FdvUsd.Value > agg.FdvUsd)
                    {
                        agg.FdvUsd = u.FdvUsd.Value;
                        fdvSet = true;
                    }
                }
                if (u.MarketCapUsd.HasValue)
                {
                    if (!mcapSet || u.MarketCapUsd.Value > agg.MarketCapUsd)
                    {
                        agg.MarketCapUsd = u.MarketCapUsd.Value;
                        mcapSet = true;
                    }
                }

                // 3) Price changes – sum them for now, average later
                agg.PriceChangeM5 += (u.PriceChangeM5 ?? 0f);
                agg.PriceChangeH1 += (u.PriceChangeH1 ?? 0f);
                agg.PriceChangeH6 += (u.PriceChangeH6 ?? 0f);
                agg.PriceChangeH24 += (u.PriceChangeH24 ?? 0f);

                // 4) Volumes
                agg.VolumeM5 += (u.VolumeM5 ?? 0f);
                agg.VolumeH1 += (u.VolumeH1 ?? 0f);
                agg.VolumeH6 += (u.VolumeH6 ?? 0f);
                agg.VolumeH24 += (u.VolumeH24 ?? 0f);

                // 5) Transactions
                agg.M5Buys += (u.M5Buys ?? 0);
                agg.M5Sells += (u.M5Sells ?? 0);
                agg.M5Buyers += (u.M5Buyers ?? 0);
                agg.M5Sellers += (u.M5Sellers ?? 0);

                agg.M15Buys += (u.M15Buys ?? 0);
                agg.M15Sells += (u.M15Sells ?? 0);
                agg.M15Buyers += (u.M15Buyers ?? 0);
                agg.M15Sellers += (u.M15Sellers ?? 0);

                agg.M30Buys += (u.M30Buys ?? 0);
                agg.M30Sells += (u.M30Sells ?? 0);
                agg.M30Buyers += (u.M30Buyers ?? 0);
                agg.M30Sellers += (u.M30Sellers ?? 0);

                agg.H1Buys += (u.H1Buys ?? 0);
                agg.H1Sells += (u.H1Sells ?? 0);
                agg.H1Buyers += (u.H1Buyers ?? 0);
                agg.H1Sellers += (u.H1Sellers ?? 0);

                agg.H24Buys += (u.H24Buys ?? 0);
                agg.H24Sells += (u.H24Sells ?? 0);
                agg.H24Buyers += (u.H24Buyers ?? 0);
                agg.H24Sellers += (u.H24Sellers ?? 0);

                // 6) Weighted average price (we'll use H1 volume for weighting)
                if (u.BaseTokenPriceUsd != null && u.VolumeH1.HasValue)
                {
                    if (float.TryParse(u.BaseTokenPriceUsd, out var priceVal))
                    {
                        var vol = u.VolumeH1.Value;
                        priceVolumeAccumulator += (priceVal * vol);
                        totalVolForWeighting += vol;
                    }
                }
            }

            agg.Pools = poolCount;

            // Average price changes
            if (poolCount > 0)
            {
                agg.PriceChangeM5 /= poolCount;
                agg.PriceChangeH1 /= poolCount;
                agg.PriceChangeH6 /= poolCount;
                agg.PriceChangeH24 /= poolCount;
            }

            // Weighted average price
            if (totalVolForWeighting > 0 && priceVolumeAccumulator > 0)
            {
                agg.WeightedAvgPriceUsd = (float)(priceVolumeAccumulator / totalVolForWeighting);
            }

            return agg;
        }

        #endregion

        #region Extended Metrics from Aggregated

        /// <summary>
        /// This method calculates liquidity, VLR, buy/sell ratio, vol-to-FDV, etc.
        /// using the newly created CoingeckoAggregatedUpdate. It then populates
        /// the RaydiumPair fields with final qualitative analysis.
        /// </summary>
        private void CalculateExtendedMetricsFromAggregate(RaydiumPair pair, CoingeckoAggregatedUpdate agg)
        {
            // 1) Basic fields
            pair.TotalLiquidityUsd = agg.TotalLiquidityUsd;

            // 2) Age of the token
            var pairAgeHours = (DateTimeOffset.UtcNow - pair.CreationTime).TotalHours;

            // 3) Volume / FDV / Price
            float volumeH1 = agg.VolumeH1;
            float fdv = agg.FdvUsd;
            float liquidity = agg.TotalLiquidityUsd;

            // 4) Compute buys vs sells for H1
            //    (Or use H24 if you prefer; up to you)
            int totalBuysH1 = agg.H1Buys;
            int totalSellsH1 = agg.H1Sells;

            // 5) VLR = volumeH1 / liquidity
            float? vlr = null;
            if (liquidity > 0 && volumeH1 > 0)
            {
                vlr = volumeH1 / liquidity;
            }

            if (vlr.HasValue)
            {
                var (analysis, recommendation) = GetVlrAnalysisAndRecommendation(vlr.Value, pairAgeHours);
                pair.VlrValue = vlr.Value;
                pair.VlrQualitativeAnalysis = analysis;
                pair.VlrRecommendation = recommendation;
            }
            else
            {
                pair.VlrValue = null;
                pair.VlrQualitativeAnalysis = "No volume or liquidity data.";
                pair.VlrRecommendation = "No recommendation.";
            }

            // 6) Buy/Sell ratio
            float? bSRatio = null;
            if (totalSellsH1 > 0)
            {
                bSRatio = (float)totalBuysH1 / totalSellsH1;
            }
            else if (totalBuysH1 > 0)
            {
                // If sells = 0 but buys > 0, treat ratio as "infinite" (or large).
                bSRatio = totalBuysH1;
            }

            if (bSRatio.HasValue)
            {
                var (analysis, recommendation) = GetBuySellRatioAnalysisAndRecommendation(bSRatio.Value);
                pair.BuySellRatioValue = bSRatio.Value;
                pair.BuySellRatioQualitativeAnalysis = analysis;
                pair.BuySellRatioRecommendation = recommendation;
            }
            else
            {
                pair.BuySellRatioValue = null;
                pair.BuySellRatioQualitativeAnalysis = "Not enough buy/sell data.";
                pair.BuySellRatioRecommendation = "No recommendation.";
            }

            // 7) Volume-to-FDV ratio (using 1h volume)
            float? volToFdv = null;
            if (fdv > 0 && volumeH1 > 0)
            {
                volToFdv = volumeH1 / fdv;
            }

            if (volToFdv.HasValue)
            {
                var (analysis, recommendation) = GetVolumeToFdvAnalysisAndRecommendation(volToFdv.Value);
                pair.VolumeToFdv1HValue = volToFdv.Value;
                pair.VolumeToFdv1HQualitativeAnalysis = analysis;
                pair.VolumeToFdv1HRecommendation = recommendation;
            }
            else
            {
                pair.VolumeToFdv1HValue = null;
                pair.VolumeToFdv1HQualitativeAnalysis = "No FDV or volume data.";
                pair.VolumeToFdv1HRecommendation = "No recommendation.";
            }

            // 8) Combine into a final composite score
            float vlrScore = GetVlrScore(vlr ?? 0);
            float bsScore = GetBuySellRatioScore(bSRatio ?? 0);
            float volFdvScore = GetVolumeToFdvScore(volToFdv ?? 0);

            if (pairAgeHours < 1)
            {
                // brand-new token penalty or caution
                vlrScore *= 0.8f;
                bsScore *= 0.8f;
                volFdvScore *= 0.8f;
            }

            float finalScore = (vlrScore + bsScore + volFdvScore) / 3.0f;


            if (agg.PriceChangeM5 > 0)
            {
                finalScore *= 1.4f;
            }
            if (agg.PriceChangeH1 > 0 && pair.CreationTime.AddHours(1) < DateTimeOffset.UtcNow)
            {
                finalScore *= 1.3f;
            }
            if (agg.PriceChangeH6 > 0 && pair.CreationTime.AddHours(6) < DateTimeOffset.UtcNow)
            {
                finalScore *= 1.2f;
            }
            if (agg.PriceChangeH24 > 0 && pair.CreationTime.AddHours(24) < DateTimeOffset.UtcNow)
            {
                finalScore *= 1.1f;
            }

            if (agg.PriceChangeM5 < 0)
            {
                finalScore *= 0.2f;
            }
            if (agg.PriceChangeH1 < 0 && pair.CreationTime.AddHours(1) < DateTimeOffset.UtcNow)
            {
                finalScore *= 0.3f;
            }
            if (agg.PriceChangeH6 < 0 && pair.CreationTime.AddHours(6) < DateTimeOffset.UtcNow)
            {
                finalScore *= 0.4f;
            }
            if (agg.PriceChangeH24 < 0 && pair.CreationTime.AddHours(24) < DateTimeOffset.UtcNow)
            {
                finalScore *= 0.5f;
            }


            pair.CombinedMetricScore = finalScore;

            var finalQa = "Overall Balanced";
            var finalRec = "Proceed with normal caution";
            if (finalScore > 8) { finalQa = "Very Strong Indicators"; finalRec = "High potential token"; }
            else if (finalScore > 5) { finalQa = "Moderate Indicators"; finalRec = "Possibly decent short-term gains"; }
            else if (finalScore > 3) { finalQa = "Weak Indicators"; finalRec = "Use caution, limited upside"; }
            else { finalQa = "Very Weak Indicators"; finalRec = "Likely avoid, high risk"; }

            if (pairAgeHours < 1)
            {
                finalQa += " (Token <1h old)";
                finalRec += " - brand new token, data uncertain";
            }

            pair.CombinedQualitativeAnalysis = finalQa;
            pair.CombinedRecommendation = finalRec;

            // 9) We can incorporate the engagement correlation logic:
            AnalyzeEngagementCorrelationAndSetSummary(pair);
        }

        #endregion

        #region Tweet Logic

        private async Task UpdateTweetCountsForPair(RaydiumPair pair, TokenStageDefinition tokenStage)
        {
            if (string.IsNullOrWhiteSpace(pair.BaseTokenAccount))
                return;

            if (pair.TwitterRefreshNextUpdateTime.HasValue && pair.TwitterRefreshNextUpdateTime.Value > DateTimeOffset.UtcNow)
            {
                return; // Skip if not time to refresh yet
            }

            if (pair.TotalLiquidityUsd >= tokenStage.MinLiquidity01MinuteRefresh)
            {
                pair.TwitterRefreshNextUpdateTime = DateTimeOffset.UtcNow.AddMinutes(1);
            }
            else if (pair.TotalLiquidityUsd >= tokenStage.MinLiquidity05MinuteRefresh)
            {
                pair.TwitterRefreshNextUpdateTime = DateTimeOffset.UtcNow.AddMinutes(5);
            }
            else if (pair.TotalLiquidityUsd >= tokenStage.MinLiquidity15MinuteRefresh)
            {
                pair.TwitterRefreshNextUpdateTime = DateTimeOffset.UtcNow.AddMinutes(15);
            }
            else if (pair.TotalLiquidityUsd >= tokenStage.MinLiquidity30MinuteRefresh)
            {
                pair.TwitterRefreshNextUpdateTime = DateTimeOffset.UtcNow.AddMinutes(30);
            }
            else if (pair.TotalLiquidityUsd >= tokenStage.MinLiquidity1HourRefresh)
            {
                pair.TwitterRefreshNextUpdateTime = DateTimeOffset.UtcNow.AddMinutes(60);
            }
            else if (pair.TotalLiquidityUsd >= tokenStage.MinLiquidity6HourRefresh)
            {
                pair.TwitterRefreshNextUpdateTime = DateTimeOffset.UtcNow.AddMinutes(360);
            }
            else if (pair.TotalLiquidityUsd >= tokenStage.MinLiquidity24HourRefresh)
            {
                pair.TwitterRefreshNextUpdateTime = DateTimeOffset.UtcNow.AddMinutes(1440);
            }
            else
            {
                pair.TwitterRefreshNextUpdateTime = DateTimeOffset.UtcNow.AddMinutes(1440);
            }


            var tweetCounts = new List<TwitterImportTweetCount>();
            var countsResponse = await _twitterAPICommunicationService.GetTweetCountsAsync(pair.BaseTokenAccount);
            pair.TwitterCAQueryCount++;

            if (countsResponse?.Data == null || !countsResponse.Data.Any())
            {
                return;
            }

            foreach (var bucket in countsResponse.Data)
            {
                var entity = new TwitterImportTweetCount
                {
                    Id = Guid.NewGuid(),
                    RaydiumPairId = pair.Id,
                    SearchQuery = pair.BaseTokenAccount,
                    StartTime = bucket.Start,
                    EndTime = bucket.End,
                    TweetCount = bucket.Tweet_Count,
                    CreationTime = DateTimeOffset.UtcNow
                };
                tweetCounts.Add(entity);

                if (bucket.Tweet_Count > 0)
                {
                    if (!pair.TwitterCAFirstMentionTime.HasValue ||
                        bucket.End < pair.TwitterCAFirstMentionTime.Value)
                    {
                        pair.TwitterCAFirstMentionTime = bucket.End;
                    }
                }
            }

            if (tweetCounts.Any())
            {
                if (countsResponse.Meta.Total_Tweet_Count == 0) return;

                await _twitterImportTweetCountBulkRepository.BulkInsertOrUpdateIncludeByAsync(
                    entities: tweetCounts,
                    propertiesToIncludeOnUpdate: new List<string> {
                        nameof(TwitterImportTweetCount.TweetCount),
                    },
                    UpdateBy: new List<string> {
                        nameof(TwitterImportTweetCount.RaydiumPairId),
                        nameof(TwitterImportTweetCount.SearchQuery),
                        nameof(TwitterImportTweetCount.StartTime),
                        nameof(TwitterImportTweetCount.EndTime)
                    }
                );

                // Basic rollups
                var tweets1H = tweetCounts.TakeLast(1)?.FirstOrDefault()?.TweetCount ?? 0;
                var tweets3H = (tweetCounts.TakeLast(3)?.Sum(x => x.TweetCount) ?? 0) - tweets1H;
                var tweets6H = (tweetCounts.TakeLast(6)?.Sum(x => x.TweetCount) ?? 0) - (tweets3H + tweets1H); ;
                var tweets12H = (tweetCounts.TakeLast(12)?.Sum(x => x.TweetCount) ?? 0) - (tweets6H + tweets3H + tweets1H);
                var tweets24H = (tweetCounts.TakeLast(24)?.Sum(x => x.TweetCount) ?? 0) - (tweets12H + tweets6H + tweets3H + tweets1H);
                var tweets1WK = tweetCounts.Sum(x => x.TweetCount) - (tweets24H + tweets12H + tweets6H + tweets3H + tweets1H);

                pair.TweetsCATweetCount1H = tweets1H;
                pair.TweetsCATweetCount3H = tweets3H;
                pair.TweetsCATweetCount6H = tweets6H;
                pair.TweetsCATweetCount12H = tweets12H;
                pair.TweetsCATweetCount24H = tweets24H;
                pair.TweetsCATweetCount1WK = tweets1WK;

                if (countsResponse.Meta?.Total_Tweet_Count != null)
                {
                    pair.TweetsCATweetCount = countsResponse.Meta.Total_Tweet_Count;
                }

                if (pair.TweetsCATweetCount > 0 && !pair.TwitterCAFound)
                {
                    pair.TwitterCAFound = true;
                    pair.TwitterCAFoundAtTime = DateTimeOffset.UtcNow;
                }
            }
        }

        private async Task UpdateTweetEngagementForPair(RaydiumPair pair, TokenStageDefinition tokenStage)
        {
            if (string.IsNullOrWhiteSpace(pair.BaseTokenAccount))
                return;

            if ((pair.TweetsCATweetCount1H + pair.TweetsCATweetCount3H) < tokenStage.MinTweetsLast03HourRefresh)
            {
                return;
            }

            var tweets = await _twitterAPICommunicationService.GetTweetsFullEngagementAsync(
                searchQuery: pair.BaseTokenAccount,
                limit: 100
            );

            pair.TwitterCAQueryCount++;
            var upsertList = new List<TwitterImportTweetEngagement>();

            foreach (var t in tweets)
            {
                var entity = new TwitterImportTweetEngagement
                {
                    Id = Guid.NewGuid(),
                    RaydiumPairId = pair.Id,
                    TweetId = t.Id,
                    AuthorId = t.AuthorId,
                    Text = t.Text,
                    CreatedAt = t.CreatedAt,
                    LikeCount = t.PublicMetrics?.LikeCount ?? 0,
                    ReplyCount = t.PublicMetrics?.ReplyCount ?? 0,
                    RetweetCount = t.PublicMetrics?.RetweetCount ?? 0,
                    QuoteCount = t.PublicMetrics?.QuoteCount ?? 0,
                    LastUpdatedAt = DateTimeOffset.UtcNow
                };
                upsertList.Add(entity);

                // If this tweet is older than our stored "first mention," update that
                if (!pair.TwitterCAFirstMentionTime.HasValue ||
                    t.CreatedAt < pair.TwitterCAFirstMentionTime.Value)
                {
                    pair.TwitterCAFirstMentionTime = t.CreatedAt;
                    pair.TwitterCAFirstMentionTweetId = t.Id;
                    pair.TwitterCAFirstMentionText = t.Text;
                    pair.TwitterCAFirstMentionHandle = t.AuthorId;
                }

                if (entity.LikeCount > pair.TwitterCAMostLikesOnSingleTweet)
                {
                    pair.TwitterCAMostLikesOnSingleTweet = entity.LikeCount;
                }
                if (entity.ReplyCount > pair.TwitterCAMostRepliesOnSingleTweet)
                {
                    pair.TwitterCAMostRepliesOnSingleTweet = entity.ReplyCount;
                }
            }

            if (upsertList.Any())
            {
                await _twitterImportTweetEngagementBulkRepository.BulkInsertOrUpdateIncludeByAsync(
                    entities: upsertList,
                    propertiesToIncludeOnUpdate: new List<string> {
                        nameof(TwitterImportTweetEngagement.AuthorId),
                        nameof(TwitterImportTweetEngagement.Text),
                        nameof(TwitterImportTweetEngagement.CreatedAt),
                        nameof(TwitterImportTweetEngagement.LikeCount),
                        nameof(TwitterImportTweetEngagement.ReplyCount),
                        nameof(TwitterImportTweetEngagement.RetweetCount),
                        nameof(TwitterImportTweetEngagement.QuoteCount),
                        nameof(TwitterImportTweetEngagement.LastUpdatedAt)
                    },
                    UpdateBy: new List<string> {
                        nameof(TwitterImportTweetEngagement.RaydiumPairId),
                        nameof(TwitterImportTweetEngagement.TweetId)
                    }
                );
            }

            // Pull aggregated stats from DB
            var aggregated = await _twitterImportTweetEngagementRepository.GetAll()
                .Where(x => x.RaydiumPairId == pair.Id)
                .GroupBy(x => x.RaydiumPairId)
                .Select(g => new
                {
                    UniqueAuthors = g.Select(x => x.AuthorId).Distinct().Count(),
                    TweetsImported = g.Count(),
                    SumLikes = g.Sum(x => x.LikeCount),
                    SumReplies = g.Sum(x => x.ReplyCount),
                    SumRetweets = g.Sum(x => x.RetweetCount),
                    SumQuotes = g.Sum(x => x.QuoteCount)
                })
                .FirstOrDefaultAsync();

            if (aggregated != null)
            {
                pair.TweetsCAEngagementUniqueAuthors = aggregated.UniqueAuthors;
                pair.TweetsCAEngagementTweetsImported = aggregated.TweetsImported;
                pair.TweetsCAEngagementTotalLikes = aggregated.SumLikes;
                pair.TweetsCAEngagementTotalReplies = aggregated.SumReplies;
                pair.TweetsCAEngagementTotalRetweets = aggregated.SumRetweets;
                pair.TweetsCAEngagementTotalQuotes = aggregated.SumQuotes;
            }

            if (pair.TweetsCATweetCount > 0 && !pair.TwitterCAFound)
            {
                pair.TwitterCAFound = true;
                pair.TwitterCAFoundAtTime = DateTimeOffset.UtcNow;
            }

            // Re-run correlation analysis if needed
            AnalyzeEngagementCorrelationAndSetSummary(pair);
        }

        #endregion

        #region Utility / Analysis Helpers

        public static CoingeckoPoolUpdate ToCoingeckoPoolEntity(CoingeckoPoolData data, Guid? raydiumPairId)
        {
            if (data == null) return null;
            var attr = data.Attributes;
            if (attr == null) return null;

            // Map JSON to DB fields
            var entity = new CoingeckoPoolUpdate
            {
                Id = Guid.NewGuid(),
                RaydiumPairId = raydiumPairId,
                PoolId = data.Id,
                PoolType = data.Type,

                BaseTokenPriceUsd = attr.BaseTokenPriceUsd,
                BaseTokenPriceNativeCurrency = attr.BaseTokenPriceNativeCurrency,
                QuoteTokenPriceUsd = attr.QuoteTokenPriceUsd,
                QuoteTokenPriceNativeCurrency = attr.QuoteTokenPriceNativeCurrency,
                BaseTokenPriceQuoteToken = attr.BaseTokenPriceQuoteToken,
                QuoteTokenPriceBaseToken = attr.QuoteTokenPriceBaseToken,

                Address = attr.Address,
                Name = attr.Name,
                PoolCreatedAt = attr.PoolCreatedAt,

                TokenPriceUsd = ToNullableFloat(attr.TokenPriceUsd),
                FdvUsd = ToNullableFloat(attr.FdvUsd),
                MarketCapUsd = ToNullableFloat(attr.MarketCapUsd),

                PriceChangeM5 = ToNullableFloat(attr.PriceChangePercentage?.M5),
                PriceChangeH1 = ToNullableFloat(attr.PriceChangePercentage?.H1),
                PriceChangeH6 = ToNullableFloat(attr.PriceChangePercentage?.H6),
                PriceChangeH24 = ToNullableFloat(attr.PriceChangePercentage?.H24),

                M5Buys = attr.Transactions?.M5?.Buys,
                M5Sells = attr.Transactions?.M5?.Sells,
                M5Buyers = attr.Transactions?.M5?.Buyers,
                M5Sellers = attr.Transactions?.M5?.Sellers,

                M15Buys = attr.Transactions?.M15?.Buys,
                M15Sells = attr.Transactions?.M15?.Sells,
                M15Buyers = attr.Transactions?.M15?.Buyers,
                M15Sellers = attr.Transactions?.M15?.Sellers,

                M30Buys = attr.Transactions?.M30?.Buys,
                M30Sells = attr.Transactions?.M30?.Sells,
                M30Buyers = attr.Transactions?.M30?.Buyers,
                M30Sellers = attr.Transactions?.M30?.Sellers,

                H1Buys = attr.Transactions?.H1?.Buys,
                H1Sells = attr.Transactions?.H1?.Sells,
                H1Buyers = attr.Transactions?.H1?.Buyers,
                H1Sellers = attr.Transactions?.H1?.Sellers,

                H24Buys = attr.Transactions?.H24?.Buys,
                H24Sells = attr.Transactions?.H24?.Sells,
                H24Buyers = attr.Transactions?.H24?.Buyers,
                H24Sellers = attr.Transactions?.H24?.Sellers,

                VolumeM5 = ToNullableFloat(attr.VolumeUsd?.M5),
                VolumeH1 = ToNullableFloat(attr.VolumeUsd?.H1),
                VolumeH6 = ToNullableFloat(attr.VolumeUsd?.H6),
                VolumeH24 = ToNullableFloat(attr.VolumeUsd?.H24),

                ReserveInUsd = ToNullableFloat(attr.ReserveInUsd),

                BaseTokenId = data.Relationships?.BaseToken?.Data?.Id,
                QuoteTokenId = data.Relationships?.QuoteToken?.Data?.Id,
                DexId = data.Relationships?.Dex?.Data?.Id
            };

            return entity;
        }

        private static float? ToNullableFloat(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return null;
            if (float.TryParse(value, out var f)) return f;
            return null;
        }

        private (string Analysis, string Recommendation) GetVlrAnalysisAndRecommendation(float vlr, double ageHours)
        {
            // Some rough examples
            if (vlr < 1f)
                return ("VLR < 1, low trading interest", "Likely low momentum; watch for volume picks");
            if (vlr < 2f)
                return ("VLR 1-2, moderate activity", "Potentially stable zone for accumulation");
            if (vlr < 5f)
                return ("VLR 2-5, healthy liquidity ratio", "Shows good trading interest; decent risk/reward");
            if (vlr < 10f)
                return ("VLR 5-10, somewhat speculative", "Expect higher volatility; be cautious");
            return ("VLR > 10, extremely high volume vs. liquidity", "Very volatile; high risk/reward scenario");
        }

        private float GetVlrScore(float vlr)
        {
            // Example scoring
            if (vlr < 1) return 3f;
            if (vlr < 2) return 6f;
            if (vlr < 5) return 8f;
            if (vlr < 10) return 5f;
            return 2f;
        }

        private (string Analysis, string Recommendation) GetBuySellRatioAnalysisAndRecommendation(float ratio)
        {
            if (ratio < 1) return ("More sells than buys", "Bearish sentiment, cautious entry");
            if (ratio < 1.5f) return ("Near 1:1, slightly bullish", "Could be stable or slightly bullish");
            if (ratio < 3f) return ("Solid buying pressure", "Uptrend potential if volume persists");
            if (ratio < 5f) return ("Strong buying dominance", "Expect upward movement but watch for profit-taking");
            if (ratio < 10f) return ("Very high buy ratio", "Likely short-term euphoria; higher risk");
            return ("Extreme buy euphoria", "Very high risk of sudden reversal");
        }

        private float GetBuySellRatioScore(float ratio)
        {
            if (ratio < 1) return 3f;
            if (ratio < 1.5f) return 5f;
            if (ratio < 3f) return 7f;
            if (ratio < 5f) return 8f;
            if (ratio < 10f) return 6f;
            return 2f;
        }

        private (string Analysis, string Recommendation) GetVolumeToFdvAnalysisAndRecommendation(float vFdv)
        {
            if (vFdv < 0.001f)
                return ("Very low vol-to-FDV (<0.1%)", "Token might be overvalued or idle");
            if (vFdv < 0.005f)
                return ("Weak vol-to-FDV", "Might be slow growth; watch for pick-up");
            if (vFdv < 0.01f)
                return ("Moderate vol-to-FDV", "Potential momentum building");
            if (vFdv < 0.02f)
                return ("Good vol-to-FDV", "Healthy trading interest for FDV");
            return ("High vol-to-FDV", "Hype or mania; big swings possible");
        }

        private float GetVolumeToFdvScore(float vFdv)
        {
            if (vFdv < 0.001f) return 2f;
            if (vFdv < 0.005f) return 4f;
            if (vFdv < 0.01f) return 6f;
            if (vFdv < 0.02f) return 8f;
            return 10f;
        }

        /// <summary>
        /// Correlate engagement (tweets) with volume-based metrics for a final summary.
        /// </summary>
        private void AnalyzeEngagementCorrelationAndSetSummary(RaydiumPair pair)
        {
            if (pair == null) return;

            float vlr = pair.VlrValue ?? 0f;
            float bSRatio = pair.BuySellRatioValue ?? 0f;
            float vFdv = pair.VolumeToFdv1HValue ?? 0f;

            int tweetsLastHour = pair.TweetsCATweetCount1H;
            bool someEngagement = pair.TwitterCAFound || (tweetsLastHour > 0);

            float correlationScore = pair.CombinedMetricScore ?? 5f;

            if (tweetsLastHour > 20 && vlr > 5)
            {
                correlationScore += 3f;
            }
            if (tweetsLastHour > 10 && bSRatio > 2f)
            {
                correlationScore += 2f;
            }
            if (tweetsLastHour > 5 && vFdv > 0.01f)
            {
                correlationScore += 2f;
            }

            string correlationAnalysis;
            if (!someEngagement)
            {
                correlationAnalysis = "No noticeable Twitter engagement. Limited synergy with trading metrics.";
            }
            else
            {
                correlationAnalysis = "Meaningful Twitter engagement observed.";
                if (correlationScore > 12f)
                    correlationAnalysis += " Strong synergy suggests short-term hype or pump potential.";
                else if (correlationScore > 8f)
                    correlationAnalysis += " Moderate synergy suggests stable interest.";
                else
                    correlationAnalysis += " Low synergy means hype not fully translating into buys.";
            }

            var finalSummary = "🌵 " + (pair.CombinedQualitativeAnalysis ?? "N/A")
                + " | Engagement correlation: " + correlationAnalysis;

            var finalPrediction = "Short-term outlook: ";
            if (correlationScore > 15)
                finalPrediction += "Potential rapid volatility, watch closely.";
            else if (correlationScore > 10)
                finalPrediction += "Likely mild uptrend if new buyers hold interest.";
            else
                finalPrediction += "Uncertain; could fade without further volume.";

            pair.EngagementCorrelationAnalysis = correlationAnalysis;
            pair.EngagementCorrelationScore = correlationScore;
            pair.PlantFinalSummary = finalSummary;
            pair.PlantFinalPrediction = finalPrediction;
        }

        #endregion
    }
}
