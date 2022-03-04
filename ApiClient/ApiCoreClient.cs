// a.snegovoy@gmail.com

using System;
using System.Collections.Generic;
using AdmitadCommon.Entities.Statistics;

using ApiClient.Requests;
using ApiClient.Responces;
using Common.Entities;
using Web.Common.Entities;

namespace ApiClient
{
    internal class ApiCoreClient
    {

        private readonly RequestSettings _settings;

        public ApiCoreClient(
            RequestSettings settings ) =>
            _settings = settings;
        
        public TopContext RunAndCheckIndex() => new IndexAllRequest( _settings ).Execute();

        public TotalShopsStatistics GetShopStatistics() => new GetShopStatisticsRequest( _settings ).Execute();

        public TotalStatistics GetTotalStatistics( DateTime? start = null ) =>
            start != null
                ? new GetTotalStatisticsRequest( start.Value, _settings ).Execute()
                : new GetTotalStatisticsRequest( _settings ).Execute();

        public TopContext RunAndCheckLinkAll() => new LinkAllRequest( _settings ).Execute();

        public Context RunRatingCalculation() => new RatingsCalculationRequest(_settings).Execute();
        
        public List<UrlIndexInfo> GetValidatesUrls() => new GetValidatesUrls(_settings).Execute();

    }
}