using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Runtime.Session;
using Icon.Matrix.AIManager.CharacterMentioned;
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
        Task<AICharacterMentionedResponse> GenerateMentionedResponseAsync(AICharacterMentionedContext context, AIModelType modelType);
        Task<string> GeneratePostTweetResponseAsync(AICharacterPostTweetContext context, AIModelType modelType);
    }

    public class AIManager : IAIManager, ITransientDependency
    {
        private readonly IOpenAIService _openAIService;
        private readonly ILlamaAIService _llamaAIService;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public AIManager(
            IOpenAIService openAIService,
            ILlamaAIService llamaAIService,
            IUnitOfWorkManager unitOfWorkManager,
            IAbpSession abpSession
        )
        {
            _openAIService = openAIService;
            _llamaAIService = llamaAIService;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public async Task<string> GenerateMentionedPromptAsync(AICharacterMentionedContext context)
        {
            await Task.CompletedTask;
            var prompt = BuildCharacterMentionedPrompt(context);
            return prompt;
        }

        public async Task<string> GeneratePostTweetPromptAsync(AICharacterPostTweetContext context)
        {
            await Task.CompletedTask;
            var prompt = BuildCharacterPostTweetPrompt(context);
            return prompt;
        }

        public async Task<string> GeneratePostTweetResponseAsync(AICharacterPostTweetContext context, AIModelType modelType)
        {
            if (modelType != AIModelType.OpenAI && modelType != AIModelType.Llama)
            {
                throw new NotSupportedException("Currently only OpenAI && Llama model is supported");
            }

            var prompt = BuildCharacterPostTweetPrompt(context);
            float temperature = 0.7f;
            int maxTokens = 500;

            string rawResponse = "";

            try
            {
                if (modelType == AIModelType.OpenAI)
                {
                    var openAIMessages = new List<ChatMessage>
                    {
                        new SystemChatMessage("You are an AI assistant that is about to post a tweet."),
                        new UserChatMessage(prompt)
                    };
                    var chatOptions = new ChatCompletionOptions
                    {
                        MaxOutputTokenCount = maxTokens,
                        Temperature = temperature
                    };

                    rawResponse = await _openAIService.GetCompletionAsync(openAIMessages, chatOptions);
                }
                else if (modelType == AIModelType.Llama)
                {
                    var llamaMessages = new List<LlamaMessage>
                    {
                        new LlamaMessage { Role = "system", Content = "You are an AI assistant that is about to post a tweet." },
                        new LlamaMessage { Role = "user", Content = prompt }
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

        public async Task<AICharacterMentionedResponse> GenerateMentionedResponseAsync(
            AICharacterMentionedContext context,
            AIModelType modelType)
        {
            if (modelType != AIModelType.OpenAI && modelType != AIModelType.Llama)
            {
                throw new NotSupportedException("Currently only OpenAI && Llama model is supported");
            }

            var prompt = BuildCharacterMentionedPrompt(context);
            float temperature = 0.7f;
            int maxTokens = 500;

            string rawResponse = "";
            var aiResponse = new AICharacterMentionedResponse();

            try
            {
                if (modelType == AIModelType.OpenAI)
                {
                    var openAIMessages = new List<ChatMessage>
                    {
                        new SystemChatMessage("You are an AI assistant analyzing social media interactions."),
                        new UserChatMessage(prompt)
                    };
                    var chatOptions = new ChatCompletionOptions
                    {
                        MaxOutputTokenCount = maxTokens,
                        Temperature = temperature
                    };

                    rawResponse = await _openAIService.GetCompletionAsync(openAIMessages, chatOptions);
                }
                else if (modelType == AIModelType.Llama)
                {
                    var llamaMessages = new List<LlamaMessage>
                    {
                        new LlamaMessage { Role = "system", Content = "You are an AI assistant analyzing social media interactions." },
                        new LlamaMessage { Role = "user", Content = prompt }
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

RULE: if the context contains TwitterPostInstruction, abide by that rule, but always return the specified JSON format.
RULE: if the context contains TwitterPostExamples, use that as a guide for the type of responses to generate. But be original and relevant to the context if the examples allow it.

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
            var prompt = $@"You are an AI Character that is about to post a tweet. You have the following context to work with:
            {JsonConvert.SerializeObject(context, Formatting.Indented)}

            Your task is to generate a tweet that is in line with the character's personality and the context provided.
            Be original and make sure the tweet is relevant to the context provided. 
            Return only the tweet content, do not include any additional text or formatting.";

            return prompt;
        }
    }
}


// public async Task<string> GenerateResponse(string prompt, AIModelType modelType)
// {
//     if (modelType != AIModelType.OpenAI && modelType != AIModelType.Llama)
//     {
//         throw new NotSupportedException("Currently only OpenAI and Llama model is supported");
//     }

//     // Hard-coded or read from config
//     float temperature = 0.7f;
//     int maxTokens = 500;

//     if (modelType == AIModelType.OpenAI)
//     {
//         // 1) Build ChatMessages (OpenAI)
//         var messages = new List<ChatMessage>()
//         {
//             new SystemChatMessage("You are a helpful assistant."),
//             new UserChatMessage(prompt)
//         };

//         var chatOptions = new ChatCompletionOptions
//         {
//             MaxOutputTokenCount = maxTokens,
//             Temperature = temperature,
//         };

//         // 2) Send to OpenAI
//         return await _openAIService.GetCompletionAsync(messages, chatOptions);
//     }
//     else
//     {
//         // 1) Build LlamaMessages
//         var llamaMessages = new List<LlamaMessage>
//         {
//             new LlamaMessage { Role = "system", Content = "You are a helpful assistant." },
//             new LlamaMessage { Role = "user", Content = prompt }
//         };

//         // 2) Send to Llama
//         return await _llamaAIService.GetCompletionAsync(llamaMessages, temperature, maxTokens);
//     }
// }