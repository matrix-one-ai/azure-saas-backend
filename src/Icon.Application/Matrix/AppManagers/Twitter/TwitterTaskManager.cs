using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.UI;
using Icon.Matrix.AIManager;
using Icon.Matrix.AIManager.CharacterPostTokenTweet;
using Icon.Matrix.Enums;
using Icon.Matrix.Models;
using Icon.Matrix.TokenDiscovery;
using Icon.Matrix.TokenPools;
using Icon.Matrix.TwitterManager;
using Microsoft.EntityFrameworkCore;
using Tweetinvi.Core.Parameters;

namespace Icon.Matrix.Twitter
{
    public interface ITwitterTaskManager
    {
        Task ProcessCharacterTweetImports();
        Task ProcessCharacterTweetsStorage(string type);
        Task ProcessCharacterPostTweets();
        Task ProcessMissingTwitterProfiles();
        Task TriggerPostCharacterTweetRisingSproutAsync();
    }

    public class TwitterTaskManager : ITwitterTaskManager, ITransientDependency
    {
        private readonly ITwitterCommunicationService _twitterCommunicationService;
        private readonly IRepository<TwitterImportTweet, Guid> _importTweetRepository;
        private readonly IRepository<TwitterImportTweetEngagement, Guid> _importTweetEngagementRepository;
        private readonly IRepository<Character, Guid> _characterRepository;
        private readonly IRepository<CharacterPersona, Guid> _characterPersonaRepository;
        private readonly IRepository<TwitterImportTask, Guid> _twitterImportTaskRepository;
        private readonly IRepository<TwitterImportLog, Guid> _twitterImportLogRepository;
        private readonly IMemoryManager _memoryManager;
        private readonly IAIManager _aiManager;
        private readonly ITokenPoolManager _tokenPoolManager;
        private readonly ITokenDiscoveryManager _tokenDiscoveryManager;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public TwitterTaskManager(
            ITwitterCommunicationService twitterCommunicationService,
            IRepository<TwitterImportTweet, Guid> importTweetRepository,
            IRepository<TwitterImportTweetEngagement, Guid> importTweetEngagementRepository,
            IRepository<Character, Guid> characterRepository,
            IRepository<CharacterPersona, Guid> characterPersonaRepository,
            IRepository<TwitterImportTask, Guid> twitterImportTaskRepository,
            IRepository<TwitterImportLog, Guid> twitterImportLogRepository,
            IMemoryManager memoryManager,
            IAIManager aiManager,
            ITokenPoolManager tokenPoolManager,
            ITokenDiscoveryManager tokenDiscoveryManager,
            IUnitOfWorkManager unitOfWorkManager)
        {
            _twitterCommunicationService = twitterCommunicationService;
            _importTweetRepository = importTweetRepository;
            _importTweetEngagementRepository = importTweetEngagementRepository;
            _characterRepository = characterRepository;
            _characterPersonaRepository = characterPersonaRepository;
            _twitterImportTaskRepository = twitterImportTaskRepository;
            _twitterImportLogRepository = twitterImportLogRepository;
            _memoryManager = memoryManager;
            _aiManager = aiManager;
            _tokenPoolManager = tokenPoolManager;
            _tokenDiscoveryManager = tokenDiscoveryManager;
            _unitOfWorkManager = unitOfWorkManager;
        }

        #region Public Methods (ITwitterTaskManager)

        public async Task ProcessCharacterTweetsStorage(string type)
        {
            switch (type)
            {
                case "ProcessNewCharacterTweets":
                    await ProcessNewCharacterTweets();
                    break;
                case "ProcessUpdatedCharacterMentionedTweets":
                    await ProcessUpdatedCharacterMentionedTweets();
                    break;
                case "ProcessNewCharacterMentionedTweets":
                    await ProcessNewCharacterMentionedTweets();
                    break;
                case "ProcessNewCharacterPersonaTweets":
                    await ProcessNewCharacterPersonaTweets();
                    break;
            }


            // await ProcessNewCharacterTweets();
            // await ProcessUpdatedCharacterMentionedTweets();
            // await ProcessNewCharacterMentionedTweets();
            // await ProcessNewCharacterPersonaTweets();
        }

        public async Task ProcessMissingTwitterProfiles()
        {
            var characters = await _characterRepository
                .GetAll()
                .Where(c => c.IsTwitterScrapingEnabled && !string.IsNullOrEmpty(c.TwitterScrapeAgentId))
                .ToListAsync();

            foreach (var character in characters)
            {
                var characterPersonas = await _characterPersonaRepository
                    .GetAll()
                    .Include(cp => cp.TwitterProfile)
                    .Include(cp => cp.Persona)
                        .ThenInclude(p => p.Platforms)
                            .ThenInclude(p => p.Platform)
                    .Where(cp => cp.CharacterId == character.Id &&
                                 (cp.TwitterProfile == null ||
                                  cp.TwitterProfile.LastImportDate < DateTime.UtcNow.AddDays(-10)))
                    .Take(50)
                    .ToListAsync();

                if (characterPersonas == null || !characterPersonas.Any()) continue;

                var imported = 0;
                var limit = new Random().Next(1, 10);
                var randomDelay = new Random().Next(300, 1500);

                foreach (var cpersona in characterPersonas)
                {
                    if (imported >= limit) break;
                    if (cpersona.Persona == null) continue;
                    if (cpersona.Persona.Platforms == null || !cpersona.Persona.Platforms.Any()) continue;

                    var twitterPlatform = cpersona.Persona.Platforms.FirstOrDefault(p => p.Platform.Name == "Twitter");
                    if (twitterPlatform == null) continue;

                    var userSearch = twitterPlatform.PlatformPersonaId.StartsWith("@")
                        ? twitterPlatform.PlatformPersonaId.Substring(1)
                        : twitterPlatform.PlatformPersonaId;

                    using (var uow = _unitOfWorkManager.Begin())
                    {
                        var twitterProfile = await TryGetUserProfileAsync(character, userSearch);

                        // If we got a profile, update it; if not, insert a minimal record
                        if (twitterProfile == null)
                        {
                            cpersona.TwitterProfile = new CharacterPersonaTwitterProfile
                            {
                                CharacterPersonaId = cpersona.Id,
                                LastImportDate = DateTime.UtcNow
                            };
                        }
                        else
                        {
                            await LogAsync(new TwitterImportLog
                            {
                                TenantId = character.TenantId,
                                CharacterId = character.Id,
                                TwitterAgentId = character.TwitterScrapeAgentId,
                                TaskName = "ProcessMissingTwitterProfiles",
                                Message = "Successfully fetched Twitter profile.",
                                LogLevel = "Information",
                                LoggedAt = DateTime.UtcNow
                            });

                            cpersona.TwitterProfile = new CharacterPersonaTwitterProfile
                            {
                                CharacterPersonaId = cpersona.Id,
                                Avatar = twitterProfile.Avatar,
                                Biography = twitterProfile.Biography,
                                FollowersCount = twitterProfile.FollowersCount,
                                FollowingCount = twitterProfile.FollowingCount,
                                FriendsCount = twitterProfile.FriendsCount,
                                MediaCount = twitterProfile.MediaCount,
                                IsPrivate = twitterProfile.IsPrivate,
                                IsVerified = twitterProfile.IsVerified,
                                LikesCount = twitterProfile.LikesCount,
                                ListedCount = twitterProfile.ListedCount,
                                Location = twitterProfile.Location,
                                Name = twitterProfile.Name,
                                PinnedTweetIds = twitterProfile.PinnedTweetIds,
                                TweetsCount = twitterProfile.TweetsCount,
                                Url = twitterProfile.Url,
                                UserId = twitterProfile.UserId,
                                Username = twitterProfile.Username,
                                IsBlueVerified = twitterProfile.IsBlueVerified,
                                CanDm = twitterProfile.CanDm,
                                Joined = twitterProfile.Joined,
                                LastImportDate = DateTime.UtcNow
                            };
                        }

                        imported++;
                        await Task.Delay(randomDelay);
                        await _characterPersonaRepository.UpdateAsync(cpersona);
                        await _unitOfWorkManager.Current.SaveChangesAsync();
                        await uow.CompleteAsync();
                    }
                }
            }
        }

