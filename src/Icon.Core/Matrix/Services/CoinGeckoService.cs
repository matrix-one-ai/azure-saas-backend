using Abp.Dependency;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Abp.Configuration.Startup;
using Icon.Configuration; // or your actual IAppConfigurationAccessor namespace

namespace Icon.Matrix.Coingecko
{
    public interface ICoingeckoService
    {
        Task<CoingeckoPoolsResponse> GetPoolsAsync(string tokenAddress);
    }

    public class CoingeckoService : ICoingeckoService, ITransientDependency
    {
        private static HttpClient _httpClient;
        private readonly ILogger<CoingeckoService> _logger;
        private readonly Microsoft.Extensions.Configuration.IConfigurationRoot _configuration;

        public CoingeckoService(
            ILogger<CoingeckoService> logger,
            IAppConfigurationAccessor appConfigurationAccessor)
        {
            _logger = logger;
            _configuration = appConfigurationAccessor.Configuration;

            InitializeHttpClient();
        }

        /// <summary>
        /// Initializes the static HttpClient, setting the base address and 
        /// default headers from the app settings (similar to TwitterCommunicationService).
        /// </summary>
        private void InitializeHttpClient()
        {
            if (_httpClient == null)
            {
                // Example config keys (adapt to your naming in appsettings.json)
                var baseAddress = _configuration["MatrixCoingecko:BaseAddress"];
                var apiKey = _configuration["MatrixCoingecko:ApiKey"];

                if (string.IsNullOrEmpty(baseAddress))
                {
                    _logger.LogError("Coingecko API base address is missing.");
                    throw new Exception("Coingecko API base address is missing.");
                }
                if (string.IsNullOrEmpty(apiKey))
                {
                    _logger.LogError("Coingecko API key is missing.");
                    throw new Exception("Coingecko API key is missing.");
                }

                _httpClient = new HttpClient
                {
                    BaseAddress = new Uri(baseAddress)
                };

                // Coingecko Pro API often uses "x-cg-pro-api-key" header
                _httpClient.DefaultRequestHeaders.Add("x-cg-pro-api-key", apiKey);
                _httpClient.DefaultRequestHeaders.Accept.Add(
                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            }
        }

        /// <summary>
        /// Example method to retrieve pools for a given Solana token address.
        /// E.g. GET /api/v3/onchain/networks/solana/tokens/{tokenAddress}/pools
        /// </summary>
        public Task<CoingeckoPoolsResponse> GetPoolsAsync(string tokenAddress)
        {
            return ExecuteRequestAsync<CoingeckoPoolsResponse>(
                () => _httpClient.GetAsync($"/api/v3/onchain/networks/solana/tokens/{tokenAddress}/pools"),
                $"GetPoolsAsync(tokenAddress: {tokenAddress})"
            );
        }

        /// <summary>
        /// Helper method to log the request, catch errors, and deserialize the JSON into T.
        /// Similar pattern as the TwitterCommunicationService.
        /// </summary>
        /// <typeparam name="T">Type to which JSON will be deserialized</typeparam>
        /// <param name="requestAction">Function performing the HttpRequest (GET/POST/DELETE...)</param>
        /// <param name="operationName">Human-readable operation name for logs</param>
        private async Task<T> ExecuteRequestAsync<T>(Func<Task<HttpResponseMessage>> requestAction, string operationName)
        {
            try
            {
                _logger.LogInformation($"Starting request: {operationName}");

                var response = await requestAction();
                var responseString = await response.Content.ReadAsStringAsync();

                // If not 2xx response, log an error and throw
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Request failed: {operationName} - StatusCode: {(int)response.StatusCode}, Content: {responseString}");
                    response.EnsureSuccessStatusCode();
                }

                // Deserialize using System.Text.Json
                var result = JsonSerializer.Deserialize<T>(responseString, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    PropertyNameCaseInsensitive = true
                });

                _logger.LogInformation($"Request successful: {operationName}");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in {operationName}");
                throw;
            }
        }
    }
}
