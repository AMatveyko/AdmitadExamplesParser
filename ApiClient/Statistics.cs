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
        
        public static void Calculate( List<ShopStatistics> statistics )
        {
            var strings = statistics.Select( GetString ).ToList();
            Logger.Info( string.Join( "\n", strings ) );
        }

        public static string GetResult( List<ShopStatistics> statistics )
        {
            var all = new ShopStatistics();
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
            ShopStatistics statistic )
        {
            var name = statistic.ShopName;
            var total = statistic.TotalAfter;
            var soldout = statistic.SoldoutAfter;

            return
                $"{name}: total {total} new {total - statistic.TotalBefore}, soldout {soldout} new {soldout - statistic.SoldoutBefore}";
        }
    }
}