// a.snegovoy@gmail.com

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

using AdmitadCommon;
using AdmitadCommon.Entities.Statistics;

using AdmitadSqlData.Entities;
using AdmitadSqlData.Repositories;

using Common;
using Common.Entities;
using Common.Extensions;
using Common.Settings;

using Country = AdmitadCommon.Country;

namespace AdmitadSqlData.Helpers
{
    public sealed class DbHelper : ISettingsRepository
    {

        #region Repository
        private readonly ShopRepository _shopRepository;
        private readonly CountryRepository _countryRepository;
        private readonly CategoryRepository _categoryRepository;
        private readonly TagRepository _tagRepository;
        private readonly TheStoreRepository _theStoreRepository;
        #endregion
        
        private static readonly ConcurrentDictionary<string, int> ShopIdCache = new();
        private static Dictionary<int, string[]> _countriesCache;
        private static Dictionary<string, string> _brandCache;
        private static readonly Dictionary<string, UnknownBrands> UnknownBrands = new();
        private static readonly Dictionary<string, int> UnknownCountries = new();
        private static bool _unknownBrandsNeedClean = true;


        public DbHelper( DbSettings settings )
        {
            var connectionString = settings.GetConnectionString();
                ( _shopRepository, _countryRepository, _categoryRepository, _tagRepository, _theStoreRepository ) = (
                new ShopRepository( connectionString, settings.Version ),
                new CountryRepository( connectionString, settings.Version ),
                new CategoryRepository( connectionString, settings.Version ),
                new TagRepository( connectionString, settings.Version ),
                new TheStoreRepository( connectionString, settings.Version ) );
        }

        public void WriteShopStatistics( ShopProcessingStatistics statistics )
        {
            var container = new DbWorkersContainer(
                UpdateShopCategory,
                UpdateShopStatistics,
                WriteShopProcessLog,
                UpdateShopUpdateDate );
            statistics.Write( container );
        }

        private void UpdateShopUpdateDate( int shopDate, DateTime dateTime )
        {
            _shopRepository.UpdateDate( shopDate, dateTime );
        }
        
        private void WriteShopProcessLog( IShopStatisticsForDb statistics )
        {
            _theStoreRepository.WriteShopProcessingLog( EntityConverter.Convert( statistics ) );
        }
        
        public List<SettingsOption> GetSettingsOptions() =>
            _theStoreRepository.GetSettingsOptions().Select( EntityConverter.Convert ).ToList();

        public List<ShopProductsStatistics> GetShopStatisticsList()
        {
            var raw = _theStoreRepository.GetShopStatistics();
            return raw.Select( EntityConverter.Convert )
                .GroupBy( i => i.ShopId )
                .Select( g => GetNeedStatistics( g.ToList() ) )
                .ToList();
        }

        private ShopProductsStatistics GetNeedStatistics(
            List<ShopProduct> statistisc )
        {
            if( statistisc.Any() == false ) {
                return null;
            }
            var sorted = statistisc.OrderByDescending( s => s.UpdateDate ).Take( 2 ).ToList();
            var after = sorted.First();
            var before = sorted.Last();
            return new ShopProductsStatistics( before, after );
        }
        
        private void UpdateShopStatistics( ShopProduct product )
        {
            _theStoreRepository.InsertShopStatistics( EntityConverter.Convert( product ) );
        }
        
        public int GetUnknownBrandsCount()
        {
            return UnknownBrands.Count;
        }

        public void UpdateProductsByCategory(
            Category category,
            int before,
            int after )
        {
            _theStoreRepository.UpdateProductsByCategory( category, before, after );
        }
        
        public void RememberVendorIfUnknown( string cleanName, string originalName )
        {
            if( _unknownBrandsNeedClean ) {
                _theStoreRepository.ClearUnknownBrands();
                _unknownBrandsNeedClean = false;
            }

            if( cleanName == null || cleanName.Trim() == string.Empty ) {
                cleanName = "NoBrandName";
            }

            var brandId = GetBrandId( cleanName );
            if( brandId != Constants.UndefinedBrandId ) {
                return;
            }

            if( UnknownBrands.ContainsKey( cleanName ) == false ) {
                UnknownBrands[ cleanName ] =  new UnknownBrands {
                    Name = cleanName,
                    OriginalName = originalName,
                    NumberOfProducts = 0
                };
            }

            UnknownBrands[ cleanName ].NumberOfProducts++;
            
        }

        public void WriteUnknownBrands()
        {
            _theStoreRepository.AddUnknownBrands(
                UnknownBrands.Where( b => b.Value.NumberOfProducts > 50 )
                    .Select( b => b.Value ) );
        }
        
        public string GetBrandId( string clearlyName )
        {
            if( _brandCache == null ) {
                _brandCache = new();
                var brands = _theStoreRepository.GetBrands();

                foreach( var brandDb in brands ) {
                    if( _brandCache.ContainsKey( brandDb.ClearlyName ) == false ) {
                        _brandCache[ brandDb.ClearlyName ] = brandDb.Id.ToString();
                    }
                    if( brandDb.SecondClearlyName.IsNotNullOrWhiteSpace() &&
                        _brandCache.ContainsKey( brandDb.SecondClearlyName ) == false ) {
                        _brandCache[ brandDb.SecondClearlyName ] = brandDb.Id.ToString();
                    }
                }
                
            }

            return _brandCache.ContainsKey( clearlyName ) ? _brandCache[ clearlyName ] : Constants.UndefinedBrandId;
        }
        
