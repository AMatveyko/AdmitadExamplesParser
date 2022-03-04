// a.snegovoy@gmail.com

using System.Collections.Generic;
using System.Linq;

namespace Common.Entities
{
    public class AgeRange
    {

        private decimal _to;

        public const int Min = 0;
        public const int Max = 1200;

        public AgeRange() { }

        public AgeRange( int from = Min, int to = Max ) =>
            ( From, To ) = ( from, to );
        
        public decimal From { get; set; }
        public decimal To
        {
            get => _to;
            set => _to = value > Max ? Max : value;
        }

        public static AgeRange GetFromYear( int years )
        {
            var months = years * 12;
            return new AgeRange( months, months + 11 );
        }

        public static AgeRange GetFromMonths(
            int months ) =>
            new AgeRange( months, months + 1 );

        public static AgeRange GetMaxRange( IEnumerable<AgeRange> ranges )
        {
            var filteredRanges = ranges?.Where( r => r != null ).ToList();
            if( filteredRanges?.Any() == false ) {
                // return new AgeRange { From = Min, To = Max };
                return null;
            }

            return new AgeRange {
                From = filteredRanges.Min( r => r.From ),
                To = filteredRanges.Max( r => r.To )
            };
        }
    }
}