using WebLogFilesTool.Workers;

namespace WebLogFilesTool
{
    class Program
    {
        private const string Path = @"o:\tools\nginxLogReader\tests";
        
        static void Main(string[] args) {
            var reader = new FileReader(Path);
            var lines = reader.GetLogs();

            var entries = LogParser.Parse(lines);
        }
    }
}