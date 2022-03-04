using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Entities
{
    public sealed class ProductRatingInfoFromElastic
    {
        [JsonProperty("id")] public string Id { get; set; }
        [JsonProperty("soldout")] public byte? Soldout { get; set; }
        [JsonProperty("shopId")] public string ShopId { get; set; }
        [JsonProperty("rating")] public long Rating { get; set; }
    }
}
