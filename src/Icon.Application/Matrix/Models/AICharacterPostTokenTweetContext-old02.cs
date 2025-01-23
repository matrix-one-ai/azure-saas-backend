// using System;
// using Icon.Matrix.Models;

// namespace Icon.Matrix.AIManager.CharacterPostTokenTweet
// {
//     public class AICharacterPostTokenTweetContext
//     {
//         public string TweetExample { get; set; }
//         public TokenToTweetAbout TokenToTweetAbout { get; set; }

//         public AICharacterPostTokenTweetContext(
//             RaydiumPair raydiumPair,
//             CoingeckoPoolUpdate coingeckoPoolUpdate)
//         {
//             // 1) Build out the initial TweetExample with placeholders
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
//     One Week: [TokenToTweetAbout.TweetsCATweetCount1WK]

// [else if !TokenToTweetAbout.TwitterCAFound]

// 𝕏 No CA engagment found!🚫

// [end if]

// 🌵 Plant score: [TokenToTweetAbout.CombinedScore] 
// 🌵 Plant summary: [Write a two / three sentence analysis for a degen memecoin trader, make sure to say something valuable, like an interesting correlation] 
// 🌵 Plant prediction: [Will be appended below]

// Dexscreener: [TokenToTweetAbout.DexscreenerUrl]

// ⚠️ Remember do your own research Gardeners. Not financial advice. I am just a Plant.

// -- Instruction to AI: 

// Prices are in USD, where applicable show dollar sign and use K, M, B for large numbers dont use decimals for numbers bigger then 1
// EXAMPLE (not final tweet, just a guide for you to craft your own).
// ";

//             // 2) Populate TokenToTweetAbout with relevant data
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
//                 TokenPriceChange24NormScore = raydiumPair.TokenPriceChange24NormScore,
//                 TokenLiquidityNormScore = raydiumPair.TokenLiquidityNormScore,
//                 TokenTweetCountNormScore = raydiumPair.TokenTweetCountNormScore,
//                 TokenLikeNormScore = raydiumPair.TokenLikeNormScore,
//                 TokenRetweetNormScore = raydiumPair.TokenRetweetNormScore,

//                 TweetsCATweetCount1H = raydiumPair.TweetsCATweetCount1H,
//                 TweetsCATweetCount3H = raydiumPair.TweetsCATweetCount3H,
//                 TweetsCATweetCount6H = raydiumPair.TweetsCATweetCount6H,
//                 TweetsCATweetCount12H = raydiumPair.TweetsCATweetCount12H,
//                 TweetsCATweetCount24H = raydiumPair.TweetsCATweetCount24H,
//                 TweetsCATweetCount1WK = raydiumPair.TweetsCATweetCount1WK,