        public async Task ProcessCharacterPostTweets()
        {
            var characters = await _characterRepository.GetAllListAsync();
            var postTypes = new List<string>
            {
                "PostCharacterTweetAsync",
                "PostCharacterPersonaWelcomeTweet",
                "PostCharacterTweetRisingSproutAsync"
            };

            // var postTypes = new List<string>
            // {
            //     "PostCharacterTweetRisingSproutAsync"
            // };

            foreach (var character in characters)
            {
                var tasks = await _twitterImportTaskRepository.GetAllListAsync(x =>
                    x.CharacterId == character.Id &&
                    x.IsEnabled &&
                    x.NextRunTime <= DateTime.UtcNow &&
                    postTypes.Contains(x.TaskName));

                if (tasks == null || !tasks.Any()) continue;

                foreach (var task in tasks)
                {
                    switch (task.TaskName)
                    {
                        case "PostCharacterTweetAsync":
                            await PostCharacterTweetAsync(task, character);
                            break;
                        case "PostCharacterPersonaWelcomeTweet":
                            await ProcessCharacterPersonaWelcomeTweet(task, character);
                            break;
                        case "PostCharacterTweetRisingSproutAsync":
                            await PostCharacterTweetRisingSproutAsync(task, character);
                            break;
                    }
                }

                // Attempt to also import tweets from the character if there’s a matching task
                var importTask = await _twitterImportTaskRepository
                    .GetAll()
                    .Where(x =>
                        x.CharacterId == character.Id &&
                        x.IsEnabled &&
                        x.TaskName == "ImportTweetsOfCharacterAsync")
                    .FirstOrDefaultAsync();

                if (importTask != null) await ImportTweetsOfCharacterAsync(importTask, character);
            }
        }

        public async Task ProcessCharacterTweetImports()
        {
            var characters = await _characterRepository.GetAllListAsync();

            foreach (var character in characters)
            {
                var tasks = await _twitterImportTaskRepository.GetAllListAsync(
                    x => x.CharacterId == character.Id &&
                         x.IsEnabled &&
                         x.NextRunTime <= DateTime.UtcNow);

                foreach (var task in tasks)
                {
                    switch (task.TaskName)
                    {
                        case "ImportTweetsOfCharacterAsync":
                            await ImportTweetsOfCharacterAsync(task, character);
                            break;
                        case "ImportMentionsOfCharacterWithAPIAsync":
                            await ImportMentionsOfCharacterWithAPIAsync(task, character);
                            break;
                        case "ImportMentionsOfCharacterAsync":
                            await ImportMentionsOfCharacterAsync(task, character);
                            break;
                        case "ImportTweetsOfCharacterPersonasAsync":
                            await ImportTweetsOfCharacterPersonasAsync(task, character);
                            break;
                    }
                }
            }
        }

        #endregion

        #region Private Methods (Tweet Storage)

        private async Task ProcessNewCharacterTweets()
        {
            var characterTweets = await _importTweetRepository
                .GetAll()
                .Where(x => !x.Exported && x.TweetType == "CharacterTweet")
                .Take(50)
                .ToListAsync();

            foreach (var tweet in characterTweets)
            {
                await _memoryManager.StoreCharacterTweets(
                    characterId: tweet.CharacterId,
                    conversationId: tweet.ConversationId,
                    tweetId: tweet.TweetId,
                    tweetContent: tweet.Text,
                    tweetUrl: tweet.PermanentUrl,
                    platformInteractionDate: tweet.TimeParsed
                );

                await UpdateImportTweetExportedStatus(tweet);
            }
        }

        private async Task ProcessNewCharacterMentionedTweets()
        {
            var characterMentionedTweetsNew = await _importTweetRepository
                .GetAll()
                .Where(x => !x.Exported && x.TweetType == "CharacterMentionedTweet")
                .Take(50)
                .ToListAsync();

            foreach (var tweet in characterMentionedTweetsNew)
            {
                using (var uow = _unitOfWorkManager.Begin())
                {
                    var memoryStatsTwitter = CreateMemoryStatsTwitter(tweet);
                    var username = FormatUsername(tweet.Username);

                    await _memoryManager.StoreCharacterMentionedTweets(
                        characterId: tweet.CharacterId,
                        mentionByPersonaPlatformId: username,
                        mentionedByPersonaName: tweet.Name,
                        conversationId: tweet.ConversationId,
                        tweetId: tweet.TweetId,
                        tweetContent: tweet.Text,
                        tweetUrl: tweet.PermanentUrl,
                        platformInteractionDate: tweet.TimeParsed,
                        memoryStatsTwitter: memoryStatsTwitter
                    );

                    await UpdateImportTweetExportedStatus(tweet);

                    await _unitOfWorkManager.Current.SaveChangesAsync();
                    await uow.CompleteAsync();
                }
            }
        }

