// a.snegovoy@gmail.com

using System.Linq;

namespace AdmitadCommon.Helpers
{
    public static class CategoryHelper
    {
        public static int GetEndCategory( int categoryId )
        {
            var categoryIdArray = categoryId.ToString().ToArray();
            for( var i = categoryIdArray.Length; i > 0; i-- ) {
                if( categoryIdArray[ i - 1 ] != '0' ) {
                    break;
                }
                categoryIdArray[ i - 1 ] = '9';
            }

            var convertedEnd = string.Join( string.Empty, categoryIdArray );

            return int.Parse( convertedEnd );
        }
    }
}