using System;
using System.ClientModel;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Abp.Dependency;
using Icon.Configuration;
using Microsoft.Extensions.Configuration;
using OpenAI.Chat;

namespace Icon.Matrix.AIManager
{
    public interface IDirectOpenAIService
    {
        Task<string> GetCompletionAsync(List<ChatMessage> messages, ChatCompletionOptions options);
    }

    public class DirectOpenAIService : IDirectOpenAIService, ITransientDependency
    {
        private readonly ChatClient _openAIClient;
        private readonly IConfigurationRoot _configuration;

        public DirectOpenAIService(IAppConfigurationAccessor appConfigurationAccessor)
        {
            _configuration = appConfigurationAccessor.Configuration;

            // Read your direct OpenAI API key from config:
            var apiKey = _configuration["DirectOpenAI:ApiKey"];
            var model = _configuration["DirectOpenAI:Model"];

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new InvalidOperationException("OpenAI:ApiKey is missing in configuration");
            }

            _openAIClient = new ChatClient(model, apiKey);
        }

        /// <summary>
        /// Basic chat-completion usage without Tools
        /// </summary>
        public async Task<string> GetCompletionAsync(List<ChatMessage> messages, ChatCompletionOptions options)
        {
            ChatCompletion completion = await _openAIClient.CompleteChatAsync(messages, options);

            return completion.Content[0].Text;
        }

    }
}
