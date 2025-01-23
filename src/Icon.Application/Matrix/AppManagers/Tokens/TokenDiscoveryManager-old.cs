// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using Abp;
// using Abp.Domain.Repositories;
// using Abp.Domain.Services;
// using Abp.Domain.Uow;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.Configuration;
// using Icon.Configuration;
// using Icon.Matrix.Coingecko;
// using Icon.Matrix.Models;
// using Icon.Matrix.Twitter;
// using Icon.EntityFrameworkCore.Matrix;
// using Abp.Json;
// using System.Net.WebSockets;

// namespace Icon.Matrix.TokenDiscovery
// {
//     public interface ITokenDiscoveryManager : IDomainService
//     {
//         Task FullDiscovery();
//     }

//     public class TokenDiscoveryManager : IconServiceBase, ITokenDiscoveryManager
//     {
//         private readonly ICoingeckoService _coingeckoService;
//         private readonly ITwitterAPICommunicationService _twitterAPICommunicationService;

//         private readonly IRepository<CoingeckoPoolUpdate, Guid> _coingeckoPoolUpdateRepository;
//         private readonly IRepository<TwitterImportTweetEngagement, Guid> _twitterImportTweetEngagementRepository;
//         private readonly IRepository<TwitterImportTweetCount, Guid> _twitterImportTweetCountRepository;
//         private readonly IRepository<RaydiumPair, Guid> _raydiumPairRepository;

//         private readonly IMatrixBulkRepository<CoingeckoPoolUpdate> _coingeckoPoolUpdateBulkRepository;
//         private readonly IMatrixBulkRepository<RaydiumPair> _raydiumPairBulkRepository;
//         private readonly IMatrixBulkRepository<TwitterImportTweetEngagement> _twitterImportTweetEngagementBulkRepository;
//         private readonly IMatrixBulkRepository<TwitterImportTweetCount> _twitterImportTweetCountBulkRepository;

//         private readonly IUnitOfWorkManager _unitOfWorkManager;
//         private readonly IConfigurationRoot _configuration;

//         public TokenDiscoveryManager(
//             ICoingeckoService coingeckoService,
//             ITwitterAPICommunicationService twitterAPICommunicationService,
//             IRepository<CoingeckoPoolUpdate, Guid> coingeckoPoolUpdateRepository,
//             IRepository<TwitterImportTweetEngagement, Guid> twitterImportTweetEngagementRepository,
//             IRepository<TwitterImportTweetCount, Guid> twitterImportTweetCountRepository,
//             IRepository<RaydiumPair, Guid> raydiumPairRepository,

//             IMatrixBulkRepository<CoingeckoPoolUpdate> coingeckoPoolUpdateBulkRepository,
//             IMatrixBulkRepository<RaydiumPair> raydiumPairBulkRepository,
//             IMatrixBulkRepository<TwitterImportTweetEngagement> twitterImportTweetEngagementBulkRepository,
//             IMatrixBulkRepository<TwitterImportTweetCount> twitterImportTweetCountBulkRepository,

//             IUnitOfWorkManager unitOfWorkManager,
//             IAppConfigurationAccessor appConfigurationAccessor
//         )
//         {
//             _coingeckoService = coingeckoService;
//             _twitterAPICommunicationService = twitterAPICommunicationService;
//             _coingeckoPoolUpdateRepository = coingeckoPoolUpdateRepository;
//             _twitterImportTweetEngagementRepository = twitterImportTweetEngagementRepository;
//             _twitterImportTweetCountRepository = twitterImportTweetCountRepository;
//             _raydiumPairRepository = raydiumPairRepository;

//             _coingeckoPoolUpdateBulkRepository = coingeckoPoolUpdateBulkRepository;
//             _raydiumPairBulkRepository = raydiumPairBulkRepository;
//             _twitterImportTweetEngagementBulkRepository = twitterImportTweetEngagementBulkRepository;
//             _twitterImportTweetCountBulkRepository = twitterImportTweetCountBulkRepository;

//             _unitOfWorkManager = unitOfWorkManager;
//             _configuration = appConfigurationAccessor.Configuration;
//         }


//         [UnitOfWork]
//         public async Task FullDiscovery()
//         {
//             Logger.Info("Starting FullDiscovery...");

//             var minDate = DateTimeOffset.UtcNow.AddHours(-2);

//             var allPairs = await _raydiumPairRepository
//                 .GetAll()
//                 .Where(x =>
//                     x.DiscoveryStageName != TokenStageDefinitions.Stages.Death &&
//                     x.CreationTime > minDate)
//                 .ToListAsync();

//             var pairsToUpdate = new List<RaydiumPair>();

//             foreach (var pair in allPairs)
//             {
//                 if (string.IsNullOrWhiteSpace(pair.DiscoveryStageName))
//                 {
//                     SetStage(pair, TokenStageDefinitions.Stages.Inception);
//                 }

//                 if (pair.DiscoveryStageName != TokenStageDefinitions.Stages.Death)
//                 {
//                     await UpdatePriceForPair(pair);
//                     CheckAndPerformStageTransition(pair);
//                 }

//                 while (true)
//                 {
//                     var oldStage = pair.DiscoveryStageName;
//                     await ApplyStageLogic(pair);

//                     CheckAndPerformStageTransition(pair);
//                     if (pair.DiscoveryStageName == oldStage ||
//                         pair.DiscoveryStageName == TokenStageDefinitions.Stages.Death)
//                     {
//                         break;
//                     }
//                 }

//                 pairsToUpdate.Add(pair);
//             }

