using Common.Entities;
using System;

namespace Admitad.Converters.Entities
{
    internal abstract class ProductRatingUpdateDataFGSGSGSFGSFGSGSGSG : IProductRatingUpdateData
    {
        public ProductRatingUpdateDataFGSGSGSFGSFGSGSGSG(string id) => Id = id;
        public string Id { get; set; }
        public DateTime RatingUpdateDate { get; set; }
        public long Rating { get; set; }
    }
}
