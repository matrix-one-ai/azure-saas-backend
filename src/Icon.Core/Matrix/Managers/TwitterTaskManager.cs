using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Icon.Matrix.AIManager;
using Icon.Matrix.Enums;
using Icon.Matrix.Models;
using Icon.Matrix.TwitterManager;
using Microsoft.EntityFrameworkCore;
using PayPalCheckoutSdk.Orders;

namespace Icon.Matrix.Twitter
{
    public interface ITwitterTaskManager
    {
        Task ProcessCharacterTweetImports();
        Task ProcessCharacterTweetsStorage();
        Task ProcessCharacterPostTweets();
        Task ProcessMissingTwitterProfiles();
    }

    public class TwitterTaskManager : ITwitterTaskManager, ITransientDependency
    {
        private readonly ITwitterCommunicationService _twitterCommunicationService;
        private readonly IRepository<TwitterImportTweet, Guid> _importTweetRepository;
        private readonly IRepository<Character, Guid> _characterRepository;
        private readonly IRepository<CharacterPersona, Guid> _characterPersonaRepository;
        private readonly IRepository<TwitterImportTask, Guid> _twitterImportTaskRepository;
        private readonly IRepository<TwitterImportLog, Guid> _twitterImportLogRepository;
        private readonly IMemoryManager _memoryManager;
        private readonly IAIManager _aiManager;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public TwitterTaskManager(
            ITwitterCommunicationService twitterCommunicationService,
            IRepository<TwitterImportTweet, Guid> importTweetRepository,
            IRepository<Character, Guid> characterRepository,
            IRepository<CharacterPersona, Guid> characterPersonaRepository,
            IRepository<TwitterImportTask, Guid> twitterImportTaskRepository,
            IRepository<TwitterImportLog, Guid> twitterImportLogRepository,
            IMemoryManager memoryManager,
            IAIManager aiManager,
            IUnitOfWorkManager unitOfWorkManager)

        {
            _twitterCommunicationService = twitterCommunicationService;
            _importTweetRepository = importTweetRepository;
            _characterRepository = characterRepository;
            _characterPersonaRepository = characterPersonaRepository;
            _twitterImportTaskRepository = twitterImportTaskRepository;
            _twitterImportLogRepository = twitterImportLogRepository;
            _memoryManager = memoryManager;
            _aiManager = aiManager;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public async Task ProcessCharacterTweetsStorage()
        {
            await ProcessNewCharacterTweets();

            await ProcessUpdatedCharacterMentionedTweets();
            await ProcessNewCharacterMentionedTweets();

            await ProcessNewCharacterPersonaTweets();
        }

        private async Task ProcessNewCharacterTweets()
        {
            var characterTweets = await _importTweetRepository
                .GetAll()
                .Where(x => x.Exported == false && x.TweetType == "CharacterTweet")
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
                .Where(x => x.Exported == false && x.TweetType == "CharacterMentionedTweet")
                .ToListAsync();

            foreach (var tweet in characterMentionedTweetsNew)
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
            }
        }

