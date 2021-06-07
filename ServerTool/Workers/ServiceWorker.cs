// a.snegovoy@gmail.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using NLog;

using ServerTool.Entities;

namespace ServerTool.Workers
{
    internal sealed class ServiceWorker
    {
        
        private static readonly Regex Pattern =
            new(@"Active: (?<status1>(.*)) (\((?<status2>(.*))\))( since (.*); (.*))?", RegexOptions.Compiled );
        private const int RestartTime = 4;
        private const int RestartAttempts = 3;

        private const string Running = "running";
        private readonly HashSet<(string,bool)> _servicesNames;
        private readonly Loggers _loggers;

        public ServiceWorker( HashSet<(string, bool)> servicesNames, Loggers loggers ) =>
            ( _servicesNames, _loggers ) = ( servicesNames, loggers );

        public void Check()
        {
            foreach( var service in CreateServices() ) {
                CheckService( service );
            }
        }

        private void CheckService( IService service )
        {
            try {
                DoCheckService( service, 0 );
            }
            catch( Exception e ) {
                _loggers.Error( e );
            }
        }

        private void DoCheckService( IService service, int attempt )
        {
            attempt++;
            var data = service.Status.Execute();
            var status = GetPrettyStatus( data );
            _loggers.Info( $"{service.Name} {status}" );
            if( status != Running || service.IsNeedRestart( RestartTime ) ) {
                
                _loggers.Info( $"{service.Name} Restarting", true );
                service.Restart.Execute();
                var newStatus = GetPrettyStatus( service.Status.Execute() );
                if( newStatus == Running ) {
                    _loggers.Info( $"{service.Name} Ok", true );
                    return;
                }
                if( attempt <= RestartAttempts ) {
                    DoCheckService( service, attempt );
                }
            }
        }
        
        private static string GetPrettyStatus( string data )
        {
            var m = Pattern.Match( data );
            if( m.Success == false ) {
                return "Output not match!";
            }

            return m.Groups[ "status2" ].Value;
        }
        
        private IEnumerable<IService> CreateServices() => _servicesNames.Select( ServiceBuilder.Create ).ToList();
    }
}