        private async Task ProcessUpdatedCharacterMentionedTweets()
        {
            var characterMentionedTweetsUpdated = await _importTweetRepository
                .GetAll()
                .Where(x =>
                    x.Exported &&
                    !x.LastTwitterImportExported &&
                    x.TweetType == "CharacterMentionedTweet")
                .Take(50)
                .ToListAsync();

            foreach (var tweet in characterMentionedTweetsUpdated)
            {
                var memoryStatsTwitter = CreateMemoryStatsTwitter(tweet);
                var username = FormatUsername(tweet.Username);

                await _memoryManager.StoreCharacterMentionedTweets(
                    characterId: tweet.CharacterId,
                    mentionByPersonaPlatformId: username,
                    mentionedByPersonaName: tweet.Name,
                    conversationId: tweet.ConversationId,
                    tweetId: tweet.TweetId,
                    tweetContent: tweet.Text,
                    tweetUrl: tweet.PermanentUrl,
                    platformInteractionDate: tweet.TimeParsed,
                    memoryStatsTwitter: memoryStatsTwitter
                );

                await UpdateImportTweetLastExportedStatus(tweet);
            }
        }

        private async Task ProcessNewCharacterPersonaTweets()
        {
            var characterPersonaTweets = await _importTweetRepository
                .GetAll()
                .Where(x => !x.Exported && x.TweetType == "CharacterPersonaTweet")
                .Take(50)
                .ToListAsync();

            foreach (var tweet in characterPersonaTweets)
            {
                var username = FormatUsername(tweet.Username);

                await _memoryManager.StoreCharacterPersonaTweets(
                    characterId: tweet.CharacterId,
                    personaPlatformId: username,
                    personaName: tweet.Name,
                    conversationId: tweet.ConversationId,
                    tweetId: tweet.TweetId,
                    tweetContent: tweet.Text,
                    tweetUrl: tweet.PermanentUrl,
                    platformInteractionDate: tweet.TimeParsed
                );

                await UpdateImportTweetExportedStatus(tweet);
            }
        }

        #endregion

        #region Private Methods (Post Tweets)

        private async Task PostCharacterTweetAsync(TwitterImportTask task, Character character)
        {
            var startTime = DateTime.UtcNow;
            using (var uow = _unitOfWorkManager.Begin())
            {
                try
                {
                    if (!IsValidTaskForPosting(task, character.TwitterPostAgentId))
                    {
                        // Log & return early
                        await LogInvalidTaskAsync(task, character, "PostCharacterTweetAsync: Invalid task or missing fields.");
                        await uow.CompleteAsync();
                        return;
                    }

                    // The actual tweet prompt
                    await _memoryManager.RunCharacterPostTweetPrompt(character.Id, AIModelType.DirectOpenAI);
                }
                catch (Exception ex)
                {
                    await LogAsync(new TwitterImportLog
                    {
                        TenantId = task?.TenantId ?? 0,
                        CharacterId = task?.CharacterId ?? Guid.Empty,
                        TwitterAgentId = character.TwitterPostAgentId,
                        TaskName = task?.TaskName,
                        Message = "PostCharacterTweetAsync failed.",
                        LogLevel = "Error",
                        Exception = ex.ToString(),
                        ExceptionMessage = ex.Message,
                        LoggedAt = DateTime.UtcNow
                    });
                    await _unitOfWorkManager.Current.SaveChangesAsync();
                    throw;
                }

                UpdateTaskTiming(task, startTime);
                await _twitterImportTaskRepository.UpdateAsync(task);

                await LogAsync(new TwitterImportLog
                {
                    TenantId = task.TenantId,
                    CharacterId = task.CharacterId,
                    TwitterAgentId = character.TwitterPostAgentId,
                    TaskName = task.TaskName,
                    Message = "Successfully posted character tweet.",
                    LogLevel = "Information",
                    LoggedAt = DateTime.UtcNow
                });

                await _unitOfWorkManager.Current.SaveChangesAsync();
                await uow.CompleteAsync();
            }
        }

        private async Task ProcessCharacterPersonaWelcomeTweet(TwitterImportTask task, Character character)
        {
            var startTime = DateTime.UtcNow;
            using (var uow = _unitOfWorkManager.Begin())
            {
                try
                {
                    if (!IsValidTaskForPosting(task, character.TwitterPostAgentId))
                    {
                        await LogInvalidTaskAsync(task, character, "ProcessCharacterPersonaWelcomeTweet: Invalid task or missing fields.");
                        await uow.CompleteAsync();
                        return;
                    }

                    var characterPersonas = await _characterPersonaRepository
                        .GetAll()
                        .Include(x => x.TwitterProfile)
                        .Include(x => x.TwitterRank)
                        .Include(x => x.Persona).ThenInclude(p => p.Platforms).ThenInclude(p => p.Platform)
                        .Where(x =>
                            x.CharacterId == character.Id &&
                            !x.WelcomeMessageSent &&
                            x.Persona.Platforms.Any(p => p.Platform.Name == "Twitter"))
                        .OrderBy(x => x.TwitterRank.Rank)
                        .Take(10)
                        .ToListAsync();

                    await TrySendPersonaWelcomeTweets(characterPersonas, character, task);
                }
                catch (Exception ex)
                {
                    await LogAsync(new TwitterImportLog
                    {
                        TenantId = task?.TenantId ?? 0,
                        CharacterId = task?.CharacterId ?? Guid.Empty,
                        TwitterAgentId = character.TwitterPostAgentId,
                        TaskName = task?.TaskName,
                        Message = "ProcessCharacterPersonaWelcomeTweet failed.",
                        LogLevel = "Error",
                        Exception = ex.ToString(),
                        ExceptionMessage = ex.Message,
                        LoggedAt = DateTime.UtcNow
                    });
                    await _unitOfWorkManager.Current.SaveChangesAsync();
                    throw;
                }

                UpdateTaskTiming(task, startTime);
                await _twitterImportTaskRepository.UpdateAsync(task);

                await LogAsync(new TwitterImportLog
                {
                    TenantId = task.TenantId,
                    CharacterId = task.CharacterId,
                    TwitterAgentId = character.TwitterPostAgentId,
                    TaskName = task.TaskName,
                    Message = "Successfully posted persona welcome tweets.",
                    LogLevel = "Information",
                    LoggedAt = DateTime.UtcNow
                });

                await _unitOfWorkManager.Current.SaveChangesAsync();
                await uow.CompleteAsync();
            }
        }
        public async Task TriggerPostCharacterTweetRisingSproutAsync()
        {
            var character = await _characterRepository
                .GetAll()
                .Where(c => c.Name == "Plant")
                .FirstOrDefaultAsync();

            if (character == null) throw new UserFriendlyException("Character not found.");

            var task = await _twitterImportTaskRepository
                .GetAll()
                .Where(t => t.CharacterId == character.Id && t.TaskName == "PostCharacterTweetRisingSproutAsync")
                .FirstOrDefaultAsync();

            if (task == null) throw new UserFriendlyException("Task not found.");

            await PostCharacterTweetRisingSproutAsync(task, character);
        }

