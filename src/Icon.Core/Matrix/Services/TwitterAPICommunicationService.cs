using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Icon.Configuration;
using Icon.Matrix.Models;
using Icon.Matrix.TwitterManager;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Stripe.TestHelpers;
using Tweetinvi;
using Tweetinvi.Models.V2;
using Tweetinvi.Parameters.V2;

namespace Icon.Matrix.Twitter
{
    public interface ITwitterAPICommunicationService
    {
        Task<TwitterApiEngagementResponse> GetEngagementDataTotalAsync(string searchQuery);
        Task<List<TweetV2>> GetTweetsFullEngagementAsync(string searchQuery, int limit = 100);
        Task<TwitterApiPostCountsResponse> GetTweetCountsAsync(string searchQuery);
        Task<List<TweetV2>> GetTweetsByKeywordInTimeRangeAsync(string searchQuery, DateTime startTime, DateTime endTime, int limit = 100);
    }

    public class TwitterAPICommunicationService : ITwitterAPICommunicationService, ITransientDependency
    {
        private readonly ILogger<TwitterAPICommunicationService> _logger;
        private readonly IRepository<TwitterAPIUsage, Guid> _twitterAPIUsageRepository;
        private IConfigurationRoot _configuration;
        private readonly TwitterClient _twitterClient;
        private readonly string _bearerToken;

        public TwitterAPICommunicationService(
            ILogger<TwitterAPICommunicationService> logger,
            IAppConfigurationAccessor appConfigurationAccessor,
            IRepository<TwitterAPIUsage, Guid> twitterAPIUsageRepository)
        {
            _logger = logger;
            _configuration = appConfigurationAccessor.Configuration;
            _twitterAPIUsageRepository = twitterAPIUsageRepository;

            var consumerKey = _configuration["MatrixTwitter:ConsumerKey"];
            var consumerSecret = _configuration["MatrixTwitter:ConsumerSecret"];
            var accessToken = _configuration["MatrixTwitter:AccessToken"];
            var accessSecret = _configuration["MatrixTwitter:AccessSecret"];

            _bearerToken = _configuration["MatrixTwitter:BearerToken"] ?? string.Empty;

            var userClient = new TwitterClient(consumerKey, consumerSecret, accessToken, accessSecret);
            _twitterClient = userClient;
        }



        public async Task<List<TweetV2>> GetTweetsByKeywordInTimeRangeAsync(
            string searchQuery,
            DateTime startTime,
            DateTime endTime,
            int limit = 100)
        {
            var allTweets = new List<TweetV2>();
            string nextToken = null;

            var adjustedSearchQuery = $"{searchQuery} -is:retweet"; // Exclude retweets from search results

            try
            {
                do
                {
                    var searchParams = new SearchTweetsV2Parameters(adjustedSearchQuery)
                    {
                        // Tweetinvi v2 parameters for time range:
                        StartTime = startTime,
                        EndTime = endTime,

                        // e.g. up to 100 tweets per request
                        //PageSize = Math.Min(100, limit - allTweets.Count), // Adjust page size to remaining limit

                        // Request tweet fields
                        //TweetFields = { "public_metrics", "created_at" },

                        // For pagination beyond the first page
                        //NextToken = nextToken
                    };

                    var searchResponse = await _twitterClient.SearchV2.SearchTweetsAsync(searchParams);
                    if (searchResponse == null || searchResponse.Tweets == null)
                    {
                        break;
                    }

                    // Append current page of tweets
                    allTweets.AddRange(searchResponse.Tweets);

                    // Try to get a next token (if there are more pages)
                    nextToken = null;// searchResponse.SearchMetadata?.NextToken;

                    // Break if we reached the limit
                    if (allTweets.Count >= limit)
                    {
                        break;
                    }
                } while (!string.IsNullOrEmpty(nextToken));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error searching tweets for '{searchQuery}' between {startTime} and {endTime}: {ex.Message}");
                throw;
            }

            // Ensure we don't exceed the limit, in case the last page went slightly over
            return allTweets.Take(limit).ToList();
        }


        public async Task<TwitterApiEngagementResponse> GetEngagementDataTotalAsync(string searchQuery)
        {
            var searchParams = new SearchTweetsV2Parameters(searchQuery)
            {
                TweetFields = { "public_metrics", "created_at" },
                PageSize = 100
            };

            var searchResponse = await _twitterClient.SearchV2.SearchTweetsAsync(searchParams);
            var tweets = searchResponse.Tweets;
            var tweetCount = tweets?.Count() ?? 0;

            int totalLikes = 0;
            int totalRetweets = 0;
            int totalReplies = 0;
            int totalQuotes = 0;

            if (tweets != null)
            {
                foreach (var tweet in tweets)
                {
                    if (tweet.PublicMetrics != null)
                    {
                        totalLikes += tweet.PublicMetrics.LikeCount;
                        totalRetweets += tweet.PublicMetrics.RetweetCount;
                        totalReplies += tweet.PublicMetrics.ReplyCount;
                        totalQuotes += tweet.PublicMetrics.QuoteCount;
                    }
                }
            }

            return new TwitterApiEngagementResponse
            {
                TweetCount = tweetCount,
                LikeCount = totalLikes,
                RetweetCount = totalRetweets,
                ReplyCount = totalReplies,
                QuoteCount = totalQuotes
            };
        }

