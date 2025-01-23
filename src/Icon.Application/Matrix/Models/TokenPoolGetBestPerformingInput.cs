using System;
using System.Collections.Generic;
using Microsoft.Identity.Client;

namespace Icon.Matrix.TokenPools
{
    public class TokenPoolGetBestPerformingInput
    {
        public DateTime? CreatedAfter { get; set; }
        public DateTime? CreatedBefore { get; set; }

        public float? MinFdvUsd { get; set; }
        public float? MinLiquidtyUsd { get; set; }
        public float? MinVolumeH1 { get; set; }
        public float? MinRisePercentageSinceCreation { get; set; }

        public int MaxPriceUpdateAgeMinutes { get; set; }
        public int MaxPools { get; set; }
        public bool ExcludeTweetedPools { get; set; }

        public bool PerformPoolUpdate { get; set; }

        public Guid? TestPairId { get; set; }


    }
}
