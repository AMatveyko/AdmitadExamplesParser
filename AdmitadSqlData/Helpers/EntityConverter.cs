// a.snegovoy@gmail.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using AdmitadCommon;
using AdmitadCommon.Entities.Statistics;

using AdmitadSqlData.Entities;

using Common;
using Common.Entities;
using Common.Entities.ForElastic;
using Common.Extensions;
using Common.Helpers;
using Common.Settings;

namespace AdmitadSqlData.Helpers
{
    internal static class EntityConverter
    {
        private static readonly Regex _categoryName = new(@"([^a-zA-zа-яА-Я\d\s])", RegexOptions.Compiled);
        
        #region Convert

        public static AgeGenderForCategoryContainer Convert( ShopCategoryDb categoryDb ) =>
            new ( categoryDb.CategoryId, categoryDb.AgeId ?? -1, categoryDb.SexId ?? -1 );
        
        public static ShopCategoryDb Convert(
            int shopId,
            ShopCategory category )
        {
            return new() {
                CategoryId = category.Id ?? string.Empty,
                ParentId = category.ParentId ?? string.Empty,
                Name = PrepareCategoryName( category.Name ),
                UpdateDate = DateTime.Now,
                ShopId = shopId
                // WomenProductsNumber = category.WomenProductsNumber,
                // MenProductsNumber = category.MenProductsNumber,
                // TotalProductsCount = category.TotalProductsNumber
            };
        }

        public static string PrepareCategoryName(
            string data )
        {
            return _categoryName.Replace( data ?? string.Empty, string.Empty );
        }

        public static XmlFileInfo Convert(
            Shop shop ) =>
            new (
                shop.Name,
                shop.NameLatin,
                shop.XmlFeed,
                shop.Id,
                shop.Weight,
                shop.VersionProcessing,
                shop.UpdateDate );
        
        public static ColorProperty Convert(
            ColorDb colorDb )
        {
            var color = new ColorProperty {
                Id = colorDb.Id.ToString(),
                Name = colorDb.Name,
                LatinName = colorDb.LatinName,
                ParentId = colorDb.ParentId.ToString(),
                Names = NotEmptyStringsToList(
                    CombineSearchWords(
                        colorDb.SynonymName,
                        colorDb.Name,
                        colorDb.Name2,
                        colorDb.Name3,
                        colorDb.Name4 ) )
            };
            return FillSearchNames( color, colorDb );
        }

        public static MaterialProperty Convert(
            SostavDb sostavDb )
        {
            var material = new MaterialProperty {
                Id = sostavDb.Id.ToString(),
                Names = NotEmptyStringsToList(
                    CombineSearchWords( sostavDb.SynonymName, sostavDb.Name, sostavDb.Name2, sostavDb.Name3 ) )
            };

            return FillSearchNames( material, sostavDb );
        }

        private static T FillSearchNames< T >(
            T entity,
            ISearchNamesDb entityFromDb )
            where T : ISearchNames
        {
            entity.SearchNames = CreateTermsList( entityFromDb.SearchNames );
            return entity;
        }

        public static SizeProperty Convert(
            SizeDb sizeDb )
        {
            return new() {
                Id = sizeDb.Id.ToString(),
                Names = NotEmptyStringsToList( CombineSearchWords( sizeDb.SynonymName, sizeDb.Name ) ),
                // TODO> неоднозначное поведение, берем поисковые слова неиз того поля. подсмотреть у цвета и у состава
                SearchNames = new List<string> {
                    sizeDb.Name
                }
            };
        }

        public static Tag Convert( TagDb tagDb, List<Category> children )
        {
            var tag = new Tag();
            tag.Id = tagDb.Id.ToString();
            tag.Fields = SplitComa( tagDb.SearchFields );
            tag.SearchTerms = CreateTerms( tagDb.Name );
            tag.Gender = GenderHelper.ConvertFromTag( tagDb.Pol );
            tag.IdCategory = tagDb.IdCategory;
            tag.SpecifyWords = CreateTerms( tagDb.SpecifyWords );
            tag.ExcludePhrase = CreateTerms( tagDb.ExcludePhrase );
            tag.Title = tagDb.NameTitle;
            //var categories = GetCategoryChildren( tagDb.IdCategory, allCategories ).Select( c => c.Id ).ToList();
            var categories = children.Select( c => c.Id ).ToList();
            categories.Add( tag.IdCategory.ToString() );
            tag.Categories = categories.ToArray();
            return tag;
        }

        public static Category Convert(
            CategoryDb fromDb )
        {
            return new() {
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
                SearchSpecify = CreateTerms( fromDb.SearchSpecify ),
                TakeUnisex = fromDb.TakeUnisex
            };
        }

        public static SettingsOption Convert(
            OptionDb optionDb )
        {
            return new() {
                Option = optionDb.Option,
                Value = optionDb.Value
            };
        }

        public static ParseLog Convert(
            IShopStatisticsForDb statistics )
        {
            return new() {
                Date = statistics.StartDownloadFeed,
                ShopId = statistics.ShopId,
                FileSize = statistics.FileSize,
                OfferCount = statistics.OfferCount,
                SoldOutOfferCount = statistics.SoldOutOfferCount,
                CategoryCount = statistics.CategoryCount,
                DownloadTime = statistics.DownloadTime
            };
        }

        public static ShopStatisticsDb Convert( ShopProduct statistics )
        { return new() {
                ShopId = statistics.ShopId,
                SoldoutCount = statistics.SoldoutCount,
                TotalCount = statistics.TotalCount,
                UpdateDate = DateTime.Now
            };
        }

        public static ShopProduct Convert( ShopStatisticsDb statisticsDb )
        {
            return new() {
                ShopId = statisticsDb.ShopId,
                TotalCount = statisticsDb.TotalCount,
                SoldoutCount = statisticsDb.SoldoutCount,
                UpdateDate = statisticsDb.UpdateDate
            };
        }

        public static Country Convert(
            CountryDb data )
        {
            return new() {
                Id = data.Id,
                Name = data.Name,
                SearchTerms = CreateTerms( data.SearchTerms )
            };
        }
        #endregion
        
        #region Routin
        private static string[] CombineSearchWords(
            string comaSeparated,
            params string[] searchWords )
        {
            var terms = CreateTerms( comaSeparated );
            return terms.Concat( searchWords ).ToArray();
        }

        private static List<string> NotEmptyStringsToList(
            params string[] strings )
        {
            return strings.Where( s => s.IsNotNullOrWhiteSpace() ).Select( CreateTerm ).ToList();
        }

        private static List<string> CreateTermsList(
            string terms )
        {
            return CreateTerms( terms ).ToList();
        }

        private static string[] CreateTerms(
            string terms )
        {
            return SplitComa( terms ).Select( CreateTerm ).ToArray();
        }

        private static string CreateTerm(
            string term )
        {
            return $"\"{term}\"";
        }

        private static string[] SplitComa(
            string input )
        {
            return input?.Split( ',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries ) ??
                   Array.Empty<string>();
        }
        #endregion
    }
}