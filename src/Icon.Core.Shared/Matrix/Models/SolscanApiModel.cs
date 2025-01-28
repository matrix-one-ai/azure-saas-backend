using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Icon.Matrix.Solscan
{
    #region 1. Token Meta (/token/meta)
    public class SolscanTokenMetaResponse
    {
        // e.g., token name, symbol, total supply, decimals, etc.
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("decimals")]
        public int Decimals { get; set; }

        [JsonPropertyName("tokenAuthority")]
        public string TokenAuthority { get; set; }

        [JsonPropertyName("totalSupply")]
        public ulong TotalSupply { get; set; }

        // TODO: Add more properties according to actual Solscan "token/meta" response
    }
    #endregion

    #region 2. Token Transfers (/token/transfer)
    public class SolscanTokenTransferResponse
    {
        [JsonPropertyName("data")]
        public List<SolscanTransferItem> Data { get; set; }

        [JsonPropertyName("total")]
        public int Total { get; set; }

        // Pagination or other metadata might be here
        // TODO: adjust to actual response
    }

    public class SolscanTransferItem
    {
        [JsonPropertyName("txHash")]
        public string TxHash { get; set; }

        [JsonPropertyName("blockTime")]
        public long BlockTime { get; set; }

        [JsonPropertyName("amount")]
        public string Amount { get; set; }

        [JsonPropertyName("from")]
        public string From { get; set; }

        [JsonPropertyName("to")]
        public string To { get; set; }

        // TODO: add additional fields as needed
    }
    #endregion

    #region 3. Token Holders (/token/holders)
    public class SolscanTokenHoldersResponse
    {
        [JsonPropertyName("data")]
        public List<SolscanHolderInfo> Data { get; set; }

        [JsonPropertyName("total")]
        public int Total { get; set; }

        // Possibly additional fields: pagination, etc.
    }

    public class SolscanHolderInfo
    {
        [JsonPropertyName("address")]
        public string Address { get; set; }

        [JsonPropertyName("amount")]
        public string Amount { get; set; }

        // TODO: add more fields if Solscan provides them (percentage ownership, etc.)
    }
    #endregion

    #region 4. Token Markets (/token/markets)
    public class SolscanTokenMarketResponse
    {
        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        [JsonPropertyName("marketCap")]
        public decimal MarketCap { get; set; }

        [JsonPropertyName("fdv")]
        public decimal Fdv { get; set; }

        [JsonPropertyName("volume24h")]
        public decimal Volume24h { get; set; }

        [JsonPropertyName("priceChange24h")]
        public decimal PriceChange24h { get; set; }

        // TODO: expand with more fields from the /token/markets endpoint
    }
    #endregion

    #region 5. Account Transactions (/account/transactions)
    public class SolscanAccountTransactionsResponse
    {
        [JsonPropertyName("transactions")]
        public List<SolscanAccountTransactionItem> Transactions { get; set; }

        [JsonPropertyName("total")]
        public int Total { get; set; }

        // Possibly additional metadata
    }

    public class SolscanAccountTransactionItem
    {
        [JsonPropertyName("signature")]
        public string Signature { get; set; }

        [JsonPropertyName("slot")]
        public long Slot { get; set; }

        [JsonPropertyName("blockTime")]
        public long BlockTime { get; set; }

        // TODO: More detail about transaction type, addresses, etc.
    }
    #endregion

    #region 6. Block Transactions (/block/transactions)
    public class SolscanBlockTransactionsResponse
    {
        [JsonPropertyName("blockNumber")]
        public long BlockNumber { get; set; }

        [JsonPropertyName("timestamp")]
        public long Timestamp { get; set; }

        [JsonPropertyName("txCount")]
        public int TxCount { get; set; }

        [JsonPropertyName("transactions")]
        public List<SolscanBlockTransactionItem> Transactions { get; set; }

        // Possibly more fields
    }

    public class SolscanBlockTransactionItem
    {
        [JsonPropertyName("signature")]
        public string Signature { get; set; }

        [JsonPropertyName("slot")]
        public long Slot { get; set; }

        [JsonPropertyName("err")]
        public object Err { get; set; } // or a specific type if it's a known structure

        // TODO: add more fields as needed
    }
    #endregion

    #region 7. Transaction Details (/transaction/details)
    public class SolscanTransactionDetailsResponse
    {
        [JsonPropertyName("transactionHash")]
        public string TransactionHash { get; set; }

        [JsonPropertyName("slot")]
        public long Slot { get; set; }

        [JsonPropertyName("blockTime")]
        public long BlockTime { get; set; }

        [JsonPropertyName("meta")]
        public SolscanTransactionMeta Meta { get; set; }

        // TODO: Expand based on actual response structure
    }

    public class SolscanTransactionMeta
    {
        [JsonPropertyName("err")]
        public object Err { get; set; }

        [JsonPropertyName("fee")]
        public long Fee { get; set; }

        // e.g., postTokenBalances, preTokenBalances, logs, etc.
        // TODO: Add fields from the Solscan transaction details doc
    }
    #endregion

    #region 8. Token List (Newly Launched Tokens) (/token/list)
    public class SolscanTokenListResponse
    {
        [JsonPropertyName("data")]
        public List<SolscanListedToken> Data { get; set; }

        [JsonPropertyName("page")]
        public int Page { get; set; }

        [JsonPropertyName("limit")]
        public int Limit { get; set; }

        [JsonPropertyName("total")]
        public int Total { get; set; }
    }

    public class SolscanListedToken
    {
        [JsonPropertyName("mint")]
        public string Mint { get; set; }

        [JsonPropertyName("symbol")]
        public string Symbol { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        // Possibly creation time, etc.
        // TODO: Add more fields based on actual Solscan response
    }
    #endregion
}
