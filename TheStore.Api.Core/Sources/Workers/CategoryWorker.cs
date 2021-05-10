// a.snegovoy@gmail.com

using System.Linq;
using System.Threading;

using AdmitadCommon.Entities;
using AdmitadCommon.Entities.Api;

using AdmitadSqlData.Helpers;

namespace TheStore.Api.Core.Sources.Workers
{
    internal sealed class CategoryWorker : BaseLinkWorker
    {

        public CategoryWorker( ElasticSearchClientSettings settings, BackgroundWorks works ) 
            :base( settings, works ) { }

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

        public void RelinkAllCategories( RelinkAllCategories context )
        {
            var categories = DbHelper.GetCategories();
            foreach( var category in categories ) {
                var singleContext = new RelinkCategoryContext( category.Id );
                context.AddContext( singleContext );
                Works.AddToQueue( RelinkCategory, singleContext, QueuePriority.Low, true );
                context.AddMessage( $"Добавили перепривязку для категории { category.Id }" );
            }

            while( context.Contexts.Any( c => c.IsFinished == false ) ) {
                Thread.Sleep( 1000 );
            }
        }

    }
}