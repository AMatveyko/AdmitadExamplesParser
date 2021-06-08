// a.snegovoy@gmail.com

using AdmitadCommon.Entities;

namespace Admitad.Converters.Handlers
{
    internal sealed class AlwaysAdultUndefined : IOfferHandler
    {
        public Offer Process(
            Offer offer,
            RawOffer rawOffer )
        {
            offer.Age = Age.Adult;
            offer.Gender = Gender.Undefined;
            return offer;
        }
    }
}