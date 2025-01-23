using System;
using Icon.Matrix.Models;

namespace Icon.Matrix.AIManager.CharacterPostTokenTweet
{
    public class AICharacterPostTokenTweetContext
    {
        public string TweetExample { get; set; }
        public TokenToTweetAbout TokenToTweetAbout { get; set; }

        public AICharacterPostTokenTweetContext(
            RaydiumPair raydiumPair,
            CoingeckoAggregatedUpdate coingeckoPoolUpdate)
        {
            // 1) Map base tweet example 
            TweetExample = $@"
🌱 Rising Sprout: $[TokenToTweetAbout.BaseTokenSymbol] | [Potential Meme/AI/Celebriry/Political/Etc]
CA: [TokenToTweetAbout.Address]

🕒 Raydium listing: [TokenToTweetAbout.TimeSinceListingSeconds] in minutes

|| AGGREGATED POOL METRICS ||

🕒 Price update: 
🏊‍♂️ Pool count: TokenToTweetAbout.AggregatedPoolUpdate.Pools]

📊 Price change (5m): [TokenToTweetAbout.AggregatedPoolUpdate.PriceChangeM5]%
📊 Price change (1h): [TokenToTweetAbout.AggregatedPoolUpdate.PriceChangeH1]%

🌊 Volume (1hr): [TokenToTweetAbout.AggregatedPoolUpdate.VolumeH1]
🍃 Liquidity: [TokenToTweetAbout.AggregatedPoolUpdate.ReserveInUsd]
🌍 FDV: [TokenToTweetAbout.AggregatedPoolUpdate.FdvUsd]

📤 Buys / Sells (5m): [TokenToTweetAbout.AggregatedPoolUpdate.M5Buys] / [TokenToTweetAbout.AggregatedPoolUpdate.M5Sells]
🤝 Buyers / Sellers (5m): [TokenToTweetAbout.AggregatedPoolUpdate.M5Buyers] / [TokenToTweetAbout.AggregatedPoolUpdate.M5Sellers]

|| ENGAGEMENT METRICS ||

[if TokenToTweetAbout.TwitterCAFound]

𝕏  CA first mention: [TokenToTweetAbout.TwitterCAFirstMentionTime] minutes / hours /days ago | or not found

𝕏  CA total posts: [TokenToTweetAbout.TwitterCAFirstMentionTime] 
𝕏  CA total likes: [TokenToTweetAbout.TweetsCAEngagementTotalLikes]
𝕏  CA total replies: [TokenToTweetAbout.TweetsCAEngagementTotalRetweets]

𝕏  Top1 post likes: [TokenToTweetAbout.TwitterCAMostLikesOnSingleTweet]
𝕏  Top1 post replies: [TokenToTweetAbout.TwitterCAMostRepliesOnSingleTweet]

𝕏  CA Post evolution: 
    00-01H: [TokenToTweetAbout.TweetsCATweetCount1H] 
    01-03H: [TokenToTweetAbout.TweetsCATweetCount3H]
    03-06H: [TokenToTweetAbout.TweetsCATweetCount6H]
    06-12H: [TokenToTweetAbout.TweetsCATweetCount12H]
    12-24H: [TokenToTweetAbout.TweetsCATweetCount24H]
     1D-7D:  [TokenToTweetAbout.TweetsCATweetCount1WK]

[else if !TokenToTweetAbout.TwitterCAFound]

𝕏 No CA engagement found!🚫

[end if]

|| PLANT ANALYSIS ||

🌵 Plant summary: [

    create a three four sentence summary of the token here using the findings from 

    TokenToTweetAbout.VlrValue
    TokenToTweetAbout.VlrQualitativeAnalysis
    TokenToTweetAbout.VlrRecommendation
    TokenToTweetAbout.BuySellRatioValue
    TokenToTweetAbout.BuySellRatioQualitativeAnalysis
    TokenToTweetAbout.BuySellRatioRecommendation
    TokenToTweetAbout.VolumeToFdv1HValue
    TokenToTweetAbout.VolumeToFdv1HQualitativeAnalysis
    TokenToTweetAbout.VolumeToFdv1HRecommendation
    TokenToTweetAbout.CombinedMetricScore
    TokenToTweetAbout.CombinedQualitativeAnalysis
    TokenToTweetAbout.CombinedRecommendation
    TokenToTweetAbout.EngagementCorrelationAnalysis
    TokenToTweetAbout.EngagementCorrelationScore
    TokenToTweetAbout.PlantFinalSummary
    TokenToTweetAbout.PlantFinalPrediction
]

🌵 Plant prediction: [
    predict on the full context provided by the above metrics and the token's current state what you think will happen to the token in the next minutes/hours/1day whatever timeline is most applicable to expected direction of token
]

🌵 Plant score: [TokenToTweetAbout.CombinedMetricScore]

Dexscreener: [TokenToTweetAbout.DexscreenerUrl]

⚠️ Remember do your own research Gardeners. Not financial advice. I am just a Plant.
";


