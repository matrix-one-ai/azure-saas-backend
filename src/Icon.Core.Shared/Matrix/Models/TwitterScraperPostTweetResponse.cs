using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Icon.Matrix.TwitterManager
{
    public class TwitterScraperPostTweetResponse
    {
        [JsonPropertyName("tweetId")]
        public string TweetId { get; set; }

        [JsonPropertyName("text")]
        public string Text { get; set; }
    }
}