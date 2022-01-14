using Common.Entities.Rating;
using RatingCalculator.Common;

namespace RatingCalculator.Workers
{
    internal static class RatingConverter
    {

        private const int RatingTemplate =        1000000000;
        private const int OpenProgrammCorrector =  100000000;
        private const int InStockCorrector =        10000000;
        private const int InternalRatingCorrector =   100000;
        private const int ECPCCorrector =               1000;

        public static ProductRating Convert( ProductRatingWorkData data) {
            var rating = RatingTemplate;
            rating += data.CTR;
            rating = AddSoldOutRating(rating, data.IsInStock);
            rating = AddECPCCorrector(rating, data.ECPCRating);
            rating = AddInternalRatingCorrector(rating, data.ShopInternalRating);
            rating = AddOpenProgramCorrector(rating, data.IsOpenProgramm);

            return new ProductRating(data.ProductId, rating);
        }

        private static int AddOpenProgramCorrector(int rating, bool isOpen) => AddBoolCorrector(rating, OpenProgrammCorrector, isOpen);
        private static int AddInternalRatingCorrector(int rating, int internalRating) => AddIntCorrector(rating, InternalRatingCorrector, internalRating);
        private static int AddECPCCorrector(int rating, int epcp) => AddIntCorrector(rating, ECPCCorrector, epcp);
        private static int AddIntCorrector(int rating, int corrector, int data) => rating + (corrector * data);
        private static int AddSoldOutRating(int rating, bool isInStock) => AddBoolCorrector(rating, InStockCorrector, isInStock);
        private static int AddBoolCorrector(int rating, int corrector, bool isAdd) => isAdd ? rating + corrector : rating;
    }
}
