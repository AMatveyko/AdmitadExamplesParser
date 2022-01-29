// a.snegovoy@gmail.com

using System;
using System.Text.Json.Serialization;

namespace AdmitadApi.Entities
{
    public sealed class WebSiteResponse
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("status")]
        public string Status { get; set; }
        [JsonPropertyName("kind")]
        public string Kind { get; set; }
        [JsonPropertyName("is_old")]
        public bool IsOld { get; set; }
        [JsonPropertyName("account_id")]
        public string AccountId { get; set; }
        [JsonPropertyName("verification_code")]
        public string VerificationCode { get; set; }
        [JsonPropertyName("creation_date")]
        public DateTime CreationDate { get; set; }
        [JsonPropertyName("adservice")]
        public string Adservice { get; set; }
        [JsonPropertyName("site_url")]
        public string SiteUrl { get; set; }
        [JsonPropertyName("validation_passed")]
        public bool ValidationPassed { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("is_lite")]
        public bool IsLite { get; set; }
    }
}