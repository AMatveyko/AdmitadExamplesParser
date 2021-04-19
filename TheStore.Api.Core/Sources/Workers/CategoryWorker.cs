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

        public void RelinkTag( RelinkCategoryContext context ) {
            
            var category = DbHelper.GetCategories().FirstOrDefault( c => c.Id == context.CategoryId );
            var client = CreateClient( context );
            var unlinkResult = client.UnlinkCategory( category );
            context.Messages.Add( $"Отвязали { unlinkResult.Pretty } товаров от категории" );
            context.PercentFinished = 50;
            
            var linkResult = client.UpdateProductsForCategoryFieldNameModel( category );
            context.Messages.Add( $"Привязвали { linkResult.Pretty } товаров к категории" );
            context.PercentFinished = 100;
            
            context.Content = $"{category.Id}: отвязали {unlinkResult.Pretty}, привязали {linkResult.Pretty}, разница { unlinkResult.GetDifferencePercent( linkResult ) }%";
        }

    }
}