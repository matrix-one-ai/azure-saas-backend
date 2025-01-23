using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Domain.Uow;
using Abp.UI;
using Icon.Matrix.AIManager;
using Icon.Matrix.AIManager.CharacterMentioned;
using Icon.Matrix.AIManager.CharacterPostTweet;
using Icon.Matrix.Enums;
using Icon.Matrix.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Icon.Matrix
{
    public partial interface IMemoryManager : IDomainService
    {
        Task<AICharacterMentionedResponse> RunCharacterMentionedPrompt(Guid memoryId, AIModelType modelType);
        Task RunCharacterPostTweetPrompt(Guid characterId, AIModelType modelType);
        Task<string> TestCharacterPostTweetPrompt(Guid characterId, AIModelType modelType);
        Task<string> GetCharacterPostTweetPrompt(Guid characterId);
    }
    public partial class MemoryManager : IconServiceBase, IMemoryManager
    {
        public async Task<AICharacterMentionedResponse> RunCharacterMentionedPrompt(Guid memoryId, AIModelType modelType)
        {
            AICharacterMentionedResponse response = new AICharacterMentionedResponse
            {
                IsSuccess = false,
            };

            MemoryPrompt memoryPrompt = new MemoryPrompt
            {
                TenantId = _tenantId,
                MemoryId = memoryId,
                PromptType = AIPromptType.ReplyMentionedTweet,
                InputContextModel = nameof(AICharacterMentionedContext),
                ResponseModel = nameof(AICharacterMentionedResponse),
                GeneratedAt = DateTimeOffset.Now
            };

            AICharacterMentionedContext context = null;
            try
            {
                context = await GetPromptContextMentioned(memoryId);
                memoryPrompt.InputContextJson = JsonConvert.SerializeObject(context, Formatting.Indented);
            }
            catch (Exception ex)
            {
                memoryPrompt.IsSuccess = false;
                memoryPrompt.Exception = "Error generating context for prompt";
                memoryPrompt.ExceptionMessage = ex.Message;
                await StorePrompt(memoryPrompt);

                return response;
            }

            string textInput;
            try
            {
                textInput = await _aiManager.GenerateMentionedPromptAsync(context);
                memoryPrompt.InputFullText = textInput;
            }
            catch (Exception ex)
            {
                memoryPrompt.IsSuccess = false;
                memoryPrompt.Exception = "Error generating input for prompt";
                memoryPrompt.ExceptionMessage = ex.Message;
                await StorePrompt(memoryPrompt);

                return response;
            }

            try
            {
                response = await _aiManager.GenerateMentionedResponseAsync(context, modelType);

                if (response.IsSuccess && response.Scores != null)
                {
                    var memory = await GetMemory(memoryId);
                    if (memory != null && memory.MemoryStatsTwitter != null)
                    {
                        memory.IsPromptGenerated = true;
                        memory.MemoryStatsTwitter.RelevanceScore = response.Scores.Relevance;
                        memory.MemoryStatsTwitter.DepthScore = response.Scores.Depth;
                        memory.MemoryStatsTwitter.SentimentScore = response.Scores.Sentiment;
                        if (response.CryptocoinsFound != null && response.CryptocoinsFound.Count > 0)
                        {
                            memory.Tags = string.Join(", ", response.CryptocoinsFound);
                            if (string.IsNullOrEmpty(memory.Tags) || memory.Tags.ToLower().Contains("plant"))
                            {
                                memory.MemoryStatsTwitter.NoveltyScore = 0;
                            }
                            else
                            {
                                memory.MemoryStatsTwitter.NoveltyScore = 1;
                            }
                        }
                        else
                        {
                            memory.MemoryStatsTwitter.NoveltyScore = 0;
                        }

                        await UpdateMemory(memory);
                    }
                }

                memoryPrompt.ResponseJson = JsonConvert.SerializeObject(response, Formatting.Indented);
                memoryPrompt.IsSuccess = response.IsSuccess;
                memoryPrompt.Exception = response.Exception;
                memoryPrompt.ExceptionMessage = response.ExceptionMessage;

                await StorePrompt(memoryPrompt);
                return response;
            }
            catch (Exception ex)
            {
                memoryPrompt.IsSuccess = false;
                memoryPrompt.Exception = "Error generating response for prompt";
                memoryPrompt.ExceptionMessage = ex.Message;
                memoryPrompt = await StorePrompt(memoryPrompt);

                return response;
            }
            ;
        }

        public async Task<string> GetCharacterPostTweetPrompt(Guid characterId)
        {
            AICharacterPostTweetContext context = null;
            try
            {
                context = await GetPromptContextPostTweet(characterId);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                throw new UserFriendlyException("An error occurred while fetching the prompt context.", ex);
            }
            try
            {
                var prompt = await _aiManager.GeneratePostTweetPromptAsync(context);
                return prompt;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                throw new UserFriendlyException("An error occurred while generating the prompt.", ex);
            }
        }

        public async Task<string> TestCharacterPostTweetPrompt(Guid characterId, AIModelType modelType)
        {
            AICharacterPostTweetContext context = null;
            try
            {
                context = await GetPromptContextPostTweet(characterId);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                throw new UserFriendlyException("An error occurred while fetching the prompt context.", ex);
            }
            try
            {
                var response = await _aiManager.GeneratePostTweetResponseAsync(context, modelType);
                var character = await _characterManager.GetCharacterById(characterId);

                if (response.StartsWith("\""))
                {
                    response = response.Substring(1);
                }
                if (response.EndsWith("\""))
                {
                    response = response.Substring(0, response.Length - 1);
                }

                return response;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                throw new UserFriendlyException("An error occurred while generating the response.", ex);
            }
        }
        public async Task RunCharacterPostTweetPrompt(Guid characterId, AIModelType modelType)
        {
            AICharacterPostTweetContext context = null;
            try
            {
                context = await GetPromptContextPostTweet(characterId);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                throw new UserFriendlyException("An error occurred while fetching the prompt context.", ex);
            }
            try
            {
                var response = await _aiManager.GeneratePostTweetResponseAsync(context, modelType);
                var character = await _characterManager.GetCharacterById(characterId);

                if (response.StartsWith("\""))
                {
                    response = response.Substring(1);
                }
                if (response.EndsWith("\""))
                {
                    response = response.Substring(0, response.Length - 1);
                }

                await _twitterManager.PostTweetAsync(character, response);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                throw new UserFriendlyException("An error occurred while generating the response.", ex);
            }
        }

        private async Task<MemoryPrompt> StorePrompt(MemoryPrompt memoryPrompt)
        {
            using (var uow = _unitOfWorkManager.Begin())
            {
                memoryPrompt = await _memoryPromptRepository.InsertAsync(memoryPrompt);
                await _unitOfWorkManager.Current.SaveChangesAsync();
                await uow.CompleteAsync();
            }

            return memoryPrompt;
        }

        private async Task<AICharacterMentionedContext> GetPromptContextMentioned(Guid memoryId)
        {
            var tweetContext = new AICharacterMentionedContext();

            try
            {
                var memory = await _memoryRepository.GetAll()
                    .Include(x => x.Character)
                    .Include(x => x.CharacterBio)
                    .Include(x => x.Platform)
                    .Include(x => x.CharacterPersona)
                        .ThenInclude(cp => cp.Persona)
                        .ThenInclude(p => p.Platforms)
                        .ThenInclude(p => p.Platform)
                    .Include(x => x.MemoryType)
                    .Include(x => x.MemoryStatsTwitter)
                    .Where(x => x.Id == memoryId)
                    .FirstOrDefaultAsync();

                if (memory == null)
                {
                    throw new UserFriendlyException("Memory not found.");
                }

                var personaPlatformId = memory.CharacterPersona?.Persona?.Platforms
                    ?.FirstOrDefault(p => p.Platform?.Name == "Twitter")?.PlatformPersonaId;

                var conversation = await _memoryRepository.GetAll()
                    .Include(x => x.MemoryType)
                    .Include(x => x.CharacterPersona)
                        .ThenInclude(cp => cp.Persona)
                        .ThenInclude(p => p.Platforms)
                        .ThenInclude(p => p.Platform)
                    .Where(x => x.PlatformInteractionParentId == memory.PlatformInteractionParentId)
                    .OrderByDescending(x => x.PlatformInteractionDate)
                    .Take(15)
                    .ToListAsync();

                var lastTweetsByUser = await _memoryRepository.GetAll()
                    .Where(x => x.CharacterPersonaId == memory.CharacterPersonaId)
                    .OrderByDescending(x => x.PlatformInteractionDate)
                    .Take(10)
                    .ToListAsync();

                var userStats = await _cpTwitterRankRepository.GetAll()
                    .Where(x => x.CharacterPersonaId == memory.CharacterPersonaId)
                    .FirstOrDefaultAsync();

                tweetContext = new AICharacterMentionedContext
                {
                    TwitterMentionReplyExamples = memory.Character.TwitterMentionReplyExamples,
                    TwitterMentionReplyInstruction = memory.Character.TwitterMentionReplyInstruction,

                    CharacterToActAs = new AIManager.CharacterMentioned.CharacterToActAsDto
                    {
                        Id = memory.Character?.Id ?? Guid.Empty,
                        Name = memory.Character?.Name,
                        BirthDate = memory.CharacterBio?.BirthDate ?? DateTimeOffset.MinValue,
                        Bio = memory.CharacterBio?.Bio,
                        Personality = memory.CharacterBio?.Personality,
                        Appearance = memory.CharacterBio?.Appearance,
                        Occupation = memory.CharacterBio?.Occupation,
                        Motivations = memory.CharacterBio?.Motivations,
                        Fears = memory.CharacterBio?.Fears,
                        Values = memory.CharacterBio?.Values,
                        SpeechPatterns = memory.CharacterBio?.SpeechPatterns,
                        Skills = memory.CharacterBio?.Skills,
                        Backstory = memory.CharacterBio?.Backstory,
                        PublicPersona = memory.CharacterBio?.PublicPersona,
                        PrivateSelf = memory.CharacterBio?.PrivateSelf,
                        MediaPresence = memory.CharacterBio?.MediaPresence,
                        CrisisBehavior = memory.CharacterBio?.CrisisBehavior,
                        Relationships = memory.CharacterBio?.Relationships,
                        TechDetails = memory.CharacterBio?.TechDetails
                    },
                    UserToRespondTo = new UserToRespondToDto
                    {
                        Name = memory.CharacterPersona?.Persona?.Name,
                        UserName = personaPlatformId,
                        AttitudeTowardsUser = memory.CharacterPersona?.Attitude,
                        ExampleResponses = memory.CharacterPersona?.Repsonses,
                        EngagementTowardsCharacter = userStats == null ? null : new EngagementToWithCharacterDto
                        {
                            TotalTweets = userStats.TotalMentions,
                            TotalLikes = userStats.TotalLikes,
                            TotalReplies = userStats.TotalReplies,
                            TotalRetweets = userStats.TotalRetweets,
                            TotalViews = userStats.TotalViews,
                            TotalEngagementScore = userStats.TotalEngagementScore,

                            TotalRelevanceScore = userStats.TotalRelevanceScore,
                            TotalDepthScore = userStats.TotalDepthScore,
                            TotalNoveltyScore = userStats.TotalNoveltyScore,
                            TotalSentimentScore = userStats.TotalSentimentScore,
                            TotalQualityScore = userStats.TotalQualityScore,

                            TotalScore = userStats.TotalScore,
                            Rank = userStats.Rank,

                            TotalMentionsCount = userStats.TotalMentionsCount,
                            TotalWordCount = userStats.TotalWordCount,
                        }
                    },
                    TweetToRespondTo = new AIManager.CharacterMentioned.Tweet
                    {
                        TweetId = memory.PlatformInteractionId,
                        TweetType = memory.MemoryType?.Name,
                        TweetUserName = personaPlatformId,
                        TweetContent = memory.MemoryContent,
                        TweetDate = memory.PlatformInteractionDate,
                        IsTweetByCharacter = false
                    },
                    ConversationTimeLine = new ConversationTimeLineDto
                    {
                        ConversationId = memory.PlatformInteractionParentId,
                        ConversationTweets = conversation?.Select(x => new AIManager.CharacterMentioned.Tweet
                        {
                            TweetId = x.PlatformInteractionId,
                            TweetType = x.MemoryType?.Name,
                            TweetUserName = x.CharacterPersona?.Persona?.Platforms?.FirstOrDefault()?.PlatformPersonaId,
                            TweetContent = x.MemoryContent,
                            TweetDate = x.PlatformInteractionDate,
                            IsTweetByCharacter = x.MemoryType?.Name == "TweetByCharacter"
                        })?.ToList()
                    },
                    LastTweetsByUser = lastTweetsByUser?.Select(x => new AIManager.CharacterMentioned.Tweet
                    {
                        TweetId = x.PlatformInteractionId,
                        TweetType = x.MemoryType?.Name,
                        TweetUserName = x.CharacterPersona?.Persona?.Platforms?.FirstOrDefault()?.PlatformPersonaId,
                        TweetContent = x.MemoryContent,
                        TweetDate = x.PlatformInteractionDate,
                        IsTweetByCharacter = false
                    }).ToList()
                };

                return tweetContext;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                throw new UserFriendlyException("An error occurred while fetching the prompt context.", ex);
            }
        }

        private async Task<AICharacterPostTweetContext> GetPromptContextPostTweet(Guid characterId)
        {
            var tweetContext = new AICharacterPostTweetContext();
            try
            {
                var character = await _characterManager.GetCharacterById(characterId);
                var activeBio = await _characterManager.GetActiveCharacterBio(characterId);

                if (character == null)
                {
                    throw new UserFriendlyException("Character not found.");
                }

                var lastTweets = await _memoryRepository.GetAll()
                    .Where(x => x.CharacterId == characterId)
                    .OrderByDescending(x => x.PlatformInteractionDate)
                    .Take(15)
                    .ToListAsync();

                tweetContext = new AICharacterPostTweetContext
                {
                    CharacterToActAs = new AIManager.CharacterPostTweet.CharacterToActAsDto
                    {
                        Id = character.Id,
                        Name = character.Name,
                        BirthDate = activeBio?.BirthDate ?? DateTimeOffset.MinValue,
                        Bio = activeBio?.Bio,
                        Personality = activeBio?.Personality,
                        Appearance = activeBio?.Appearance,
                        Occupation = activeBio?.Occupation,
                        Motivations = activeBio?.Motivations,
                        Fears = activeBio?.Fears,
                        Values = activeBio?.Values,
                        SpeechPatterns = activeBio?.SpeechPatterns,
                        Skills = activeBio?.Skills,
                        Backstory = activeBio?.Backstory,
                        PublicPersona = activeBio?.PublicPersona,
                        PrivateSelf = activeBio?.PrivateSelf,
                        MediaPresence = activeBio?.MediaPresence,
                        CrisisBehavior = activeBio?.CrisisBehavior,
                        Relationships = activeBio?.Relationships,
                        TechDetails = activeBio?.TechDetails
                    },
                    PreviousCharacterTweets = lastTweets?.Select(x => new AIManager.CharacterPostTweet.Tweet
                    {
                        TweetContent = x.MemoryContent,
                        TweetDate = x.PlatformInteractionDate
                    }).ToList(),
                    TwitterAutoPostExamples = character.TwitterAutoPostExamples,
                    TwitterAutoPostInstruction = character.TwitterAutoPostInstruction
                };

                return tweetContext;

            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                throw new UserFriendlyException("An error occurred while fetching the prompt context.", ex);
            }
        }


        // public async Task<string> GetPromptInput(Guid memoryId, AIPromptType promptType)
        // {
        //     if (promptType == AIPromptType.ReplyMentionedTweet)
        //     {
        //         var context = await GetPromptContextMentioned(memoryId, promptType);
        //         var prompt = await _aiManager.GenerateMentionedPromptAsync(context);
        //         return prompt;
        //     }

        //     else if (promptType == AIPromptType.PostCharacterTweet)
        //     {
        //         var context = await GetPromptContextPostTweet(memoryId, promptType);
        //         var prompt = await _aiManager.GeneratePostTweetPromptAsync(context);
        //     }

        //     return string.Empty;
        // }

        // public async Task<MemoryPrompt> GeneratePromptResponse(Guid memoryId, AIPromptType promptType, AIModelType modelType)
        // {
        //     MemoryPrompt memoryPrompt = new MemoryPrompt
        //     {
        //         TenantId = _tenantId,
        //         MemoryId = memoryId,
        //         PromptType = promptType,
        //         InputContextModel = nameof(AICharacterMentionedContext),
        //         ResponseModel = nameof(AICharacterMentionedResponse),
        //         GeneratedAt = DateTimeOffset.Now
        //     };

        //     AICharacterMentionedContext context;
        //     try
        //     {
        //         context = await GetPromptContext(memoryId, promptType);
        //         memoryPrompt.InputContextJson = JsonConvert.SerializeObject(context, Formatting.Indented);
        //     }
        //     catch (Exception ex)
        //     {
        //         memoryPrompt.IsSuccess = false;
        //         memoryPrompt.Exception = "Error generating context for prompt";
        //         memoryPrompt.ExceptionMessage = ex.Message;
        //         return await StorePrompt(memoryPrompt);
        //     }

        //     string textInput;
        //     try
        //     {
        //         textInput = await GetPromptInput(memoryId, promptType);
        //         memoryPrompt.InputFullText = textInput;
        //     }
        //     catch (Exception ex)
        //     {
        //         memoryPrompt.IsSuccess = false;
        //         memoryPrompt.Exception = "Error generating input for prompt";
        //         memoryPrompt.ExceptionMessage = ex.Message;
        //         return await StorePrompt(memoryPrompt);
        //     }

        //     AICharacterMentionedResponse response;
        //     try
        //     {
        //         response = await _aiManager.GenerateMentionedResponseAsync(context, modelType);

        //         if (response.IsSuccess && response.Scores != null)
        //         {
        //             var memory = await GetMemory(memoryId);
        //             if (memory != null && memory.MemoryStatsTwitter != null)
        //             {
        //                 memory.MemoryStatsTwitter.RelevanceScore = response.Scores.Relevance;
        //                 memory.MemoryStatsTwitter.DepthScore = response.Scores.Depth;
        //                 memory.MemoryStatsTwitter.SentimentScore = response.Scores.Sentiment;
        //                 if (response.CryptocoinsFound != null && response.CryptocoinsFound.Count > 0)
        //                 {
        //                     memory.Tags = string.Join(", ", response.CryptocoinsFound);
        //                 }

        //                 await UpdateMemory(memory);
        //             }
        //         }

        //         memoryPrompt.ResponseJson = JsonConvert.SerializeObject(response, Formatting.Indented);

        //         memoryPrompt.IsSuccess = response.IsSuccess;
        //         memoryPrompt.Exception = response.Exception;
        //         memoryPrompt.ExceptionMessage = response.ExceptionMessage;
        //     }
        //     catch (Exception ex)
        //     {
        //         memoryPrompt.IsSuccess = false;
        //         memoryPrompt.Exception = "Error generating response for prompt";
        //         memoryPrompt.ExceptionMessage = ex.Message;

        //         memoryPrompt = await StorePrompt(memoryPrompt);
        //         //await SetMemoryPromptGenerated(memoryId);

        //         return memoryPrompt;

        //     }

        //     return await StorePrompt(memoryPrompt);
        // }





    }
}