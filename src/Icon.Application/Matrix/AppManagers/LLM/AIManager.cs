using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Runtime.Session;
using Icon.Matrix.AIManager.CharacterMentioned;
using Icon.Matrix.AIManager.CharacterPostTokenTweet;
using Icon.Matrix.AIManager.CharacterPostTweet;
using Icon.Matrix.Enums;
using Icon.Matrix.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OpenAI.Chat;


namespace Icon.Matrix.AIManager
{
    public interface IAIManager
    {
        Task<string> GenerateMentionedPromptAsync(AICharacterMentionedContext context);
        Task<string> GeneratePostTweetPromptAsync(AICharacterPostTweetContext context);
        Task<string> GeneratePostTokenTweetPromptAsync(AICharacterPostTokenTweetContext context);

        Task<AICharacterMentionedResponse> GenerateMentionedResponseAsync(AICharacterMentionedContext context, AIModelType modelType);
        Task<string> GeneratePostTweetResponseAsync(AICharacterPostTweetContext context, AIModelType modelType);
        Task<string> GeneratePostTokenTweetResponseAsync(AICharacterPostTokenTweetContext context, AIModelType modelType, bool shouldUseSmartModel = false);
    }

    public class AIManager : IAIManager, ITransientDependency
    {
        private readonly IAzureOpenAIService _azureOpenAIService;
        private readonly IDirectOpenAIService _directOpenAIService;
        private readonly ILlamaAIService _llamaAIService;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public AIManager(
            IAzureOpenAIService azureOpenAIService,
            IDirectOpenAIService directOpenAIService,
            ILlamaAIService llamaAIService,
            IUnitOfWorkManager unitOfWorkManager,
            IAbpSession abpSession
        )
        {
            _azureOpenAIService = azureOpenAIService;
            _directOpenAIService = directOpenAIService;
            _llamaAIService = llamaAIService;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public async Task<string> GenerateMentionedPromptAsync(AICharacterMentionedContext context)
        {
            var prompt = BuildCharacterMentionedPrompt(context);
            await Task.CompletedTask;
            return prompt;
        }

        public async Task<string> GeneratePostTweetPromptAsync(AICharacterPostTweetContext context)
        {
            var prompt = BuildCharacterPostTweetPrompt(context);
            await Task.CompletedTask;
            return prompt;
        }

        public async Task<string> GeneratePostTokenTweetPromptAsync(AICharacterPostTokenTweetContext context)
        {
            var prompt = BuildCharacterPostTokenTweetPrompt(context);
            await Task.CompletedTask;
            return prompt;
        }

        public async Task<string> GeneratePostTweetResponseAsync(AICharacterPostTweetContext context, AIModelType modelType)
        {
            if (modelType != AIModelType.DirectOpenAI && modelType != AIModelType.AzureOpenAI && modelType != AIModelType.Llama)
            {
                throw new NotSupportedException("Currently only OpenAI && Llama model is supported");
            }

            var prompt = BuildCharacterPostTweetPrompt(context);
            float temperature = 1.2f;
            int maxTokens = 1500;

            string rawResponse = "";

            try
            {
                if (modelType == AIModelType.DirectOpenAI)
                {
                    var openAIMessages = new List<ChatMessage>
                    {
                        new SystemChatMessage(prompt),
                        new UserChatMessage("Provide the next tweet to post")
                    };
                    var chatOptions = new ChatCompletionOptions
                    {
                        MaxOutputTokenCount = maxTokens,
                        Temperature = temperature
                    };

                    rawResponse = await _directOpenAIService.GetCompletionAsync(openAIMessages, chatOptions);
                }
                else if (modelType == AIModelType.Llama)
                {
                    var llamaMessages = new List<LlamaMessage>
                    {
                        new LlamaMessage { Role = "system", Content = prompt },
                        new LlamaMessage { Role = "user", Content = "Provide the next tweet to post" }
                    };

                    rawResponse = await _llamaAIService.GetCompletionAsync(llamaMessages, temperature, maxTokens);
                }
            }
            catch (Exception ex)
            {
                return $"Error generating response from AI Service: {ex.Message}";
            }

            return rawResponse;
        }

        public async Task<AICharacterMentionedResponse> GenerateMentionedResponseAsync(AICharacterMentionedContext context, AIModelType modelType)
        {
            if (modelType != AIModelType.DirectOpenAI && modelType != AIModelType.AzureOpenAI && modelType != AIModelType.Llama)
            {
                throw new NotSupportedException("Currently only OpenAI && Llama model is supported");
            }

            var prompt = BuildCharacterMentionedPrompt(context);
            float temperature = 1.2f;
            int maxTokens = 1500;

            string rawResponse = "";
            var aiResponse = new AICharacterMentionedResponse();

            try
            {
                if (modelType == AIModelType.DirectOpenAI)
                {
                    var openAIMessages = new List<ChatMessage>
                    {
                        new SystemChatMessage(prompt),
                        new UserChatMessage("Respond to mentioned tweet")
                    };
                    var chatOptions = new ChatCompletionOptions
                    {
                        MaxOutputTokenCount = maxTokens,
                        Temperature = temperature
                    };

                    rawResponse = await _directOpenAIService.GetCompletionAsync(openAIMessages, chatOptions);
                }
                else if (modelType == AIModelType.Llama)
                {
                    var llamaMessages = new List<LlamaMessage>
                    {
                        new LlamaMessage { Role = "system", Content = prompt },
                        new LlamaMessage { Role = "user", Content = "Respond to mentioned tweet" }
                    };

                    rawResponse = await _llamaAIService.GetCompletionAsync(llamaMessages, temperature, maxTokens);
                }
            }
            catch (Exception ex)
            {
                aiResponse.IsSuccess = false;
                aiResponse.Exception = "Error generating response from AI Service";
                aiResponse.ExceptionMessage = ex.Message;
                return aiResponse;
            }

            // Attempt to deserialize the rawResponse
            try
            {
                aiResponse = JsonConvert.DeserializeObject<AICharacterMentionedResponse>(rawResponse);
                aiResponse.IsSuccess = true;
            }
            catch (Newtonsoft.Json.JsonException ex)
            {
                aiResponse.IsSuccess = false;
                aiResponse.Exception = "Error deserializing response to JSON Model - try again";
                aiResponse.ExceptionMessage = ex.Message;
                aiResponse.RawResponse = rawResponse;
            }
            return aiResponse;
        }

        public async Task<string> GeneratePostTokenTweetResponseAsync(AICharacterPostTokenTweetContext context, AIModelType modelType, bool shouldUseSmartModel = false)
        {
            if (modelType != AIModelType.DirectOpenAI && modelType != AIModelType.AzureOpenAI && modelType != AIModelType.Llama)
            {
                throw new NotSupportedException("Currently only OpenAI && Llama model is supported");
            }

            var prompt = BuildCharacterPostTokenTweetPrompt(context);
            float temperature = 0.7f;
            int maxTokens = 500;

            string rawResponse = "";

            try
            {
                if (modelType == AIModelType.DirectOpenAI)
                {
                    var openAIMessages = new List<ChatMessage>
                    {
                        new SystemChatMessage(prompt),
                        new UserChatMessage("Provide the next tweet to post")
                    };
                    var chatOptions = new ChatCompletionOptions
                    {
                        MaxOutputTokenCount = maxTokens,
                        Temperature = temperature
                    };

                    rawResponse = await _directOpenAIService.GetCompletionAsync(openAIMessages, chatOptions, shouldUseSmartModel);
                }
                else if (modelType == AIModelType.Llama)
                {
                    var llamaMessages = new List<LlamaMessage>
                    {
                        new LlamaMessage { Role = "system", Content = prompt },
                        new LlamaMessage { Role = "user", Content = "Provide the next tweet to post" }
                    };

                    rawResponse = await _llamaAIService.GetCompletionAsync(llamaMessages, temperature, maxTokens);
                }
            }
            catch (Exception ex)
            {
                return $"Error generating response from AI Service: {ex.Message}";
            }

            return rawResponse;
        }

        private string BuildCharacterMentionedPrompt(AICharacterMentionedContext context)
        {
            var prompt = $@"
You are an AI assistant that reads the given context JSON and must produce a strictly formatted JSON response. 
The JSON response must have the following fields:
{{
  ""tagsFound"": [""...""],
  ""cryptocoinsFound"": [""...""],
  ""actionsToTake"": [
    {{ ""action"": ""getWebSearch"", ""params"": {{ ""query"": ""..."" }} }},
    {{ ""action"": ""getCoinInfo"", ""params"": {{ ""coin"": ""..."" }} }},
  ],
  ""scores"": {{
        ""Relevance"": 0-4,
        ""Depth"": 1-4,
        ""Sentiment"": 0-1
    }},  
  ""resultToPost"": ""...""
}}

Analyze the 'tweetToRespondTo' in relation to the 'characterToActAs' and other context. Respond to that post, only that post, the rest is context or parts of an earlier conversation. 
Tweets in the converstation marked as CharacterTweet are your own replies, make sure to be original in your reponses. 
Make the person you are interacting with 'heard', name them by name or use an analogy to respond more personal. 

Identify if there are tags to be distilled from the last user post, cryptocoins (like $FTM or any other), etc.
If no tags or coins are found, leave the arrays empty. If no actions are needed, return an empty array for actionsToTake.

Include a scoring system to evaluate each topic (Relevance, Depth and Sentiment Value). 
Relevance score: it mentions a cryptocoin / coin$ / ticker or project. If yes then 4 points if no then 0 points.
Depth score: Count words, everything with a space in between is a word. 1 word = 1 point, 4 words = 2 points, 8 words = 3 points, 12+ words = 4 points.
Sentiment score: Either positive or negative. Positive = 1 point, negative = 0 points.

Current context:
{JsonConvert.SerializeObject(context, Formatting.Indented)}

RULE: if the context contains TwitterMentionReplyInstruction, abide by that rule, but always return the specified JSON format.
RULE: if the context contains TwitterMentionReplyExamples, use that as a guide for the type of responses to generate. But be original and relevant to the context if the examples allow it.

Remember:
- Identify tags or return none like crypto, stocks, tech, love, music, etc.
- Identify cryptocoins like $FTM if mentioned
- List additional actions if more context is needed to create a better response
- Provide the final resultToPost as a short direct response to the tweet
- Be sure to adhere to the rules of the scoring system and score correctly

IMPORTANT: Return only valid JSON. Do not include additional text, formatting, Markdown or commentary. Only the JSON object.

The result response can only have this format
{{
    ""tagsFound"": [""...""],
    ""cryptocoinsFound"": [""...""],
    ""actionsToTake"": [
        {{ ""action"": ""getWebSearch"", ""params"": {{ ""query"": ""..."" }} }},
        {{ ""action"": ""getCoinInfo"", ""params"": {{ ""coin"": ""..."" }} }},
    ],
    ""scores"": {{
        ""Relevance"": 0-4,
        ""Depth"": 1-4,
        ""Sentiment"": 0-1
    }},  
    ""resultToPost"": ""...""
}}
";

            return prompt;
        }

        private string BuildCharacterPostTweetPrompt(AICharacterPostTweetContext context)
        {
            // Convert your context into plain text
            var formattedContext = ConvertJsonToPlainText(context);

            // Add random seed/key or date/time
            var randomSeed = Guid.NewGuid().ToString("N");
            var currentDateTime = DateTime.UtcNow.ToString("O");

            var prompt = $@"
                You are an AI Character that is about to post a tweet.
                (Random Key: {randomSeed})
                Current UTC DateTime: {currentDateTime}

                You have the following context to work with:
                {formattedContext}

                Your task is to generate a tweet that is in line with the character's personality and the context provided.
                Be original and make sure the tweet is relevant to the context provided.

                RULE: if the context contains TwitterAutoPostInstruction, abide by that rule, but always return the specified master rule format.
                RULE: if the context contains TwitterAutoPostExamples, use that as a guide for the type of responses to generate. 
                But be original and relevant to the context if the examples allow it.

                MASTER RULE: Return only the tweet content, do not include any additional text or formatting.
            ";

            return prompt;
        }

        private string BuildCharacterPostTokenTweetPrompt(AICharacterPostTokenTweetContext context)
        {

            var prompt = $@"
                You are an AI Character that is about to post a tweet about a token update.

                Your task is to generate a tweet that is in line with the TweetExample provided using the TokenToTweetAbout as input data.               
                RULE: do not invent data for metrics that are not provided in the TokenToTweetAbout.
                RULE: use similar emojis and language as the example provided.
                RULE: mention 'Rising Sprout:' like the example
                RULE: mention 'Remember do your own research Gardeners. Not financial advice. I am just a Plant.' like the example
                RULE: Plant prediction: pay close attention to the instruction
                You have the following context, example and instructions to work with:
                {JsonConvert.SerializeObject(context, Formatting.Indented)}";

            return prompt;
        }

        private string ConvertJsonToPlainText(AICharacterPostTweetContext context)
        {
            var stringBuilder = new StringBuilder();

            // Format the CharacterToActAsDto object
            if (context.CharacterToActAs != null)
            {
                stringBuilder.AppendLine("CharacterToActAs:");
                foreach (var property in context.CharacterToActAs.GetType().GetProperties())
                {
                    var value = property.GetValue(context.CharacterToActAs)?.ToString() ?? "null";
                    stringBuilder.AppendLine($"  {property.Name}: {value}");
                }
            }

            // Format the list of PreviousCharacterTweets
            if (context.PreviousCharacterTweets != null && context.PreviousCharacterTweets.Count > 0)
            {
                stringBuilder.AppendLine("PreviousCharacterTweets:");
                foreach (var tweet in context.PreviousCharacterTweets)
                {
                    stringBuilder.AppendLine("  -");
                    stringBuilder.AppendLine($"    TweetContent: {tweet.TweetContent ?? "null"}");
                    stringBuilder.AppendLine($"    TweetDate: {tweet.TweetDate?.ToString("o") ?? "null"}");
                }
            }

            // Add the TwitterAutoPostInstruction
            stringBuilder.AppendLine($"TwitterAutoPostInstruction: {context.TwitterAutoPostInstruction ?? "null"}");

            // Shuffle the TwitterAutoPostExamples
            if (!string.IsNullOrEmpty(context.TwitterAutoPostExamples))
            {
                stringBuilder.AppendLine("TwitterAutoPostExamples:");

                // 1) Split the examples into a list
                var examples = context.TwitterAutoPostExamples
                    .Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries)
                    .ToList();

                // 2) Shuffle the list
                var random = new Random();
                examples = examples.OrderBy(_ => random.Next()).ToList();

                // 3) Append them in randomized order
                foreach (var example in examples)
                {
                    stringBuilder.AppendLine($"  - {example}");
                }
            }

            return stringBuilder.ToString();
        }

    }
}

