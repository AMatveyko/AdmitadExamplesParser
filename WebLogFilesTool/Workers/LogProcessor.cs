using System;
using System.Collections.Generic;
using System.Linq;
using WebLogFilesTool.Entities;

namespace WebLogFilesTool.Workers
{
    internal sealed class LogProcessor
    {
        private readonly Func<IReader> _readerBuilder;
        private readonly Func<IParser> _parserBuilder;

        public LogProcessor(Func<IReader> readerBuilder, Func<IParser> parserBuilder) =>
            (_readerBuilder, _parserBuilder) = (readerBuilder, parserBuilder);

        public List<LogEntryInfo> GetLogs(List<string> paths) =>
            paths.AsParallel().SelectMany(GetLogsFromFile).ToList();

        private List<LogEntryInfo> GetLogsFromFile(string path) {
            var lines = ReadFile(path);
            return ParseLines(lines);
        }

        private List<string> ReadFile(string path) =>
            _readerBuilder().GetLogLines(path);

        private List<LogEntryInfo> ParseLines(List<string> lines) =>
            _parserBuilder().Parse(lines);
    }
}