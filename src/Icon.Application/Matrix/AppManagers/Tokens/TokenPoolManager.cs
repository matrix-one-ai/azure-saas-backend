using System;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Domain.Uow;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Icon.Configuration;
using System.Collections.Generic;
using Icon.Matrix.Models;
using Icon.Matrix.Coingecko;
using System.Linq.Dynamic.Core;
using System.Linq;
using Icon.EntityFrameworkCore.Matrix;
using Icon.Matrix.Twitter;
using Icon.Matrix.TokenDiscovery;

namespace Icon.Matrix.TokenPools
{
    public interface ITokenPoolManager : IDomainService
    {
        Task<RaydiumPair> GetBestPerformingPair();
        Task<List<TwitterImportTweetEngagement>> GetTweetEngagements(Guid raydiumPairId);
        Task<List<TwitterImportTweetCount>> GetTweetCounts(Guid raydiumPairId);
        Task<List<RaydiumPair>> GetLatestRaydiumpairs();
        Task<CoingeckoPoolUpdate> GetLastestCoingeckoPoolUpdate(Guid raydiumPairId);
        Task<CoingeckoAggregatedUpdate> GetCoingeckoAggregatedUpdate(Guid id);
        Task<RaydiumPair> GetRaydiumPair(Guid id);
        Task SetTokenPoolTweeted(Guid poolId, string tweet);
    }
    public class TokenPoolManager : IconServiceBase, ITokenPoolManager
    {
        private readonly ICoingeckoService _coingeckoService;
        private readonly ITwitterAPICommunicationService _twitterAPICommunicationService;
        private readonly IRepository<CoingeckoPoolUpdate, Guid> _coingeckoPoolUpdateRepository;
        private readonly IRepository<CoingeckoAggregatedUpdate, Guid> _coingeckoAggregatedUpdateRepository;
        private readonly IRepository<TwitterImportTweetEngagement, Guid> _twitterImportTweetEngagementRepository;
        private readonly IRepository<TwitterImportTweetCount, Guid> _twitterImportTweetCountRepository;
        private readonly IRepository<RaydiumPair, Guid> _raydiumPairRepository;

        private readonly IMatrixBulkRepository<CoingeckoPoolUpdate> _coingeckoPoolUpdateBulkRepository;
        private readonly IMatrixBulkRepository<RaydiumPair> _raydiumPairBulkRepository;
        private readonly IMatrixBulkRepository<TwitterImportTweetEngagement> _twitterImportTweetEngagementBulkRepository;
        private readonly IMatrixBulkRepository<TwitterImportTweetCount> _twitterImportTweetCountBulkRepository;


        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private IConfigurationRoot _configuration;

        public TokenPoolManager(
            ICoingeckoService coingeckoService,
            ITwitterAPICommunicationService twitterAPICommunicationService,
            IRepository<CoingeckoPoolUpdate, Guid> coingeckoPoolUpdateRepository,
            IRepository<CoingeckoAggregatedUpdate, Guid> coingeckoAggregatedUpdateRepository,
            IRepository<TwitterImportTweetEngagement, Guid> twitterImportTweetEngagementRepository,
            IRepository<TwitterImportTweetCount, Guid> twitterImportTweetCountRepository,
            IRepository<RaydiumPair, Guid> raydiumPairRepository,

            IMatrixBulkRepository<CoingeckoPoolUpdate> coingeckoPoolUpdateBulkRepository,
            IMatrixBulkRepository<RaydiumPair> raydiumPairBulkRepository,
            IMatrixBulkRepository<TwitterImportTweetEngagement> twitterImportTweetEngagementBulkRepository,
            IMatrixBulkRepository<TwitterImportTweetCount> twitterImportTweetCountBulkRepository,

            IUnitOfWorkManager unitOfWorkManager,
            IAppConfigurationAccessor appConfigurationAccessor)
        {
            _coingeckoService = coingeckoService;
            _twitterAPICommunicationService = twitterAPICommunicationService;
            _coingeckoPoolUpdateRepository = coingeckoPoolUpdateRepository;
            _coingeckoAggregatedUpdateRepository = coingeckoAggregatedUpdateRepository;
            _twitterImportTweetEngagementRepository = twitterImportTweetEngagementRepository;
            _twitterImportTweetCountRepository = twitterImportTweetCountRepository;
            _raydiumPairRepository = raydiumPairRepository;

            _coingeckoPoolUpdateBulkRepository = coingeckoPoolUpdateBulkRepository;
            _raydiumPairBulkRepository = raydiumPairBulkRepository;
            _twitterImportTweetEngagementBulkRepository = twitterImportTweetEngagementBulkRepository;
            _twitterImportTweetCountBulkRepository = twitterImportTweetCountBulkRepository;

            _unitOfWorkManager = unitOfWorkManager;
            _configuration = appConfigurationAccessor.Configuration;
        }

