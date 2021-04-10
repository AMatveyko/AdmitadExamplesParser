// a.snegovoy@gmail.com

namespace AdmitadExamplesParser.Entities
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