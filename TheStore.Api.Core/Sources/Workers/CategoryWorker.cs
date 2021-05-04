// a.snegovoy@gmail.com

using System.Linq;

using AdmitadCommon.Entities;
using AdmitadCommon.Entities.Api;

using AdmitadSqlData.Helpers;

namespace TheStore.Api.Core.Sources.Workers
{
    internal sealed class CategoryWorker : BaseLinkWorker
    {

        public CategoryWorker( ElasticSearchClientSettings settings ) 
            :base( settings ) {}

        public void RelinkCategory( RelinkCategoryContext context ) {
            
            var category = DbHelper.GetCategories().FirstOrDefault( c => c.Id == context.CategoryId );
            context.Name = category.NameH1;
            var linker = CreateLinker( context );
            linker.RelinkCategory( category );
        }

        public void LinkCategories( LinkCategoriesContext context )
        {
            var categories = DbHelper.GetCategories();
            var linker = CreateLinker( context );
            linker.LinkCategories( categories );
        }

    }
}