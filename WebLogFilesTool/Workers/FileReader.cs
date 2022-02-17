using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using WebLogFilesTool.Entities;

namespace WebLogFilesTool.Workers
{
    internal sealed class FileReader
    {

        private static Regex _fileName = new(@"access.log-(?<date>\d{8}).gz", RegexOptions.Compiled);
        
        private readonly string _filepath;

        public FileReader(string filepath) => _filepath = filepath;

        public List<string> GetLogs() =>
            ReadFiles(GetFiles);

        public List<string> GetLogs(int lastDays) =>
            ReadFiles(() => GetFiles(lastDays));

        private static List<string> ReadFiles(Func<IEnumerable<LogFileInfo>> getter) {
            
            var files = getter().ToList();
            var result = new List<string>();
            foreach (var fileInfo in files) {
                result.AddRange(ReadFile(fileInfo.Path));
            }

            return result;
        }

        private static List<string> ReadFile(string fileName) {
            var bytes = ReadAndDecompressFile(fileName);
            return Encoding.UTF8.GetString(bytes).Split('\n', StringSplitOptions.RemoveEmptyEntries)
                .Where( s => string.IsNullOrWhiteSpace(s) == false)
                .ToList();
        }

        private static byte[] ReadAndDecompressFile(string fileName) {
            using var compressedFileStream = File.Open(fileName, FileMode.Open);
            using var decompressor = new GZipStream(compressedFileStream, CompressionMode.Decompress);
            using var memoryStream = new MemoryStream();
            decompressor.CopyTo(memoryStream);
            
            return memoryStream.ToArray();
        }
        
        private IEnumerable<LogFileInfo> GetFiles(int lastDays) =>
            GetFiles().Where(f => f.CreationDate >= DateTime.Now.AddDays(-lastDays));

        private IEnumerable<LogFileInfo> GetFiles() =>
            Directory.GetFiles(_filepath).Select(f => _fileName.Match(f)).Where(m => m.Success).Select(GetFileInfo);

        private LogFileInfo GetFileInfo(Match m) =>
            new(
                DateTime.ParseExact(m.Groups["date"].Value, "yyyyMMdd", CultureInfo.InvariantCulture)
                , $"{_filepath}\\{m.Value}");
    }
}