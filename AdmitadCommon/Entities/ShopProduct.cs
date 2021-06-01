// a.snegovoy@gmail.com

using System;

namespace AdmitadCommon.Entities
{
    public sealed class ShopProduct
    {
        public int ShopId { get; set; }
        public string ShopName { get; set; }
        public int TotalCount { get; set; }
        public int SoldoutCount { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}