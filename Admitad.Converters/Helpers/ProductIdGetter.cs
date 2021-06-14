// a.snegovoy@gmail.com

using System.Linq;

using Common.Entities;
using Common.Helpers;

namespace Admitad.Converters.Helpers
{
    internal static class ProductIdGetter
    {
        public static string FirstImageUrl( RawOffer offer, int shopId ) =>
            HashHelper.GetMd5Hash( offer.Pictures.FirstOrDefault() ?? offer.Url );
        
        public static string GroupIdAndShopId( RawOffer offer, int shopId ) =>
            HashHelper.GetMd5Hash( shopId.ToString(), offer.GroupId );
        
        public static string OfferIdAndShopId( RawOffer offer, int shopId ) =>
            HashHelper.GetMd5Hash( shopId.ToString(), ":", offer.OfferId  );

    }
}