// a.snegovoy@gmail.com

using System.Collections.Generic;
using System.Text.RegularExpressions;
using Common.Extensions;

namespace Common.Helpers
{
    public static class BrandHelper
    {
        
        private static readonly Regex VendorCleaner = new Regex( @"[^a-zA-Zа-яА-Я0-9]", RegexOptions.Compiled );
        
        public static string GetClearlyVendor( string vendor )
        {
            // return VendorCleaner.Replace( vendor ?? string.Empty, "" ).ToLower();

            if( vendor.IsNullOrWhiteSpace() ) {
                return string.Empty;
            }

            vendor = vendor.ToLower();
            
            foreach( var replace in _replaceDictionary ) {
                vendor = vendor.Replace( replace.Key, replace.Value );
            }

            vendor = VendorCleaner.Replace( vendor, string.Empty );

            vendor = vendor.Replace( " ", string.Empty );

            return vendor;
        }
        
        private static Dictionary<string, string> _replaceDictionary = new Dictionary<string, string>() {
            // соблюдать последовательность пока не будет 100% понимания
            {"&gt;", " "},
            {"&lt;", " "},
            {"&amp;", " "},
            {"&apos;", " "},
            {"&#039;", " "},
            {"&quot;", " "},
            {"&", string.Empty},
            {" and ", string.Empty},
            {"%", string.Empty},
            {"#", string.Empty},
            {".", string.Empty},
            {",", string.Empty},
            {"-", string.Empty},
            {"+", string.Empty},
            {"(", string.Empty},
            {")", string.Empty},
            {"/", string.Empty},
            {"!", string.Empty},
            {"?", string.Empty},
            {"`", string.Empty},
            {"’", string.Empty},
            {"\"", string.Empty},
            {"'", string.Empty},
            {"™", string.Empty},
            {"®", string.Empty},
            {"ё","e"},
            {"Ë","e"},
            {"ê","e"},
            {"é","e"},
            {"è","e"},
            {"É","e"},
            {"Ē","e"},
            {"ü","u"},
            {"ú","u"},
            {"ù","u"},
            {"ã",""},
            {"â","a"},
            {"ä","a"},
            {"á","a"},
            {"à","a"},
            {"Å","a"},
            {"å","a"},
            {"ò","o"},
            {"ó","o"},
            {"ø","o"},
            {"ô","o"},
            {"ö","o"},
            {"ō","o"},
            {"Ọ","o"},
            {"ç","c"},
            {"ñ","n"},
            {"Ð","d"},
            {"ï","i"},
            {"ì","i"},
            {"Í","i"}
        };
    }
}