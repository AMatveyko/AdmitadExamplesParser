// a.snegovoy@gmail.com

namespace AdmitadCommon.Entities.Queue
{
    public class QueueStatus
    {
        public QueueState Parallel { get; set; }
        public QueueState Single { get; set; }
    }
}