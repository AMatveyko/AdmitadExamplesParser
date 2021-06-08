// a.snegovoy@gmail.com

namespace Common.Helpers
{
    public static class MathHelper
    {
        public static int GetPercent( int current, int total )
        {

            if( total == 0 || current == total ) {
                return 100;
            }

            if( current == 0 ) {
                return 0;
            }
            
            var convertedCurrent = ( double ) current;
            var convertedTotal = ( double ) total;

            var result = ( convertedCurrent / convertedTotal ) * 100;
            return ( int ) result;
        }
    }
}