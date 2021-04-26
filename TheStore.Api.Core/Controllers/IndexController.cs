// a.snegovoy@gmail.com

using System.ComponentModel;

using AdmitadCommon.Entities;

using Microsoft.AspNetCore.Mvc;

using TheStore.Api.Core.Sources.Entity;
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
        [ Route( "FillBrandId" ) ]
        public IActionResult FillBrandId( string clearlyName, bool clean = true )
        {
            var context = new FillBrandIdContext( clearlyName );
            var worker = new BrandWorker( _settings );
            return BackgroundWorks.AddToQueue( worker.FillBrandId, context, QueuePriority.Low, clean );
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
            var context = new LinkTagsContext();
            var worker = new TagsWorker( _settings.ElasticSearchClientSettings );
            return BackgroundWorks.AddToQueue( worker.LinkTags, context, QueuePriority.Low, clean );
        }

    }
}