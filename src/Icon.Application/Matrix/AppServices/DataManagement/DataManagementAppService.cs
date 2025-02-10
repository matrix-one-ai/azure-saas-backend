using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Authorization;
using System;
using Microsoft.AspNetCore.Mvc;
using Icon.EntityFrameworkCore.Matrix;
using Icon.Matrix.Models;
using Microsoft.Extensions.Configuration;
using Icon.Configuration;
using Azure.Data.Tables;
using Icon.Matrix.Twitter;
using Icon.Matrix.TokenPools;
using Icon.Matrix.TwitterManager;
using Tweetinvi.Models.V2;
using Icon.Matrix.TokenDiscovery;
using Abp.Domain.Uow;

namespace Icon.Matrix
{

    // Trigger imports 
    // New memories will result in processing steps
    // Trigger processing steps in batches
    // Trigger processing steps for all characters
    // Processing done will result in ranking calculations

    public interface IDataManagementAppService
    {
        // Task ProcessCharacterTwitterImports();
        // Task ProcessMemoryProcessingSteps(int batchSize = 10);
        // Task ProcessCharacterPostTweet();

        //Task ProcessCharacterPersonaRanking();
        // Task SyncAzureCharacterTweets(int batchSize = 100);
        // Task SyncAzureCharacterPersonaTweets(int batchSize = 100);
        // Task SyncAzureCharacterMentionedTweets(int batchSize = 100);
        // Task SyncAzureResetTweetsAsync();
        // Task SyncAzureResetTwitterCharacterPersonas(Guid characterId, bool areYouSure = false);
    }

    //[AbpAuthorize]
    [ApiKeyAuth]
    [AbpAllowAnonymous]
    public partial class DataManagementAppService : IconAppServiceBase, IDataManagementAppService
    {
        private IConfigurationRoot _configuration;
        private IMemoryManager _memoryManager;
        private IPlatformManager _platformManager;
        private ICharacterManager _characterManager;
        private ITwitterTaskManager _twitterManager;
        private ITokenPoolManager _tokenPoolManager;
        private ITokenDiscoveryManager _tokenDiscoveryManager;
        private ITwitterAPICommunicationService _twitterAPICommunicationService;
        private readonly ISharedSqlRepository<CharacterPersonaTwitterRank> _cpTwitterRankSqlRepository;

        public DataManagementAppService(
            IAppConfigurationAccessor appConfigurationAccessor,

            IMemoryManager memoryManager,
            IPlatformManager platformManager,
            ICharacterManager characterManager,
            ITwitterTaskManager twitterManager,
            ITokenPoolManager tokenPoolManager,
            ITokenDiscoveryManager tokenDiscoveryManager,
            ITwitterAPICommunicationService twitterAPICommunicationService,

            ISharedSqlRepository<CharacterPersonaTwitterRank> cpTwitterRankSqlRepository)
        {
            _configuration = appConfigurationAccessor.Configuration;

            _memoryManager = memoryManager;
            _platformManager = platformManager;
            _characterManager = characterManager;
            _twitterManager = twitterManager;
            _tokenPoolManager = tokenPoolManager;
            _tokenDiscoveryManager = tokenDiscoveryManager;
            _twitterAPICommunicationService = twitterAPICommunicationService;

            _cpTwitterRankSqlRepository = cpTwitterRankSqlRepository;
        }

        public async Task<RaydiumPair> GetLatestRaydiumPair()
        {

            return await _tokenPoolManager.GetLatestRaydiumPair();
        }

        [HttpPost]
        //[UnitOfWork(IsDisabled = true)]
        public async Task ProcessTokenDiscovery()
        {
            await Task.CompletedTask;

            //await _tokenDiscoveryManager.FullDiscovery();
        }

        public async Task<List<RaydiumPair>> GetLatestRaydiumTweets()
        {
            return await _tokenPoolManager.GetLatestRaydiumpairs();
        }


        [HttpPost]
        public async Task<TwitterApiEngagementResponse> GetTwitterEngagementData()
        {
            await Task.CompletedTask;
            return null;

            // var searchQuery = "6p6xgHyF7AeE6TZkSmFsko444wqoP15icUSqi2jfGiPN";
            // var result = await _twitterAPICommunicationService.GetEngagementDataTotalAsync(searchQuery);
            // return result;
        }

        [HttpPost]
        public async Task<TwitterApiPostCountsResponse> GetTwitterTweetCount()
        {
            // var searchQuery = "9tNUwsQzBGWSUanjrdc2xtK369vPojPAxe3qjVrYpump";
            // //var searchQuery = "$TRUMP";
            // var result = await _twitterAPICommunicationService.GetTweetCountsAsync(searchQuery);
            // return result;

            await Task.CompletedTask;
            return null;
        }

        [HttpPost]
        public async Task<List<TweetV2>> GetTwitterTweetsInTimeRange()
        {
            // var searchQuery = "6p6xgHyF7AeE6TZkSmFsko444wqoP15icUSqi2jfGiPN";
            // // DateTime startDate = DateTime.Parse("2025-01-17T23:00:00+00:00Z");
            // // DateTime endDate = DateTime.Parse("2025-01-18T00:00:00+00:00Z");

            // // DateTime startDate = DateTime.Parse("2025-01-17T00:00:00Z"); // 'Z' ensures UTC
            // // DateTime endDate = DateTime.Parse("2025-01-17T23:59:59Z");

            // DateTimeOffset startDTO = DateTimeOffset.Parse("2025-01-17T23:00:00Z");
            // DateTimeOffset endDTO = DateTimeOffset.Parse("2025-01-17T23:59:59Z");

            // var result = await _twitterAPICommunicationService.GetTweetsByKeywordInTimeRangeAsync(searchQuery, startDTO.UtcDateTime, endDTO.UtcDateTime, 100);
            // return result;

            await Task.CompletedTask;
            return null;
        }

