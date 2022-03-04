using System;
using System.Net;

namespace WebLogFilesTool.Entities
{
    internal record LogEntryInfo
    {
        public DateTime Date { get; init; }
        public int Code { get; init; }
        public string Type { get; init; }
        public string Url { get; init; }
        public int HttpVersion { get; init; }
        public string UserAgent { get; init; }
        public string Referer { get; init; }
        public IPAddress Ip { get; init; }
        public long Bytes { get; init; }
        public string Text { get; init; }
    }
}