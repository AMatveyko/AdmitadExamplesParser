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
            return GetUrls( parameters.BotType, parameters.Url );
        }

        private List<string> GetUrls( BotType botType, string url )
        {
            var urls = _worker.GetUrls( botType, url );
            foreach( var entry in urls ) {
                SetShownDate( entry, botType );
                RunUpdateShowUrl( entry );
            }
            return urls.Select( u => u.Url ).ToList();
        }

        private void RunUpdateShowUrl( UrlStatisticEntry entry ) {
            RunInTask( () => DoRunUpdate( entry.Id, () => _worker.UpdateEntry( entry )));
        }
        
        private void RunUpdateVisitUrl( UrlStatisticsParameters parameters )
        {
            var id = HashHelper.GetMd5Hash( parameters.Url );
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
        
    }
}