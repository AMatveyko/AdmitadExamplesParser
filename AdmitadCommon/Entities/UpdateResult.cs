// a.snegovoy@gmail.com

using Nest;

namespace AdmitadCommon.Entities
{
    public sealed class UpdateResult
    {
        public UpdateResult(
            long total,
            long updated )
        {
            Total = total;
            Updated = updated;
        }

        public UpdateResult( UpdateByQueryResponse response )
        {
            Total = response.Total;
            Updated = response.Updated;
        }
        
        public long Total { get; }
        public long Updated { get; }

        public bool IsError => Updated < Total;
        
        public string Pretty => IsError
            ? $"{Updated} ({Total - Updated} !)"
            : $"{Updated}";

        public int GetDifferencePercent( UpdateResult otherResult )
        {
            if( Updated == 0 ) {
                return 100;
            }
            var newCount = ( double ) otherResult.Updated;
            var oldCount = ( double ) Updated;
            var result = ( newCount - oldCount ) / oldCount * 100;
            return ( int ) result;
        }

        public override string ToString()
        {
            return Pretty;
        }
    }
}