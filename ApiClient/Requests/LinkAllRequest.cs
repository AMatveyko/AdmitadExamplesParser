// a.snegovoy@gmail.com

using ApiClient.Responces;

using Web.Common.Entities;
using Web.Common.Entities.Requests;

namespace ApiClient.Requests
{
    internal sealed class LinkAllRequest : BaseRequest<TopContext>
    {
        
        private const string Controller = "Index";
        private const string MethodName = "LinkAll";

        public LinkAllRequest(
            RequestSettings settings )
            : base( Controller, MethodName, settings )
        {
            AddParam( "clean", "true" );
        }
    }
}