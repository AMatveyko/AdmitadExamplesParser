using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace WebLogFilesTool.Workers
{
    internal sealed class FileReader : IReader
    {

        public List<string> GetLogLines(string filePath) => ReadFile(filePath);

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
    }
}