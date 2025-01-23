// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Net.Http;
// using System.Text.Json;
// using System.Threading.Tasks;
// using Abp.Dependency;
// using Abp.Domain.Repositories;
// using Abp.Domain.Uow;
// using Icon.Matrix.AIManager;
// using Icon.Matrix.Enums;
// using Icon.Matrix.Models;
// using Icon.Matrix.TwitterManager;
// using Microsoft.EntityFrameworkCore;
// using PayPalCheckoutSdk.Orders;

// namespace Icon.Matrix.Twitter
// {
//     public interface ITwitterTaskManager
//     {
//         Task ProcessCharacterTweetImports();
//         Task ProcessCharacterTweetsStorage();
//         Task ProcessCharacterPostTweets();
//         Task ProcessMissingTwitterProfiles();
//     }

//     public class TwitterTaskManager : ITwitterTaskManager, ITransientDependency
//     {
//         private readonly ITwitterCommunicationService _twitterCommunicationService;
//         private readonly IRepository<TwitterImportTweet, Guid> _importTweetRepository;
//         private readonly IRepository<Character, Guid> _characterRepository;
//         private readonly IRepository<CharacterPersona, Guid> _characterPersonaRepository;
//         private readonly IRepository<TwitterImportTask, Guid> _twitterImportTaskRepository;
//         private readonly IRepository<TwitterImportLog, Guid> _twitterImportLogRepository;
//         private readonly IMemoryManager _memoryManager;
//         private readonly IAIManager _aiManager;
//         private readonly IUnitOfWorkManager _unitOfWorkManager;

//         public TwitterTaskManager(
//             ITwitterCommunicationService twitterCommunicationService,
//             IRepository<TwitterImportTweet, Guid> importTweetRepository,
//             IRepository<Character, Guid> characterRepository,
//             IRepository<CharacterPersona, Guid> characterPersonaRepository,
//             IRepository<TwitterImportTask, Guid> twitterImportTaskRepository,
//             IRepository<TwitterImportLog, Guid> twitterImportLogRepository,
//             IMemoryManager memoryManager,
//             IAIManager aiManager,
//             IUnitOfWorkManager unitOfWorkManager)
//         {
//             _twitterCommunicationService = twitterCommunicationService;
//             _importTweetRepository = importTweetRepository;
//             _characterRepository = characterRepository;
//             _characterPersonaRepository = characterPersonaRepository;
//             _twitterImportTaskRepository = twitterImportTaskRepository;
//             _twitterImportLogRepository = twitterImportLogRepository;
//             _memoryManager = memoryManager;
//             _aiManager = aiManager;
//             _unitOfWorkManager = unitOfWorkManager;
//         }

//         public async Task ProcessCharacterTweetsStorage()
//         {
//             await ProcessNewCharacterTweets();
//             await ProcessUpdatedCharacterMentionedTweets();
//             await ProcessNewCharacterMentionedTweets();
//             await ProcessNewCharacterPersonaTweets();
//         }

//         private async Task ProcessNewCharacterTweets()
//         {
//             var characterTweets = await _importTweetRepository
//                 .GetAll()
//                 .Where(x => x.Exported == false && x.TweetType == "CharacterTweet")
//                 .Take(50)
//                 .ToListAsync();

//             foreach (var tweet in characterTweets)
//             {
//                 await _memoryManager.StoreCharacterTweets(
//                     characterId: tweet.CharacterId,
//                     conversationId: tweet.ConversationId,
//                     tweetId: tweet.TweetId,
//                     tweetContent: tweet.Text,
//                     tweetUrl: tweet.PermanentUrl,
//                     platformInteractionDate: tweet.TimeParsed
//                 );

//                 await UpdateImportTweetExportedStatus(tweet);
//             }
//         }

//         private async Task ProcessNewCharacterMentionedTweets()
//         {
//             var characterMentionedTweetsNew = await _importTweetRepository
//                 .GetAll()
//                 .Where(x => x.Exported == false && x.TweetType == "CharacterMentionedTweet")
//                 .Take(50)
//                 .ToListAsync();

//             foreach (var tweet in characterMentionedTweetsNew)
//             {
//                 using (var uow = _unitOfWorkManager.Begin())
//                 {

//                     var memoryStatsTwitter = CreateMemoryStatsTwitter(tweet);
//                     var username = FormatUsername(tweet.Username);

//                     await _memoryManager.StoreCharacterMentionedTweets(
//                         characterId: tweet.CharacterId,
//                         mentionByPersonaPlatformId: username,
//                         mentionedByPersonaName: tweet.Name,
//                         conversationId: tweet.ConversationId,
//                         tweetId: tweet.TweetId,
//                         tweetContent: tweet.Text,
//                         tweetUrl: tweet.PermanentUrl,
//                         platformInteractionDate: tweet.TimeParsed,
//                         memoryStatsTwitter: memoryStatsTwitter
//                     );

//                     await UpdateImportTweetExportedStatus(tweet);

//                     await _unitOfWorkManager.Current.SaveChangesAsync();
//                     await uow.CompleteAsync();
//                 }
//             }
//         }

//         private async Task ProcessUpdatedCharacterMentionedTweets()
//         {
//             var characterMentionedTweetsUpdated = await _importTweetRepository
//                 .GetAll()
//                 .Where(x => x.Exported == true && x.LastTwitterImportExported == false && x.TweetType == "CharacterMentionedTweet")
//                 .Take(50)
//                 .ToListAsync();

//             foreach (var tweet in characterMentionedTweetsUpdated)
//             {
//                 var memoryStatsTwitter = CreateMemoryStatsTwitter(tweet);
//                 var username = FormatUsername(tweet.Username);

//                 await _memoryManager.StoreCharacterMentionedTweets(
//                     characterId: tweet.CharacterId,
//                     mentionByPersonaPlatformId: username,
//                     mentionedByPersonaName: tweet.Name,
//                     conversationId: tweet.ConversationId,
//                     tweetId: tweet.TweetId,
//                     tweetContent: tweet.Text,
//                     tweetUrl: tweet.PermanentUrl,
//                     platformInteractionDate: tweet.TimeParsed,
//                     memoryStatsTwitter: memoryStatsTwitter
//                 );

//                 await UpdateImportTweetLastExportedStatus(tweet);
//             }
//         }

//         private async Task ProcessNewCharacterPersonaTweets()
//         {
//             var characterPersonaTweets = await _importTweetRepository
//                 .GetAll()
//                 .Where(x => x.Exported == false && x.TweetType == "CharacterPersonaTweet")
//                 .Take(50)
//                 .ToListAsync();

//             foreach (var tweet in characterPersonaTweets)
//             {
//                 var username = FormatUsername(tweet.Username);

//                 await _memoryManager.StoreCharacterPersonaTweets(
//                     characterId: tweet.CharacterId,
//                     personaPlatformId: username,
//                     personaName: tweet.Name,
//                     conversationId: tweet.ConversationId,
//                     tweetId: tweet.TweetId,
//                     tweetContent: tweet.Text,
//                     tweetUrl: tweet.PermanentUrl,
//                     platformInteractionDate: tweet.TimeParsed
//                 );

//                 await UpdateImportTweetExportedStatus(tweet);
//             }
//         }

//         public async Task ProcessMissingTwitterProfiles()
//         {
//             var characters = await _characterRepository
//                 .GetAll()
//                 .Where(c => c.IsTwitterScrapingEnabled && !string.IsNullOrEmpty(c.TwitterScrapeAgentId))
//                 .ToListAsync();

//             foreach (var character in characters)
//             {
//                 var characterPersonas = await _characterPersonaRepository
//                     .GetAll()
//                     .Include(cp => cp.TwitterProfile)
//                     .Include(cp => cp.Persona)
//                     .ThenInclude(p => p.Platforms)
//                     .ThenInclude(p => p.Platform)
//                     .Where(cp => cp.CharacterId == character.Id && (cp.TwitterProfile == null || cp.TwitterProfile.LastImportDate < DateTime.UtcNow.AddDays(-10)))
//                     .Take(50)
//                     .ToListAsync();

