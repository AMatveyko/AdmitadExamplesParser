// a.snegovoy@gmail.com

namespace TheStore.Api.Front.Entity
{
    internal sealed class PageInfo
    {

        public PageInfo(
            int products,
            int shops ) =>
            ( ProductsCount, ShopCount ) = ( products, shops );
        
        public int ProductsCount { get; }
        public int ShopCount { get; }
    }
}