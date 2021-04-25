// a.snegovoy@gmail.com

using AdmitadCommon.Entities;

namespace TheStore.Api.Core.Sources.Entity
{
    public class RelinkCategoryContext : BackgroundBaseContext
    {
        public RelinkCategoryContext(
            string categoryId ) : base( $"RelinkCategoryContext:{categoryId}" )
        {
            CategoryId = categoryId;
        }
        public string CategoryId { get; }
        public string Name { get; set; }
    }
}