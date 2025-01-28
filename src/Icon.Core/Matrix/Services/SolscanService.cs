using Abp.Dependency;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Abp.Configuration.Startup;
using Icon.Configuration; // or your actual IAppConfigurationAccessor namespace

namespace Icon.Matrix.Solscan
{
    public interface ISolscanService
    {
        // 1. Token Metadata
        Task<SolscanTokenMetaResponse> GetTokenMetaAsync(string tokenMintAddress);

        // 2. Token Transfers
        Task<SolscanTokenTransferResponse> GetTokenTransfersAsync(string tokenMintAddress);

        // 3. Token Holders
        Task<SolscanTokenHoldersResponse> GetTokenHoldersAsync(string tokenMintAddress);

        // 4. Token Markets
        Task<SolscanTokenMarketResponse> GetTokenMarketsAsync(string tokenMintAddress);

        // 5. Account Transactions
        Task<SolscanAccountTransactionsResponse> GetAccountTransactionsAsync(string accountAddress);

        // 6. Block Transactions
        Task<SolscanBlockTransactionsResponse> GetBlockTransactionsAsync(long blockNumber);

        // 7. Transaction Details
        Task<SolscanTransactionDetailsResponse> GetTransactionDetailsAsync(string txSignature);

        // 8. (Optional) Newly Listed Tokens
        //    If Solscan has a list endpoint for newly created tokens
        Task<SolscanTokenListResponse> GetNewlyLaunchedTokensAsync(int page = 1, int limit = 50);
    }

    public class SolscanService : ISolscanService, ITransientDependency
    {
        private static HttpClient _httpClient;
        private readonly ILogger<SolscanService> _logger;
        private readonly Microsoft.Extensions.Configuration.IConfigurationRoot _configuration;

        public SolscanService(
            ILogger<SolscanService> logger,
            IAppConfigurationAccessor appConfigurationAccessor)
        {
            _logger = logger;
            _configuration = appConfigurationAccessor.Configuration;
            InitializeHttpClient();
        }

