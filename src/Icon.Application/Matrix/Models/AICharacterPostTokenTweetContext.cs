using System;
using System.Collections.Generic;
using Icon.Matrix.Models;

namespace Icon.Matrix.AIManager.CharacterPostTokenTweet
{

    // TokenToTweetAbout.VlrValue
    // TokenToTweetAbout.VlrQualitativeAnalysis
    // TokenToTweetAbout.VlrRecommendation

    // TokenToTweetAbout.BuySellRatioValue

    // TokenToTweetAbout.BuySellRatioQualitativeAnalysis
    // TokenToTweetAbout.BuySellRatioRecommendation
    // TokenToTweetAbout.VolumeToFdv1HValue
    // TokenToTweetAbout.VolumeToFdv1HQualitativeAnalysis
    // TokenToTweetAbout.VolumeToFdv1HRecommendation
    // TokenToTweetAbout.CombinedMetricScore
    // TokenToTweetAbout.CombinedQualitativeAnalysis
    // TokenToTweetAbout.CombinedRecommendation
    // TokenToTweetAbout.EngagementCorrelationAnalysis
    // TokenToTweetAbout.EngagementCorrelationScore
    // TokenToTweetAbout.PlantFinalSummary
    // TokenToTweetAbout.PlantFinalPrediction

    public class AICharacterPostTokenTweetContext
    {
        public string TweetExample { get; set; }
        public TokenToTweetAbout TokenToTweetAbout { get; set; }

