// using System;
// using Icon.Matrix.Models;

// namespace Icon.Matrix.AIManager.CharacterPostTokenTweet
// {
//     public class AICharacterPostTokenTweetContextxx
//     {
//         public string TweetExample { get; set; }
//         public TokenToTweetAbout TokenToTweetAbout { get; set; }

//         public AICharacterPostTokenTweetContext(
//             RaydiumPair raydiumPair,
//             CoingeckoPoolUpdate coingeckoPoolUpdate)
//         {
//             // 1) Set up your base TweetExample
//             TweetExample = $@"
// 🌱 Rising Sprout: $[TokenToTweetAbout.BaseTokenSymbol] | [Determine type of token is Meme, AI, AI Agent, Political, Celebrity, or other]
// CA: [TokenToTweetAbout.Address]

// 🕒 Raydium listing: [TokenToTweetAbout.TimeSinceListingSeconds] [in minutes round to minute (or say less then a minute if applicable)]
// 📊 Price change (5m): [TokenToTweetAbout.LastTokenUpdate.PriceChangeM5]%
// 📊 Price change (1h): [TokenToTweetAbout.LastTokenUpdate.PriceChangeM5]%

// 🌊 Volume (1hr): [TokenToTweetAbout.LastTokenUpdate.VolumeH1]
// 🍃 Liquidity: [TokenToTweetAbout.LastTokenUpdate.ReserveInUsd]
// 🌍 FDV: [TokenToTweetAbout.LastTokenUpdate.FdvUsd]

// 📤 Buys / Sells (5m): [TokenToTweetAbout.LastTokenUpdate.M5Buys] / [TokenToTweetAbout.LastTokenUpdate.M5Sells]
// 🤝 Buyers / Sellers (5m): [TokenToTweetAbout.LastTokenUpdate.M5Buyers] / [TokenToTweetAbout.LastTokenUpdate.M5Sellers] 

// [if TokenToTweetAbout.TwitterCAFound]

// 𝕏  CA first mention: [TokenToTweetAbout.TwitterCAFirstMentionTime] minutes / hours /days ago | or not found

// 𝕏  CA total posts: [TokenToTweetAbout.TwitterCAFirstMentionTime] 
// 𝕏  CA total likes: [TokenToTweetAbout.TweetsCAEngagementTotalLikes]
// 𝕏  CA total replies: [TokenToTweetAbout.TweetsCAEngagementTotalRetweets]

// 𝕏  Top1 post likes: [TokenToTweetAbout.TwitterCAMostLikesOnSingleTweet]
// 𝕏  Top1 post replies: [TokenToTweetAbout.TwitterCAMostRepliesOnSingleTweet]

// 𝕏  Post evolution: 
//     This hour: [TokenToTweetAbout.TweetsCATweetCount1H] 
//     Last 3H: [TokenToTweetAbout.TweetsCATweetCount3H]
//     Last 6H: [TokenToTweetAbout.TweetsCATweetCount6H]
//     Last 12H: [TokenToTweetAbout.TweetsCATweetCount12H]
//     Last 24H: [TokenToTweetAbout.TweetsCATweetCount24H]
//     Last Week: [TokenToTweetAbout.TweetsCATweetCount1WK]

// [else if !TokenToTweetAbout.TwitterCAFound]

// 𝕏 No CA engagment found!🚫

// [end if]

// 🌵 Plant summary: [Write a two / three sentence analysis for a degen memecoin trader, make sure to say something valuable, like an interesting correlation] 

// 🌵 Plant prediction: [Will be appended below]

// 🌵 Plant score: [TokenToTweetAbout.CombinedScore]

// Dexscreener: [TokenToTweetAbout.DexscreenerUrl]

// ⚠️ Remember do your own research Gardeners. Not financial advice. I am just a Plant.

// -- Instruction to AI: 

// Prices are in USD, where applicable show dollar sign and use K, M, B for large numbers dont use decimals for numbers bigger then 1
// EXAMPLE (not final tweet, just a guide for you to craft your own).
// ";

//             // 2) Populate TokenToTweetAbout from the arguments
//             TokenToTweetAbout = new TokenToTweetAbout
//             {
//                 Address = coingeckoPoolUpdate?.Address,
//                 Name = coingeckoPoolUpdate?.Name,
//                 PoolId = coingeckoPoolUpdate?.PoolId,
//                 PoolType = coingeckoPoolUpdate?.PoolType,
//                 BaseTokenId = coingeckoPoolUpdate?.BaseTokenId,
//                 BaseTokenSymbol = raydiumPair?.BaseTokenSymbol,