//             var propertiesToUpdate = new List<string>
//             {
//                 nameof(RaydiumPair.DiscoveryStageName),
//                 nameof(RaydiumPair.DiscoveryStageLastUpdated),
//                 nameof(RaydiumPair.LiquidityUsd),
//                 nameof(RaydiumPair.TweetsCATweetCount),
//                 nameof(RaydiumPair.TweetsCATweetCount1H),
//                 nameof(RaydiumPair.TweetsCATweetCount3H),
//                 nameof(RaydiumPair.TweetsCATweetCount6H),
//                 nameof(RaydiumPair.TweetsCATweetCount12H),
//                 nameof(RaydiumPair.TweetsCATweetCount24H),
//                 nameof(RaydiumPair.TweetsCATweetCount1WK),
//                 nameof(RaydiumPair.TwitterCAQueryCount),
//                 nameof(RaydiumPair.TwitterCAFirstMentionTime),
//                 nameof(RaydiumPair.TwitterCAFound),
//                 nameof(RaydiumPair.TwitterCAFoundAtTime),
//                 nameof(RaydiumPair.TwitterCAFirstMentionTweetId),
//                 nameof(RaydiumPair.TwitterCAFirstMentionText),
//                 nameof(RaydiumPair.TwitterCAFirstMentionHandle),
//                 nameof(RaydiumPair.TwitterCAMostLikesOnSingleTweet),
//                 nameof(RaydiumPair.TwitterCAMostRepliesOnSingleTweet),
//                 nameof(RaydiumPair.TweetsCAEngagementTweetsImported),
//                 nameof(RaydiumPair.TweetsCAEngagementTotalLikes),
//                 nameof(RaydiumPair.TweetsCAEngagementTotalReplies),
//                 nameof(RaydiumPair.TweetsCAEngagementTotalRetweets),
//                 nameof(RaydiumPair.TweetsCAEngagementTotalQuotes),
//                 nameof(RaydiumPair.PriceRefreshLastUpdateTime),
//                 nameof(RaydiumPair.VlrValue),
//                 nameof(RaydiumPair.VlrQualitativeAnalysis),
//                 nameof(RaydiumPair.VlrRecommendation),
//                 nameof(RaydiumPair.BuySellRatioValue),
//                 nameof(RaydiumPair.BuySellRatioQualitativeAnalysis),
//                 nameof(RaydiumPair.BuySellRatioRecommendation),
//                 nameof(RaydiumPair.VolumeToFdv1HValue),
//                 nameof(RaydiumPair.VolumeToFdv1HQualitativeAnalysis),
//                 nameof(RaydiumPair.VolumeToFdv1HRecommendation),
//                 nameof(RaydiumPair.CombinedMetricScore),
//                 nameof(RaydiumPair.CombinedQualitativeAnalysis),
//                 nameof(RaydiumPair.CombinedRecommendation),
//                 nameof(RaydiumPair.EngagementCorrelationAnalysis),
//                 nameof(RaydiumPair.EngagementCorrelationScore),
//                 nameof(RaydiumPair.PlantFinalSummary),
//                 nameof(RaydiumPair.PlantFinalPrediction)
//             };

//             await _raydiumPairBulkRepository.BulkInsertOrUpdateIncludeAsync(pairsToUpdate, propertiesToUpdate);

//             Logger.Info("FullDiscovery completed.");
//         }

//         private async Task ApplyStageLogic(RaydiumPair pair)
//         {
//             switch (pair.DiscoveryStageName)
//             {
//                 case var s when s == TokenStageDefinitions.Stages.Death:
//                     Logger.Debug($"Pair {pair.Id} is in Death stage, skipping logic.");
//                     break;

//                 case var s when s == TokenStageDefinitions.Stages.Inception:
//                     Logger.Debug($"Pair {pair.Id} is in Inception stage, no direct logic here.");
//                     break;

//                 case var s when s == TokenStageDefinitions.Stages.PriceTracking:
//                     Logger.Debug($"Pair {pair.Id} is in PriceTracking stage, no extra logic after price update.");
//                     break;

//                 case var s when s == TokenStageDefinitions.Stages.EngagementPostTracking:
//                     Logger.Debug($"Pair {pair.Id} in EngagementPostTracking => updating tweet counts...");
//                     await UpdateTweetCountsForPair(pair);
//                     break;

//                 case var s when s == TokenStageDefinitions.Stages.EngagementDetailTracking:
//                     Logger.Debug($"Pair {pair.Id} in EngagementDetailTracking => updating tweet counts & engagement...");
//                     await UpdateTweetCountsForPair(pair);
//                     await UpdateTweetEngagementForPair(pair);
//                     break;

//                 default:
//                     Logger.Warn($"Pair {pair.Id} has unknown stage {pair.DiscoveryStageName}. Doing nothing.");
//                     break;
//             }
//         }

//         private void CheckAndPerformStageTransition(RaydiumPair pair)
//         {
//             var currentStageName = pair.DiscoveryStageName;
//             var currentDefinition = TokenStageDefinitions.Definitions[currentStageName];

//             if (currentStageName == TokenStageDefinitions.Stages.Death)
//             {
//                 // do nothing
//                 return;
//             }

//             else if (currentStageName == TokenStageDefinitions.Stages.Inception)
//             {
//                 SetStage(pair, TokenStageDefinitions.Stages.PriceTracking);
//             }

//             else if (currentStageName == TokenStageDefinitions.Stages.PriceTracking)
//             {
//                 var liquidityUsd = pair.LiquidityUsd ?? 0;
//                 var minRequired = currentDefinition.MinLiquidityUsdForStage;

//                 if (liquidityUsd < minRequired)
//                 {
//                     SetStage(pair, TokenStageDefinitions.Stages.Death);
//                 }
//                 else
//                 {
//                     if (liquidityUsd >= currentDefinition.MaxLiquidityUsdForStage)
//                     {
//                         SetStage(pair, TokenStageDefinitions.Stages.EngagementPostTracking);
//                     }
//                 }
//             }

//             else if (currentStageName == TokenStageDefinitions.Stages.EngagementPostTracking)
//             {
//                 var liquidityUsd = pair.LiquidityUsd ?? 0;
//                 var minRequired = TokenStageDefinitions.Definitions[TokenStageDefinitions.Stages.EngagementPostTracking].MinLiquidityUsdForStage;

//                 if (liquidityUsd < minRequired)
//                 {
//                     SetStage(pair, TokenStageDefinitions.Stages.PriceTracking);
//                 }
//                 else
//                 {
//                     if (pair.TweetsCATweetCount > 0)
//                     {
//                         SetStage(pair, TokenStageDefinitions.Stages.EngagementDetailTracking);
//                     }
//                 }
//             }

//             else if (currentStageName == TokenStageDefinitions.Stages.EngagementDetailTracking)
//             {
//                 var liquidityUsd = pair.LiquidityUsd ?? 0;
//                 var minRequired = TokenStageDefinitions.Definitions[TokenStageDefinitions.Stages.EngagementDetailTracking].MinLiquidityUsdForStage;
//                 if (liquidityUsd < minRequired)
//                 {
//                     SetStage(pair, TokenStageDefinitions.Stages.PriceTracking);
//                 }
//             }
//         }

//         private void SetStage(RaydiumPair pair, string newStageName)
//         {
//             if (pair.DiscoveryStageName == newStageName)
//                 return; // No change

