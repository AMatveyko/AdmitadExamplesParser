// a.snegovoy@gmail.com

namespace Common.Api
{
    public sealed class IndexAllShopsContext : ParallelBackgroundContext
    {
        public IndexAllShopsContext()
            : base( GetCollectedId( nameof(IndexAllShopsContext), "All" ), "Index all shops" ) { }
    }
}