//                 // Last Token Update
//                 LastTokenUpdate = new LastTokenUpdate
//                 {
//                     PriceUpdateTime = coingeckoPoolUpdate?.CreationTime ?? DateTimeOffset.UtcNow,
//                     BaseTokenPriceUsd = coingeckoPoolUpdate?.BaseTokenPriceUsd,
//                     BaseTokenPriceNativeCurrency = coingeckoPoolUpdate?.BaseTokenPriceNativeCurrency,
//                     QuoteTokenPriceUsd = coingeckoPoolUpdate?.QuoteTokenPriceUsd,
//                     QuoteTokenPriceNativeCurrency = coingeckoPoolUpdate?.QuoteTokenPriceNativeCurrency,
//                     BaseTokenPriceQuoteToken = coingeckoPoolUpdate?.BaseTokenPriceQuoteToken,
//                     QuoteTokenPriceBaseToken = coingeckoPoolUpdate?.QuoteTokenPriceBaseToken,
//                     TokenPriceUsd = coingeckoPoolUpdate?.TokenPriceUsd,
//                     FdvUsd = coingeckoPoolUpdate?.FdvUsd,
//                     MarketCapUsd = coingeckoPoolUpdate?.MarketCapUsd,
//                     PriceChangeM5 = coingeckoPoolUpdate?.PriceChangeM5,
//                     PriceChangeH1 = coingeckoPoolUpdate?.PriceChangeH1,
//                     PriceChangeH6 = coingeckoPoolUpdate?.PriceChangeH6,
//                     PriceChangeH24 = coingeckoPoolUpdate?.PriceChangeH24,
//                     M5Buys = coingeckoPoolUpdate?.M5Buys,
//                     M5Sells = coingeckoPoolUpdate?.M5Sells,
//                     M5Buyers = coingeckoPoolUpdate?.M5Buyers,
//                     M5Sellers = coingeckoPoolUpdate?.M5Sellers,
//                     M15Buys = coingeckoPoolUpdate?.M15Buys,
//                     M15Sells = coingeckoPoolUpdate?.M15Sells,
//                     M15Buyers = coingeckoPoolUpdate?.M15Buyers,
//                     M15Sellers = coingeckoPoolUpdate?.M15Sellers,
//                     M30Buys = coingeckoPoolUpdate?.M30Buys,
//                     M30Sells = coingeckoPoolUpdate?.M30Sells,
//                     M30Buyers = coingeckoPoolUpdate?.M30Buyers,
//                     M30Sellers = coingeckoPoolUpdate?.M30Sellers,
//                     H1Buys = coingeckoPoolUpdate?.H1Buys,
//                     H1Sells = coingeckoPoolUpdate?.H1Sells,
//                     H1Buyers = coingeckoPoolUpdate?.H1Buyers,
//                     H1Sellers = coingeckoPoolUpdate?.H1Sellers,
//                     H24Buys = coingeckoPoolUpdate?.H24Buys,
//                     H24Sells = coingeckoPoolUpdate?.H24Sells,
//                     H24Buyers = coingeckoPoolUpdate?.H24Buyers,
//                     H24Sellers = coingeckoPoolUpdate?.H24Sellers,
//                     VolumeM5 = coingeckoPoolUpdate?.VolumeM5,
//                     VolumeH1 = coingeckoPoolUpdate?.VolumeH1,
//                     VolumeH6 = coingeckoPoolUpdate?.VolumeH6,
//                     VolumeH24 = coingeckoPoolUpdate?.VolumeH24,
//                     ReserveInUsd = coingeckoPoolUpdate?.ReserveInUsd
//                 }
//             };

//             // 3) Append the custom “Plant prediction” instruction
//             AppendPlantPredictionInstruction();
//         }

//         /// <summary>
//         /// Appends the final instruction block under “🌵 Plant prediction:” 
//         /// so the LLM can create a short correlation-based forecast.
//         /// </summary>
//         private void AppendPlantPredictionInstruction()
//         {
//             var scenario = CalculatePredictionScenario();
//             string scenarioCheatSheet = GetScenarioDefinitionsAndDataPoints();

//             // Insert scenario data + correlation cheat sheet
//             string plantPredictionInstruction = $@"
// 🌵 Plant prediction: [
// Instruction to LLM:
// Below is scenario data from this token:
// {scenario.OverallScenario}

// === SCENARIO DEFINITIONS & TYPICAL OUTCOMES ===
// {scenarioCheatSheet}

// Task:
// Use both the scenario data and the cheat sheet to produce a short (1–3 sentence) prediction 
// on how this token's price, volume, and interest may evolve in minutes (short term), 
// hours (medium), or days (long). Focus on potential pump/dump timing, 
// especially referencing whether Twitter engagement came before or after listing, 
// the FDV scale, and the buys/sells ratio. 
// Explain if it might crash within minutes or hold for an extended wave. 
// Your audience is degen traders looking for quick flips. 
// ]";

//             TweetExample += plantPredictionInstruction;
//         }

//         /// <summary>
//         /// Gathers scenario data: FDV, time since listing, short-term buys/sells, 
//         /// liquidity/volume ratio, price changes, Twitter engagement, 
//         /// THEN runs correlation logic to produce an outcome string.
//         /// </summary>
//         private PredictionScenario CalculatePredictionScenario()
//         {
//             var scenario = new PredictionScenario();

//             // 1) Bucket FDV
//             float fdv = TokenToTweetAbout.LastTokenUpdate.FdvUsd ?? 0f;
//             scenario.FdvBucket = GetFdvBucket(fdv);

