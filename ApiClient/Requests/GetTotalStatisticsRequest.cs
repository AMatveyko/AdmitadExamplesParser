// a.snegovoy@gmail.com

using System;

using AdmitadCommon.Entities.Statistics;

namespace ApiClient.Requests
{
    internal sealed class GetTotalStatisticsRequest : BaseRequest<TotalStatistics>
    {
        
        private const string Controller = "Index";
        private const string MethodName = "GetStatistics";

        public GetTotalStatisticsRequest()
            : base( Controller, MethodName ) { }
        
        public GetTotalStatisticsRequest( DateTime startPoint )
            : base( Controller, MethodName )
        {
            AddParam( nameof( startPoint ), startPoint.ToString( "O" ) );
        }
    }
}