using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Abp.Dependency;
using Icon.Configuration;
using Microsoft.Extensions.Configuration;
using OpenAI.Chat;

namespace Icon.Matrix.AIManager
{
    public interface ILlamaAIService
    {
        Task<string> GetCompletionAsync(
            List<LlamaMessage> llamaMessages,
            float temperature,
            int maxTokens
        );
    }

    public class AzureLlamaAIService : ILlamaAIService, ITransientDependency
    {
        private readonly string _endpoint;
        private readonly IConfigurationRoot _configuration;

        public AzureLlamaAIService(IAppConfigurationAccessor appConfigurationAccessor)
        {
            _configuration = appConfigurationAccessor.Configuration;

            _endpoint = _configuration["Llama:Endpoint"];
            if (string.IsNullOrWhiteSpace(_endpoint))
            {
                throw new InvalidOperationException("Llama:Endpoint is missing in configuration");
            }
        }

        public async Task<string> GetCompletionAsync(
            List<LlamaMessage> llamaMessages,
            float temperature,
            int maxTokens
        )
        {
            // Build the request body
            var requestBody = new LlamaChatRequest
            {
                Model = "Meta-Llama-3-3-70B-Instruct",
                Messages = llamaMessages,
                // If your endpoint supports these:
                Temperature = temperature,
                MaxTokens = maxTokens
            };

            // Prepare JSON for POST
            using var httpClient = new HttpClient();

            // If an API key or header is needed:
            var apiKey = _configuration["Llama:ApiKey"];

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new InvalidOperationException("Llama:ApiKey is missing in configuration");
            }

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

            var jsonContent = new StringContent(
                JsonSerializer.Serialize(requestBody),
                Encoding.UTF8,
                "application/json"
            );

            using var response = await httpClient.PostAsync(_endpoint, jsonContent);
            response.EnsureSuccessStatusCode();

            var responseJson = await response.Content.ReadAsStringAsync();
            var llamaResponse = JsonSerializer.Deserialize<LlamaChatResponse>(
                responseJson,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );

            // For now, just return the content of the first choice
            if (llamaResponse?.Choices?.Count > 0)
            {
                return llamaResponse.Choices[0].Message.Content;
            }

            return string.Empty;
        }
    }

    public class LlamaChatRequest
    {
        [JsonPropertyName("model")]
        public string Model { get; set; }
        [JsonPropertyName("messages")]
        public List<LlamaMessage> Messages { get; set; }

        [JsonPropertyName("temperature")]
        public float Temperature { get; set; }
        [JsonPropertyName("max_tokens")]
        public int MaxTokens { get; set; }
    }

    public class LlamaMessage
    {
        [JsonPropertyName("role")]
        public string Role { get; set; }      // "system", "user", or "assistant"
        [JsonPropertyName("content")]
        public string Content { get; set; }
    }

    public class LlamaChatResponse
    {
        public string Id { get; set; }
        public string Object { get; set; }
        public long Created { get; set; }
        public string Model { get; set; }
        public List<LlamaChoice> Choices { get; set; }
    }

    public class LlamaChoice
    {
        public int Index { get; set; }
        public LlamaChoiceMessage Message { get; set; }
        public string FinishReason { get; set; }
    }

    public class LlamaChoiceMessage
    {
        public string Role { get; set; }
        public string Content { get; set; }
    }
}

