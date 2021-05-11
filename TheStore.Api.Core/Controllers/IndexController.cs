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
        private readonly BackgroundWorks _works;
        
        public IndexController( ProcessorSettings settings, BackgroundWorks works )
        {
            _works = works;
            _settings = settings;
        }

        [ HttpGet ]
        [ Route( "FillBrandId" ) ]
        public IActionResult FillBrandId( string clearlyName, bool clean = true )
        {
            var context = new FillBrandIdContext( clearlyName );
            var worker = new BrandWorker( _settings, _works );
            return _works.AddToQueue( worker.FillBrandId, context, QueuePriority.Low, clean );
        }
        
        [ HttpGet ]
        [ Route( "ShowWorks" ) ]
        public IActionResult ShowWorks() => _works.ShowAllWorks();

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
            var worker = new IndexWorker( _settings, context, _works );
            return _works.AddToQueue( worker.Index, context, QueuePriority.Parallel, clean );
        }

        [ HttpGet ]
        [ Route( "IndexAllShops" ) ]
        public IActionResult IndexAllShops( bool clean = true )
        {
            var context = new IndexAllShopsContext();
            var worker = new IndexWorker( _settings, context, _works );
            return _works.AddToQueue( worker.IndexAll, context, QueuePriority.Parallel, clean );
        }
        
        [ HttpGet ]
        [ Route("RelinkTag") ]
        public IActionResult RelinkTag( int id, bool clean = true )
        {
            var context = new RelinkTagContext( id.ToString() );
            var worker = new TagsWorker( _settings.ElasticSearchClientSettings, _works );
            return _works.AddToQueue( worker.RelinkTag, context, QueuePriority.Low, clean );
        }
        
        [ HttpGet ]
        [ Route("RelinkCategory") ]
        public IActionResult RelinkCategory( string id, bool clean = true )
        {
            var context = new RelinkCategoryContext( id );
            var worker = new CategoryWorker( _settings.ElasticSearchClientSettings, _works );
            return _works.AddToQueue( worker.RelinkCategory, context, QueuePriority.Low, clean );
        }

        [ HttpGet ]
        [ Route( "RelinkAllCategories" ) ]
        public IActionResult RelinkAllCategories( bool clean = true )
        {
            var context = new RelinkAllCategories();
            var worker = new CategoryWorker( _settings.ElasticSearchClientSettings, _works );
            return _works.AddToQueue( worker.RelinkAllCategories, context, QueuePriority.Parallel, clean );
        }

        [ HttpGet ]
        [ Route( "LinkTags" ) ]
        public IActionResult LinkTags( bool clean = false )
        {
            var context = new LinkTagsContext( "" );
            var worker = new TagsWorker( _settings.ElasticSearchClientSettings, _works );
            return _works.AddToQueue( worker.LinkTags, context, QueuePriority.Low, clean );
        }

    }
}