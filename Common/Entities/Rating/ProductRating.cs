using System;

namespace Common.Entities.Rating
{
    public sealed class ProductRating
    {
        public ProductRating(string productId, int rating) => (ProductId, Rating) = (productId, rating);
        public string ProductId { get; }
        public long Rating { get; }
    }
}
