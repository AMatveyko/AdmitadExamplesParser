// a.snegovoy@gmail.com

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Common.Elastic.Workers;
using Common.Entities;
using Common.Helpers;

using TheStore.Api.Front.Entity;

namespace TheStore.Api.Front.Workers
{
    internal sealed class UrlStatisticsWithQueues : IUrlStatisticsWorker
    {
        private static readonly ConcurrentDictionary<string, Queue<Action>> Queues = new ConcurrentDictionary<string, Queue<Action>>();
        private readonly UrlStatisticsWorker _worker;

        private static readonly object LockFlag = new object();  

        public UrlStatisticsWithQueues( UrlStatisticsIndexClient client ) =>
            _worker = new UrlStatisticsWorker( client );

        public List<string> Update( UrlStatisticsParameters parameters )
        {
            RunUpdateVisitUrl( parameters );
            return GetUrls( parameters.BotType, parameters.Url, parameters.UrlNumber );
        }

        private List<string> GetUrls( BotType botType, string url, short urlNumber )
        {
            var urls = _worker.GetUrls( botType, url, urlNumber );
            foreach( var entry in urls ) {
                SetShownDate( entry, botType );
                RunUpdateEntry( entry );
            }
            return urls.Select( u => u.Url ).ToList();
        }

        private void RunUpdateEntry( UrlStatisticEntry entry ) {
            RunInTask( () => DoRunUpdate( entry.Id, () => _worker.UpdateEntry( entry )));
        }
        
        private void RunUpdateVisitUrl( UrlStatisticsParameters parameters )
        {
            var id = GetId(parameters.Url);
            RunInTask( () => DoRunUpdate( id, () => _worker.Update( parameters ) ) );
        }

        private static void DoRunUpdate( string id, Action action )
        {
            lock( LockFlag ) {
                if( Queues.ContainsKey( id ) == false ) {
                    Queues[id] = new Queue<Action>();
                    Queues[id].Enqueue( action );
                    StartWork( Queues[id], id );
                }
                else {
                    Queues[id].Enqueue( action );
                }
            }
        }

        private static void StartWork(
            Queue<Action> queue,
            string id ) =>
            RunInTask( () => Work( queue, () => DeleteQueue( id ) ) );

        private static void Work( Queue<Action> queue, Action deleteQueue ) {
            while( queue.Count > 0 ) {
                var action = queue.Dequeue();
                action();
            }

            deleteQueue();
        }

        private static void DeleteQueue( string id ) {
            lock( LockFlag ) {
                Queues.Remove( id, out var deletedQueue );
            }
        }

        public void AddUrls( List<string> urls ) => _worker.AddUrls( urls );
        public void SaveCheckingResult(List<UrlIndexInfo> infos) {
            var entries = infos.Select(r => GetIndexCheckedEntry(r.Url, r.IsIndexed));
            foreach (var entry in entries) {
                RunUpdateEntry(entry);
            }
        }

        private static void RunInTask(
            Action action ) =>
            Task.Factory.StartNew( action );

        private static void SetShownDate(
            IUrlStatisticsEntryShown entry,
            BotType botType )
        {
            switch( botType ) {
                case BotType.Yandex:
                    entry.DateLastShowYandex = DateTime.Now;
                    break;
                case BotType.Google:
                    entry.DateLastShowGoogle = DateTime.Now;
                    break;
                default:
                    return;
            }
        }

        private static UrlStatisticEntry GetIndexCheckedEntry(string url, bool isIndexed) =>
            new UrlStatisticEntry(url) {
                DateLastIndexCheckYandex = DateTime.Now,
                IndexedYandex = isIndexed
            };
        
        private static string GetId(string url) => HashHelper.GetMd5Hash(url);
        
    }
}