        public async Task<TwitterApiPostCountsResponse> GetTweetCountsAsync(string searchQuery)
        {
            var url = $"https://api.twitter.com/2/tweets/counts/recent?query={Uri.EscapeDataString(searchQuery)}";

            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_bearerToken}");

            HttpResponseMessage response = null;
            string responseBody = null;

            try
            {
                response = await httpClient.GetAsync(url);
                responseBody = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    var errorMsg = $"Failed to get tweet counts for '{searchQuery}'. " +
                                   $"StatusCode: {response.StatusCode}, Reason: {response.ReasonPhrase}";
                    _logger.LogError(errorMsg);
                    response.EnsureSuccessStatusCode(); // Will throw
                }

                await TrackApiUsageAsync("counts/recent", searchQuery, response, responseBody);

                var countsResponse = System.Text.Json.JsonSerializer
                    .Deserialize<TwitterApiPostCountsResponse>(responseBody);

                if (countsResponse == null)
                    return new TwitterApiPostCountsResponse();

                return countsResponse;
            }
            catch (Exception ex)
            {
                var err = $"Error fetching tweet counts for '{searchQuery}': {ex.Message}";
                _logger.LogError(ex, err);
                if (response != null)
                    await TrackApiUsageAsync("counts/recent", searchQuery, response, responseBody, err);
                throw;
            }

            // try
            // {
            //     var response = await httpClient.GetAsync(url);
            //     if (!response.IsSuccessStatusCode)
            //     {
            //         var errorMsg = $"Failed to get tweet counts for '{searchQuery}'. " +
            //                        $"StatusCode: {response.StatusCode}, Reason: {response.ReasonPhrase}";
            //         _logger.LogError(errorMsg);
            //         response.EnsureSuccessStatusCode(); // Will throw
            //     }

            //     // Deserialize the response into a DTO that matches the structure from the docs
            //     var countsResponse = await response.Content.ReadFromJsonAsync<TwitterApiPostCountsResponse>();

            //     if (countsResponse == null)
            //     {
            //         // Return an empty object or throw, depending on your preference
            //         return new TwitterApiPostCountsResponse();
            //     }

            //     return countsResponse;
            // }
            // catch (Exception ex)
            // {
            //     _logger.LogError(ex, $"Error fetching tweet counts for '{searchQuery}': {ex.Message}");
            //     throw;
            // }
        }

        public async Task<List<TweetV2>> GetTweetsFullEngagementAsync(string searchQuery, int limit = 100)
        {
            var allTweets = new List<TweetV2>();
            string nextToken = null;

            do
            {
                var searchParams = new SearchTweetsV2Parameters(searchQuery)
                {
                    PageSize = 100,
                    TweetFields = { "public_metrics", "created_at", "author_id" },
                    NextToken = nextToken,
                };

                var searchResponse = await _twitterClient.SearchV2.SearchTweetsAsync(searchParams);
                if (searchResponse?.Tweets == null) break;

                allTweets.AddRange(searchResponse.Tweets);
                nextToken = searchResponse.SearchMetadata?.NextToken;

            } while (!string.IsNullOrEmpty(nextToken) && allTweets.Count < limit);

            return allTweets;
        }


        private async Task TrackApiUsageAsync(
            string endpoint,
            string query,
            HttpResponseMessage response,
            string responseBody = null,
            string errorMessage = null)
        {
            try
            {
                var usage = new TwitterAPIUsage
                {
                    Id = Guid.NewGuid(),
                    RequestTime = DateTimeOffset.UtcNow,
                    Endpoint = endpoint,
                    Query = query,
                    StatusCode = (int)response.StatusCode,
                    ResponseBody = responseBody,
                    ErrorMessage = errorMessage,
                };

                // If Twitter provided rate-limit headers, parse them
                if (response.Headers.Contains("x-rate-limit-remaining"))
                {
                    usage.RateLimitRemaining = int.Parse(response.Headers.GetValues("x-rate-limit-remaining").FirstOrDefault() ?? "0");
                }
                if (response.Headers.Contains("x-rate-limit-limit"))
                {
                    usage.RateLimitLimit = int.Parse(response.Headers.GetValues("x-rate-limit-limit").FirstOrDefault() ?? "0");
                }
                if (response.Headers.Contains("x-rate-limit-reset"))
                {
                    var resetSec = long.Parse(response.Headers.GetValues("x-rate-limit-reset").FirstOrDefault() ?? "0");
                    usage.RateLimitResetTime = DateTimeOffset.FromUnixTimeSeconds(resetSec);
                }

                await _twitterAPIUsageRepository.InsertAsync(usage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error tracking Twitter API usage.");
            }
        }

    }

}

