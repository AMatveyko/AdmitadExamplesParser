// a.snegovoy@gmail.com

using AdmitadCommon.Entities;

namespace Admitad.Converters.Handlers
{
    internal sealed class SoldOutIfAvailableFalse : IOfferHandler
    {
        public Offer Process( Offer offer, RawOffer rawOffer )
        {
            offer.SoldOut = rawOffer.Available == false;
            return offer;
        }
    }
}