//                 QuoteTokenId = coingeckoPoolUpdate?.QuoteTokenId,
//                 DexId = coingeckoPoolUpdate?.DexId,
//                 DexscreenerUrl = "https://dexscreener.com/solana/" + raydiumPair.AmmAccount,
//                 RaydiumListingTime = DateTimeOffset.FromUnixTimeSeconds(raydiumPair.BlockTime),
//                 TimeSinceListingSeconds = (DateTimeOffset.UtcNow - DateTimeOffset.FromUnixTimeSeconds(raydiumPair.BlockTime)).Seconds,

//                 TwitterCAFirstMentionTime = raydiumPair.TwitterCAFirstMentionTime,
//                 TwitterCAFirstMentionTweetId = raydiumPair.TwitterCAFirstMentionTweetId,

//                 TweetsCAEngagementTotalLikes = raydiumPair.TweetsCAEngagementTotalLikes,
//                 TweetsCAEngagementTotalReplies = raydiumPair.TweetsCAEngagementTotalReplies,
//                 TweetsCAEngagementTotalRetweets = raydiumPair.TweetsCAEngagementTotalRetweets,
//                 TweetsCAEngagementTotalViews = raydiumPair.TweetsCAEngagementTotalViews,
//                 TweetsCAEngagementTotalQuotes = raydiumPair.TweetsCAEngagementTotalQuotes,

//                 TwitterCAMostLikesOnSingleTweet = raydiumPair.TwitterCAMostLikesOnSingleTweet,
//                 TwitterCAMostRepliesOnSingleTweet = raydiumPair.TwitterCAMostRepliesOnSingleTweet,

//                 TweetsCATweetCount = raydiumPair.TweetsCATweetCount,
//                 TwitterCAFound = raydiumPair.TwitterCAFound,
//                 TokenCombinedScore = raydiumPair.TokenCombinedScore,

//                 TweetsCATweetCount1H = raydiumPair.TweetsCATweetCount1H,
//                 TweetsCATweetCount3H = raydiumPair.TweetsCATweetCount3H,
//                 TweetsCATweetCount6H = raydiumPair.TweetsCATweetCount6H,
//                 TweetsCATweetCount12H = raydiumPair.TweetsCATweetCount12H,
//                 TweetsCATweetCount24H = raydiumPair.TweetsCATweetCount24H,
//                 TweetsCATweetCount1WK = raydiumPair.TweetsCATweetCount1WK,

//                 VlrValue = raydiumPair.VlrValue,
//                 VlrQualitativeAnalysis = raydiumPair.VlrQualitativeAnalysis,
//                 VlrRecommendation = raydiumPair.VlrRecommendation,
//                 BuySellRatioValue = raydiumPair.BuySellRatioValue,
//                 BuySellRatioQualitativeAnalysis = raydiumPair.BuySellRatioQualitativeAnalysis,
//                 BuySellRatioRecommendation = raydiumPair.BuySellRatioRecommendation,
//                 VolumeToFdv1HValue = raydiumPair.VolumeToFdv1HValue,
//                 VolumeToFdv1HQualitativeAnalysis = raydiumPair.VolumeToFdv1HQualitativeAnalysis,
//                 VolumeToFdv1HRecommendation = raydiumPair.VolumeToFdv1HRecommendation,
//                 CombinedMetricScore = raydiumPair.CombinedMetricScore,
//                 CombinedQualitativeAnalysis = raydiumPair.CombinedQualitativeAnalysis,
//                 CombinedRecommendation = raydiumPair.CombinedRecommendation,


//                 // Last Token Update
//                 LastTokenUpdate = coingeckoPoolUpdate

//             };

//             // 3) Append the final instruction block to the tweet
//             AppendPlantPredictionInstruction();
//         }

//         /// <summary>
//         /// Appends the bracketed instruction under “🌵 Plant prediction”.
//         /// </summary>
//         private void AppendPlantPredictionInstruction()
//         {
//             var scenario = CalculatePredictionScenario();
//             string scenarioCheatSheet = GetScenarioDefinitionsAndDataPoints();

//             // This text merges new liquidity-vs-FDV logic into the final instructions
//             string plantPredictionInstruction = $@"
// 🌵 Plant prediction: [
// Instruction to LLM:
// Below is scenario data from this token:
// {scenario.OverallScenario}

// === SCENARIO DEFINITIONS & TYPICAL OUTCOMES ===
// {scenarioCheatSheet}

