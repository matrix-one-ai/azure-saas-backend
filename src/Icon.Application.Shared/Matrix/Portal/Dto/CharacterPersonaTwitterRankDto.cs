using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;

namespace Icon.Matrix.Portal.Dto
{
    public class CharacterPersonaTwitterRankDto : EntityDto<Guid>
    {
        // MENTION SCORE
        public int TotalMentions { get; set; }
        public int TotalMentionsScore { get; set; }  // 10 * mentionCount

        // ENGAGEMENT SCORE
        public int TotalLikes { get; set; } // 0.02
        public int TotalReplies { get; set; } // 1
        public int TotalRetweets { get; set; } //  0.5
        public int TotalViews { get; set; } // 0.003        
        public int TotalEngagementScore { get; set; } // sum of all engagement scores

        // QUALITY SCORE


        // it mentions a cryptocoin / coin$ / ticker - yes or no - yes = 4 no = 0         
        public int TotalRelevanceScore { get; set; }

        // count everything with a space in between is a word     
        // count only unique words   
        // 1 word = 1
        // 4 words = 2 
        // 8 words = 3
        // 12=+ words = 4
        public int TotalDepthScore { get; set; }

        // list of unique ** in the tweet compared to all the unique words of all the tweets
        // ** unique cryptocoin / coin$ / ticker 
        // first mention = 4
        // mentioned 5 or less then 5 times = 2
        // mention less then 10 times but more then 5 times = 1
        // mentiond more then 10 times = 0
        public int TotalNoveltyScore { get; set; }

        // either positive or negative
        // positive = 1
        // negative = 0
        public int TotalSentimentScore { get; set; }

        public int TotalQualityScore { get; set; } // sums quality scores


        // TOTAL SCORE
        public int TotalScore { get; set; } // sums all scores  TotalMentionsScore + TotalEngagementScore + TotalQualityScore
        public int TotalScoreTimeDecayed { get; set; } // sums all scores with time decay
        public int Rank { get; set; }

    }
}