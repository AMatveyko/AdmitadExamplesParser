// a.snegovoy@gmail.com

using System.Linq;

using Common.Entities;

using HtmlAgilityPack;

namespace ShopsParser.Parsers
{
    internal sealed class VipAvenuCategoryParser : IParser
    {
        public (Age, Gender) Parse( string data )
        {
            var doc = new HtmlDocument();
            doc.LoadHtml( data );
            var text = doc.DocumentNode.SelectNodes("html/body/main/div").First().InnerText;

            return DetermineAgeAndGender( text );
        }

        private static (Age, Gender) DetermineAgeAndGender( string text )
        {
            if( text.Contains( "Женская" ) ) {
                return ( Age.Adult, Gender.Woman );
            }

            if( text.Contains( "Мужская" ) ) {
                return ( Age.Adult, Gender.Man );
            }

            if( text.Contains( "для девочек" ) ) {
                return ( Age.Child, Gender.Woman );
            }

            if( text.Contains( "для мальчиков" ) ) {
                return ( Age.Child, Gender.Man );
            }

            if( text.Contains( "для детей" ) ) {
                return ( Age.Child, Gender.Unisex );
            }

            return ( Age.Undefined, Gender.Undefined );
        }
    }
}