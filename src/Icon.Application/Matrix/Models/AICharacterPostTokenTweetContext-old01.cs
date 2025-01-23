// using System;
// using System.Collections.Generic;
// using Icon.Matrix.Models;

// // 4. Future Improvements / Missing Pieces
// // Influencer/Account Quality: Knowing if tweets are from large, reputable Twitter accounts vs. small/bot accounts heavily impacts the credibility of pre-listing hype.
// // Unique Wallet Distribution: On-chain data of how many wallets hold the token, and how concentrated the supply is. High concentration in one or two wallets indicates a bigger rug risk.
// // Detailed Retweet-to-Like Ratio: Right now we only do a simple ratio. Weighted by account size or repeated spamming could refine the hype analysis.
// // Successive Price Candles: Checking the last 5–10 minute-by-minute price bars can give more precise “momentum or meltdown” predictions.

// namespace Icon.Matrix.AIManager.CharacterPostTokenTweet
// {
//     public class AICharacterPostTokenTweetContext
//     {
//         //public string TweetInstruction { get; set; }
//         public string TweetExample { get; set; }
//         public TokenToTweetAbout TokenToTweetAbout { get; set; }

//         public AICharacterPostTokenTweetContext(
//             RaydiumPair raydiumPair,
//             CoingeckoPoolUpdate coingeckoPoolUpdate)
//         {

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

// 🌵 Plant score: [TokenToTweetAbout.CombinedScore] 
// 🌵 Plant summary: [Write a two / three sentence analysis for a degen memecoin trader, make sure to say something valuable, like an interesting correlation] 
// 🌵 Plant prediction: [Will be appended below]

// Dexscreener: [TokenToTweetAbout.DexscreenerUrl]

// ⚠️ Remember do your own research Gardeners. Not financial advice. I am just a Plant.

// -- Instruction to AI: 

// Prices are in USD, where applicable show dollar sign and use K, M, B for large numbers dont use decimals for numbers bigger then 1
// EXAMPLE (not final tweet, just a guide for you to craft your own).";

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


//                 // The last token update (already existed)
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
//                 },
//             };

//             AppendPlantPredictionInstruction();
//         }
//         /// <summary>
//         /// Appends a bracketed instruction under “Plant prediction” that includes:
//         /// 1) The scenario data from CalculatePredictionScenario()
//         /// 2) A cheat sheet of typical FDV/time/buys-sells/volume/Twitter engagement patterns
//         /// so even a simple LLM can form a correct 1–3 sentence forecast.
//         /// </summary>
//         public void AppendPlantPredictionInstruction()
//         {
//             // 1) Run the scenario calculation
//             var scenario = CalculatePredictionScenario();

//             // 2) Prepare the scenario definitions & typical outcomes cheat sheet
//             string scenarioCheatSheet = GetScenarioDefinitionsAndDataPoints();

//             // 3) Build the instruction text for the LLM
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
// hours (medium), or days (long). Consider if it's likely to rug quickly, 
// pump then dump, or maintain enough hype to last longer. 
// Mention potential profit or caution. 
// Your target group is degen memecoin traders - most looking for quick gains and are looking for how fast something will pump or dump.
// The prediction should mention why it will likely pump or dump based on the values provided
// ]";

//             // 4) Append it to the TweetExample
//             TweetExample += plantPredictionInstruction;
//         }

//         /// <summary>
//         /// Main scenario classification logic (unchanged from prior example, 
//         /// but you can customize thresholds or add more checks).
//         /// </summary>
//         public PredictionScenario CalculatePredictionScenario()
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

//             // Combine a textual summary
//             scenario.OverallScenario = $"FDV Bucket: {scenario.FdvBucket}, " +
//                                        $"Time Bucket: {scenario.TimeSinceListingBucket}, " +
//                                        $"Buys/Sells(5m): {scenario.ShortTermBuysSellsIndicator}, " +
//                                        $"Liquidity/Volume(1h): {scenario.LiquidityVolumeIndicator}, " +
//                                        $"Price Change(5m,1h): {scenario.PriceChangeIndicator}, " +
//                                        $"Twitter Eng.: {scenario.TwitterEngagementType}";

//             return scenario;
//         }

//         /// <summary>
//         /// Returns a multi-line string with scenario definitions and typical outcomes,
//         /// so even a simpler LLM can see the direct correlation between data and likely result.
//         /// </summary>
//         private string GetScenarioDefinitionsAndDataPoints()
//         {
//             return @"
// 1. FDV Ranges:
//    - <50k: Extremely low FDV, often a tiny microcap. Can pump or rug within minutes.
//    - 50k–150k: Microcap meme territory; high volatility, 2–5× or vanish quickly.
//    - 150k–1M: Still “meme-grade” risk but may have a bit more traction.
//    - 1M–10M: Bigger fish, can last days/weeks but still prone to volatility.
//    - >10M: Possibly more established or mainstream hype, less likely to rug instantly.

// 2. Time Since Listing (Raydium):
//    - <5 minutes: Super fresh; pre-hype can lead to quick pump or dump.
//    - 5–30 minutes: Early trades define accumulation vs. dumping.
//    - 30–60 minutes: Patterns may start forming; watch if whales hold or sell.
//    - >60 minutes: Price/volume stability is more telling of longer-term viability.

// 3. Short-Term Buys vs. Sells (M5, M15):
//    - Many buys vs. sells = potential quick pump.
//    - Many sells vs. buys = immediate fade or rug.
//    - Balanced = uncertain or stable short-term.

// 4. Liquidity & Volume:
//    - Low liquidity (< ~$50k) = high slippage, quick 2× potential but risky exits.
//    - High volume relative to liquidity (e.g. 2–3× or more) = big volatility, sudden moves.

