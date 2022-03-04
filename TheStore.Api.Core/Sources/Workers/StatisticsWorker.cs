// a.snegovoy@gmail.com

using System;
using System.Threading.Tasks;

using Admitad.Converters.Workers;

using AdmitadCommon.Entities;
using AdmitadCommon.Entities.Statistics;

using AdmitadSqlData.Helpers;

using Common.Api;
using Common.Entities;
using Common.Settings;

using Microsoft.AspNetCore.Mvc;

namespace TheStore.Api.Core.Sources.Workers
{
    internal sealed class StatisticsWorker
    {

        private readonly DbHelper _dbHelper;
        
        public StatisticsWorker( DbHelper dbHelper ) {
            _dbHelper = dbHelper;
        }
        
        public IActionResult GetShopStatistics()
        {
            var statistics = new TotalShopsStatistics {
                ByShop = _dbHelper.GetShopStatisticsList(),
                TotalEnabledShops = _dbHelper.GetNumberEnabledShops()
            };
            return new JsonResult( statistics );
        }

        public static async Task<IActionResult> GetTotalStatistics( DateTime? startPoint, ElasticSearchClientSettings setting )
        {
            var client = CreateElasticClient( new BackgroundBaseContext( "1", "none" ), setting );
            var statistics = new TotalStatistics {
                Products = await Task.Run( () => client.CountProducts() ),
                SoldOut = await Task.Run( () => client.CountSoldOutProducts() ),
                CountForSoldOut = startPoint != null
                    ? await Task.Run( () => client.CountForDisableProducts( startPoint.Value, null ) )
                    : 0
            };

            return new JsonResult( statistics );
        }
        
        
        
        private static IElasticClient<IIndexedEntities> CreateElasticClient( BackgroundBaseContext context, ElasticSearchClientSettings settings ) =>
            new ElasticSearchClient<IIndexedEntities>( settings, context );
    }
}