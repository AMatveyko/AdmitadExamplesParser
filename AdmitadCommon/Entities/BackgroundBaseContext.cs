// a.snegovoy@gmail.com

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AdmitadCommon.Entities
{
    public class BackgroundBaseContext
    {

        public BackgroundBaseContext( string id ) {
            Id = id;
        }

        [ JsonIgnore ] public BackgroundStatus WorkStatus { get; set; } = BackgroundStatus.OutInLine;
        public string Status => WorkStatus.ToString();
        
        public bool IsFinished { get; set; }
        [ JsonIgnore ] public string Id { get; }
        public int PercentFinished { get; set; }
        public string Content { get; set; } = "Ждем результат...";
        public bool IsError { get; set; }
        public long Time { get; set; }
        public List<string> Messages { get; set; } = new List<string>();
    }
}