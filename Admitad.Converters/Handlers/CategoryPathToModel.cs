// a.snegovoy@gmail.com

using Common.Entities;
using Common.Extensions;

namespace Admitad.Converters.Handlers
{
    internal sealed class CategoryPathToModel : IOfferHandler
    {
        public Offer Process(
            Offer offer,
            RawOffer rawOffer )
        {
            if( offer.Model.IsNullOrWhiteSpace() ) {
                offer.Model = offer.CategoryPath;
            }

            return offer;
        }
    }
}