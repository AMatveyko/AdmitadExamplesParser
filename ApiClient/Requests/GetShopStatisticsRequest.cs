// a.snegovoy@gmail.com

using AdmitadCommon.Entities.Statistics;

using Web.Common.Entities;
using Web.Common.Entities.Requests;

namespace ApiClient.Requests
{
    internal sealed class GetShopStatisticsRequest : BaseRequest<TotalShopsStatistics>
    {
        
        private const string Controller = "Index";
        private const string MethodName = "GetShopStatistics";
        
        public GetShopStatisticsRequest( RequestSettings settings )
            : base( Controller, MethodName, settings ) { }
    }
}