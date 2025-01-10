using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using Icon.BaseManagement;

namespace Icon.Matrix.Portal.Dto
{
    public class CharacterTweetDto
    {
        public string Name { get; set; } = "Plant";
        public string Handle { get; set; } = "@plantdotfun";
        public string AvatarUrl { get; set; } = "https://pbs.twimg.com/profile_images/1874483608099971072/41AndH6s.jpg";
        public DateTimeOffset TweetDate { get; set; }
        public string TweetUrl { get; set; }
        public int BookmarkCount { get; set; }
        public int Likes { get; set; }
        public int Replies { get; set; }
        public int Retweets { get; set; }
        public int Views { get; set; }

    }
}