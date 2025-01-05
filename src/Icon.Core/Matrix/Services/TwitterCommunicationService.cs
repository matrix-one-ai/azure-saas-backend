using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Abp.Dependency;
using Icon.Configuration;
using Icon.Matrix.TwitterManager;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Icon.Matrix.Twitter
{
    public interface ITwitterCommunicationService
    {
        Task<TwitterApiPostTweetResponse> PostTweetAsync(string twitterAgentId, string text);
        Task<TwitterApiPostTweetResponse> ReplyToTweetAsync(string twitterAgentId, string tweetId, string text);
        Task<List<TwitterScraperTweetResponse>> GetTweetsAsync(string twitterAgentId, string username, int count);
        Task<List<TwitterScraperTweetResponse>> GetTweetsFromDateAsync(string twitterAgentId, string username, DateTime fromDate, int limit = 5);
        Task<List<TwitterScraperTweetResponse>> GetTweetsByKeywordsAsync(string twitterAgentId, List<string> keywords, int limit = 5);
        Task<List<TwitterScraperTweetResponse>> GetUserMentionsAsync(string twitterAgentId, string username, int limit = 5);
        Task<List<string>> GetTrendingTopicsAsync(string twitterAgentId, int limit = 5);
        Task<TwitterScraperUserProfileResponse> GetUserProfileAsync(string twitterAgentId, string username);
        Task<List<TwitterScraperTweetResponse>> GetRepliesToTweetAsync(string twitterAgentId, string tweetId, int limit = 5);

        Task<TwitterScraperPostTweetResponse> PostScraperTweetAsync(string twitterAgentId, string text);
        Task<TwitterScraperPostTweetResponse> ReplyToScraperTweetAsync(string twitterAgentId, string tweetId, string text);

    }

    public class TwitterCommunicationService : ITwitterCommunicationService, ITransientDependency
    {
        private static HttpClient _httpClient;
        private readonly ILogger<TwitterCommunicationService> _logger;
        private IConfigurationRoot _configuration;

        public TwitterCommunicationService(
            ILogger<TwitterCommunicationService> logger,
            IAppConfigurationAccessor appConfigurationAccessor)
        {
            _logger = logger;
            _configuration = appConfigurationAccessor.Configuration;

            InitializeHttpClient();
        }

        private void InitializeHttpClient()
        {
            if (_httpClient == null)
            {
                var baseAddress = _configuration["MatrixTwitter:TwitterApiBaseAddress"];
                var apiKey = _configuration["MatrixTwitter:TwitterApiKey"];

                if (string.IsNullOrEmpty(baseAddress))
                {
                    _logger.LogError("Twitter API base address is missing.");
                    throw new Exception("Twitter API base address is missing.");
                }
                if (string.IsNullOrEmpty(apiKey))
                {
                    _logger.LogError("Twitter API key is missing.");
                    throw new Exception("Twitter API key is missing.");
                }

                _httpClient = new HttpClient
                {
                    BaseAddress = new Uri(baseAddress)
                };

                _httpClient.DefaultRequestHeaders.Add("x-api-key", apiKey);
                _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            }
        }

        public Task<TwitterApiPostTweetResponse> PostTweetAsync(string twitterAgentId, string text)
        {
            return ExecuteRequestAsync<TwitterApiPostTweetResponse>(
                () => _httpClient.PostAsJsonAsync("/postTweet", new { agentId = twitterAgentId, text }),
                $"PostTweetAsync for agentId: {twitterAgentId}"
            );
        }

        public Task<TwitterApiPostTweetResponse> ReplyToTweetAsync(string twitterAgentId, string tweetId, string text)
        {
            return ExecuteRequestAsync<TwitterApiPostTweetResponse>(
                () => _httpClient.PostAsJsonAsync("/replyToTweet", new { agentId = twitterAgentId, tweetId, text }),
                $"ReplyToTweetAsync for tweetId: {tweetId}"
            );
        }

        private async Task<T> ExecuteRequestAsync<T>(Func<Task<HttpResponseMessage>> requestFunc, string operationDescription)
        {
            try
            {
                var response = await requestFunc();
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<T>() ?? Activator.CreateInstance<T>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error during {operationDescription}: {ex.Message}");
                return Activator.CreateInstance<T>();
            }
        }

        public Task<List<TwitterScraperTweetResponse>> GetTweetsAsync(string twitterAgentId, string username, int count)
        {
            return ExecuteRequestAsync<List<TwitterScraperTweetResponse>>(
                () => _httpClient.PostAsJsonAsync("/getScraperTweets", new { agentId = twitterAgentId, username, count }),
                $"GetTweetsAsync for username: {username}"
            );
        }

        public Task<List<TwitterScraperTweetResponse>> GetTweetsFromDateAsync(string twitterAgentId, string username, DateTime fromDate, int limit = 5)
        {
            return ExecuteRequestAsync<List<TwitterScraperTweetResponse>>(
                () => _httpClient.PostAsJsonAsync("/getScraperUserTweetsByDate", new { agentId = twitterAgentId, username, fromDate = fromDate.ToString("o"), limit }),
                $"GetTweetsFromDateAsync for username: {username} from date: {fromDate}"
            );
        }

        public Task<List<TwitterScraperTweetResponse>> GetTweetsByKeywordsAsync(string twitterAgentId, List<string> keywords, int limit = 5)
        {
            return ExecuteRequestAsync<List<TwitterScraperTweetResponse>>(
                () => _httpClient.PostAsJsonAsync("/getScraperTweetsByKeywords", new { agentId = twitterAgentId, keywords, limit }),
                $"GetTweetsByKeywordsAsync with keywords: {string.Join(", ", keywords)}"
            );
        }

        public Task<List<TwitterScraperTweetResponse>> GetUserMentionsAsync(string twitterAgentId, string username, int limit = 5)
        {
            return ExecuteRequestAsync<List<TwitterScraperTweetResponse>>(
                () => _httpClient.PostAsJsonAsync("/getScraperUserMentions", new { agentId = twitterAgentId, username, limit }),
                $"GetUserMentionsAsync for username: {username}"
            );
        }

        public Task<List<string>> GetTrendingTopicsAsync(string twitterAgentId, int limit = 5)
        {
            return ExecuteRequestAsync<List<string>>(
                () => _httpClient.PostAsJsonAsync("/getScraperTrendingTopics", new { agentId = twitterAgentId, limit }),
                $"GetTrendingTopicsAsync with limit: {limit}"
            );
        }

        public Task<TwitterScraperUserProfileResponse> GetUserProfileAsync(string twitterAgentId, string username)
        {
            return ExecuteRequestAsync<TwitterScraperUserProfileResponse>(
                () => _httpClient.PostAsJsonAsync("/getScraperUserProfileData", new { agentId = twitterAgentId, username }),
                $"GetUserProfileAsync for username: {username}"
            );
        }

        public Task<List<TwitterScraperTweetResponse>> GetRepliesToTweetAsync(string twitterAgentId, string tweetId, int limit = 5)
        {
            return ExecuteRequestAsync<List<TwitterScraperTweetResponse>>(
                () => _httpClient.PostAsJsonAsync("/getScraperRepliesToTweet", new { agentId = twitterAgentId, tweetId, limit }),
                $"GetRepliesToTweetAsync for tweetId: {tweetId}"
            );
        }

        public Task<TwitterScraperPostTweetResponse> PostScraperTweetAsync(string twitterAgentId, string text)
        {
            return ExecuteRequestAsync<TwitterScraperPostTweetResponse>(
                () => _httpClient.PostAsJsonAsync("/postScraperTweet", new { agentId = twitterAgentId, text }),
                $"PostScraperTweetAsync for agentId: {twitterAgentId}"
            );
        }

        public Task<TwitterScraperPostTweetResponse> ReplyToScraperTweetAsync(string twitterAgentId, string tweetId, string text)
        {
            return ExecuteRequestAsync<TwitterScraperPostTweetResponse>(
                () => _httpClient.PostAsJsonAsync("/replyToScraperTweet", new { agentId = twitterAgentId, tweetId, text }),
                $"ReplyToScraperTweetAsync for tweetId: {tweetId}"
            );
        }
    }

}
