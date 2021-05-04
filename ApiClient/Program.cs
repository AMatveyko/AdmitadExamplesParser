using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

using Admitad.Converters;

using AdmitadCommon.Entities;

using Messenger;

using Newtonsoft.Json;

using NLog;

namespace ApiClient
{
    class Program
    {
        
        private static readonly Logger IndexLogger = LogManager.GetLogger( "IndexLogger" );
        private static readonly Logger StatisticsLogger = LogManager.GetLogger( "StatisticsLogger" );

        private static readonly Regex ShopPattern =
            new(@"Info: Всего (?<numberShops>\d+) c ошибками \d+ time \d+", RegexOptions.Compiled);
        private static readonly Regex ShopNamePattern =
            new(@"Error: (?<shopName>.+): ClosedStore time \d+", RegexOptions.Compiled);
        
        private static bool _finish;
        private static TopContext _lastResult;

        static void Main( string[] args )
        {
            
            var statBefore = ApiClient.GetTotalStatistics();
            var report = new Report {
                TotalBefore = statBefore.Products,
                SoldOutBefore = statBefore.SoldOut
            };
            
            
            while( _finish == false ) {
                var response = ApiClient.RunAndCheckIndex();
                _lastResult = response;
                _finish = response.IsFinished || response.IsError;
                Thread.Sleep( 30000 );
            }
            
            IndexLogger.Info( JsonConvert.SerializeObject( _lastResult ) );
            
            report.IsError = _lastResult.IsError || _lastResult.Contexts.Any( c => c.IsError );


            var statAfter = ApiClient.GetTotalStatistics();
            report.TotalAfter = statAfter.Products;
            report.SoldOutAfter = statAfter.SoldOut;
            
            var shopStats = ApiClient.GetShopStatistics();
            StatisticsLogger.Info( JsonConvert.SerializeObject( shopStats ) );
            
            report.TotalShops = shopStats.TotalEnabledShops;

            var allMessages = _lastResult.Messages;
            allMessages.AddRange( _lastResult.Contexts.SelectMany( c => c.Messages ) );
            report.WorkErrors = allMessages.Count( m => m.Contains( "Error" ) );

            foreach( var message in allMessages ) {
                var m = ShopPattern.Match( message );
                if( m.Success == false ) {
                    continue;
                }
                report.DownloadedShops = int.Parse( m.Groups[ "numberShops" ].Value );
                break;
            }

            foreach( var message in allMessages ) {
                var m = ShopNamePattern.Match( message );
                if( m.Success == false ) {
                    continue;
                }
                report.ClosedStores.Add( m.Groups["shopName"].Value );
            }

            report.Time = _lastResult.Time;
            
            SendMessage( report.ToString() );

        }

        private static void SendMessage( string message )
        {
            var settings = SettingsBuilder.GetSettings();

            var messengerSettings = new MessengerSettings();
            foreach( var client in new IClientSettings[] { settings.TelegramSettings } ) {
                messengerSettings.Clients.Add( client );
            }
            var messenger = new Messenger.Messenger( messengerSettings );
            messenger.Send( message );
        }
    }
}