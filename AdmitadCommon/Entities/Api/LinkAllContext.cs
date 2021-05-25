// a.snegovoy@gmail.com

namespace AdmitadCommon.Entities.Api
{
    public sealed class LinkAllContext : ParallelBackgroundContext
    {
        public LinkAllContext()
            : base( GetCollectedId( nameof(LinkAllContext) ), nameof(LinkAllContext) ) { }
    }
}