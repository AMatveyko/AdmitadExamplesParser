// a.snegovoy@gmail.com

using System;
using System.Text.Json.Serialization;

namespace AdmitadApi.Entities
{
    public sealed class AdmitadFeedInfoResponse
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("xml_link")]
        public string XmlLink { get; set; }
        [JsonPropertyName("admitad_last_update")]
        public string AdmitadLastUpdate { get; set; }
        [JsonPropertyName("advertiser_last_update")]
        public string AdvertiserLastUpdate { get; set; }
    }
}