        // [HttpPost]
        // public async Task TriggerPostCharacterTweetRisingSproutAsync()
        // {
        //     await _twitterManager.TriggerPostCharacterTweetRisingSproutAsync();
        // }

        // [HttpPost]
        // public async Task<List<CoingeckoPoolUpdate>> TempGetPerformingPools()
        // {
        //     var findPoolInput = new TokenPoolGetBestPerformingInput
        //     {
        //         CreatedAfter = DateTime.UtcNow.AddMinutes(-60),
        //         CreatedBefore = DateTime.UtcNow,
        //         MinFdvUsd = 50000,
        //         MinLiquidtyUsd = 50000,
        //         MinVolumeH1 = 5000,
        //         MinRisePercentageSinceCreation = 10f,

        //         MaxPriceUpdateAgeMinutes = 60,
        //         MaxPools = 1,
        //         ExcludeTweetedPools = true,
        //         PerformPoolUpdate = true,
        //     };

        //     return null;//await _tokenPoolManager.FindPerformingPools(findPoolInput);
        // }


        [HttpPost]
        public async Task ProcessCharacterTweetImports()
        {
            await _twitterManager.ProcessCharacterTweetImports();
        }

        [HttpPost]
        public async Task ProcessCharacterTweetsStorage(RunMethodTypeInput input)
        {
            await _twitterManager.ProcessCharacterTweetsStorage(input.Type);
        }

        [HttpPost]
        public async Task ProcessMissingTwitterProfiles()
        {
            await _twitterManager.ProcessMissingTwitterProfiles();
        }


        [HttpPost]
        // STEP 02
        public async Task ProcessMemoryProcessingSteps(int batchSize = 10)
        {
            //AbpSession.Use(2, 3);
            await _memoryManager.RunPendingProcesses(batchSize);
        }


        [HttpPost]
        // STEP 03
        public async Task ProcessCharacterPostTweet()
        {
            //AbpSession.Use(2, 3);
            await _twitterManager.ProcessCharacterPostTweets();
        }

        [HttpPost]
        public async Task ProcessCharacterPersonaRanking()
        {
            var sql = @"
TRUNCATE TABLE CharacterPersonaTwitterRanks;

WITH PersonaScores AS (
    SELECT
        c.Id AS CharacterId,
        c.Name AS CharacterName,
        cp.Id AS CharacterPersonaId,
        p.Id AS PersonaId,
        p.Name AS PersonaName,

        SUM(mst.Likes) AS TotalLikes,
        SUM(mst.Replies) AS TotalReplies,
        SUM(mst.Retweets) AS TotalRetweets,
        SUM(mst.Views) AS TotalViews,
        SUM(mst.MentionsCount) AS TotalMentionsCount,
        SUM(mst.TweetWordCount) AS TotalWordCount,

        -- Mention Score        
        COUNT(m.Id) AS TotalMentions,
        COUNT(m.Id) * 10 AS TotalMentionsScore,

        -- Engagement Score
        SUM(mst.Likes * 0.5) + 
        SUM(mst.Replies * 2) + 
        SUM(mst.Retweets * 1) + 
        SUM(mst.Views * 0.05) AS TotalEngagementScore,

        -- Quality Score
        SUM(mst.RelevanceScore) AS TotalRelevanceScore,
        SUM(mst.DepthScore) AS TotalDepthScore,
        SUM(mst.NoveltyScore) AS TotalNoveltyScore,
        SUM(mst.SentimentScore) AS TotalSentimentScore,
        SUM(mst.RelevanceScore + mst.DepthScore + mst.NoveltyScore + mst.SentimentScore) AS TotalQualityScore,

        -- Total Scores
        SUM(
            (1 * 2) +
            (mst.Likes * 0.5) + 
            (mst.Replies * 2) + 
            (mst.Retweets * 1) + 
            (mst.Views * 0.05) +
            (mst.RelevanceScore + mst.DepthScore + mst.NoveltyScore + mst.SentimentScore)
        ) AS TotalScore,

        ROUND(SUM(
            (
                (1 * 10) +
                (mst.Likes * 0.5) + 
                (mst.Replies * 2) + 
                (mst.Retweets * 1) + 
                (mst.Views * 0.05) +
                (mst.RelevanceScore + mst.DepthScore + mst.NoveltyScore + mst.SentimentScore)
            ) * EXP(-0.03 * DATEDIFF(DAY, m.PlatformInteractionDate, GETDATE()))
        ), 0) AS TotalScoreTimeDecayed
    FROM Memories m
    INNER JOIN MemoryStatsTwitters mst
        ON m.Id = mst.MemoryId
    INNER JOIN Characters c
        ON m.CharacterId = c.Id
    INNER JOIN MemoryTypes mt
        ON m.MemoryTypeId = mt.Id
    INNER JOIN CharacterPersonas cp
        ON m.CharacterPersonaId = cp.Id
    INNER JOIN Personas p
        ON cp.PersonaId = p.Id
    WHERE mt.Name = 'CharacterMentionedTweet'
    AND cp.TwitterBlockInRanking = 0 
    GROUP BY c.Id, c.Name, cp.Id, p.Id, p.Name
)

INSERT INTO CharacterPersonaTwitterRanks
(
    Id,
    TenantId,
    CharacterPersonaId,    
    TotalLikes,
    TotalReplies,
    TotalRetweets,
    TotalViews,
    TotalMentionsCount,
    TotalWordCount,
    TotalMentions,
    TotalMentionsScore,
    TotalEngagementScore,
    TotalRelevanceScore,
    TotalDepthScore,
    TotalNoveltyScore,
    TotalSentimentScore,
    TotalQualityScore,
    TotalScore,
    TotalScoreTimeDecayed,
    Rank
)
SELECT 
    NEWID(),
    2 AS TenantId,
    CharacterPersonaId,
    TotalLikes,
    TotalReplies,
    TotalRetweets,
    TotalViews,
    TotalMentionsCount,
    TotalWordCount,
    TotalMentions,
    TotalMentionsScore,
    TotalEngagementScore,
    TotalRelevanceScore,
    TotalDepthScore,
    TotalNoveltyScore,
    TotalSentimentScore,
    TotalQualityScore,
    TotalScore,
    TotalScoreTimeDecayed,
    ROW_NUMBER() OVER (PARTITION BY CharacterId ORDER BY TotalScoreTimeDecayed DESC) AS Rank
FROM PersonaScores
ORDER BY CharacterId, Rank;
            ";

            await _cpTwitterRankSqlRepository.ExecuteSqlRawAsync(sql);
        }