// Task:
// Use both the scenario data and the cheat sheet to produce a short (1–3 sentence) prediction 
// for degen traders on how quickly this might pump or dump, referencing:
//  - FDV vs. Liquidity mismatch and potential illusions/manipulations
//  - Timing of Twitter engagement (if any)
//  - Buy/Sell pressure in the first minutes
//  - Big pre-listing vs. post-listing hype correlation
// Provide a concise summary of likely short-term (minutes/hours) and possible medium-term (days) action. 
// ]";

//             TweetExample += plantPredictionInstruction;
//         }

//         /// <summary>
//         /// Main scenario classification logic. We add a check for liquidity vs FDV mismatch 
//         /// inside the correlation outcome method.
//         /// </summary>
//         private PredictionScenario CalculatePredictionScenario()
//         {
//             var scenario = new PredictionScenario();

//             // 1) FDV Bucket
//             float fdv = TokenToTweetAbout.LastTokenUpdate.FdvUsd ?? 0f;
//             scenario.FdvBucket = GetFdvBucket(fdv);

//             // 2) Time since listing (in minutes)
//             double minutesSinceListing = TokenToTweetAbout.TimeSinceListingSeconds / 60.0;
//             scenario.TimeSinceListingBucket = GetTimeSinceListingBucket(minutesSinceListing);

//             // 3) Short-term buys vs. sells
//             scenario.ShortTermBuysSellsIndicator = GetShortTermBuysSellsIndicator(
//                 TokenToTweetAbout.LastTokenUpdate.M5Buys,
//                 TokenToTweetAbout.LastTokenUpdate.M5Sells
//             );

//             // 4) Liquidity & Volume data
//             scenario.LiquidityVolumeIndicator = GetLiquidityVolumeIndicator(
//                 TokenToTweetAbout.LastTokenUpdate.VolumeH1,
//                 TokenToTweetAbout.LastTokenUpdate.ReserveInUsd
//             );

//             // 5) Price changes
//             scenario.PriceChangeIndicator = GetPriceChangeIndicator(
//                 TokenToTweetAbout.LastTokenUpdate.PriceChangeM5,
//                 TokenToTweetAbout.LastTokenUpdate.PriceChangeH1
//             );

//             // 6) Twitter Engagement
//             scenario.TwitterEngagementType = GetTwitterEngagementType(TokenToTweetAbout);

//             // 7) Build a correlation outcome (the “best of the best” trader logic)
//             float liquidity = TokenToTweetAbout.LastTokenUpdate.ReserveInUsd ?? 0f;
//             var correlationOutcome = GetDegenCorrelationOutcome(
//                 fdv,
//                 liquidity,
//                 minutesSinceListing,
//                 scenario.ShortTermBuysSellsIndicator,
//                 scenario.TwitterEngagementType
//             );

//             scenario.OverallScenario =
//                 $"FDV Bucket: {scenario.FdvBucket}, " +
//                 $"Time Bucket: {scenario.TimeSinceListingBucket}, " +
//                 $"Buys/Sells(5m): {scenario.ShortTermBuysSellsIndicator}, " +
//                 $"Liquidity/Volume(1h): {scenario.LiquidityVolumeIndicator}, " +
//                 $"Price Change(5m,1h): {scenario.PriceChangeIndicator}, " +
//                 $"Twitter Eng.: {scenario.TwitterEngagementType}\n" +
//                 $"Correlation: {correlationOutcome}";

//             return scenario;
//         }

//         #region Sub-Methods for Scenario Logic

//         /// <summary>
//         /// This method is the “heart” of your advanced correlation logic. 
//         /// We incorporate liquidity vs. FDV mismatch, buy/sell data, and 
//         /// timing of engagement to produce an outcome statement.
//         /// </summary>
//         private string GetDegenCorrelationOutcome(
//             float fdv,
//             float liquidity,
//             double minutesSinceListing,
//             string shortTermBuySellIndicator,
//             string twitterEngagementType
//         )
//         {
//             bool isHighFDV = fdv > 500_000_000; // e.g. “hundreds of millions or more”
//             bool isLargeLiquidity = liquidity > 200_000;

//             // Check mismatch
//             string mismatchNote = "";
//             if (!isLargeLiquidity && isHighFDV)
//             {
//                 mismatchNote = "Huge FDV with relatively small liquidity—suggests potential inflated valuation or manipulation. ";
//             }
//             else if (isLargeLiquidity && isHighFDV)
//             {
//                 mismatchNote = "High FDV and strong liquidity—rare but can still be extremely volatile. ";
//             }
//             else if (!isLargeLiquidity && fdv > 1_000_000)
//             {
//                 mismatchNote = "FDV over a million, but liquidity is modest—price swings may be dramatic. ";
//             }
//             // else we don’t mention mismatch

