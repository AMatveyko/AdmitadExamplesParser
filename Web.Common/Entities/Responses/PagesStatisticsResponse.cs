// a.snegovoy@gmail.com

using System.Collections.Generic;

namespace Web.Common.Entities.Responses
{
    public sealed class PagesStatisticsResponse
    {
        public string Error { get; set; }
        public string ErrorMessage { get; set; }
        public List<string> Lines { get; set; }
    }
}