        public AICharacterPostTokenTweetContext(
            RaydiumPair raydiumPair,
            CoingeckoAggregatedUpdate coingeckoPoolUpdate,
            List<TwitterImportTweetEngagement> first10Tweets,
            List<TwitterImportTweetEngagement> last20Tweets)
        {
            // 1) Map base tweet example 
            TweetExample = $@"
🌱 Rising Sprout: $[TokenToTweetAbout.BaseTokenSymbol] | [Potential Meme/AI/Celebriry/Political/Etc]

🕒 Raydium listing: [TokenToTweetAbout.TimeSinceListingText]
🪪 CA: [TokenToTweetAbout.Address]

--| AGGREGATED POOL METRICS |--

🕒 Pool update: [TokenToTweetAbout.TimeSincePriceUpdateText]
🏊‍♂️ Pool count: [TokenToTweetAbout.AggregatedPoolUpdate.Pools]

📊 Price change (5m): [TokenToTweetAbout.AggregatedPoolUpdate.PriceChangeM5]%
📊 Price change (1h): [TokenToTweetAbout.AggregatedPoolUpdate.PriceChangeH1]%

🌊 Volume (1hr): [TokenToTweetAbout.AggregatedPoolUpdate.VolumeH1]
🍃 Liquidity: [TokenToTweetAbout.AggregatedPoolUpdate.ReserveInUsd]
🌍 FDV: [TokenToTweetAbout.AggregatedPoolUpdate.FdvUsd]

📤 Buys / Sells (5m): [TokenToTweetAbout.AggregatedPoolUpdate.M5Buys] / [TokenToTweetAbout.AggregatedPoolUpdate.M5Sells]
🤝 Buyers / Sellers (5m): [TokenToTweetAbout.AggregatedPoolUpdate.M5Buyers] / [TokenToTweetAbout.AggregatedPoolUpdate.M5Sellers]

--| ENGAGEMENT METRICS |--

[if TokenToTweetAbout.TwitterCAFound]

𝕏  CA first mention: [TokenToTweetAbout.TwitterCAFirstMentionTime] minutes / hours /days ago | or not found

𝕏  CA total posts: [TokenToTweetAbout.TwitterCAFirstMentionTime] 
𝕏  CA total likes: [TokenToTweetAbout.TweetsCAEngagementTotalLikes]
𝕏  CA total replies: [TokenToTweetAbout.TweetsCAEngagementTotalRetweets]
𝕏  CA unique authors: [TokenToTweetAbout.TweetsCAEngagementTotalRetweets]

𝕏  Top1 post likes: [TokenToTweetAbout.TwitterCAMostLikesOnSingleTweet]
𝕏  Top1 post replies: [TokenToTweetAbout.TwitterCAMostRepliesOnSingleTweet]

𝕏  CA Post evolution:
    00-01H: [TokenToTweetAbout.TweetsCATweetCount1H] 
    01-03H: [TokenToTweetAbout.TweetsCATweetCount3H]
    03-06H: [TokenToTweetAbout.TweetsCATweetCount6H]
    06-12H: [TokenToTweetAbout.TweetsCATweetCount12H]
    12-24H: [TokenToTweetAbout.TweetsCATweetCount24H]
     1D-7D: [TokenToTweetAbout.TweetsCATweetCount1WK]

[else if !TokenToTweetAbout.TwitterCAFound]

𝕏 No CA engagement found!🚫

[end if]

--| PLANT ANALYSIS |--

🌵 Plant engagement: [

    create a three four sentence anlaysis of the engagment, is there a sharp increase in engagement, is the token being shilled, is there a lot of hype around it, etc.
    determine the likelyhood of the tweets being organic, bots or paid for, is the token being shilled, is there a lot of hype around it, etc.
    Use the following metrics to create the analysis:

    TokenToTweetAbout.TweetsCATweetCount1WK
    TokenToTweetAbout.TweetsCATweetCount24H
    TokenToTweetAbout.TweetsCATweetCount12H
    TokenToTweetAbout.TweetsCATweetCount6H
    TokenToTweetAbout.TweetsCATweetCount3H
    TokenToTweetAbout.TweetsCATweetCount1H
    TokenToTweetAbout.TweetsCAEngagementUniqueAuthors
    TokenToTweetAbout.TweetsCAEngagementTotalLikes
    TokenToTweetAbout.TweetsCAEngagementTotalReplies
    TokenToTweetAbout.TweetsCAEngagementTotalRetweets
    TokenToTweetAbout.TweetsCAEngagementTotalViews
    TokenToTweetAbout.TweetsCAEngagementTotalQuotes
    TokenToTweetAbout.TwitterCAMostLikesOnSingleTweet
    TokenToTweetAbout.TwitterCAMostRepliesOnSingleTweet
    TokenToTweetAbout.TwitterCAFirstMentionTime
    TokenToTweetAbout.First10Tweets
    TokenToTweetAbout.Last20Tweets

]
🌵 Plant summary: [

    create a three four sentence summary of the token here using the findings from:

    TokenToTweetAbout.VlrValue  
    TokenToTweetAbout.BuySellRatioValue    
    TokenToTweetAbout.VolumeToFdv1HValue    
    TokenToTweetAbout.EngagementCorrelationScore

    TokenToTweetAbout.TweetsCATweetCount1WK
    TokenToTweetAbout.TweetsCATweetCount24H
    TokenToTweetAbout.TweetsCATweetCount12H
    TokenToTweetAbout.TweetsCATweetCount6H
    TokenToTweetAbout.TweetsCATweetCount3H
    TokenToTweetAbout.TweetsCATweetCount1H
    TokenToTweetAbout.TweetsCAEngagementUniqueAuthors

    Summary target audience: Degen traders, meme traders, moonboys, etc.
    Summary & prediction guidelines: 
        Mostly meme tokens, trading is done not on real value but on hype and speculation. 99,9% of the time the token will go to 0, the questions is when.
        The summary should provide an insight into the token's current state and what the future might hold for it.
        If the first CA address tweet is less then 3h ago, only mentioned a couple of times in that window (1-20posts) and the liquidity is less then 300K, and if the token is less then 30 minutes old, it will very likely go to 0 in the next few minutes
        If there is no contract address at token listing and the contract address is after listing time, and the token has more then 200K liquidity, it will likely go up in the next few minutes
        If there is no contract address found and the token is younger then 30 minutes, and the liquidity is around 200K or less it will likely go to 0 in the next few minutes
        Analyse the tokens tweets versus the price action is there a positive or negative correlation between the two, if the token is being shilled and the price is going down, it will likely go to 0 in the next few minutes, if the tweets are recent and the price is going up then it will likely go up in the next few minutes
]

🌵 Plant prediction: [
    predict on the full context provided what you think will happen to the token in the next minutes/hours/1day whatever timeline is most applicable to expected direction of token.
]

🌵 Plant score: [TokenToTweetAbout.CombinedMetricScore]

Dexscreener: [TokenToTweetAbout.DexscreenerUrl]

⚠️ Remember do your own research Gardeners. Not financial advice. I am just a Plant.

[
    AI INSTRUCTIONS: 
    Show prices in USD
    Show prices bigger then 10 without decimals
    Use for prices bigger then 1000 the K suffix, for prices bigger then 1M the M suffix and for prices bigger then 1B the B suffix    
    Use only one decimal for the plant score
]

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

                TimeSinceListingText = TimeSince(DateTimeOffset.FromUnixTimeSeconds(raydiumPair.BlockTime)),
                TimeSincePriceUpdateText = TimeSince(coingeckoPoolUpdate.CreationTime),

                TwitterCAFirstMentionTime = raydiumPair.TwitterCAFirstMentionTime,
                TwitterCAFirstMentionTweetId = raydiumPair.TwitterCAFirstMentionTweetId,

                TweetsCAEngagementUniqueAuthors = raydiumPair.TweetsCAEngagementUniqueAuthors,
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
                AggregatedPoolUpdate = coingeckoPoolUpdate,
                First10Tweets = first10Tweets,
                Last20Tweets = last20Tweets
            };
        }


