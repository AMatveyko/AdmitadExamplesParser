using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Entities
{
    public interface IProductRatingUpdateData
    {
        [JsonProperty("id")] public string Id { get; set; }
        [JsonProperty("ratingUpdateDate")] public DateTime RatingUpdateDate { get; set; }
        [JsonProperty("rating")] public long Rating { get; set; }
    }
}
