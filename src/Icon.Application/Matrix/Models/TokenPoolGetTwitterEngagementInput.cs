using System;
using System.Collections.Generic;
using Icon.Matrix.Models;
using Microsoft.Identity.Client;

namespace Icon.Matrix.TokenPools
{
    public class TokenPoolGetTwitterEngagementInput
    {
        public List<TokenPool> Pools { get; set; }
        public bool PerformTwitterPostCountUpdate { get; set; }
        public bool PerformTwitterEngagementUpdate { get; set; }
        public int PerformTwitterEngagementUpdateMaxTweets { get; set; }

        public TokenPoolGetTwitterEngagementInput(List<TokenPool> pools)
        {
            Pools = pools;
        }
    }
}
