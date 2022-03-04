// a.snegovoy@gmail.com

using System;

using Common.Entities;

namespace Admitad.Converters.Handlers
{
    internal sealed class NameFromDescription : IOfferHandler
    {
        public Offer Process(
            Offer offer,
            RawOffer rawOffer )
        {
            offer.Name = offer.Description;
            return offer;
        }
    }
}