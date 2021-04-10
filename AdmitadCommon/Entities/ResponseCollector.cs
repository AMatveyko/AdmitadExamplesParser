// a.snegovoy@gmail.com

using System.Collections.Generic;

using Nest;

namespace AdmitadCommon.Entities
{
    public static class ResponseCollector
    {
        public static List<( string, string, BulkResponse)> Responses { get; set; } = new();
    }
}