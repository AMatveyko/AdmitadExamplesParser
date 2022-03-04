using System.Collections.Generic;

namespace WebLogFilesTool.Workers
{
    internal interface IReader
    {
        public List<string> GetLogLines(string filePath);
    }
}