//                 if (characterPersonas == null || !characterPersonas.Any())
//                 {
//                     continue;
//                 }

//                 var imported = 0;
//                 var limit = new Random().Next(1, 10);
//                 var randomDelay = new Random().Next(300, 1500);

//                 foreach (var cpersona in characterPersonas)
//                 {
//                     if (imported >= limit)
//                     {
//                         break;
//                     }

//                     if (cpersona.Persona == null)
//                     {
//                         continue;
//                     }

//                     if (cpersona.Persona.Platforms == null || !cpersona.Persona.Platforms.Any())
//                     {
//                         continue;
//                     }

//                     var twitterPlatform = cpersona.Persona.Platforms.FirstOrDefault(p => p.Platform.Name == "Twitter");
//                     if (twitterPlatform == null)
//                     {
//                         continue;
//                     }

//                     var userSearch = twitterPlatform.PlatformPersonaId.StartsWith("@")
//                         ? twitterPlatform.PlatformPersonaId.Substring(1)
//                         : twitterPlatform.PlatformPersonaId;

//                     using (var uow = _unitOfWorkManager.Begin())
//                     {
//                         TwitterScraperUserProfileResponse twitterProfile = null;
//                         try
//                         {
//                             twitterProfile = await _twitterCommunicationService.GetUserProfileAsync(character.TwitterScrapeAgentId, userSearch);
//                         }
//                         catch (Exception ex)
//                         {
//                             var errorLog = new TwitterImportLog
//                             {
//                                 TenantId = character.TenantId,
//                                 CharacterId = character.Id,
//                                 TwitterAgentId = character.TwitterScrapeAgentId,
//                                 TaskName = "ProcessMissingTwitterProfiles",
//                                 Message = "GetUserProfileAsync failed.",
//                                 LogLevel = "Error",
//                                 Exception = ex.ToString(),
//                                 ExceptionMessage = ex.Message,
//                                 LoggedAt = DateTime.UtcNow
//                             };
//                             await _twitterImportLogRepository.InsertAsync(errorLog);
//                         }

//                         if (twitterProfile == null)
//                         {
//                             cpersona.TwitterProfile = new CharacterPersonaTwitterProfile
//                             {
//                                 CharacterPersonaId = cpersona.Id,
//                                 LastImportDate = DateTime.UtcNow
//                             };
//                         }
//                         else
//                         {
//                             var successLog = new TwitterImportLog
//                             {
//                                 TenantId = character.TenantId,
//                                 CharacterId = character.Id,
//                                 TwitterAgentId = character.TwitterScrapeAgentId,
//                                 TaskName = "ProcessMissingTwitterProfiles",
//                                 Message = "Successfully fetched Twitter profile.",
//                                 LogLevel = "Information",
//                                 LoggedAt = DateTime.UtcNow
//                             };
//                             await _twitterImportLogRepository.InsertAsync(successLog);

//                             cpersona.TwitterProfile = new CharacterPersonaTwitterProfile
//                             {
//                                 CharacterPersonaId = cpersona.Id,
//                                 Avatar = twitterProfile.Avatar,
//                                 Biography = twitterProfile.Biography,
//                                 FollowersCount = twitterProfile.FollowersCount,
//                                 FollowingCount = twitterProfile.FollowingCount,
//                                 FriendsCount = twitterProfile.FriendsCount,
//                                 MediaCount = twitterProfile.MediaCount,
//                                 IsPrivate = twitterProfile.IsPrivate,
//                                 IsVerified = twitterProfile.IsVerified,
//                                 LikesCount = twitterProfile.LikesCount,
//                                 ListedCount = twitterProfile.ListedCount,
//                                 Location = twitterProfile.Location,
//                                 Name = twitterProfile.Name,
//                                 PinnedTweetIds = twitterProfile.PinnedTweetIds,
//                                 TweetsCount = twitterProfile.TweetsCount,
//                                 Url = twitterProfile.Url,
//                                 UserId = twitterProfile.UserId,
//                                 Username = twitterProfile.Username,
//                                 IsBlueVerified = twitterProfile.IsBlueVerified,
//                                 CanDm = twitterProfile.CanDm,
//                                 Joined = twitterProfile.Joined,
//                                 LastImportDate = DateTime.UtcNow
//                             };
//                         }

//                         imported++;
//                         await Task.Delay(randomDelay);
//                         await _characterPersonaRepository.UpdateAsync(cpersona);
//                         await _unitOfWorkManager.Current.SaveChangesAsync();
//                         await uow.CompleteAsync();
//                     }
//                 }
//             }
//         }

//         public async Task ProcessCharacterPostTweets()
//         {
//             var characters = await _characterRepository.GetAllListAsync();
//             var postTypes = new List<string> { "PostCharacterTweetAsync", "PostCharacterPersonaWelcomeTweet", "PostCharacterTweetRisingSproutAsync" };

//             foreach (var character in characters)
//             {
//                 var tasks = await _twitterImportTaskRepository.GetAllListAsync(x =>
//                     x.CharacterId == character.Id &&
//                     x.IsEnabled &&
//                     x.NextRunTime <= DateTime.UtcNow &&
//                     postTypes.Contains(x.TaskName));

//                 if (tasks == null || !tasks.Any())
//                 {
//                     continue;
//                 }

//                 foreach (var task in tasks)
//                 {
//                     if (task.TaskName == "PostCharacterTweetAsync")
//                     {
//                         await PostCharacterTweetAsync(task, character);
//                     }
//                     else if (task.TaskName == "PostCharacterPersonaWelcomeTweet")
//                     {
//                         await ProcessCharacterPersonaWelcomeTweet(task, character);
//                     }
//                     else if (task.TaskName == "PostCharacterTweetRisingSproutAsync")
//                     {
//                         await PostCharacterTweetRisingSproutAsync(task, character);
//                     }
//                 }

//                 var importTask = await _twitterImportTaskRepository
//                     .GetAll()
//                     .Where(x =>
//                         x.CharacterId == character.Id &&
//                         x.IsEnabled &&
//                         x.TaskName == "ImportTweetsOfCharacterAsync")
//                     .FirstOrDefaultAsync();

//                 if (importTask != null)
//                 {
//                     await ImportTweetsOfCharacterAsync(importTask, character);
//                 }
//             }
//         }

//         private async Task PostCharacterTweetAsync(TwitterImportTask task, Character character)
//         {
//             var startTime = DateTime.UtcNow;

//             using (var uow = _unitOfWorkManager.Begin())
//             {
//                 try
//                 {
//                     if (task == null || task.CharacterId == Guid.Empty || string.IsNullOrEmpty(character.TwitterPostAgentId))
//                     {
//                         task ??= new TwitterImportTask();
//                         task.LastRunCompletionTime = null;

//                         var invalidLog = new TwitterImportLog
//                         {
//                             TenantId = task.TenantId,
//                             CharacterId = task.CharacterId,
//                             TwitterAgentId = character.TwitterPostAgentId,
//                             TaskName = task.TaskName,
//                             Message = "PostCharacterTweetAsync: Invalid task or missing fields.",
//                             LogLevel = "Warning",
//                             LoggedAt = DateTime.UtcNow
//                         };
//                         await _twitterImportLogRepository.InsertAsync(invalidLog);

//                         await _unitOfWorkManager.Current.SaveChangesAsync();
//                         await uow.CompleteAsync();
//                         return;
//                     }

//                     await _memoryManager.RunCharacterPostTweetPrompt(character.Id, AIModelType.DirectOpenAI);
//                 }
//                 catch (Exception ex)
//                 {
//                     var errorLog = new TwitterImportLog
//                     {
//                         TenantId = task?.TenantId ?? 0,
//                         CharacterId = task?.CharacterId ?? Guid.Empty,
//                         TwitterAgentId = character.TwitterPostAgentId,
//                         TaskName = task?.TaskName,
//                         Message = "PostCharacterTweetAsync failed.",
//                         LogLevel = "Error",
//                         Exception = ex.ToString(),
//                         ExceptionMessage = ex.Message,
//                         LoggedAt = DateTime.UtcNow
//                     };
//                     await _twitterImportLogRepository.InsertAsync(errorLog);