//             pair.DiscoveryStageName = newStageName;
//             pair.DiscoveryStageLastUpdated = DateTimeOffset.UtcNow;

//             Logger.Debug($"Pair {pair.Id} moved to stage: {newStageName}");
//         }

//         private async Task UpdatePriceForPair(RaydiumPair pair)
//         {
//             if (string.IsNullOrWhiteSpace(pair.BaseTokenAccount))
//                 return;

//             CoingeckoPoolsResponse coingeckoPoolsResponse;
//             try
//             {
//                 coingeckoPoolsResponse = await _coingeckoService.GetPoolsAsync(pair.BaseTokenAccount);
//             }
//             catch (Exception ex)
//             {
//                 Logger.Error($"Error getting coingecko pools for {pair.BaseTokenAccount}", ex);
//                 return;
//             }

//             if (coingeckoPoolsResponse?.Data == null)
//                 return;

//             pair.PriceRefreshLastUpdateTime = DateTimeOffset.UtcNow;


//             var aggregated = AggregateAllPools(coingeckoPoolsResponse.Data);
//             pair.TotalLiquidityUsd = aggregated.TotalLiquidityUsd;


//             // 3) If you want to do further extended calculations, pass `aggregated` into your method:
//             // CalculateExtendedMetrics(pair, aggregated);

//             // 4) Optionally record each pool’s raw data (CoingeckoPoolUpdate) in DB as before
//             var poolUpdates = new List<CoingeckoPoolUpdate>();
//             var priceGroupId = Guid.NewGuid();
//             foreach (var data in coingeckoPoolsResponse.Data)
//             {
//                 if (data.Attributes == null) continue;

//                 var entity = ToCoingeckoPoolEntity(data, pair.Id);
//                 if (entity == null) continue;

//                 entity.PriceGroupId = priceGroupId;
//                 poolUpdates.Add(entity);
//             }

//             if (poolUpdates.Any())
//             {
//                 await _coingeckoPoolUpdateBulkRepository.BulkInsertAsync(poolUpdates);
//             }
//         }


//         // private async Task UpdatePriceForPair(RaydiumPair pair)
//         // {
//         //     if (string.IsNullOrEmpty(pair.BaseTokenAccount))
//         //     {
//         //         return;
//         //     }

//         //     CoingeckoPoolsResponse coingeckoPoolsResponse = null;
//         //     try
//         //     {
//         //         coingeckoPoolsResponse = await _coingeckoService.GetPoolsAsync(pair.BaseTokenAccount);
//         //     }
//         //     catch (Exception ex)
//         //     {
//         //         Logger.Error($"Error getting coingecko pools for {pair.BaseTokenAccount}", ex);
//         //         return;
//         //     }

//         //     if (coingeckoPoolsResponse == null || coingeckoPoolsResponse.Data == null)
//         //     {
//         //         return;
//         //     }

//         //     pair.PriceRefreshLastUpdateTime = DateTimeOffset.UtcNow;

//         //     var firstData = coingeckoPoolsResponse.Data.FirstOrDefault();
//         //     if (firstData?.Attributes != null)
//         //     {
//         //         float? parsedReserve = ToNullableFloat(firstData.Attributes.ReserveInUsd);
//         //         if (parsedReserve.HasValue)
//         //         {
//         //             pair.LiquidityUsd = parsedReserve.Value;
//         //         }

//         //         CalculateExtendedMetrics(pair, firstData?.Attributes);
//         //     }


//         //     var poolUpdates = new List<CoingeckoPoolUpdate>();
//         //     var priceGroupId = Guid.NewGuid();
//         //     foreach (var coingeckoPoolUpdate in coingeckoPoolsResponse.Data)
//         //     {
//         //         if (coingeckoPoolUpdate.Attributes == null)
//         //         {
//         //             continue;
//         //         }

//         //         var entity = ToCoingeckoPoolEntity(coingeckoPoolUpdate, pair.Id);
//         //         if (entity == null)
//         //         {
//         //             continue;
//         //         }
//         //         entity.PriceGroupId = priceGroupId;

//         //         poolUpdates.Add(entity);
//         //     }

//         //     if (poolUpdates.Any())
//         //     {
//         //         await _coingeckoPoolUpdateBulkRepository.BulkInsertAsync(poolUpdates);
//         //     }
//         // }

//         private async Task UpdateTweetCountsForPair(RaydiumPair pair)
//         {
//             if (string.IsNullOrWhiteSpace(pair.BaseTokenAccount))
//                 return;

//             var tweetCounts = new List<TwitterImportTweetCount>();
//             var countsResponse = await _twitterAPICommunicationService.GetTweetCountsAsync(pair.BaseTokenAccount);
//             pair.TwitterCAQueryCount++;

//             if (countsResponse?.Data == null || !countsResponse.Data.Any())
//             {
//                 return;
//             }


//             foreach (var bucket in countsResponse.Data)
//             {
//                 var entity = new TwitterImportTweetCount
//                 {
//                     Id = Guid.NewGuid(),
//                     RaydiumPairId = pair.Id,
//                     SearchQuery = pair.BaseTokenAccount,
//                     StartTime = bucket.Start,
//                     EndTime = bucket.End,
//                     TweetCount = bucket.Tweet_Count,
//                     CreationTime = DateTimeOffset.UtcNow
//                 };
//                 tweetCounts.Add(entity);

//                 if (bucket.Tweet_Count > 0)
//                 {
//                     if (!pair.TwitterCAFirstMentionTime.HasValue ||
//                             bucket.End < pair.TwitterCAFirstMentionTime.Value)
//                     {
//                         pair.TwitterCAFirstMentionTime = bucket.End;
//                     }
//                 }
//             }

//             if (tweetCounts.Any())
//             {
//                 await _twitterImportTweetCountBulkRepository.BulkInsertOrUpdateIncludeByAsync(
//                     entities: tweetCounts,
//                     propertiesToIncludeOnUpdate: new List<string> {
//                         nameof(TwitterImportTweetCount.TweetCount),
//                     },
//                     UpdateBy: new List<string> {
//                         nameof(TwitterImportTweetCount.RaydiumPairId),
//                         nameof(TwitterImportTweetCount.SearchQuery),
//                         nameof(TwitterImportTweetCount.StartTime),
//                         nameof(TwitterImportTweetCount.EndTime)
//                     }
//                 );

//                 var UtcNow = DateTimeOffset.UtcNow;

