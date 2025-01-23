using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Dependency;
using Icon.Configuration;
using Microsoft.Extensions.Configuration;
using OpenAI.Chat;

namespace Icon.Matrix.AIManager
{
    public interface IDirectOpenAIService
    {
        Task<string> GetCompletionAsync(List<ChatMessage> messages, ChatCompletionOptions options, bool shouldUseSmartModel = false);
    }

    public class DirectOpenAIService : IDirectOpenAIService, ITransientDependency
    {
        //private readonly ChatClient _openAIClient;
        private readonly IConfigurationRoot _configuration;
        private readonly string _modelMini;
        private readonly string _model;
        private readonly string _apiKey;

        public DirectOpenAIService(IAppConfigurationAccessor appConfigurationAccessor)
        {
            _configuration = appConfigurationAccessor.Configuration;

            // Read your direct OpenAI API key and models from config:
            _apiKey = _configuration["DirectOpenAI:ApiKey"];
            _modelMini = _configuration["DirectOpenAI:ModelMini"];
            _model = _configuration["DirectOpenAI:Model"];

            if (string.IsNullOrWhiteSpace(_apiKey))
            {
                throw new InvalidOperationException("OpenAI:ApiKey is missing in configuration");
            }
        }

        /// <summary>
        /// Basic chat-completion usage without Tools
        /// </summary>
        public async Task<string> GetCompletionAsync(List<ChatMessage> messages, ChatCompletionOptions options, bool shouldUseSmartModel = false)
        {
            // Choose the appropriate model based on the flag
            string modelToUse = shouldUseSmartModel ? _model : _modelMini;

            // Initialize the ChatClient with the chosen model
            var chatClient = new ChatClient(modelToUse, _apiKey);

            // Perform the chat completion
            ChatCompletion completion = await chatClient.CompleteChatAsync(messages, options);

            return completion.Content[0].Text;
        }
    }
}
