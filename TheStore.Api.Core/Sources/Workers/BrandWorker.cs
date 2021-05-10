// a.snegovoy@gmail.com

using AdmitadCommon;
using AdmitadCommon.Entities;

using AdmitadSqlData.Helpers;

using TheStore.Api.Core.Sources.Entity;

namespace TheStore.Api.Core.Sources.Workers
{
    public sealed class BrandWorker : BaseLinkWorker
    {

        private readonly ProcessorSettings _settings;

        public BrandWorker( ProcessorSettings settings )
            :base( settings.ElasticSearchClientSettings )
        {
            _settings = settings;
        }

        public void FillBrandId(
            FillBrandIdContext context )
        {
            var brandId = DbHelper.GetBrandId( context.ClearlyName );
            if( brandId == Constants.UndefinedBrandId ) {
                context.PercentFinished = 100;
                context.Content = "Brand notFound";
                return;
            }

            var client = CreateElasticClient( context );
            var totalCount = client.GetCountWithClearlyName( context.ClearlyName );
            context.PercentFinished = 20;
            context.Messages.Add( $"Всего найдено товаров {totalCount}" );
            var linkedCount = client.GetCountWithClearlyName( context.ClearlyName, brandId );
            context.PercentFinished = 40;
            context.Messages.Add( $"Из них с верным ид {linkedCount}" );
            var result = client.UpdateBrandId( context.ClearlyName, brandId );
            context.Messages.Add( $"Обновили { result.Updated } товаров" );
            context.Content =
                $"Всего товаров {totalCount}, из нис с правильным ид {linkedCount}, обновили {result.Updated}";
        }
    }
}