        /// <summary>
        /// Initializes the static HttpClient, setting the base address and default headers.
        /// E.g., "MatrixSolscan:BaseAddress" and "MatrixSolscan:ApiKey" in your appsettings.json
        /// </summary>
        private void InitializeHttpClient()
        {
            if (_httpClient == null)
            {
                var baseAddress = _configuration["MatrixSolscan:BaseAddress"];
                var apiKey = _configuration["MatrixSolscan:ApiKey"];

                if (string.IsNullOrEmpty(baseAddress))
                {
                    _logger.LogError("Solscan API base address is missing.");
                    throw new Exception("Solscan API base address is missing.");
                }
                if (string.IsNullOrEmpty(apiKey))
                {
                    _logger.LogError("Solscan API key is missing.");
                    throw new Exception("Solscan API key is missing.");
                }

                _httpClient = new HttpClient
                {
                    BaseAddress = new Uri(baseAddress)
                };

                // If Solscan requires a custom header (example):
                _httpClient.DefaultRequestHeaders.Add("x-api-key", apiKey);

                // Accept JSON
                _httpClient.DefaultRequestHeaders.Accept.Add(
                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            }
        }

        #region Endpoint Methods

        /// <summary>
        /// 1. Token Information (/token/meta)
        /// </summary>
        public Task<SolscanTokenMetaResponse> GetTokenMetaAsync(string tokenMintAddress)
        {
            // Example: GET /v2.0/token/meta?token={tokenMintAddress}
            return ExecuteRequestAsync<SolscanTokenMetaResponse>(
                () => _httpClient.GetAsync($"/v2.0/token/meta?token={tokenMintAddress}"),
                $"GetTokenMetaAsync({tokenMintAddress})"
            );
        }

        /// <summary>
        /// 2. Token Transfers (/token/transfer)
        /// </summary>
        public Task<SolscanTokenTransferResponse> GetTokenTransfersAsync(string tokenMintAddress)
        {
            // Example: GET /v2.0/token/transfer?token={tokenMintAddress}
            return ExecuteRequestAsync<SolscanTokenTransferResponse>(
                () => _httpClient.GetAsync($"/v2.0/token/transfer?token={tokenMintAddress}"),
                $"GetTokenTransfersAsync({tokenMintAddress})"
            );
        }

        /// <summary>
        /// 3. Token Holders (/token/holders)
        /// </summary>
        public Task<SolscanTokenHoldersResponse> GetTokenHoldersAsync(string tokenMintAddress)
        {
            // Example: GET /v2.0/token/holders?token={tokenMintAddress}
            return ExecuteRequestAsync<SolscanTokenHoldersResponse>(
                () => _httpClient.GetAsync($"/v2.0/token/holders?token={tokenMintAddress}"),
                $"GetTokenHoldersAsync({tokenMintAddress})"
            );
        }

        /// <summary>
        /// 4. Token Market Data (/token/markets)
        /// </summary>
        public Task<SolscanTokenMarketResponse> GetTokenMarketsAsync(string tokenMintAddress)
        {
            // Example: GET /v2.0/token/markets?token={tokenMintAddress}
            return ExecuteRequestAsync<SolscanTokenMarketResponse>(
                () => _httpClient.GetAsync($"/v2.0/token/markets?token={tokenMintAddress}"),
                $"GetTokenMarketsAsync({tokenMintAddress})"
            );
        }

        /// <summary>
        /// 5. Account Transactions (/account/transactions)
        /// </summary>
        public Task<SolscanAccountTransactionsResponse> GetAccountTransactionsAsync(string accountAddress)
        {
            // Example: GET /v2.0/account/transactions?account={accountAddress}
            return ExecuteRequestAsync<SolscanAccountTransactionsResponse>(
                () => _httpClient.GetAsync($"/v2.0/account/transactions?account={accountAddress}"),
                $"GetAccountTransactionsAsync({accountAddress})"
            );
        }

        /// <summary>
        /// 6. Block Transactions (/block/transactions)
        /// </summary>
        public Task<SolscanBlockTransactionsResponse> GetBlockTransactionsAsync(long blockNumber)
        {
            // Example: GET /v2.0/block/transactions?block={blockNumber}
            return ExecuteRequestAsync<SolscanBlockTransactionsResponse>(
                () => _httpClient.GetAsync($"/v2.0/block/transactions?block={blockNumber}"),
                $"GetBlockTransactionsAsync({blockNumber})"
            );
        }

        /// <summary>
        /// 7. Transaction Details (/transaction/details)
        /// </summary>
        public Task<SolscanTransactionDetailsResponse> GetTransactionDetailsAsync(string txSignature)
        {
            // Example: GET /v2.0/transaction/details?tx={txSignature}
            return ExecuteRequestAsync<SolscanTransactionDetailsResponse>(
                () => _httpClient.GetAsync($"/v2.0/transaction/details?tx={txSignature}"),
                $"GetTransactionDetailsAsync({txSignature})"
            );
        }

        /// <summary>
        /// 8. List Newly Launched Tokens (for example, if Solscan has such an endpoint)
        /// </summary>
        public Task<SolscanTokenListResponse> GetNewlyLaunchedTokensAsync(int page = 1, int limit = 50)
        {
            // Example: GET /v2.0/token/list?page={page}&limit={limit}
            return ExecuteRequestAsync<SolscanTokenListResponse>(
                () => _httpClient.GetAsync($"/v2.0/token/list?page={page}&limit={limit}"),
                $"GetNewlyLaunchedTokensAsync(page: {page}, limit: {limit})"
            );
        }

        #endregion

        #region Reusable Helper

        /// <summary>
        /// Helper method to log the request, catch errors, and deserialize the JSON into T.
        /// </summary>
        private async Task<T> ExecuteRequestAsync<T>(
            Func<Task<HttpResponseMessage>> requestAction,
            string operationName)
        {
            try
            {
                _logger.LogInformation($"Starting request: {operationName}");

                var response = await requestAction();
                var responseString = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Request failed: {operationName} " +
                                     $"- StatusCode: {(int)response.StatusCode}, " +
                                     $"Content: {responseString}");
                    response.EnsureSuccessStatusCode(); // will throw
                }

                var result = JsonSerializer.Deserialize<T>(
                    responseString,
                    new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        PropertyNameCaseInsensitive = true
                    }
                );

                _logger.LogInformation($"Request successful: {operationName}");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in {operationName}");
                throw;
            }
        }

        #endregion
    }
}
