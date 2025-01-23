using System;
using System.Collections.Generic;

namespace Icon.Matrix.TokenDiscovery
{

    public static class TokenStageDefinitions
    {
        /// <summary>
        /// All available stage names. You can add or remove stages here.
        /// </summary>
        public static class Stages
        {
            public const string Death = "Death";
            public const string Inception = "Inception";
            public const string PriceTracking = "PriceTracking";
            public const string EngagementPostTracking = "EngagementPostTracking";
            public const string EngagementDetailTracking = "EngagementDetailTracking";
            public const string TrackingEndedTokenAge = "TrackingEndedTokenAge";
        }

        public static List<string> ActiveStages = new List<string>
        {
            Stages.Inception,
            Stages.PriceTracking,
            Stages.EngagementPostTracking,
            Stages.EngagementDetailTracking
        };

        public static List<string> InactiveStages = new List<string>
        {
            Stages.Death,
            Stages.TrackingEndedTokenAge
        };

        public static Dictionary<string, TokenStageDefinition> Definitions = new Dictionary<string, TokenStageDefinition>
        {
            {
                Stages.Death,
                new TokenStageDefinition
                {
                    StageName = Stages.Death,
                }
            },
            {
                Stages.Inception,
                new TokenStageDefinition
                {
                    StageName = Stages.Inception,
                }
            },
            {
                Stages.PriceTracking,
                new TokenStageDefinition
                {
                    StageName = Stages.PriceTracking,

                    MinLiquidity01MinuteRefresh = 50000,
                    MinLiquidity05MinuteRefresh = 45000,
                    MinLiquidity15MinuteRefresh = 40000,
                    MinLiquidity30MinuteRefresh = 35000,
                    MinLiquidity1HourRefresh = 30000,
                    MinLiquidity6HourRefresh = 25000,
                    MinLiquidity24HourRefresh = 21000
                }
            },
            {
                Stages.EngagementPostTracking,
                new TokenStageDefinition
                {
                    StageName = Stages.EngagementPostTracking,

                    MinLiquidity01MinuteRefresh = 500000,
                    MinLiquidity05MinuteRefresh = 200000,
                    MinLiquidity15MinuteRefresh = 100000,
                    MinLiquidity30MinuteRefresh = 80000,
                    MinLiquidity1HourRefresh = 50000,
                    MinLiquidity6HourRefresh = 30000,
                    MinLiquidity24HourRefresh = 21000
                }
            },
            {
                Stages.EngagementDetailTracking,
                new TokenStageDefinition
                {
                    StageName = Stages.EngagementDetailTracking,
                    MinTweetsLast03HourRefresh = 1,
                }
            }
        };
    }

    public class TokenTrackingRequirements
    {
        public float? MinLiquidity { get; set; }
        public int MaxTokenAgeHours { get; set; }

        public TokenTrackingRequirements(float? minLiquidity, int maxTokenAgeHours)
        {
            MinLiquidity = minLiquidity;
            MaxTokenAgeHours = maxTokenAgeHours;
        }
    }

    public class TokenStageDefinition
    {
        public string StageName { get; set; }

        public float? MinLiquidity01MinuteRefresh { get; set; }
        public float? MinLiquidity05MinuteRefresh { get; set; }
        public float? MinLiquidity15MinuteRefresh { get; set; }
        public float? MinLiquidity30MinuteRefresh { get; set; }
        public float? MinLiquidity1HourRefresh { get; set; }
        public float? MinLiquidity6HourRefresh { get; set; }
        public float? MinLiquidity24HourRefresh { get; set; }

        public int? MinTweetsLast03HourRefresh { get; set; }

    }


}
