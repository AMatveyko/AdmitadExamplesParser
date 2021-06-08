// a.snegovoy@gmail.com

using AdmitadCommon.Entities;
using AdmitadCommon.Helpers;

namespace Admitad.Converters.Handlers
{
    internal sealed class IdFromOfferIdAndShopId : IOfferHandler
    {
        public Offer Process( Offer offer, RawOffer rawOffer )
        {
            var rawId = $"{offer.ShopId}:{rawOffer.OfferId}";
            offer.Id = HashHelper.GetMd5Hash( rawId );
            return offer;
        }
    }
}