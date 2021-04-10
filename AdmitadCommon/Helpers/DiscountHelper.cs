// a.snegovoy@gmail.com

namespace AdmitadCommon.Helpers
{
    public static class DiscountHelper
    {
        public static byte CalculateDiscount( decimal price, decimal? oldPrice ) {
            if( oldPrice == null ||
                price == oldPrice ||
                price > oldPrice ) {
                return 0;
            }

            var percent =  (byte) ( ( 1 - price / oldPrice.Value ) * 100 );
            
            return percent;
        }
    }
}