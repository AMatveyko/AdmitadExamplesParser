// a.snegovoy@gmail.com

using Web.Common.Entities;
using Web.Common.Entities.Requests;
using Web.Common.Entities.Responses;

namespace TheStore.Api.Core.Sources.Entities
{
    public sealed class PageStatisticsRequest : BaseRequest<PagesStatisticsResponse>
    {
        protected override string Controller => "api/Files";
        protected override string MethodName => "GetPageStatistics";
        
        public PageStatisticsRequest( RequestSettings settings )
            : base( settings, false ) { }
    }
}