// a.snegovoy@gmail.com

namespace AdmitadCommon.Extensions
{
    public static class StringExtensions
    {
        public static bool IsNullOrWhiteSpace(
            this string input )
        {
            return string.IsNullOrEmpty( input );
        }

        public static bool IsNotNullOrWhiteSpace(
            this string input )
        {
            return input.IsNullOrWhiteSpace() == false;
        }
        
    }
}