        private async Task PostCharacterTweetRisingSproutAsync(TwitterImportTask task, Character character)
        {
            var startTime = DateTime.UtcNow;
            using (var uow = _unitOfWorkManager.Begin())
            {
                try
                {
                    if (!IsValidTaskForPosting(task, character.TwitterPostAgentId))
                    {
                        await LogInvalidTaskAsync(task, character, "PostCharacterTweetRisingSproutAsync: Invalid task or missing fields.");
                        await uow.CompleteAsync();
                        return;
                    }

                    var bestRaydiumPair = await _tokenPoolManager.GetBestPerformingPair();
                    if (bestRaydiumPair == null)
                    {
                        await LogAsync(new TwitterImportLog
                        {
                            TenantId = task.TenantId,
                            CharacterId = task.CharacterId,
                            TwitterAgentId = character.TwitterPostAgentId,
                            TaskName = task.TaskName,
                            Message = "No sprouting pool found.",
                            LogLevel = "Information",
                            LoggedAt = DateTime.UtcNow
                        });

                        await uow.CompleteAsync();
                        return;
                    }


                    var lastPriceUpdateId = bestRaydiumPair.CoingeckoLastAggregatedUpdateId;
                    if (lastPriceUpdateId == null)
                    {
                        await LogAsync(new TwitterImportLog
                        {
                            TenantId = task.TenantId,
                            CharacterId = task.CharacterId,
                            TwitterAgentId = character.TwitterPostAgentId,
                            TaskName = task.TaskName,
                            Message = "No price update found. for Raydium pair with ID: " + bestRaydiumPair.Id,
                            LogLevel = "Information",
                            LoggedAt = DateTime.UtcNow
                        });

                        await uow.CompleteAsync();
                        return;
                    }

                    var lastCoinGeckoAggregate = await _tokenPoolManager.GetCoingeckoAggregatedUpdate((Guid)lastPriceUpdateId);

                    if (lastCoinGeckoAggregate.VolumeH1 < 50000) return;
                    if (lastCoinGeckoAggregate.PriceChangeM5 < 0) return;


                    List<TwitterImportTweetEngagement> first10Tweets = null;
                    List<TwitterImportTweetEngagement> lastTweets = null;
                    if (bestRaydiumPair.TwitterCAFound)
                    {
                        first10Tweets = await _importTweetEngagementRepository
                            .GetAll()
                            .Where(x => x.RaydiumPairId == bestRaydiumPair.Id)
                            .OrderBy(x => x.CreatedAt)
                            .Take(10)
                            .ToListAsync();

                        var lastAmountToTake = 20;
                        var tweetsSentAfterFirst10 = bestRaydiumPair.TweetsCATweetCount - 10;

                        if (tweetsSentAfterFirst10 >= 20)
                        {
                            lastAmountToTake = 20;
                        }
                        else if (tweetsSentAfterFirst10 < 20 && tweetsSentAfterFirst10 > 0)
                        {
                            lastAmountToTake = tweetsSentAfterFirst10;
                        }
                        else if (tweetsSentAfterFirst10 <= 0)
                        {
                            lastAmountToTake = 0;
                        }

                        if (lastAmountToTake > 0)
                        {
                            lastTweets = await _importTweetEngagementRepository
                                .GetAll()
                                .Where(x => x.RaydiumPairId == bestRaydiumPair.Id)
                                .OrderByDescending(x => x.CreatedAt)
                                .Take(lastAmountToTake)
                                .ToListAsync();
                        }
                    }

                    var promptContext = new AICharacterPostTokenTweetContext(bestRaydiumPair, lastCoinGeckoAggregate, first10Tweets, lastTweets);
                    var promptResponse = await _aiManager.GeneratePostTokenTweetResponseAsync(promptContext, AIModelType.DirectOpenAI, shouldUseSmartModel: true);

                    await _tokenPoolManager.SetTokenPoolTweeted(bestRaydiumPair.Id, promptResponse);
                    await TrySendRisingSproutTweet(character, task, promptResponse);
                }
                catch (Exception ex)
                {
                    await LogAsync(new TwitterImportLog
                    {
                        TenantId = task?.TenantId ?? 0,
                        CharacterId = task?.CharacterId ?? Guid.Empty,
                        TwitterAgentId = character.TwitterPostAgentId,
                        TaskName = task?.TaskName,
                        Message = "PostCharacterTweetRisingSproutAsync failed.",
                        LogLevel = "Error",
                        Exception = ex.ToString(),
                        ExceptionMessage = ex.Message,
                        LoggedAt = DateTime.UtcNow
                    });
                    await _unitOfWorkManager.Current.SaveChangesAsync();
                    throw;
                }

                UpdateTaskTiming(task, startTime);
                await _twitterImportTaskRepository.UpdateAsync(task);

                await LogAsync(new TwitterImportLog
                {
                    TenantId = task.TenantId,
                    CharacterId = task.CharacterId,
                    TwitterAgentId = character.TwitterPostAgentId,
                    TaskName = task.TaskName,
                    Message = "Successfully posted character rising sprout tweet.",
                    LogLevel = "Information",
                    LoggedAt = DateTime.UtcNow
                });

                await _unitOfWorkManager.Current.SaveChangesAsync();
                await uow.CompleteAsync();
            }
        }

        #endregion


        #region Private Methods (Import Tweets)

        private async Task<TwitterImportTask> ImportTweetsOfCharacterAsync(TwitterImportTask task, Character character)
        {
            var startTime = DateTime.UtcNow;

            using (var uow = _unitOfWorkManager.Begin())
            {
                try
                {
                    if (character == null)
                    {
                        await LogNoCharacterFoundAsync(task, "No matching character found in DB.");
                        await uow.CompleteAsync();
                        return task;
                    }

                    if (!IsValidTaskForScraping(task, character?.TwitterScrapeAgentId))
                    {
                        await LogInvalidTaskAsync(task, character, "ImportTweetsOfCharacterAsync: Invalid task or missing fields.");
                        await uow.CompleteAsync();
                        return task;
                    }

                    var importLimit = task.ImportLimitTotal > 0 ? task.ImportLimitTotal : 10;
                    var userSearch = character.TwitterUserName.StartsWith("@")
                        ? character.TwitterUserName.Substring(1)
                        : character.TwitterUserName;

                    var tweets = await _twitterCommunicationService.GetTweetsAsync(
                        character.TwitterScrapeAgentId,
                        userSearch,
                        importLimit
                    );

                    foreach (var tweet in tweets)
                    {
                        await MapAndUpsertTweetAsync(
                            tweet,
                            task,
                            character.Id,
                            character.Name,
                            "CharacterTweet"
                        );
                    }

                    UpdateTaskTiming(task, startTime);
                    await _twitterImportTaskRepository.UpdateAsync(task);

                    await LogAsync(new TwitterImportLog
                    {
                        TenantId = task.TenantId,
                        CharacterId = task.CharacterId,
                        TwitterAgentId = character.TwitterScrapeAgentId,
                        TaskName = task.TaskName,
                        Message = $"Successfully imported {tweets.Count} tweets.",
                        LogLevel = "Information",
                        LoggedAt = DateTime.UtcNow
                    });

                    await _unitOfWorkManager.Current.SaveChangesAsync();
                    await uow.CompleteAsync();

                    return task;
                }
                catch (Exception ex)
                {
                    await LogImportErrorAsync(task, character, ex, "Import failed.");
                    throw;
                }
            }
        }