//                     await _unitOfWorkManager.Current.SaveChangesAsync();
//                     throw;
//                 }

//                 var endTime = DateTime.UtcNow;
//                 task.LastRunCompletionTime = endTime;
//                 task.LastRunStartTime = startTime;
//                 task.LastRunDurationSeconds = (int)(endTime - startTime).TotalSeconds;
//                 if (task.RunEveryXMinutes > 0)
//                 {
//                     task.NextRunTime = endTime.AddMinutes(task.RunEveryXMinutes);
//                 }

//                 await _twitterImportTaskRepository.UpdateAsync(task);

//                 var successLog = new TwitterImportLog
//                 {
//                     TenantId = task.TenantId,
//                     CharacterId = task.CharacterId,
//                     TwitterAgentId = character.TwitterPostAgentId,
//                     TaskName = task.TaskName,
//                     Message = "Successfully posted character tweet.",
//                     LogLevel = "Information",
//                     LoggedAt = DateTime.UtcNow
//                 };

//                 await _twitterImportLogRepository.InsertAsync(successLog);

//                 await _unitOfWorkManager.Current.SaveChangesAsync();
//                 await uow.CompleteAsync();
//             }
//         }
//         private async Task ProcessCharacterPersonaWelcomeTweet(TwitterImportTask task, Character character)
//         {
//             var startTime = DateTime.UtcNow;

//             using (var uow = _unitOfWorkManager.Begin())
//             {
//                 try
//                 {
//                     if (task == null || task.CharacterId == Guid.Empty || string.IsNullOrEmpty(character.TwitterPostAgentId))
//                     {
//                         task ??= new TwitterImportTask();
//                         task.LastRunCompletionTime = null;

//                         var invalidLog = new TwitterImportLog
//                         {
//                             TenantId = task.TenantId,
//                             CharacterId = task.CharacterId,
//                             TwitterAgentId = character.TwitterPostAgentId,
//                             TaskName = task.TaskName,
//                             Message = "ProcessCharacterPersonaWelcomeTweet: Invalid task or missing fields.",
//                             LogLevel = "Warning",
//                             LoggedAt = DateTime.UtcNow
//                         };
//                         await _twitterImportLogRepository.InsertAsync(invalidLog);

//                         await _unitOfWorkManager.Current.SaveChangesAsync();
//                         await uow.CompleteAsync();
//                         return;
//                     }

//                     var characterPersonas = await _characterPersonaRepository
//                         .GetAll()
//                         .Include(x => x.TwitterProfile)
//                         .Include(x => x.TwitterRank)
//                         .Include(x => x.Persona).ThenInclude(p => p.Platforms).ThenInclude(p => p.Platform)
//                         .Where(x =>
//                             x.CharacterId == character.Id &&
//                             x.WelcomeMessageSent == false &&
//                             x.Persona.Platforms.Any(p => p.Platform.Name == "Twitter"))
//                         .OrderBy(x => x.TwitterRank.Rank)
//                         .Take(10)
//                         .ToListAsync();

//                     var limitToSend = 1;
//                     var successCount = 0;

//                     foreach (var characterPersona in characterPersonas)
//                     {
//                         if (successCount >= limitToSend) break;

//                         var twitterPlatform = characterPersona.Persona.Platforms.FirstOrDefault(p => p.Platform.Name == "Twitter");
//                         if (twitterPlatform == null) continue;

//                         var userHandle = twitterPlatform.PlatformPersonaId;
//                         var userSearch = userHandle.StartsWith("@") ? userHandle.Substring(1) : userHandle;
//                         var avatarUrl = characterPersona.TwitterProfile?.Avatar
//                             ?? "https://abs.twimg.com/sticky/default_profile_images/default_profile_normal.png";
//                         var userNameForQuery = Uri.EscapeDataString(userSearch);
//                         var avatarUrlForQuery = Uri.EscapeDataString(avatarUrl);
//                         var imageUrl = $"https://plant.fun/api/leaderboard/image?userName={userNameForQuery}&twitterAvatarUrl={avatarUrlForQuery}";

//                         try
//                         {
//                             using (var httpClient = new HttpClient())
//                             {
//                                 var imageBytes = await httpClient.GetByteArrayAsync(imageUrl);
//                                 var imageBase64 = Convert.ToBase64String(imageBytes);
//                                 var welcomeText = $"Follow me {userHandle} my new Gardener. and then reply under this post with your Solana wallet for Rain(air)drops! Keep feeding me mentions on X and watch me grow. \uD83C\uDF35\uD83C\uDF89";

//                                 await _twitterCommunicationService.PostTweetWithImageAsync(
//                                     character.TwitterPostAgentId,
//                                     imageBase64,
//                                     welcomeText
//                                 );

//                                 characterPersona.WelcomeMessageSent = true;
//                                 characterPersona.WelcomeMessageSentAt = DateTime.UtcNow;
//                                 await _characterPersonaRepository.UpdateAsync(characterPersona);

//                                 var successLog = new TwitterImportLog
//                                 {
//                                     TenantId = characterPersona.TenantId,
//                                     CharacterId = characterPersona.CharacterId,
//                                     TwitterAgentId = character.TwitterPostAgentId,
//                                     TaskName = task.TaskName,
//                                     Message = $"Successfully posted welcome tweet to {userHandle}.",
//                                     LogLevel = "Information",
//                                     LoggedAt = DateTime.UtcNow
//                                 };
//                                 await _twitterImportLogRepository.InsertAsync(successLog);

//                                 successCount++;
//                             }
//                         }
//                         catch (Exception ex)
//                         {
//                             var errorLog = new TwitterImportLog
//                             {
//                                 TenantId = characterPersona.TenantId,
//                                 CharacterId = characterPersona.CharacterId,
//                                 TwitterAgentId = character.TwitterPostAgentId,
//                                 TaskName = task.TaskName,
//                                 Message = $"Failed posting welcome tweet to {userHandle}.",
//                                 LogLevel = "Error",
//                                 Exception = ex.ToString(),
//                                 ExceptionMessage = ex.Message,
//                                 LoggedAt = DateTime.UtcNow
//                             };
//                             await _twitterImportLogRepository.InsertAsync(errorLog);
//                         }

//                         await _unitOfWorkManager.Current.SaveChangesAsync();
//                     }
//                 }
//                 catch (Exception ex)
//                 {
//                     var errorLog = new TwitterImportLog
//                     {
//                         TenantId = task?.TenantId ?? 0,
//                         CharacterId = task?.CharacterId ?? Guid.Empty,
//                         TwitterAgentId = character.TwitterPostAgentId,
//                         TaskName = task?.TaskName,
//                         Message = "ProcessCharacterPersonaWelcomeTweet failed.",
//                         LogLevel = "Error",
//                         Exception = ex.ToString(),
//                         ExceptionMessage = ex.Message,
//                         LoggedAt = DateTime.UtcNow
//                     };
//                     await _twitterImportLogRepository.InsertAsync(errorLog);

//                     await _unitOfWorkManager.Current.SaveChangesAsync();
//                     throw;
//                 }

//                 var endTime = DateTime.UtcNow;
//                 task.LastRunCompletionTime = endTime;
//                 task.LastRunStartTime = startTime;
//                 task.LastRunDurationSeconds = (int)(endTime - startTime).TotalSeconds;
//                 if (task.RunEveryXMinutes > 0) task.NextRunTime = endTime.AddMinutes(task.RunEveryXMinutes);

//                 await _twitterImportTaskRepository.UpdateAsync(task);

