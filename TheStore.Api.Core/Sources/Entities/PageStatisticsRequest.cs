// a.snegovoy@gmail.com

using Web.Common.Entities;
using Web.Common.Entities.Requests;
using Web.Common.Entities.Responses;

namespace TheStore.Api.Core.Sources.Entities
{
    public sealed class PageStatisticsRequest : BaseRequest<PagesStatisticsResponse>
    {
        private const string Controller = "api/Files";
        private const string MethodName = "GetPageStatistics";
        
        public PageStatisticsRequest( RequestSettings settings )
            : base( Controller, MethodName, settings ) { }
    }
}