        private async Task<TwitterImportTask> ImportMentionsOfCharacterAsync(TwitterImportTask task, Character character)
        {
            var startTime = DateTime.UtcNow;

            using (var uow = _unitOfWorkManager.Begin())
            {
                try
                {
                    if (character == null)
                    {
                        await LogNoCharacterFoundAsync(task, "No matching character found in DB.");
                        await uow.CompleteAsync();
                        return task;
                    }

                    if (!IsValidTaskForScraping(task, character?.TwitterScrapeAgentId))
                    {
                        await LogInvalidTaskAsync(task, character, "ImportMentionsOfCharacterAsync: Invalid task or missing fields.");
                        await uow.CompleteAsync();
                        return task;
                    }

                    var userSearch = character.TwitterUserName.StartsWith("@")
                        ? character.TwitterUserName.Substring(1)
                        : character.TwitterUserName;

                    var mentions = await _twitterCommunicationService.GetUserMentionsAsync(
                        character.TwitterScrapeAgentId,
                        userSearch,
                        task.ImportLimitTotal
                    );

                    foreach (var tweet in mentions)
                    {
                        await MapAndUpsertTweetAsync(
                            tweet,
                            task,
                            character.Id,
                            character.Name,
                            "CharacterMentionedTweet"
                        );
                    }

                    UpdateTaskTiming(task, startTime);
                    await _twitterImportTaskRepository.UpdateAsync(task);

                    await LogAsync(new TwitterImportLog
                    {
                        TenantId = task.TenantId,
                        CharacterId = task.CharacterId,
                        TwitterAgentId = character.TwitterScrapeAgentId,
                        TaskName = task.TaskName,
                        Message = "Successfully imported mentions for character.",
                        LogLevel = "Information",
                        LoggedAt = DateTime.UtcNow
                    });

                    await _unitOfWorkManager.Current.SaveChangesAsync();
                    await uow.CompleteAsync();

                    return task;
                }
                catch (Exception ex)
                {
                    await LogImportErrorAsync(task, character, ex, "ImportMentionsOfCharacter failed.");
                    throw;
                }
            }
        }

        private async Task<TwitterImportTask> ImportMentionsOfCharacterWithAPIAsync(TwitterImportTask task, Character character)
        {
            var startTime = DateTime.UtcNow;

            try
            {
                if (character == null)
                {
                    await LogNoCharacterFoundAsync(task, "No matching character found in DB.");
                    return task;
                }

                if (!IsValidTaskForPosting(task, character.TwitterPostAgentId))
                {
                    await LogInvalidTaskAsync(task, character, "ImportMentionsOfCharacterWithAPIAsync: Invalid task or missing fields.");
                    return task;
                }

                var sinceId = task.LastTweetImportId;
                var apiMentions = await _twitterCommunicationService.GetMentionsFromTweetIdAsync(
                    character.TwitterPostAgentId,
                    sinceId,
                    task.ImportLimitTotal
                );

                var highestId = sinceId;
                var mentionList = apiMentions.ToList();
                var totalMentions = mentionList.Count;
                const int batchSize = 25;
                var index = 0;

                // Process in smaller batches
                while (index < totalMentions)
                {
                    using (var batchUow = _unitOfWorkManager.Begin())
                    {
                        var batchTweets = mentionList
                            .Skip(index)
                            .Take(batchSize)
                            .ToList();

                        foreach (var tweet in batchTweets)
                        {
                            var mappedScraperTweet = ConvertApiTweetToScraperTweet(tweet);
                            await MapAndUpsertTweetAsync(
                                mappedScraperTweet,
                                task,
                                character.Id,
                                character.Name,
                                "CharacterMentionedTweet"
                            );

                            if (IsGreaterTweetId(tweet.Id, highestId))
                            {
                                highestId = tweet.Id;
                            }
                        }

                        await _unitOfWorkManager.Current.SaveChangesAsync();
                        await batchUow.CompleteAsync();
                    }

                    index += batchSize;
                }

                if (!string.IsNullOrEmpty(highestId))
                {
                    task.LastTweetImportId = highestId;
                }

                UpdateTaskTiming(task, startTime);

                using (var finalUow = _unitOfWorkManager.Begin())
                {
                    await _twitterImportTaskRepository.UpdateAsync(task);

                    await LogAsync(new TwitterImportLog
                    {
                        TenantId = task.TenantId,
                        CharacterId = task.CharacterId,
                        TwitterAgentId = character.TwitterPostAgentId,
                        TaskName = task.TaskName,
                        Message = "Successfully imported mentions for character using API.",
                        LogLevel = "Information",
                        LoggedAt = DateTime.UtcNow
                    });

                    await _unitOfWorkManager.Current.SaveChangesAsync();
                    await finalUow.CompleteAsync();
                }

                return task;
            }
            catch (Exception ex)
            {
                using (var errorUow = _unitOfWorkManager.Begin())
                {
                    await LogAsync(new TwitterImportLog
                    {
                        TenantId = task?.TenantId ?? 0,
                        CharacterId = task?.CharacterId ?? Guid.Empty,
                        TwitterAgentId = character?.TwitterPostAgentId,
                        TaskName = task?.TaskName,
                        Message = "ImportMentionsOfCharacterWithAPIAsync failed.",
                        LogLevel = "Error",
                        Exception = ex.ToString(),
                        ExceptionMessage = ex.Message,
                        LoggedAt = DateTime.UtcNow
                    });

                    await _unitOfWorkManager.Current.SaveChangesAsync();
                    await errorUow.CompleteAsync();
                }
                throw;
            }
        }