//                 var tweets1H = tweetCounts.TakeLast(1)?.FirstOrDefault()?.TweetCount ?? 0;
//                 var tweets3H = tweetCounts.TakeLast(3)?.Sum(x => x.TweetCount) ?? 0;
//                 var tweets6H = tweetCounts.TakeLast(6)?.Sum(x => x.TweetCount) ?? 0;
//                 var tweets12H = tweetCounts.TakeLast(12)?.Sum(x => x.TweetCount) ?? 0;
//                 var tweets24H = tweetCounts.TakeLast(24)?.Sum(x => x.TweetCount) ?? 0;
//                 var tweets1WK = tweetCounts.Sum(x => x.TweetCount);

//                 pair.TweetsCATweetCount1H = tweets1H;
//                 pair.TweetsCATweetCount3H = tweets3H;
//                 pair.TweetsCATweetCount6H = tweets6H;
//                 pair.TweetsCATweetCount12H = tweets12H;
//                 pair.TweetsCATweetCount24H = tweets24H;
//                 pair.TweetsCATweetCount1WK = tweets1WK;

//                 if (countsResponse.Meta?.Total_Tweet_Count != null)
//                 {
//                     pair.TweetsCATweetCount = countsResponse.Meta.Total_Tweet_Count;
//                 }

//                 if (pair.TweetsCATweetCount > 0 && !pair.TwitterCAFound)
//                 {
//                     pair.TwitterCAFound = true;
//                     pair.TwitterCAFoundAtTime = DateTimeOffset.UtcNow;
//                 }
//             }
//         }

//         private async Task UpdateTweetEngagementForPair(RaydiumPair pair)
//         {
//             if (string.IsNullOrWhiteSpace(pair.BaseTokenAccount))
//                 return;

//             var tweets = await _twitterAPICommunicationService.GetTweetsFullEngagementAsync(
//                 searchQuery: pair.BaseTokenAccount,
//                 limit: 100
//             );

//             pair.TwitterCAQueryCount++;
//             var upsertList = new List<TwitterImportTweetEngagement>();
//             foreach (var t in tweets)
//             {
//                 var entity = new TwitterImportTweetEngagement
//                 {
//                     Id = Guid.NewGuid(),
//                     RaydiumPairId = pair.Id,
//                     TweetId = t.Id,
//                     AuthorId = t.AuthorId,
//                     Text = t.Text,
//                     CreatedAt = t.CreatedAt,
//                     LikeCount = t.PublicMetrics?.LikeCount ?? 0,
//                     ReplyCount = t.PublicMetrics?.ReplyCount ?? 0,
//                     RetweetCount = t.PublicMetrics?.RetweetCount ?? 0,
//                     QuoteCount = t.PublicMetrics?.QuoteCount ?? 0,
//                     LastUpdatedAt = DateTimeOffset.UtcNow
//                 };
//                 upsertList.Add(entity);

//                 if (!pair.TwitterCAFirstMentionTime.HasValue ||
//                         t.CreatedAt < pair.TwitterCAFirstMentionTime.Value)
//                 {
//                     pair.TwitterCAFirstMentionTime = t.CreatedAt;
//                     pair.TwitterCAFirstMentionTweetId = t.Id;
//                     pair.TwitterCAFirstMentionText = t.Text;
//                     pair.TwitterCAFirstMentionHandle = t.AuthorId;
//                 }

//                 if (pair.TwitterCAMostLikesOnSingleTweet < entity.LikeCount)
//                 {
//                     pair.TwitterCAMostLikesOnSingleTweet = entity.LikeCount;
//                 }

//                 if (pair.TwitterCAMostRepliesOnSingleTweet < entity.ReplyCount)
//                 {
//                     pair.TwitterCAMostRepliesOnSingleTweet = entity.ReplyCount;
//                 }
//             }

//             if (upsertList.Any())
//             {
//                 await _twitterImportTweetEngagementBulkRepository.BulkInsertOrUpdateIncludeByAsync(
//                     entities: upsertList,
//                     propertiesToIncludeOnUpdate: new List<string> {
//                         nameof(TwitterImportTweetEngagement.AuthorId),
//                         nameof(TwitterImportTweetEngagement.Text),
//                         nameof(TwitterImportTweetEngagement.CreatedAt),
//                         nameof(TwitterImportTweetEngagement.LikeCount),
//                         nameof(TwitterImportTweetEngagement.ReplyCount),
//                         nameof(TwitterImportTweetEngagement.RetweetCount),
//                         nameof(TwitterImportTweetEngagement.QuoteCount),
//                         nameof(TwitterImportTweetEngagement.LastUpdatedAt)
//                     },
//                     UpdateBy: new List<string> {
//                         nameof(TwitterImportTweetEngagement.RaydiumPairId),
//                         nameof(TwitterImportTweetEngagement.TweetId)
//                     }
//                 );
//             }

//             var aggregated = await _twitterImportTweetEngagementRepository.GetAll()
//                 .Where(x => x.RaydiumPairId == pair.Id)
//                 .GroupBy(x => x.RaydiumPairId)
//                 .Select(g => new
//                 {
//                     TweetsImported = g.Count(),
//                     SumLikes = g.Sum(x => x.LikeCount),
//                     SumReplies = g.Sum(x => x.ReplyCount),
//                     SumRetweets = g.Sum(x => x.RetweetCount),
//                     SumQuotes = g.Sum(x => x.QuoteCount)
//                 })
//                 .FirstOrDefaultAsync();

//             if (aggregated != null)
//             {
//                 pair.TweetsCAEngagementTweetsImported = aggregated.TweetsImported;
//                 pair.TweetsCAEngagementTotalLikes = aggregated.SumLikes;
//                 pair.TweetsCAEngagementTotalReplies = aggregated.SumReplies;
//                 pair.TweetsCAEngagementTotalRetweets = aggregated.SumRetweets;
//                 pair.TweetsCAEngagementTotalQuotes = aggregated.SumQuotes;
//             }

//             if (pair.TweetsCATweetCount > 0 && !pair.TwitterCAFound)
//             {
//                 pair.TwitterCAFound = true;
//                 pair.TwitterCAFoundAtTime = DateTimeOffset.UtcNow;
//             }

//             AnalyzeEngagementCorrelationAndSetSummary(pair);
//         }

//         private static float? ToNullableFloat(string value)
//         {
//             if (string.IsNullOrWhiteSpace(value))
//             {
//                 return null;
//             }
//             if (float.TryParse(value, out var result))
//             {
//                 return result;
//             }
//             return null;
//         }


