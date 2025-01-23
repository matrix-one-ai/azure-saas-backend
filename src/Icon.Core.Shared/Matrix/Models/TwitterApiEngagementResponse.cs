using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Icon.Matrix.TwitterManager
{
    public class TwitterApiEngagementResponse
    {
        public int TweetCount { get; set; }
        public int LikeCount { get; set; }
        public int RetweetCount { get; set; }
        public int ReplyCount { get; set; }
        public int QuoteCount { get; set; }
    }

}