// a.snegovoy@gmail.com

using AdmitadCommon.Entities.Statistics;

using Web.Common.Entities;
using Web.Common.Entities.Requests;

namespace ApiClient.Requests
{
    internal sealed class GetShopStatisticsRequest : BaseRequest<TotalShopsStatistics>
    {
        
        protected override string Controller => "Index";
        protected override string MethodName => "GetShopStatistics";
        
        public GetShopStatisticsRequest( RequestSettings settings )
            : base( settings, false ) { }
    }
}