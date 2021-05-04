// a.snegovoy@gmail.com

namespace AdmitadCommon.Entities
{
    public sealed class ShopStatistics
    {
        public int ShopId { get; set; }
        public string ShopName { get; set; }
        public int? TotalBefore { get; set; }
        public int? TotalAfter { get; set; }
        public int? SoldoutBefore { get; set; }
        public int? SoldoutAfter { get; set; }
        public string Error { get; set; }
    }
}