using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Icon.Matrix.TwitterManager
{


    public class TwitterScraperUserProfileResponse
    {
        [JsonPropertyName("avatar")]
        public string Avatar { get; set; }

        [JsonPropertyName("biography")]
        public string Biography { get; set; }

        [JsonPropertyName("followersCount")]
        public int? FollowersCount { get; set; }

        [JsonPropertyName("followingCount")]
        public int? FollowingCount { get; set; }

        [JsonPropertyName("friendsCount")]
        public int? FriendsCount { get; set; }

        [JsonPropertyName("mediaCount")]
        public int? MediaCount { get; set; }

        [JsonPropertyName("isPrivate")]
        public bool? IsPrivate { get; set; }

        [JsonPropertyName("isVerified")]
        public bool? IsVerified { get; set; }

        [JsonPropertyName("likesCount")]
        public int? LikesCount { get; set; }

        [JsonPropertyName("listedCount")]
        public int? ListedCount { get; set; }

        [JsonPropertyName("location")]
        public string Location { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("pinnedTweetIds")]
        public List<string> PinnedTweetIds { get; set; }

        [JsonPropertyName("tweetsCount")]
        public int? TweetsCount { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("userId")]
        public string UserId { get; set; }

        [JsonPropertyName("username")]
        public string Username { get; set; }

        [JsonPropertyName("isBlueVerified")]
        public bool? IsBlueVerified { get; set; }

        [JsonPropertyName("canDm")]
        public bool? CanDm { get; set; }

        [JsonPropertyName("joined")]
        public DateTime? Joined { get; set; }

    }


}