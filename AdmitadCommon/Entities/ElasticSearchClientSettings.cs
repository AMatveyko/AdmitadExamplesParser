// a.snegovoy@gmail.com

namespace AdmitadCommon.Entities
{
    public sealed class ElasticSearchClientSettings
    {
        public ComponentType ComponentForIndex { get; set; }
        public string ElasticSearchUrl { get; set; }
        public string DefaultIndex { get; set; }
        public int FrameSize { get; set; }
        public string ShopName { get; set; }
    }
}