// a.snegovoy@gmail.com

using System;
using System.Threading.Tasks;

using AdmitadCommon.Entities;
using AdmitadCommon.Entities.Api;

using Microsoft.AspNetCore.Mvc;

using TheStore.Api.Core.Sources.Workers;

namespace TheStore.Api.Core.Controllers
{
    [ ApiController ]
    [ Route( "[controller]" ) ]
    public class IndexController : ControllerBase
    {

        private readonly ProcessorSettings _settings;
        
        public IndexController( ProcessorSettings settings )
        {
            _settings = settings;
        }

        [ HttpGet ]
        [ Route( "ShowWorks" ) ]
        public IActionResult ShowWorks() => BackgroundWorks.ShowAllWorks();

        [ HttpGet ]
        [ Route( "GetStatistics" ) ]
        public async Task<IActionResult> GetStatistics( DateTime? startPoint = null )
        {
            return await StatisticsWorker.GetTotalStatistics( startPoint, _settings.ElasticSearchClientSettings );
        }
        
        [ HttpGet ]
        [ Route( "GetShopStatistics" ) ]
        public IActionResult GetShopStatistics()
        {
            return StatisticsWorker.GetShopStatistics();
        }
        
        [ HttpGet ]
        [ Route( "IndexShop" ) ]
        public IActionResult IndexShop( int id, bool downloadFresh = false, bool clean = true )
        {
            var context = new IndexShopContext( id, downloadFresh );
            var worker = new IndexWorker( _settings, context );
            return BackgroundWorks.AddToQueue( worker.Index, context, QueuePriority.Parallel, clean );
        }

        [ HttpGet ]
        [ Route( "IndexAllShops" ) ]
        public IActionResult IndexAllShops( bool clean = true )
        {
            var context = new IndexAllShopsContext();
            var worker = new IndexWorker( _settings, context );
            return BackgroundWorks.AddToQueue( worker.IndexAll, context, QueuePriority.Parallel, clean );
        }
        
        [ HttpGet ]
        [ Route("RelinkTag") ]
        public IActionResult RelinkTag( int id, bool clean = true )
        {
            var context = new RelinkTagContext( id.ToString() );
            var worker = new TagsWorker( _settings.ElasticSearchClientSettings );
            return BackgroundWorks.AddToQueue( worker.RelinkTag, context, QueuePriority.Low, clean );
        }
        
        [ HttpGet ]
        [ Route("RelinkCategory") ]
        public IActionResult RelinkCategory( string id, bool clean = true )
        {
            var context = new RelinkCategoryContext( id );
            var worker = new CategoryWorker( _settings.ElasticSearchClientSettings );
            return BackgroundWorks.AddToQueue( worker.RelinkCategory, context, QueuePriority.Low, clean );
        }

        [ HttpGet ]
        [ Route( "LinkTags" ) ]
        public IActionResult LinkTags( bool clean = false )
        {
            var context = new LinkTagsContext( "" );
            var worker = new TagsWorker( _settings.ElasticSearchClientSettings );
            return BackgroundWorks.AddToQueue( worker.LinkTags, context, QueuePriority.Low, clean );
        }

    }
}