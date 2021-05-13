// a.snegovoy@gmail.com

using ApiClient.Responces;

using Web.Common.Entities;
using Web.Common.Entities.Requests;

namespace ApiClient.Requests
{
    internal sealed class RunAndCheckIndexRequest : BaseRequest<TopContext>
    {
        private const string Controller = "Index";
        private const string MethodName = "IndexAllShops";

        public RunAndCheckIndexRequest( RequestSettings settings, bool clean = true )
            : base( Controller, MethodName, settings )
        {
            AddParam( nameof(clean), clean.ToString().ToLower() );
        }
    }
}