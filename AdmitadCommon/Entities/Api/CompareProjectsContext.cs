// a.snegovoy@gmail.com

namespace AdmitadCommon.Entities.Api
{
    public sealed class CompareProjectsContext : BackgroundBaseContext
    {
        public CompareProjectsContext()
            : base( GetCollectedId( nameof( CompareProjectsContext ) ), nameof( CompareProjectsContext ) ) { }
    }
}