//             // Twitter logic
//             bool noTwitter = twitterEngagementType.Contains("No Twitter", StringComparison.OrdinalIgnoreCase);
//             bool strongPostListing = twitterEngagementType.Contains("Strong post-listing", StringComparison.OrdinalIgnoreCase);

//             // Buys/sells logic
//             bool highBuyPressure = shortTermBuySellIndicator.Contains("High buy", StringComparison.OrdinalIgnoreCase);
//             bool highSellPressure = shortTermBuySellIndicator.Contains("High sell", StringComparison.OrdinalIgnoreCase);

//             // We combine each dimension into a short narrative
//             string outcome = mismatchNote;

//             // If no Twitter + massive FDV
//             if (noTwitter && isHighFDV)
//             {
//                 outcome += "No social hype, yet FDV is enormous. This could be stealth whales or pure speculation. ";
//             }
//             // If strong post-listing tweets
//             else if (strongPostListing && isHighFDV)
//             {
//                 outcome += "Big FDV plus post-listing hype can mimic major political/celebrity tokens. Potentially big swings. ";
//             }
//             // Otherwise, minimal mention
//             else if (noTwitter)
//             {
//                 outcome += "No Twitter engagement—often leads to a quick fade unless whales keep buying. ";
//             }
//             else if (strongPostListing)
//             {
//                 outcome += "Post-listing hype can trigger second-wave FOMO. ";
//             }

//             // Now factor in buy/sell pressure & listing time
//             if (highBuyPressure)
//             {
//                 outcome += "High early buy pressure can spike prices quickly—ideal for immediate flips. ";
//             }
//             else if (highSellPressure)
//             {
//                 outcome += "High sell pressure signals a near-instant dump or rug risk. ";
//             }

//             if (minutesSinceListing < 5)
//             {
//                 outcome += "Listed under 5 minutes ago, hyper-volatile—watch for whiplash changes. ";
//             }

//             // If everything is balanced
//             if (string.IsNullOrWhiteSpace(outcome))
//             {
//                 outcome = "No extreme signals; watch for typical meme volatility. ";
//             }

//             return outcome.Trim();
//         }

//         private string GetFdvBucket(float fdv)
//         {
//             if (fdv < 50_000) return "<50k (tiny microcap)";
//             if (fdv < 150_000) return "50k–150k (microcap)";
//             if (fdv < 1_000_000) return "150k–1M (smallcap)";
//             if (fdv < 10_000_000) return "1M–10M (midcap)";
//             if (fdv < 500_000_000) return "10M–500M (largecap)";
//             return ">500M (very large cap)";
//         }

//         private string GetTimeSinceListingBucket(double minutes)
//         {
//             if (minutes < 5) return "<5 min (just launched)";
//             if (minutes < 30) return "5–30 min (very early)";
//             if (minutes < 60) return "30–60 min (early)";
//             return ">60 min (over an hour)";
//         }

//         private string GetShortTermBuysSellsIndicator(int? m5Buys, int? m5Sells)
//         {
//             double buys = m5Buys ?? 0;
//             double sells = m5Sells ?? 0;
//             double ratio = (buys + 1) / (sells + 1); // avoid div by zero

//             if (ratio >= 2.0) return "High buy pressure";
//             if (ratio > 1.0) return "Moderate buy pressure";
//             if (ratio < 0.5) return "High sell pressure";
//             return "Balanced/mixed";
//         }

//         private string GetLiquidityVolumeIndicator(float? volume1h, float? liquidityUsd)
//         {
//             float vol = volume1h ?? 0f;
//             float liq = liquidityUsd ?? 0f;

//             if (liq <= 0) return "No/low liquidity data";

//             double ratio = vol / liq;
//             if (ratio >= 3.0) return $"Vol/Liq={ratio:F1} (very high vol)";
//             if (ratio >= 1.0) return $"Vol/Liq={ratio:F1} (moderate vol)";
//             return $"Vol/Liq={ratio:F1} (low vol)";
//         }

//         private string GetPriceChangeIndicator(float? priceChangeM5, float? priceChangeH1)
//         {
//             float p5 = priceChangeM5 ?? 0f;
//             float p60 = priceChangeH1 ?? 0f;

//             // Classify 5m
//             string shortTerm;
//             if (p5 > 50) shortTerm = "Huge spike (5m)";
//             else if (p5 > 0) shortTerm = "Up (5m)";
//             else if (p5 < -20) shortTerm = "Down big (5m)";
//             else shortTerm = "Flat (5m)";