        public async Task<RaydiumPair> GetBestPerformingPair()
        {
            var activeStages = TokenStageDefinitions.ActiveStages;

            var bestPair = await _raydiumPairRepository
                .GetAll()
                .Where(x => activeStages.Contains(x.DiscoveryStageName) && !x.TweetSent)
                .OrderByDescending(x => x.CombinedMetricScore)
                .FirstOrDefaultAsync();

            return bestPair;
        }

        public async Task<CoingeckoAggregatedUpdate> GetCoingeckoAggregatedUpdate(Guid id)
        {
            return await _coingeckoAggregatedUpdateRepository
                .GetAll()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<CoingeckoPoolUpdate> GetLastestCoingeckoPoolUpdate(Guid raydiumPairId)
        {
            return await _coingeckoPoolUpdateRepository
                .GetAll()
                .Where(x => x.RaydiumPairId == raydiumPairId)
                .OrderByDescending(x => x.CreationTime)
                .FirstOrDefaultAsync();
        }

        public async Task<List<RaydiumPair>> GetLatestRaydiumpairs()
        {
            return await _raydiumPairRepository
                .GetAll()
                .Where(x => x.TweetSent)
                .OrderByDescending(x => x.TweetSentAt)
                .Take(10)
                .ToListAsync();
        }

        public async Task<List<TwitterImportTweetEngagement>> GetTweetEngagements(Guid raydiumPairId)
        {
            return await _twitterImportTweetEngagementRepository
                .GetAll()
                .Where(x => x.RaydiumPairId == raydiumPairId)
                .ToListAsync();
        }

        public async Task<List<TwitterImportTweetCount>> GetTweetCounts(Guid raydiumPairId)
        {
            return await _twitterImportTweetCountRepository
                .GetAll()
                .Where(x => x.RaydiumPairId == raydiumPairId)
                .ToListAsync();
        }

        public async Task<RaydiumPair> GetRaydiumPair(Guid id)
        {
            return await _raydiumPairRepository.FirstOrDefaultAsync(id);
        }

        public async Task SetTokenPoolTweeted(Guid poolId, string tweet)
        {
            var pool = await _raydiumPairRepository.FirstOrDefaultAsync(poolId);
            if (pool == null)
            {
                return;
            }

            pool.TweetSent = true;
            pool.TweetText = tweet;
            pool.TweetSentAt = DateTimeOffset.Now;

            await _raydiumPairRepository.UpdateAsync(pool);
        }
    }
}

// public async Task<TokenPoolGetBestPerformingResponse> FindPerformingPools(TokenPoolGetBestPerformingInput input)
// {
//     if (input.PerformPoolUpdate)
//     {
//         await ImportRaydiumPoolUpdates();
//     }

//     var response = new TokenPoolGetBestPerformingResponse();

//     var pools = await _raydiumPairRepository
//         .GetAll()
//         .AsNoTracking()
//         .Where(x =>
//             x.CreationTime > input.CreatedAfter &&
//             x.CreationTime < input.CreatedBefore &&
//             x.TweetSent == !input.ExcludeTweetedPools)
//         .ToListAsync();

//     var poolIds = pools.Select(x => x.Id).ToList();

//     var poolLastUpdates = await _coingeckoPoolUpdateRepository
//         .GetAll()
//         .AsNoTracking()
//         .Where(x =>
//             x.RaydiumPairId != null && poolIds.Contains((Guid)x.RaydiumPairId) &&
//             x.CreationTime > DateTimeOffset.UtcNow.AddMinutes(-input.MaxPriceUpdateAgeMinutes))
//         .GroupBy(x => x.RaydiumPairId)
//         .Select(group => group.OrderByDescending(x => x.CreationTime).FirstOrDefault())
//         .ToListAsync();

//     var filteredPoolUpdates = poolLastUpdates
//             .WhereIf(input.MinFdvUsd != null, x => x.FdvUsd > input.MinFdvUsd)
//             .WhereIf(input.MinLiquidtyUsd != null, x => x.ReserveInUsd > input.MinLiquidtyUsd)
//             .WhereIf(input.MinVolumeH1 != null, x => x.VolumeH1 > input.MinVolumeH1)
//             .WhereIf(input.MinRisePercentageSinceCreation != null, x =>
//                 x.PriceChangeM5 > input.MinRisePercentageSinceCreation &&
//                 x.PriceChangeH1 > input.MinRisePercentageSinceCreation &&
//                 x.PriceChangeH6 > input.MinRisePercentageSinceCreation &&
//                 x.PriceChangeH24 > input.MinRisePercentageSinceCreation)
//             .Where(x => x.RaydiumPairId != null && poolIds.Contains((Guid)x.RaydiumPairId))
//             .ToList();

//     var bestPerformingPools = filteredPoolUpdates
//         .OrderByDescending(x => x.PriceChangeH24)
//         .Take(input.MaxPools)
//         .ToList();

//     foreach (var pool in bestPerformingPools)
//     {
//         var performingPool = new TokenPool
//         {
//             RaydiumPair = pools.FirstOrDefault(x => x.Id == pool.RaydiumPairId),
//             CoinGeckoLastUpdate = pool
//         };

//         response.PerformingPools.Add(performingPool);
//     }

//     return response;
// }

//     public async Task ImportRaydiumPoolUpdates()
//     {
//         var raydiumPairs = await _raydiumPairRepository
//             .GetAll()
//             .Where(x => x.PriceRefreshEnabled && x.PriceRefreshNextUpdateTime < DateTimeOffset.UtcNow)
//             .ToListAsync();

//         var poolUpdates = new List<CoingeckoPoolUpdate>();
//         foreach (var raydiumPair in raydiumPairs)
//         {
//             if (string.IsNullOrEmpty(raydiumPair.BaseTokenAccount))
//             {
//                 continue;
//             }

//             CoingeckoPoolsResponse coingeckoPoolsResponse = null;
//             try
//             {
//                 coingeckoPoolsResponse = await _coingeckoService.GetPoolsAsync(raydiumPair.BaseTokenAccount);
//             }
//             catch (Exception ex)
//             {
//                 Logger.Error("Error getting coingecko pools for " + raydiumPair.BaseTokenAccount, ex);
//                 continue;
//             }

//             if (coingeckoPoolsResponse == null || coingeckoPoolsResponse.Data == null)
//             {
//                 continue;
//             }

//             foreach (var coingeckoPoolUpdate in coingeckoPoolsResponse.Data)
//             {
//                 if (coingeckoPoolUpdate.Attributes == null)
//                 {
//                     continue;
//                 }

//                 var entity = CoingeckoPoolConverter.ToCoingeckoPoolEntity(coingeckoPoolUpdate, raydiumPair.Id);

//                 if (entity == null)
//                 {
//                     continue;
//                 }

//                 poolUpdates.Add(entity);

//                 var poolAge = DateTimeOffset.UtcNow - raydiumPair.CreationTime;
//                 var nextRefreshInterval = CalculateNextRefreshInterval(poolAge);
//                 raydiumPair.PriceRefreshNextUpdateTime = DateTimeOffset.UtcNow.Add(nextRefreshInterval);
//                 raydiumPair.LastPoolUpdate = DateTimeOffset.UtcNow;

//                 if (poolAge > TimeSpan.FromHours(1))
//                 {
//                     raydiumPair.PriceRefreshEnabled = false;
//                 }
//             }
//         }

//         var propertiesToIncludeOnUpdate = new List<string>
//             {
//                 "PriceRefreshNextUpdateTime",
//                 "LastPoolUpdate",
//                 "PriceRefreshEnabled"
//             };

//         await _raydiumPairBulkRepository.BulkInsertOrUpdateExcludeAsync(raydiumPairs, propertiesToIncludeOnUpdate);
//         await _coingeckoPoolUpdateBulkRepository.BulkInsertAsync(poolUpdates);
//     }

//     public async Task UpdateTwitterEngagementForPools(TokenPoolGetTwitterEngagementInput input)
//     {
//         if (input.Pools == null || !input.Pools.Any())
//         {
//             return;
//         }

//         foreach (var pool in input.Pools)
//         {

//             var pair = pool.RaydiumPair;

//             if (string.IsNullOrWhiteSpace(pair.BaseTokenAccount))
//                 continue;

//             if (input.PerformTwitterPostCountUpdate)
//             {
//                 var tweetCounts = new List<TwitterImportTweetCount>();
//                 var countsResponse = await _twitterAPICommunicationService.GetTweetCountsAsync(pair.BaseTokenAccount);
//                 pair.TwitterCAQueryCount++;

//                 if (countsResponse?.Data != null && countsResponse.Data.Any())
//                 {
//                     foreach (var bucket in countsResponse.Data)
//                     {
//                         var entity = new TwitterImportTweetCount
//                         {
//                             Id = Guid.NewGuid(),
//                             RaydiumPairId = pair.Id,
//                             SearchQuery = pair.BaseTokenAccount,
//                             StartTime = bucket.Start,
//                             EndTime = bucket.End,
//                             TweetCount = bucket.Tweet_Count,
//                             CreationTime = DateTimeOffset.UtcNow
//                         };
//                         tweetCounts.Add(entity);

//                         if (bucket.Tweet_Count > 0)
//                         {
//                             if (!pair.TwitterCAFirstMentionTime.HasValue ||
//                                  bucket.End < pair.TwitterCAFirstMentionTime.Value)
//                             {
//                                 pair.TwitterCAFirstMentionTime = bucket.End;
//                             }
//                         }
//                     }

//                     if (tweetCounts.Any())
//                     {
//                         await _twitterImportTweetCountBulkRepository.BulkInsertOrUpdateIncludeByAsync(
//                             entities: tweetCounts,
//                             propertiesToIncludeOnUpdate: new List<string> {
//                                 nameof(TwitterImportTweetCount.TweetCount),
//                             },
//                             UpdateBy: new List<string> {
//                                 nameof(TwitterImportTweetCount.RaydiumPairId),
//                                 nameof(TwitterImportTweetCount.SearchQuery),
//                                 nameof(TwitterImportTweetCount.StartTime),
//                                 nameof(TwitterImportTweetCount.EndTime)
//                             }
//                         );

//                         var UtcNow = DateTimeOffset.UtcNow;

//                         var tweets1H = tweetCounts.TakeLast(1)?.FirstOrDefault()?.TweetCount ?? 0;
//                         var tweets3H = tweetCounts.TakeLast(3)?.Sum(x => x.TweetCount) ?? 0;
//                         var tweets6H = tweetCounts.TakeLast(6)?.Sum(x => x.TweetCount) ?? 0;
//                         var tweets12H = tweetCounts.TakeLast(12)?.Sum(x => x.TweetCount) ?? 0;
//                         var tweets24H = tweetCounts.TakeLast(24)?.Sum(x => x.TweetCount) ?? 0;
//                         var tweets1WK = tweetCounts.Sum(x => x.TweetCount);

//                         pair.TweetsCATweetCount1H = tweets1H;
//                         pair.TweetsCATweetCount3H = tweets3H;
//                         pair.TweetsCATweetCount6H = tweets6H;
//                         pair.TweetsCATweetCount12H = tweets12H;
//                         pair.TweetsCATweetCount24H = tweets24H;
//                         pair.TweetsCATweetCount1WK = tweets1WK;

//                         pair.TweetsCATweetCount = countsResponse?.Meta?.Total_Tweet_Count ?? pair.TweetsCATweetCount;

//                         if (pair.TweetsCATweetCount > 0 && !pair.TwitterCAFound)
//                         {
//                             pair.TwitterCAFound = true;
//                             pair.TwitterCAFoundAtTime = DateTimeOffset.UtcNow;
//                         }
//                     }
//                 }
//             }

//             if (input.PerformTwitterEngagementUpdate)
//             {
//                 var tweets = await _twitterAPICommunicationService.GetTweetsFullEngagementAsync(pair.BaseTokenAccount, input.PerformTwitterEngagementUpdateMaxTweets);
//                 pair.TwitterCAQueryCount++;

//                 var upsertList = new List<TwitterImportTweetEngagement>();
//                 foreach (var t in tweets)
//                 {
//                     var entity = new TwitterImportTweetEngagement
//                     {
//                         Id = Guid.NewGuid(),
//                         RaydiumPairId = pair.Id,
//                         TweetId = t.Id,
//                         AuthorId = t.AuthorId,
//                         Text = t.Text,
//                         CreatedAt = t.CreatedAt,
//                         LikeCount = t.PublicMetrics?.LikeCount ?? 0,
//                         ReplyCount = t.PublicMetrics?.ReplyCount ?? 0,
//                         RetweetCount = t.PublicMetrics?.RetweetCount ?? 0,
//                         QuoteCount = t.PublicMetrics?.QuoteCount ?? 0,
//                         LastUpdatedAt = DateTimeOffset.UtcNow
//                     };
//                     upsertList.Add(entity);

//                     if (!pair.TwitterCAFirstMentionTime.HasValue ||
//                             t.CreatedAt < pair.TwitterCAFirstMentionTime.Value)
//                     {
//                         pair.TwitterCAFirstMentionTime = t.CreatedAt;
//                         pair.TwitterCAFirstMentionTweetId = t.Id;
//                         pair.TwitterCAFirstMentionText = t.Text;
//                         pair.TwitterCAFirstMentionHandle = t.AuthorId;
//                     }

//                     if (pair.TwitterCAMostLikesOnSingleTweet < entity.LikeCount)
//                     {
//                         pair.TwitterCAMostLikesOnSingleTweet = entity.LikeCount;
//                     }

//                     if (pair.TwitterCAMostRepliesOnSingleTweet < entity.ReplyCount)
//                     {
//                         pair.TwitterCAMostRepliesOnSingleTweet = entity.ReplyCount;
//                     }
//                 }

//                 await _twitterImportTweetEngagementBulkRepository.BulkInsertOrUpdateIncludeByAsync(
//                      entities: upsertList,
//                      propertiesToIncludeOnUpdate: new List<string> {
//                         nameof(TwitterImportTweetEngagement.AuthorId),
//                         nameof(TwitterImportTweetEngagement.Text),
//                         nameof(TwitterImportTweetEngagement.CreatedAt),
//                         nameof(TwitterImportTweetEngagement.LikeCount),
//                         nameof(TwitterImportTweetEngagement.ReplyCount),
//                         nameof(TwitterImportTweetEngagement.RetweetCount),
//                         nameof(TwitterImportTweetEngagement.QuoteCount),
//                         nameof(TwitterImportTweetEngagement.LastUpdatedAt)
//                      },
//                     UpdateBy: new List<string> {
//                         nameof(TwitterImportTweetEngagement.RaydiumPairId),
//                         nameof(TwitterImportTweetEngagement.TweetId)
//                     }
//                  );

//                 var aggregated = await _twitterImportTweetEngagementRepository.GetAll()
//                     .Where(x => x.RaydiumPairId == pair.Id)
//                     .GroupBy(x => x.RaydiumPairId)
//                     .Select(g => new
//                     {
//                         TweetsImported = g.Count(),
//                         SumLikes = g.Sum(x => x.LikeCount),
//                         SumReplies = g.Sum(x => x.ReplyCount),
//                         SumRetweets = g.Sum(x => x.RetweetCount),
//                         SumQuotes = g.Sum(x => x.QuoteCount)
//                     })
//                     .FirstOrDefaultAsync();

//                 if (aggregated != null)
//                 {
//                     pair.TweetsCAEngagementTweetsImported = aggregated.TweetsImported;
//                     pair.TweetsCAEngagementTotalLikes = aggregated.SumLikes;
//                     pair.TweetsCAEngagementTotalReplies = aggregated.SumReplies;
//                     pair.TweetsCAEngagementTotalRetweets = aggregated.SumRetweets;
//                     pair.TweetsCAEngagementTotalQuotes = aggregated.SumQuotes;
//                 }

//                 // Mark found if we have a tweet count
//                 if (pair.TweetsCATweetCount > 0 && !pair.TwitterCAFound)
//                 {
//                     pair.TwitterCAFound = true;
//                     pair.TwitterCAFoundAtTime = DateTimeOffset.UtcNow;
//                 }
//             }

//             ComputePoolScore(pool);
//         }

//         var propertiesToUpdate = new List<string>
//         {
//             nameof(RaydiumPair.TweetsCATweetCount),
//             nameof(RaydiumPair.TwitterCAFound),
//             nameof(RaydiumPair.TwitterCAFoundAtTime),
//             nameof(RaydiumPair.TwitterCAFirstMentionTime),
//             nameof(RaydiumPair.TwitterCAFirstMentionTweetId),
//             nameof(RaydiumPair.TwitterCAFirstMentionText),
//             nameof(RaydiumPair.TwitterCAFirstMentionHandle),

//             nameof(RaydiumPair.TweetsCAEngagementTweetsImported),
//             nameof(RaydiumPair.TweetsCAEngagementTotalLikes),
//             nameof(RaydiumPair.TweetsCAEngagementTotalReplies),
//             nameof(RaydiumPair.TweetsCAEngagementTotalRetweets),
//             nameof(RaydiumPair.TweetsCAEngagementTotalQuotes),

//             // nameof(RaydiumPair.TokenPriceChange24NormScore),
//             // nameof(RaydiumPair.TokenLiquidityNormScore),
//             // nameof(RaydiumPair.TokenTweetCountNormScore),
//             // nameof(RaydiumPair.TokenLikeNormScore),
//             // nameof(RaydiumPair.TokenRetweetNormScore),
//             nameof(RaydiumPair.TokenCombinedScore),

//             nameof(RaydiumPair.TwitterCAMostLikesOnSingleTweet),
//             nameof(RaydiumPair.TwitterCAMostRepliesOnSingleTweet)
//         };

//         var raydiumPairsToUpdate = input.Pools.Select(x => x.RaydiumPair).ToList();
//         await _raydiumPairBulkRepository.BulkInsertOrUpdateIncludeAsync(raydiumPairsToUpdate, propertiesToUpdate);
//     }

//     private void ComputePoolScore(TokenPool pool)
//     {
//         var raydiumPair = pool.RaydiumPair;
//         var coingecko = pool.CoinGeckoLastUpdate;

//         float weightPriceChange24h = 0.40f;
//         float weightLiquidity = 0.20f;
//         float weightTweetCount = 0.15f;
//         float weightLikes = 0.15f;
//         float weightRetweets = 0.10f;

//         float priceChange24 = coingecko?.PriceChangeH24 ?? 0;
//         float liquidity = coingecko?.ReserveInUsd ?? 0;

//         float tweetCount = raydiumPair.TweetsCATweetCount;
//         float totalLikes = raydiumPair.TweetsCAEngagementTotalLikes;
//         float totalRetweets = raydiumPair.TweetsCAEngagementTotalRetweets;

//         float normPriceChange24 = priceChange24 / 100f;
//         float normLiquidity = (float)Math.Log10(Math.Max(1, liquidity));
//         float normTweetCount = (float)Math.Log10(Math.Max(1, tweetCount));
//         float normLikes = (float)Math.Log10(Math.Max(1, totalLikes));
//         float normRetweets = (float)Math.Log10(Math.Max(1, totalRetweets));

//         // raydiumPair.TokenPriceChange24NormScore = normPriceChange24;
//         // raydiumPair.TokenLiquidityNormScore = normLiquidity;
//         // raydiumPair.TokenTweetCountNormScore = normTweetCount;
//         // raydiumPair.TokenLikeNormScore = normLikes;
//         // raydiumPair.TokenRetweetNormScore = normRetweets;

//         float score = (weightPriceChange24h * normPriceChange24)
//                     + (weightLiquidity * normLiquidity)
//                     + (weightTweetCount * normTweetCount)
//                     + (weightLikes * normLikes)
//                     + (weightRetweets * normRetweets);

//         raydiumPair.TokenCombinedScore = score;
//     }



//     private TimeSpan CalculateNextRefreshInterval(TimeSpan poolAge)
//     {
//         var refreshIntervalLessThan5Min = TimeSpan.FromSeconds(10);
//         var refreshIntervalBetween5And10Min = TimeSpan.FromSeconds(30);
//         var refreshIntervalBetween10And30Min = TimeSpan.FromSeconds(60);
//         var refreshIntervalBetween30And60Min = TimeSpan.FromSeconds(120);
//         var refreshIntervalBetween1HourAnd12Hour = TimeSpan.FromSeconds(300);
//         var refreshIntervalBetween12HourAnd1Day = TimeSpan.FromSeconds(900);
//         var refreshIntervalOlderThan1Day = TimeSpan.FromSeconds(1800);

//         if (poolAge < TimeSpan.FromMinutes(5))
//         {
//             // Less than 5 minutes old
//             return refreshIntervalLessThan5Min;
//         }
//         if (poolAge < TimeSpan.FromMinutes(10))
//         {
//             // 5 - 10 minutes old
//             return refreshIntervalBetween5And10Min;
//         }
//         if (poolAge < TimeSpan.FromMinutes(30))
//         {
//             // 10 - 30 minutes old
//             return refreshIntervalBetween10And30Min;
//         }
//         if (poolAge < TimeSpan.FromMinutes(60))
//         {
//             // 30 - 60 minutes old
//             return refreshIntervalBetween30And60Min;
//         }
//         if (poolAge < TimeSpan.FromHours(12))
//         {
//             // 1 hour - 12 hours old
//             return refreshIntervalBetween1HourAnd12Hour;
//         }
//         if (poolAge < TimeSpan.FromHours(24))
//         {
//             // 12 hours - 24 hours old
//             return refreshIntervalBetween12HourAnd1Day;
//         }

//         // Older than 1 day
//         return refreshIntervalOlderThan1Day;
//     }
// }

// public static class CoingeckoPoolConverter
// {
//         public static CoingeckoPoolUpdate ToCoingeckoPoolEntity(
//             CoingeckoPoolData data,
//             Guid? raydiumPairId = null
//         )
// {
//     if (data == null) return null;

//     var attr = data.Attributes;

//     var entity = new CoingeckoPoolUpdate
//     {
//         Id = Guid.NewGuid(),

//         CreationTime = DateTimeOffset.UtcNow,
//         RaydiumPairId = raydiumPairId,
//         PoolId = data.Id,
//         PoolType = data.Type,

//         BaseTokenPriceUsd = attr?.BaseTokenPriceUsd,
//         BaseTokenPriceNativeCurrency = attr?.BaseTokenPriceNativeCurrency,
//         QuoteTokenPriceUsd = attr?.QuoteTokenPriceUsd,
//         QuoteTokenPriceNativeCurrency = attr?.QuoteTokenPriceNativeCurrency,
//         BaseTokenPriceQuoteToken = attr?.BaseTokenPriceQuoteToken,
//         QuoteTokenPriceBaseToken = attr?.QuoteTokenPriceBaseToken,

//         Address = attr?.Address,
//         Name = attr?.Name,
//         PoolCreatedAt = attr?.PoolCreatedAt,
//         TokenPriceUsd = ToNullableFloat(attr?.TokenPriceUsd),
//         FdvUsd = ToNullableFloat(attr?.FdvUsd),
//         MarketCapUsd = ToNullableFloat(attr?.MarketCapUsd),

//         PriceChangeM5 = ToNullableFloat(attr?.PriceChangePercentage?.M5),
//         PriceChangeH1 = ToNullableFloat(attr?.PriceChangePercentage?.H1),
//         PriceChangeH6 = ToNullableFloat(attr?.PriceChangePercentage?.H6),
//         PriceChangeH24 = ToNullableFloat(attr?.PriceChangePercentage?.H24),

//         M5Buys = attr?.Transactions?.M5?.Buys,
//         M5Sells = attr?.Transactions?.M5?.Sells,
//         M5Buyers = attr?.Transactions?.M5?.Buyers,
//         M5Sellers = attr?.Transactions?.M5?.Sellers,

//         M15Buys = attr?.Transactions?.M15?.Buys,
//         M15Sells = attr?.Transactions?.M15?.Sells,
//         M15Buyers = attr?.Transactions?.M15?.Buyers,
//         M15Sellers = attr?.Transactions?.M15?.Sellers,

//         M30Buys = attr?.Transactions?.M30?.Buys,
//         M30Sells = attr?.Transactions?.M30?.Sells,
//         M30Buyers = attr?.Transactions?.M30?.Buyers,
//         M30Sellers = attr?.Transactions?.M30?.Sellers,

//         H1Buys = attr?.Transactions?.H1?.Buys,
//         H1Sells = attr?.Transactions?.H1?.Sells,
//         H1Buyers = attr?.Transactions?.H1?.Buyers,
//         H1Sellers = attr?.Transactions?.H1?.Sellers,

//         H24Buys = attr?.Transactions?.H24?.Buys,
//         H24Sells = attr?.Transactions?.H24?.Sells,
//         H24Buyers = attr?.Transactions?.H24?.Buyers,
//         H24Sellers = attr?.Transactions?.H24?.Sellers,

//         VolumeM5 = ToNullableFloat(attr?.VolumeUsd?.M5),
//         VolumeH1 = ToNullableFloat(attr?.VolumeUsd?.H1),
//         VolumeH6 = ToNullableFloat(attr?.VolumeUsd?.H6),
//         VolumeH24 = ToNullableFloat(attr?.VolumeUsd?.H24),

//         ReserveInUsd = ToNullableFloat(attr?.ReserveInUsd),

//         BaseTokenId = data.Relationships?.BaseToken?.Data?.Id,
//         QuoteTokenId = data.Relationships?.QuoteToken?.Data?.Id,
//         DexId = data.Relationships?.Dex?.Data?.Id
//     };

//     return entity;
// }

// private static float? ToNullableFloat(string value)
// {
//     if (string.IsNullOrWhiteSpace(value))
//     {
//         return null; // Return null for empty or whitespace strings.
//     }

//     if (float.TryParse(value, out var result))
//     {
//         return result; // Return the parsed float value.
//     }

//     return null; // Return null if parsing fails.
// }
//     }
