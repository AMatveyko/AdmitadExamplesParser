// a.snegovoy@gmail.com

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Common.Elastic.Workers;
using Common.Entities;
using Common.Helpers;

using TheStore.Api.Core.Sources.Entities;

namespace TheStore.Api.Core.Sources.Workers
{
    internal sealed class UrlStatisticsWithQueues : IUrlStatisticsWorker
    {

        private static readonly ConcurrentDictionary<string, Queue<UrlStatisticsQueueData>> Queues = new();
        private readonly IUrlStatisticsWorker _worker;

        private static object _lockFlag = new();  

        public UrlStatisticsWithQueues( UrlStatisticsIndexClient client ) =>
            _worker = new UrlStatisticsWorker( client );
        
        public void Update( string url, BotType botType, short? errorCode )
        {
            var data = GetData( url, botType, errorCode );
            EnqueueAndStartWorkIfNeed( data );
        }

        private void EnqueueAndStartWorkIfNeed( UrlStatisticsQueueData data )
        {
            var id = HashHelper.GetMd5Hash( data.Url );
            lock( _lockFlag ) {
                if( Queues.ContainsKey( id ) == false ) {
                    Queues[ id ] = new ();
                    Queues[id].Enqueue(data);
                    StartWork( Queues[id], id );
                }
                else {
                    Queues[id].Enqueue( data );
                }
            }
        }

        private void StartWork( Queue<UrlStatisticsQueueData> queue, string id ) =>
            Task.Factory.StartNew( () => Work( queue, () => DeleteQueue(id) ) );

        private void Work( Queue<UrlStatisticsQueueData> queue, Action deleteQueue ) {
            while( queue.Count > 0 ) {
                var data = queue.Dequeue();
                _worker.Update( data.Url, data.BotType, data.ErrorCode );
            }

            deleteQueue();
        }

        private void DeleteQueue( string id ) {
            lock( _lockFlag ) {
                Queues.Remove( id, out var deletedQueue );
            }
        }
        
        private static UrlStatisticsQueueData GetData(
            string url,
            BotType botType,
            short? errorCode ) =>
            new() {
                Url = url,
                BotType = botType,
                ErrorCode = errorCode
            };
        
        public void AddUrls( List<string> urls ) => _worker.AddUrls( urls );
    }
}