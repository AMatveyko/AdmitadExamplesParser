using System.Collections.Generic;
using Common.Entities;
using Web.Common.Entities;
using Web.Common.Entities.Requests;

namespace ApiClient.Requests
{
    internal sealed class GetValidatesUrls : BaseRequest<List<UrlIndexInfo>>
    {
        public GetValidatesUrls(RequestSettings settings) : base(settings, false) {
            AddParam("urlNumber", "1000");
            AddParam("amountWorkers", "5");
        }
        protected override string Controller => "Utils";
        protected override string MethodName => "CheckIndex";
    }
}