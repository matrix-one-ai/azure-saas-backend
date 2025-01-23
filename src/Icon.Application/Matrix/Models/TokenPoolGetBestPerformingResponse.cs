using System;
using System.Collections.Generic;
using Icon.Matrix.Models;
using Icon.Migrations;
using Microsoft.Identity.Client;

namespace Icon.Matrix.TokenPools
{
    public class TokenPoolGetBestPerformingResponse
    {
        public List<TokenPool> PerformingPools { get; set; }

        public TokenPoolGetBestPerformingResponse()
        {
            PerformingPools = new List<TokenPool>();
        }
    }

    public class TokenPool
    {
        public RaydiumPair RaydiumPair { get; set; }
        public CoingeckoPoolUpdate CoinGeckoLastUpdate { get; set; }
    }
}
