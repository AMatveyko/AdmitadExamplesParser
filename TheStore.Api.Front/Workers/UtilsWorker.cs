// a.snegovoy@gmail.com

using System.Diagnostics;
using System.Threading.Tasks;

using NLog;

namespace TheStore.Api.Front.Workers
{
    internal sealed class UtilsWorker
    {
        
        private static readonly Logger Logger = LogManager.GetLogger( "SitemapGeneration" );
        
        public static void StartSitemapGeneration()
        {
            Task.Run( DoStartSitemapGeneration );
        }

        private static void DoStartSitemapGeneration()
        {
            var process = new Process {
                StartInfo = new ProcessStartInfo {
                    WindowStyle = ProcessWindowStyle.Hidden,
                    FileName = "/opt/remi/php56/root/usr/bin/php",
                    Arguments = "/var/www/thestore/www/sitemap/make.php",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                }
            };
            
            process.Start();
            var output = string.Concat( process.StandardError.ReadToEnd(), process.StandardOutput.ReadToEnd() );
            Logger.Info( output );
        }
    }
}