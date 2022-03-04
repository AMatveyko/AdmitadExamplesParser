using System;
using System.Collections.Generic;
using System.Linq;
using WebLogFilesTool.Entities;

namespace WebLogFilesTool.Workers
{
    internal sealed class StatisticsWorker
    {

        private readonly List<Action<LogsStatistics, LogEntryInfo>> _actions = new () {
            CalculateServerError,
            CalculateNotFound,
            CalculateForbidden,
            CalculateOk,
            AddToAll,
            AddIsUpdateUrl
        };
        
        public LogsStatistics Calculate(IEnumerable<LogEntryInfo> logs) {
            var statistics = new LogsStatistics();

            // logs.AsParallel().ForAll( e => Calculate(statistics, e) );

            foreach (var logEntryInfo in logs) {
                Calculate(statistics, logEntryInfo);
            }
            
            
            return statistics;
        }

        private void Calculate(LogsStatistics statistics, LogEntryInfo logEntryInfo) {
            foreach (var action in _actions) {
                action(statistics, logEntryInfo);
            }
        }

        private static void CalculateServerError(LogsStatistics statistics, LogEntryInfo logEntryInfo) =>
            DetermineCode(statistics.Ok, logEntryInfo, 500);
        
        private static void CalculateNotFound(LogsStatistics statistics, LogEntryInfo logEntryInfo) =>
            DetermineCode(statistics.NotFound, logEntryInfo, 400);

        private static void CalculateForbidden(LogsStatistics statistics, LogEntryInfo logEntryInfo) =>
            DetermineCode(statistics.Forbidden, logEntryInfo, 300);

        private static void CalculateOk(LogsStatistics statistics, LogEntryInfo logEntryInfo) =>
            DetermineCode(statistics.Ok, logEntryInfo, 200);

        private static void AddToAll(LogsStatistics statistics, LogEntryInfo logEntryInfo) =>
            statistics.All.Add(logEntryInfo);
        
        private static void DetermineCode(ICollection<LogEntryInfo> logs, LogEntryInfo logEntryInfo, int code) {
            if (IsCodeRange(code, logEntryInfo)) {
                logs.Add(logEntryInfo);
            }
        }

        private static void AddIsUpdateUrl(LogsStatistics statistics, LogEntryInfo logEntryInfo) {
            if (logEntryInfo.Url.Contains("/UrlStatistics/Update")) {
                statistics.UpdateUrl.Add(logEntryInfo);
            }
        }

        private static bool IsCodeRange(int codeGroup, LogEntryInfo logEntryInfo) =>
            logEntryInfo.Code >= codeGroup && logEntryInfo.Code < codeGroup+100;
    }
}