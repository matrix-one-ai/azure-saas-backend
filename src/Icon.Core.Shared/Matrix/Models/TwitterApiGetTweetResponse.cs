using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;



namespace Icon.Matrix.TwitterManager
{
    public class TwitterApiGetTweetResponse
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime? CreatedAt { get; set; }

        [JsonPropertyName("text")]
        public string Text { get; set; }

        [JsonPropertyName("author_id")]
        public string AuthorId { get; set; }

        [JsonPropertyName("conversation_id")]
        public string ConversationId { get; set; }

        [JsonPropertyName("authorName")]
        public string AuthorName { get; set; }

        [JsonPropertyName("authorUsername")]
        public string AuthorUsername { get; set; }

        [JsonPropertyName("authorProfileImage")]
        public string AuthorProfileImage { get; set; }

        [JsonPropertyName("public_metrics")]
        public TwitterPublicMetrics PublicMetrics { get; set; }

        [JsonPropertyName("edit_history_tweet_ids")]
        public List<string> EditHistoryTweetIds { get; set; }
    }

    public class TwitterPublicMetrics
    {
        [JsonPropertyName("retweet_count")]
        public int RetweetCount { get; set; }

        [JsonPropertyName("reply_count")]
        public int ReplyCount { get; set; }

        [JsonPropertyName("like_count")]
        public int LikeCount { get; set; }

        [JsonPropertyName("quote_count")]
        public int QuoteCount { get; set; }

        [JsonPropertyName("bookmark_count")]
        public int BookmarkCount { get; set; }

        [JsonPropertyName("impression_count")]
        public int ImpressionCount { get; set; }
    }
}


