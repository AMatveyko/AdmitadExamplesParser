// a.snegovoy@gmail.com

using System.Collections.Generic;

using Common.Entities;

namespace Admitad.Converters.Entities
{
    internal abstract class BaseCategoryPropertiesContainer : ICategoryPropertiesHandler
    {
        
        private readonly Dictionary<string, ShopCategoryProperties> _properties;

        protected BaseCategoryPropertiesContainer( Dictionary<string, ShopCategoryProperties> properties ) =>
            _properties = properties;

        public ShopCategoryProperties GetProperties( RawOfferWithCategoryList rawOffer )
        {
            foreach( var categoryId in rawOffer.Categories ) {
                if( _properties.ContainsKey( categoryId ) ) {
                    return _properties[ categoryId ];
                }
            }
            
            return null;
        }
    }
}