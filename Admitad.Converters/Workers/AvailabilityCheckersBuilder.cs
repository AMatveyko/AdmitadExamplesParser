using Admitad.Converters.OfferFilters;
using ShopsNames = Common.Constants.ShopsNames;

namespace Admitad.Converters.Workers
{
    internal static class AvailabilityCheckersBuilder
    {
        public static IAvailabilityChecker GetChecker(string shopName) => shopName switch
        {
            ShopsNames.Asos => new AsosAvailabilituChecker(),
            ShopsNames.Ecco => new EccoAvailabilityChecker(),
            _ => new DefaultAvailabilityChecker()
        };

    }
}