        [HttpPost]
        public async Task ProcessInfluencerScoring()
        {

            await Task.CompletedTask;
            //             var sql = @"
            // TRUNCATE TABLE dbo.InfluencerTokenMentions
            // TRUNCATE TABLE dbo.InfluencerTokenScores

            // ;WITH FirstTweetPerAuthorPair AS
            // (
            //     SELECT
            //         t.Id,
            //         t.AuthorId,
            //         t.RaydiumPairId,
            //         t.CreatedAt,
            //         ROW_NUMBER() OVER (
            //             PARTITION BY t.AuthorId, t.RaydiumPairId
            //             ORDER BY t.CreatedAt ASC
            //         ) AS RN
            //     FROM dbo.TwitterImportTweetEngagements t
            // ),
            // TweetPriceWindows AS
            // (
            //     SELECT
            //         ft.Id AS TweetId,
            //         ft.AuthorId AS Influencer,
            //         ft.RaydiumPairId,
            //         ft.CreatedAt AS TweetTime,

            //         -- Pre-price (price just before the tweet)
            //         COALESCE(preAgg.WeightedAvgPriceUsd, 0) AS PrePrice,
            //         COALESCE(preAggBackup.WeightedAvgPriceUsd, 0) AS PrePriceBackup,

            //         -- Snapshots at specific future times
            //         COALESCE(p5.WeightedAvgPriceUsd, 0)     AS Price5m,
            //         COALESCE(p15.WeightedAvgPriceUsd, 0)    AS Price15m,
            //         COALESCE(p30.WeightedAvgPriceUsd, 0)    AS Price30m,
            //         COALESCE(p60.WeightedAvgPriceUsd, 0)    AS Price1h,
            //         COALESCE(p120.WeightedAvgPriceUsd, 0)   AS Price2h
            //     FROM FirstTweetPerAuthorPair ft
            //     INNER JOIN dbo.TwitterImportTweetEngagements t 
            //         ON t.Id = ft.Id

            //     OUTER APPLY
            //     (
            //         SELECT TOP 1 c.*
            //         FROM dbo.CoingeckoAggregatedUpdates c
            //         WHERE c.RaydiumPairId = ft.RaydiumPairId
            //           AND c.CreationTime <= ft.CreatedAt
            //         ORDER BY c.CreationTime DESC
            //     ) preAgg

            //     -- Fallback preprice: earliest price AFTER tweet time, up to 10 minutes
            //     OUTER APPLY
            //     (
            //         SELECT TOP 1 c.*
            //         FROM dbo.CoingeckoAggregatedUpdates c
            //         WHERE c.RaydiumPairId = ft.RaydiumPairId
            //           AND c.CreationTime >= ft.CreatedAt
            //           AND c.CreationTime <= DATEADD(MINUTE, 10, ft.CreatedAt)
            //         ORDER BY c.CreationTime ASC
            //     ) preAggBackup

            //     OUTER APPLY
            //     (
            //         SELECT TOP 1 c.*
            //         FROM dbo.CoingeckoAggregatedUpdates c
            //         WHERE c.RaydiumPairId = ft.RaydiumPairId
            //           AND c.CreationTime >= DATEADD(MINUTE, 5, ft.CreatedAt)
            //         ORDER BY c.CreationTime ASC
            //     ) p5

            //     OUTER APPLY
            //     (
            //         SELECT TOP 1 c.*
            //         FROM dbo.CoingeckoAggregatedUpdates c
            //         WHERE c.RaydiumPairId = ft.RaydiumPairId
            //           AND c.CreationTime >= DATEADD(MINUTE, 15, ft.CreatedAt)
            //         ORDER BY c.CreationTime ASC
            //     ) p15

            //     OUTER APPLY
            //     (
            //         SELECT TOP 1 c.*
            //         FROM dbo.CoingeckoAggregatedUpdates c
            //         WHERE c.RaydiumPairId = ft.RaydiumPairId
            //           AND c.CreationTime >= DATEADD(MINUTE, 30, ft.CreatedAt)
            //         ORDER BY c.CreationTime ASC
            //     ) p30

            //     OUTER APPLY
            //     (
            //         SELECT TOP 1 c.*
            //         FROM dbo.CoingeckoAggregatedUpdates c
            //         WHERE c.RaydiumPairId = ft.RaydiumPairId
            //           AND c.CreationTime >= DATEADD(MINUTE, 60, ft.CreatedAt)
            //         ORDER BY c.CreationTime ASC
            //     ) p60

            //     OUTER APPLY
            //     (
            //         SELECT TOP 1 c.*
            //         FROM dbo.CoingeckoAggregatedUpdates c
            //         WHERE c.RaydiumPairId = ft.RaydiumPairId
            //           AND c.CreationTime >= DATEADD(MINUTE, 120, ft.CreatedAt)
            //         ORDER BY c.CreationTime ASC
            //     ) p120

            //     WHERE ft.RN = 1
            // ),
            // PumpCalculations AS
            // (
            //     SELECT
            //         tw.TweetId,
            //         tw.Influencer,
            //         tw.RaydiumPairId,
            //         tw.TweetTime,
            //         tw.PrePrice,
            //         tw.PrePriceBackup,

            //         tw.Price5m,
            //         tw.Price15m,
            //         tw.Price30m,
            //         tw.Price1h,
            //         tw.Price2h,

            //         -- Pump calculations that first attempt to use PrePrice; if 0 (or NULL), fallback to PrePriceBackup.
            //         CASE 
            //             WHEN tw.PrePrice IS NOT NULL 
            //                  AND tw.PrePrice <> 0
            //             THEN (tw.Price5m - tw.PrePrice) / tw.PrePrice * 100
            //             WHEN tw.PrePriceBackup IS NOT NULL
            //                  AND tw.PrePriceBackup <> 0
            //             THEN (tw.Price5m - tw.PrePriceBackup) / tw.PrePriceBackup * 100
            //             ELSE NULL
            //         END AS Pump5m,

            //         CASE 
            //             WHEN tw.PrePrice IS NOT NULL 
            //                  AND tw.PrePrice <> 0
            //             THEN (tw.Price15m - tw.PrePrice) / tw.PrePrice * 100
            //             WHEN tw.PrePriceBackup IS NOT NULL
            //                  AND tw.PrePriceBackup <> 0
            //             THEN (tw.Price15m - tw.PrePriceBackup) / tw.PrePriceBackup * 100
            //             ELSE NULL
            //         END AS Pump15m,

            //         CASE 
            //             WHEN tw.PrePrice IS NOT NULL 
            //                  AND tw.PrePrice <> 0
            //             THEN (tw.Price30m - tw.PrePrice) / tw.PrePrice * 100
            //             WHEN tw.PrePriceBackup IS NOT NULL
            //                  AND tw.PrePriceBackup <> 0
            //             THEN (tw.Price30m - tw.PrePriceBackup) / tw.PrePriceBackup * 100
            //             ELSE NULL
            //         END AS Pump30m,

            //         CASE 
            //             WHEN tw.PrePrice IS NOT NULL 
            //                  AND tw.PrePrice <> 0
            //             THEN (tw.Price1h - tw.PrePrice) / tw.PrePrice * 100
            //             WHEN tw.PrePriceBackup IS NOT NULL
            //                  AND tw.PrePriceBackup <> 0
            //             THEN (tw.Price1h - tw.PrePriceBackup) / tw.PrePriceBackup * 100
            //             ELSE NULL
            //         END AS Pump1h,

            //         CASE 
            //             WHEN tw.PrePrice IS NOT NULL 
            //                  AND tw.PrePrice <> 0
            //             THEN (tw.Price2h - tw.PrePrice) / tw.PrePrice * 100
            //             WHEN tw.PrePriceBackup IS NOT NULL
            //                  AND tw.PrePriceBackup <> 0
            //             THEN (tw.Price2h - tw.PrePriceBackup) / tw.PrePriceBackup * 100
            //             ELSE NULL
            //         END AS Pump2h
            //     FROM TweetPriceWindows tw
            // )
            // INSERT INTO dbo.InfluencerTokenMentions
            // (
            //     Id,
            //     Influencer,
            //     RaydiumPairId,
            //     TweetCount,
            //     AvgPump5m,
            //     AvgPump15m,
            //     AvgPump30m,
            //     AvgPump1h,
            //     AvgPump2h,
            //     SuccessRate5m,
            //     SuccessRate15m,
            //     SuccessRate30m,
            //     SuccessRate1h,
            //     SuccessRate2h,
            //     DeathCount,
            //     AliveCount
            // )
            // SELECT
            //     NEWID()                                  AS Id,
            //     pc.Influencer,
            //     pc.RaydiumPairId,

            //     COUNT(*)                                 AS TweetCount,

            //     AVG(pc.Pump5m)                           AS AvgPump5m,
            //     AVG(pc.Pump15m)                          AS AvgPump15m,
            //     AVG(pc.Pump30m)                          AS AvgPump30m,
            //     AVG(pc.Pump1h)                           AS AvgPump1h,
            //     AVG(pc.Pump2h)                           AS AvgPump2h,

            //     -- success rates: fraction of tweets with pump > 0
            //     AVG(CASE WHEN pc.Pump5m  > 0 THEN 1.0 ELSE 0.0 END) AS SuccessRate5m,
            //     AVG(CASE WHEN pc.Pump15m > 0 THEN 1.0 ELSE 0.0 END) AS SuccessRate15m,
            //     AVG(CASE WHEN pc.Pump30m > 0 THEN 1.0 ELSE 0.0 END) AS SuccessRate30m,
            //     AVG(CASE WHEN pc.Pump1h  > 0 THEN 1.0 ELSE 0.0 END) AS SuccessRate1h,
            //     AVG(CASE WHEN pc.Pump2h  > 0 THEN 1.0 ELSE 0.0 END) AS SuccessRate2h,

            //     -- Each token is either death or alive. So for the entire group
            //     -- it is the same for all tweets of that token. We'll just check RaydiumPairs once:
            //     SUM(CASE WHEN rp.DiscoveryStageName = 'Death' THEN 1 ELSE 0 END) AS DeathCount,
            //     SUM(CASE WHEN rp.DiscoveryStageName = 'Death' THEN 0 ELSE 1 END) AS AliveCount

            // FROM PumpCalculations pc
            // JOIN dbo.RaydiumPairs rp 
            //    ON rp.Id = pc.RaydiumPairId
            // GROUP BY pc.Influencer, pc.RaydiumPairId, rp.DiscoveryStageName
            // ;


            // INSERT INTO dbo.InfluencerTokenScores
            // (
            //     Id,
            //     Influencer,
            //     TweetCount,
            //     UniqueTokensCount,
            //     AvgPump5m,
            //     AvgPump15m,
            //     AvgPump30m,
            //     AvgPump1h,
            //     AvgPump2h,
            //     SuccessRate5m,
            //     SuccessRate15m,
            //     SuccessRate30m,
            //     SuccessRate1h,
            //     SuccessRate2h,
            //     DeathCount,
            //     AliveCount,
            //     TotalScore,
            //     TimedWindowScore
            // )
            // SELECT 
            //     NEWID() AS Id,
            //     itm.Influencer,

            //     -- Summation of tweet counts across all tokens:
            //     SUM(itm.TweetCount) AS TweetCount,

            //     -- Number of distinct tokens for that influencer:
            //     COUNT(DISTINCT itm.RaydiumPairId) AS UniqueTokensCount,

            //     -- Example of simple average of the token-level averages:
            //     -- (Could also do a weighted average by the token's TweetCount if desired.)
            //     AVG(itm.AvgPump5m)  AS AvgPump5m,
            //     AVG(itm.AvgPump15m) AS AvgPump15m,
            //     AVG(itm.AvgPump30m) AS AvgPump30m,
            //     AVG(itm.AvgPump1h)  AS AvgPump1h,
            //     AVG(itm.AvgPump2h)  AS AvgPump2h,

            //     -- success rates: average of token-level success rates
            //     AVG(itm.SuccessRate5m)  AS SuccessRate5m,
            //     AVG(itm.SuccessRate15m) AS SuccessRate15m,
            //     AVG(itm.SuccessRate30m) AS SuccessRate30m,
            //     AVG(itm.SuccessRate1h)  AS SuccessRate1h,
            //     AVG(itm.SuccessRate2h)  AS SuccessRate2h,

            //     -- death/alive across all tokens
            //     SUM(itm.DeathCount) AS DeathCount,
            //     SUM(itm.AliveCount) AS AliveCount,

            //     ----------------------------------------------------------------------------
            //     -- Example scoring logic (mimicking original):
            //     ----------------------------------------------------------------------------
            //     CASE 
            //         WHEN COUNT(DISTINCT itm.RaydiumPairId) < 5 THEN 0
            //         ELSE 
            //          COUNT(DISTINCT itm.RaydiumPairId) * 
            //          (
            //            (
            // 		        AVG(itm.SuccessRate5m)  +
            // 		        AVG(itm.SuccessRate15m) +
            // 		        AVG(itm.SuccessRate30m) +
            //                 AVG(itm.SuccessRate1h)  +
            //                 AVG(itm.SuccessRate2h)
            //            ) / 5.0
            //          )
            //     END AS TotalScore,

            //     (
            //         -- Weighted timed window idea
            //         AVG(itm.SuccessRate5m)  * 30 +
            //         AVG(itm.SuccessRate15m) * 60 +
            //         AVG(itm.SuccessRate30m) * 90   +
            //         AVG(itm.SuccessRate1h)  * 60   +
            //         AVG(itm.SuccessRate2h)  * 30
            //     ) AS TimedWindowScore

            // FROM dbo.InfluencerTokenMentions itm
            // GROUP BY itm.Influencer
            // HAVING COUNT(DISTINCT itm.RaydiumPairId) >= 5
            // ORDER BY TimedWindowScore DESC;
            //             ";

            //             await _cpTwitterRankSqlRepository.ExecuteSqlRawAsync(sql);
        }


