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

namespace Icon.Matrix
{

    // Trigger imports 
    // New memories will result in processing steps
    // Trigger processing steps in batches
    // Trigger processing steps for all characters
    // Processing done will result in ranking calculations

    public interface IDataManagementAppService
    {
        Task ProcessCharacterTwitterImports();
        Task ProcessMemoryProcessingSteps(int batchSize = 10);
        Task ProcessCharacterPostTweet();

        //Task ProcessCharacterPersonaRanking();
        // Task SyncAzureCharacterTweets(int batchSize = 100);
        // Task SyncAzureCharacterPersonaTweets(int batchSize = 100);
        // Task SyncAzureCharacterMentionedTweets(int batchSize = 100);
        // Task SyncAzureResetTweetsAsync();
        // Task SyncAzureResetTwitterCharacterPersonas(Guid characterId, bool areYouSure = false);
    }

    [AbpAuthorize]
    public partial class DataManagementAppService : IconAppServiceBase, IDataManagementAppService
    {
        private IConfigurationRoot _configuration;

        private IMemoryManager _memoryManager;
        private IPlatformManager _platformManager;
        private ICharacterManager _characterManager;
        private ITwitterTaskManager _twitterManager;
        private ITokenPoolManager _tokenPoolManager;

        private readonly ISharedSqlRepository<CharacterPersonaTwitterRank> _cpTwitterRankSqlRepository;

        public DataManagementAppService(
            IAppConfigurationAccessor appConfigurationAccessor,

            IMemoryManager memoryManager,
            IPlatformManager platformManager,
            ICharacterManager characterManager,
            ITwitterTaskManager twitterManager,
            ITokenPoolManager tokenPoolManager,

            ISharedSqlRepository<CharacterPersonaTwitterRank> cpTwitterRankSqlRepository)
        {
            _configuration = appConfigurationAccessor.Configuration;

            _memoryManager = memoryManager;
            _platformManager = platformManager;
            _characterManager = characterManager;
            _twitterManager = twitterManager;
            _tokenPoolManager = tokenPoolManager;

            _cpTwitterRankSqlRepository = cpTwitterRankSqlRepository;
        }

        [HttpPost]
        public async Task ProcessTokenPoolUpdates()
        {
            AbpSession.Use(2, 3);
            await _tokenPoolManager.ImportRaydiumPoolUpdates();
        }


        [HttpPost]
        // STEP 01
        public async Task ProcessCharacterTwitterImports()
        {
            AbpSession.Use(2, 3);

            await _twitterManager.ProcessCharacterTweetImports();
            await _twitterManager.ProcessCharacterTweetsStorage();
            await _twitterManager.ProcessMissingTwitterProfiles();
        }

        [HttpPost]
        // STEP 02
        public async Task ProcessMemoryProcessingSteps(int batchSize = 10)
        {
            AbpSession.Use(2, 3);
            await _memoryManager.RunPendingProcesses(batchSize);
            await ProcessCharacterPersonaRanking();
        }

        [HttpPost]
        // STEP 03
        public async Task ProcessCharacterPostTweet()
        {
            AbpSession.Use(2, 3);
            await _twitterManager.ProcessCharacterPostTweets();
        }

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
        SUM(mst.Likes * 0.02) + 
        SUM(mst.Replies * 1) + 
        SUM(mst.Retweets * 0.5) + 
        SUM(mst.Views * 0.003) AS TotalEngagementScore,

        -- Quality Score
        SUM(mst.RelevanceScore) AS TotalRelevanceScore,
        SUM(mst.DepthScore) AS TotalDepthScore,
        SUM(mst.NoveltyScore) AS TotalNoveltyScore,
        SUM(mst.SentimentScore) AS TotalSentimentScore,
        SUM(mst.RelevanceScore + mst.DepthScore + mst.NoveltyScore + mst.SentimentScore) AS TotalQualityScore,

        -- Total Scores
        SUM(
            (1 * 10) +
            (mst.Likes * 0.02) + 
            (mst.Replies * 1) + 
            (mst.Retweets * 0.5) + 
            (mst.Views * 0.003) +
            (mst.RelevanceScore + mst.DepthScore + mst.NoveltyScore + mst.SentimentScore)
        ) AS TotalScore,

        ROUND(SUM(
            (
                (1 * 10) +
                (mst.Likes * 0.02) + 
                (mst.Replies * 1) + 
                (mst.Retweets * 0.5) + 
                (mst.Views * 0.003) +
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