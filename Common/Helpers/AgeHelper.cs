// a.snegovoy@gmail.com

using System.Linq;
using System.Text.RegularExpressions;

using Common.Entities;
using Common.Extensions;

namespace Common.Helpers
{
    public static class AgeHelper
    {

        private static Regex AdultPattern = new Regex( @"взрослый", RegexOptions.Compiled );
        private static Regex ChildPattern = new Regex( @"(детский|детские|детская|малышей)", RegexOptions.Compiled );

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

        public static Age GetByName(
            string name ) =>
            name switch {
                "Adult" => Age.Adult,
                "Child" => Age.Child,
                "Baby" => Age.Baby,
                _ => Age.Undefined
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