using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Icon.Matrix.TwitterManager
{
    public class TwitterApiPostTweetResponse
    {
        [JsonPropertyName("data")]
        public TwitterApiPostTweetResponseData Data { get; set; }
    }

    public class TwitterApiPostTweetResponseData
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("text")]
        public string Text { get; set; }
    }

}