//             // 2) Time since listing
//             double minutesSinceListing = TokenToTweetAbout.TimeSinceListingSeconds / 60.0;
//             scenario.TimeSinceListingBucket = GetTimeSinceListingBucket(minutesSinceListing);

//             // 3) Short-term buys vs. sells (5m)
//             scenario.ShortTermBuysSellsIndicator = GetShortTermBuysSellsIndicator(
//                 TokenToTweetAbout.LastTokenUpdate.M5Buys,
//                 TokenToTweetAbout.LastTokenUpdate.M5Sells
//             );

//             // 4) Liquidity & Volume
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

//             // 7) Correlation-based outcome (the heart of the “best degen trader” logic)
//             string correlationOutcome = GetDegenCorrelationOutcome(
//                 fdv,
//                 minutesSinceListing,
//                 scenario.ShortTermBuysSellsIndicator,
//                 scenario.TwitterEngagementType
//             );

//             // 8) Combine everything into scenario.OverallScenario
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

//         #region Sub-Methods: Scenario Logic + Correlations

//         /// <summary>
//         /// Produces a “degen correlation outcome” string that references 
//         /// typical patterns of FDV scale, listing time, and Twitter hype timing 
//         /// to guess if it’s more like a quick rug, a potential extended run, or something else.
//         /// </summary>
//         private string GetDegenCorrelationOutcome(
//             float fdv,
//             double minutesSinceListing,
//             string shortTermBuySellIndicator,
//             string twitterEngagementType
//         )
//         {
//             // Basic approach: 
//             // 1) Check if FDV is extremely high or extremely low
//             // 2) Check if Twitter is “No Twitter found,” “Strong post-listing tweets,” or “Moderate/earlier tweets”
//             // 3) If FDV < 200k & strong pre-listing => typical quick pump & dump
//             // 4) If FDV is big (>1M) & hype is post-listing => extended wave scenario, 
//             //    referencing “Trump-like scenario” if you want.
//             // 5) Buys vs. sells might further confirm near-immediate or moderate meltdown.
//             // 6) etc.

//             bool isLowFDV = (fdv < 200_000);
//             bool isHighFDV = (fdv > 1_000_000);

//             // For a simplistic example, we’ll parse the “twitterEngagementType”:
//             // - “No Twitter found”
//             // - “Strong post-listing tweets”
//             // - “Moderate or earlier tweets”

//             // Also note shortTermBuySellIndicator can be:
//             // - “High buy pressure” 
//             // - “Moderate buy pressure”
//             // - “High sell pressure”
//             // - “Balanced/mixed”

//             string outcome = "";

//             // 1) No Twitter
//             if (twitterEngagementType.Contains("No Twitter", StringComparison.OrdinalIgnoreCase))
//             {
//                 // If FDV is low and no Twitter => very likely to vanish quickly
//                 if (isLowFDV)
//                     outcome = "Microcap with no engagement. Likely a quick fade/rug within minutes.";
//                 else if (isHighFDV)
//                     outcome = "High FDV but no hype yet. Could be a slow start or a stealth whale buy. Watch the next few minutes/hours.";
//                 else
//                     outcome = "No Twitter + mid FDV. Risky; might see minor trading but no big push without hype.";
//                 return outcome;
//             }

//             // 2) “Strong post-listing tweets” => Usually a second wave of interest
//             if (twitterEngagementType.Contains("Strong post-listing", StringComparison.OrdinalIgnoreCase))
//             {
//                 if (isHighFDV)
//                     outcome = "Large cap + fresh hype. Could mimic the 'Trump scenario' with big pumps/volatility over hours/days.";
//                 else if (isLowFDV)
//                     outcome = "Microcap + sudden post-listing hype. May pump short-term, but watch for a fast dump if whales exit.";
//                 else
//                     outcome = "Mid FDV + post-listing hype. May hold for hours with potential for quick ups/downs.";
//             }
//             // 3) “Moderate or earlier tweets” => Could be pre-launch or mild hype
//             else
//             {
//                 if (isLowFDV)
//                     outcome = "Microcap with prior or moderate hype. Often an orchestrated quick pump/dump in minutes.";
//                 else if (isHighFDV)
//                     outcome = "Bigger FDV with some earlier tweets. Could see extended speculation but caution for sudden sell-offs.";
//                 else
//                     outcome = "Mid FDV with moderate hype. Might hold a bit longer than pure microcap, but watch for dump within the hour.";
//             }

