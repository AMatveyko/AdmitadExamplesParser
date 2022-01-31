// a.snegovoy@gmail.com

using System;
using System.Collections.Generic;
using System.Linq;

using Common.Elastic.Workers;
using Common.Entities;
using Common.Helpers;

namespace TheStore.Api.Core.Sources.Workers
{

    internal sealed class UrlStatisticsWorker : IUrlStatisticsWorker
    {
        private readonly UrlStatisticsIndexClient _client;

        public UrlStatisticsWorker( UrlStatisticsIndexClient client ) => _client = client;

        public void Update(
            string url,
            BotType botType,
            short? errorCode )
        {
            var code = errorCode ?? 200;
            var entry = GetEntry( url );
            if( entry == null ) {
                CreateAndInsert( url, botType, code );
            }
            else {
                UpdateEntry( entry, botType, code );
            }
        }

        public void AddUrls( List<string> urls )
        {
            var entries = urls.Select( u => new UrlStatisticEntry( u ) ).ToList();
            _client.Insert( entries );
        }
        
        private void CreateAndInsert( string url, BotType botType, short errorCode )
        {
            var entry = new UrlStatisticEntry( url );
            SetData( entry, errorCode, botType );
            _client.Insert( entry );
        }

        private static void SetData( UrlStatisticEntry entry, short errorCode, BotType botType )
        {
            switch( botType ) {
                case BotType.Yandex:
                    SetYandex( entry, errorCode );
                    break;
                case BotType.Google:
                    SetGoogle( entry, errorCode );
                    break;
            }
        }
        
        private static void SetYandex( IUrlStatisticsEntryYandex entry, short errorCode )
        {
            entry.NumberVisitsYandex = entry.NumberVisitsYandex == null ? 1 : entry.NumberVisitsYandex + 1;
            entry.LastErrorCodeYandex = errorCode;
            entry.LastVisitDateYandex = DateTime.Now;
        }

        private static void SetGoogle( IUrlStatisticsEntryGoogle entry, short errorCode )
        {
            entry.NumberVisitsGoogle = entry.NumberVisitsGoogle == null ? 1 : entry.NumberVisitsGoogle + 1;
            entry.LastErrorCodeGoogle = errorCode;
            entry.LastVisitDateGoogle = DateTime.Now;
        }

        private void UpdateEntry( UrlStatisticEntry entry, BotType botType, short errorCode )
        {
            SetData( entry, errorCode, botType );
            _client.Update( entry );
        }
        
        private UrlStatisticEntry GetEntry( string url )
        {
            var id = HashHelper.GetMd5Hash( url );
            return _client.Get( id );
        }


    }
}