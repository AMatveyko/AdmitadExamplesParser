// a.snegovoy@gmail.com

namespace ElasticSearchData
{
    public sealed class SearchParameters
    {
        public string Descriptions { get; set; }
        public string ParamName { get; set; }
        public string ParamValue { get; set; }
        public string VendorName { get; set; }
        public int ShopId { get; set; }
        public bool Aggregation { get; set; }
        
        public const string ProductIdField = "productId";
        public const string DescriptionField = "description";
        public const string ParamNameField = "";
        public const string ParamValueField = "";
        public const string VendorNameField = "vendorNameClearly";
        public const string ShopIdField = "shopId";
    }
}