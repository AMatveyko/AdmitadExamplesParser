using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Entities.Rating
{
    public sealed class ProductRatingData
    {
        public ProductRatingData(string productId, string shopId, bool soldOut) => (ProductId, ShopId, SoldOut) = (productId, shopId, soldOut);
        public string ProductId { get; }
        public string ShopId { get; }
        public bool SoldOut { get; }
    }
}
