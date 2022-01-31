// a.snegovoy@gmail.com

using System.Collections.Generic;
using System.Linq;

using Common.Api;
using Common.Entities;
using Common.Settings;

using Nest;

namespace Common.Elastic.Workers
{
    public sealed class UrlStatisticsIndexClient : IndexClientBase
    {
        public UrlStatisticsIndexClient(
            ElasticSearchClientSettings settings,
            BackgroundBaseContext context )
            : base( settings, context, "urlstatistics" ) { }

        public UrlStatisticEntry Get( string id ) =>
            Client.Get<UrlStatisticEntry>( id ).Source;

        public void Insert( UrlStatisticEntry entry ) =>
            Client.IndexDocument( entry );

        public void Update( UrlStatisticEntry entry ) {
            var result = Client.Update<UrlStatisticEntry>( entry.Id, i => i.Doc( entry ) );
        }

        public void Insert( List<UrlStatisticEntry> entries )
        {
            var from = 0;
            while( from < entries.Count ) {
                var part = entries.Skip(from).Take( Settings.FrameSize );
                var result = Client.Bulk( s => s.CreateMany( part ) );
                from += Settings.FrameSize;
            }
        }
    }
}