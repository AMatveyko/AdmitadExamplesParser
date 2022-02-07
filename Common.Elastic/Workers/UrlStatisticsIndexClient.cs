// a.snegovoy@gmail.com

using System;
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

        public UrlStatisticEntry Get(
            string id ) =>
            Client.Get<UrlStatisticEntry>( id ).Source;

        public void Insert(
            UrlStatisticEntry entry ) =>
            Client.IndexDocument( entry );

        public void Update(
            UrlStatisticEntry entry )
        {
            var result = Client.Update<UrlStatisticEntry>( entry.Id, i => i.Doc( entry ) );
        }

        public void Insert(
            List<UrlStatisticEntry> entries )
        {
            var from = 0;
            while( from < entries.Count ) {
                var part = entries.Skip( from ).Take( Settings.FrameSize );
                var result = Client.Bulk( s => s.CreateMany( part ) );
                from += Settings.FrameSize;
            }
        }

        public void UpdateShown( List<UrlStatisticEntry> entries )
        {
            var result = Client.Bulk( b => b.UpdateMany( entries, (
                descriptor,
                entry ) => descriptor.Doc( entry ) ));
        }
        
        public List<UrlStatisticEntry> GetUrlsInfos(
            string domain,
            string vertical,
            string fieldName,
            DateTime olderThan,
            int size )
        {
            var query = $"({domain}) AND ({vertical})";
            
            var result = Client.Search<UrlStatisticEntry>( s =>
                s.Source( s =>
                        s.Includes( f => f.Field( i => i.Url)))
                    .Size( size )
                    .Query( q => 
                        q.FunctionScore( fs => fs.Functions( f => f.RandomScore() ).Query(
                                fq =>
                                    fq.Bool( b =>
                                        b.Must(
                                            m1 => m1.QueryString( qs => qs.Query(query).Fields( f => f.Field(i => i.Url )) ),
                                            m2 => m2.Bool( m2b =>
                                                m2b.Should(
                                                    sh1 => sh1.Bool( sh1b =>
                                                        sh1b.MustNot( mn => mn.Exists( e => e.Field( fieldName )))),
                                                    sh2 => sh2.DateRange( dr => dr.Field(fieldName).LessThan(olderThan)
                                                    )
                                                    ))
                                        ))
                            )
                        ) 
                    ));
            return result.Documents.ToList();
        }

        public List<string> GetUrlsForChecking(BotType botType, int size) {
            const int dateRange = 30;
            var fieldName = botType switch {
                BotType.Yandex => "dateLastIndexCheckYandex",
                BotType.Google => "dateLastIndexCheckGoogle",
                _ => throw new ArgumentException("unavailable bot")
            };
            var result = Client.Search<UrlStatisticEntry>(s =>
                    s.Query(q =>
                            q.Bool(b =>
                                b.Should(bs =>
                                        bs.Bool(bbs =>
                                            bbs.MustNot(bbsm =>
                                                bbsm.Exists(e =>
                                                    e.Field(fieldName)
                                                )
                                            )
                                        ),
                                    bs2 =>
                                        bs2.DateRange(r =>
                                            r.LessThan(DateTime.Now.AddDays(-dateRange)).Field(fieldName)))))
                        .Size(size).Source(s => s.Includes(i => i.Field(e => e.Url))));

            return result.Documents.Select(d => d.Url).ToList();
        }

    }
}