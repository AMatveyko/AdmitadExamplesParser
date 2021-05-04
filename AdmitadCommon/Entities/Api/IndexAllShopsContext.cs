// a.snegovoy@gmail.com

namespace AdmitadCommon.Entities.Api
{
    public sealed class IndexAllShopsContext : ParallelBackgroundContext
    {
        public IndexAllShopsContext()
            : base( GetCollectedId( nameof(IndexAllShopsContext), "All" ), "Index all shops" ) { }
    }
}