using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

using Admitad.Converters;

using AdmitadCommon.Entities.Settings;

using ApiClient.Responces;

using MySqlConnector;

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


        private static void RunAndCheck(
            Func<TopContext> func )
        {
            var iterationCount = 0;
            while( _finish == false ) {
                iterationCount++;
                Console.Write( $"{iterationCount} " );
                var response = func();
                _lastResult = response;
                _finish = response.IsFinished || response.IsError;
                Thread.Sleep( 30000 );
            }

        }

        static void Main(
            string[] args )
        {
            try {
                DoWork( args );
            }
            catch( Exception e ) {
                ErrorLogger.Error( e );
            }
        }

        private static void DoWork( string[] args )
        {
            
            var apiClient = new ApiClient( new RequestSettings( "http://localhost:8080", ErrorLogger ) );
            
            var statBefore = apiClient.GetTotalStatistics();
            var report = new Report {
                TotalBefore = statBefore.Products,
                SoldOutBefore = statBefore.SoldOut
            };
            
            Func<TopContext> func = args.Length == 0 || args[ 0 ] == "index"
                ? apiClient.RunAndCheckIndex
                : apiClient.RunAndCheckLinkAll;  
            
            RunAndCheck( func );
            
            if( args.Length == 0 || args[ 0 ] == "index" ) {
                IndexWork( report, apiClient );
            }
            else {
                SendMessage( "Завершили линковку" );
            }
            
            FlushListingCash();
        }
        
        private static void FlushListingCash()
        {
            const string settingsPath = @"o:\admitad\workData\settings.json";
            var settings = JsonConvert.DeserializeObject<TotalSettings>( File.ReadAllText( settingsPath ) );
            var connectionString =
                $"Server={settings.MySQLHost};User ID={settings.MySQLUser};Password={settings.MySQLPassword};Database={settings.MySQLDatabase}";
            using var connection = new MySqlConnection( connectionString );
            connection.Open();
            var commandString = $"TRUNCATE TABLE {settings.MySQLListingCashTable}"; 
            using var command = new MySqlCommand( commandString, connection );
            command.ExecuteScalar();
        }
        
        private static void IndexWork( Report report, ApiClient apiClient )
        {
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