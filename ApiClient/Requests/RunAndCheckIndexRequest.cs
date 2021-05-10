// a.snegovoy@gmail.com

using System.Collections.Generic;

using ApiClient.Responces;

namespace ApiClient.Requests
{
    internal sealed class RunAndCheckIndexRequest : BaseRequest<TopContext>
    {
        private const string Controller = "Index";
        private const string MethodName = "IndexAllShops";

        public RunAndCheckIndexRequest( bool clean = true )
            : base( Controller, MethodName )
        {
            AddParam( nameof(clean), clean.ToString().ToLower() );
        }
    }
}