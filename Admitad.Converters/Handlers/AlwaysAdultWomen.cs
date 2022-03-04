// a.snegovoy@gmail.com

using AdmitadCommon.Entities;

using Common.Entities;

namespace Admitad.Converters.Handlers
{
    internal sealed class AlwaysAdultWomen : IOfferHandler
    {
        public Offer Process(
            Offer offer,
            RawOffer rawOffer )
        {
            offer.Age = Age.Adult;
            offer.Gender = Gender.Woman;
            return offer;
        }
    }
}