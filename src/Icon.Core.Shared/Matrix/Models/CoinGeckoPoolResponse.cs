using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Icon.Matrix.Coingecko
{
    public class CoingeckoPoolsResponse
    {
        [JsonPropertyName("data")]
        public List<CoingeckoPoolData> Data { get; set; }
    }

    public class CoingeckoPoolData
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("attributes")]
        public CoingeckoPoolAttributes Attributes { get; set; }

        [JsonPropertyName("relationships")]
        public CoingeckoPoolRelationships Relationships { get; set; }
    }

    public class CoingeckoPoolAttributes
    {
        [JsonPropertyName("base_token_price_usd")]
        public string BaseTokenPriceUsd { get; set; }

        [JsonPropertyName("base_token_price_native_currency")]
        public string BaseTokenPriceNativeCurrency { get; set; }

        [JsonPropertyName("quote_token_price_usd")]
        public string QuoteTokenPriceUsd { get; set; }

        [JsonPropertyName("quote_token_price_native_currency")]
        public string QuoteTokenPriceNativeCurrency { get; set; }

        [JsonPropertyName("base_token_price_quote_token")]
        public string BaseTokenPriceQuoteToken { get; set; }

        [JsonPropertyName("quote_token_price_base_token")]
        public string QuoteTokenPriceBaseToken { get; set; }

        [JsonPropertyName("address")]
        public string Address { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// Pool creation time in UTC, if returned in ISO8601 format.
        /// </summary>
        [JsonPropertyName("pool_created_at")]
        public DateTime? PoolCreatedAt { get; set; }

        [JsonPropertyName("token_price_usd")]
        public string TokenPriceUsd { get; set; }

        [JsonPropertyName("fdv_usd")]
        public string FdvUsd { get; set; }

        [JsonPropertyName("market_cap_usd")]
        public string MarketCapUsd { get; set; }

        [JsonPropertyName("price_change_percentage")]
        public CoingeckoPoolPriceChangePercentage PriceChangePercentage { get; set; }

        [JsonPropertyName("transactions")]
        public CoingeckoPoolTransactions Transactions { get; set; }

        [JsonPropertyName("volume_usd")]
        public CoingeckoPoolVolume VolumeUsd { get; set; }

        [JsonPropertyName("reserve_in_usd")]
        public string ReserveInUsd { get; set; }
    }

    public class CoingeckoPoolPriceChangePercentage
    {
        [JsonPropertyName("m5")]
        public string M5 { get; set; }

        [JsonPropertyName("h1")]
        public string H1 { get; set; }

        [JsonPropertyName("h6")]
        public string H6 { get; set; }

        [JsonPropertyName("h24")]
        public string H24 { get; set; }
    }

    public class CoingeckoPoolTransactions
    {
        [JsonPropertyName("m5")]
        public CoingeckoPoolTxnData M5 { get; set; }

        [JsonPropertyName("m15")]
        public CoingeckoPoolTxnData M15 { get; set; }

        [JsonPropertyName("m30")]
        public CoingeckoPoolTxnData M30 { get; set; }

        [JsonPropertyName("h1")]
        public CoingeckoPoolTxnData H1 { get; set; }

        [JsonPropertyName("h24")]
        public CoingeckoPoolTxnData H24 { get; set; }
    }

    public class CoingeckoPoolTxnData
    {
        [JsonPropertyName("buys")]
        public int Buys { get; set; }

        [JsonPropertyName("sells")]
        public int Sells { get; set; }

        [JsonPropertyName("buyers")]
        public int Buyers { get; set; }

        [JsonPropertyName("sellers")]
        public int Sellers { get; set; }
    }

    public class CoingeckoPoolVolume
    {
        [JsonPropertyName("m5")]
        public string M5 { get; set; }

        [JsonPropertyName("h1")]
        public string H1 { get; set; }

        [JsonPropertyName("h6")]
        public string H6 { get; set; }

        [JsonPropertyName("h24")]
        public string H24 { get; set; }
    }

    public class CoingeckoPoolRelationships
    {
        [JsonPropertyName("base_token")]
        public CoingeckoPoolRelationshipData BaseToken { get; set; }

        [JsonPropertyName("quote_token")]
        public CoingeckoPoolRelationshipData QuoteToken { get; set; }

        [JsonPropertyName("dex")]
        public CoingeckoPoolRelationshipData Dex { get; set; }
    }

    public class CoingeckoPoolRelationshipData
    {
        [JsonPropertyName("data")]
        public CoingeckoPoolRelationshipInfo Data { get; set; }
    }

    public class CoingeckoPoolRelationshipInfo
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }
    }
}
