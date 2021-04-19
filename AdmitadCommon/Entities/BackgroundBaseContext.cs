// a.snegovoy@gmail.com

using System.Collections.Generic;

namespace AdmitadCommon.Entities
{
    public class BackgroundBaseContext
    {
        public string Id { get; set; }
        public bool IsFinished { get; set; }
        public int PercentFinished { get; set; }
        public string Content { get; set; }
        public bool IsError { get; set; }
        public long Time { get; set; }
        public List<string> Messages { get; set; } = new List<string>();
    }
}