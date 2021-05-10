// a.snegovoy@gmail.com

using AdmitadCommon.Entities.Statistics;

namespace ApiClient.Requests
{
    internal sealed class GetShopStatisticsRequest : BaseRequest<TotalShopsStatistics>
    {
        
        private const string Controller = "Index";
        private const string MethodName = "GetShopStatistics";
        
        public GetShopStatisticsRequest()
            : base( Controller, MethodName ) { }
    }
}