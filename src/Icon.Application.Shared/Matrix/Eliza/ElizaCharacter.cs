using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Icon.Matrix.Eliza
{
    /// <summary>
    /// Matches TypeScript: export type UUID = `${string}-${string}-${string}-${string}-${string}`;
    /// In C#, we’ll use a Guid (or Guid? if optional).
    /// </summary>
    public struct ElizaUUID
    {
        [JsonIgnore]
        public Guid Value;

        public ElizaUUID(Guid guid)
        {
            Value = guid;
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public static implicit operator Guid(ElizaUUID e) => e.Value;
        public static implicit operator ElizaUUID(Guid g) => new ElizaUUID(g);
    }

    /// <summary>
    /// Represents your main Character model (TypeScript: Character).
    /// </summary>
    public class ElizaCharacter
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("system")]
        public string System { get; set; }

        [JsonProperty("modelProvider")]
        //[JsonConverter(typeof(StringEnumConverter))]
        // public ElizaModelProviderName ModelProvider { get; set; }
        public string ModelProvider { get; set; }

        [JsonProperty("imageModelProvider")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ElizaModelProviderName? ImageModelProvider { get; set; }

        [JsonProperty("imageVisionModelProvider")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ElizaModelProviderName? ImageVisionModelProvider { get; set; }

        [JsonProperty("modelEndpointOverride")]
        public string ModelEndpointOverride { get; set; }

        [JsonProperty("templates")]
        public ElizaTemplates Templates { get; set; }

        /// <summary>
        /// TypeScript: bio: string | string[];
        /// C#: we store as object to handle single string or array of strings
        /// </summary>
        [JsonProperty("bio")]
        public object Bio { get; set; }

        [JsonProperty("lore")]
        public List<string> Lore { get; set; }

        /// <summary>
        /// TypeScript: messageExamples: MessageExample[][];
        /// C#: list of lists
        /// </summary>
        [JsonProperty("messageExamples")]
        public List<List<ElizaMessageExample>> MessageExamples { get; set; }

        [JsonProperty("postExamples")]
        public List<string> PostExamples { get; set; }

        [JsonProperty("topics")]
        public List<string> Topics { get; set; }

        [JsonProperty("adjectives")]
        public List<string> Adjectives { get; set; }

        /// <summary>
        /// TypeScript: knowledge?: (string | { path: string; shared?: boolean })[];
        /// We store as a list of objects to allow either string or object.
        /// </summary>
        [JsonProperty("knowledge")]
        public List<object> Knowledge { get; set; }

        [JsonProperty("clients")]
        [JsonConverter(typeof(StringEnumConverter))]
        public List<ElizaClients> Clients { get; set; }

        [JsonProperty("plugins")]
        public List<ElizaPlugin> Plugins { get; set; }

        [JsonProperty("settings")]
        public ElizaSettings Settings { get; set; }

        [JsonProperty("clientConfig")]
        public ElizaClientConfig ClientConfig { get; set; }

        [JsonProperty("style")]
        public ElizaStyle Style { get; set; }

        [JsonProperty("twitterProfile")]
        public ElizaTwitterProfile TwitterProfile { get; set; }

        [JsonProperty("nft")]
        public ElizaNft Nft { get; set; }

        [JsonProperty("extends")]
        public List<string> Extends { get; set; }
    }

    #region Enums referenced by Character

    /// <summary>
    /// Matches: export enum ModelProviderName { ... }
    /// Uses string values for JSON.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ElizaModelProviderName
    {
        [EnumMember(Value = "openai")]
        OPENAI,
        [EnumMember(Value = "eternalai")]
        ETERNALAI,
        [EnumMember(Value = "anthropic")]
        ANTHROPIC,
        [EnumMember(Value = "grok")]
        GROK,
        [EnumMember(Value = "groq")]
        GROQ,
        [EnumMember(Value = "llama_cloud")]
        LLAMACLOUD,
        [EnumMember(Value = "together")]
        TOGETHER,
        [EnumMember(Value = "llama_local")]
        LLAMALOCAL,
        [EnumMember(Value = "google")]
        GOOGLE,
        [EnumMember(Value = "mistral")]
        MISTRAL,
        [EnumMember(Value = "claude_vertex")]
        CLAUDE_VERTEX,
        [EnumMember(Value = "redpill")]
        REDPILL,
        [EnumMember(Value = "openrouter")]
        OPENROUTER,
        [EnumMember(Value = "ollama")]
        OLLAMA,
        [EnumMember(Value = "heurist")]
        HEURIST,
        [EnumMember(Value = "galadriel")]
        GALADRIEL,
        [EnumMember(Value = "falai")]
        FAL,
        [EnumMember(Value = "gaianet")]
        GAIANET,
        [EnumMember(Value = "ali_bailian")]
        ALI_BAILIAN,
        [EnumMember(Value = "volengine")]
        VOLENGINE,
        [EnumMember(Value = "nanogpt")]
        NANOGPT,
        [EnumMember(Value = "hyperbolic")]
        HYPERBOLIC,
        [EnumMember(Value = "venice")]
        VENICE,
        [EnumMember(Value = "nineteen_ai")]
        NINETEEN_AI,
        [EnumMember(Value = "akash_chat_api")]
        AKASH_CHAT_API,
        [EnumMember(Value = "livepeer")]
        LIVEPEER,
        [EnumMember(Value = "deepseek")]
        DEEPSEEK,
        [EnumMember(Value = "infera")]
        INFERA,
        [EnumMember(Value = "letzai")]
        LETZAI
    }

    /// <summary>
    /// Matches: export enum Clients { ... }
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ElizaClients
    {
        [EnumMember(Value = "discord")]
        DISCORD,
        [EnumMember(Value = "direct")]
        DIRECT,
        [EnumMember(Value = "twitter")]
        TWITTER,
        [EnumMember(Value = "telegram")]
        TELEGRAM,
        [EnumMember(Value = "farcaster")]
        FARCASTER,
        [EnumMember(Value = "lens")]
        LENS,
        [EnumMember(Value = "auto")]
        AUTO,
        [EnumMember(Value = "slack")]
        SLACK,
        [EnumMember(Value = "github")]
        GITHUB
    }

    /// <summary>
    /// Matches: export enum TranscriptionProvider { ... }
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ElizaTranscriptionProvider
    {
        [EnumMember(Value = "openai")]
        OpenAI,
        [EnumMember(Value = "deepgram")]
        Deepgram,
        [EnumMember(Value = "local")]
        Local
    }

    #endregion

    #region Sub-objects used by Character

    /// <summary>
    /// Mirrors the optional template properties from TypeScript.
    /// (TypeScript: templates?: { ... })
    /// Each template is a "TemplateType" which can be string or function; here we store as string.
    /// </summary>
    public class ElizaTemplates
    {
        [JsonProperty("goalsTemplate")]
        public string GoalsTemplate { get; set; }

        [JsonProperty("factsTemplate")]
        public string FactsTemplate { get; set; }

        [JsonProperty("messageHandlerTemplate")]
        public string MessageHandlerTemplate { get; set; }

        [JsonProperty("shouldRespondTemplate")]
        public string ShouldRespondTemplate { get; set; }

        [JsonProperty("continueMessageHandlerTemplate")]
        public string ContinueMessageHandlerTemplate { get; set; }

        [JsonProperty("evaluationTemplate")]
        public string EvaluationTemplate { get; set; }

        [JsonProperty("twitterSearchTemplate")]
        public string TwitterSearchTemplate { get; set; }

        [JsonProperty("twitterActionTemplate")]
        public string TwitterActionTemplate { get; set; }

        [JsonProperty("twitterPostTemplate")]
        public string TwitterPostTemplate { get; set; }

        [JsonProperty("twitterMessageHandlerTemplate")]
        public string TwitterMessageHandlerTemplate { get; set; }

        [JsonProperty("twitterShouldRespondTemplate")]
        public string TwitterShouldRespondTemplate { get; set; }

        [JsonProperty("farcasterPostTemplate")]
        public string FarcasterPostTemplate { get; set; }

        [JsonProperty("lensPostTemplate")]
        public string LensPostTemplate { get; set; }

        [JsonProperty("farcasterMessageHandlerTemplate")]
        public string FarcasterMessageHandlerTemplate { get; set; }

        [JsonProperty("lensMessageHandlerTemplate")]
        public string LensMessageHandlerTemplate { get; set; }

        [JsonProperty("farcasterShouldRespondTemplate")]
        public string FarcasterShouldRespondTemplate { get; set; }

        [JsonProperty("lensShouldRespondTemplate")]
        public string LensShouldRespondTemplate { get; set; }

        [JsonProperty("telegramMessageHandlerTemplate")]
        public string TelegramMessageHandlerTemplate { get; set; }

        [JsonProperty("telegramShouldRespondTemplate")]
        public string TelegramShouldRespondTemplate { get; set; }

        [JsonProperty("discordVoiceHandlerTemplate")]
        public string DiscordVoiceHandlerTemplate { get; set; }

        [JsonProperty("discordShouldRespondTemplate")]
        public string DiscordShouldRespondTemplate { get; set; }

        [JsonProperty("discordMessageHandlerTemplate")]
        public string DiscordMessageHandlerTemplate { get; set; }

        [JsonProperty("slackMessageHandlerTemplate")]
        public string SlackMessageHandlerTemplate { get; set; }

        [JsonProperty("slackShouldRespondTemplate")]
        public string SlackShouldRespondTemplate { get; set; }
    }

    /// <summary>
    /// TypeScript: export interface MessageExample { user: string; content: Content }
    /// Used in Character.messageExamples
    /// </summary>
    public class ElizaMessageExample
    {
        [JsonProperty("user")]
        public string User { get; set; }

        [JsonProperty("content")]
        public ElizaContent Content { get; set; }
    }

    /// <summary>
    /// TypeScript: export interface Content { ... }
    /// </summary>
    public class ElizaContent
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("action")]
        public string Action { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("inReplyTo")]
        public ElizaUUID? InReplyTo { get; set; }

        [JsonProperty("attachments")]
        public List<ElizaMedia> Attachments { get; set; }

        /// <summary>
        /// Additional dynamic properties (TypeScript: [key: string]: unknown).
        /// We capture as a dictionary of string -> object.
        /// </summary>
        [JsonExtensionData]
        public Dictionary<string, object> AdditionalProperties { get; set; }
    }

    /// <summary>
    /// TypeScript: export type Media = { id, url, title, source, description, text, contentType? }
    /// </summary>
    public class ElizaMedia
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("contentType")]
        public string ContentType { get; set; }
    }

    /// <summary>
    /// TypeScript: export interface Plugin { ... }
    /// </summary>
    public class ElizaPlugin
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("actions")]
        public List<ElizaAction> Actions { get; set; }

        [JsonProperty("providers")]
        public List<ElizaProvider> Providers { get; set; }

        [JsonProperty("evaluators")]
        public List<ElizaEvaluator> Evaluators { get; set; }

        /// <summary>
        /// In TypeScript: services?: Service[];
        /// Because "Service" is abstract and can have many forms, we’ll store them as a simple placeholder list.
        /// </summary>
        [JsonProperty("services")]
        public List<ElizaService> Services { get; set; }

        [JsonProperty("clients")]
        public List<ElizaClient> Clients { get; set; }
    }

    /// <summary>
    /// TypeScript: export interface Action { ... }
    /// This includes references to function types (handler, validate), which are replaced with object? placeholders.
    /// </summary>
    public class ElizaAction
    {
        [JsonProperty("similes")]
        public List<string> Similes { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// TypeScript: examples: ActionExample[][]
        /// We’ll store as list of list of ElizaActionExample.
        /// </summary>
        [JsonProperty("examples")]
        public List<List<ElizaActionExample>> Examples { get; set; }

        /// <summary>
        /// TypeScript: handler: Handler => function
        /// Here: just store as object for JSON placeholder
        /// </summary>
        [JsonProperty("handler")]
        public object Handler { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// TypeScript: validate: Validator => function
        /// We store as object placeholder
        /// </summary>
        [JsonProperty("validate")]
        public object Validate { get; set; }

        [JsonProperty("suppressInitialMessage")]
        public bool? SuppressInitialMessage { get; set; }
    }

    /// <summary>
    /// TypeScript: export interface ActionExample { user: string; content: Content }
    /// Similar shape to MessageExample but with a different name.
    /// </summary>
    public class ElizaActionExample
    {
        [JsonProperty("user")]
        public string User { get; set; }

        [JsonProperty("content")]
        public ElizaContent Content { get; set; }
    }

    /// <summary>
    /// TypeScript: export interface Provider { get: (runtime, message, state?) => Promise<any> }
    /// We'll store the function as object.
    /// </summary>
    public class ElizaProvider
    {
        [JsonProperty("get")]
        public object Get { get; set; }
    }

    /// <summary>
    /// TypeScript: export interface Evaluator { ... }
    /// </summary>
    public class ElizaEvaluator
    {
        [JsonProperty("alwaysRun")]
        public bool? AlwaysRun { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("similes")]
        public List<string> Similes { get; set; }

        [JsonProperty("examples")]
        public List<ElizaEvaluationExample> Examples { get; set; }

        [JsonProperty("handler")]
        public object Handler { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("validate")]
        public object Validate { get; set; }
    }

    /// <summary>
    /// TypeScript: export interface EvaluationExample { context, messages, outcome }
    /// </summary>
    public class ElizaEvaluationExample
    {
        [JsonProperty("context")]
        public string Context { get; set; }

        [JsonProperty("messages")]
        public List<ElizaActionExample> Messages { get; set; }

        [JsonProperty("outcome")]
        public string Outcome { get; set; }
    }

    /// <summary>
    /// TypeScript: export type Client = { start, stop }
    /// We can't serialize function references, so placeholders.
    /// </summary>
    public class ElizaClient
    {
        [JsonProperty("start")]
        public object Start { get; set; }

        [JsonProperty("stop")]
        public object Stop { get; set; }
    }

    /// <summary>
    /// TypeScript: export abstract class Service { ... }
    /// For data transfer, we store minimal placeholders.
    /// </summary>
    public class ElizaService
    {
        // Possibly more fields if you truly need them. 
        // For now, this is a placeholder to avoid "skipping."
    }

    /// <summary>
    /// TypeScript: export interface ModelConfiguration { ... }
    /// </summary>
    public class ElizaModelConfiguration
    {
        [JsonProperty("temperature")]
        public double? Temperature { get; set; }

        [JsonProperty("max_response_length")]
        public int? MaxResponseLength { get; set; }

        [JsonProperty("frequency_penalty")]
        public double? FrequencyPenalty { get; set; }

        [JsonProperty("presence_penalty")]
        public double? PresencePenalty { get; set; }

        [JsonProperty("maxInputTokens")]
        public int? MaxInputTokens { get; set; }

        [JsonProperty("experimental_telemetry")]
        public ElizaTelemetrySettings ExperimentalTelemetry { get; set; }
    }

    /// <summary>
    /// Matches TypeScript: export type TelemetrySettings
    /// </summary>
    public class ElizaTelemetrySettings
    {
        [JsonProperty("isEnabled")]
        public bool? IsEnabled { get; set; }

        [JsonProperty("recordInputs")]
        public bool? RecordInputs { get; set; }

        [JsonProperty("recordOutputs")]
        public bool? RecordOutputs { get; set; }

        [JsonProperty("functionId")]
        public string FunctionId { get; set; }
    }

    /// <summary>
    /// TypeScript: export interface IAgentConfig { [key: string]: string; }
    /// If you end up needing it, it would be a dictionary.
    /// </summary>
    // public class ElizaAgentConfig : Dictionary<string, string> {}

    /// <summary>
    /// TypeScript: export interface IImageSettings portion
    /// Used in settings.imageSettings
    /// </summary>
    public class ElizaImageSettings
    {
        [JsonProperty("steps")]
        public int? Steps { get; set; }

        [JsonProperty("width")]
        public int? Width { get; set; }

        [JsonProperty("height")]
        public int? Height { get; set; }

        [JsonProperty("negativePrompt")]
        public string NegativePrompt { get; set; }

        [JsonProperty("numIterations")]
        public int? NumIterations { get; set; }

        [JsonProperty("guidanceScale")]
        public double? GuidanceScale { get; set; }

        [JsonProperty("seed")]
        public int? Seed { get; set; }

        [JsonProperty("modelId")]
        public string ModelId { get; set; }

        [JsonProperty("jobId")]
        public string JobId { get; set; }

        [JsonProperty("count")]
        public int? Count { get; set; }

        [JsonProperty("stylePreset")]
        public string StylePreset { get; set; }

        [JsonProperty("hideWatermark")]
        public bool? HideWatermark { get; set; }
    }

    /// <summary>
    /// TypeScript: voice?: { model?: string; url?: string; elevenlabs?: { ... } }
    /// </summary>
    public class ElizaVoiceSettings
    {
        [JsonProperty("model")]
        public string Model { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("elevenlabs")]
        public ElizaElevenLabsSettings ElevenLabs { get; set; }
    }

    public class ElizaElevenLabsSettings
    {
        [JsonProperty("voiceId")]
        public string VoiceId { get; set; }

        [JsonProperty("model")]
        public string Model { get; set; }

        [JsonProperty("stability")]
        public string Stability { get; set; }

        [JsonProperty("similarityBoost")]
        public string SimilarityBoost { get; set; }

        [JsonProperty("style")]
        public string Style { get; set; }

        [JsonProperty("useSpeakerBoost")]
        public string UseSpeakerBoost { get; set; }
    }

    /// <summary>
    /// TypeScript: export type Settings = { secrets?:..., intiface?:..., imageSettings?:..., voice?:..., model?:..., modelConfig?:..., embeddingModel?:..., chains?:..., transcription?:..., ragKnowledge?:... }
    /// </summary>
    public class ElizaSettings
    {
        [JsonProperty("secrets")]
        public Dictionary<string, string> Secrets { get; set; }

        [JsonProperty("intiface")]
        public bool? Intiface { get; set; }

        [JsonProperty("imageSettings")]
        public ElizaImageSettings ImageSettings { get; set; }

        [JsonProperty("voice")]
        public ElizaVoiceSettings Voice { get; set; }

        [JsonProperty("model")]
        public string Model { get; set; }

        [JsonProperty("modelConfig")]
        public ElizaModelConfiguration ModelConfig { get; set; }

        [JsonProperty("embeddingModel")]
        public string EmbeddingModel { get; set; }

        /// <summary>
        /// TypeScript: chains?: { evm?: any[]; solana?: any[]; [key: string]: any[]; }
        /// We store as Dictionary where each key is string, each value is a list of objects.
        /// </summary>
        [JsonProperty("chains")]
        public Dictionary<string, List<object>> Chains { get; set; }

        [JsonProperty("transcription")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ElizaTranscriptionProvider? Transcription { get; set; }

        [JsonProperty("ragKnowledge")]
        public bool? RagKnowledge { get; set; }
    }

    /// <summary>
    /// TypeScript: export interface ClientConfig { ... }
    /// </summary>
    public class ElizaClientConfig
    {
        [JsonProperty("discord")]
        public ElizaDiscordClientConfig Discord { get; set; }

        [JsonProperty("telegram")]
        public ElizaTelegramClientConfig Telegram { get; set; }

        [JsonProperty("slack")]
        public ElizaSlackClientConfig Slack { get; set; }

        [JsonProperty("gitbook")]
        public ElizaGitbookClientConfig Gitbook { get; set; }
    }

    public class ElizaDiscordClientConfig
    {
        [JsonProperty("shouldIgnoreBotMessages")]
        public bool? ShouldIgnoreBotMessages { get; set; }

        [JsonProperty("shouldIgnoreDirectMessages")]
        public bool? ShouldIgnoreDirectMessages { get; set; }

        [JsonProperty("shouldRespondOnlyToMentions")]
        public bool? ShouldRespondOnlyToMentions { get; set; }

        [JsonProperty("messageSimilarityThreshold")]
        public double? MessageSimilarityThreshold { get; set; }

        [JsonProperty("isPartOfTeam")]
        public bool? IsPartOfTeam { get; set; }

        [JsonProperty("teamAgentIds")]
        public List<string> TeamAgentIds { get; set; }

        [JsonProperty("teamLeaderId")]
        public string TeamLeaderId { get; set; }

        [JsonProperty("teamMemberInterestKeywords")]
        public List<string> TeamMemberInterestKeywords { get; set; }
    }

    public class ElizaTelegramClientConfig
    {
        [JsonProperty("shouldIgnoreBotMessages")]
        public bool? ShouldIgnoreBotMessages { get; set; }

        [JsonProperty("shouldIgnoreDirectMessages")]
        public bool? ShouldIgnoreDirectMessages { get; set; }

        [JsonProperty("shouldRespondOnlyToMentions")]
        public bool? ShouldRespondOnlyToMentions { get; set; }

        [JsonProperty("shouldOnlyJoinInAllowedGroups")]
        public bool? ShouldOnlyJoinInAllowedGroups { get; set; }

        [JsonProperty("allowedGroupIds")]
        public List<string> AllowedGroupIds { get; set; }

        [JsonProperty("messageSimilarityThreshold")]
        public double? MessageSimilarityThreshold { get; set; }

        [JsonProperty("isPartOfTeam")]
        public bool? IsPartOfTeam { get; set; }

        [JsonProperty("teamAgentIds")]
        public List<string> TeamAgentIds { get; set; }

        [JsonProperty("teamLeaderId")]
        public string TeamLeaderId { get; set; }

        [JsonProperty("teamMemberInterestKeywords")]
        public List<string> TeamMemberInterestKeywords { get; set; }
    }

    public class ElizaSlackClientConfig
    {
        [JsonProperty("shouldIgnoreBotMessages")]
        public bool? ShouldIgnoreBotMessages { get; set; }

        [JsonProperty("shouldIgnoreDirectMessages")]
        public bool? ShouldIgnoreDirectMessages { get; set; }
    }

    public class ElizaGitbookClientConfig
    {
        [JsonProperty("keywords")]
        public ElizaGitbookKeywords Keywords { get; set; }

        [JsonProperty("documentTriggers")]
        public List<string> DocumentTriggers { get; set; }
    }

    public class ElizaGitbookKeywords
    {
        [JsonProperty("projectTerms")]
        public List<string> ProjectTerms { get; set; }

        [JsonProperty("generalQueries")]
        public List<string> GeneralQueries { get; set; }
    }

    /// <summary>
    /// TypeScript: style: { all: string[]; chat: string[]; post: string[]; }
    /// </summary>
    public class ElizaStyle
    {
        [JsonProperty("all")]
        public List<string> All { get; set; }

        [JsonProperty("chat")]
        public List<string> Chat { get; set; }

        [JsonProperty("post")]
        public List<string> Post { get; set; }
    }

    /// <summary>
    /// TypeScript: twitterProfile?: { id, username, screenName, bio, nicknames? }
    /// </summary>
    public class ElizaTwitterProfile
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("screenName")]
        public string ScreenName { get; set; }

        [JsonProperty("bio")]
        public string Bio { get; set; }

        [JsonProperty("nicknames")]
        public List<string> Nicknames { get; set; }
    }

    /// <summary>
    /// TypeScript: nft?: { prompt: string }
    /// </summary>
    public class ElizaNft
    {
        [JsonProperty("prompt")]
        public string Prompt { get; set; }
    }

    #endregion
}

