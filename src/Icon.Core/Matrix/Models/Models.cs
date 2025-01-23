using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Abp.Domain.Entities;
using Icon.Matrix.Enums;

namespace Icon.Matrix.Models
{
    public class Agent : Entity<Guid>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public string AgentType { get; set; }
        public string AzureAgentTableId { get; set; }
        public string AzureAgentId { get; set; }
        public string AgentDescription { get; set; }
        public Guid? CharacterId { get; set; }
        public string CharacterName { get; set; }
        public bool IsActive { get; set; }
    }

    public class Character : Entity<Guid>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public string Name { get; set; }
        public IList<CharacterBio> Bios { get; set; }
        public IList<CharacterTopic> Topics { get; set; }
        // public IList<CharacterPlatform> Platforms { get; set; }
        //public IList<CharacterPersona> Personas { get; set; }

        public string TwitterPostAgentId { get; set; }
        public string TwitterScrapeAgentId { get; set; }
        public TwitterCommType TwitterCommType { get; set; }
        public bool IsTwitterScrapingEnabled { get; set; }
        public bool IsTwitterPostingEnabled { get; set; }
        public bool IsPromptingEnabled { get; set; }
        public string TwitterUserName { get; set; }

        public string TwitterAutoPostInstruction { get; set; }
        public string TwitterAutoPostExamples { get; set; }
        public int TwitterAutoPostDelayMinutes { get; set; }

        public string TwitterMentionReplyInstruction { get; set; }
        public string TwitterMentionReplyExamples { get; set; }

    }

    public class CharacterBio : Entity<Guid>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public Guid CharacterId { get; set; }
        public Character Character { get; set; }
        public string Bio { get; set; }
        public string Personality { get; set; }
        public string Appearance { get; set; }
        public string Occupation { get; set; }
        public DateTimeOffset BirthDate { get; set; }
        public DateTimeOffset ActiveFrom { get; set; }
        public DateTimeOffset? ActiveTo { get; set; }
        public bool IsActive { get; set; }
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


    public class Topic : Entity<Guid>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public string Name { get; set; }
    }

    public class CharacterTopic : Entity<Guid>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public Guid CharacterId { get; set; }
        public Character Character { get; set; }
        public Guid TopicId { get; set; }
        public Topic Topic { get; set; }
        public string Attitudes { get; set; }
        public string Repsonses { get; set; }
    }

    public class CharacterPlatform : Entity<Guid>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public Guid CharacterId { get; set; }
        public Character Character { get; set; }
        public Guid PlatformId { get; set; }
        public Platform Platform { get; set; }
        public string PlatformCharacterId { get; set; }
        public string PlatformCharacterName { get; set; }
        public string PlatformLogin { get; set; }
        public string PlatformPassword { get; set; }
    }

    public class Platform : Entity<Guid>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public string Name { get; set; }
    }

    public class Persona : Entity<Guid>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public string Name { get; set; }
        public IList<PersonaPlatform> Platforms { get; set; }
    }

    public class CharacterPersona : Entity<Guid>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public Guid CharacterId { get; set; }
        public Character Character { get; set; }
        public Guid PersonaId { get; set; }
        public Persona Persona { get; set; }
        public string Attitude { get; set; }
        public string Repsonses { get; set; }
        public bool ShouldRespondNewPosts { get; set; }
        public bool ShouldRespondMentions { get; set; }
        public bool ShouldImportNewPosts { get; set; }
        public bool TwitterBlockInRanking { get; set; }
        public bool PersonaIsAi { get; set; }
        public bool WelcomeMessageSent { get; set; }
        public DateTime? WelcomeMessageSentAt { get; set; }
        public string SolanaWallet { get; set; }
        public CharacterPersonaTwitterRank TwitterRank { get; set; }
        public CharacterPersonaTwitterProfile TwitterProfile { get; set; }
    }

    public class PersonaPlatform : Entity<Guid>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public Guid PersonaId { get; set; }
        public Guid PlatformId { get; set; }
        public Platform Platform { get; set; }
        public string PlatformPersonaId { get; set; }
    }


    public class MemoryParent : Entity<Guid>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public string PlatformInteractionParentId { get; set; }
        public int MemoryCount { get; set; }
        public int CharacterReplyCount { get; set; }
        public int UniquePersonasCount { get; set; }
        public DateTimeOffset? LastReplyAt { get; set; }
        public IList<Memory> Memories { get; set; } = new List<Memory>();
    }


    public class Memory : Entity<Guid>, IMustHaveTenant
    {
        public int TenantId { get; set; }

        public Guid? MemoryParentId { get; set; }
        public MemoryParent MemoryParent { get; set; }

        public Guid CharacterId { get; set; }
        public Character Character { get; set; }

        public Guid CharacterBioId { get; set; }
        public CharacterBio CharacterBio { get; set; }

        public Guid? PlatformId { get; set; }
        public Platform Platform { get; set; }

        public string PlatformInteractionId { get; set; }
        public string PlatformInteractionParentId { get; set; }
        public DateTimeOffset? PlatformInteractionDate { get; set; }

        public Guid? CharacterPersonaId { get; set; }
        public CharacterPersona CharacterPersona { get; set; }

        public Guid MemoryTypeId { get; set; }
        public MemoryType MemoryType { get; set; }

        public MemoryStatsTwitter MemoryStatsTwitter { get; set; }

        public string MemoryTitle { get; set; }
        public string MemoryContent { get; set; }
        public string MemoryUrl { get; set; }

        public int RememberDays { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
        public string Tags { get; set; }
        public IList<MemoryTopic> Topics { get; set; } = new List<MemoryTopic>();
        public IList<MemoryPrompt> Prompts { get; set; } = new List<MemoryPrompt>();

        public MemoryProcess MemoryProcess { get; set; }

        public bool IsPromptGenerated { get; set; }
        public bool IsActionTaken { get; set; }
        public bool ShouldVectorize { get; set; }
        public string VectorHash { get; set; }

    }

    public class MemoryPrompt : Entity<Guid>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public AIPromptType PromptType { get; set; }
        public Guid MemoryId { get; set; }
        public string InputContextModel { get; set; }
        public string InputContextJson { get; set; }
        public string InputFullText { get; set; }
        public string ResponseModel { get; set; }
        public string ResponseJson { get; set; }
        public bool IsSuccess { get; set; }
        public string Exception { get; set; }
        public string ExceptionMessage { get; set; }
        public DateTimeOffset GeneratedAt { get; set; }
    }

    // public class MemoryAction : Entity<Guid>, IMustHaveTenant
    // {
    //     public int TenantId { get; set; }

    //     public ActionType ActionType { get; set; }

    //     public Guid MemoryId { get; set; }

    //     public Guid? MemoryPromptId { get; set; }
    //     public string ActionDescription { get; set; }

    //     public DateTimeOffset? ExecutedAt { get; set; }
    //     public bool IsSuccess { get; set; }
    //     public string Exception { get; set; }
    //     public string ExceptionMessage { get; set; }
    // }



    public class MemoryStatsTwitter : Entity<Guid>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public Guid MemoryId { get; set; }

        public bool IsPin { get; set; }
        public bool IsQuoted { get; set; }
        public bool IsReply { get; set; }
        public bool IsRetweet { get; set; }
        public bool SensitiveContent { get; set; }

        public int BookmarkCount { get; set; }
        public int Likes { get; set; }
        public int Replies { get; set; }
        public int Retweets { get; set; }
        public int Views { get; set; }

        public int RelevanceScore { get; set; }
        public int DepthScore { get; set; }
        public int NoveltyScore { get; set; }
        public int SentimentScore { get; set; }

        public int TweetWordCount { get; set; }
        public int MentionsCount { get; set; }
    }

    public class CharacterPersonaTwitterRank : Entity<Guid>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public Guid CharacterPersonaId { get; set; }

        // MENTIONS
        public int TotalMentions { get; set; }
        public int TotalMentionsScore { get; set; }

        // ENGAGEMENT
        public int TotalLikes { get; set; } // 0.02
        public int TotalReplies { get; set; } // 1
        public int TotalRetweets { get; set; } //  0.5
        public int TotalViews { get; set; } // 0.003        
        public int TotalEngagementScore { get; set; } // sum of all engagement scores

        // QUALITY
        public int TotalRelevanceScore { get; set; }
        public int TotalDepthScore { get; set; }
        public int TotalNoveltyScore { get; set; }
        public int TotalSentimentScore { get; set; }
        public int TotalQualityScore { get; set; }

        // TOTAL SCORE
        public int TotalScore { get; set; }
        public int TotalScoreTimeDecayed { get; set; }
        public int Rank { get; set; }


        // OTHERS 
        public int TotalMentionsCount { get; set; }
        public int TotalWordCount { get; set; }

    }

    public class MemoryTopic
    {
        public Guid Id { get; set; }
        public Guid MemoryId { get; set; }
        public Memory Memory { get; set; }
        public Guid TopicId { get; set; }
        public Topic Topic { get; set; }
    }

    public class MemoryType : Entity<Guid>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public string Name { get; set; }
    }

    // public class ActionType : Entity<Guid>, IMustHaveTenant
    // {
    //     public int TenantId { get; set; }
    //     public string Name { get; set; }
    // }

    public class Feedback : Entity<Guid>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public Guid MemoryId { get; set; }
        public Memory Memory { get; set; }
        public string FeedbackText { get; set; }
        public int Score { get; set; } // Multi-point scores as JSON or separate fields
        public DateTimeOffset CreatedAt { get; set; }
        public Guid UserId { get; set; } // To track who gave the feedback
    }

    public class MemoryProcess : Entity<Guid>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public Guid MemoryId { get; set; }
        [JsonIgnore]
        public Memory Memory { get; set; }

        public MemoryProcessState State { get; set; } = MemoryProcessState.NotStarted;
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? StartedAt { get; set; }
        public DateTimeOffset? CompletedAt { get; set; }

        public ICollection<MemoryProcessStep> Steps { get; set; } = new List<MemoryProcessStep>();
        public ICollection<MemoryProcessLog> Logs { get; set; } = new List<MemoryProcessLog>();
    }


    public class MemoryProcessStep : Entity<Guid>, IMustHaveTenant
    {
        public int TenantId { get; set; }

        public Guid MemoryProcessId { get; set; }
        [JsonIgnore]
        public MemoryProcess MemoryProcess { get; set; }

        public int OrderIndex { get; set; }
        public string StepName { get; set; }
        public string MethodName { get; set; }
        public string ParametersJson { get; set; }

        public MemoryProcessStepState State { get; set; } = MemoryProcessStepState.Pending;
        public string DependenciesJson { get; set; }
        public int RetryCount { get; set; }
        public int MaxRetries { get; set; } = 3;

        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? StartedAt { get; set; }
        public DateTimeOffset? CompletedAt { get; set; }
    }

    public class MemoryProcessLog : Entity<Guid>, IMustHaveTenant
    {
        public int TenantId { get; set; }

        public Guid MemoryProcessId { get; set; }
        [JsonIgnore]
        public MemoryProcess MemoryProcess { get; set; }

        public Guid? MemoryProcessStepId { get; set; }
        [JsonIgnore]
        public MemoryProcessStep MemoryProcessStep { get; set; }

        public string Message { get; set; }
        public DateTimeOffset LoggedAt { get; set; } = DateTimeOffset.UtcNow;

        // Optionally add fields for severity, exception, etc.
        public string LogLevel { get; set; } // "Info", "Warn", "Error"
        public string Exception { get; set; }
        public string ExceptionMessage { get; set; }
    }

    public class TwitterImportTask : Entity<Guid>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public string TaskName { get; set; }
        public Guid CharacterId { get; set; }
        //public string TwitterAgentId { get; set; }
        public int ImportLimitTotal { get; set; }
        public DateTime? LastRunCompletionTime { get; set; }
        public int LastRunDurationSeconds { get; set; }
        public DateTime? LastRunStartTime { get; set; }
        public DateTime? NextRunTime { get; set; }
        public int RunEveryXMinutes { get; set; }
        public bool IsEnabled { get; set; }
        public string LastTweetImportId { get; set; }
    }

    public class TwitterImportLog : Entity<Guid>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public Guid CharacterId { get; set; }
        public string TwitterAgentId { get; set; }
        public string TaskName { get; set; }
        public string Message { get; set; }
        public string LogLevel { get; set; }
        public string Exception { get; set; }
        public string ExceptionMessage { get; set; }
        public DateTime LoggedAt { get; set; }
    }

    public class TwitterAPIUsage : Entity<Guid>
    {
        public DateTimeOffset RequestTime { get; set; }
        public string Endpoint { get; set; }
        public string Query { get; set; }
        public int? StatusCode { get; set; }
        public string RateLimitType { get; set; }
        public int? RateLimitRemaining { get; set; }
        public int? RateLimitLimit { get; set; }
        public DateTimeOffset? RateLimitResetTime { get; set; }

        public string ResponseBody { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class TwitterImportTweet : Entity<Guid>, IMustHaveTenant
    {
        public virtual int TenantId { get; set; }
        public virtual string TweetType { get; set; }
        public virtual Guid CharacterId { get; set; }
        public virtual string CharacterName { get; set; }
        public virtual int BookmarkCount { get; set; }
        public virtual string ConversationId { get; set; }
        public virtual string Hashtags { get; set; }
        public virtual string Html { get; set; }
        public virtual string InReplyToStatusId { get; set; }
        public virtual bool IsQuoted { get; set; }
        public virtual bool IsPin { get; set; }
        public virtual bool IsReply { get; set; }
        public virtual bool IsRetweet { get; set; }
        public virtual bool IsSelfThread { get; set; }
        public virtual int Likes { get; set; }
        public virtual string Name { get; set; }
        public virtual string PermanentUrl { get; set; }
        public virtual string QuotedStatusId { get; set; }
        public virtual int Replies { get; set; }
        public virtual int Retweets { get; set; }
        public virtual string RetweetedStatusId { get; set; }
        public virtual string Text { get; set; }
        public virtual DateTime TimeParsed { get; set; }
        public virtual long Timestamp { get; set; }
        public virtual string Urls { get; set; }
        public virtual string UserId { get; set; }
        public virtual string Username { get; set; }
        public virtual int Views { get; set; }
        public virtual bool SensitiveContent { get; set; }
        public virtual string MentionsJson { get; set; }
        public virtual string PhotosJson { get; set; }
        public virtual string VideosJson { get; set; }
        public virtual string PlaceJson { get; set; }
        public virtual string PollJson { get; set; }
        public virtual string InReplyToStatusJson { get; set; }
        public virtual string QuotedStatusJson { get; set; }
        public virtual string RetweetedStatusJson { get; set; }
        public virtual string ThreadJson { get; set; }

        // Additional fields that were in the Table Entity:
        public virtual bool Exported { get; set; }
        public virtual DateTime? ExportDate { get; set; }
        public virtual DateTime LastTwitterImportDate { get; set; }
        public virtual bool LastTwitterImportExported { get; set; }

        // If you want to store the original TweetId as well.
        public virtual string TweetId { get; set; }
    }


    public class CharacterPersonaTwitterProfile : Entity<Guid>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public Guid? CharacterPersonaId { get; set; }
        public string Avatar { get; set; }
        public string Biography { get; set; }
        public int? FollowersCount { get; set; }
        public int? FollowingCount { get; set; }
        public int? FriendsCount { get; set; }
        public int? MediaCount { get; set; }
        public bool? IsPrivate { get; set; }
        public bool? IsVerified { get; set; }
        public int? LikesCount { get; set; }
        public int? ListedCount { get; set; }
        public string Location { get; set; }
        public string Name { get; set; }
        public List<string> PinnedTweetIds { get; set; }
        public int? TweetsCount { get; set; }
        public string Url { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }
        public bool? IsBlueVerified { get; set; }
        public bool? CanDm { get; set; }
        public DateTime? Joined { get; set; }

        public DateTime? LastImportDate { get; set; }
    }


    public class TwitterImportTweetCount : Entity<Guid>
    {
        public Guid? RaydiumPairId { get; set; }
        public string SearchQuery { get; set; }
        public DateTimeOffset StartTime { get; set; }
        public DateTimeOffset EndTime { get; set; }
        public int TweetCount { get; set; }
        public DateTimeOffset CreationTime { get; set; }
    }


    public class TwitterImportTweetEngagement : Entity<Guid>
    {
        public Guid RaydiumPairId { get; set; }
        public string TweetId { get; set; }
        public string AuthorId { get; set; }
        public string Text { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public int LikeCount { get; set; }
        public int ReplyCount { get; set; }
        public int RetweetCount { get; set; }
        public int QuoteCount { get; set; }
        public int ImpressionCount { get; set; }
        public DateTimeOffset LastUpdatedAt { get; set; }
    }





    public class CoingeckoPoolUpdate : Entity<Guid>
    {
        public Guid? RaydiumPairId { get; set; }
        public Guid? CoingeckoAggregatedUpdateId { get; set; }

        public DateTimeOffset CreationTime { get; set; }

        // Coingecko pool fields:
        public string PoolId { get; set; }
        public string PoolType { get; set; }

        public string BaseTokenPriceUsd { get; set; }
        public string BaseTokenPriceNativeCurrency { get; set; }
        public string QuoteTokenPriceUsd { get; set; }
        public string QuoteTokenPriceNativeCurrency { get; set; }
        public string BaseTokenPriceQuoteToken { get; set; }
        public string QuoteTokenPriceBaseToken { get; set; }

        public string Address { get; set; }
        public string Name { get; set; }
        public DateTimeOffset? PoolCreatedAt { get; set; }
        public float? TokenPriceUsd { get; set; }
        public float? FdvUsd { get; set; }
        public float? MarketCapUsd { get; set; }

        // Price changes:
        public float? PriceChangeM5 { get; set; }
        public float? PriceChangeH1 { get; set; }
        public float? PriceChangeH6 { get; set; }
        public float? PriceChangeH24 { get; set; }

        // Transactions:
        public int? M5Buys { get; set; }
        public int? M5Sells { get; set; }
        public int? M5Buyers { get; set; }
        public int? M5Sellers { get; set; }

        public int? M15Buys { get; set; }
        public int? M15Sells { get; set; }
        public int? M15Buyers { get; set; }
        public int? M15Sellers { get; set; }

        public int? M30Buys { get; set; }
        public int? M30Sells { get; set; }
        public int? M30Buyers { get; set; }
        public int? M30Sellers { get; set; }

        public int? H1Buys { get; set; }
        public int? H1Sells { get; set; }
        public int? H1Buyers { get; set; }
        public int? H1Sellers { get; set; }

        public int? H24Buys { get; set; }
        public int? H24Sells { get; set; }
        public int? H24Buyers { get; set; }
        public int? H24Sellers { get; set; }

        // Volume (USD)
        public float? VolumeM5 { get; set; }
        public float? VolumeH1 { get; set; }
        public float? VolumeH6 { get; set; }
        public float? VolumeH24 { get; set; }

        public float? ReserveInUsd { get; set; }

        // Relationship “IDs”
        public string BaseTokenId { get; set; }
        public string QuoteTokenId { get; set; }
        public string DexId { get; set; }

    }


    public class CoingeckoAggregatedUpdate : Entity<Guid>
    {
        public DateTimeOffset CreationTime { get; set; }

        public int Pools { get; set; }

        public float TotalLiquidityUsd { get; set; }
        public float WeightedAvgPriceUsd { get; set; }

        public float FdvUsd { get; set; }
        public float MarketCapUsd { get; set; }

        public float PriceChangeM5 { get; set; }
        public float PriceChangeH1 { get; set; }
        public float PriceChangeH6 { get; set; }
        public float PriceChangeH24 { get; set; }

        public float VolumeM5 { get; set; }
        public float VolumeH1 { get; set; }
        public float VolumeH6 { get; set; }
        public float VolumeH24 { get; set; }

        public int M5Buys { get; set; }
        public int M5Sells { get; set; }
        public int M15Buys { get; set; }
        public int M15Sells { get; set; }
        public int M30Buys { get; set; }
        public int M30Sells { get; set; }
        public int H1Buys { get; set; }
        public int H1Sells { get; set; }
        public int H24Buys { get; set; }
        public int H24Sells { get; set; }
    }






}