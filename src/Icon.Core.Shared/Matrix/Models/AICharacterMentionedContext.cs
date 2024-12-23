using System;
using System.Collections.Generic;

namespace Icon.Matrix.AIManager.CharacterMentioned
{
    public class AICharacterMentionedContext
    {
        public CharacterToActAsDto CharacterToActAs { get; set; }
        public UserToRespondToDto UserToRespondTo { get; set; }
        public Tweet TweetToRespondTo { get; set; }
        public ConversationTimeLineDto ConversationTimeLine { get; set; }
        public List<Tweet> LastTweetsByUser { get; set; }
    }

    public class CharacterToActAsDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTimeOffset BirthDate { get; set; }
        public string Bio { get; set; }
        public string Personality { get; set; }
        public string Appearance { get; set; }
        public string Occupation { get; set; }
        public string Motivations { get; set; }
        public string Fears { get; set; }
        public string Values { get; set; }
        public string SpeechPatterns { get; set; }
        public string Skills { get; set; }
        public string Backstory { get; set; }
        public string PublicPersona { get; set; }
        public string PrivateSelf { get; set; }
        public string MediaPresence { get; set; }
        public string CrisisBehavior { get; set; }
        public string Relationships { get; set; }
        public string TechDetails { get; set; }
    }

    public class UserToRespondToDto
    {
        public string Name { get; set; }
        public string UserName { get; set; }
        public string AttitudeTowardsUser { get; set; }
        public string ExampleResponses { get; set; }

        public EngagementToWithCharacterDto EngagementTowardsCharacter { get; set; }
    }

    public class EngagementToWithCharacterDto
    {
        public int TotalTweets { get; set; }
        public int TotalLikes { get; set; }
        public int TotalReplies { get; set; }
        public int TotalRetweets { get; set; }
        public int TotalViews { get; set; }
        public int TotalEngagementScore { get; set; }


        public int TotalRelevanceScore { get; set; }
        public int TotalDepthScore { get; set; }
        public int TotalNoveltyScore { get; set; }
        public int TotalSentimentScore { get; set; }
        public int TotalQualityScore { get; set; }


        public int TotalMentionsCount { get; set; }
        public int TotalWordCount { get; set; }



        public int TotalScore { get; set; }
        public int Rank { get; set; }
    }


    public class ConversationTimeLineDto
    {
        public string ConversationId { get; set; }
        public List<Tweet> ConversationTweets { get; set; }
    }

    public class Tweet
    {
        public string TweetId { get; set; }
        public string TweetType { get; set; }
        public string TweetUserName { get; set; }
        public string TweetContent { get; set; }
        public DateTimeOffset? TweetDate { get; set; }
        public bool IsTweetByCharacter { get; set; }
    }
}