        private async Task ProcessUpdatedCharacterMentionedTweets()
        {
            var characterMentionedTweetsUpdated = await _importTweetRepository
                .GetAll()
                .Where(x => x.Exported == true && x.LastTwitterImportExported == false && x.TweetType == "CharacterMentionedTweet")
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
                .Where(x => x.Exported == false && x.TweetType == "CharacterPersonaTweet")
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
                    .Where(cp => cp.CharacterId == character.Id && (cp.TwitterProfile == null || cp.TwitterProfile.LastImportDate < DateTime.UtcNow.AddDays(-10)))
                    .Take(50)
                    .ToListAsync();

                if (characterPersonas == null || !characterPersonas.Any())
                {
                    continue;
                }

                var imported = 0;
                var limit = new Random().Next(1, 10);
                var randomDelay = new Random().Next(300, 1500);

                foreach (var cpersona in characterPersonas)
                {
                    if (imported >= limit)
                    {
                        break;
                    }

                    if (cpersona.Persona == null)
                    {
                        continue;
                    }

                    if (cpersona.Persona.Platforms == null || !cpersona.Persona.Platforms.Any())
                    {
                        continue;
                    }

                    var twitterPlatform = cpersona.Persona.Platforms.FirstOrDefault(p => p.Platform.Name == "Twitter");

                    if (twitterPlatform == null)
                    {
                        continue;
                    }

                    var userSearch = twitterPlatform.PlatformPersonaId.StartsWith("@")
                        ? twitterPlatform.PlatformPersonaId.Substring(1)
                        : twitterPlatform.PlatformPersonaId;

                    using (var uow = _unitOfWorkManager.Begin())
                    {
                        TwitterScraperUserProfileResponse twitterProfile = null;
                        try
                        {
                            twitterProfile = await _twitterCommunicationService.GetUserProfileAsync(character.TwitterScrapeAgentId, userSearch);
                        }
                        catch (Exception ex)
                        {
                            var errorLog = new TwitterImportLog
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
                            };
                            await _twitterImportLogRepository.InsertAsync(errorLog);
                        }

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
                            var successLog = new TwitterImportLog
                            {
                                TenantId = character.TenantId,
                                CharacterId = character.Id,
                                TwitterAgentId = character.TwitterScrapeAgentId,
                                TaskName = "ProcessMissingTwitterProfiles",
                                Message = "Successfully fetched Twitter profile.",
                                LogLevel = "Information",
                                LoggedAt = DateTime.UtcNow
                            };
                            await _twitterImportLogRepository.InsertAsync(successLog);

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

        private MemoryStatsTwitter CreateMemoryStatsTwitter(TwitterImportTweet tweet)
        {
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
                TweetWordCount = tweet.Text.Split(' ').Length,
                MentionsCount = tweet.MentionsJson.ToString().Split('}').Length - 1
            };
        }

        private string FormatUsername(string username)
        {
            return username.StartsWith("@") ? username : "@" + username;
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

        public async Task ProcessCharacterPostTweets()
        {
            var characters = await _characterRepository.GetAllListAsync();
            var postTypes = new List<string> { "PostCharacterTweetAsync", "PostCharacterPersonaWelcomeTweet" };

            foreach (var character in characters)
            {
                var tasks = await _twitterImportTaskRepository.GetAllListAsync(x =>
                    x.CharacterId == character.Id &&
                    x.IsEnabled &&
                    x.NextRunTime <= DateTime.UtcNow &&
                    postTypes.Contains(x.TaskName));

                if (tasks == null || !tasks.Any())
                {
                    continue;
                }

                foreach (var task in tasks)
                {
                    if (task.TaskName == "PostCharacterTweetAsync")
                    {
                        await PostCharacterTweetAsync(task, character);
                    }
                    else if (task.TaskName == "PostCharacterPersonaWelcomeTweet")
                    {
                        await ProcessCharacterPersonaWelcomeTweet(task, character);
                    }
                }

                var importTask = await _twitterImportTaskRepository
                    .GetAll()
                    .Where(x =>
                        x.CharacterId == character.Id &&
                        x.IsEnabled &&
                        x.TaskName == "ImportTweetsOfCharacterAsync")
                    .FirstOrDefaultAsync();

                if (importTask != null)
                {
                    await ImportTweetsOfCharacterAsync(importTask, character);
                }
            }
        }

        private async Task PostCharacterTweetAsync(TwitterImportTask task, Character character)
        {
            var startTime = DateTime.UtcNow;

            using (var uow = _unitOfWorkManager.Begin())
            {
                try
                {
                    if (task == null || task.CharacterId == Guid.Empty || string.IsNullOrEmpty(character.TwitterPostAgentId))
                    {
                        task ??= new TwitterImportTask();
                        task.LastRunCompletionTime = null;

                        var invalidLog = new TwitterImportLog
                        {
                            TenantId = task.TenantId,
                            CharacterId = task.CharacterId,
                            TwitterAgentId = character.TwitterPostAgentId,
                            TaskName = task.TaskName,
                            Message = "PostCharacterTweetAsync: Invalid task or missing fields.",
                            LogLevel = "Warning",
                            LoggedAt = DateTime.UtcNow
                        };
                        await _twitterImportLogRepository.InsertAsync(invalidLog);

                        await _unitOfWorkManager.Current.SaveChangesAsync();
                        await uow.CompleteAsync();
                        return;
                    }

                    await _memoryManager.RunCharacterPostTweetPrompt(character.Id, AIModelType.DirectOpenAI);
                }
                catch (Exception ex)
                {
                    var errorLog = new TwitterImportLog
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
                    };
                    await _twitterImportLogRepository.InsertAsync(errorLog);

                    await _unitOfWorkManager.Current.SaveChangesAsync();
                    throw;
                };

                var endTime = DateTime.UtcNow;
                task.LastRunCompletionTime = endTime;
                task.LastRunStartTime = startTime;
                task.LastRunDurationSeconds = (int)(endTime - startTime).TotalSeconds;
                if (task.RunEveryXMinutes > 0)
                {
                    task.NextRunTime = endTime.AddMinutes(task.RunEveryXMinutes);
                }

                await _twitterImportTaskRepository.UpdateAsync(task);

                var successLog = new TwitterImportLog
                {
                    TenantId = task.TenantId,
                    CharacterId = task.CharacterId,
                    TwitterAgentId = character.TwitterPostAgentId,
                    TaskName = task.TaskName,
                    Message = "Successfully posted character tweet.",
                    LogLevel = "Information",
                    LoggedAt = DateTime.UtcNow
                };

                await _twitterImportLogRepository.InsertAsync(successLog);

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
                    if (task == null || task.CharacterId == Guid.Empty || string.IsNullOrEmpty(character.TwitterPostAgentId))
                    {
                        task ??= new TwitterImportTask();
                        task.LastRunCompletionTime = null;

                        var invalidLog = new TwitterImportLog
                        {
                            TenantId = task.TenantId,
                            CharacterId = task.CharacterId,
                            TwitterAgentId = character.TwitterPostAgentId,
                            TaskName = task.TaskName,
                            Message = "ProcessCharacterPersonaWelcomeTweet: Invalid task or missing fields.",
                            LogLevel = "Warning",
                            LoggedAt = DateTime.UtcNow
                        };
                        await _twitterImportLogRepository.InsertAsync(invalidLog);

                        await _unitOfWorkManager.Current.SaveChangesAsync();
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
                            x.WelcomeMessageSent == false &&
                            x.Persona.Platforms.Any(p => p.Platform.Name == "Twitter"))
                        .OrderBy(x => x.TwitterRank.Rank)
                        .Take(10)
                        .ToListAsync();

                    var limitToSend = 1;
                    var successCount = 0;

                    foreach (var characterPersona in characterPersonas)
                    {
                        if (successCount >= limitToSend) break;

                        var twitterPlatform = characterPersona.Persona.Platforms.FirstOrDefault(p => p.Platform.Name == "Twitter");
                        if (twitterPlatform == null) continue;

                        var userHandle = twitterPlatform.PlatformPersonaId;
                        var userSearch = userHandle.StartsWith("@") ? userHandle.Substring(1) : userHandle;
                        var avatarUrl = characterPersona.TwitterProfile?.Avatar ?? "https://abs.twimg.com/sticky/default_profile_images/default_profile_normal.png";
                        var userNameForQuery = Uri.EscapeDataString(userSearch);
                        var avatarUrlForQuery = Uri.EscapeDataString(avatarUrl);
                        var imageUrl = $"https://plant.fun/api/leaderboard/image?userName={userNameForQuery}&twitterAvatarUrl={avatarUrlForQuery}";

                        try
                        {
                            using (var httpClient = new HttpClient())
                            {
                                var imageBytes = await httpClient.GetByteArrayAsync(imageUrl);
                                var imageBase64 = Convert.ToBase64String(imageBytes);
                                var welcomeText = $"Welcome {userHandle} my new Gardener. Reply under this post with your wallet address for Rain(air)drops! Keep feeding me mentions on X and watch me grow. \uD83C\uDF35\uD83C\uDF89";

                                await _twitterCommunicationService.PostTweetWithImageAsync(
                                    character.TwitterPostAgentId,
                                    imageBase64,
                                    welcomeText
                                );

                                characterPersona.WelcomeMessageSent = true;
                                characterPersona.WelcomeMessageSentAt = DateTime.UtcNow;
                                await _characterPersonaRepository.UpdateAsync(characterPersona);

                                var successLog = new TwitterImportLog
                                {
                                    TenantId = characterPersona.TenantId,
                                    CharacterId = characterPersona.CharacterId,
                                    TwitterAgentId = character.TwitterPostAgentId,
                                    TaskName = task.TaskName,
                                    Message = $"Successfully posted welcome tweet to {userHandle}.",
                                    LogLevel = "Information",
                                    LoggedAt = DateTime.UtcNow
                                };
                                await _twitterImportLogRepository.InsertAsync(successLog);

                                successCount++;
                            }
                        }
                        catch (Exception ex)
                        {
                            var errorLog = new TwitterImportLog
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
                            };
                            await _twitterImportLogRepository.InsertAsync(errorLog);
                        }

                        await _unitOfWorkManager.Current.SaveChangesAsync();
                    }
                }
                catch (Exception ex)
                {
                    var errorLog = new TwitterImportLog
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
                    };
                    await _twitterImportLogRepository.InsertAsync(errorLog);

                    await _unitOfWorkManager.Current.SaveChangesAsync();
                    throw;
                }