        [HttpPost]
        public async Task ProcessInfluencerBuyRecommendations()
        {
            await Task.CompletedTask;

            //             var sql = @"
            // --RESET
            // UPDATE RaydiumPairs
            // SET EngagementBuyRecommendation = 0
            // WHERE EngagementBuyRecommendation = 1;

            // --SET BUY REC
            // WITH FilteredInfluences AS
            // (
            //     SELECT
            //         r.RaydiumPairId,
            //         s.Influencer,
            //         s.TimedWindowScore,
            //         s.SuccessRate5m,
            //         s.SuccessRate15m,
            //         s.SuccessRate30m,
            //         s.SuccessRate1h,
            //         s.SuccessRate2h
            //     FROM
            //     (
            //         SELECT 
            //             rp.Id AS RaydiumPairId,
            //             te.AuthorId AS Influencer
            //         FROM RaydiumPairs rp
            //         INNER JOIN [dbo].[TwitterImportTweetEngagements] te
            //             ON rp.Id = te.RaydiumPairId
            //         WHERE te.CreatedAt > DATEADD(MINUTE, -30, GETDATE())
            //         GROUP BY rp.Id, te.AuthorId
            //     ) AS r
            //     JOIN dbo.InfluencerTokenScores s
            //         ON r.Influencer = s.Influencer
            // )
            // UPDATE rp
            // SET 
            //     -- Decide on the buy recommendation:
            //     rp.EngagementBuyRecommendation = CASE 
            //         WHEN f.TimedWindowScore >= 80 
            //              AND 
            //              (
            //                f.SuccessRate5m  > 0.6 OR
            //                f.SuccessRate15m > 0.6 OR
            //                f.SuccessRate30m > 0.6 OR
            //                f.SuccessRate1h  > 0.6 OR
            //                f.SuccessRate2h  > 0.6
            //              )
            //         THEN 1
            //         ELSE 0
            //     END,

            //     -- Decide on recommended sell time:
            //     rp.EngagementRecommendedSellByTime = CASE 
            //         WHEN f.SuccessRate5m = (SELECT MAX(sr) FROM (VALUES (f.SuccessRate5m),(f.SuccessRate15m),(f.SuccessRate30m),(f.SuccessRate1h),(f.SuccessRate2h)) AS X(sr))
            //              THEN DATEADD(MINUTE, 5, GETDATE())

            //         WHEN f.SuccessRate15m = (SELECT MAX(sr) FROM (VALUES (f.SuccessRate5m),(f.SuccessRate15m),(f.SuccessRate30m),(f.SuccessRate1h),(f.SuccessRate2h)) AS X(sr))
            //              THEN DATEADD(MINUTE, 15, GETDATE())

            //         WHEN f.SuccessRate30m = (SELECT MAX(sr) FROM (VALUES (f.SuccessRate5m),(f.SuccessRate15m),(f.SuccessRate30m),(f.SuccessRate1h),(f.SuccessRate2h)) AS X(sr))
            //              THEN DATEADD(MINUTE, 30, GETDATE())

            //         WHEN f.SuccessRate1h  = (SELECT MAX(sr) FROM (VALUES (f.SuccessRate5m),(f.SuccessRate15m),(f.SuccessRate30m),(f.SuccessRate1h),(f.SuccessRate2h)) AS X(sr))
            //              THEN DATEADD(MINUTE, 60, GETDATE())

            //         WHEN f.SuccessRate2h  = (SELECT MAX(sr) FROM (VALUES (f.SuccessRate5m),(f.SuccessRate15m),(f.SuccessRate30m),(f.SuccessRate1h),(f.SuccessRate2h)) AS X(sr))
            //              THEN DATEADD(MINUTE, 120, GETDATE())

            //         ELSE NULL
            //     END
            // FROM dbo.RaydiumPairs rp
            // JOIN FilteredInfluences f 
            // ON rp.Id = f.RaydiumPairId;

            // --BUY
            // UPDATE rp
            // SET 
            //     rp.DummyBuyTime     = GETUTCDATE(),
            //     rp.DummyBuyAmount   = 100,
            //     rp.DummyBuyPrice    = c.WeightedAvgPriceUsd,
            //     rp.DummyBuyQuantity = CASE 
            //                              WHEN c.WeightedAvgPriceUsd > 0 
            //                                  THEN 100 / c.WeightedAvgPriceUsd 
            //                                  ELSE 0 
            //                           END
            // FROM dbo.RaydiumPairs rp
            // JOIN dbo.CoingeckoAggregatedUpdates c
            //     ON rp.CoingeckoLastAggregatedUpdateId = c.Id
            // WHERE 
            //     rp.EngagementBuyRecommendation = 1
            //     AND rp.DummyBuyTime IS NULL
            //     AND c.WeightedAvgPriceUsd IS NOT NULL;

            // --SELL
            // UPDATE rp
            // SET 
            //     rp.DummySellTime     = GETUTCDATE(),
            //     rp.DummySellPrice    = c.WeightedAvgPriceUsd,
            //     rp.DummySellQuantity = rp.DummyBuyQuantity,
            //     rp.DummySellAmount   = CASE 
            //                               WHEN c.WeightedAvgPriceUsd > 0 
            //                                   THEN rp.DummyBuyQuantity * c.WeightedAvgPriceUsd 
            //                                   ELSE 0 
            //                            END
            // FROM dbo.RaydiumPairs rp
            // JOIN dbo.CoingeckoAggregatedUpdates c
            //     ON rp.CoingeckoLastAggregatedUpdateId = c.Id
            // WHERE 
            // 	rp.EngagementRecommendedSellByTime IS NOT NULL
            //     AND rp.EngagementRecommendedSellByTime < GETUTCDATE()
            //     AND rp.DummyBuyTime IS NOT NULL
            //     AND rp.DummySellTime IS NULL
            //     AND c.WeightedAvgPriceUsd IS NOT NULL;
            //         ";
            //             await _cpTwitterRankSqlRepository.ExecuteSqlRawAsync(sql);
        }

