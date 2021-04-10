// a.snegovoy@gmail.com

using System.Linq;
using System.Text.RegularExpressions;

using AdmitadCommon.Entities;
using AdmitadCommon.Extensions;

namespace AdmitadCommon.Helpers
{
    public static class AgeHelper
    {

        private static Regex AdultPattern = new( @"взрослый", RegexOptions.Compiled );
        private static Regex ChildPattern = new( @"(детский|детские|детская|малышей)", RegexOptions.Compiled );

        public static string Convert(
            Age age ) =>
            age switch {
                Age.Adult => "a",
                Age.All => "l",
                Age.Child => "c",
                Age.Undefined => "n",
                _ => "n"
            };

        public static string Convert( int age ) =>
            age switch {
                1 => "a",
                2 => "c",
                _ => "l"
            };

        public static Age GetAge( string line ) {
            if( line.IsNullOrWhiteSpace() ) {
                return Age.Undefined;
            }

            line = line.ToLower();
            
            var patterns = new[] { ( AdultPattern, Age.Adult ), ( ChildPattern, Age.Child ) };

            foreach( var ( pattern, age ) in patterns ) {
                var m = pattern.Match( line );
                if( m.Success ) {
                    return age;
                }
            }

            return Age.Undefined;
        }
    }
}