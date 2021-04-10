// a.snegovoy@gmail.com

namespace AdmitadExamplesParser.Entities
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
    }
}