// a.snegovoy@gmail.com

using System.Collections.Generic;
using System.Linq;

using AdmitadCommon.Entities;
using AdmitadCommon.Helpers;

using NLog;

namespace ApiClient
{
    internal static class Statistics
    {
        
        private static readonly Logger Logger = LogManager.GetLogger( "StatisticLogger" );
        
        public static void Calculate( List<ShopProductStatistics> statistics )
        {
            var strings = statistics.Select( GetString ).ToList();
            Logger.Info( string.Join( "\n", strings ) );
        }

        public static string GetResult( List<ShopProductStatistics> statistics )
        {
            var all = new ShopProductStatistics();
            statistics.ForEach(
                s => {
                    all.SoldoutAfter += s.SoldoutAfter;
                    all.SoldoutBefore += s.SoldoutBefore;
                    all.TotalAfter += s.TotalAfter;
                    all.TotalBefore += s.SoldoutBefore;
                } );
            return $"total products {all.TotalAfter} new {all.TotalAfter - all.TotalBefore}, soldout {all.SoldoutAfter} new {all.SoldoutAfter - all.SoldoutBefore}";
        }

        private static string GetString(
            ShopProductStatistics productStatistic )
        {
            var name = productStatistic.ShopName;
            var total = productStatistic.TotalAfter;
            var soldout = productStatistic.SoldoutAfter;

            return
                $"{name}: total {total} new {total - productStatistic.TotalBefore}, soldout {soldout} new {soldout - productStatistic.SoldoutBefore}";
        }
    }
}