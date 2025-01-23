using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Abp.Domain.Entities;
using Icon.Matrix.Enums;

namespace Icon.Matrix.Models
{
    public class RaydiumPair : Entity<Guid>
    {
        public DateTimeOffset CreationTime { get; set; }


        public long Slot { get; set; }
        public string Signature { get; set; }
        public long BlockTime { get; set; }
        public string SourceExchange { get; set; }
        public string AmmAccount { get; set; }
        public string BaseTokenAccount { get; set; }
        public int? BaseTokenDecimals { get; set; }
        public string BaseTokenSupply { get; set; }
        public string BaseTokenName { get; set; }
        public string BaseTokenSymbol { get; set; }
        public string BaseTokenLogo { get; set; }
        public string BaseTokenLiquidityAdded { get; set; }
        public string QuoteTokenAccount { get; set; }
        public string QuoteTokenLiquidityAdded { get; set; }



        //public DateTimeOffset? LastPoolUpdate { get; set; }

        public string DiscoveryStageName { get; set; }
        public DateTimeOffset? DiscoveryStageLastUpdated { get; set; }

        public bool TweetSent { get; set; }
        public string TweetText { get; set; }
        public DateTimeOffset? TweetSentAt { get; set; }


        public Guid? CoingeckoLastAggregatedUpdateId { get; set; }

        public bool PriceRefreshEnabled { get; set; }
        public int PriceRefreshIntervalSeconds { get; set; }
        public DateTimeOffset? PriceRefreshLastUpdateTime { get; set; }
        public DateTimeOffset? PriceRefreshNextUpdateTime { get; set; }

        public float? TotalLiquidityUsd { get; set; }



        public int TwitterCAQueryCount { get; set; }
        public int TweetsCATweetCount { get; set; }
        public int TweetsCATweetCount1H { get; set; }
        public int TweetsCATweetCount3H { get; set; }
        public int TweetsCATweetCount6H { get; set; }
        public int TweetsCATweetCount12H { get; set; }
        public int TweetsCATweetCount24H { get; set; }
        public int TweetsCATweetCount1WK { get; set; }

        public bool TwitterCAFound { get; set; }
        public DateTimeOffset? TwitterCAFoundAtTime { get; set; }
        public DateTimeOffset? TwitterCAFirstMentionTime { get; set; }
        public string TwitterCAFirstMentionTweetId { get; set; }
        public string TwitterCAFirstMentionText { get; set; }
        public string TwitterCAFirstMentionHandle { get; set; }

        public int TwitterCAMostLikesOnSingleTweet { get; set; }
        public int TwitterCAMostRepliesOnSingleTweet { get; set; }

        public int TweetsCAEngagementTweetsImported { get; set; }
        public int TweetsCAEngagementTotalLikes { get; set; }
        public int TweetsCAEngagementTotalReplies { get; set; }
        public int TweetsCAEngagementTotalRetweets { get; set; }
        public int TweetsCAEngagementTotalViews { get; set; }
        public int TweetsCAEngagementTotalQuotes { get; set; }


        public bool TwitterRefreshEnabled { get; set; }
        public DateTimeOffset? TwitterRefreshLastUpdateTime { get; set; }
        public DateTimeOffset? TwitterRefreshNextUpdateTime { get; set; }
        public DateTimeOffset? TwitterRefreshEnabledUntilTime { get; set; }
        public int TwitterRefreshIntervalSeconds { get; set; }


        // Volume-to-Liquidity Ratio (VLR)
        public float? VlrValue { get; set; }
        public string VlrQualitativeAnalysis { get; set; }
        public string VlrRecommendation { get; set; }

        // Buys-to-Sells Ratio (B/S)
        public float? BuySellRatioValue { get; set; }
        public string BuySellRatioQualitativeAnalysis { get; set; }
        public string BuySellRatioRecommendation { get; set; }

        // 1-Hour Volume-to-FDV Ratio
        public float? VolumeToFdv1HValue { get; set; }
        public string VolumeToFdv1HQualitativeAnalysis { get; set; }
        public string VolumeToFdv1HRecommendation { get; set; }

        // Combined final metric
        public float? CombinedMetricScore { get; set; }
        public string CombinedQualitativeAnalysis { get; set; }
        public string CombinedRecommendation { get; set; }

        public string EngagementCorrelationAnalysis { get; set; }
        public float? EngagementCorrelationScore { get; set; }
        public string PlantFinalSummary { get; set; }
        public string PlantFinalPrediction { get; set; }
    }


}