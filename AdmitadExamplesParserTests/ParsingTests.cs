// a.snegovoy@gmail.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using Admitad.Converters;

using AdmitadCommon.Entities;
using AdmitadCommon.Helpers;

using AdmitadExamplesParser.Entities;

using AdmitadExamplesParser.Workers.Components;

using NUnit.Framework;

namespace AdmitadExamplesParserTests
{
    public sealed class ParsingTests
    {

        private static readonly Regex BadParams = new ( @"[^0-9\w]", RegexOptions.Compiled );
        private static readonly Regex DefineCountry12Storeez = new ( @"Сделано в (?<country>[a-zA-Zа-яА-Я0-9_]+)\.", RegexOptions.Compiled );

        [ TestCase( "lamoda" ) ]
        [ TestCase( "adidas" ) ]
        [ TestCase( "asos" ) ]
        [ TestCase( "yoox" ) ]
        [ TestCase( "12storeez" ) ]
        [ TestCase( "akusherstvo" ) ]
        [ TestCase( "amersport" ) ]
        [ TestCase( "anabel" ) ]
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
            
            var downloadInfo = new DownloadInfo {
                FilePath = $@"o:\admitad\feeds\{ shopName }.xml",
                ShopName = shopName
            };
            
            var shopData = ParseFile( downloadInfo, false );
            var sortedRawOffers = shopData.Offers.OrderBy( o => o.OldPrice ).ToList();
            var unique = GetUnique( shopData.Offers, shopName );
            var offers = ConvertOffers( shopData );
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
            var products = ProductConverter.GetProductsContainer( offers );
            var sorterProducts = products.OrderBy( p => p.OldPrice ).ToList();
            Console.WriteLine( offers.Count );
        }

        //Сделано в (?<country>.+)\.
        
        private class ShopUnique
        {
            public string GendersFromParam { get; set; }
            public string AgeFromParam { get; set; }
            public List<string> CategoryPaths { get; set; }
            public List<string> Countries { get; set; }
        }
        
        private static ShopUnique GetUnique( List<RawOffer> offers, string shopName )
        {

            var result = new ShopUnique {
                GendersFromParam = string.Join(
                    ",",
                    offers.SelectMany( o => o.Params.Where( p => p.Name.ToLower() == "пол" ) ).Select( p => p.Value )
                        .Distinct() ),
                AgeFromParam = string.Join(
                    ",",
                    offers.SelectMany( o => o.Params.Where( p => p.Name.ToLower() == "возраст" ) )
                        .Select( p => p.Value ).Distinct() ),
                CategoryPaths = offers.Select( o => o.CategoryPath ).Distinct().ToList(),
                Countries = offers.Select( GetCountryGetter( shopName ) ).Distinct().ToList()
            };



            return result;
        }

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
            ShopData shopData ) {
            var converter = new OfferConverter( shopData );
            return converter.GetCleanOffers();
        }
        
        private static ShopData ParseFile( DownloadInfo fileInfo, bool enableExtendedStat ) {
            var parser = new GeneralParser( fileInfo.FilePath, fileInfo.ShopName, enableExtendedStat );
            return parser.Parse();
        }
    }
}