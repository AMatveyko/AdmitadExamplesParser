using System.Collections.Generic;
using WebLogFilesTool.Entities;

namespace WebLogFilesTool.Workers
{
    internal interface IParser
    {
        public List<LogEntryInfo> Parse(IEnumerable<string> entries);
    }
}