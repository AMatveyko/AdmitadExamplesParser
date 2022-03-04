// a.snegovoy@gmail.com

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AdmitadApi.Entities
{
    public sealed class CompaingListResponse
    {
        [JsonPropertyName("results")]
        public List<CompaignResponse> Result { get; set; }
    }
}