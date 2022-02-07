using System.Collections.Generic;
using Common.Entities;
using Web.Common.Entities;
using Web.Common.Entities.Requests;

namespace CheckIndexTool.Responses
{
    internal sealed class SaveCheckingResultRequest : BaseRequest<string>
    {
        public SaveCheckingResultRequest(string host, List<UrlIndexInfo> content) : base(new RequestSettings(host), false) {
            AddContent( content );
        }

        protected override string Controller => "UrlStatistics";
        protected override string MethodName => "SaveCheckingResult";
    }
}