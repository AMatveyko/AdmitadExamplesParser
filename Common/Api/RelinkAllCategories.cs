// a.snegovoy@gmail.com

namespace Common.Api
{
    public sealed class RelinkAllCategories : ParallelBackgroundContext
    {
        public RelinkAllCategories() : base( GetCollectedId( nameof(RelinkAllCategories) ), nameof(RelinkAllCategories) ) { }
    }
}