//                 var successLog2 = new TwitterImportLog
//                 {
//                     TenantId = task.TenantId,
//                     CharacterId = task.CharacterId,
//                     TwitterAgentId = character.TwitterPostAgentId,
//                     TaskName = task.TaskName,
//                     Message = "Successfully posted persona welcome tweets.",
//                     LogLevel = "Information",
//                     LoggedAt = DateTime.UtcNow
//                 };
//                 await _twitterImportLogRepository.InsertAsync(successLog2);

//                 await _unitOfWorkManager.Current.SaveChangesAsync();
//                 await uow.CompleteAsync();
//             }
//         }

//         private async Task PostCharacterTweetRisingSproutAsync(TwitterImportTask task, Character character)
//         {
//             var startTime = DateTime.UtcNow;

//             using (var uow = _unitOfWorkManager.Begin())
//             {
//                 try
//                 {
//                     if (task == null || task.CharacterId == Guid.Empty || string.IsNullOrEmpty(character.TwitterPostAgentId))
//                     {
//                         task ??= new TwitterImportTask();
//                         task.LastRunCompletionTime = null;

//                         var invalidLog = new TwitterImportLog
//                         {
//                             TenantId = task.TenantId,
//                             CharacterId = task.CharacterId,
//                             TwitterAgentId = character.TwitterPostAgentId,
//                             TaskName = task.TaskName,
//                             Message = "PostCharacterTweetRisingSproutAsync: Invalid task or missing fields.",
//                             LogLevel = "Warning",
//                             LoggedAt = DateTime.UtcNow
//                         };
//                         await _twitterImportLogRepository.InsertAsync(invalidLog);

//                         await _unitOfWorkManager.Current.SaveChangesAsync();
//                         await uow.CompleteAsync();
//                         return;
//                     }
//                 }
//                 catch (Exception ex)
//                 {
//                     var errorLog = new TwitterImportLog
//                     {
//                         TenantId = task?.TenantId ?? 0,
//                         CharacterId = task?.CharacterId ?? Guid.Empty,
//                         TwitterAgentId = character.TwitterPostAgentId,
//                         TaskName = task?.TaskName,
//                         Message = "PostCharacterTweetRisingSproutAsync failed.",
//                         LogLevel = "Error",
//                         Exception = ex.ToString(),
//                         ExceptionMessage = ex.Message,
//                         LoggedAt = DateTime.UtcNow
//                     };
//                     await _twitterImportLogRepository.InsertAsync(errorLog);

//                     await _unitOfWorkManager.Current.SaveChangesAsync();
//                     throw;
//                 }

//                 var endTime = DateTime.UtcNow;
//                 task.LastRunCompletionTime = endTime;
//                 task.LastRunStartTime = startTime;
//                 task.LastRunDurationSeconds = (int)(endTime - startTime).TotalSeconds;
//                 if (task.RunEveryXMinutes > 0)
//                 {
//                     task.NextRunTime = endTime.AddMinutes(task.RunEveryXMinutes);
//                 }

//                 await _twitterImportTaskRepository.UpdateAsync(task);

//                 var successLog = new TwitterImportLog
//                 {
//                     TenantId = task.TenantId,
//                     CharacterId = task.CharacterId,
//                     TwitterAgentId = character.TwitterPostAgentId,
//                     TaskName = task.TaskName,
//                     Message = "Successfully posted character rising sprout tweet.",
//                     LogLevel = "Information",
//                     LoggedAt = DateTime.UtcNow
//                 };

//                 await _twitterImportLogRepository.InsertAsync(successLog);
//                 await _unitOfWorkManager.Current.SaveChangesAsync();
//                 await uow.CompleteAsync();
//             }
//         }

//         public async Task ProcessCharacterTweetImports()
//         {
//             var characters = await _characterRepository.GetAllListAsync();

//             foreach (var character in characters)
//             {
//                 var tasks = await _twitterImportTaskRepository.GetAllListAsync(x => x.CharacterId == character.Id && x.IsEnabled && x.NextRunTime <= DateTime.UtcNow);

//                 foreach (var task in tasks)
//                 {
//                     if (task.TaskName == "ImportTweetsOfCharacterAsync")
//                     {
//                         await ImportTweetsOfCharacterAsync(task, character);
//                     }
//                     else if (task.TaskName == "ImportMentionsOfCharacterWithAPIAsync")
//                     {
//                         await ImportMentionsOfCharacterWithAPIAsync(task, character);
//                     }
//                     else if (task.TaskName == "ImportMentionsOfCharacterAsync")
//                     {
//                         await ImportMentionsOfCharacterAsync(task, character);
//                     }
//                     else if (task.TaskName == "ImportTweetsOfCharacterPersonasAsync")
//                     {
//                         await ImportTweetsOfCharacterPersonasAsync(task, character);
//                     }
//                 }
//             }
//         }

//         private async Task<TwitterImportTask> ImportTweetsOfCharacterAsync(TwitterImportTask task, Character character)
//         {
//             var startTime = DateTime.UtcNow;

//             using (var uow = _unitOfWorkManager.Begin())
//             {
//                 try
//                 {
//                     if (character == null)
//                     {
//                         task.LastRunCompletionTime = null;
//                         var noCharacterLog = new TwitterImportLog
//                         {
//                             TenantId = task.TenantId,
//                             CharacterId = task.CharacterId,
//                             TaskName = task.TaskName,
//                             Message = "No matching character found in DB.",
//                             LogLevel = "Warning",
//                             LoggedAt = DateTime.UtcNow
//                         };
//                         await _twitterImportLogRepository.InsertAsync(noCharacterLog);

//                         await _unitOfWorkManager.Current.SaveChangesAsync();
//                         await uow.CompleteAsync();
//                         return task;
//                     }

//                     if (task == null || task.CharacterId == Guid.Empty || string.IsNullOrEmpty(character?.TwitterScrapeAgentId))
//                     {
//                         task ??= new TwitterImportTask();
//                         task.LastRunCompletionTime = null;

//                         var invalidLog = new TwitterImportLog
//                         {
//                             TenantId = task.TenantId,
//                             CharacterId = task.CharacterId,
//                             TwitterAgentId = character?.TwitterScrapeAgentId,
//                             TaskName = task.TaskName,
//                             Message = "ImportTweetsOfCharacterAsync: Invalid task or missing fields.",
//                             LogLevel = "Warning",
//                             LoggedAt = DateTime.UtcNow
//                         };

//                         await _twitterImportLogRepository.InsertAsync(invalidLog);

//                         await _unitOfWorkManager.Current.SaveChangesAsync();
//                         await uow.CompleteAsync();
//                         return task;
//                     }

//                     int importLimit = task.ImportLimitTotal > 0 ? task.ImportLimitTotal : 10;
//                     var userSearch = character.TwitterUserName.StartsWith("@")
//                         ? character.TwitterUserName.Substring(1)
//                         : character.TwitterUserName;

//                     var tweets = await _twitterCommunicationService.GetTweetsAsync(
//                         character.TwitterScrapeAgentId,
//                         userSearch,
//                         importLimit
//                     );

//                     foreach (var tweet in tweets)
//                     {
//                         await MapAndUpsertTweetAsync(
//                             tweet,
//                             task,
//                             character.Id,
//                             character.Name,
//                             "CharacterTweet"
//                         );
//                     }

//                     var endTime = DateTime.UtcNow;
//                     task.LastRunCompletionTime = endTime;
//                     task.LastRunStartTime = startTime;
//                     task.LastRunDurationSeconds = (int)(endTime - startTime).TotalSeconds;
//                     if (task.RunEveryXMinutes > 0)
//                     {
//                         task.NextRunTime = endTime.AddMinutes(task.RunEveryXMinutes);
//                     }

//                     await _twitterImportTaskRepository.UpdateAsync(task);

//                     var successLog = new TwitterImportLog
//                     {
//                         TenantId = task.TenantId,
//                         CharacterId = task.CharacterId,
//                         TwitterAgentId = character.TwitterScrapeAgentId,
//                         TaskName = task.TaskName,
//                         Message = $"Successfully imported {tweets.Count} tweets.",
//                         LogLevel = "Information",
//                         LoggedAt = DateTime.UtcNow
//                     };
//                     await _twitterImportLogRepository.InsertAsync(successLog);

