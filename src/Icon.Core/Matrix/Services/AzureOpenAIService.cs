using System;
using System.ClientModel;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Abp.Dependency;
using Azure;
using Azure.AI.OpenAI;
using Icon.Configuration;
using Microsoft.Extensions.Configuration;
using OpenAI.Chat;

namespace Icon.Matrix.AIManager
{
    public interface IAzureOpenAIService
    {
        Task<string> GetCompletionAsync(List<ChatMessage> messages, ChatCompletionOptions options);
        Task<string> GetCompletionWithToolsAsync(string prompt);
    }

    public class AzureOpenAIService : IAzureOpenAIService, ITransientDependency
    {
        private readonly AzureOpenAIClient _azureClient;
        private IConfigurationRoot _configuration;
        private readonly string _model;

        public AzureOpenAIService(IAppConfigurationAccessor appConfigurationAccessor)
        {
            _configuration = appConfigurationAccessor.Configuration;

            var endpoint = _configuration["AzureOpenAI:Endpoint"];
            var apiKey = _configuration["AzureOpenAI:ApiKey"];
            _model = _configuration["AzureOpenAI:Model"];

            if (string.IsNullOrWhiteSpace(endpoint) || string.IsNullOrWhiteSpace(apiKey))
            {
                throw new InvalidOperationException("OpenAI configuration is missing");
            }

            _azureClient = new AzureOpenAIClient(new Uri(endpoint), new ApiKeyCredential(apiKey));
        }

        public async Task<string> GetCompletionAsync(List<ChatMessage> messages, ChatCompletionOptions options)
        {
            var chatClient = _azureClient.GetChatClient(_model);

            ChatCompletion completion = await chatClient.CompleteChatAsync(messages, options);
            return completion.Content[0].Text;
        }

        public async Task<string> GetCompletionWithToolsAsync(string prompt)
        {
            var chatClient = _azureClient.GetChatClient("gpt-4o");

            ChatCompletionOptions options = new()
            {
                Tools = { getCurrentLocationTool, getCurrentWeatherTool },
            };

            List<ChatMessage> conversationMessages =
                [
                    new UserChatMessage("What's the weather like in Boston?"),
                ];

            ChatCompletion completion = await chatClient.CompleteChatAsync(conversationMessages, options);

            if (completion.FinishReason == ChatFinishReason.ToolCalls)
            {
                // Add a new assistant message to the conversation history that includes the tool calls
                conversationMessages.Add(new AssistantChatMessage(completion));

                foreach (ChatToolCall toolCall in completion.ToolCalls)
                {
                    conversationMessages.Add(new ToolChatMessage(toolCall.Id, GetToolCallContent(toolCall)));
                }

                // Now make a new request with all the messages thus far, including the original
            }

            completion = await chatClient.CompleteChatAsync(conversationMessages);

            return $"{completion.Role}: {completion.Content[0].Text}";

        }


        static string GetCurrentLocation()
        {
            // Call the location API here.
            return "San Francisco";
        }

        static string GetCurrentWeather(string location, string unit = "celsius")
        {
            // Call the weather API here.
            return $"31 {unit}";
        }

        ChatTool getCurrentLocationTool = ChatTool.CreateFunctionTool(
            functionName: nameof(GetCurrentLocation),
            functionDescription: "Get the user's current location"
        );

        ChatTool getCurrentWeatherTool = ChatTool.CreateFunctionTool(
            functionName: nameof(GetCurrentWeather),
            functionDescription: "Get the current weather in a given location",
            functionParameters: BinaryData.FromString("""
                {
                    "type": "object",
                    "properties": {
                        "location": {
                            "type": "string",
                            "description": "The city and state, e.g. Boston, MA"
                        },
                        "unit": {
                            "type": "string",
                            "enum": [ "celsius", "fahrenheit" ],
                            "description": "The temperature unit to use. Infer this from the specified location."
                        }
                    },
                    "required": [ "location" ]
                }
                """)
        );

        string GetToolCallContent(ChatToolCall toolCall)
        {
            if (toolCall.FunctionName == getCurrentWeatherTool.FunctionName)
            {
                // Validate arguments before using them; it's not always guaranteed to be valid JSON!
                try
                {
                    using JsonDocument argumentsDocument = JsonDocument.Parse(toolCall.FunctionArguments);
                    if (!argumentsDocument.RootElement.TryGetProperty("location", out JsonElement locationElement))
                    {
                        // Handle missing required "location" argument
                    }
                    else
                    {
                        string location = locationElement.GetString();
                        if (argumentsDocument.RootElement.TryGetProperty("unit", out JsonElement unitElement))
                        {
                            return GetCurrentWeather(location, unitElement.GetString());
                        }
                        else
                        {
                            return GetCurrentWeather(location);
                        }
                    }
                }
                catch (JsonException)
                {
                    // Handle the JsonException (bad arguments) here
                }
            }
            // Handle unexpected tool calls
            throw new NotImplementedException();
        }

    }
}
