// a.snegovoy@gmail.com

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Common.Api;
using Common.Elastic.Workers;
using Common.Entities;
using Common.Workers;

using NUnit.Framework;

using TheStore.Api.Front.Data.Helpers;
using TheStore.Api.Front.Data.Repositories;
using TheStore.Api.Front.Entity;
using TheStore.Api.Front.Workers;

namespace UrlStatisticsTests
{
    public class UrlStatisticsWorkerTests
    {

        private static IUrlStatisticsWorker GetWorker()
        {
            var dbSettings = SettingsBuilder.GetDbSettings();
            var settings = new SettingsBuilder( new TheStoreRepository( dbSettings ) ).GetSettings()
                .ElasticSearchClientSettings;
            var client = new UrlStatisticsIndexClient( settings, new BackgroundBaseContext( "", "" ) );
            // return new UrlStatisticsWorker( client );
            return new UrlStatisticsWithQueues( client );
        }
        
        [ Test ]
        public void ManyUpdatesTest()
        {
            var worker = GetWorker();
            var url1 = "https://thestore.ru/dlya/zhenschin/odezhda/bluzki_rubashki/kupit_atlasnye/";
            var url2 = "https://thestore.ru/dlya/zhenschin/odezhda/bruki/";
            var url3 = "https://thestore.ru/dlya/zhenschin/odezhda/tolstovki_svitshoty/";
            var botType = BotType.Yandex;
            var actions = new List<Action> {
                () => Work( url1, botType, 1 ),
                () => Work( url2, botType, 1 ),
                () => Work( url1, botType, 1 ),
                () => Work( url2, botType, 1 ),
                () => Work( url1, botType, 2 ),
                () => Work( url2, botType, 2 ),
                () => Work( url1, botType, 1 ),
                () => Work( url2, botType, 1 ),
                () => Work( url1, botType, 3 ),
                () => Work( url2, botType, 3 ),
                () => Work( url1, botType, 1 ),
                () => Work( url2, botType, 1 ),
                () => Work( url1, botType, 1 ),
                () => Work( url2, botType, 1 ),
                () => Work( url1, botType, 2 ),
                () => Work( url1, botType, 1 ),
                () => Work( url1, botType, 1 ),
                () => Work( url1, botType, 1 ),
                () => Work( url1, botType, 3 ),
                () => Work( url1, botType, 1 ),
                () => Work( url1, botType, 3 ),
                () => Work( url2, botType, 2 ),
                () => Work( url2, botType, 1 ),
                () => Work( url2, botType, 1 ),
                () => Work( url2, botType, 1 ),
                () => Work( url2, botType, 1 ),
                () => Work( url2, botType, 3 ),
                () => Work( url2, botType, 1 ),
                () => Work( url1, botType, 2 ),
                () => Work( url2, botType, 1 ),
            };

            actions.ForEach( RunInTask );
            
            Thread.Sleep(60000);
        }

        private static void Work( string url, BotType botType, int count )
        {
            var worker = GetWorker();
            for( var i = 0; i < count; i++ ) {
                worker.Update( new UrlStatisticsParameters( url, botType, null, 5, null ) );
            }
        }

        private static void RunInTask( Action action ) =>
            Task.Factory.StartNew( action );

        [ Test ]
        public void FillDb()
        {
            var repository = RepositoryFabric.GetUrlStatisticsRepository();
            var entries = repository.GetAll().Select( e => e.Url ).ToList();
            var uniqEntries = entries.Distinct().ToList();
            var grouped = entries.GroupBy( e => e ).OrderByDescending( e => e.Count() )
                .ToDictionary( e => e, e => e.Count() );
            var worker = GetWorker();
            var sw = new Stopwatch();
            sw.Start();
            worker.AddUrls( entries );
            sw.Stop();
            var sec = sw.ElapsedMilliseconds / 1000;
        }

    }
}