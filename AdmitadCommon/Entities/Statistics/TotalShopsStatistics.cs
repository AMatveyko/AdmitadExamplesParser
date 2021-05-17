// a.snegovoy@gmail.com

using System.Collections.Generic;

namespace AdmitadCommon.Entities.Statistics
{
    public sealed class TotalShopsStatistics
    {
        public int TotalEnabledShops { get; set; }
        public List<ShopProductStatistics> ByShop { get; set; }
    }
}