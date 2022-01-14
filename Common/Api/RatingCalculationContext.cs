using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Api
{
    public sealed class RatingCalculationContext : BackgroundBaseContext
    {
        public RatingCalculationContext() : base(nameof(RatingCalculationContext), string.Empty) {}
    }
}
