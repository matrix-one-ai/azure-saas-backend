using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Icon.Matrix.Enums;
using Icon.Matrix.Models;
using Icon.Matrix.TwitterManager;
using Microsoft.EntityFrameworkCore;
using PayPalCheckoutSdk.Orders;

namespace Icon.Matrix.Twitter
{
    public interface ITwitterManager
    {
        Task<TwitterApiPostTweetResponse> ReplyToTweetAsync(Character character, string tweetId, string text);
        Task<TwitterApiPostTweetResponse> PostTweetAsync(Character character, string text);
    }

    public class TwitterManager : ITwitterManager, ITransientDependency
    {
        private readonly ITwitterCommunicationService _twitterCommunicationService;

        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public TwitterManager(
            ITwitterCommunicationService twitterCommunicationService,
            IUnitOfWorkManager unitOfWorkManager)

        {
            _twitterCommunicationService = twitterCommunicationService;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public async Task<TwitterApiPostTweetResponse> ReplyToTweetAsync(Character character, string tweetId, string text)
        {
            ValidateCharacter(character);
            ValidateTweetId(tweetId);
            ValidateText(text);

            var twitterAgentId = character.TwitterPostAgentId;

            if (character.TwitterCommType == TwitterCommType.TwitterApi)
            {
                return await _twitterCommunicationService.ReplyToTweetAsync(twitterAgentId, tweetId, text);
            }
            else if (character.TwitterCommType == TwitterCommType.TwitterScraper)
            {
                var result = await _twitterCommunicationService.ReplyToScraperTweetAsync(twitterAgentId, tweetId, text);

                return new TwitterApiPostTweetResponse
                {
                    Data = new TwitterApiPostTweetResponseData
                    {
                        Id = result.TweetId,
                        Text = result.Text
                    }
                };
            }
            else
            {
                throw new Exception($"Twitter communication type for character {character.Id} is missing.");
            }
        }

        public async Task<TwitterApiPostTweetResponse> PostTweetAsync(Character character, string text)
        {
            ValidateCharacter(character);
            ValidateText(text);

            var twitterAgentId = character.TwitterPostAgentId;

            if (character.TwitterCommType == TwitterCommType.TwitterApi)
            {
                return await _twitterCommunicationService.PostTweetAsync(twitterAgentId, text);
            }
            else if (character.TwitterCommType == TwitterCommType.TwitterScraper)
            {
                var result = await _twitterCommunicationService.PostScraperTweetAsync(twitterAgentId, text);

                return new TwitterApiPostTweetResponse
                {
                    Data = new TwitterApiPostTweetResponseData
                    {
                        Id = result.TweetId,
                        Text = result.Text
                    }
                };
            }
            else
            {
                throw new Exception($"Twitter communication type for character {character.Id} is missing.");
            }
        }

        private void ValidateCharacter(Character character)
        {
            if (character == null)
            {
                throw new Exception("Character is missing.");
            }

            if (string.IsNullOrEmpty(character.TwitterPostAgentId))
            {
                throw new Exception($"Twitter agent id for character {character.Id} is missing.");
            }

            if (character.TwitterCommType == TwitterCommType.Unset)
            {
                throw new Exception($"Twitter communication type for character {character.Id} is missing.");
            }
        }

        private void ValidateTweetId(string tweetId)
        {
            if (string.IsNullOrEmpty(tweetId))
            {
                throw new Exception("Tweet id is missing.");
            }
        }

        private void ValidateText(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new Exception("Text is missing.");
            }
        }

    }
}
