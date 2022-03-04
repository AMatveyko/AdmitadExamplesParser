using System;

namespace WebLogFilesTool.Entities
{
    internal sealed class LogFileInfo
    {
        public LogFileInfo(DateTime creationDate, string path ) =>
            (CreationDate, Path) = (creationDate, path);
        public DateTime CreationDate { get; }
        public string Path { get; }
    }
}