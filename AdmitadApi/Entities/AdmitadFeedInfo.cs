// a.snegovoy@gmail.com

using System;

namespace AdmitadApi.Entities
{
    public sealed class AdmitadFeedInfo
    {
        public string Name { get; set; }
        public string XmlFeed { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}