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

        public static int GetUnknownBrandsCount()
        {
            return UnknownBrands.Count;
        }

        public static void RememberVendorIfUnknown( string vendorName, string cleanName )
        {
            if( UnknownBrandsNeedClean ) {
                _theStoreRepository.ClearUnknownBrands();
                UnknownBrandsNeedClean = false;
            }

            if( vendorName == null || vendorName.Trim().IsNullOrWhiteSpace() ) {
                vendorName = "NoBrandName";
            }

            var brandId = GetBrandId( cleanName );
            if( brandId != Constants.UndefinedBrandId ) {
                return;
            }

            if( UnknownBrands.ContainsKey( vendorName ) == false ) {
                UnknownBrands[ vendorName ] =  new UnknownBrands { Name = vendorName, NumberOfProducts = 0 };
            }

            UnknownBrands[ vendorName ].NumberOfProducts++;
            
        }

        public static void WriteUnknownBrands()
        {
            _theStoreRepository.AddUnknownBrands( UnknownBrands.Select( b => b.Value ) );
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
            return _tagRepository.GetTags().Select( Convert ).ToList();
        }

        public static List<XmlFileInfo> GetEnableShops() =>
            _shopRepository.GetEnableShops();

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
            var categories = _categoryRepository.GetCategories().Select( Convert ).ToList();
            //categories = categories.Where( c => c.Id[ 0 ] == '1' || c.Id[ 0 ] == '2' ).ToList();
            return categories;
        }

        public static List<ColorProperty> GetColors() => _theStoreRepository.GetColors().Select( Convert ).ToList();

        public static List<MaterialProperty> GetMaterials() =>
            _theStoreRepository.GetMaterials().Select( Convert ).ToList();

        public static List<SizeProperty> GetSizes() => _theStoreRepository.GetSizes().Select( Convert ).ToList();

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

        private static Tag Convert( TagDb tagDb )
        {
            var tag = new Tag();
            tag.Id = tagDb.Id.ToString();
            tag.Fields = SplitComa( tagDb.SearchFields );
            tag.SearchTerms = CreateTerms( tagDb.Name );
            tag.Gender = GenderHelper.ConvertFromTag( tagDb.Pol );
            tag.IdCategory = tagDb.IdCategory;
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
                    : SplitComa( fromDb.Fields )
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