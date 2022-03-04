// a.snegovoy@gmail.com

using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AdmitadApi.Entities
{
    public sealed class CompaignResponse
    {
        
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("status")]
        public string Status { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("name_aliases")]
        public string NameAliases { get; set; }
        [JsonPropertyName("currency")]
        public string Currency { get; set; }
        [JsonPropertyName("rating")]
        public string Rating { get; set; }
        [JsonPropertyName("ecpc")]
        public decimal ECPC { get; set; }
        [JsonPropertyName("epc")]
        public decimal EPC { get; set; }
        [JsonPropertyName("modified_date")]
        public DateTime ModifiedDate { get; set; }
        [JsonPropertyName("feeds_info")]
        public List<AdmitadFeedInfoResponse> Feeds { get; set; }
        [JsonPropertyName("connection_status")]
        public string ConnectionStatus { get; set; }

    }
}