// a.snegovoy@gmail.com

using AdmitadCommon.Entities;

namespace Admitad.Converters.Handlers
{
    internal interface IOfferHandler
    {
        Offer Process(
            Offer offer,
            RawOffer rawOffer );
    }
}