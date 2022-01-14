using Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admitad.Converters.OfferFilters
{
    internal class DefaultAvailabilityChecker : IAvailabilityChecker
    {
        public bool IsOfferAvailable(RawOffer offer) => string.IsNullOrWhiteSpace(offer.AvailableAttribute) || offer.AvailableAttribute == "true";
    }
}
