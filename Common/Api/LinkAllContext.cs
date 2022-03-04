// a.snegovoy@gmail.com

namespace Common.Api
{
    public sealed class LinkAllContext : ParallelBackgroundContext
    {
        public LinkAllContext()
            : base( GetCollectedId( nameof(LinkAllContext) ), nameof(LinkAllContext) ) { }
    }
}