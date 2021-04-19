// a.snegovoy@gmail.com
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
        [ Route("RelinkTag") ]
        public IActionResult RelinkTag( int id, bool clean = true )
        {
            var context = new RelinkTagContext {
                TagId = id.ToString()
            };
            var worker = new TagsWorker( _settings.ElasticSearchClientSettings );
            return BackgroundWorks.Run( worker.RelinkTag, context, clean );
        }
        
        [ HttpGet ]
        [ Route("RelinkCategory") ]
        public IActionResult RelinkCategory( string id, bool clean = true )
        {
            var context = new RelinkCategoryContext() {
                CategoryId = id
            };
            var worker = new CategoryWorker( _settings.ElasticSearchClientSettings );
            return BackgroundWorks.Run( worker.RelinkTag, context, clean );
        }

        [ HttpGet ]
        [ Route( "LinkTags" ) ]
        public IActionResult LinkTags( bool clean = false )
        {
            var context = new LinkTagsContext();
            var worker = new TagsWorker( _settings.ElasticSearchClientSettings );
            return BackgroundWorks.Run( worker.LinkTags, context, clean );
        }

    }
}