// a.snegovoy@gmail.com

using System;

using AdmitadCommon.Entities.Statistics;

using Web.Common.Entities;
using Web.Common.Entities.Requests;

namespace ApiClient.Requests
{
    internal sealed class GetTotalStatisticsRequest : BaseRequest<TotalStatistics>
    {
        
        private const string Controller = "Index";
        private const string MethodName = "GetStatistics";

        public GetTotalStatisticsRequest( RequestSettings settings )
            : base( Controller, MethodName, settings ) { }
        
        public GetTotalStatisticsRequest( DateTime startPoint, RequestSettings settings )
            : base( Controller, MethodName, settings )
        {
            AddParam( nameof( startPoint ), startPoint.ToString( "O" ) );
        }
    }
}