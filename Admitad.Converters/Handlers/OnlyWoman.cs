// a.snegovoy@gmail.com

using Common.Entities;

namespace Admitad.Converters.Handlers
{
    internal sealed class OnlyWoman : IOfferHandler
    {
        public Offer Process(
            Offer offer,
            RawOffer rawOffer )
        {
            offer.Gender = Gender.Woman;
            return offer;
        }
    }
}