// a.snegovoy@gmail.com

using ApiClient.Responces;

using Web.Common.Entities;
using Web.Common.Entities.Requests;

namespace ApiClient.Requests
{
    internal sealed class IndexAllRequest : BaseRequest<TopContext>
    {
        protected override string Controller => "Index";
        protected override string MethodName => "IndexAllShops";

        public IndexAllRequest( RequestSettings settings, bool clean = true )
            : base( settings, clean )  {}
    }
}