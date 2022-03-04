using System.Collections.Generic;

namespace WebLogFilesTool.Entities
{
    internal sealed class LogsStatistics
    {
        public List<LogEntryInfo> ServerError { get; } = new();
        public List<LogEntryInfo> NotFound { get; } = new();
        public List<LogEntryInfo> Forbidden { get; } = new();
        public List<LogEntryInfo> Ok { get; } = new();
        public List<LogEntryInfo> UpdateUrl { get; } = new();
        public List<LogEntryInfo> All { get; } = new();

    }
}