//         public static CoingeckoPoolUpdate ToCoingeckoPoolEntity(
//             CoingeckoPoolData data,
//             Guid? raydiumPairId = null
//         )
//         {
//             if (data == null) return null;

//             var attr = data.Attributes;

//             var entity = new CoingeckoPoolUpdate
//             {
//                 Id = Guid.NewGuid(),

//                 CreationTime = DateTimeOffset.UtcNow,
//                 RaydiumPairId = raydiumPairId,
//                 PoolId = data.Id,
//                 PoolType = data.Type,

//                 BaseTokenPriceUsd = attr?.BaseTokenPriceUsd,
//                 BaseTokenPriceNativeCurrency = attr?.BaseTokenPriceNativeCurrency,
//                 QuoteTokenPriceUsd = attr?.QuoteTokenPriceUsd,
//                 QuoteTokenPriceNativeCurrency = attr?.QuoteTokenPriceNativeCurrency,
//                 BaseTokenPriceQuoteToken = attr?.BaseTokenPriceQuoteToken,
//                 QuoteTokenPriceBaseToken = attr?.QuoteTokenPriceBaseToken,

//                 Address = attr?.Address,
//                 Name = attr?.Name,
//                 PoolCreatedAt = attr?.PoolCreatedAt,
//                 TokenPriceUsd = ToNullableFloat(attr?.TokenPriceUsd),
//                 FdvUsd = ToNullableFloat(attr?.FdvUsd),
//                 MarketCapUsd = ToNullableFloat(attr?.MarketCapUsd),

//                 PriceChangeM5 = ToNullableFloat(attr?.PriceChangePercentage?.M5),
//                 PriceChangeH1 = ToNullableFloat(attr?.PriceChangePercentage?.H1),
//                 PriceChangeH6 = ToNullableFloat(attr?.PriceChangePercentage?.H6),
//                 PriceChangeH24 = ToNullableFloat(attr?.PriceChangePercentage?.H24),

//                 M5Buys = attr?.Transactions?.M5?.Buys,
//                 M5Sells = attr?.Transactions?.M5?.Sells,
//                 M5Buyers = attr?.Transactions?.M5?.Buyers,
//                 M5Sellers = attr?.Transactions?.M5?.Sellers,

//                 M15Buys = attr?.Transactions?.M15?.Buys,
//                 M15Sells = attr?.Transactions?.M15?.Sells,
//                 M15Buyers = attr?.Transactions?.M15?.Buyers,
//                 M15Sellers = attr?.Transactions?.M15?.Sellers,

//                 M30Buys = attr?.Transactions?.M30?.Buys,
//                 M30Sells = attr?.Transactions?.M30?.Sells,
//                 M30Buyers = attr?.Transactions?.M30?.Buyers,
//                 M30Sellers = attr?.Transactions?.M30?.Sellers,

//                 H1Buys = attr?.Transactions?.H1?.Buys,
//                 H1Sells = attr?.Transactions?.H1?.Sells,
//                 H1Buyers = attr?.Transactions?.H1?.Buyers,
//                 H1Sellers = attr?.Transactions?.H1?.Sellers,

//                 H24Buys = attr?.Transactions?.H24?.Buys,
//                 H24Sells = attr?.Transactions?.H24?.Sells,
//                 H24Buyers = attr?.Transactions?.H24?.Buyers,
//                 H24Sellers = attr?.Transactions?.H24?.Sellers,

//                 VolumeM5 = ToNullableFloat(attr?.VolumeUsd?.M5),
//                 VolumeH1 = ToNullableFloat(attr?.VolumeUsd?.H1),
//                 VolumeH6 = ToNullableFloat(attr?.VolumeUsd?.H6),
//                 VolumeH24 = ToNullableFloat(attr?.VolumeUsd?.H24),

//                 ReserveInUsd = ToNullableFloat(attr?.ReserveInUsd),

//                 BaseTokenId = data.Relationships?.BaseToken?.Data?.Id,
//                 QuoteTokenId = data.Relationships?.QuoteToken?.Data?.Id,
//                 DexId = data.Relationships?.Dex?.Data?.Id
//             };

//             return entity;
//         }

//         /// <summary>
//         /// Calculates the extended metrics (VLR, B/S ratio, 1h Volume-to-FDV ratio, etc.)
//         /// and stores them as properties on the RaydiumPair.
//         /// </summary>
//         /// <param name="pair">The RaydiumPair entity to update.</param>
//         /// <param name="attr">The CoingeckoPoolData.Attributes from the latest update (or any you choose).</param>
//         private void CalculateExtendedMetrics(RaydiumPair pair, CoingeckoPoolAttributes attr)
//         {
//             if (attr == null)
//                 return;

//             // 1) Get the age of the token in hours
//             var pairAge = DateTimeOffset.UtcNow - pair.CreationTime;
//             var pairAgeHours = pairAge.TotalHours;

//             // 2) We'll read the 1-hour volume from the Coingecko attribute (VolumeH1).
//             //    Also read FDV, Buys/Sells, etc. If you prefer 24-hour or M5, adapt accordingly.
//             float? volumeH1 = ToNullableFloat(attr.VolumeUsd?.H1);
//             float? fdvUsd = ToNullableFloat(attr.FdvUsd);
//             float liquidity = pair.LiquidityUsd ?? 0f;

//             // For Buys/Sells, we might sum the last 1 hour buys/sells from coingecko. 
//             // As an example, we might approximate by adding M30 + M15 + M5 if that's how you store it.
//             // Or if you have direct H1 buys/sells, use that.
//             int totalBuysH1 = (attr.Transactions?.M5?.Buys ?? 0)
//                             + (attr.Transactions?.M15?.Buys ?? 0)
//                             + (attr.Transactions?.M30?.Buys ?? 0)
//                             + (attr.Transactions?.H1?.Buys ?? 0);

//             int totalSellsH1 = (attr.Transactions?.M5?.Sells ?? 0)
//                              + (attr.Transactions?.M15?.Sells ?? 0)
//                              + (attr.Transactions?.M30?.Sells ?? 0)
//                              + (attr.Transactions?.H1?.Sells ?? 0);

//             // ------------------------------------------------------------------------
//             // VLR = Volume / Liquidity  (Here we'll use the H1 volume for demonstration)
//             // ------------------------------------------------------------------------
//             float? vlr = null;
//             if (volumeH1.HasValue && liquidity > 0)
//             {
//                 vlr = volumeH1.Value / liquidity;
//             }

