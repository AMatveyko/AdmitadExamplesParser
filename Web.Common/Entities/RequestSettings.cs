// a.snegovoy@gmail.com

using NLog;

namespace Web.Common.Entities
{
    public sealed class RequestSettings
    {

        public RequestSettings( string host, Logger logger = null ) =>
            ( Host, Logger ) = ( host, logger );
        
        public string Host { get; }
        public Logger Logger { get; }
    }
}