using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

using Admitad.Converters;

using AdmitadCommon.Entities;

using ApiClient.Responces;

using Messenger;

using Newtonsoft.Json;

using NLog;

using Web.Common.Entities;

namespace ApiClient
{
    static class Program
    {
        
        private static readonly Logger IndexLogger = LogManager.GetLogger( "IndexLogger" );
        private static readonly Logger StatisticsLogger = LogManager.GetLogger( "StatisticsLogger" );
        private static readonly Logger ErrorLogger = LogManager.GetLogger( "ErrorLogger" );

        private static readonly Regex ShopPattern =
            new(@"Info: Всего (?<numberShops>\d+) c ошибками \d+ time \d+", RegexOptions.Compiled);
        private static readonly Regex ShopNamePattern =
            new(@"Error: (?<shopName>.+): ClosedStore time \d+", RegexOptions.Compiled);
        
        private static bool _finish;
        private static TopContext _lastResult;

        static void Main( string[] args )
        {


            var apiClient = new ApiClient( new RequestSettings( "http://localhost:8080", ErrorLogger ) );
            
            var statBefore = apiClient.GetTotalStatistics();
            var report = new Report {
                TotalBefore = statBefore.Products,
                SoldOutBefore = statBefore.SoldOut
            };

            var iterationCount = 0;
            while( _finish == false ) {
                iterationCount++;
                Console.Write( $"{iterationCount } " );
                var response = apiClient.RunAndCheckIndex();
                _lastResult = response;
                _finish = response.IsFinished || response.IsError;
                Thread.Sleep( 30000 );
            }
            
            IndexLogger.Info( JsonConvert.SerializeObject( _lastResult ) );
            
            report.IsError = _lastResult.IsError || _lastResult.Contexts.Any( c => c.IsError );


            var statAfter = apiClient.GetTotalStatistics( _lastResult.StartDate );

            report.TotalAfter = statAfter.Products;
            report.SoldOutAfter = statAfter.SoldOut;
            report.ProductsForDisable = statAfter.CountForSoldOut;
            
            var shopStats = apiClient.GetShopStatistics();
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
            var settings = SettingsBuilder.GetMessengerSettings();
            var messenger = new Messenger.Messenger( settings );
            messenger.Send( message );
        }
    }
}