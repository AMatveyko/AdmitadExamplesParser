// a.snegovoy@gmail.com

using System.Collections.Generic;

namespace AdmitadCommon.Entities
{
    public sealed class StatisticsBlock
    {
        public StatisticsBlock(
            string componentName )
        {
            ComponentName = componentName;
        }

        public string ComponentName { get; }
        
        public long WorkTime { get; set; }
        public List<( string Line, long? Time)> Lines { get; } = new();

        public void AddLine(
            string line,
            long? workTime ) {
            Lines.Add( ( line, workTime) );
        }
        
        public void AddLine( ( string, long? ) line ) {
            Lines.Add( line );
        }

    }
}