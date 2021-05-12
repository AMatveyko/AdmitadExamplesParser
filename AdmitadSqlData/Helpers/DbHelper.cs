// a.snegovoy@gmail.com

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

using AdmitadCommon;
using AdmitadCommon.Entities;
using AdmitadCommon.Extensions;
using AdmitadCommon.Helpers;

using AdmitadSqlData.Entities;
using AdmitadSqlData.Repositories;

namespace AdmitadSqlData.Helpers
{
    public static class DbHelper
    {
        
        #region Repository
        private static ShopRepository _shopRepository = new();
        private static CountryRepository _countryRepository = new();
        private static CategoryRepository _categoryRepository = new();
        private static TagRepository _tagRepository = new();
        private static TheStoreRepository _theStoreRepository = new();
        #endregion
        
        private static readonly ConcurrentDictionary<string, int> ShopIdCache = new();
        private static Dictionary<int, string[]> CountriesCache;
        private static Dictionary<string, string> BrandCache;
        private static Dictionary<string, UnknownBrands> UnknownBrands = new();
        private static bool UnknownBrandsNeedClean = true;

        public static List<SettingsOption> GetSettingsOptions() =>
            _theStoreRepository.GetSettingsOptions().Select( Convert ).ToList();

        public static List<ShopStatistics> GetShopStatisticsList()
        {
            var raw = _theStoreRepository.GetShopStatistics();
            var converted = raw.Select( Convert ).ToList();
            var shops = _shopRepository.GetEnableShops();
            foreach( var stat in converted ) {
                var shop = shops.FirstOrDefault( s => s.ShopId == stat.ShopId );
                if( shop == null ) {
                    continue;
                }

                stat.ShopName = shop.Name;
            }

            return converted;
        }
        
        public static ShopStatistics GetShopStatistics( int shopId )
        {
            return Convert( _theStoreRepository.GetShopStatistics( shopId ) );
        }

        public static void UpdateShopStatistics( ShopStatistics statistics )
        {
            _theStoreRepository.UpdateShopStatistics( statistics, DateTime.Now );
        }
        
        public static int GetUnknownBrandsCount()
        {
            return UnknownBrands.Count;
        }

        public static void UpdateProductsByCategory(
            Category category,
            int before,
            int after )
        {
            _theStoreRepository.UpdateProductsByCategory( category, before, after );
        }
        
        public static void RememberVendorIfUnknown( string cleanName )
        {
            if( UnknownBrandsNeedClean ) {
                _theStoreRepository.ClearUnknownBrands();
                UnknownBrandsNeedClean = false;
            }

            if( cleanName == null || cleanName.Trim() == string.Empty ) {
                cleanName = "NoBrandName";
            }

            var brandId = GetBrandId( cleanName );
            if( brandId != Constants.UndefinedBrandId ) {
                return;
            }

            if( UnknownBrands.ContainsKey( cleanName ) == false ) {
                UnknownBrands[ cleanName ] =  new UnknownBrands { Name = cleanName, NumberOfProducts = 0 };
            }

            UnknownBrands[ cleanName ].NumberOfProducts++;
            
        }

        public static void WriteUnknownBrands()
        {
            _theStoreRepository.AddUnknownBrands(
                UnknownBrands.Where( b => b.Value.NumberOfProducts > 50 )
                    .Select( b => b.Value ) );
        }
        
        public static string GetBrandId( string clearlyName )
        {
            if( BrandCache == null ) {
                BrandCache = new();
                var brands = _theStoreRepository.GetBrands();

                foreach( var brandDb in brands ) {
                    if( BrandCache.ContainsKey( brandDb.ClearlyName ) == false ) {
                        BrandCache[ brandDb.ClearlyName ] = brandDb.Id.ToString();
                    }
                    if( brandDb.SecondClearlyName.IsNotNullOrWhiteSpace() &&
                        BrandCache.ContainsKey( brandDb.SecondClearlyName ) == false ) {
                        BrandCache[ brandDb.SecondClearlyName ] = brandDb.Id.ToString();
                    }
                }
                
            }

            return BrandCache.ContainsKey( clearlyName ) ? BrandCache[ clearlyName ] : Constants.UndefinedBrandId;
        }
        
        public static List<Tag> GetTags()
        {
            var categories = GetDictionaryCategory();
            return _tagRepository.GetTags().Select( t => Convert( t, categories ) ).ToList();
        }

        public static List<XmlFileInfo> GetEnableShops() =>
            _shopRepository.GetEnableShops();

        public static XmlFileInfo GetShop( int id ) {
            var shop = _shopRepository.GetShop( id );
            return new XmlFileInfo( shop.Name, shop.NameLatin, shop.XmlFeed, shop.Id );
        }

        public static int GetShopId(
            string shopNameLatin ) =>
            GetFromCache( ShopIdCache, shopNameLatin, _shopRepository.GetShopId );

        public static int GetCountryId( string countryName ) {
            if( CountriesCache == null ) {
                FillCountryCache();
            }

            return GetCountryIdFromCache( countryName );
        }

        public static List<Category> GetCategories() {
            var categories = _categoryRepository.GetEnabledCategories().Select( Convert ).ToList();
            return categories;
        }

        public static List<Category> GetAllCategories()
        {
            return _categoryRepository.GetAllCategories().Select( Convert ).ToList();
        }
        
        public static List<Category> GetCategoryChildren( int categoryId, Dictionary<int, List<CategoryDb>> allCategories = null )
        {
            var categories = allCategories ?? GetDictionaryCategory();
            return DoGetCategoryChildren( categoryId, categories ).Select( Convert ).ToList();
        }

        private static Dictionary<int, List<CategoryDb>> GetDictionaryCategory() =>
            _categoryRepository.GetAllCategories().GroupBy( c => c.ParentId ).ToDictionary( g => g.Key, g => g.ToList() );
        
