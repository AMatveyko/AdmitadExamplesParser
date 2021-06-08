// a.snegovoy@gmail.com

using System.Collections.Generic;

namespace ImportFromOld
{
    internal sealed class OfferLinks
    {
        public OfferLinks( long offerId ) => OfferId = offerId;
        
        public long OfferId { get; }
        public string CountryId { get; set; }
        public string BrandId { get; set; }
        public List<string> Tags { get; set; }
        public List<string> Colors { get; set; }
        public List<string> Sizes { get; set; }
        public List<string> Materials { get; set; }
    }
}