//                     await _unitOfWorkManager.Current.SaveChangesAsync();
//                     await uow.CompleteAsync();

//                     return task;
//                 }
//                 catch (Exception ex)
//                 {
//                     var errorLog = new TwitterImportLog
//                     {
//                         TenantId = task?.TenantId ?? 0,
//                         CharacterId = task?.CharacterId ?? Guid.Empty,
//                         TwitterAgentId = character?.TwitterScrapeAgentId,
//                         TaskName = task?.TaskName,
//                         Message = "Import failed.",
//                         LogLevel = "Error",
//                         Exception = ex.ToString(),
//                         ExceptionMessage = ex.Message,
//                         LoggedAt = DateTime.UtcNow
//                     };
//                     await _twitterImportLogRepository.InsertAsync(errorLog);

//                     await _unitOfWorkManager.Current.SaveChangesAsync();
//                     throw;
//                 }
//             }
//         }

//         private async Task<TwitterImportTask> ImportMentionsOfCharacterAsync(TwitterImportTask task, Character character)
//         {
//             var startTime = DateTime.UtcNow;

//             using (var uow = _unitOfWorkManager.Begin())
//             {
//                 try
//                 {
//                     if (character == null)
//                     {
//                         task.LastRunCompletionTime = null;

//                         var noCharacterLog = new TwitterImportLog
//                         {
//                             TenantId = task.TenantId,
//                             CharacterId = task.CharacterId,
//                             TwitterAgentId = character.TwitterScrapeAgentId,
//                             TaskName = task.TaskName,
//                             Message = "No matching character found in DB.",
//                             LogLevel = "Warning",
//                             LoggedAt = DateTime.UtcNow
//                         };
//                         await _twitterImportLogRepository.InsertAsync(noCharacterLog);

//                         await _unitOfWorkManager.Current.SaveChangesAsync();
//                         await uow.CompleteAsync();
//                         return task;
//                     }

//                     if (task == null || task.CharacterId == Guid.Empty || string.IsNullOrEmpty(character?.TwitterScrapeAgentId))
//                     {
//                         task ??= new TwitterImportTask();
//                         task.LastRunCompletionTime = null;

//                         var invalidLog = new TwitterImportLog
//                         {
//                             TenantId = task.TenantId,
//                             CharacterId = task.CharacterId,
//                             TwitterAgentId = character?.TwitterScrapeAgentId,
//                             TaskName = task.TaskName,
//                             Message = "ImportMentionsOfCharacterAsync: Invalid task or missing fields.",
//                             LogLevel = "Warning",
//                             LoggedAt = DateTime.UtcNow
//                         };
//                         await _twitterImportLogRepository.InsertAsync(invalidLog);

//                         await _unitOfWorkManager.Current.SaveChangesAsync();
//                         await uow.CompleteAsync();
//                         return task;
//                     }

//                     var userSearch = character.TwitterUserName.StartsWith("@")
//                         ? character.TwitterUserName.Substring(1)
//                         : character.TwitterUserName;

//                     var mentions = await _twitterCommunicationService.GetUserMentionsAsync(
//                         character.TwitterScrapeAgentId,
//                         userSearch,
//                         task.ImportLimitTotal
//                     );

//                     foreach (var tweet in mentions)
//                     {
//                         await MapAndUpsertTweetAsync(
//                             tweet,
//                             task,
//                             character.Id,
//                             character.Name,
//                             "CharacterMentionedTweet"
//                         );
//                     }

//                     var endTime = DateTime.UtcNow;
//                     task.LastRunCompletionTime = endTime;
//                     task.LastRunStartTime = startTime;
//                     task.LastRunDurationSeconds = (int)(endTime - startTime).TotalSeconds;
//                     if (task.RunEveryXMinutes > 0)
//                     {
//                         task.NextRunTime = endTime.AddMinutes(task.RunEveryXMinutes);
//                     }

//                     await _twitterImportTaskRepository.UpdateAsync(task);

//                     var successLog = new TwitterImportLog
//                     {
//                         TenantId = task.TenantId,
//                         CharacterId = task.CharacterId,
//                         TwitterAgentId = character.TwitterScrapeAgentId,
//                         TaskName = task.TaskName,
//                         Message = "Successfully imported mentions for character.",
//                         LogLevel = "Information",
//                         LoggedAt = DateTime.UtcNow
//                     };
//                     await _twitterImportLogRepository.InsertAsync(successLog);

//                     await _unitOfWorkManager.Current.SaveChangesAsync();
//                     await uow.CompleteAsync();

//                     return task;
//                 }
//                 catch (Exception ex)
//                 {
//                     var errorLog = new TwitterImportLog
//                     {
//                         TenantId = task?.TenantId ?? 0,
//                         CharacterId = task?.CharacterId ?? Guid.Empty,
//                         TwitterAgentId = character?.TwitterScrapeAgentId,
//                         TaskName = task?.TaskName,
//                         Message = "ImportMentionsOfCharacter failed.",
//                         LogLevel = "Error",
//                         Exception = ex.ToString(),
//                         ExceptionMessage = ex.Message,
//                         LoggedAt = DateTime.UtcNow
//                     };
//                     await _twitterImportLogRepository.InsertAsync(errorLog);

//                     await _unitOfWorkManager.Current.SaveChangesAsync();
//                     throw;
//                 }
//             }
//         }

//         private async Task<TwitterImportTask> ImportMentionsOfCharacterWithAPIAsync(
//              TwitterImportTask task,
//              Character character)
//         {
//             var startTime = DateTime.UtcNow;

//             try
//             {
//                 if (character == null)
//                 {
//                     task.LastRunCompletionTime = null;
//                     var noCharacterLog = new TwitterImportLog
//                     {
//                         TenantId = task.TenantId,
//                         CharacterId = task.CharacterId,
//                         TwitterAgentId = null,
//                         TaskName = task.TaskName,
//                         Message = "No matching character found in DB.",
//                         LogLevel = "Warning",
//                         LoggedAt = DateTime.UtcNow
//                     };

//                     // One short UoW just for logging
//                     using (var uow = _unitOfWorkManager.Begin())
//                     {
//                         await _twitterImportLogRepository.InsertAsync(noCharacterLog);
//                         await _unitOfWorkManager.Current.SaveChangesAsync();
//                         await uow.CompleteAsync();
//                     }

//                     return task;
//                 }

//                 if (task == null || task.CharacterId == Guid.Empty || string.IsNullOrEmpty(character.TwitterPostAgentId))
//                 {
//                     task ??= new TwitterImportTask();
//                     task.LastRunCompletionTime = null;

//                     var invalidLog = new TwitterImportLog
//                     {
//                         TenantId = task.TenantId,
//                         CharacterId = task.CharacterId,
//                         TwitterAgentId = character?.TwitterPostAgentId,
//                         TaskName = task.TaskName,
//                         Message = "ImportMentionsOfCharacterWithAPIAsync: Invalid task or missing fields.",
//                         LogLevel = "Warning",
//                         LoggedAt = DateTime.UtcNow
//                     };

//                     using (var uow = _unitOfWorkManager.Begin())
//                     {
//                         await _twitterImportLogRepository.InsertAsync(invalidLog);
//                         await _unitOfWorkManager.Current.SaveChangesAsync();
//                         await uow.CompleteAsync();
//                     }
//                     return task;
//                 }

//                 var sinceId = task.LastTweetImportId;
//                 var apiMentions = await _twitterCommunicationService.GetMentionsFromTweetIdAsync(
//                     character.TwitterPostAgentId,
//                     sinceId,
//                     task.ImportLimitTotal
//                 );

//                 var highestId = sinceId;

//                 // Decide on a batch size
//                 const int batchSize = 25;

//                 // We'll track all tweets in a list, then process in slices
//                 var mentionList = apiMentions.ToList();
//                 var totalMentions = mentionList.Count;
//                 var index = 0;