        private async Task<TwitterImportTask> ImportTweetsOfCharacterPersonasAsync(TwitterImportTask task, Character character)
        {
            var startTime = DateTime.UtcNow;

            using (var uow = _unitOfWorkManager.Begin())
            {
                try
                {
                    if (character == null)
                    {
                        await LogNoCharacterFoundAsync(task, "No matching character found in DB.");
                        await uow.CompleteAsync();
                        return task;
                    }

                    if (!IsValidTaskForScraping(task, character?.TwitterScrapeAgentId))
                    {
                        await LogInvalidTaskAsync(task, character, "ImportTweetsOfCharacterPersonasAsync: Invalid task or missing fields.");
                        await uow.CompleteAsync();
                        return task;
                    }

                    var characterPersonas = await _characterPersonaRepository
                        .GetAll()
                        .Include(cp => cp.Persona)
                            .ThenInclude(p => p.Platforms)
                                .ThenInclude(p => p.Platform)
                        .Where(cp => cp.CharacterId == character.Id && cp.ShouldImportNewPosts)
                        .ToListAsync();

                    if (characterPersonas == null || !characterPersonas.Any())
                    {
                        task.LastRunCompletionTime = DateTime.UtcNow;
                        await _unitOfWorkManager.Current.SaveChangesAsync();
                        await uow.CompleteAsync();
                        return task;
                    }

                    characterPersonas = characterPersonas
                        .Where(cp => cp.Persona.Platforms.Any(p => p.Platform.Name == "Twitter"))
                        .ToList();

                    var personas = characterPersonas.Select(cp => cp.Persona).ToList();
                    if (personas == null || !personas.Any())
                    {
                        task.LastRunCompletionTime = DateTime.UtcNow;
                        await _unitOfWorkManager.Current.SaveChangesAsync();
                        await uow.CompleteAsync();
                        return task;
                    }

                    foreach (var persona in personas)
                    {
                        var twitterPlatform = persona.Platforms.FirstOrDefault(p => p.Platform.Name == "Twitter");
                        if (twitterPlatform == null) continue;

                        var userSearch = twitterPlatform.PlatformPersonaId.StartsWith("@")
                            ? twitterPlatform.PlatformPersonaId.Substring(1)
                            : twitterPlatform.PlatformPersonaId;

                        var tweets = await _twitterCommunicationService.GetTweetsAsync(
                            character.TwitterScrapeAgentId,
                            userSearch,
                            task.ImportLimitTotal > 0 ? task.ImportLimitTotal : 10
                        );

                        foreach (var tweet in tweets)
                        {
                            await MapAndUpsertTweetAsync(
                                tweet,
                                task,
                                character.Id,
                                character.Name,
                                "CharacterPersonaTweet"
                            );
                        }
                    }

                    UpdateTaskTiming(task, startTime);
                    await _twitterImportTaskRepository.UpdateAsync(task);

                    await LogAsync(new TwitterImportLog
                    {
                        TenantId = task.TenantId,
                        CharacterId = task.CharacterId,
                        TwitterAgentId = character.TwitterScrapeAgentId,
                        TaskName = task.TaskName,
                        Message = "Successfully imported tweets for character personas.",
                        LogLevel = "Information",
                        LoggedAt = DateTime.UtcNow
                    });

                    await _unitOfWorkManager.Current.SaveChangesAsync();
                    await uow.CompleteAsync();

                    return task;
                }
                catch (Exception ex)
                {
                    await LogImportErrorAsync(task, character, ex, "ImportCharacterPersonas failed.");
                    throw;
                }
            }
        }

        #endregion

        #region Private Helpers (Tweet Upsert, Logging, Etc.)

        private async Task<TwitterImportTweet> MapAndUpsertTweetAsync(
            TwitterScraperTweetResponse tweet,
            TwitterImportTask importTask,
            Guid characterId,
            string characterName,
            string tweetType)
        {
            var tweetId = tweet.Id ?? string.Empty;
            var existingEntity = await _importTweetRepository.FirstOrDefaultAsync(x =>
                x.TweetId == tweetId && x.CharacterId == characterId);

            var isNew = existingEntity == null;
            if (isNew)
            {
                existingEntity = new TwitterImportTweet
                {
                    TenantId = importTask.TenantId,
                    TweetType = tweetType,
                    CharacterId = characterId,
                    CharacterName = characterName,
                    TweetId = tweetId
                };
            }

            // Merge data
            existingEntity.Hashtags = MergeValues(existingEntity.Hashtags,
                tweet.Hashtags != null ? string.Join(",", tweet.Hashtags) : null);
            existingEntity.Urls = MergeValues(existingEntity.Urls,
                tweet.Urls != null ? string.Join(",", tweet.Urls) : null);

            existingEntity.MentionsJson = MergeValues(existingEntity.MentionsJson,
                tweet.Mentions != null ? JsonSerializer.Serialize(tweet.Mentions) : null);
            existingEntity.PhotosJson = MergeValues(existingEntity.PhotosJson,
                tweet.Photos != null ? JsonSerializer.Serialize(tweet.Photos) : null);
            existingEntity.VideosJson = MergeValues(existingEntity.VideosJson,
                tweet.Videos != null ? JsonSerializer.Serialize(tweet.Videos) : null);
            existingEntity.PlaceJson = MergeValues(existingEntity.PlaceJson,
                tweet.Place != null ? JsonSerializer.Serialize(tweet.Place) : null);
            existingEntity.PollJson = MergeValues(existingEntity.PollJson,
                tweet.Poll != null ? JsonSerializer.Serialize(tweet.Poll) : null);
            existingEntity.InReplyToStatusJson = MergeValues(existingEntity.InReplyToStatusJson,
                tweet.InReplyToStatus != null ? JsonSerializer.Serialize(tweet.InReplyToStatus) : null);
            existingEntity.QuotedStatusJson = MergeValues(existingEntity.QuotedStatusJson,
                tweet.QuotedStatus != null ? JsonSerializer.Serialize(tweet.QuotedStatus) : null);
            existingEntity.RetweetedStatusJson = MergeValues(existingEntity.RetweetedStatusJson,
                tweet.RetweetedStatus != null ? JsonSerializer.Serialize(tweet.RetweetedStatus) : null);
            existingEntity.ThreadJson = MergeValues(existingEntity.ThreadJson,
                tweet.Thread != null ? JsonSerializer.Serialize(tweet.Thread) : null);

            existingEntity.BookmarkCount = tweet.BookmarkCount ?? existingEntity.BookmarkCount;
            existingEntity.ConversationId = MergeValues(existingEntity.ConversationId, tweet.ConversationId);
            existingEntity.Html = MergeValues(existingEntity.Html, tweet.Html);
            existingEntity.InReplyToStatusId = MergeValues(existingEntity.InReplyToStatusId, tweet.InReplyToStatusId);
            existingEntity.IsQuoted = tweet.IsQuoted ?? existingEntity.IsQuoted;
            existingEntity.IsPin = tweet.IsPin ?? existingEntity.IsPin;
            existingEntity.IsReply = tweet.IsReply ?? existingEntity.IsReply;
            existingEntity.IsRetweet = tweet.IsRetweet ?? existingEntity.IsRetweet;
            existingEntity.IsSelfThread = tweet.IsSelfThread ?? existingEntity.IsSelfThread;
            existingEntity.Likes = tweet.Likes ?? existingEntity.Likes;
            existingEntity.Name = MergeValues(existingEntity.Name, tweet.Name);
            existingEntity.PermanentUrl = MergeValues(existingEntity.PermanentUrl, tweet.PermanentUrl);
            existingEntity.QuotedStatusId = MergeValues(existingEntity.QuotedStatusId, tweet.QuotedStatusId);
            existingEntity.Replies = tweet.Replies ?? existingEntity.Replies;
            existingEntity.Retweets = tweet.Retweets ?? existingEntity.Retweets;
            existingEntity.RetweetedStatusId = MergeValues(existingEntity.RetweetedStatusId, tweet.RetweetedStatusId);
            existingEntity.Text = MergeValues(existingEntity.Text, tweet.Text);
            existingEntity.TimeParsed = tweet.TimeParsed ?? existingEntity.TimeParsed;
            existingEntity.Timestamp = tweet.Timestamp ?? existingEntity.Timestamp;
            existingEntity.UserId = MergeValues(existingEntity.UserId, tweet.UserId);
            existingEntity.Username = MergeValues(existingEntity.Username, tweet.Username);
            existingEntity.Views = tweet.Views ?? existingEntity.Views;
            existingEntity.SensitiveContent = tweet.SensitiveContent ?? existingEntity.SensitiveContent;
            existingEntity.LastTwitterImportDate = DateTime.UtcNow;
            existingEntity.LastTwitterImportExported = false;

            if (isNew)
            {
                await _importTweetRepository.InsertAsync(existingEntity);
            }
            else
            {
                await _importTweetRepository.UpdateAsync(existingEntity);
            }

            return existingEntity;
        }

