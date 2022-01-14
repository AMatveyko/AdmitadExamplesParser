// a.snegovoy@gmail.com

using ApiClient.Responces;

using Web.Common.Entities;
using Web.Common.Entities.Requests;

namespace ApiClient.Requests
{
    internal sealed class LinkAllRequest : BaseRequest<TopContext>
    {
        
        protected override string Controller => "Index";
        protected override string MethodName => "LinkAll";

        public LinkAllRequest( RequestSettings settings ) : base( settings, true ) { }
    }
}