// 5. Price Change (M5, H1):
//    - Huge % spikes often crash fast.
//    - Moderate +% can be more sustainable.
//    - Negative % early on often signals rapid fade or whale dump.

// 6. Twitter Engagement:
//    - No Twitter: High risk, no hype, minimal buys.
//    - Pre-listing hype: Often orchestrated pump/dump.
//    - Post-listing hype: Can spark a second wave of buyers.
//    - Fake or spammy influencer buzz: Watch out for quick dumps after big tweets.

// 7. Medium/Longer-Term Survival:
//    - Hours: If volume stays high, can keep pumping or go sideways.
//    - Days/Weeks: Usually need real community or big FDV + celebrity/political hype 
//      (e.g. “Trump coin,” but note thousands of fakes exist with quick rugs).
//    - AI-themed or comedic meme tokens can see short hype cycles 
//      unless backed by consistent attention or whale support.

// Overall note: This space is infested with imposter tokens. Many launch simultaneously, 
// using the same name as a trending theme (Trump, AI, etc.), usually failing within minutes 
// or hours. Rare ones sustain multi-day hype but remain high risk.";
//         }

//         #region Sub-methods for scenario logic

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

//             if (ratio > 0.5) return "Strong post-listing tweets";
//             else return "Moderate or earlier tweets";
//         }

//         #endregion
//     }




// }


// public class PredictionScenario
// {
//     public string FdvBucket { get; set; }
//     public string TimeSinceListingBucket { get; set; }
//     public string ShortTermBuysSellsIndicator { get; set; }
//     public string LiquidityVolumeIndicator { get; set; }
//     public string PriceChangeIndicator { get; set; }
//     public string TwitterEngagementType { get; set; }

//     public string OverallScenario { get; set; }
// }

// public class TokenToTweetAbout
// {
//     public string Address { get; set; }
//     public string Name { get; set; }
//     public string PoolId { get; set; }
//     public string PoolType { get; set; }
//     public string BaseTokenId { get; set; }
//     public string BaseTokenSymbol { get; set; }
//     public string QuoteTokenId { get; set; }
//     public string DexId { get; set; }
//     public string DexscreenerUrl { get; set; }
//     public DateTimeOffset? RaydiumListingTime { get; set; }
//     public long TimeSinceListingSeconds { get; set; }

//     public LastTokenUpdate LastTokenUpdate { get; set; }
//     public DateTimeOffset? TwitterCAFirstMentionTime { get; set; }
//     public string TwitterCAFirstMentionTweetId { get; set; }
//     public int TweetsCAEngagementTotalLikes { get; set; }
//     public int TweetsCAEngagementTotalReplies { get; set; }
//     public int TweetsCAEngagementTotalRetweets { get; set; }
//     public int TweetsCAEngagementTotalViews { get; set; }
//     public int TweetsCAEngagementTotalQuotes { get; set; }

//     public int TwitterCAMostLikesOnSingleTweet { get; set; }
//     public int TwitterCAMostRepliesOnSingleTweet { get; set; }


//     public int TweetsCATweetCount { get; set; }
//     public bool TwitterCAFound { get; set; }

//     public float TokenCombinedScore { get; set; }
//     public float TokenPriceChange24NormScore { get; set; }
//     public float TokenLiquidityNormScore { get; set; }
//     public float TokenTweetCountNormScore { get; set; }
//     public float TokenLikeNormScore { get; set; }
//     public float TokenRetweetNormScore { get; set; }

//     public int TweetsCATweetCount1H { get; set; }
//     public int TweetsCATweetCount3H { get; set; }
//     public int TweetsCATweetCount6H { get; set; }
//     public int TweetsCATweetCount12H { get; set; }
//     public int TweetsCATweetCount24H { get; set; }
//     public int TweetsCATweetCount1WK { get; set; }
// }

// public class LastTokenUpdate
// {
//     public DateTimeOffset PriceUpdateTime { get; set; }

//     public string BaseTokenPriceUsd { get; set; }
//     public string BaseTokenPriceNativeCurrency { get; set; }
//     public string QuoteTokenPriceUsd { get; set; }
//     public string QuoteTokenPriceNativeCurrency { get; set; }
//     public string BaseTokenPriceQuoteToken { get; set; }
//     public string QuoteTokenPriceBaseToken { get; set; }

//     public float? TokenPriceUsd { get; set; }
//     public float? FdvUsd { get; set; }
//     public float? MarketCapUsd { get; set; }

//     public float? PriceChangeM5 { get; set; }
//     public float? PriceChangeH1 { get; set; }
//     public float? PriceChangeH6 { get; set; }
//     public float? PriceChangeH24 { get; set; }

//     public int? M5Buys { get; set; }
//     public int? M5Sells { get; set; }
//     public int? M5Buyers { get; set; }
//     public int? M5Sellers { get; set; }

//     public int? M15Buys { get; set; }
//     public int? M15Sells { get; set; }
//     public int? M15Buyers { get; set; }
//     public int? M15Sellers { get; set; }

//     public int? M30Buys { get; set; }
//     public int? M30Sells { get; set; }
//     public int? M30Buyers { get; set; }
//     public int? M30Sellers { get; set; }

//     public int? H1Buys { get; set; }
//     public int? H1Sells { get; set; }
//     public int? H1Buyers { get; set; }
//     public int? H1Sellers { get; set; }

//     public int? H24Buys { get; set; }
//     public int? H24Sells { get; set; }
//     public int? H24Buyers { get; set; }
//     public int? H24Sellers { get; set; }

//     public float? VolumeM5 { get; set; }
//     public float? VolumeH1 { get; set; }
//     public float? VolumeH6 { get; set; }
//     public float? VolumeH24 { get; set; }

//     public float? ReserveInUsd { get; set; }
// }