//             // Evaluate VLR ranges from conversation
//             if (vlr.HasValue)
//             {
//                 // We'll do a simple method that returns (analysis, recommendation).
//                 var (analysis, recommendation) = GetVlrAnalysisAndRecommendation(vlr.Value, pairAgeHours);
//                 pair.VlrValue = vlr.Value;
//                 pair.VlrQualitativeAnalysis = analysis;
//                 pair.VlrRecommendation = recommendation;
//             }
//             else
//             {
//                 pair.VlrValue = null;
//                 pair.VlrQualitativeAnalysis = "Not enough data to compute VLR";
//                 pair.VlrRecommendation = "No recommendation";
//             }

//             // ------------------------------------------------------------------------
//             // B/S Ratio = totalBuys / totalSells
//             // ------------------------------------------------------------------------
//             float? bSRatio = null;
//             if (totalSellsH1 > 0)
//             {
//                 bSRatio = (float)totalBuysH1 / totalSellsH1;
//             }
//             else if (totalBuysH1 > 0)
//             {
//                 // If totalSellsH1 = 0 but we have buys, ratio is effectively "infinite".
//                 bSRatio = totalBuysH1; // or some large value to denote "all buys"
//             }

//             if (bSRatio.HasValue)
//             {
//                 var (analysis, recommendation) = GetBuySellRatioAnalysisAndRecommendation(bSRatio.Value);
//                 pair.BuySellRatioValue = bSRatio.Value;
//                 pair.BuySellRatioQualitativeAnalysis = analysis;
//                 pair.BuySellRatioRecommendation = recommendation;
//             }
//             else
//             {
//                 pair.BuySellRatioValue = null;
//                 pair.BuySellRatioQualitativeAnalysis = "Not enough data to compute B/S ratio";
//                 pair.BuySellRatioRecommendation = "No recommendation";
//             }

//             // ------------------------------------------------------------------------
//             // 1-Hour Volume-to-FDV Ratio = (volumeH1 / fdv)
//             // ------------------------------------------------------------------------
//             float? volToFdvRatio = null;
//             if (volumeH1.HasValue && fdvUsd.HasValue && fdvUsd.Value > 0)
//             {
//                 volToFdvRatio = volumeH1.Value / fdvUsd.Value;
//             }

//             if (volToFdvRatio.HasValue)
//             {
//                 var (analysis, recommendation) = GetVolumeToFdvAnalysisAndRecommendation(volToFdvRatio.Value);
//                 pair.VolumeToFdv1HValue = volToFdvRatio.Value;
//                 pair.VolumeToFdv1HQualitativeAnalysis = analysis;
//                 pair.VolumeToFdv1HRecommendation = recommendation;
//             }
//             else
//             {
//                 pair.VolumeToFdv1HValue = null;
//                 pair.VolumeToFdv1HQualitativeAnalysis = "Not enough data to compute 1H Volume-to-FDV";
//                 pair.VolumeToFdv1HRecommendation = "No recommendation";
//             }

//             // ------------------------------------------------------------------------
//             // Combine them into a final "score" or "analysis"
//             // Simple approach: each metric returns a numeric score 1-10, we average them
//             // and build a final recommendation string.
//             // ------------------------------------------------------------------------
//             float vlrScore = GetVlrScore(vlr ?? 0f);
//             float bsScore = GetBuySellRatioScore(bSRatio ?? 0f);
//             float volFdvScore = GetVolumeToFdvScore(volToFdvRatio ?? 0f);

//             // For brand-new tokens, you might reduce or adjust the score to reflect uncertainty:
//             if (pairAgeHours < 1)
//             {
//                 // Example: reduce total score by 20% for brand new tokens
//                 vlrScore *= 0.8f;
//                 bsScore *= 0.8f;
//                 volFdvScore *= 0.8f;
//             }

//             float finalScore = (vlrScore + bsScore + volFdvScore) / 3f;
//             pair.CombinedMetricScore = finalScore;

//             var finalQa = "Overall Balanced";
//             var finalRec = "Proceed with normal caution";
//             if (finalScore > 8) { finalQa = "Very Strong Indicators"; finalRec = "High-potential token"; }
//             else if (finalScore > 5) { finalQa = "Moderate Indicators"; finalRec = "Decent short-term potential"; }
//             else if (finalScore > 3) { finalQa = "Weak Indicators"; finalRec = "Caution advised"; }
//             else { finalQa = "Very Weak Indicators"; finalRec = "Likely avoid"; }

//             // If the token is extremely new:
//             if (pairAgeHours < 1)
//             {
//                 finalQa += " (Token <1h old)";
//                 finalRec += " - very new token, data may be unreliable";
//             }

//             pair.CombinedQualitativeAnalysis = finalQa;
//             pair.CombinedRecommendation = finalRec;
//         }

//         /// <summary>
//         /// Interpret the VLR for a token of a certain age (in hours).
//         /// Return (QualitativeAnalysis, Recommendation) text.
//         /// </summary>
//         private (string Analysis, string Recommendation) GetVlrAnalysisAndRecommendation(float vlr, double ageHours)
//         {
//             // If token is <1 day old, we might have "sprouting token" logic:
//             // e.g. for newly launched tokens, aim for 3:1 to 5:1 ratio.
//             // This is just an example; adapt your text as needed.
//             if (vlr < 1.0f)
//                 return ("VLR below 1:1, low trading activity", "Likely lacking momentum, watch for volume pickup");
//             if (vlr < 2.0f)
//                 return ("VLR 1:1 to 2:1, balanced market", "Potentially stable trading with adequate liquidity");
//             if (vlr < 5.0f)
//                 return ("VLR 2:1 to 5:1, healthy momentum", "Good range for growth with manageable risk");
//             if (vlr < 10.0f)
//                 return ("VLR 5:1 to 10:1, speculative interest", "High volume relative to liquidity, watch slippage");
//             // else
//             return ("VLR > 10:1, extremely high volume vs liquidity", "High risk/high reward scenario, watch volatility");
//         }

//         /// <summary>
//         /// Convert VLR to a rough numeric score for combining into a final total (1-10 scale).
//         /// </summary>
//         private float GetVlrScore(float vlr)
//         {
//             if (vlr < 1) return 3f;       // under 1 => weak
//             if (vlr < 2) return 7f;       // 1-2 => good
//             if (vlr < 5) return 9f;       // 2-5 => very good
//             if (vlr < 10) return 6f;      // 5-10 => somewhat risky
//             return 2f;                    // above 10 => quite risky
//         }