        private async Task UpdateImportTweetExportedStatus(TwitterImportTweet tweet)
        {
            using (var uow = _unitOfWorkManager.Begin())
            {
                tweet.Exported = true;
                tweet.ExportDate = DateTime.UtcNow;
                await _importTweetRepository.UpdateAsync(tweet);
                await _unitOfWorkManager.Current.SaveChangesAsync();
                await uow.CompleteAsync();
            }
        }

        private async Task UpdateImportTweetLastExportedStatus(TwitterImportTweet tweet)
        {
            using (var uow = _unitOfWorkManager.Begin())
            {
                tweet.ExportDate = DateTime.UtcNow;
                tweet.LastTwitterImportExported = true;
                await _importTweetRepository.UpdateAsync(tweet);
                await _unitOfWorkManager.Current.SaveChangesAsync();
                await uow.CompleteAsync();
            }
        }

        #endregion

        #region Private Helpers (Profiles, Logging, Utility)

        private async Task<TwitterScraperUserProfileResponse> TryGetUserProfileAsync(Character character, string userSearch)
        {
            TwitterScraperUserProfileResponse twitterProfile = null;
            try
            {
                twitterProfile = await _twitterCommunicationService.GetUserProfileAsync(
                    character.TwitterScrapeAgentId,
                    userSearch
                );
            }
            catch (Exception ex)
            {
                await LogAsync(new TwitterImportLog
                {
                    TenantId = character.TenantId,
                    CharacterId = character.Id,
                    TwitterAgentId = character.TwitterScrapeAgentId,
                    TaskName = "ProcessMissingTwitterProfiles",
                    Message = "GetUserProfileAsync failed.",
                    LogLevel = "Error",
                    Exception = ex.ToString(),
                    ExceptionMessage = ex.Message,
                    LoggedAt = DateTime.UtcNow
                });
            }

            return twitterProfile;
        }

        private MemoryStatsTwitter CreateMemoryStatsTwitter(TwitterImportTweet tweet)
        {
            var mentionCount = 0;
            if (!string.IsNullOrEmpty(tweet.MentionsJson))
            {
                // Very rough approach: count curly braces for multiple mentions, 
                // though your original code had: (tweet.MentionsJson.ToString().Split('}').Length - 1)
                mentionCount = tweet.MentionsJson.Split('}').Length - 1;
            }

            return new MemoryStatsTwitter
            {
                TenantId = tweet.TenantId,
                IsPin = tweet.IsPin,
                IsQuoted = tweet.IsQuoted,
                IsReply = tweet.IsReply,
                IsRetweet = tweet.IsRetweet,
                SensitiveContent = tweet.SensitiveContent,
                BookmarkCount = tweet.BookmarkCount,
                Likes = tweet.Likes,
                Replies = tweet.Replies,
                Retweets = tweet.Retweets,
                Views = tweet.Views,
                TweetWordCount = (tweet.Text ?? string.Empty).Split(' ').Length,
                MentionsCount = mentionCount
            };
        }

        private static string FormatUsername(string username)
        {
            return username.StartsWith("@") ? username : "@" + username;
        }

        private bool IsGreaterTweetId(string newId, string currentId)
        {
            if (string.IsNullOrWhiteSpace(newId)) return false;
            if (string.IsNullOrWhiteSpace(currentId)) return true;
            if (long.TryParse(newId, out var newLong) && long.TryParse(currentId, out var currLong))
            {
                return newLong > currLong;
            }
            return false;
        }

        private TwitterScraperTweetResponse ConvertApiTweetToScraperTweet(TwitterApiGetTweetResponse apiTweet)
        {
            return new TwitterScraperTweetResponse
            {
                Id = apiTweet.Id,
                Text = apiTweet.Text,
                Username = apiTweet.AuthorUsername,
                Name = apiTweet.AuthorName,
                UserId = apiTweet.AuthorId,
                ConversationId = apiTweet.ConversationId,
                PermanentUrl = $"https://x.com/{apiTweet.AuthorUsername}/status/{apiTweet.Id}",
                TimeParsed = apiTweet.CreatedAt,
                BookmarkCount = apiTweet.PublicMetrics?.BookmarkCount,
                Likes = apiTweet.PublicMetrics?.LikeCount,
                Replies = apiTweet.PublicMetrics?.ReplyCount,
                Retweets = apiTweet.PublicMetrics?.RetweetCount,
                Views = apiTweet.PublicMetrics?.ImpressionCount
            };
        }

        private static string MergeValues(string existingValue, string updatedValue)
        {
            if (!string.IsNullOrEmpty(updatedValue)) return updatedValue;
            return existingValue ?? string.Empty;
        }

        #endregion

        #region Private Helpers (Task Validation & Updates)

