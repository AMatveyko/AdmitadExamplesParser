// a.snegovoy@gmail.com

namespace WorkWithTags
{
    internal static class Helper
    {
        public static string ToUpperFirstLetter( string text ) => char.ToUpper( text[ 0 ] ) + text.Substring( 1 );
    }
}