        private static List<CategoryDb> DoGetCategoryChildren( int categoryId, Dictionary<int,List<CategoryDb>> allCategory )
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

        public static void ExcludeSearchField( string name )
        {
            _categoryRepository.ExcludeSearchField( name );
        }
        
        public static List<ColorProperty> GetColors() => _theStoreRepository.GetColors().Select( Convert ).ToList();

        public static List<MaterialProperty> GetMaterials() =>
            _theStoreRepository.GetMaterials().Select( Convert ).ToList();

        public static List<SizeProperty> GetSizes() => _theStoreRepository.GetSizes().Select( Convert ).ToList();

        public static void UpdateTags()
        {
            _tagRepository.AddDescriptionField();
        }

        public static void DeleteWordFromTag(
            string word,
            int categoryId )
        {
            _tagRepository.DeleteWordFromTagSearch( word, categoryId );
        }

        #region Convert

        private static ColorProperty Convert( ColorDb colorDb ) =>
            new () {
                Id = colorDb.Id.ToString(),
                ParentId = colorDb.ParentId.ToString(),
                Names = NotEmptyStringsToList(
                    colorDb.Name,
                    colorDb.Name2,
                    colorDb.Name3,
                    colorDb.Name4,
                    colorDb.SynonymName )
            };

        private static MaterialProperty Convert(
            SostavDb sostavDb ) =>
            new() {
                Id = sostavDb.Id.ToString(),
                Names = NotEmptyStringsToList( sostavDb.Name, sostavDb.Name2, sostavDb.Name3, sostavDb.SynonymName )
            };

        private static SizeProperty Convert(
            SizeDb sizeDb ) =>
            new() {
                Id = sizeDb.Id.ToString(),
                Names = NotEmptyStringsToList( sizeDb.Name, sizeDb.SynonymName )
            };

        private static Tag Convert( TagDb tagDb, Dictionary<int,List<CategoryDb>> allCategories )
        {
            var tag = new Tag();
            tag.Id = tagDb.Id.ToString();
            tag.Fields = SplitComa( tagDb.SearchFields );
            tag.SearchTerms = CreateTerms( tagDb.Name );
            tag.Gender = GenderHelper.ConvertFromTag( tagDb.Pol );
            tag.IdCategory = tagDb.IdCategory;
            tag.SpecifyWords = SplitComa( tagDb.SpecifyWords );
            tag.ExcludePhrase = CreateTerms( tagDb.ExcludePhrase );
            tag.Title = tagDb.NameTitle;
            var categories = GetCategoryChildren( tagDb.IdCategory, allCategories ).Select( c => c.Id ).ToList();
            categories.Add( tag.IdCategory.ToString() );
            tag.Categories = categories.ToArray();
            return tag;
        }
        
        private static Category Convert( CategoryDb fromDb ) =>
            new Category {
                Id = fromDb.Id.ToString(),
                Age = AgeHelper.Convert( fromDb.Age ),
                ExcludeTerms = CreateTerms( fromDb.SearchExclude ),
                Fields = SplitComa( fromDb.Fields ),
                Gender = fromDb.Gender,
                Terms = CreateTerms( fromDb.Search ),
                ExcludeWordsFields = fromDb.ExcludeWordsFields.IsNotNullOrWhiteSpace()
                    ? SplitComa( fromDb.ExcludeWordsFields )
                    : SplitComa( fromDb.Fields ),
                Name = fromDb.Name,
                NameH1 = fromDb.NameH1,
                SearchSpecify = CreateTerms( fromDb.SearchSpecify )
            };

        private static SettingsOption Convert( OptionDb optionDb ) =>
            new SettingsOption {
                Option = optionDb.Option,
                Value = optionDb.Value
            };

        private static ShopStatistics Convert(
            ShopStatisticsDb statisticsDb ) =>
            new ShopStatistics {
                ShopId = statisticsDb.ShopId,
                Error = statisticsDb.Error,
                SoldoutAfter = statisticsDb.SoldoutAfter,
                SoldoutBefore = statisticsDb.SoldoutBefore,
                TotalAfter = statisticsDb.TotalAfter,
                TotalBefore = statisticsDb.TotalBefore
            };
        
        #endregion

        #region Routin

        private static List<string> NotEmptyStringsToList(
            params string[] strings ) =>
            strings.Where( s => s.IsNotNullOrWhiteSpace() ).Select( CreateTerm ).ToList();
        
        private static string[] CreateTerms( string terms )
        {
            return SplitComa( terms ).Select( CreateTerm ).ToArray();
        }
        
        private static string CreateTerm( string term )
        {
            return $"\"{term}\"";
        }
        
        private static string[] SplitComa( string input ) =>
            input?.Split( ',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries )
            ?? Array.Empty<string>();
        
        private static int GetCountryIdFromCache( string countryName ) {

            if( countryName.IsNullOrWhiteSpace() ) {
                return Constants.UndefinedCountryId;
            }
            
            foreach( var (key, value) in CountriesCache ) {
                if( value.Contains( countryName.ToLower() ) ) {
                    return key;
                }
            }

            return Constants.UndefinedCountryId;
        }
        
        private static void FillCountryCache() {
            CountriesCache = new Dictionary<int, string[]>();
            var countries = _countryRepository.GetAllCountries();
            foreach( var country in countries ) {
                var synonyms = new List<string>();
                FillSynonyms( synonyms, country.From );
                FillSynonyms( synonyms, country.Name );
                FillSynonyms( synonyms, country.Synonym );
                FillSynonyms( synonyms, country.LatinName );
                CountriesCache[ country.Id ] = synonyms.ToArray();
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