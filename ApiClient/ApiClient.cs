// a.snegovoy@gmail.com

using System;

using AdmitadCommon.Entities.Statistics;

using ApiClient.Requests;
using ApiClient.Responces;

using Web.Common.Entities;

namespace ApiClient
{
    internal class ApiClient
    {

        private readonly RequestSettings _settings;

        public ApiClient(
            RequestSettings settings ) =>
            _settings = settings;
        
        public TopContext RunAndCheckIndex() => new RunAndCheckIndexRequest( _settings ).Execute();

        public TotalShopsStatistics GetShopStatistics() => new GetShopStatisticsRequest( _settings ).Execute();

        public TotalStatistics GetTotalStatistics( DateTime? start = null ) =>
            start != null
                ? new GetTotalStatisticsRequest( start.Value, _settings ).Execute()
                : new GetTotalStatisticsRequest( _settings ).Execute();

    }
}