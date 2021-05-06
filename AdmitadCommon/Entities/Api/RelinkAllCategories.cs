// a.snegovoy@gmail.com

namespace AdmitadCommon.Entities.Api
{
    public sealed class RelinkAllCategories : ParallelBackgroundContext
    {
        public RelinkAllCategories() : base( GetCollectedId( nameof(RelinkAllCategories) ), nameof(RelinkAllCategories) ) { }
    }
}