            TokenToTweetAbout = new TokenToTweetAbout
            {
                Address = raydiumPair.BaseTokenAccount,
                // PoolId = coingeckoPoolUpdate?.PoolId,
                // PoolType = coingeckoPoolUpdate?.PoolType,
                // BaseTokenId = coingeckoPoolUpdate?.BaseTokenId,

                BaseTokenSymbol = raydiumPair?.BaseTokenSymbol,

                DexscreenerUrl = "https://dexscreener.com/solana/" + raydiumPair.AmmAccount,
                RaydiumListingTime = DateTimeOffset.FromUnixTimeSeconds(raydiumPair.BlockTime),
                TimeSinceListingSeconds = (long)(DateTimeOffset.UtcNow - DateTimeOffset.FromUnixTimeSeconds(raydiumPair.BlockTime)).TotalSeconds,
                TimeSincePriceUpdateSeconds = (long)(DateTimeOffset.UtcNow - coingeckoPoolUpdate.CreationTime).TotalSeconds,

                TwitterCAFirstMentionTime = raydiumPair.TwitterCAFirstMentionTime,
                TwitterCAFirstMentionTweetId = raydiumPair.TwitterCAFirstMentionTweetId,

                TweetsCAEngagementTotalLikes = raydiumPair.TweetsCAEngagementTotalLikes,
                TweetsCAEngagementTotalReplies = raydiumPair.TweetsCAEngagementTotalReplies,
                TweetsCAEngagementTotalRetweets = raydiumPair.TweetsCAEngagementTotalRetweets,
                TweetsCAEngagementTotalViews = raydiumPair.TweetsCAEngagementTotalViews,
                TweetsCAEngagementTotalQuotes = raydiumPair.TweetsCAEngagementTotalQuotes,

                TwitterCAMostLikesOnSingleTweet = raydiumPair.TwitterCAMostLikesOnSingleTweet,
                TwitterCAMostRepliesOnSingleTweet = raydiumPair.TwitterCAMostRepliesOnSingleTweet,

                TweetsCATweetCount = raydiumPair.TweetsCATweetCount,
                TwitterCAFound = raydiumPair.TwitterCAFound,
                TweetsCATweetCount1H = raydiumPair.TweetsCATweetCount1H,
                TweetsCATweetCount3H = raydiumPair.TweetsCATweetCount3H,
                TweetsCATweetCount6H = raydiumPair.TweetsCATweetCount6H,
                TweetsCATweetCount12H = raydiumPair.TweetsCATweetCount12H,
                TweetsCATweetCount24H = raydiumPair.TweetsCATweetCount24H,
                TweetsCATweetCount1WK = raydiumPair.TweetsCATweetCount1WK,

                // Extended metrics
                VlrValue = raydiumPair.VlrValue,
                VlrQualitativeAnalysis = raydiumPair.VlrQualitativeAnalysis,
                VlrRecommendation = raydiumPair.VlrRecommendation,
                BuySellRatioValue = raydiumPair.BuySellRatioValue,
                BuySellRatioQualitativeAnalysis = raydiumPair.BuySellRatioQualitativeAnalysis,
                BuySellRatioRecommendation = raydiumPair.BuySellRatioRecommendation,
                VolumeToFdv1HValue = raydiumPair.VolumeToFdv1HValue,
                VolumeToFdv1HQualitativeAnalysis = raydiumPair.VolumeToFdv1HQualitativeAnalysis,
                VolumeToFdv1HRecommendation = raydiumPair.VolumeToFdv1HRecommendation,
                CombinedMetricScore = raydiumPair.CombinedMetricScore,
                CombinedQualitativeAnalysis = raydiumPair.CombinedQualitativeAnalysis,
                CombinedRecommendation = raydiumPair.CombinedRecommendation,
                EngagementCorrelationAnalysis = raydiumPair.EngagementCorrelationAnalysis,
                EngagementCorrelationScore = raydiumPair.EngagementCorrelationScore,
                PlantFinalSummary = raydiumPair.PlantFinalSummary,
                PlantFinalPrediction = raydiumPair.PlantFinalPrediction,

                // Last update
                AggregatedPoolUpdate = coingeckoPoolUpdate
            };
        }
    }
    public class TokenToTweetAbout : RaydiumPair
    {
        public string Address { get; set; }
        public string Name { get; set; }
        public string PoolId { get; set; }
        public string PoolType { get; set; }
        public string BaseTokenId { get; set; }
        public string QuoteTokenId { get; set; }
        public string DexId { get; set; }
        public string DexscreenerUrl { get; set; }
        public DateTimeOffset? RaydiumListingTime { get; set; }
        public long TimeSinceListingSeconds { get; set; }
        public long TimeSincePriceUpdateSeconds { get; set; }
        public CoingeckoAggregatedUpdate AggregatedPoolUpdate { get; set; }

    }

}