        // public async Task SyncAzureCharacterTweets(int batchSize = 100)
        // {
        //     AbpSession.Use(2, 3);

        //     var connectionString = _configuration["AzureStorage:ConnectionString"];
        //     TableClient tableClient = new TableClient(connectionString, "TwitterCharacterTweets");
        //     List<TableEntity> entities = new List<TableEntity>();

        //     await foreach (var entity in tableClient.QueryAsync<TableEntity>(filter: "Exported eq false", maxPerPage: batchSize))
        //     {
        //         entities.Add(entity);

        //         await _memoryManager.StoreCharacterTweets(
        //             characterId: new Guid(entity["CharacterId"].ToString()),
        //             conversationId: entity["ConversationId"].ToString(),
        //             tweetId: entity.GetString("RowKey"),
        //             tweetContent: entity["Text"].ToString(),
        //             tweetUrl: entity["PermanentUrl"].ToString(),
        //             platformInteractionDate: entity["TimeParsed"] as DateTimeOffset?
        //         );

        //         if (entities.Count >= batchSize)
        //             break;
        //     }

        //     foreach (var entity in entities)
        //     {
        //         entity["Exported"] = true;
        //         entity["ExportDate"] = DateTime.UtcNow;

        //         await tableClient.UpdateEntityAsync(entity, entity.ETag, TableUpdateMode.Replace);
        //     }
        // }

