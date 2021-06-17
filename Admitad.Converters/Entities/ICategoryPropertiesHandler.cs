// a.snegovoy@gmail.com

using Common.Entities;

namespace Admitad.Converters.Entities
{
    internal interface ICategoryPropertiesHandler
    {
        ShopCategoryProperties GetProperties( RawOfferWithCategoryList rawOffer );
    }
}