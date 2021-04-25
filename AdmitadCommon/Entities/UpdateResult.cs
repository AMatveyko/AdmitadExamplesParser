// a.snegovoy@gmail.com

namespace AdmitadCommon.Entities
{
    public class UpdateResult
    {
        public UpdateResult(
            long total,
            long updated )
        {
            Total = total;
            Updated = updated;
        }
        
        public long Total { get; }
        public long Updated { get; }

        public string Pretty => Updated < Total
            ? $"{Updated} ({Total - Updated} !)"
            : $"{Updated}";

        public int GetDifferencePercent( UpdateResult otherResult )
        {
            var newCount = ( double ) otherResult.Updated;
            var oldCount = ( double ) Updated;
            var result = ( newCount - oldCount ) / oldCount * 100;
            return ( int ) result;
        }
    }
}