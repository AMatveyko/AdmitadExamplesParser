using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using WebLogFilesTool.Entities;

namespace WebLogFilesTool.Workers
{
    internal sealed class FilePathGetter : IPathGetter
    {
        
        private static readonly Regex FileName = new(@"access.log-(?<date>\d{8}).gz", RegexOptions.Compiled);
        
        private readonly string _filepath;
        private readonly int _lastDays;

        public FilePathGetter(string filepath, int lastDays) =>
            (_filepath, _lastDays) = (filepath, lastDays);
        
        public List<string> Get() =>
            GetFiles().Where(IsSuitable).Select( f => f.Path ).ToList();

        private IEnumerable<LogFileInfo> GetFiles() =>
            Directory.GetFiles(_filepath).Select(f => FileName.Match(f)).Where(m => m.Success).Select(GetFileInfo);

        private LogFileInfo GetFileInfo(Match m) =>
            new(
                DateTime.ParseExact(m.Groups["date"].Value, "yyyyMMdd", CultureInfo.InvariantCulture)
                , $"{_filepath}\\{m.Value}");

        private bool IsSuitable(LogFileInfo file) =>
            file.CreationDate >= DateTime.Now.AddDays(-_lastDays);
    }
}