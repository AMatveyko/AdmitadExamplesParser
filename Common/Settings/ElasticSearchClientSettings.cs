// a.snegovoy@gmail.com

using Common.Entities;

namespace Common.Settings
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