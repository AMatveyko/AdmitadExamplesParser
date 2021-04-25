// a.snegovoy@gmail.com

using System.Linq;

using AdmitadCommon.Entities;

using AdmitadSqlData.Helpers;

using TheStore.Api.Core.Sources.Entity;

namespace TheStore.Api.Core.Sources.Workers
{
    public sealed class CategoryWorker : BaseLinkWorker
    {

        public CategoryWorker( ElasticSearchClientSettings settings ) 
            :base( settings ) {}

        public void RelinkCategory( RelinkCategoryContext context ) {
            
            var category = DbHelper.GetCategories().FirstOrDefault( c => c.Id == context.CategoryId );
            context.Name = category.NameH1;
            var linker = CreateLinker( context );
            linker.RelinkCategory( category );
        }

    }
}