//                 while (index < totalMentions)
//                 {
//                     using (var batchUow = _unitOfWorkManager.Begin(/* requiresNew: true if desired */))
//                     {
//                         var batchTweets = mentionList
//                             .Skip(index)
//                             .Take(batchSize)
//                             .ToList();

//                         foreach (var tweet in batchTweets)
//                         {
//                             var mappedScraperTweet = ConvertApiTweetToScraperTweet(tweet);

//                             // Insert or Update the tweet (MapAndUpsertTweetAsync)
//                             await MapAndUpsertTweetAsync(
//                                 mappedScraperTweet,
//                                 task,
//                                 character.Id,
//                                 character.Name,
//                                 "CharacterMentionedTweet"
//                             );

//                             // Track highest tweet ID
//                             if (IsGreaterTweetId(tweet.Id, highestId))
//                             {
//                                 highestId = tweet.Id;
//                             }
//                         }

//                         // Save changes *once* per batch
//                         await _unitOfWorkManager.Current.SaveChangesAsync();

//                         // Commit the batch
//                         await batchUow.CompleteAsync();
//                     }

//                     index += batchSize;
//                 }

//                 if (!string.IsNullOrEmpty(highestId))
//                 {
//                     task.LastTweetImportId = highestId;
//                 }

//                 var endTime = DateTime.UtcNow;
//                 task.LastRunCompletionTime = endTime;
//                 task.LastRunStartTime = startTime;
//                 task.LastRunDurationSeconds = (int)(endTime - startTime).TotalSeconds;
//                 if (task.RunEveryXMinutes > 0)
//                 {
//                     task.NextRunTime = endTime.AddMinutes(task.RunEveryXMinutes);
//                 }

//                 // Log success outside the loop
//                 using (var finalUow = _unitOfWorkManager.Begin())
//                 {
//                     await _twitterImportTaskRepository.UpdateAsync(task);

//                     var successLog = new TwitterImportLog
//                     {
//                         TenantId = task.TenantId,
//                         CharacterId = task.CharacterId,
//                         TwitterAgentId = character.TwitterPostAgentId,
//                         TaskName = task.TaskName,
//                         Message = "Successfully imported mentions for character using API.",
//                         LogLevel = "Information",
//                         LoggedAt = DateTime.UtcNow
//                     };
//                     await _twitterImportLogRepository.InsertAsync(successLog);

//                     await _unitOfWorkManager.Current.SaveChangesAsync();
//                     await finalUow.CompleteAsync();
//                 }

//                 return task;
//             }
//             catch (Exception ex)
//             {
//                 using (var errorUow = _unitOfWorkManager.Begin())
//                 {
//                     var errorLog = new TwitterImportLog
//                     {
//                         TenantId = task?.TenantId ?? 0,
//                         CharacterId = task?.CharacterId ?? Guid.Empty,
//                         TwitterAgentId = character?.TwitterPostAgentId,
//                         TaskName = task?.TaskName,
//                         Message = "ImportMentionsOfCharacterWithAPIAsync failed.",
//                         LogLevel = "Error",
//                         Exception = ex.ToString(),
//                         ExceptionMessage = ex.Message,
//                         LoggedAt = DateTime.UtcNow
//                     };
//                     await _twitterImportLogRepository.InsertAsync(errorLog);

//                     await _unitOfWorkManager.Current.SaveChangesAsync();
//                     await errorUow.CompleteAsync();
//                 }

//                 throw;
//             }
//         }



//         // private async Task<TwitterImportTask> ImportMentionsOfCharacterWithAPIAsync(TwitterImportTask task, Character character)
//         // {
//         //     var startTime = DateTime.UtcNow;

//         //     using (var uow = _unitOfWorkManager.Begin())
//         //     {
//         //         try
//         //         {
//         //             if (character == null)
//         //             {
//         //                 task.LastRunCompletionTime = null;

//         //                 var noCharacterLog = new TwitterImportLog
//         //                 {
//         //                     TenantId = task.TenantId,
//         //                     CharacterId = task.CharacterId,
//         //                     TwitterAgentId = null,
//         //                     TaskName = task.TaskName,
//         //                     Message = "No matching character found in DB.",
//         //                     LogLevel = "Warning",
//         //                     LoggedAt = DateTime.UtcNow
//         //                 };
//         //                 await _twitterImportLogRepository.InsertAsync(noCharacterLog);
//         //                 await _unitOfWorkManager.Current.SaveChangesAsync();
//         //                 await uow.CompleteAsync();
//         //                 return task;
//         //             }

//         //             if (task == null || task.CharacterId == Guid.Empty || string.IsNullOrEmpty(character.TwitterPostAgentId))
//         //             {
//         //                 task ??= new TwitterImportTask();
//         //                 task.LastRunCompletionTime = null;

//         //                 var invalidLog = new TwitterImportLog
//         //                 {
//         //                     TenantId = task.TenantId,
//         //                     CharacterId = task.CharacterId,
//         //                     TwitterAgentId = character?.TwitterPostAgentId,
//         //                     TaskName = task.TaskName,
//         //                     Message = "ImportMentionsOfCharacterWithAPIAsync: Invalid task or missing fields.",
//         //                     LogLevel = "Warning",
//         //                     LoggedAt = DateTime.UtcNow
//         //                 };
//         //                 await _twitterImportLogRepository.InsertAsync(invalidLog);
//         //                 await _unitOfWorkManager.Current.SaveChangesAsync();
//         //                 await uow.CompleteAsync();
//         //                 return task;
//         //             }

//         //             var sinceId = task.LastTweetImportId;
//         //             var apiMentions = await _twitterCommunicationService.GetMentionsFromTweetIdAsync(
//         //                 character.TwitterPostAgentId,
//         //                 sinceId,
//         //                 task.ImportLimitTotal
//         //             );

//         //             var highestId = sinceId;
//         //             foreach (var tweet in apiMentions)
//         //             {
//         //                 var mappedScraperTweet = ConvertApiTweetToScraperTweet(tweet);

//         //                 await MapAndUpsertTweetAsync(
//         //                     mappedScraperTweet,
//         //                     task,
//         //                     character.Id,
//         //                     character.Name,
//         //                     "CharacterMentionedTweet"
//         //                 );

//         //                 if (IsGreaterTweetId(tweet.Id, highestId))
//         //                 {
//         //                     highestId = tweet.Id;
//         //                 }
//         //             }

//         //             if (!string.IsNullOrEmpty(highestId))
//         //             {
//         //                 task.LastTweetImportId = highestId;
//         //             }

//         //             var endTime = DateTime.UtcNow;
//         //             task.LastRunCompletionTime = endTime;
//         //             task.LastRunStartTime = startTime;
//         //             task.LastRunDurationSeconds = (int)(endTime - startTime).TotalSeconds;
//         //             if (task.RunEveryXMinutes > 0)
//         //             {
//         //                 task.NextRunTime = endTime.AddMinutes(task.RunEveryXMinutes);
//         //             }

//         //             await _twitterImportTaskRepository.UpdateAsync(task);

//         //             var successLog = new TwitterImportLog
//         //             {
//         //                 TenantId = task.TenantId,
//         //                 CharacterId = task.CharacterId,
//         //                 TwitterAgentId = character.TwitterPostAgentId,
//         //                 TaskName = task.TaskName,
//         //                 Message = "Successfully imported mentions for character using API.",
//         //                 LogLevel = "Information",
//         //                 LoggedAt = DateTime.UtcNow
//         //             };
//         //             await _twitterImportLogRepository.InsertAsync(successLog);

//         //             await _unitOfWorkManager.Current.SaveChangesAsync();
//         //             await uow.CompleteAsync();