//         private (string Analysis, string Recommendation) GetBuySellRatioAnalysisAndRecommendation(float bSRatio)
//         {
//             // Example from the conversation:
//             if (bSRatio < 1.0f)
//                 return ("Bearish sentiment, more sells than buys", "Potential discount or fading interest");
//             if (bSRatio < 1.5f)
//                 return ("Mildly bullish, near 1:1", "Stable, early accumulation possible");
//             if (bSRatio < 3.0f)
//                 return ("Bullish, strong accumulation", "Momentum traders can look for an uptrend");
//             if (bSRatio < 5.0f)
//                 return ("Very bullish, parabolic potential", "Great for momentum but watch for pullbacks");
//             if (bSRatio < 10.0f)
//                 return ("Hyper-bullish, mania territory", "High risk of reversal after euphoria");
//             return ("Extreme euphoria, unsustainable", "Only for degens; expect sharp reversals");
//         }

//         private float GetBuySellRatioScore(float bSRatio)
//         {
//             if (bSRatio < 1) return 3f;      // bearish
//             if (bSRatio < 1.5f) return 5f;   // mildly bullish
//             if (bSRatio < 3f) return 7f;     // bullish
//             if (bSRatio < 5f) return 8f;     // very bullish
//             if (bSRatio < 10f) return 6f;    // mania
//             return 2f;                       // extreme mania
//         }

//         private (string Analysis, string Recommendation) GetVolumeToFdvAnalysisAndRecommendation(float ratio)
//         {
//             // Using conversation’s 1-hour Volume-to-FDV guidelines:
//             if (ratio < 0.001f)
//                 return ("Extremely low 1H activity for FDV", "Likely overvalued or stagnant");
//             if (ratio < 0.005f)
//                 return ("Low 1H activity, underperforming", "Suitable for cautious, long-term entry");
//             if (ratio < 0.01f)
//                 return ("Moderate 1H activity, building momentum", "Watch for further volume increases");
//             if (ratio < 0.02f)
//                 return ("Strong 1H activity, bullish sentiment", "Good sign for healthy short-term trading");
//             return ("Extremely high 1H activity, potential mania", "High-risk/high-reward; monitor closely");
//         }

//         private float GetVolumeToFdvScore(float ratio)
//         {
//             if (ratio < 0.001f) return 2f;
//             if (ratio < 0.005f) return 4f;
//             if (ratio < 0.01f) return 6f;
//             if (ratio < 0.02f) return 8f;
//             return 10f;
//         }

//         /// <summary>
//         /// Uses the token’s VLR, B/S ratio, VolumeToFdv, plus engagement data 
//         /// (tweets in last hour, total tweets, etc.) to produce a final correlation 
//         /// analysis, summary, and prediction stored in the pair.
//         /// </summary>
//         private void AnalyzeEngagementCorrelationAndSetSummary(RaydiumPair pair)
//         {
//             // 1) Basic concurrency checks
//             if (pair == null) return;

//             // 2) Combine the newly computed metrics with engagement data
//             float vlr = pair.VlrValue ?? 0f;
//             float bSRatio = pair.BuySellRatioValue ?? 0f;
//             float volToFdv = pair.VolumeToFdv1HValue ?? 0f;

//             // Example: read short-term tweet counts or total tweets
//             int tweetsLastHour = pair.TweetsCATweetCount1H;
//             int tweetsLast24H = pair.TweetsCATweetCount24H;
//             bool hasSomeEngagement = pair.TwitterCAFound || (tweetsLastHour > 0);

//             // 3) Attempt a rudimentary correlation approach
//             // For example, “If VLR is high AND we see high tweet engagement, 
//             // we might assume a short-term hype correlation.” 
//             // This is just a simplified example; adapt to your own logic.
//             float correlationScore = 0f;

//             // If we have decent tweet volume last hour AND VLR > 5, 
//             // treat that as a high speculation synergy
//             if (tweetsLastHour > 20 && vlr > 5)
//             {
//                 correlationScore += 3f;
//             }
//             if (bSRatio > 2.0 && tweetsLastHour > 10)
//             {
//                 correlationScore += 2f;
//             }
//             if (volToFdv > 0.01 && tweetsLastHour > 5)
//             {
//                 correlationScore += 2f;
//             }

//             // We can also factor in the pair.CombinedMetricScore if we want to unify them
//             float baseCombinedScore = pair.CombinedMetricScore ?? 5f;  // default to mid
//             correlationScore += baseCombinedScore;  // merge them

//             // 4) Build a text analysis from these correlation-based signals
//             //    (In real code, you might have a range-based approach, or call out to 
//             //    a separate scenario builder. For brevity, we do some if-else logic.)
//             string correlationAnalysis = "";
//             if (!hasSomeEngagement)
//             {
//                 correlationAnalysis = "No Twitter engagement found; limited correlation with volume/price metrics.";
//             }
//             else
//             {
//                 correlationAnalysis = "Found meaningful Twitter engagement—possible synergy with strong volume metrics.";
//                 if (correlationScore > 12f)
//                 {
//                     correlationAnalysis += " High correlation suggests short-term hype or potential pump scenario.";
//                 }
//                 else if (correlationScore > 8f)
//                 {
//                     correlationAnalysis += " Moderate correlation suggests healthy interest but not mania.";
//                 }
//                 else
//                 {
//                     correlationAnalysis += " Low correlation indicates hype isn't fully translating into trading momentum.";
//                 }
//             }

//             // 5) Final summary & prediction logic
//             //    For instance, combine your existing CombinedQualitativeAnalysis with the new correlation.
//             var finalSummary = "🌵 " + (pair.CombinedQualitativeAnalysis ?? "No base analysis")
//                 + " | Engagement correlation: "
//                 + correlationAnalysis;

//             var finalPrediction = "Short-term outlook: ";
//             if (correlationScore > 15)
//                 finalPrediction += "Expect rapid swings, watch for a potential pump and quick profit-taking.";
//             else if (correlationScore > 10)
//                 finalPrediction += "Likely mild upward momentum if the hype sustains. Keep an eye on sudden sells.";
//             else
//                 finalPrediction += "Market seems uncertain. Could fizzle unless new buyers step in.";

//             // 6) Store them
//             pair.EngagementCorrelationAnalysis = correlationAnalysis;
//             pair.EngagementCorrelationScore = correlationScore;
//             pair.PlantFinalSummary = finalSummary;
//             pair.PlantFinalPrediction = finalPrediction;
//         }