//             // 4) Factor in shortTermBuySellIndicator for an extra nuance
//             if (shortTermBuySellIndicator.Contains("High sell pressure", StringComparison.OrdinalIgnoreCase))
//             {
//                 outcome += " Immediate sells outnumber buys, so watch for a near-term drop.";
//             }
//             else if (shortTermBuySellIndicator.Contains("High buy pressure", StringComparison.OrdinalIgnoreCase))
//             {
//                 outcome += " Short-term buy surge might push a quick pump—take profits fast or risk reversal.";
//             }

//             // 5) Very early listing time => possible hyper-volatility
//             if (minutesSinceListing < 5)
//             {
//                 outcome += " Token is extremely new (<5 min). Watch carefully for instant whale moves.";
//             }

//             return outcome;
//         }

//         private string GetFdvBucket(float fdv)
//         {
//             if (fdv < 50_000) return "<50k (tiny microcap)";
//             if (fdv < 150_000) return "50k–150k (microcap)";
//             if (fdv < 1_000_000) return "150k–1M (smallcap)";
//             if (fdv < 10_000_000) return "1M–10M (midcap)";
//             return ">10M (largecap)";
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
//             double ratio = (buys + 1) / (sells + 1); // avoids zero division

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

//             // “Strong post-listing tweets” vs. “Moderate or earlier tweets” is a rough heuristic
//             if (ratio > 0.5)
//                 return "Strong post-listing tweets";
//             else
//                 return "Moderate or earlier tweets";
//         }

//         /// <summary>
//         /// Returns the multi-line scenario definitions you want included in the final LLM prompt.
//         /// </summary>
//         private string GetScenarioDefinitionsAndDataPoints()
//         {
//             return @"
// 1. FDV Ranges:
//    - <50k: Extremely low FDV, often a tiny microcap. Quick pump or rug in minutes.
//    - 50k–150k: Microcap meme territory; high volatility, can vanish quickly.
//    - 150k–1M: Still “meme-grade” but slightly more traction.
//    - 1M–10M: Midcap; can survive days/weeks but remains volatile.
//    - >10M: Largecap; possibly mainstream, less likely to rug immediately.

// 2. Time Since Listing (Raydium):
//    - <5 minutes: Super fresh; if big pre-hype, watch for quick pump/dump.
//    - 5–30 minutes: Early trades set the trend (whale accumulation or exit).
//    - 30–60 minutes: More stable pattern or big dump if hype fails.
//    - >60 minutes: If volume persists, can last longer; otherwise typically dies off.

// 3. Short-Term Buys vs. Sells (M5, M15):
//    - More buys than sells = possible immediate pump.
//    - More sells than buys = fade or rug risk.
//    - Balanced = uncertain short-term direction.

// 4. Liquidity & Volume:
//    - Very low liquidity (<~50k) = high slippage, can 2× quickly or be impossible to exit.
//    - High volume vs. liquidity (2–3× or more) = big volatility, often a mania or short mania.

// 5. Price Change (M5, H1):
//    - Large % spikes can crash fast.
//    - Moderate gains more sustainable.
//    - Negative % often signals a whale dump or slow fade.

// 6. Twitter Engagement:
//    - No Twitter: No hype, high rug risk if nobody cares.
//    - Pre-listing hype: Often orchestrated pump-and-dump right at launch.
//    - Post-listing hype: May trigger a second wave of FOMO buyers if price didn’t already crash.
//    - Fake influencer or bot spam: High retweets but suspicious pattern can signal quick dump.

// 7. Medium/Longer-Term Survival:
//    - Hours: If big volume remains, can keep pumping or sideways chop.
//    - Days/Weeks: Needs consistent attention, or large FDV with real brand/celeb support (e.g., “Trump coin”).
//    - AI-themed or comedic memes might see short hype cycles, unless whales keep fueling it.

// Imposter tokens: Many are fake versions of a popular theme (Trump, AI, celebrity). 
// They typically fail within minutes/hours, though a few stand out if a real influencer tweets post-listing.
// ";
//         }

//         #endregion
//     }


