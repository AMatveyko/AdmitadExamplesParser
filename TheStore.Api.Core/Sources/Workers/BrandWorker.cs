// a.snegovoy@gmail.com

using AdmitadCommon;
using AdmitadCommon.Entities;
using AdmitadCommon.Entities.Api;

using AdmitadSqlData.Helpers;

using Common.Settings;

namespace TheStore.Api.Core.Sources.Workers
{
    internal sealed class BrandWorker : BaseLinkWorker
    {

        private readonly ProcessorSettings _settings;

        public BrandWorker( ProcessorSettings settings, BackgroundWorks works, DbHelper dbHelper )
            :base( settings.ElasticSearchClientSettings, works, dbHelper )
        {
            _settings = settings;
        }

        public void FillBrandId(
            FillBrandIdContext context )
        {
            var brandId = Db.GetBrandId( context.ClearlyName );
            if( brandId == Constants.UndefinedBrandId ) {
                context.Finish();
                context.Content = "Brand notFound";
                return;
            }

            var client = CreateElasticClient( context );
            var totalCount = client.GetCountWithClearlyName( context.ClearlyName );
            context.SetProgress( 20, 100 );
            context.Messages.Add( $"Всего найдено товаров {totalCount}" );
            var linkedCount = client.GetCountWithClearlyName( context.ClearlyName, brandId );
            context.SetProgress( 40, 100 );
            context.Messages.Add( $"Из них с верным ид {linkedCount}" );
            var result = client.UpdateBrandId( context.ClearlyName, brandId );
            context.Messages.Add( $"Обновили { result.Updated } товаров" );
            context.Content =
                $"Всего товаров {totalCount}, из нис с правильным ид {linkedCount}, обновили {result.Updated}";
        }
    }
}