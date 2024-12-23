using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Services;
using Icon.Matrix.Models;
using Microsoft.EntityFrameworkCore;


namespace Icon.Matrix
{
    public partial interface IMemoryManager : IDomainService
    {

        Task<Memory> StoreCharacterTweets(
            Guid characterId,
            string conversationId,
            string tweetId,
            string tweetContent,
            string tweetUrl,
            DateTimeOffset? platformInteractionDate
        );

        Task<Memory> StoreCharacterReplyTweets(
            Guid characterId,
            string conversationId,
            string tweetId,
            string tweetContent
        );

        Task<Memory> StoreCharacterPersonaTweets(
            Guid characterId,
            string personaPlatformId,
            string personaName,
            string conversationId,
            string tweetId,
            string tweetContent,
            string tweetUrl,
            DateTimeOffset? platformInteractionDate
        );

        Task<Memory> StoreCharacterMentionedTweets(
            Guid characterId,
            string mentionByPersonaPlatformId,
            string mentionedByPersonaName,
            string conversationId,
            string tweetId,
            string tweetContent,
            string tweetUrl,
            DateTimeOffset? platformInteractionDate,
            MemoryStatsTwitter memoryStatsTwitter
        );

    }
    public partial class MemoryManager : IconServiceBase, IMemoryManager
    {
        public async Task<Memory> StoreCharacterTweets(
            Guid characterId,
            string conversationId,
            string tweetId,
            string tweetContent,
            string tweetUrl,
            DateTimeOffset? platformInteractionDate
        )
        {
            var characterBioId = await _characterManager.GetActiveCharacterBioId(characterId);
            var platformId = await _platformManager.GetPlatformId("Twitter");

            var memory = new Memory
            {
                TenantId = _tenantId,
                CharacterId = characterId,
                CharacterBioId = characterBioId,
                MemoryTypeId = await GetMemoryTypeId("CharacterTweet"),
                MemoryUrl = tweetUrl,
                MemoryContent = tweetContent,
                PlatformId = platformId,
                PlatformInteractionId = tweetId,
                PlatformInteractionParentId = conversationId,
                PlatformInteractionDate = platformInteractionDate,
                CreatedAt = DateTimeOffset.Now
            };

            var existingMemory = await _memoryRepository.FirstOrDefaultAsync(m =>
                m.CharacterId == characterId &&
                m.PlatformInteractionId == tweetId
            );

            if (existingMemory != null)
            {
                existingMemory.MemoryContent = tweetContent;
                existingMemory.MemoryUrl = tweetUrl;
                existingMemory.PlatformInteractionParentId = conversationId;
                memory = await UpdateMemory(existingMemory);
            }
            else
            {
                memory = await CreateMemory(memory);
            }

            return memory;
        }

        // store CharacterReplyTweet
        public async Task<Memory> StoreCharacterReplyTweets(
            Guid characterId,
            string conversationId,
            string tweetId,
            string tweetContent
        )
        {
            var characterBioId = await _characterManager.GetActiveCharacterBioId(characterId);
            var platformId = await _platformManager.GetPlatformId("Twitter");

            var memory = new Memory
            {
                TenantId = _tenantId,
                CharacterId = characterId,
                CharacterBioId = characterBioId,
                MemoryTypeId = await GetMemoryTypeId("CharacterReplyTweet"),
                MemoryContent = tweetContent,
                PlatformId = platformId,
                PlatformInteractionId = tweetId,
                PlatformInteractionParentId = conversationId,
                PlatformInteractionDate = DateTimeOffset.Now,
                CreatedAt = DateTimeOffset.Now
            };

            var existingMemory = await _memoryRepository.FirstOrDefaultAsync(m =>
                m.CharacterId == characterId &&
                m.PlatformInteractionId == tweetId
            );

            if (existingMemory != null)
            {
                existingMemory.MemoryContent = tweetContent;
                existingMemory.PlatformInteractionParentId = conversationId;
                memory = await UpdateMemory(existingMemory);
            }
            else
            {
                memory = await CreateMemory(memory);
            }

            return memory;
        }

