﻿// a.snegovoy@gmail.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using Admitad.Converters;
using Admitad.Converters.Workers;

using AdmitadSqlData.Helpers;

using Common.Api;
using Common.Entities;
using Common.Helpers;
using Common.Workers;

using NUnit.Framework;

namespace AdmitadExamplesParserTests
{
    public sealed class ParsingTests
    {

        private static readonly Regex BadParams = new ( @"[^0-9\w]", RegexOptions.Compiled );
        private static readonly Regex DefineCountry12Storeez = new ( @"Сделано в (?<country>[a-zA-Zа-яА-Я0-9_]+)\.", RegexOptions.Compiled );
        private static readonly Regex Digits = new ( @"\d+", RegexOptions.Compiled );
        private static readonly Regex StartRange = new("(рождения|для новорожденных)", RegexOptions.Compiled);
        private static readonly Regex Month =
            new( @"(месяца|месяцев|месяц|months|month|мес(\.|\+)?)", RegexOptions.Compiled );
        private static readonly Regex Years =
            new( @"(года|год|years|лет|л|г|\+)", RegexOptions.Compiled );
        private static readonly Regex Split = new( "( от |-)", RegexOptions.Compiled );
        private static readonly Regex FromYear = new(@"от\s+года", RegexOptions.Compiled);
        private static readonly Regex ToYear = new(@"до\s+года", RegexOptions.Compiled);

        // [ TestCase( "lamoda" ) ]
        // [ TestCase( "adidas" ) ]
        // [ TestCase( "asos" ) ]
        // [ TestCase( "yoox" ) ]
        // [ TestCase( "12storeez" ) ]
        // [ TestCase( "akusherstvo" ) ]
        // [ TestCase( "amersport" ) ]
        // [ TestCase( "anabel" ) ]
        // [ TestCase( "brandshop" ) ]
        // [ TestCase( "vipavenue" ) ]
        // [ TestCase( "gloriajeans" ) ]
        // [ TestCase( "tamaris" ) ]
        // [ TestCase( "laredoute" ) ]
        // [ TestCase( "gullivermarket" ) ]
        // [ TestCase( "svmoscow" ) ]
        // [ TestCase( "intimshop" ) ]
        // [ TestCase( "yoins" ) ]
        // [ TestCase( "goldenline" ) ]
        [ TestCase( "dochkisinochki" ) ]
        public void ParsingTest( string shopName )
        {
            DoParsing( shopName );
        }

        [ Test ]
        public void GenderTest()
        {
            var str = "женская одежда\\\\брюки";
            var gender = GenderHelper.GetGender( new[] {str} );
        } 
        
        private static void DoParsing(
            string shopName )
        {

            var dbSettings = SettingsBuilder.GetDbSettings();
            var dbHelper = new DbHelper( dbSettings );
            
            var downloadInfo = new DownloadInfo( new XmlFileInfo( "n", shopName, "n", 0, 0, 1, null ) ) {
                FilePath = $@"g:\admitadFeeds\{ shopName }.xml",
                ShopName = shopName
            };
            
            var shopData = ParseFile( downloadInfo, false );
            var sortedRawOffers = shopData.NewOffers.OrderBy( o => o.OldPrice ).ToList();
            var unique = GetUnique( shopData.NewOffers, shopName );
            var offers = ConvertOffers( shopData );
            var years = offers.SelectMany( o => o.Params ).Where( p => p.Unit.ToLower() == "Years" )
                .SelectMany( p => p.Values ).ToList();
            var sortedOffers = offers.OrderBy( o => o.OldPrice ).ToList();
            var countryIds = offers.Select( o => o.CountryId ).Distinct().ToList();
            var genders = offers.Select( o => o.Gender ).Distinct().ToList();
            var ages = offers.Select( o => o.Age ).Distinct().ToList();
            var undAge = offers.Where( o => o.Age == Age.Undefined ).ToList();
            var undGender = offers.Where( o => o.Gender == Gender.Undefined ).ToList();
            var undAll = offers.Where( o => o.Gender == Gender.Undefined && o.Age == Age.Undefined ).ToList();
            var childs = offers.Where( o => o.Age == Age.Child ).ToList();
            var allOffers = offers.Count;
            var emptyParams = offers.Where( o => o.Params.Count == 0 ).ToList();
            var oneParams = offers.Where( o => o.Params.Count == 1 ).ToList();
            var products = new ProductConverter( dbHelper, new RatingCalculation( 0 ) ).GetProducts( offers );
            var clothesCount = products.Count( p => p.CategoryName.ToLower().Contains( "рюкзаки" ) );
            var sorterProducts = products.OrderBy( p => p.OldPrice ).ToList();
            Console.WriteLine( offers.Count );
        }

