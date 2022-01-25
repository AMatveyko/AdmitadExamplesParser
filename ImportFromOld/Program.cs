using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using Admitad.Converters.Workers;

using Common.Api;
using Common.Entities;
using Common.Settings;

using ImportFromOld.Data;
using ImportFromOld.Data.Entity;
using ImportFromOld.Entities;

namespace ImportFromOld
{
    class Program
    {
        
        static void Main( string[] args )
        {

            var elasticSettings = new ElasticSearchClientSettings {
                ComponentForIndex = ComponentType.ElasticSearch,
                DefaultIndex = "products-old-1",
                ElasticSearchUrl = "http://185.221.152.127:9200",
                FrameSize = 20000,
                ShopName = "OldTheStore"
            };
            
            var swCash = new Stopwatch();
            swCash.Start();
            
            var db1 = new ImportDbContext();

            var cashTask = Task.Run( Cash.FillCash );
            var offerTask = Task.Run(
                () => db1.Offers.Where( o => o.Enabled == 1 ).Select(
                    o => new OfferId {
                        Id = o.Id,
                        Photo1 = o.Photo1
                    } ).ToList() );
            
            Task.WaitAll( cashTask, offerTask );
            //Task.WaitAll(  offerTask );
            var allIds = offerTask.Result;
            
            //Cash.FillCash();

            // var offersByPhoto = db.Offers.Where( o => o.Enabled == 1 ).Select(
            //     o => new OfferId {
            //         Id = o.Id,
            //         Photo1 = o.Photo1
            //     } ).ToList();
            
            var groups = allIds.GroupBy( oi => oi.Photo1 ).Select( g => g.ToList() ).ToList();

            swCash.Stop();
            var timeCash = swCash.ElapsedMilliseconds;
            
            var skip = 0;
            var step = 15000;
            while( skip <= groups.Count ) {

                var swIteration = new Stopwatch();
                swIteration.Start();
                
                var ids = groups.Skip( skip ).Take( step )
                    .SelectMany( g => g.Select( o => o.Id ) ).ToHashSet();
                
                //var part = db.Offers.Where( o => ids.Contains( o.Id ) ).ToList();
                
                var db2 = new ImportDbContext();
                
                var part = new List<OfferOld>();
                var querySkip = 0;
                var queryStep = 50000;
                while( querySkip < ids.Count ) {
                    
                    var idsPart = ids.Skip( querySkip ).Take( queryStep ).ToList();
                    part.AddRange( db2.Offers.Where( o => idsPart.Contains( o.Id ) ) );
                    
                    querySkip += queryStep;
                }
                
                var partCash = Cash.Values.Where( p => ids.Contains( p.Key ) )
                    .ToDictionary( p => p.Key, p => p.Value );
                
                var cleanOffers = part.Where( o => string.IsNullOrWhiteSpace( o.Photo1 ) == false ).ToList();
                if( cleanOffers.Any() == false ) {
                    skip += step;
                    continue;
                }
                var products = Converter.GetProducts( cleanOffers, partCash );

                var elasticClient = new ElasticSearchClient<Product>(
                    elasticSettings,
                    new BackgroundBaseContext( "id", "name" ) );
                
                elasticClient.DoBulkAllForImport( products );
                skip += step;
                
                swIteration.Stop();
                var time = swIteration.ElapsedMilliseconds;
            }
        }
    }
}