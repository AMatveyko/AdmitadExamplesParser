// a.snegovoy@gmail.com

using System;

using AdmitadCommon.Entities.Statistics;

using Web.Common.Entities;
using Web.Common.Entities.Requests;

namespace ApiClient.Requests
{
    internal sealed class GetTotalStatisticsRequest : BaseRequest<TotalStatistics>
    {
        
        protected override string Controller => "Index";
        protected override string MethodName => "GetStatistics";

        public GetTotalStatisticsRequest( RequestSettings settings )
            : base( settings, false ) { }
        
        public GetTotalStatisticsRequest( DateTime startPoint, RequestSettings settings )
            : base( settings, false ) {
            AddParam( nameof( startPoint ), startPoint.ToString( "O" ) );
        }
    }
}