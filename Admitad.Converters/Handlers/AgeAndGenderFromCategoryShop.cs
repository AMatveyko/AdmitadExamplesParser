// a.snegovoy@gmail.com

using System.Collections.Generic;
using System.Linq;

using AdmitadSqlData.Helpers;

using Common.Entities;
using Common.Helpers;

namespace Admitad.Converters.Handlers
{
    internal sealed class AgeAndGenderFromCategoryShop : IOfferHandler
    {

        private readonly DbHelper _dbHelper;
        private Dictionary<string, AgeGenderForCategoryContainer> _cache;

        public AgeAndGenderFromCategoryShop( DbHelper dbHelper ) => _dbHelper = dbHelper;
        
        public Offer Process( Offer offer, RawOffer rawOffer )
        {
            offer.Age = GetAge( offer );
            offer.Gender = GetGender( offer );
            
            return offer;
        }

        private void FillCache( int shopId )
        {
            _cache = _dbHelper.GetAgeGenderFromCategories( shopId )
                .ToDictionary( k => k.CategoryId, v => v );
        }
        
        private Age GetAge( Offer offer )
        {
            var container = GetFromCache( offer.CategoryId, offer.ShopId );
            return container == null || container.AgeId == -1
                ? offer.Age
                : AgeHelper.GetByName( _dbHelper.GetAgeName( container.AgeId ) );
        }
        
        private Gender GetGender( Offer offer )
        {
            var container = GetFromCache( offer.CategoryId, offer.ShopId );
            return container == null || container.GenderId == -1
                ? offer.Gender
                : GenderHelper.GetByName( _dbHelper.GetSexName( container.GenderId ) );
        }

        private AgeGenderForCategoryContainer GetFromCache( string categoryId, int shopId )
        {
            if( _cache == null ) {
                FillCache( shopId );
            }
            return _cache.ContainsKey( categoryId ) ? _cache[ categoryId ] : null;
        }
    }
}