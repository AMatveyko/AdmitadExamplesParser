// a.snegovoy@gmail.com

using System.Collections.Generic;

namespace AdmitadCommon.Entities.Queue
{
    public sealed class QueueState
    {
        public string InWork { get; set; }
        public List<string> Awaiting { get; set; }
        public List<string> Completed { get; set; }
    }
}