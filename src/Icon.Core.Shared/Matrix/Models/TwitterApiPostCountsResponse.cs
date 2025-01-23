using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Icon.Matrix.TwitterManager
{
    public class TwitterApiPostCountsResponse
    {
        [JsonPropertyName("data")]
        public TweetCountData[] Data { get; set; }

        [JsonPropertyName("meta")]
        public TweetCountMeta Meta { get; set; }
    }

    public class TweetCountData
    {
        [JsonPropertyName("tweet_count")]
        public int Tweet_Count { get; set; }
        [JsonPropertyName("start")]
        public DateTimeOffset Start { get; set; }
        [JsonPropertyName("end")]
        public DateTimeOffset End { get; set; }
    }

    public class TweetCountMeta
    {
        [JsonPropertyName("total_tweet_count")]
        public int Total_Tweet_Count { get; set; }
        // "next_token" if you want pagination, or "oldest", "newest", etc.
    }

}