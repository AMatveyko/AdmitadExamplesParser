using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using WebLogFilesTool.Entities;
using WebLogFilesTool.Extensions;

namespace WebLogFilesTool.Workers
{
    internal sealed class NginxLogParser : IParser
    {

        private static readonly Regex Pattern =
            new(
                @"(?<date>(\d{2}\/\w{3}\/\d{4}:\d{2}:\d{2}:\d{2} \+\d{4})) \[(?<errorCode>\d{3})] \""((?<requestType>\w{3,6}) )?(?<url>(((\/)?(.+)?)( HTTP\/(?<version>\d)\.\d))|(\/)?(.+)?)\"" \""(?<userAgent>(.+)?)\"" referer: \""(?<referer>(.+)?)\"" \| (?<bytes>\d+) \| (?<ipAddress>\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3})",
                RegexOptions.Compiled);

        public List<LogEntryInfo> Parse(IEnumerable<string> entries) =>
            entries.AsParallel().Select(MatchEndGetEntry).ToList();

        private static LogEntryInfo MatchEndGetEntry(string entry) =>
            GetEntry(Pattern.Match(entry));


        private static LogEntryInfo GetEntry(Match m) =>
            new () {
                Date = m.GetDateValue("date"),
                Code = m.GetIntValue("errorCode"),
                Url = m.GetValue("url"),
                Type = m.GetValue("requestType"),
                HttpVersion = m.GetIntValue("version"),
                UserAgent = m.GetValue("userAgent"),
                Referer = m.GetValue("referer"),
                Ip = m.GetIPAddress("ipAddress"),
                Bytes = m.GetLongValue("bytes")
            };
    }
}