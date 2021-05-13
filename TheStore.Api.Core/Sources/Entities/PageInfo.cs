// a.snegovoy@gmail.com

namespace TheStore.Api.Core.Sources.Entities
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