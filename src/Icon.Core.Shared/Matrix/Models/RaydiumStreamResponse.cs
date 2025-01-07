using System.Text.Json.Serialization;

namespace Icon.Matrix.Raydium
{
    public class RaydiumResponseWrapper
    {
        [JsonPropertyName("subscription_id")]
        public int SubscriptionId { get; set; }

        [JsonPropertyName("method")]
        public string Method { get; set; }

        [JsonPropertyName("params")]
        public RaydiumNewPairNotification Params { get; set; }
    }


    public class RaydiumNewPairNotification
    {
        [JsonPropertyName("slot")]
        public long Slot { get; set; }

        [JsonPropertyName("signature")]
        public string Signature { get; set; }

        [JsonPropertyName("blockTime")]
        public long BlockTime { get; set; }

        [JsonPropertyName("pair")]
        public RaydiumNewPairPairData Pair { get; set; }
    }

    public class RaydiumNewPairPairData
    {
        [JsonPropertyName("sourceExchange")]
        public string SourceExchange { get; set; }

        [JsonPropertyName("ammAccount")]
        public string AmmAccount { get; set; }

        [JsonPropertyName("baseToken")]
        public RaydiumNewPairTokenObject BaseToken { get; set; }

        [JsonPropertyName("quoteToken")]
        public RaydiumNewPairTokenObject QuoteToken { get; set; }

        [JsonPropertyName("baseTokenLiquidityAdded")]
        public string BaseTokenLiquidityAdded { get; set; }

        [JsonPropertyName("quoteTokenLiquidityAdded")]
        public string QuoteTokenLiquidityAdded { get; set; }
    }

    public class RaydiumNewPairTokenObject
    {
        [JsonPropertyName("account")]
        public string Account { get; set; }

        [JsonPropertyName("info")]
        public RaydiumNewPairTokenInfo Info { get; set; }
    }

    public class RaydiumNewPairTokenInfo
    {
        [JsonPropertyName("decimals")]
        public int Decimals { get; set; }

        [JsonPropertyName("supply")]
        public string Supply { get; set; }

        [JsonPropertyName("metadata")]
        public RaydiumNewPairTokenMetadata Metadata { get; set; }

        [JsonPropertyName("mintAuthority")]
        public object MintAuthority { get; set; }

        [JsonPropertyName("freezeAuthority")]
        public object FreezeAuthority { get; set; }
    }

    public class RaydiumNewPairTokenMetadata
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("symbol")]
        public string Symbol { get; set; }

        [JsonPropertyName("logo")]
        public string Logo { get; set; }
    }
}

