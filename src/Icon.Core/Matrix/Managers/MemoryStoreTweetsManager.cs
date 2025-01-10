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
            var characterPersonaId = await _characterManager.GetCharacterPersonaId(
                characterId,
                platformId,
                mentionByPersonaPlatformId,
                mentionedByPersonaName
            );

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

            var existing = await _memoryRepository
                .GetAll()
                .Include(m => m.MemoryStatsTwitter)
                .Where(m => m.CharacterId == characterId && m.PlatformInteractionId == tweetId)
                .FirstOrDefaultAsync();

            if (existing != null)
            {
                existing.MemoryContent = tweetContent;
                existing.MemoryUrl = tweetUrl;
                existing.PlatformInteractionParentId = conversationId;
                existing.PlatformInteractionDate = platformInteractionDate;
                if (existing.MemoryStatsTwitter == null)
                {
                    existing.MemoryStatsTwitter = memoryStatsTwitter;
                }
                else
                {
                    existing.MemoryStatsTwitter.IsPin = memoryStatsTwitter.IsPin;
                    existing.MemoryStatsTwitter.IsQuoted = memoryStatsTwitter.IsQuoted;
                    existing.MemoryStatsTwitter.IsReply = memoryStatsTwitter.IsReply;
                    existing.MemoryStatsTwitter.IsRetweet = memoryStatsTwitter.IsRetweet;
                    existing.MemoryStatsTwitter.SensitiveContent = memoryStatsTwitter.SensitiveContent;
                    existing.MemoryStatsTwitter.BookmarkCount = memoryStatsTwitter.BookmarkCount;
                    existing.MemoryStatsTwitter.Likes = memoryStatsTwitter.Likes;
                    existing.MemoryStatsTwitter.Replies = memoryStatsTwitter.Replies;
                    existing.MemoryStatsTwitter.Retweets = memoryStatsTwitter.Retweets;
                    existing.MemoryStatsTwitter.Views = memoryStatsTwitter.Views;
                    existing.MemoryStatsTwitter.TweetWordCount = memoryStatsTwitter.TweetWordCount;
                    existing.MemoryStatsTwitter.MentionsCount = memoryStatsTwitter.MentionsCount;
                }
                return await UpdateMemory(existing);
            }
            else
            {
                return await CreateMemory(memory);
            }
        }
    }
}