        // public async Task SyncAzureCharacterPersonaTweets(int batchSize = 100)
        // {
        //     AbpSession.Use(2, 3);

        //     var connectionString = _configuration["AzureStorage:ConnectionString"];
        //     TableClient tableClient = new TableClient(connectionString, "TwitterCharacterPersonaTweets");
        //     List<TableEntity> entities = new List<TableEntity>();

        //     await foreach (var entity in tableClient.QueryAsync<TableEntity>(filter: "Exported eq false", maxPerPage: batchSize))
        //     {
        //         entities.Add(entity);

        //         var username = entity["Username"].ToString();
        //         if (!username.StartsWith("@"))
        //             username = "@" + username;

        //         await _memoryManager.StoreCharacterPersonaTweets(
        //             characterId: new Guid(entity["CharacterId"].ToString()),
        //             personaPlatformId: username,
        //             personaName: entity["Name"].ToString(),
        //             conversationId: entity["ConversationId"].ToString(),
        //             tweetId: entity.GetString("RowKey"),
        //             tweetContent: entity["Text"].ToString(),
        //             tweetUrl: entity["PermanentUrl"].ToString(),
        //             platformInteractionDate: entity["TimeParsed"] as DateTimeOffset?
        //         );

        //         if (entities.Count >= batchSize)
        //             break;
        //     }

