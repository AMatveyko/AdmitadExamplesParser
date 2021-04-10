// a.snegovoy@gmail.com

using System.Collections.Generic;
using System.Text.RegularExpressions;

using AdmitadCommon.Entities;
using AdmitadCommon.Extensions;

namespace AdmitadCommon.Helpers
{
    public static class GenderHelper
    {

        private static Regex _womanPattern = new( @"(женщинам|беременных|женский|женская|женские|женское|женщина|женщин|women)", RegexOptions.Compiled );
        private static Regex _menPattern = new( @"(мужчинам|мужской|мужская|мужские|мужское|мужчина|мужчин|men)", RegexOptions.Compiled );
        private static Regex _unisexPattern = new( @"(унисекс|unisex|nisex)", RegexOptions.Compiled );
        private static Regex _childPattern = new( @"(детская|(для )?детей|детям)", RegexOptions.Compiled );
        private static Regex _babyPattern = new( @"(новорожденных|новорожденным|(для )?младенцев|младенцам)", RegexOptions.Compiled );
        private static Regex _girlPattern = new( @"(девочкам|девочки|(для )?девочек|girl)", RegexOptions.Compiled );
        private static Regex _boyPattern = new( @"(мальчикам|мальчики|(для )?мальчиков|boy)", RegexOptions.Compiled );


        public static string ConvertFromTag( string line )
        {
            return line switch {
                "devochki" => "g",
                "muzh" => "m",
                "zhen" => "w",
                _ => "u"
            };
        }
        
        public static string Convert( Gender gender ) =>
            gender switch {
                Gender.Boy => "b",
                Gender.Child => "c",
                Gender.Girl => "g",
                Gender.Man => "m",
                Gender.Unisex => "u",
                Gender.Woman => "w",
                Gender.Undefined => "n",
                _ => "n"
            };

        public static Gender GetGender( IEnumerable<string> lines )
        {
            foreach( var line in lines ) {
                var gender = GetGender( line );
                if( gender != Gender.Undefined ) {
                    return gender;
                }
            }

            return Gender.Undefined;
        }
        
        public static Gender GetGender( string line )
        {

            if( line.IsNullOrWhiteSpace() ) {
                return Gender.Undefined;
            }
            
            var patterns = new[] {
                ( _womanPattern, Gender.Woman ),
                ( _menPattern, Gender.Man ),
                ( _unisexPattern, Gender.Unisex ),
                ( _childPattern, Gender.Child ),
                //( _babyPattern, Gender.Child ),
                ( _girlPattern, Gender.Girl ),
                ( _boyPattern, Gender.Boy )
            };
            foreach( var ( pattern, gender ) in patterns ) {
                var m = pattern.Match( line.ToLower() );
                if( m.Success ) {
                    return gender;
                }
            }

            return Gender.Undefined;
        }
    }
}