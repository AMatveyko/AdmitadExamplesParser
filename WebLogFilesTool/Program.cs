using WebLogFilesTool.Workers;

namespace WebLogFilesTool
{
    class Program
    {
        private const string Path = @"o:\tools\nginxLogReader\tests";
        
        static void Main(string[] args) {

            var pathGetter = new FilePathGetter(Path, 10);
            var filesPaths = pathGetter.Get();

            var processor = new LogProcessor(() => new FileReader(), ()=> new NginxLogParser());

            var result = processor.GetLogs(filesPaths);

            var statWorker = new StatisticsWorker();
            var statistics = statWorker.Calculate(result);
        }
    }
}