                var endTime = DateTime.UtcNow;
                task.LastRunCompletionTime = endTime;
                task.LastRunStartTime = startTime;
                task.LastRunDurationSeconds = (int)(endTime - startTime).TotalSeconds;
                if (task.RunEveryXMinutes > 0) task.NextRunTime = endTime.AddMinutes(task.RunEveryXMinutes);

                await _twitterImportTaskRepository.UpdateAsync(task);

                var successLog2 = new TwitterImportLog
                {
                    TenantId = task.TenantId,
                    CharacterId = task.CharacterId,
                    TwitterAgentId = character.TwitterPostAgentId,
                    TaskName = task.TaskName,
                    Message = "Successfully posted persona welcome tweets.",
                    LogLevel = "Information",
                    LoggedAt = DateTime.UtcNow
                };
                await _twitterImportLogRepository.InsertAsync(successLog2);

                await _unitOfWorkManager.Current.SaveChangesAsync();
                await uow.CompleteAsync();
            }
        }



        public async Task ProcessCharacterTweetImports()
        {
            // Get all characters with Twitter import tasks
            var characters = await _characterRepository.GetAllListAsync();

            foreach (var character in characters)
            {
                var tasks = await _twitterImportTaskRepository.GetAllListAsync(x => x.CharacterId == character.Id && x.IsEnabled && x.NextRunTime <= DateTime.UtcNow);

                foreach (var task in tasks)
                {
                    if (task.TaskName == "ImportTweetsOfCharacterAsync")
                    {
                        await ImportTweetsOfCharacterAsync(task, character);
                    }
                    else if (task.TaskName == "ImportMentionsOfCharacterAsync")
                    {
                        await ImportMentionsOfCharacterAsync(task, character);
                    }
                    else if (task.TaskName == "ImportTweetsOfCharacterPersonasAsync")
                    {
                        await ImportTweetsOfCharacterPersonasAsync(task, character);
                    }
                }
            }
        }

        private async Task<TwitterImportTask> ImportTweetsOfCharacterAsync(TwitterImportTask task, Character character)
        {
            var startTime = DateTime.UtcNow;

            using (var uow = _unitOfWorkManager.Begin())
            {
                try
                {

                    // 1) Retrieve character             
                    if (character == null)
                    {
                        task.LastRunCompletionTime = null;
                        var noCharacterLog = new TwitterImportLog
                        {
                            TenantId = task.TenantId,
                            CharacterId = task.CharacterId,
                            TaskName = task.TaskName,
                            Message = "No matching character found in DB.",
                            LogLevel = "Warning",
                            LoggedAt = DateTime.UtcNow
                        };
                        await _twitterImportLogRepository.InsertAsync(noCharacterLog);

                        await _unitOfWorkManager.Current.SaveChangesAsync();
                        await uow.CompleteAsync();
                        return task;
                    }

                    // 2) Basic validations
                    if (task == null || task.CharacterId == Guid.Empty || string.IsNullOrEmpty(character?.TwitterScrapeAgentId))
                    {
                        task ??= new TwitterImportTask();
                        task.LastRunCompletionTime = null;

                        // Insert a log message if needed
                        var invalidLog = new TwitterImportLog
                        {
                            TenantId = task.TenantId,
                            CharacterId = task.CharacterId,
                            TwitterAgentId = character?.TwitterScrapeAgentId,
                            TaskName = task.TaskName,
                            Message = "ImportTweetsOfCharacterAsync: Invalid task or missing fields.",
                            LogLevel = "Warning",
                            LoggedAt = DateTime.UtcNow
                        };
                        await _twitterImportLogRepository.InsertAsync(invalidLog);

                        await _unitOfWorkManager.Current.SaveChangesAsync();
                        await uow.CompleteAsync();
                        return task;
                    }

                    // 3) Determine import limit
                    int importLimit = task.ImportLimitTotal > 0 ? task.ImportLimitTotal : 10;

                    // 4) Prepare username for search
                    var userSearch = character.TwitterUserName.StartsWith("@")
                        ? character.TwitterUserName.Substring(1)
                        : character.TwitterUserName;

                    // 5) Fetch tweets
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

                    // 7) Update task
                    var endTime = DateTime.UtcNow;
                    task.LastRunCompletionTime = endTime;
                    task.LastRunStartTime = startTime;
                    task.LastRunDurationSeconds = (int)(endTime - startTime).TotalSeconds;

                    // If you want to schedule the next run
                    if (task.RunEveryXMinutes > 0)
                    {
                        task.NextRunTime = endTime.AddMinutes(task.RunEveryXMinutes);
                    }

                    await _twitterImportTaskRepository.UpdateAsync(task);

                    // 8) Insert a "success" log
                    var successLog = new TwitterImportLog
                    {
                        TenantId = task.TenantId,
                        CharacterId = task.CharacterId,
                        TwitterAgentId = character.TwitterScrapeAgentId,
                        TaskName = task.TaskName,
                        Message = $"Successfully imported {tweets.Count} tweets.",
                        LogLevel = "Information",
                        LoggedAt = DateTime.UtcNow
                    };
                    await _twitterImportLogRepository.InsertAsync(successLog);

                    // 9) Save & commit the UOW
                    await _unitOfWorkManager.Current.SaveChangesAsync();
                    await uow.CompleteAsync();

                    return task;
                }
                catch (Exception ex)
                {
                    // 10) Insert an "error" log
                    var errorLog = new TwitterImportLog
                    {
                        TenantId = task?.TenantId ?? 0,  // or handle null
                        CharacterId = task?.CharacterId ?? Guid.Empty,
                        TwitterAgentId = character?.TwitterScrapeAgentId,
                        TaskName = task?.TaskName,
                        Message = "Import failed.",
                        LogLevel = "Error",
                        Exception = ex.ToString(),
                        ExceptionMessage = ex.Message,
                        LoggedAt = DateTime.UtcNow
                    };
                    await _twitterImportLogRepository.InsertAsync(errorLog);

                    await _unitOfWorkManager.Current.SaveChangesAsync();
                    // Re-throw the exception (or handle it as you prefer)
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
                    // 1) Retrieve character                    
                    if (character == null)
                    {
                        task.LastRunCompletionTime = null;

                        var noCharacterLog = new TwitterImportLog
                        {
                            TenantId = task.TenantId,
                            CharacterId = task.CharacterId,
                            TwitterAgentId = character.TwitterScrapeAgentId,
                            TaskName = task.TaskName,
                            Message = "No matching character found in DB.",
                            LogLevel = "Warning",
                            LoggedAt = DateTime.UtcNow
                        };
                        await _twitterImportLogRepository.InsertAsync(noCharacterLog);

                        await _unitOfWorkManager.Current.SaveChangesAsync();
                        await uow.CompleteAsync();
                        return task;
                    }

                    // 2) Basic validations
                    if (task == null || task.CharacterId == Guid.Empty || string.IsNullOrEmpty(character?.TwitterScrapeAgentId))
                    {
                        task ??= new TwitterImportTask();
                        task.LastRunCompletionTime = null;

                        var invalidLog = new TwitterImportLog
                        {
                            TenantId = task.TenantId,
                            CharacterId = task.CharacterId,
                            TwitterAgentId = character?.TwitterScrapeAgentId,
                            TaskName = task.TaskName,
                            Message = "ImportMentionsOfCharacterAsync: Invalid task or missing fields.",
                            LogLevel = "Warning",
                            LoggedAt = DateTime.UtcNow
                        };
                        await _twitterImportLogRepository.InsertAsync(invalidLog);

                        await _unitOfWorkManager.Current.SaveChangesAsync();
                        await uow.CompleteAsync();
                        return task;
                    }

                    // 3) Possibly remove '@'
                    var userSearch = character.TwitterUserName.StartsWith("@")
                        ? character.TwitterUserName.Substring(1)
                        : character.TwitterUserName;

                    // 4) Fetch mentions via the comm. service
                    var mentions = await _twitterCommunicationService.GetUserMentionsAsync(
                        character.TwitterScrapeAgentId,
                        userSearch,
                        task.ImportLimitTotal
                    );

                    // 5) Upsert each mention with a different TweetType
                    foreach (var tweet in mentions)
                    {
                        await MapAndUpsertTweetAsync(
                            tweet,
                            task,
                            character.Id,
                            character.Name,
                            "CharacterMentionedTweet" // or "CharacterMentionedTweet"
                        );
                    }

                    // 6) Update the import task
                    var endTime = DateTime.UtcNow;
                    task.LastRunCompletionTime = endTime;
                    task.LastRunStartTime = startTime;
                    task.LastRunDurationSeconds = (int)(endTime - startTime).TotalSeconds;
                    if (task.RunEveryXMinutes > 0)
                    {
                        task.NextRunTime = endTime.AddMinutes(task.RunEveryXMinutes);
                    }

                    await _twitterImportTaskRepository.UpdateAsync(task);

                    // 7) Log success
                    var successLog = new TwitterImportLog
                    {
                        TenantId = task.TenantId,
                        CharacterId = task.CharacterId,
                        TwitterAgentId = character.TwitterScrapeAgentId,
                        TaskName = task.TaskName,
                        Message = "Successfully imported mentions for character.",
                        LogLevel = "Information",
                        LoggedAt = DateTime.UtcNow
                    };
                    await _twitterImportLogRepository.InsertAsync(successLog);

                    await _unitOfWorkManager.Current.SaveChangesAsync();
                    await uow.CompleteAsync();

                    return task;
                }
                catch (Exception ex)
                {
                    // 8) Error log
                    var errorLog = new TwitterImportLog
                    {
                        TenantId = task?.TenantId ?? 0,
                        CharacterId = task?.CharacterId ?? Guid.Empty,
                        TwitterAgentId = character?.TwitterScrapeAgentId,
                        TaskName = task?.TaskName,
                        Message = "ImportMentionsOfCharacter failed.",
                        LogLevel = "Error",
                        Exception = ex.ToString(),
                        ExceptionMessage = ex.Message,
                        LoggedAt = DateTime.UtcNow
                    };
                    await _twitterImportLogRepository.InsertAsync(errorLog);

                    await _unitOfWorkManager.Current.SaveChangesAsync();
                    throw;
                }
            }
        }

        private async Task<TwitterImportTask> ImportTweetsOfCharacterPersonasAsync(TwitterImportTask task, Character character)
        {
            var startTime = DateTime.UtcNow;

            using (var uow = _unitOfWorkManager.Begin())
            {
                try
                {
                    // 1) Retrieve character                    
                    if (character == null)
                    {
                        task.LastRunCompletionTime = null;

                        var noCharacterLog = new TwitterImportLog
                        {
                            TenantId = task.TenantId,
                            CharacterId = task.CharacterId,
                            TwitterAgentId = character?.TwitterScrapeAgentId,
                            TaskName = task.TaskName,
                            Message = "No matching character found in DB.",
                            LogLevel = "Warning",
                            LoggedAt = DateTime.UtcNow
                        };
                        await _twitterImportLogRepository.InsertAsync(noCharacterLog);

                        await _unitOfWorkManager.Current.SaveChangesAsync();
                        await uow.CompleteAsync();
                        return task;
                    }


                    // 2) Basic validations
                    if (task == null || task.CharacterId == Guid.Empty || string.IsNullOrEmpty(character.TwitterScrapeAgentId))
                    {
                        task ??= new TwitterImportTask();
                        task.LastRunCompletionTime = null;

                        // Insert a log message
                        var invalidLog = new TwitterImportLog
                        {
                            TenantId = task.TenantId,
                            CharacterId = task.CharacterId,
                            TwitterAgentId = character?.TwitterScrapeAgentId,
                            TaskName = task.TaskName,
                            Message = "ImportTweetsOfCharacterPersonasAsync: Invalid task or missing fields.",
                            LogLevel = "Warning",
                            LoggedAt = DateTime.UtcNow
                        };
                        await _twitterImportLogRepository.InsertAsync(invalidLog);

                        await _unitOfWorkManager.Current.SaveChangesAsync();
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
                        // no persona found
                        task.LastRunCompletionTime = DateTime.UtcNow;
                        await _unitOfWorkManager.Current.SaveChangesAsync();
                        await uow.CompleteAsync();
                        return task;
                    }

                    // personas = list wheere platform is twitter
                    characterPersonas = characterPersonas.Where(cp => cp.Persona.Platforms.Any(p => p.Platform.Name == "Twitter")).ToList();
                    var personas = characterPersonas.Select(cp => cp.Persona).ToList();

                    if (personas == null || !personas.Any())
                    {
                        // no persona found
                        task.LastRunCompletionTime = DateTime.UtcNow;
                        await _unitOfWorkManager.Current.SaveChangesAsync();
                        await uow.CompleteAsync();
                        return task;
                    }

                    // 4) For each persona, fetch the tweets and map them
                    foreach (var persona in personas)
                    {
                        var twitterPlatform = persona.Platforms.FirstOrDefault(p => p.Platform.Name == "Twitter");

                        if (twitterPlatform == null)
                        {
                            // no twitter platform found
                            continue;
                        }

                        // Possibly strip the '@'
                        var userSearch = twitterPlatform.PlatformPersonaId.StartsWith("@")
                            ? twitterPlatform.PlatformPersonaId.Substring(1)
                            : twitterPlatform.PlatformPersonaId;

                        // Example: use the same "GetTweetsAsync"
                        // Or if you have multiple "agents," pass in the persona's agent
                        var tweets = await _twitterCommunicationService.GetTweetsAsync(
                            character.TwitterScrapeAgentId,
                            userSearch,
                            task.ImportLimitTotal > 0 ? task.ImportLimitTotal : 10
                        );

                        // 5) Upsert each tweet into the same table, but different TweetType
                        foreach (var tweet in tweets)
                        {
                            // Re-use the shared helper
                            await MapAndUpsertTweetAsync(
                                tweet,
                                task,
                                character.Id,
                                character.Name,
                                "CharacterPersonaTweet"
                            );
                        }
                    }

                    // 6) Update the import task
                    var endTime = DateTime.UtcNow;
                    task.LastRunCompletionTime = endTime;
                    task.LastRunStartTime = startTime;
                    task.LastRunDurationSeconds = (int)(endTime - startTime).TotalSeconds;
                    if (task.RunEveryXMinutes > 0)
                    {
                        task.NextRunTime = endTime.AddMinutes(task.RunEveryXMinutes);
                    }

                    await _twitterImportTaskRepository.UpdateAsync(task);

                    // 7) Success log
                    var successLog = new TwitterImportLog
                    {
                        TenantId = task.TenantId,
                        CharacterId = task.CharacterId,
                        TwitterAgentId = character.TwitterScrapeAgentId,
                        TaskName = task.TaskName,
                        Message = "Successfully imported tweets for character personas.",
                        LogLevel = "Information",
                        LoggedAt = DateTime.UtcNow
                    };
                    await _twitterImportLogRepository.InsertAsync(successLog);

                    await _unitOfWorkManager.Current.SaveChangesAsync();
                    await uow.CompleteAsync();

                    return task;
                }
                catch (Exception ex)
                {
                    // 8) Error log
                    var errorLog = new TwitterImportLog
                    {
                        TenantId = task?.TenantId ?? 0,
                        CharacterId = task?.CharacterId ?? Guid.Empty,
                        TwitterAgentId = character?.TwitterScrapeAgentId,
                        TaskName = task?.TaskName,
                        Message = "ImportCharacterPersonas failed.",
                        LogLevel = "Error",
                        Exception = ex.ToString(),
                        ExceptionMessage = ex.Message,
                        LoggedAt = DateTime.UtcNow
                    };
                    await _twitterImportLogRepository.InsertAsync(errorLog);

                    await _unitOfWorkManager.Current.SaveChangesAsync();
                    throw;
                }
            }
        }

        private async Task<TwitterImportTweet> MapAndUpsertTweetAsync(TwitterScraperTweetResponse tweet, TwitterImportTask importTask, Guid characterId, string characterName, string tweetType)
        {
            var tweetId = tweet.Id ?? string.Empty;

            // Check if it already exists
            var existingEntity = await _importTweetRepository.FirstOrDefaultAsync(x =>
                x.TweetId == tweetId && x.CharacterId == characterId);

            // Create new if needed
            if (existingEntity == null)
            {
                existingEntity = new TwitterImportTweet
                {
                    TenantId = importTask.TenantId,           // from your task
                    TweetType = tweetType,                    // "CharacterPersona" or "CharacterMention", etc.
                    CharacterId = characterId,
                    CharacterName = characterName,
                    TweetId = tweetId,
                    LastTwitterImportDate = DateTime.UtcNow,
                    LastTwitterImportExported = false
                };
            }
            // else
            // {
            //     // If it's not transient, we simply update fields below
            //     // preserving the PK / existing fields.
            //     existingEntity.TweetType = tweetType; // possibly override the type
            // }

            // ----- Arrays -> CSV strings
            existingEntity.Hashtags = tweet.Hashtags != null
                ? string.Join(",", tweet.Hashtags)
                : string.Empty;
            existingEntity.Urls = tweet.Urls != null
                ? string.Join(",", tweet.Urls)
                : string.Empty;

            // ----- Objects -> JSON
            existingEntity.MentionsJson = tweet.Mentions != null
                ? JsonSerializer.Serialize(tweet.Mentions)
                : string.Empty;
            existingEntity.PhotosJson = tweet.Photos != null
                ? JsonSerializer.Serialize(tweet.Photos)
                : string.Empty;
            existingEntity.VideosJson = tweet.Videos != null
                ? JsonSerializer.Serialize(tweet.Videos)
                : string.Empty;
            existingEntity.PlaceJson = tweet.Place != null
                ? JsonSerializer.Serialize(tweet.Place)
                : string.Empty;
            existingEntity.PollJson = tweet.Poll != null
                ? JsonSerializer.Serialize(tweet.Poll)
                : string.Empty;
            existingEntity.InReplyToStatusJson = tweet.InReplyToStatus != null
                ? JsonSerializer.Serialize(tweet.InReplyToStatus)
                : string.Empty;
            existingEntity.QuotedStatusJson = tweet.QuotedStatus != null
                ? JsonSerializer.Serialize(tweet.QuotedStatus)
                : string.Empty;
            existingEntity.RetweetedStatusJson = tweet.RetweetedStatus != null
                ? JsonSerializer.Serialize(tweet.RetweetedStatus)
                : string.Empty;
            existingEntity.ThreadJson = tweet.Thread != null
                ? JsonSerializer.Serialize(tweet.Thread)
                : string.Empty;

            // ----- Other scalars
            existingEntity.BookmarkCount = tweet.BookmarkCount ?? 0;
            existingEntity.ConversationId = tweet.ConversationId ?? string.Empty;
            existingEntity.Html = tweet.Html ?? string.Empty;
            existingEntity.InReplyToStatusId = tweet.InReplyToStatusId ?? string.Empty;
            existingEntity.IsQuoted = tweet.IsQuoted ?? false;
            existingEntity.IsPin = tweet.IsPin ?? false;
            existingEntity.IsReply = tweet.IsReply ?? false;
            existingEntity.IsRetweet = tweet.IsRetweet ?? false;
            existingEntity.IsSelfThread = tweet.IsSelfThread ?? false;
            existingEntity.Likes = tweet.Likes ?? 0;
            existingEntity.Name = tweet.Name ?? string.Empty;
            existingEntity.PermanentUrl = tweet.PermanentUrl ?? string.Empty;
            existingEntity.QuotedStatusId = tweet.QuotedStatusId ?? string.Empty;
            existingEntity.Replies = tweet.Replies ?? 0;
            existingEntity.Retweets = tweet.Retweets ?? 0;
            existingEntity.RetweetedStatusId = tweet.RetweetedStatusId ?? string.Empty;
            existingEntity.Text = tweet.Text ?? string.Empty;
            existingEntity.TimeParsed = tweet.TimeParsed ?? DateTime.UtcNow;
            existingEntity.Timestamp = tweet.Timestamp ?? 0;
            existingEntity.UserId = tweet.UserId ?? string.Empty;
            existingEntity.Username = tweet.Username ?? string.Empty;
            existingEntity.Views = tweet.Views ?? 0;
            existingEntity.SensitiveContent = tweet.SensitiveContent ?? false;
            existingEntity.LastTwitterImportDate = DateTime.UtcNow;

            // ----- Insert or Update
            if (existingEntity.IsTransient())
            {
                await _importTweetRepository.InsertAsync(existingEntity);
            }
            else
            {
                await _importTweetRepository.UpdateAsync(existingEntity);
            }

            return existingEntity;
        }


        // public async Task ProccessCharacterTweetsStorage()
        // {
        //     var characterTweets = await _importTweetRepository
        //         .GetAll()
        //         .Where(x => x.Exported == false && x.TweetType == "CharacterTweet")
        //         .ToListAsync();

        //     foreach (var tweet in characterTweets)
        //     {
        //         await _memoryManager.StoreCharacterTweets(
        //             characterId: tweet.CharacterId,
        //             conversationId: tweet.ConversationId,
        //             tweetId: tweet.TweetId,
        //             tweetContent: tweet.Text,
        //             tweetUrl: tweet.PermanentUrl,
        //             platformInteractionDate: tweet.TimeParsed
        //         );

        //         await UpdateImportTweetExportedStatus(tweet);
        //     }

        //     var characterMentionedTweetsNew = await _importTweetRepository
        //         .GetAll()
        //         .Where(x => x.Exported == false && x.TweetType == "CharacterMentionedTweet")
        //         .ToListAsync();

        //     foreach (var tweet in characterMentionedTweetsNew)
        //     {
        //         var memoryStatsTwitter = new MemoryStatsTwitter
        //         {
        //             TenantId = tweet.TenantId,
        //             IsPin = tweet.IsPin,
        //             IsQuoted = tweet.IsQuoted,
        //             IsReply = tweet.IsReply,
        //             IsRetweet = tweet.IsRetweet,
        //             SensitiveContent = tweet.SensitiveContent,
        //             BookmarkCount = tweet.BookmarkCount,
        //             Likes = tweet.Likes,
        //             Replies = tweet.Replies,
        //             Retweets = tweet.Retweets,
        //             Views = tweet.Views,
        //             TweetWordCount = tweet.Text.Split(' ').Length,
        //             MentionsCount = tweet.MentionsJson.ToString().Split('}').Length - 1
        //         };

        //         var username = tweet.Username;
        //         if (!username.StartsWith("@"))
        //             username = "@" + username;

        //         await _memoryManager.StoreCharacterMentionedTweets(
        //             characterId: tweet.CharacterId,
        //             mentionByPersonaPlatformId: username,
        //             mentionedByPersonaName: tweet.Name,
        //             conversationId: tweet.ConversationId,
        //             tweetId: tweet.TweetId,
        //             tweetContent: tweet.Text,
        //             tweetUrl: tweet.PermanentUrl,
        //             platformInteractionDate: tweet.TimeParsed,
        //             memoryStatsTwitter: memoryStatsTwitter
        //         );

        //         await UpdateImportTweetExportedStatus(tweet);
        //     }

        //     var characterMentiondTweetsUpdated = await _importTweetRepository
        //         .GetAll()
        //         .Where(x => x.Exported == true && x.LastTwitterImportExported == false && x.TweetType == "CharacterMentionedTweet")
        //         .ToListAsync();

        //     foreach (var tweet in characterMentiondTweetsUpdated)
        //     {
        //         var memoryStatsTwitter = new MemoryStatsTwitter
        //         {
        //             TenantId = tweet.TenantId,
        //             IsPin = tweet.IsPin,
        //             IsQuoted = tweet.IsQuoted,
        //             IsReply = tweet.IsReply,
        //             IsRetweet = tweet.IsRetweet,
        //             SensitiveContent = tweet.SensitiveContent,
        //             BookmarkCount = tweet.BookmarkCount,
        //             Likes = tweet.Likes,
        //             Replies = tweet.Replies,
        //             Retweets = tweet.Retweets,
        //             Views = tweet.Views,
        //             TweetWordCount = tweet.Text.Split(' ').Length,
        //             MentionsCount = tweet.MentionsJson.ToString().Split('}').Length - 1
        //         };

        //         var username = tweet.Username;
        //         if (!username.StartsWith("@"))
        //             username = "@" + username;

        //         await _memoryManager.StoreCharacterMentionedTweets(
        //             characterId: tweet.CharacterId,
        //             mentionByPersonaPlatformId: username,
        //             mentionedByPersonaName: tweet.Name,
        //             conversationId: tweet.ConversationId,
        //             tweetId: tweet.TweetId,
        //             tweetContent: tweet.Text,
        //             tweetUrl: tweet.PermanentUrl,
        //             platformInteractionDate: tweet.TimeParsed,
        //             memoryStatsTwitter: memoryStatsTwitter
        //         );

        //         await UpdateImportTweetLastExportedStatus(tweet);
        //     }

        //     var characterPersonaTweets = await _importTweetRepository
        //         .GetAll()
        //         .Where(x => x.LastTwitterImportExported == false && x.TweetType == "CharacterPersonaTweet")
        //         .ToListAsync();

        //     foreach (var tweet in characterPersonaTweets)
        //     {
        //         var username = tweet.Username;
        //         if (!username.StartsWith("@"))
        //             username = "@" + username;

        //         await _memoryManager.StoreCharacterPersonaTweets(
        //             characterId: tweet.CharacterId,
        //             personaPlatformId: username,
        //             personaName: tweet.Name,
        //             conversationId: tweet.ConversationId,
        //             tweetId: tweet.TweetId,
        //             tweetContent: tweet.Text,
        //             tweetUrl: tweet.PermanentUrl,
        //             platformInteractionDate: tweet.TimeParsed
        //         );

        //         await UpdateImportTweetExportedStatus(tweet);
        //     }
        // }

    }
}
