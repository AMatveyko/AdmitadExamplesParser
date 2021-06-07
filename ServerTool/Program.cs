using System.Collections.Generic;

using Admitad.Converters;

using AdmitadSqlData.Helpers;

using Common.Workers;

using ServerTool.Workers;

namespace ServerTool
{
    class Program
    {
        private static HashSet<(string, bool)> _services = new() {
            ( "php56-php-fpm", false ),
            ( "mariadb", false ),
            ( "docker", false ),
            ( "elasticsearch", false )
        };

        private const string ServiceCheckArg = "serviceCheck";
        private const string NginxLogAnalyze = "nginxLogAnalyze";

        private const string NginxLogPath = "";
        
        static void Main(
            string[] args )
        {
            if( args.Length == 0 ) {
                return;
            }

            switch( args[0] ) {
                case ServiceCheckArg:
                    ServiceCheck();
                    break;
                case NginxLogAnalyze:
                    break;
            }
            
        }

        private static void ServiceCheck()
        {
            var builder = new SettingsBuilder( new DbHelper( SettingsBuilder.GetDbSettings() ) );
            var settings = builder.GetMessengerSettings();
            var loggers = new Loggers( settings );
            var serviceWorker = new ServiceWorker( _services, loggers );
            serviceWorker.Check();
        }
    }
}