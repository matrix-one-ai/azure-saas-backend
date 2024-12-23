using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
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
            var twitterAgentId = character.TwitterPostAgentId;
            if (string.IsNullOrEmpty(twitterAgentId))
            {
                throw new Exception($"Twitter agent id for character {character.Id} is missing.");
            }

            return await _twitterCommunicationService.ReplyToTweetAsync(twitterAgentId, tweetId, text);
        }

        public async Task<TwitterApiPostTweetResponse> PostTweetAsync(Character character, string text)
        {
            var twitterAgentId = character.TwitterPostAgentId;
            if (string.IsNullOrEmpty(twitterAgentId))
            {
                throw new Exception($"Twitter agent id for character {character.Id} is missing.");
            }

            return await _twitterCommunicationService.PostTweetAsync(twitterAgentId, text);
        }

    }
}
