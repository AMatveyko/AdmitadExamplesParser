// a.snegovoy@gmail.com

using Common.Entities;

namespace Admitad.Converters.Handlers
{
    internal sealed class OnlyAdult : IOfferHandler
    {
        public Offer Process(
            Offer offer,
            RawOffer rawOffer )
        {
            offer.Age = Age.Adult;
            return offer;
        }
    }
}