//         //             return task;
//         //         }
//         //         catch (Exception ex)
//         //         {
//         //             var errorLog = new TwitterImportLog
//         //             {
//         //                 TenantId = task?.TenantId ?? 0,
//         //                 CharacterId = task?.CharacterId ?? Guid.Empty,
//         //                 TwitterAgentId = character?.TwitterPostAgentId,
//         //                 TaskName = task?.TaskName,
//         //                 Message = "ImportMentionsOfCharacterWithAPIAsync failed.",
//         //                 LogLevel = "Error",
//         //                 Exception = ex.ToString(),
//         //                 ExceptionMessage = ex.Message,
//         //                 LoggedAt = DateTime.UtcNow
//         //             };
//         //             await _twitterImportLogRepository.InsertAsync(errorLog);

//         //             await _unitOfWorkManager.Current.SaveChangesAsync();
//         //             throw;
//         //         }
//         //     }
//         // }

//         private bool IsGreaterTweetId(string newId, string currentId)
//         {
//             if (string.IsNullOrWhiteSpace(newId)) return false;
//             if (string.IsNullOrWhiteSpace(currentId)) return true;

//             if (long.TryParse(newId, out long newLong) && long.TryParse(currentId, out long currLong))
//             {
//                 return newLong > currLong;
//             }
//             return false;
//         }

//         private TwitterScraperTweetResponse ConvertApiTweetToScraperTweet(TwitterApiGetTweetResponse apiTweet)
//         {

//             return new TwitterScraperTweetResponse
//             {
//                 Id = apiTweet.Id,
//                 Text = apiTweet.Text,
//                 Username = apiTweet.AuthorUsername,
//                 Name = apiTweet.AuthorName,
//                 UserId = apiTweet.AuthorId,
//                 ConversationId = apiTweet.ConversationId,
//                 PermanentUrl = $"https://x.com/{apiTweet.AuthorUsername}/status/{apiTweet.Id}",
//                 TimeParsed = apiTweet.CreatedAt,
//                 BookmarkCount = apiTweet.PublicMetrics?.BookmarkCount,
//                 Likes = apiTweet.PublicMetrics?.LikeCount,
//                 Replies = apiTweet.PublicMetrics?.ReplyCount,
//                 Retweets = apiTweet.PublicMetrics?.RetweetCount,
//                 Views = apiTweet.PublicMetrics?.ImpressionCount,
//                 Photos = null,
//                 Videos = null,
//                 Mentions = null,
//                 Hashtags = null,
//                 Place = null,
//                 SensitiveContent = null,
//                 Poll = null,
//                 QuotedStatusId = null,
//                 RetweetedStatusId = null,
//             };
//         }



//         private async Task<TwitterImportTask> ImportTweetsOfCharacterPersonasAsync(TwitterImportTask task, Character character)
//         {
//             var startTime = DateTime.UtcNow;

//             using (var uow = _unitOfWorkManager.Begin())
//             {
//                 try
//                 {
//                     if (character == null)
//                     {
//                         task.LastRunCompletionTime = null;

//                         var noCharacterLog = new TwitterImportLog
//                         {
//                             TenantId = task.TenantId,
//                             CharacterId = task.CharacterId,
//                             TwitterAgentId = character?.TwitterScrapeAgentId,
//                             TaskName = task.TaskName,
//                             Message = "No matching character found in DB.",
//                             LogLevel = "Warning",
//                             LoggedAt = DateTime.UtcNow
//                         };
//                         await _twitterImportLogRepository.InsertAsync(noCharacterLog);

//                         await _unitOfWorkManager.Current.SaveChangesAsync();
//                         await uow.CompleteAsync();
//                         return task;
//                     }

//                     if (task == null || task.CharacterId == Guid.Empty || string.IsNullOrEmpty(character.TwitterScrapeAgentId))
//                     {
//                         task ??= new TwitterImportTask();
//                         task.LastRunCompletionTime = null;

//                         var invalidLog = new TwitterImportLog
//                         {
//                             TenantId = task.TenantId,
//                             CharacterId = task.CharacterId,
//                             TwitterAgentId = character?.TwitterScrapeAgentId,
//                             TaskName = task.TaskName,
//                             Message = "ImportTweetsOfCharacterPersonasAsync: Invalid task or missing fields.",
//                             LogLevel = "Warning",
//                             LoggedAt = DateTime.UtcNow
//                         };
//                         await _twitterImportLogRepository.InsertAsync(invalidLog);

//                         await _unitOfWorkManager.Current.SaveChangesAsync();
//                         await uow.CompleteAsync();
//                         return task;
//                     }

//                     var characterPersonas = await _characterPersonaRepository
//                         .GetAll()
//                         .Include(cp => cp.Persona)
//                         .ThenInclude(p => p.Platforms)
//                         .ThenInclude(p => p.Platform)
//                         .Where(cp => cp.CharacterId == character.Id && cp.ShouldImportNewPosts)
//                         .ToListAsync();

//                     if (characterPersonas == null || !characterPersonas.Any())
//                     {
//                         task.LastRunCompletionTime = DateTime.UtcNow;
//                         await _unitOfWorkManager.Current.SaveChangesAsync();
//                         await uow.CompleteAsync();
//                         return task;
//                     }

//                     characterPersonas = characterPersonas.Where(cp => cp.Persona.Platforms.Any(p => p.Platform.Name == "Twitter")).ToList();
//                     var personas = characterPersonas.Select(cp => cp.Persona).ToList();
//                     if (personas == null || !personas.Any())
//                     {
//                         task.LastRunCompletionTime = DateTime.UtcNow;
//                         await _unitOfWorkManager.Current.SaveChangesAsync();
//                         await uow.CompleteAsync();
//                         return task;
//                     }

//                     foreach (var persona in personas)
//                     {
//                         var twitterPlatform = persona.Platforms.FirstOrDefault(p => p.Platform.Name == "Twitter");
//                         if (twitterPlatform == null)
//                         {
//                             continue;
//                         }

//                         var userSearch = twitterPlatform.PlatformPersonaId.StartsWith("@")
//                             ? twitterPlatform.PlatformPersonaId.Substring(1)
//                             : twitterPlatform.PlatformPersonaId;

//                         var tweets = await _twitterCommunicationService.GetTweetsAsync(
//                             character.TwitterScrapeAgentId,
//                             userSearch,
//                             task.ImportLimitTotal > 0 ? task.ImportLimitTotal : 10
//                         );

//                         foreach (var tweet in tweets)
//                         {
//                             await MapAndUpsertTweetAsync(
//                                 tweet,
//                                 task,
//                                 character.Id,
//                                 character.Name,
//                                 "CharacterPersonaTweet"
//                             );
//                         }
//                     }

//                     var endTime = DateTime.UtcNow;
//                     task.LastRunCompletionTime = endTime;
//                     task.LastRunStartTime = startTime;
//                     task.LastRunDurationSeconds = (int)(endTime - startTime).TotalSeconds;
//                     if (task.RunEveryXMinutes > 0)
//                     {
//                         task.NextRunTime = endTime.AddMinutes(task.RunEveryXMinutes);
//                     }

//                     await _twitterImportTaskRepository.UpdateAsync(task);

//                     var successLog = new TwitterImportLog
//                     {
//                         TenantId = task.TenantId,
//                         CharacterId = task.CharacterId,
//                         TwitterAgentId = character.TwitterScrapeAgentId,
//                         TaskName = task.TaskName,
//                         Message = "Successfully imported tweets for character personas.",
//                         LogLevel = "Information",
//                         LoggedAt = DateTime.UtcNow
//                     };
//                     await _twitterImportLogRepository.InsertAsync(successLog);

//                     await _unitOfWorkManager.Current.SaveChangesAsync();
//                     await uow.CompleteAsync();

//                     return task;
//                 }
//                 catch (Exception ex)
//                 {
//                     var errorLog = new TwitterImportLog
//                     {
//                         TenantId = task?.TenantId ?? 0,
//                         CharacterId = task?.CharacterId ?? Guid.Empty,
//                         TwitterAgentId = character?.TwitterScrapeAgentId,
//                         TaskName = task?.TaskName,
//                         Message = "ImportCharacterPersonas failed.",
//                         LogLevel = "Error",
//                         Exception = ex.ToString(),
//                         ExceptionMessage = ex.Message,
//                         LoggedAt = DateTime.UtcNow
//                     };
//                     await _twitterImportLogRepository.InsertAsync(errorLog);

