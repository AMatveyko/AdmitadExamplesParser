using Common.Entities;

namespace Admitad.Converters.OfferFilters
{
    internal interface IAvailabilityChecker
    {
        bool IsOfferAvailable(RawOffer offer);
    }
}