//         private AggregatedPoolMetrics AggregateAllPools(IEnumerable<CoingeckoPoolData> allPools)
//         {
//             var metrics = new AggregatedPoolMetrics();

//             int poolCount = 0;

//             // For weighted price
//             double totalVolumeForWeighting = 0.0;
//             double priceVolumeAccumulator = 0.0;

//             // For FDV and MarketCap, we might choose "max" or "first non-null." 
//             // We'll track if we’ve set it at least once:
//             bool fdvSet = false;
//             bool marketCapSet = false;

//             foreach (var poolData in allPools)
//             {
//                 var attr = poolData.Attributes;
//                 if (attr == null) continue;

//                 poolCount++;

//                 // ------------------------------------------------------------------
//                 // 1) Liquidity (ReserveInUsd)
//                 // ------------------------------------------------------------------
//                 if (float.TryParse(attr.ReserveInUsd, out var reserveUsd))
//                 {
//                     metrics.TotalLiquidityUsd += reserveUsd;
//                 }

//                 // ------------------------------------------------------------------
//                 // 2) Volume (we sum across all pools)
//                 // ------------------------------------------------------------------
//                 if (attr.VolumeUsd != null)
//                 {
//                     // M5
//                     if (float.TryParse(attr.VolumeUsd.M5, out var volM5))
//                         metrics.VolumeM5 += volM5;
//                     // H1
//                     if (float.TryParse(attr.VolumeUsd.H1, out var volH1))
//                         metrics.VolumeH1 += volH1;
//                     // H6
//                     if (float.TryParse(attr.VolumeUsd.H6, out var volH6))
//                         metrics.VolumeH6 += volH6;
//                     // H24
//                     if (float.TryParse(attr.VolumeUsd.H24, out var volH24))
//                         metrics.VolumeH24 += volH24;
//                 }

//                 // ------------------------------------------------------------------
//                 // 3) Transactions
//                 // ------------------------------------------------------------------
//                 if (attr.Transactions != null)
//                 {
//                     // M5
//                     metrics.M5Buys += attr.Transactions.M5?.Buys ?? 0;
//                     metrics.M5Sells += attr.Transactions.M5?.Sells ?? 0;

//                     // M15
//                     metrics.M15Buys += attr.Transactions.M15?.Buys ?? 0;
//                     metrics.M15Sells += attr.Transactions.M15?.Sells ?? 0;

//                     // M30
//                     metrics.M30Buys += attr.Transactions.M30?.Buys ?? 0;
//                     metrics.M30Sells += attr.Transactions.M30?.Sells ?? 0;

//                     // H1
//                     metrics.H1Buys += attr.Transactions.H1?.Buys ?? 0;
//                     metrics.H1Sells += attr.Transactions.H1?.Sells ?? 0;

//                     // H24
//                     metrics.H24Buys += attr.Transactions.H24?.Buys ?? 0;
//                     metrics.H24Sells += attr.Transactions.H24?.Sells ?? 0;
//                 }

//                 // ------------------------------------------------------------------
//                 // 4) FDV & MarketCap
//                 //    (Storing the maximum encountered as an example.)
//                 // ------------------------------------------------------------------
//                 if (float.TryParse(attr.FdvUsd, out var fdvVal))
//                 {
//                     if (!fdvSet || fdvVal > metrics.FdvUsd)
//                     {
//                         metrics.FdvUsd = fdvVal;
//                         fdvSet = true;
//                     }
//                 }

//                 if (float.TryParse(attr.MarketCapUsd, out var mcapVal))
//                 {
//                     if (!marketCapSet || mcapVal > metrics.MarketCapUsd)
//                     {
//                         metrics.MarketCapUsd = mcapVal;
//                         marketCapSet = true;
//                     }
//                 }

//                 // ------------------------------------------------------------------
//                 // 5) Price Changes (we’ll sum them and average at the end)
//                 // ------------------------------------------------------------------
//                 if (attr.PriceChangePercentage != null)
//                 {
//                     metrics.PriceChangeM5 += ParseFloat(attr.PriceChangePercentage.M5);
//                     metrics.PriceChangeH1 += ParseFloat(attr.PriceChangePercentage.H1);
//                     metrics.PriceChangeH6 += ParseFloat(attr.PriceChangePercentage.H6);
//                     metrics.PriceChangeH24 += ParseFloat(attr.PriceChangePercentage.H24);
//                 }

//                 // ------------------------------------------------------------------
//                 // 6) Weighted Avg Price 
//                 //    (We'll use H1 volume to weight this, or choose another metric.)
//                 // ------------------------------------------------------------------
//                 if (float.TryParse(attr.BaseTokenPriceUsd, out var basePriceUsd) &&
//                     float.TryParse(attr.VolumeUsd?.H1, out var volH1ForWeighting))
//                 {
//                     priceVolumeAccumulator += (basePriceUsd * volH1ForWeighting);
//                     totalVolumeForWeighting += volH1ForWeighting;
//                 }
//             }

//             // ------------------------------------------------------------------
//             // Now finalize the average calculations (price changes, weighted price)
//             // ------------------------------------------------------------------
//             if (poolCount > 0)
//             {
//                 // Price change average
//                 metrics.PriceChangeM5 /= poolCount;
//                 metrics.PriceChangeH1 /= poolCount;
//                 metrics.PriceChangeH6 /= poolCount;
//                 metrics.PriceChangeH24 /= poolCount;
//             }

//             // Weighted price
//             if (priceVolumeAccumulator > 0 && totalVolumeForWeighting > 0)
//             {
//                 metrics.WeightedAvgPriceUsd = (float)(priceVolumeAccumulator / totalVolumeForWeighting);
//             }

//             return metrics;
//         }

//         private float ParseFloat(string str)
//         {
//             return float.TryParse(str, out var val) ? val : 0f;
//         }


//         private string SumNullableFloats(string currentStr, string addStr)
//         {
//             var currentVal = 0f;
//             var addVal = 0f;

//             float.TryParse(currentStr, out currentVal);
//             float.TryParse(addStr, out addVal);
//             var sum = currentVal + addVal;
//             return sum == 0 ? null : sum.ToString();
//         }

//         private string DivideFloat(string valueStr, int divisor)
//         {
//             if (string.IsNullOrWhiteSpace(valueStr)) return null;
//             if (!float.TryParse(valueStr, out var val)) return null;
//             var result = val / divisor;
//             return result.ToString("F2");
//         }


//     }



// }