        // store CharacterPersonaTweet
        public async Task<Memory> StoreCharacterPersonaTweets(
            Guid characterId,
            string personaPlatformId,
            string personaName,
            string conversationId,
            string tweetId,
            string tweetContent,
            string tweetUrl,
            DateTimeOffset? platformInteractionDate
        )
        {
            var characterBioId = await _characterManager.GetActiveCharacterBioId(characterId);
            var platformId = await _platformManager.GetPlatformId("Twitter");
            var characterPersonaId = await _characterManager.GetCharacterPersonaId(characterId, platformId, personaPlatformId, personaName);

            var memory = new Memory
            {
                TenantId = _tenantId,
                CharacterId = characterId,
                CharacterBioId = characterBioId,
                MemoryTypeId = await GetMemoryTypeId("CharacterPersonaTweet"),
                MemoryUrl = tweetUrl,
                MemoryContent = tweetContent,
                CharacterPersonaId = characterPersonaId,
                PlatformId = platformId,
                PlatformInteractionId = tweetId,
                PlatformInteractionParentId = conversationId,
                PlatformInteractionDate = platformInteractionDate,
                CreatedAt = DateTimeOffset.Now,
            };

            var existingMemory = await _memoryRepository.FirstOrDefaultAsync(m =>
                m.CharacterId == characterId &&
                m.CharacterPersonaId == characterPersonaId &&
                m.PlatformInteractionId == tweetId
            );

            if (existingMemory != null)
            {
                existingMemory.MemoryContent = tweetContent;
                existingMemory.MemoryUrl = tweetUrl;
                existingMemory.PlatformInteractionParentId = conversationId;
                memory = await UpdateMemory(existingMemory);
            }
            else
            {
                memory = await CreateMemory(memory);
            }

            return memory;
        }

        public async Task<Memory> StoreCharacterMentionedTweets(
            Guid characterId,
            string mentionByPersonaPlatformId,
            string mentionedByPersonaName,
            string conversationId,
            string tweetId,
            string tweetContent,
            string tweetUrl,
            DateTimeOffset? platformInteractionDate,
            MemoryStatsTwitter memoryStatsTwitter
        )
        {
            var characterBioId = await _characterManager.GetActiveCharacterBioId(characterId);
            var platformId = await _platformManager.GetPlatformId("Twitter");
            var characterPersonaId = await _characterManager.GetCharacterPersonaId(characterId, platformId, mentionByPersonaPlatformId, mentionedByPersonaName);

            var memory = new Memory
            {
                TenantId = _tenantId,
                CharacterId = characterId,
                CharacterBioId = characterBioId,
                CharacterPersonaId = characterPersonaId,
                MemoryTypeId = await GetMemoryTypeId("CharacterMentionedTweet"),
                MemoryUrl = tweetUrl,
                MemoryContent = tweetContent,
                PlatformId = platformId,
                PlatformInteractionId = tweetId,
                PlatformInteractionParentId = conversationId,
                PlatformInteractionDate = platformInteractionDate,
                CreatedAt = DateTimeOffset.Now,
                MemoryStatsTwitter = memoryStatsTwitter
            };


            var existingMemory = await _memoryRepository
                .GetAll()
                .Include(m => m.MemoryStatsTwitter)
                .Where(m =>
                    m.CharacterId == characterId &&
                    m.PlatformInteractionId == tweetId
                )
                .FirstOrDefaultAsync();

            if (existingMemory != null)
            {
                existingMemory.MemoryContent = tweetContent;
                existingMemory.MemoryUrl = tweetUrl;
                existingMemory.PlatformInteractionParentId = conversationId;
                existingMemory.PlatformInteractionDate = platformInteractionDate;

                if (existingMemory.MemoryStatsTwitter == null)
                {
                    existingMemory.MemoryStatsTwitter = memoryStatsTwitter;
                }
                else
                {
                    existingMemory.MemoryStatsTwitter.IsPin = memoryStatsTwitter.IsPin;
                    existingMemory.MemoryStatsTwitter.IsQuoted = memoryStatsTwitter.IsQuoted;
                    existingMemory.MemoryStatsTwitter.IsReply = memoryStatsTwitter.IsReply;
                    existingMemory.MemoryStatsTwitter.IsRetweet = memoryStatsTwitter.IsRetweet;
                    existingMemory.MemoryStatsTwitter.SensitiveContent = memoryStatsTwitter.SensitiveContent;
                    existingMemory.MemoryStatsTwitter.BookmarkCount = memoryStatsTwitter.BookmarkCount;
                    existingMemory.MemoryStatsTwitter.Likes = memoryStatsTwitter.Likes;
                    existingMemory.MemoryStatsTwitter.Replies = memoryStatsTwitter.Replies;
                    existingMemory.MemoryStatsTwitter.Retweets = memoryStatsTwitter.Retweets;
                    existingMemory.MemoryStatsTwitter.Views = memoryStatsTwitter.Views;
                    existingMemory.MemoryStatsTwitter.TweetWordCount = memoryStatsTwitter.TweetWordCount;
                    existingMemory.MemoryStatsTwitter.MentionsCount = memoryStatsTwitter.MentionsCount;
                }

                memory = await UpdateMemory(existingMemory);
            }
            else
            {
                memory = await CreateMemory(memory);
            }

            return memory;
        }
    }
}