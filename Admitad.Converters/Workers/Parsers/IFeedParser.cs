// a.snegovoy@gmail.com

using Common.Entities;

namespace Admitad.Converters.Workers
{
    public interface IFeedParser
    {
        public ShopData Parse(
            bool isOnlyCategories = false );
    }
}