//             // Classify 1h
//             string hourTerm;
//             if (p60 > 100) hourTerm = "Massive pump (1h)";
//             else if (p60 > 0) hourTerm = "Up (1h)";
//             else if (p60 < -20) hourTerm = "Down big (1h)";
//             else hourTerm = "Flat (1h)";

//             return $"{shortTerm}, {hourTerm}";
//         }

//         private string GetTwitterEngagementType(TokenToTweetAbout token)
//         {
//             if (!token.TwitterCAFound)
//                 return "No Twitter found";

//             int tweets1h = token.TweetsCATweetCount1H;
//             int tweets24h = token.TweetsCATweetCount24H;
//             double ratio = (tweets1h + 1.0) / (tweets24h + 1.0);

//             if (ratio > 0.5) return "Strong post-listing tweets";
//             return "Moderate or earlier tweets";
//         }

//         /// <summary>
//         /// Additional cheat-sheet with enhanced FDV vs. liquidity notes.
//         /// </summary>
//         private string GetScenarioDefinitionsAndDataPoints()
//         {
//             return @"
// 1. FDV vs Liquidity:
//    - High FDV but low liquidity often means a tiny circulating supply at a high price, 
//      inflating the perceived valuation. Volatility can be extreme. 
//    - A more balanced liquidity relative to FDV might indicate deeper pools, 
//      but in meme territory, manipulation is still possible.

// 2. FDV Ranges:
//    - <50k: Extremely low FDV, often a tiny microcap. Quick pump or rug in minutes.
//    - 50k–150k: Microcap meme territory, can vanish quickly.
//    - 150k–1M: Still “meme-grade” but slightly more traction.
//    - 1M–10M: Midcap; can survive days/weeks but remains volatile.
//    - 10M–500M: Largecap; more potential for multi-day mania if hype is consistent.
//    - >500M: Very large cap; possibly mainstream hype or big investor backing.

// 3. Time Since Listing (Raydium):
//    - <5 minutes: Super fresh; big pre-hype can pump/dump instantly.
//    - 5–30 minutes: Early trades define trend (accumulation vs. quick exit).
//    - 30–60 minutes: Potential partial stabilization or next wave of hype/dump.
//    - >60 minutes: If no traction by now, typically fizzles out (unless big news).

// 4. Short-Term Buys vs. Sells (M5, M15):
//    - More buys = possible immediate pump.
//    - More sells = fade or rug risk.
//    - Balanced = uncertain short-term direction.

// 5. Price Change (M5, H1):
//    - Big % spikes can crash fast if liquidity is low.
//    - Moderate gains more sustainable, but still at risk in meme territory.
//    - Negative % early on often signals a whale dump or weak interest.

// 6. Twitter Engagement:
//    - No Twitter: No hype, typically quick fade unless whales quietly speculate.
//    - Pre-listing hype: Possibly orchestrated pump/dump at T0.
//    - Post-listing hype: Can trigger second wave if price didn’t already collapse.
//    - Fake/bot activity: High retweets, suspicious patterns → watch for manipulation.

// 7. Medium/Long-Term Survival:
//    - Hours: If volume stays high, can keep pumping or sideways. 
//    - Days/Weeks: Needs real brand, influencer push, or consistent speculation 
//      (like a high-profile political or AI-themed coin).
//    - Beware of imposter tokens using known brand names (Trump, Ivanka, etc.) 
//      with inflated FDV but minimal real backing.
// ";
//         }

//         #endregion
//     }


//     //==========================
//     // Model Classes
//     //==========================

//     public class PredictionScenario
//     {
//         public string FdvBucket { get; set; }
//         public string TimeSinceListingBucket { get; set; }
//         public string ShortTermBuysSellsIndicator { get; set; }
//         public string LiquidityVolumeIndicator { get; set; }
//         public string PriceChangeIndicator { get; set; }
//         public string TwitterEngagementType { get; set; }

//         public string OverallScenario { get; set; }
//     }

//     public class TokenToTweetAbout : RaydiumPair
//     {
//         public string Address { get; set; }
//         public string Name { get; set; }
//         public string PoolId { get; set; }
//         public string PoolType { get; set; }
//         public string BaseTokenId { get; set; }
//         public string QuoteTokenId { get; set; }
//         public string DexId { get; set; }
//         public string DexscreenerUrl { get; set; }
//         public DateTimeOffset? RaydiumListingTime { get; set; }
//         public long TimeSinceListingSeconds { get; set; }
//         public CoingeckoPoolUpdate LastTokenUpdate { get; set; }

//     }
// }