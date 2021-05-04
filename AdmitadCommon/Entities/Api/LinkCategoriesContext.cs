// a.snegovoy@gmail.com

namespace AdmitadCommon.Entities.Api
{
    public sealed class LinkCategoriesContext : BackgroundBaseContext
    {
        public LinkCategoriesContext( string id )
            : base( GetCollectedId( nameof(LinkCategoriesContext), id ), id ) { }
    }
}