        public string TimeSince(DateTimeOffset time)
        {
            var timeSpan = DateTimeOffset.UtcNow - time;

            if (timeSpan.TotalSeconds < 60)
            {
                return $"{(int)timeSpan.TotalSeconds} seconds ago";
            }
            else if (timeSpan.TotalMinutes < 60)
            {
                return $"{(int)timeSpan.TotalMinutes} minutes ago";
            }
            else if (timeSpan.TotalHours < 24)
            {
                int hours = (int)timeSpan.TotalHours;
                int minutes = timeSpan.Minutes;
                return $"{hours} hour{(hours > 1 ? "s" : string.Empty)} and {minutes} minute{(minutes > 1 ? "s" : string.Empty)} ago";
            }
            else
            {
                int days = (int)timeSpan.TotalDays;
                int hours = timeSpan.Hours;
                return $"{days} day{(days > 1 ? "s" : string.Empty)} and {hours} hour{(hours > 1 ? "s" : string.Empty)} ago";
            }
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
        // public long TimeSinceListingSeconds { get; set; }
        // public long TimeSincePriceUpdateSeconds { get; set; }
        public string TimeSinceListingText { get; set; }
        public string TimeSincePriceUpdateText { get; set; }
        public CoingeckoAggregatedUpdate AggregatedPoolUpdate { get; set; }
        public List<TwitterImportTweetEngagement> First10Tweets { get; set; }
        public List<TwitterImportTweetEngagement> Last20Tweets { get; set; }

        public string TimeSince(DateTimeOffset time)
        {
            var timeSpan = DateTimeOffset.UtcNow - time;

            if (timeSpan.TotalSeconds < 60)
            {
                return $"{(int)timeSpan.TotalSeconds} seconds ago";
            }
            else if (timeSpan.TotalMinutes < 60)
            {
                return $"{(int)timeSpan.TotalMinutes} minutes ago";
            }
            else if (timeSpan.TotalHours < 24)
            {
                int hours = (int)timeSpan.TotalHours;
                int minutes = timeSpan.Minutes;
                return $"{hours} hour{(hours > 1 ? "s" : string.Empty)} and {minutes} minute{(minutes > 1 ? "s" : string.Empty)} ago";
            }
            else
            {
                int days = (int)timeSpan.TotalDays;
                int hours = timeSpan.Hours;
                return $"{days} day{(days > 1 ? "s" : string.Empty)} and {hours} hour{(hours > 1 ? "s" : string.Empty)} ago";
            }
        }


    }

}