        //     foreach (var entity in entities)
        //     {
        //         entity["Exported"] = true;
        //         entity["ExportDate"] = DateTime.UtcNow;

        //         await tableClient.UpdateEntityAsync(entity, entity.ETag, TableUpdateMode.Replace);
        //     }
        // }

        // public async Task SyncAzureCharacterMentionedTweets(int batchSize = 100)
        // {
        //     AbpSession.Use(2, 3);

        //     var connectionString = _configuration["AzureStorage:ConnectionString"];
        //     TableClient tableClient = new TableClient(connectionString, "TwitterCharacterMentionedTweets");


        //     List<TableEntity> newEntities = new List<TableEntity>();
        //     await foreach (var entity in tableClient.QueryAsync<TableEntity>(filter: "Exported eq false", maxPerPage: batchSize))
        //     {
        //         newEntities.Add(entity);

        //         var username = entity["Username"].ToString();
        //         if (!username.StartsWith("@"))
        //             username = "@" + username;

        //         var memoryStatsTwitter = new MemoryStatsTwitter
        //         {
        //             TenantId = AbpSession.TenantId.Value,
        //             IsPin = entity["IsPin"] as bool? ?? false,
        //             IsQuoted = entity["IsQuoted"] as bool? ?? false,
        //             IsReply = entity["IsReply"] as bool? ?? false,
        //             IsRetweet = entity["IsRetweet"] as bool? ?? false,
        //             SensitiveContent = entity["SensitiveContent"] as bool? ?? false,
        //             BookmarkCount = entity["BookmarkCount"] as int? ?? 0,
        //             Likes = entity["Likes"] as int? ?? 0,
        //             Replies = entity["Replies"] as int? ?? 0,
        //             Retweets = entity["Retweets"] as int? ?? 0,
        //             Views = entity["Views"] as int? ?? 0,
        //             TweetWordCount = entity["Text"].ToString().Split(' ').Length,
        //             MentionsCount = entity["MentionsJson"].ToString().Split('}').Length - 1
        //         };

        //         await _memoryManager.StoreCharacterMentionedTweets(
        //             characterId: new Guid(entity["CharacterId"].ToString()),
        //             mentionByPersonaPlatformId: username,
        //             mentionedByPersonaName: entity["Name"].ToString(),
        //             conversationId: entity["ConversationId"].ToString(),
        //             tweetId: entity.GetString("RowKey"),
        //             tweetContent: entity["Text"].ToString(),
        //             tweetUrl: entity["PermanentUrl"].ToString(),
        //             platformInteractionDate: entity["TimeParsed"] as DateTimeOffset?,
        //             memoryStatsTwitter: memoryStatsTwitter
        //         );

