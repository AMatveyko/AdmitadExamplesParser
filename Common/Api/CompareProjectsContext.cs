// a.snegovoy@gmail.com

namespace Common.Api
{
    public sealed class CompareProjectsContext : BackgroundBaseContext
    {
        public CompareProjectsContext()
            : base( GetCollectedId( nameof( CompareProjectsContext ) ), nameof( CompareProjectsContext ) ) { }
    }
}