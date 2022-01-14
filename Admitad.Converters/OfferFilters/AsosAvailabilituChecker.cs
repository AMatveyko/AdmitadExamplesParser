
using Common.Entities;

namespace Admitad.Converters.OfferFilters
{
    internal class AsosAvailabilituChecker : IAvailabilityChecker
    {
        public bool IsOfferAvailable(RawOffer offer) =>
            offer.AvailableAttribute == "true" && offer.AvailableElement == "In Stock";
    }
}
