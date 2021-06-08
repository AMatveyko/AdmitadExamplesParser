// a.snegovoy@gmail.com

using Common.Entities;
using Common.Helpers;

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