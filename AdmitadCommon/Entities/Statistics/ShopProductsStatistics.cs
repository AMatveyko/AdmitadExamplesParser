// a.snegovoy@gmail.com

namespace AdmitadCommon.Entities.Statistics
{
    public sealed class ShopProductsStatistics
    {
        public ShopProductsStatistics( ShopProduct before, ShopProduct after ) =>
            ( Before, After ) = ( before, after );
        public ShopProduct After { get; }
        public ShopProduct Before { get; }
    }
}