//                     await _unitOfWorkManager.Current.SaveChangesAsync();
//                     throw;
//                 }
//             }
//         }

//         private async Task<TwitterImportTweet> MapAndUpsertTweetAsync(
//             TwitterScraperTweetResponse tweet,
//             TwitterImportTask importTask,
//             Guid characterId,
//             string characterName,
//             string tweetType)
//         {

//             var isNew = false;

//             var tweetId = tweet.Id ?? string.Empty;

//             var existingEntity = await _importTweetRepository.FirstOrDefaultAsync(x =>
//                 x.TweetId == tweetId && x.CharacterId == characterId);

//             if (existingEntity == null)
//             {
//                 isNew = true;

//                 existingEntity = new TwitterImportTweet
//                 {
//                     TenantId = importTask.TenantId,
//                     TweetType = tweetType,
//                     CharacterId = characterId,
//                     CharacterName = characterName,
//                     TweetId = tweetId
//                 };
//             }

//             existingEntity.Hashtags = MergeValues(existingEntity.Hashtags, tweet.Hashtags != null ? string.Join(",", tweet.Hashtags) : null);
//             existingEntity.Urls = MergeValues(existingEntity.Urls, tweet.Urls != null ? string.Join(",", tweet.Urls) : null);

//             existingEntity.MentionsJson = MergeValues(existingEntity.MentionsJson, tweet.Mentions != null ? JsonSerializer.Serialize(tweet.Mentions) : null);
//             existingEntity.PhotosJson = MergeValues(existingEntity.PhotosJson, tweet.Photos != null ? JsonSerializer.Serialize(tweet.Photos) : null);
//             existingEntity.VideosJson = MergeValues(existingEntity.VideosJson, tweet.Videos != null ? JsonSerializer.Serialize(tweet.Videos) : null);
//             existingEntity.PlaceJson = MergeValues(existingEntity.PlaceJson, tweet.Place != null ? JsonSerializer.Serialize(tweet.Place) : null);
//             existingEntity.PollJson = MergeValues(existingEntity.PollJson, tweet.Poll != null ? JsonSerializer.Serialize(tweet.Poll) : null);
//             existingEntity.InReplyToStatusJson = MergeValues(existingEntity.InReplyToStatusJson, tweet.InReplyToStatus != null ? JsonSerializer.Serialize(tweet.InReplyToStatus) : null);
//             existingEntity.QuotedStatusJson = MergeValues(existingEntity.QuotedStatusJson, tweet.QuotedStatus != null ? JsonSerializer.Serialize(tweet.QuotedStatus) : null);
//             existingEntity.RetweetedStatusJson = MergeValues(existingEntity.RetweetedStatusJson, tweet.RetweetedStatus != null ? JsonSerializer.Serialize(tweet.RetweetedStatus) : null);
//             existingEntity.ThreadJson = MergeValues(existingEntity.ThreadJson, tweet.Thread != null ? JsonSerializer.Serialize(tweet.Thread) : null);

//             existingEntity.BookmarkCount = tweet.BookmarkCount ?? existingEntity.BookmarkCount;
//             existingEntity.ConversationId = MergeValues(existingEntity.ConversationId, tweet.ConversationId);
//             existingEntity.Html = MergeValues(existingEntity.Html, tweet.Html);
//             existingEntity.InReplyToStatusId = MergeValues(existingEntity.InReplyToStatusId, tweet.InReplyToStatusId);
//             existingEntity.IsQuoted = tweet.IsQuoted ?? existingEntity.IsQuoted;
//             existingEntity.IsPin = tweet.IsPin ?? existingEntity.IsPin;
//             existingEntity.IsReply = tweet.IsReply ?? existingEntity.IsReply;
//             existingEntity.IsRetweet = tweet.IsRetweet ?? existingEntity.IsRetweet;
//             existingEntity.IsSelfThread = tweet.IsSelfThread ?? existingEntity.IsSelfThread;
//             existingEntity.Likes = tweet.Likes ?? existingEntity.Likes;
//             existingEntity.Name = MergeValues(existingEntity.Name, tweet.Name);
//             existingEntity.PermanentUrl = MergeValues(existingEntity.PermanentUrl, tweet.PermanentUrl);
//             existingEntity.QuotedStatusId = MergeValues(existingEntity.QuotedStatusId, tweet.QuotedStatusId);
//             existingEntity.Replies = tweet.Replies ?? existingEntity.Replies;
//             existingEntity.Retweets = tweet.Retweets ?? existingEntity.Retweets;
//             existingEntity.RetweetedStatusId = MergeValues(existingEntity.RetweetedStatusId, tweet.RetweetedStatusId);
//             existingEntity.Text = MergeValues(existingEntity.Text, tweet.Text);
//             existingEntity.TimeParsed = tweet.TimeParsed ?? existingEntity.TimeParsed;
//             existingEntity.Timestamp = tweet.Timestamp ?? existingEntity.Timestamp;
//             existingEntity.UserId = MergeValues(existingEntity.UserId, tweet.UserId);
//             existingEntity.Username = MergeValues(existingEntity.Username, tweet.Username);
//             existingEntity.Views = tweet.Views ?? existingEntity.Views;
//             existingEntity.SensitiveContent = tweet.SensitiveContent ?? existingEntity.SensitiveContent;
//             existingEntity.LastTwitterImportDate = DateTime.UtcNow;
//             existingEntity.LastTwitterImportExported = false;

//             if (isNew)
//             {
//                 await _importTweetRepository.InsertAsync(existingEntity);
//             }
//             else
//             {
//                 await _importTweetRepository.UpdateAsync(existingEntity);
//             }

//             return existingEntity;
//         }

//         private string MergeValues(string existingValue, string updatedValue)
//         {
//             if (!string.IsNullOrEmpty(updatedValue))
//             {
//                 return updatedValue;
//             }
//             return existingValue ?? string.Empty;
//         }


//         private MemoryStatsTwitter CreateMemoryStatsTwitter(TwitterImportTweet tweet)
//         {
//             return new MemoryStatsTwitter
//             {
//                 TenantId = tweet.TenantId,
//                 IsPin = tweet.IsPin,
//                 IsQuoted = tweet.IsQuoted,
//                 IsReply = tweet.IsReply,
//                 IsRetweet = tweet.IsRetweet,
//                 SensitiveContent = tweet.SensitiveContent,
//                 BookmarkCount = tweet.BookmarkCount,
//                 Likes = tweet.Likes,
//                 Replies = tweet.Replies,
//                 Retweets = tweet.Retweets,
//                 Views = tweet.Views,
//                 TweetWordCount = tweet.Text.Split(' ').Length,
//                 MentionsCount = tweet.MentionsJson.ToString().Split('}').Length - 1
//             };
//         }

//         private string FormatUsername(string username)
//         {
//             return username.StartsWith("@") ? username : "@" + username;
//         }

//         private async Task UpdateImportTweetExportedStatus(TwitterImportTweet tweet)
//         {
//             using (var uow = _unitOfWorkManager.Begin())
//             {
//                 tweet.Exported = true;
//                 tweet.ExportDate = DateTime.UtcNow;

//                 await _importTweetRepository.UpdateAsync(tweet);
//                 await _unitOfWorkManager.Current.SaveChangesAsync();
//                 await uow.CompleteAsync();
//             }
//         }

//         private async Task UpdateImportTweetLastExportedStatus(TwitterImportTweet tweet)
//         {
//             using (var uow = _unitOfWorkManager.Begin())
//             {
//                 tweet.ExportDate = DateTime.UtcNow;
//                 tweet.LastTwitterImportExported = true;

//                 await _importTweetRepository.UpdateAsync(tweet);
//                 await _unitOfWorkManager.Current.SaveChangesAsync();
//                 await uow.CompleteAsync();
//             }
//         }
//     }
// }