        public List<Tag> GetTags()
        {
            var categories = GetDictionaryCategory();
            return _tagRepository.GetTags().Select( t =>
                EntityConverter.Convert(
                    t,
                    GetCategoryChildren( t.IdCategory, categories ) ) )
                .ToList();
        }

        public List<XmlFileInfo> GetEnableShops() =>
            _shopRepository.GetEnableShops().Select( EntityConverter.Convert ).ToList();

        public XmlFileInfo GetShop( int id ) {
            var shop = _shopRepository.GetShop( id );
            return EntityConverter.Convert( shop );
        }

        public int GetNumberEnabledShops() => _shopRepository.GetEnableShops().Count;

        public int GetShopId(
            string shopNameLatin ) =>
            GetFromCache( ShopIdCache, shopNameLatin, _shopRepository.GetShopId );

        public int GetCountryId( string countryName ) {
            if( _countriesCache == null ) {
                FillCountryCache();
            }

            return GetCountryIdFromCache( countryName );
        }

        public List<Country> GetCountries()
        {
            return _countryRepository.GetAllCountries().Select( EntityConverter.Convert ).ToList();
        }
        
        public void SaveUnknownCountries()
        {
            var newCountries = UnknownCountries.Select(
                c => new UnknownCountry {
                    Name = c.Key,
                    OfferCount = c.Value
                } ).ToList();
            _theStoreRepository.SaveUnknownCountries( newCountries );
        }
        
        public List<Category> GetCategories() {
            var categories = _categoryRepository.GetEnabledCategories().Select( EntityConverter.Convert ).ToList();
            return categories;
        }

        public List<Category> GetAllCategories()
        {
            return _categoryRepository.GetAllCategories().Select( EntityConverter.Convert ).ToList();
        }
        
        public List<Category> GetCategoryChildren( int categoryId, Dictionary<int, List<CategoryDb>> allCategories = null )
        {
            var categories = allCategories ?? GetDictionaryCategory();
            return DoGetCategoryChildren( categoryId, categories ).Select( EntityConverter.Convert ).ToList();
        }

        private Dictionary<int, List<CategoryDb>> GetDictionaryCategory() =>
            _categoryRepository.GetAllCategories().GroupBy( c => c.ParentId ).ToDictionary( g => g.Key, g => g.ToList() );
        
        private List<CategoryDb> DoGetCategoryChildren( int categoryId, Dictionary<int,List<CategoryDb>> allCategory )
        {
            if( allCategory.ContainsKey( categoryId ) == false ) {
                return new List<CategoryDb>();
            }

            var children = allCategory[ categoryId ];
            var newChildren = new List<CategoryDb>();
            foreach( var child in children ) {
                newChildren.AddRange( DoGetCategoryChildren( child.Id, allCategory ) );
            }

            children.AddRange( newChildren );
            
            return children;
        }

        public void ExcludeSearchField( string name )
        {
            _categoryRepository.ExcludeSearchField( name );
        }
        
        public List<ColorProperty> GetColors() => _theStoreRepository.GetColors().Select( EntityConverter.Convert ).ToList();

        public List<MaterialProperty> GetMaterials() =>
            _theStoreRepository.GetMaterials().Select( EntityConverter.Convert ).ToList();

        public List<SizeProperty> GetSizes() => _theStoreRepository.GetSizes().Select( EntityConverter.Convert ).ToList();

        public void UpdateTags()
        {
            _tagRepository.AddDescriptionField();
        }

        public void DeleteWordFromTag(
            string word,
            int categoryId )
        {
            _tagRepository.DeleteWordFromTagSearch( word, categoryId );
        }




        #region OriginalCategory

        private void UpdateShopCategory( string shopName, List<ShopCategory> categories )
        {
            if( categories == null ||
                categories.Any() == false ) {
                return;
            }

            var shopId = GetShopId( shopName );
            
            var categoriesForDb = categories.Select( c => EntityConverter.Convert( shopId, c ) ).ToList();
            _theStoreRepository.UpdateShopCategories( categoriesForDb );

        }
        
        #endregion

        #region Routin
        
        private static int GetCountryIdFromCache( string countryName ) {

            if( countryName.IsNullOrWhiteSpace() ) {
                return Constants.UndefinedCountryId;
            }
            
            foreach( var (key, value) in _countriesCache ) {
                if( value.Contains( countryName.ToLower() ) ) {
                    return key;
                }
            }

            if( UnknownCountries.ContainsKey( countryName ) == false ) {
                UnknownCountries[ countryName ] = 0;
            }
            UnknownCountries[ countryName ]++;

            return Constants.UndefinedCountryId;
        }
        
        private void FillCountryCache() {
            _countriesCache = new Dictionary<int, string[]>();
            var countries = _countryRepository.GetAllCountries();
            foreach( var country in countries ) {
                var synonyms = new List<string>();
                FillSynonyms( synonyms, country.From );
                FillSynonyms( synonyms, country.From.Replace( "из ", string.Empty ) );
                FillSynonyms( synonyms, country.Name );
                FillSynonyms( synonyms, country.Synonym );
                FillSynonyms( synonyms, country.LatinName );
                _countriesCache[ country.Id ] = synonyms.ToArray();
            }
        }

        private static void FillSynonyms( ICollection<string> synonyms, string newValue ) {
            if( newValue.IsNotNullOrWhiteSpace() ) {
                synonyms.Add( newValue.ToLower() );
            }
        }
        
        private static T GetFromCache<T,TO>( IDictionary<TO, T> cache, TO key, Func< TO, T > func ) {
            if( cache.ContainsKey( key ) == false ) {
                var value = func( key );
                cache[ key ] = value;
            }

            return cache[ key ];
        }

        #endregion
    }
}