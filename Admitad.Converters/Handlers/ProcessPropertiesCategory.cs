// a.snegovoy@gmail.com

using Admitad.Converters.Entities;

using Common.Entities;

namespace Admitad.Converters.Handlers
{
    internal sealed class ProcessPropertiesCategory : IOfferHandler
    {

        private readonly ICategoryPropertiesHandler _handler;

        public ProcessPropertiesCategory(
            ICategoryPropertiesHandler handler ) =>
            _handler = handler;
        
        public Offer Process( Offer offer, RawOffer rawOffer )
        {
            var properties = _handler.GetProperties( rawOffer );
            
            if( properties == null ) {
                return offer;
            }
            
            offer.Age = properties.Age ?? offer.Age;
            offer.Gender = properties.Gender ?? offer.Gender;
            offer.Type = properties.Type;
            offer.AgeRange ??= properties.AgeRange;
            
            return offer;
        }
    }
}