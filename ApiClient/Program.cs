using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

using AdmitadSqlData.Helpers;

using ApiClient.Responces;

using Common.Workers;

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

        private static TopContext _lastResult;

        private readonly static List<string> _ratingCalculationMessages = new ();

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

        private static void DoWork( string[] args ) {
            
            var apiClient = new ApiCoreClient( new RequestSettings( "http://localhost:8080", ErrorLogger ) );

            // SendMessage("test");

            if( args.Contains("index") || args.Contains("linkAll")) {
                RunIndexOrLinking(args, apiClient);
            }
            
            if(args.Contains("ratingCalculation")) {
                RunRatingCalculation(apiClient);
            }

            FlushListingCash();

        }

        private static void RunIndexOrLinking(string[] args, ApiCoreClient apiCoreClient) {
            var statBefore = apiCoreClient.GetTotalStatistics();
            var report = new Report
            {
                TotalBefore = statBefore.Products,
                SoldOutBefore = statBefore.SoldOut
            };

            Func<TopContext> func = args.Length == 0 || args[0] == "index"
                ? apiCoreClient.RunAndCheckIndex
                : apiCoreClient.RunAndCheckLinkAll;

            RunAndCheck(func, SaveLastTopContext);

            if (args.Length == 0 || args[0] == "index")
            {
                IndexWork(report, apiCoreClient);
            }
            else
            {
                SendMessage("Завершили линковку");
            }
        }

        private static void RunAndCheck<T>(
            Func<T> func, Action<T> responceHandler ) where T : Context {

            var iterationCount = 0;
            var isFinish = false;

            while( isFinish == false ) {
                iterationCount++;
                Console.Write( $"{iterationCount} " );
                var response = func();
                responceHandler(response);
                isFinish = response.IsFinished || response.IsError;
                Thread.Sleep( 30000 );
            }

        }
        
        private static void SaveLastTopContext( TopContext context ) => _lastResult = context;

        private static void RunRatingCalculation(ApiCoreClient apiCoreClient) {
            RunAndCheck(apiCoreClient.RunRatingCalculation, CheckRatingCalculationResult);
            SendMessage(CompileRatingCalculationMessage());
        }

        private static string CompileRatingCalculationMessage() =>
            string.Join("\n", _ratingCalculationMessages.Select(m => m.Replace("Info: ", string.Empty).Replace(" time 0",string.Empty)));

        private static void CheckRatingCalculationResult(Context context) {
            foreach( var message in context.Messages) {
                if( _ratingCalculationMessages.Contains(message) == false) {
                    _ratingCalculationMessages.Add(message);
                }
            }
        }

        private static void FlushListingCash()
        {
            var settings = SettingsBuilder.GetApiClientSettings();
            var connectionString =
                $"Server={settings.Host};User ID={settings.UserName};Password={settings.Password};Database={settings.DatabaseName}";
            using var connection = new MySqlConnection( connectionString );
            connection.Open();
            var commandString = $"TRUNCATE TABLE {settings.ListingCashTable}"; 
            using var command = new MySqlCommand( commandString, connection );
            command.ExecuteScalar();
        }
        
        private static void IndexWork( Report report, ApiCoreClient apiCoreClient )
        {
            IndexLogger.Info( JsonConvert.SerializeObject( _lastResult ) );
            
            report.IsError = _lastResult.IsError || _lastResult.Contexts.Any( c => c.IsError );


            var statAfter = apiCoreClient.GetTotalStatistics( _lastResult.StartDate );

            report.TotalAfter = statAfter.Products;
            report.SoldOutAfter = statAfter.SoldOut;
            report.ProductsForDisable = statAfter.CountForSoldOut;
            
            var shopStats = apiCoreClient.GetShopStatistics();
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
            var builder = new SettingsBuilder( new DbHelper( SettingsBuilder.GetDbSettings() ) );
            var settings = builder.GetMessengerSettings();
            var messenger = new Messenger.Messenger( settings );
            messenger.Send( message );
        }
    }
}