        //Сделано в (?<country>.+)\.
        
        private sealed class ShopUnique
        {
            public string GendersFromParam { get; set; }
            public string AgeFromParam { get; set; }
            public string AgeFromParam2 { get; set; }
            public List<string> CategoryPaths { get; set; }
            public List<string> Countries { get; set; }
            public List<string> Sizes { get; set; }
        }
        
        private static ShopUnique GetUnique( List<RawOffer> offers, string shopName )
        {

            var result = new ShopUnique {
                GendersFromParam = string.Join(
                    ",",
                    offers.SelectMany( o => o.Params.Where( p => p.Name.ToLower() == "пол" ) ).Select( p => p.Value )
                        .Distinct() ),
                AgeFromParam = GetUniqueWithoutDigits( offers, "возраст" ),
                AgeFromParam2 = GetUniqueWithoutDigits( offers, "возраст ребенка" ),
                Sizes = offers.SelectMany( o => o.Params ).Select( p => $"{p.Name} {p.UnitFromXml} {p.Value}").Distinct().ToList(),
                CategoryPaths = offers.Select( o => o.CategoryPath ).Distinct().ToList(),
                Countries = offers.Select( GetCountryGetter( shopName ) ).Distinct().ToList()
            };



            return result;
        }
        
        private static string GetUniqueWithoutDigits(
            List<RawOffer> offers, string paramName ) =>
            string.Join(
                ",",
                offers.SelectMany( o => o.Params.Where( p => p.Name.ToLower() == paramName ) )
                    .Select( p => $"{p.UnitFromXml} {p.Value}{( p.Value == "да" ? p.Name : string.Empty)}" )
                    .Select( p => p.ToLower() )
                    .Select( p => FromYear.Replace( p, "от 1 года") )
                    .Select( p => ToYear.Replace( p, "до 1 года") )
                    .Select( p => StartRange.Replace( p, "0" ) )
                    .Select( p => Month.Replace( p, "months" ) )
                    .Select( p => Years.Replace( p, "years" ) )
                    .Select( p => Split.Split( p ) )
                    //.Select( p => Digits.Replace( p, "X" ) )
                    .Distinct() );

        private static Func<RawOffer, string> GetCountryGetter(
            string shopName )
        {
            return shopName switch {
                "12storeez" => GetCountry12Storeez,
                _ => (
                    o ) => string.Empty
            };
        }

        private static string GetCountry12Storeez( RawOffer offer )
        {
            if( offer.Description == null ) {
                return string.Empty;
            }
            var m = DefineCountry12Storeez.Match( offer.Description );
            return m.Success ? m.Groups[ "country" ].Value : string.Empty;
        }
        
        private static void GetUnique( List<Offer> offers )
        {
            var genders = offers.SelectMany( o => o.Params.Where( p => p.Name.ToLower() == "пол" ) )
                .SelectMany( p => p.Values ).Distinct().ToList();
            var age = offers.SelectMany( o => o.Params.Where( p => p.Name.ToLower() == "возраст" ) )
                .SelectMany( p => p.Values ).Distinct().ToList();
            var badParams = offers.SelectMany( o => o.Params.Where( p => p.Values.Any( v => BadParams.IsMatch( v ) ) ) )
                .SelectMany( p => p.Values ).Distinct();

            //var offer = offers.Where( o => o.Params.Any( p => p.Value.Contains( "Тяжелая" ) ) ).ToList();
            Console.WriteLine( $"Gender from param: {string.Join( ",", genders )}" );
            Console.WriteLine( $"Age from param: {string.Join( ",", age )}" );
            Console.WriteLine( $"BadParams: {string.Join( ",", badParams )}" );
        }
        
        private static List<Offer> ConvertOffers(
            IShopDataWithNewOffers shopData ) {
            var dbSettings = SettingsBuilder.GetDbSettings();
            var dbHelper = new DbHelper( dbSettings );
            var converter = new OfferConverter( shopData,  dbHelper, new BackgroundBaseContext("1", "name" ) );
            return converter.GetCleanOffers();
        }
        
        private static IShopDataWithNewOffers ParseFile( DownloadInfo fileInfo, bool enableExtendedStat ) {
            var parser = new GeneralParser(
                //fileInfo.FilePath,
                //fileInfo.ShopName,
                fileInfo,
                new BackgroundBaseContext("1", "name" ),
                enableExtendedStat );
            return parser.Parse();
        }
    }
}