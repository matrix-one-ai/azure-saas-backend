using System;
using System.Collections.Generic;

namespace Icon.Matrix.AIManager.CharacterPostTweet
{
    public class AICharacterPostTweetContext
    {
        public CharacterToActAsDto CharacterToActAs { get; set; }
        public List<Tweet> PreviousCharacterTweets { get; set; }
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

    public class Tweet
    {
        public string TweetContent { get; set; }
        public DateTimeOffset? TweetDate { get; set; }
    }
}