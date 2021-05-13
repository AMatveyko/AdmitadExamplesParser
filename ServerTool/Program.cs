using System.Collections.Generic;

using Admitad.Converters;

using ServerTool.Workers;

namespace ServerTool
{
    class Program
    {
        private static HashSet<(string, bool)> _services = new() {
            ( "php56-php-fpm", true ),
            ( "mariadb", false ),
            ( "docker", false ),
            ( "elasticsearch", false )
        };
        
        static void Main(
            string[] args )
        {
            var settings = SettingsBuilder.GetMessengerSettings();
            var loggers = new Loggers( settings );
            var serviceWorker = new ServiceWorker( _services, loggers );
            serviceWorker.Check();
        }
    }
}