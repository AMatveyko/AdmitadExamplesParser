// a.snegovoy@gmail.com

using AdmitadCommon.Entities;

using Common.Entities;

namespace Admitad.Converters.Handlers
{
    internal sealed class CurrencyUSD : IOfferHandler
    {
        public Offer Process(
            Offer offer,
            RawOffer rawOffer )
        {
            offer.Currency = Currency.USD;
            return offer;
        }
    }
}