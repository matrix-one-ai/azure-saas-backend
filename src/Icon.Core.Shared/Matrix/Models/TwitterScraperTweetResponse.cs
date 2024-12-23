using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Icon.Matrix.TwitterManager
{
    public class Profile
    {
        [JsonPropertyName("avatar")]
        public string Avatar { get; set; }

        [JsonPropertyName("banner")]
        public string Banner { get; set; }

        [JsonPropertyName("biography")]
        public string Biography { get; set; }

        [JsonPropertyName("birthday")]
        public string Birthday { get; set; }

        [JsonPropertyName("followersCount")]
        public int? FollowersCount { get; set; }

        [JsonPropertyName("followingCount")]
        public int? FollowingCount { get; set; }

        [JsonPropertyName("friendsCount")]
        public int? FriendsCount { get; set; }

        [JsonPropertyName("mediaCount")]
        public int? MediaCount { get; set; }

        [JsonPropertyName("statusesCount")]
        public int? StatusesCount { get; set; }

        [JsonPropertyName("isPrivate")]
        public bool? IsPrivate { get; set; }

        [JsonPropertyName("isVerified")]
        public bool? IsVerified { get; set; }

        [JsonPropertyName("isBlueVerified")]
        public bool? IsBlueVerified { get; set; }

        [JsonPropertyName("joined")]
        public DateTime? Joined { get; set; }

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

        [JsonPropertyName("website")]
        public string Website { get; set; }

        [JsonPropertyName("canDm")]
        public bool? CanDm { get; set; }
    }

    public class Mention
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("username")]
        public string Username { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public class Photo
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("alt_text")]
        public string AltText { get; set; }
    }

    public class Video
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("preview")]
        public string Preview { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }
    }

    public class PlaceRaw
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("place_type")]
        public string PlaceType { get; set; }

        [JsonPropertyName("name")]
        public string PlaceName { get; set; }

        [JsonPropertyName("full_name")]
        public string FullName { get; set; }

        [JsonPropertyName("country_code")]
        public string CountryCode { get; set; }

        [JsonPropertyName("country")]
        public string Country { get; set; }

        [JsonPropertyName("bounding_box")]
        public BoundingBox BoundingBox { get; set; }
    }

    public class BoundingBox
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        // coordinates is a nested array (multi-dimensional) of numbers
        [JsonPropertyName("coordinates")]
        public double[][][] Coordinates { get; set; }
    }

    public class PollV2
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("end_datetime")]
        public string EndDatetime { get; set; }

        [JsonPropertyName("voting_status")]
        public string VotingStatus { get; set; }

        [JsonPropertyName("duration_minutes")]
        public int DurationMinutes { get; set; }

        [JsonPropertyName("options")]
        public List<PollOption> Options { get; set; }
    }

    public class PollOption
    {
        [JsonPropertyName("position")]
        public int? Position { get; set; }

        [JsonPropertyName("label")]
        public string Label { get; set; }

        [JsonPropertyName("votes")]
        public int? Votes { get; set; }
    }

    public class TwitterScraperTweetResponse
    {
        [JsonPropertyName("bookmarkCount")]
        public int? BookmarkCount { get; set; }

        [JsonPropertyName("conversationId")]
        public string ConversationId { get; set; }

        [JsonPropertyName("hashtags")]
        public List<string> Hashtags { get; set; }

        [JsonPropertyName("html")]
        public string Html { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("inReplyToStatus")]
        public TwitterScraperTweetResponse InReplyToStatus { get; set; }

        [JsonPropertyName("inReplyToStatusId")]
        public string InReplyToStatusId { get; set; }

        [JsonPropertyName("isQuoted")]
        public bool? IsQuoted { get; set; }

        [JsonPropertyName("isPin")]
        public bool? IsPin { get; set; }

        [JsonPropertyName("isReply")]
        public bool? IsReply { get; set; }

        [JsonPropertyName("isRetweet")]
        public bool? IsRetweet { get; set; }

        [JsonPropertyName("isSelfThread")]
        public bool? IsSelfThread { get; set; }

        [JsonPropertyName("likes")]
        public int? Likes { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("mentions")]
        public List<Mention> Mentions { get; set; }

        [JsonPropertyName("permanentUrl")]
        public string PermanentUrl { get; set; }

        [JsonPropertyName("photos")]
        public List<Photo> Photos { get; set; }

        [JsonPropertyName("place")]
        public PlaceRaw Place { get; set; }

        [JsonPropertyName("quotedStatus")]
        public TwitterScraperTweetResponse QuotedStatus { get; set; }

        [JsonPropertyName("quotedStatusId")]
        public string QuotedStatusId { get; set; }

        [JsonPropertyName("replies")]
        public int? Replies { get; set; }

        [JsonPropertyName("retweets")]
        public int? Retweets { get; set; }

        [JsonPropertyName("retweetedStatus")]
        public TwitterScraperTweetResponse RetweetedStatus { get; set; }

        [JsonPropertyName("retweetedStatusId")]
        public string RetweetedStatusId { get; set; }

        [JsonPropertyName("text")]
        public string Text { get; set; }

        [JsonPropertyName("thread")]
        public List<TwitterScraperTweetResponse> Thread { get; set; }

        [JsonPropertyName("timeParsed")]
        public DateTime? TimeParsed { get; set; }

        [JsonPropertyName("timestamp")]
        public long? Timestamp { get; set; }

        [JsonPropertyName("urls")]
        public List<string> Urls { get; set; }

        [JsonPropertyName("userId")]
        public string UserId { get; set; }

        [JsonPropertyName("username")]
        public string Username { get; set; }

        [JsonPropertyName("videos")]
        public List<Video> Videos { get; set; }

        [JsonPropertyName("views")]
        public int? Views { get; set; }

        [JsonPropertyName("sensitiveContent")]
        public bool? SensitiveContent { get; set; }

        [JsonPropertyName("poll")]
        public PollV2 Poll { get; set; }
    }


}