//     //==============================================================
//     // PREDICTION SCENARIO & MODEL CLASSES
//     //==============================================================

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

//     public class TokenToTweetAbout
//     {
//         public string Address { get; set; }
//         public string Name { get; set; }
//         public string PoolId { get; set; }
//         public string PoolType { get; set; }
//         public string BaseTokenId { get; set; }
//         public string BaseTokenSymbol { get; set; }
//         public string QuoteTokenId { get; set; }
//         public string DexId { get; set; }
//         public string DexscreenerUrl { get; set; }
//         public DateTimeOffset? RaydiumListingTime { get; set; }
//         public long TimeSinceListingSeconds { get; set; }

//         public LastTokenUpdate LastTokenUpdate { get; set; }
//         public DateTimeOffset? TwitterCAFirstMentionTime { get; set; }
//         public string TwitterCAFirstMentionTweetId { get; set; }
//         public int TweetsCAEngagementTotalLikes { get; set; }
//         public int TweetsCAEngagementTotalReplies { get; set; }
//         public int TweetsCAEngagementTotalRetweets { get; set; }
//         public int TweetsCAEngagementTotalViews { get; set; }
//         public int TweetsCAEngagementTotalQuotes { get; set; }

//         public int TwitterCAMostLikesOnSingleTweet { get; set; }
//         public int TwitterCAMostRepliesOnSingleTweet { get; set; }

//         public int TweetsCATweetCount { get; set; }
//         public bool TwitterCAFound { get; set; }

//         public float TokenCombinedScore { get; set; }
//         public float TokenPriceChange24NormScore { get; set; }
//         public float TokenLiquidityNormScore { get; set; }
//         public float TokenTweetCountNormScore { get; set; }
//         public float TokenLikeNormScore { get; set; }
//         public float TokenRetweetNormScore { get; set; }

//         public int TweetsCATweetCount1H { get; set; }
//         public int TweetsCATweetCount3H { get; set; }
//         public int TweetsCATweetCount6H { get; set; }
//         public int TweetsCATweetCount12H { get; set; }
//         public int TweetsCATweetCount24H { get; set; }
//         public int TweetsCATweetCount1WK { get; set; }
//     }

//     public class LastTokenUpdate
//     {
//         public DateTimeOffset PriceUpdateTime { get; set; }

//         public string BaseTokenPriceUsd { get; set; }
//         public string BaseTokenPriceNativeCurrency { get; set; }
//         public string QuoteTokenPriceUsd { get; set; }
//         public string QuoteTokenPriceNativeCurrency { get; set; }
//         public string BaseTokenPriceQuoteToken { get; set; }
//         public string QuoteTokenPriceBaseToken { get; set; }

//         public float? TokenPriceUsd { get; set; }
//         public float? FdvUsd { get; set; }
//         public float? MarketCapUsd { get; set; }

//         public float? PriceChangeM5 { get; set; }
//         public float? PriceChangeH1 { get; set; }
//         public float? PriceChangeH6 { get; set; }
//         public float? PriceChangeH24 { get; set; }

//         public int? M5Buys { get; set; }
//         public int? M5Sells { get; set; }
//         public int? M5Buyers { get; set; }
//         public int? M5Sellers { get; set; }

//         public int? M15Buys { get; set; }
//         public int? M15Sells { get; set; }
//         public int? M15Buyers { get; set; }
//         public int? M15Sellers { get; set; }

//         public int? M30Buys { get; set; }
//         public int? M30Sells { get; set; }
//         public int? M30Buyers { get; set; }
//         public int? M30Sellers { get; set; }

//         public int? H1Buys { get; set; }
//         public int? H1Sells { get; set; }
//         public int? H1Buyers { get; set; }
//         public int? H1Sellers { get; set; }

//         public int? H24Buys { get; set; }
//         public int? H24Sells { get; set; }
//         public int? H24Buyers { get; set; }
//         public int? H24Sellers { get; set; }

//         public float? VolumeM5 { get; set; }
//         public float? VolumeH1 { get; set; }
//         public float? VolumeH6 { get; set; }
//         public float? VolumeH24 { get; set; }

//         public float? ReserveInUsd { get; set; }
//     }
// }
