namespace RatingCalculator.Common
{
    internal sealed class ProductRatingWorkData
    {
        public ProductRatingWorkData(string productId, bool isInStock) => (ProductId, IsInStock) = (productId, isInStock);
        public string ProductId { get; }
        public bool IsInStock { get; }
        public int CTR { get; set; }
        public int ECPCRating { get; set; }
        public int ShopInternalRating { get; set; }
        public bool IsOpenProgramm { get; set; }
    }
}