        //         if (newEntities.Count >= batchSize)
        //             break;
        //     }

        //     foreach (var entity in newEntities)
        //     {
        //         entity["Exported"] = true;
        //         entity["ExportDate"] = DateTime.UtcNow;

        //         await tableClient.UpdateEntityAsync(entity, entity.ETag, TableUpdateMode.Replace);
        //     }

        //     List<TableEntity> updatedEntities = new List<TableEntity>();
        //     var filter = "(Exported eq true) and (LastTwitterImportExported eq false)";
        //     await foreach (var entity in tableClient.QueryAsync<TableEntity>(filter: filter, maxPerPage: batchSize))
        //     {
        //         updatedEntities.Add(entity);

        //         var username = entity["Username"].ToString();
        //         if (!username.StartsWith("@"))
        //         {
        //             username = "@" + username;
        //         }

        //         var memoryStatsTwitter = new MemoryStatsTwitter
        //         {
        //             TenantId = AbpSession.TenantId.Value,
        //             IsPin = entity["IsPin"] as bool? ?? false,
        //             IsQuoted = entity["IsQuoted"] as bool? ?? false,
        //             IsReply = entity["IsReply"] as bool? ?? false,
        //             IsRetweet = entity["IsRetweet"] as bool? ?? false,
        //             SensitiveContent = entity["SensitiveContent"] as bool? ?? false,
        //             BookmarkCount = entity["BookmarkCount"] as int? ?? 0,
        //             Likes = entity["Likes"] as int? ?? 0,
        //             Replies = entity["Replies"] as int? ?? 0,
        //             Retweets = entity["Retweets"] as int? ?? 0,
        //             Views = entity["Views"] as int? ?? 0,
        //             TweetWordCount = entity["Text"].ToString().Split(' ').Length,
        //             MentionsCount = entity["MentionsJson"].ToString().Split('}').Length - 1
        //         };

        //         await _memoryManager.StoreCharacterMentionedTweets(
        //             characterId: new Guid(entity["CharacterId"].ToString()),
        //             mentionByPersonaPlatformId: username,
        //             mentionedByPersonaName: entity["Name"].ToString(),
        //             conversationId: entity["ConversationId"].ToString(),
        //             tweetId: entity.GetString("RowKey"),
        //             tweetContent: entity["Text"].ToString(),
        //             tweetUrl: entity["PermanentUrl"].ToString(),
        //             platformInteractionDate: entity["TimeParsed"] as DateTimeOffset?,
        //             memoryStatsTwitter: memoryStatsTwitter
        //         );

        //         if (updatedEntities.Count >= batchSize)
        //             break;
        //     }

        //     foreach (var entity in updatedEntities)
        //     {
        //         entity["Exported"] = true;
        //         entity["ExportDate"] = DateTime.UtcNow;
        //         entity["LastTwitterImportExported"] = true;

        //         await tableClient.UpdateEntityAsync(entity, entity.ETag, TableUpdateMode.Replace);
        //     }
        // }

        // public async Task SyncAzureResetTweetsAsync()
        // {
        //     // reset character tweets
        //     var connectionString = _configuration["AzureStorage:ConnectionString"];
        //     TableClient tableClient = new TableClient(connectionString, "TwitterCharacterTweets");

        //     await foreach (var entity in tableClient.QueryAsync<TableEntity>(filter: "Exported eq true"))
        //     {
        //         entity["Exported"] = false;
        //         entity["ExportDate"] = null;

        //         await tableClient.UpdateEntityAsync(entity, entity.ETag, TableUpdateMode.Replace);
        //     }

        //     // // reset character persona tweets
        //     tableClient = new TableClient(connectionString, "TwitterCharacterPersonaTweets");
        //     await foreach (var entity in tableClient.QueryAsync<TableEntity>(filter: "Exported eq true"))
        //     {
        //         entity["Exported"] = false;
        //         entity["ExportDate"] = null;

        //         await tableClient.UpdateEntityAsync(entity, entity.ETag, TableUpdateMode.Replace);
        //     }

        //     // reset character mentioned tweets
        //     tableClient = new TableClient(connectionString, "TwitterCharacterMentionedTweets");
        //     await foreach (var entity in tableClient.QueryAsync<TableEntity>(filter: "Exported eq true"))
        //     {
        //         entity["Exported"] = false;
        //         entity["ExportDate"] = null;

        //         await tableClient.UpdateEntityAsync(entity, entity.ETag, TableUpdateMode.Replace);
        //     }
        // }

        // public async Task SyncAzureResetTwitterCharacterPersonas(Guid characterId, bool areYouSure = false)
        // {
        //     if (!areYouSure)
        //         return;

        //     var connectionString = _configuration["AzureStorage:ConnectionString"];
        //     TableClient tableClient = new TableClient(connectionString, "TwitterCharacterPersonas");
        //     await foreach (var entity in tableClient.QueryAsync<TableEntity>())
        //     {
        //         await tableClient.DeleteEntityAsync(entity.PartitionKey, entity.RowKey);
        //     }

        //     var twitterPlatformId = await _platformManager.GetPlatformId("Twitter");
        //     var twitterCharacterPersonas = await _characterManager.GetCharacterPlatformPersonas(characterId, twitterPlatformId);

        //     foreach (var persona in twitterCharacterPersonas)
        //     {
        //         await _characterManager.UpdateCharacterPersona(persona);
        //     }
        // }
    }
}