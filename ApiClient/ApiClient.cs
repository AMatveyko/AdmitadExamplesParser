// a.snegovoy@gmail.com

using System;

using AdmitadCommon.Entities.Statistics;

using ApiClient.Requests;
using ApiClient.Responces;

namespace ApiClient
{
    internal static class ApiClient
    {
        public static TopContext RunAndCheckIndex() => new RunAndCheckIndexRequest().Execute();

        public static TotalShopsStatistics GetShopStatistics() => new GetShopStatisticsRequest().Execute();

        public static TotalStatistics GetTotalStatistics( DateTime? start = null ) =>
            start != null
                ? new GetTotalStatisticsRequest( start.Value ).Execute()
                : new GetTotalStatisticsRequest().Execute();

    }
}