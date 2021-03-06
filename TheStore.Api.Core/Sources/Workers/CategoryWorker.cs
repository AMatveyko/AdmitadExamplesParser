// a.snegovoy@gmail.com

using System.Linq;
using Admitad.Converters.Workers;
using AdmitadCommon.Entities;

using AdmitadSqlData.Helpers;

using Common.Api;
using Common.Settings;

namespace TheStore.Api.Core.Sources.Workers
{
    internal sealed class CategoryWorker : BaseLinkWorker
    {

        public CategoryWorker( ElasticSearchClientSettings settings, BackgroundWorks works, DbHelper db, ProductRatingCalculation productRatingCalculation ) 
            :base( settings, works, db, productRatingCalculation ) { }

        public void RelinkCategory( RelinkCategoryContext context ) {
            
            var category = Db.GetCategories().FirstOrDefault( c => c.Id == context.CategoryId );
            context.CategoryName = category.NameH1;
            var linker = CreateLinker( context );
            if( context.Relink ) {
                linker.RelinkCategory( category );    
            }
            else {
                linker.LinkCategory( category );
            }
        }

        public void LinkCategories( LinkCategoriesContext context )
        {
            var categories = Db.GetCategories();
            var linker = CreateLinker( context );
            linker.LinkCategories( categories );
        }

        // public void RelinkAllCategories( RelinkAllCategories context )
        // {
        //     var categories = DbHelper.GetCategories();
        //     foreach( var category in categories ) {
        //         var singleContext = new RelinkCategoryContext( category.Id );
        //         context.AddContext( singleContext );
        //         Works.AddToQueue( RelinkCategory, singleContext, QueuePriority.Low, true );
        //         context.AddMessage( $"Добавили перепривязку для категории { category.Id }" );
        //     }
        //
        //     while( context.Contexts.Any( c => c.IsFinished == false ) ) {
        //         Thread.Sleep( 1000 );
        //     }
        // }

    }
}