        private static bool IsValidTaskForPosting(TwitterImportTask task, string agentId)
        {
            if (task == null) return false;
            if (task.CharacterId == Guid.Empty) return false;
            if (string.IsNullOrEmpty(agentId)) return false;
            return true;
        }

        private static bool IsValidTaskForScraping(TwitterImportTask task, string agentId)
        {
            if (task == null) return false;
            if (task.CharacterId == Guid.Empty) return false;
            if (string.IsNullOrEmpty(agentId)) return false;
            return true;
        }

        private static void UpdateTaskTiming(TwitterImportTask task, DateTime startTime)
        {
            var endTime = DateTime.UtcNow;
            task.LastRunCompletionTime = endTime;
            task.LastRunStartTime = startTime;
            task.LastRunDurationSeconds = (int)(endTime - startTime).TotalSeconds;

            if (task.RunEveryXMinutes > 0)
            {
                task.NextRunTime = endTime.AddMinutes(task.RunEveryXMinutes);
            }
        }

        #endregion

        #region Private Helpers (Logging & Persona Welcomes)

        private async Task TrySendRisingSproutTweet(
            Character character,
            TwitterImportTask task,
            string tweet)
        {
            try
            {
                // send the tweet

            }
            catch (Exception ex)
            {
                await LogAsync(new TwitterImportLog
                {
                    TenantId = task?.TenantId ?? 0,
                    CharacterId = task?.CharacterId ?? Guid.Empty,
                    TwitterAgentId = character.TwitterPostAgentId,
                    TaskName = task?.TaskName,
                    Message = "TrySendRisingSproutTweet failed.",
                    LogLevel = "Error",
                    Exception = ex.ToString(),
                    ExceptionMessage = ex.Message,
                    LoggedAt = DateTime.UtcNow
                });
                throw;
            }
        }

        private async Task TrySendPersonaWelcomeTweets(
            IEnumerable<CharacterPersona> characterPersonas,
            Character character,
            TwitterImportTask task)
        {
            const int limitToSend = 1;
            var successCount = 0;

            foreach (var characterPersona in characterPersonas)
            {
                if (successCount >= limitToSend) break;
                var twitterPlatform = characterPersona.Persona.Platforms.FirstOrDefault(p => p.Platform.Name == "Twitter");
                if (twitterPlatform == null) continue;

                var userHandle = twitterPlatform.PlatformPersonaId;
                var userSearch = userHandle.StartsWith("@") ? userHandle.Substring(1) : userHandle;
                var avatarUrl = characterPersona.TwitterProfile?.Avatar
                    ?? "https://abs.twimg.com/sticky/default_profile_images/default_profile_normal.png";
                var userNameForQuery = Uri.EscapeDataString(userSearch);
                var avatarUrlForQuery = Uri.EscapeDataString(avatarUrl);
                var imageUrl = $"https://plant.fun/api/leaderboard/image?userName={userNameForQuery}&twitterAvatarUrl={avatarUrlForQuery}";

                try
                {
                    using (var httpClient = new HttpClient())
                    {
                        var imageBytes = await httpClient.GetByteArrayAsync(imageUrl);
                        var imageBase64 = Convert.ToBase64String(imageBytes);

                        var welcomeText =
                            $"Follow me {userHandle} my new Gardener. and then reply under this post with your Solana wallet for Rain(air)drops! " +
                            "Keep feeding me mentions on X and watch me grow. \uD83C\uDF35\uD83C\uDF89";

                        await _twitterCommunicationService.PostTweetWithImageAsync(
                            character.TwitterPostAgentId,
                            imageBase64,
                            welcomeText
                        );

                        characterPersona.WelcomeMessageSent = true;
                        characterPersona.WelcomeMessageSentAt = DateTime.UtcNow;
                        await _characterPersonaRepository.UpdateAsync(characterPersona);

                        await LogAsync(new TwitterImportLog
                        {
                            TenantId = characterPersona.TenantId,
                            CharacterId = characterPersona.CharacterId,
                            TwitterAgentId = character.TwitterPostAgentId,
                            TaskName = task.TaskName,
                            Message = $"Successfully posted welcome tweet to {userHandle}.",
                            LogLevel = "Information",
                            LoggedAt = DateTime.UtcNow
                        });

                        successCount++;
                    }
                }
                catch (Exception ex)
                {
                    await LogAsync(new TwitterImportLog
                    {
                        TenantId = characterPersona.TenantId,
                        CharacterId = characterPersona.CharacterId,
                        TwitterAgentId = character.TwitterPostAgentId,
                        TaskName = task.TaskName,
                        Message = $"Failed posting welcome tweet to {userHandle}.",
                        LogLevel = "Error",
                        Exception = ex.ToString(),
                        ExceptionMessage = ex.Message,
                        LoggedAt = DateTime.UtcNow
                    });
                }

                await _unitOfWorkManager.Current.SaveChangesAsync();
            }
        }

        private async Task LogInvalidTaskAsync(TwitterImportTask task, Character character, string message)
        {
            task ??= new TwitterImportTask();
            task.LastRunCompletionTime = null;

            await LogAsync(new TwitterImportLog
            {
                TenantId = task.TenantId,
                CharacterId = task.CharacterId,
                TwitterAgentId = character.TwitterPostAgentId,
                TaskName = task.TaskName,
                Message = message,
                LogLevel = "Warning",
                LoggedAt = DateTime.UtcNow
            });
            await _unitOfWorkManager.Current.SaveChangesAsync();
        }

        private async Task LogNoCharacterFoundAsync(TwitterImportTask task, string message)
        {
            task.LastRunCompletionTime = null;

            await LogAsync(new TwitterImportLog
            {
                TenantId = task.TenantId,
                CharacterId = task.CharacterId,
                TaskName = task.TaskName,
                Message = message,
                LogLevel = "Warning",
                LoggedAt = DateTime.UtcNow
            });
            await _unitOfWorkManager.Current.SaveChangesAsync();
        }

        private async Task LogImportErrorAsync(TwitterImportTask task, Character character, Exception ex, string message)
        {
            await LogAsync(new TwitterImportLog
            {
                TenantId = task?.TenantId ?? 0,
                CharacterId = task?.CharacterId ?? Guid.Empty,
                TwitterAgentId = character?.TwitterScrapeAgentId,
                TaskName = task?.TaskName,
                Message = message,
                LogLevel = "Error",
                Exception = ex.ToString(),
                ExceptionMessage = ex.Message,
                LoggedAt = DateTime.UtcNow
            });
            await _unitOfWorkManager.Current.SaveChangesAsync();
        }

        private async Task LogAsync(TwitterImportLog log)
        {
            // Single place to insert logs; can be extended if needed
            await _twitterImportLogRepository.InsertAsync(log);
        }

        #endregion
    }



}
