// a.snegovoy@gmail.com

using Common.Api;
using Common.Elastic.Workers;
using Common.Entities;

using Microsoft.AspNetCore.Mvc;
using TheStore.Api.Core.Sources.Workers;
using TheStore.Api.Front.Data.Repositories;

namespace TheStore.Api.Core.Controllers
{
    [ ApiController ]
    [ Route( "[controller]" ) ]
    public class UtilsController : ControllerBase
    {

        private readonly TheStoreRepository _repository;
        private readonly BackgroundWorks _works;
        private readonly UrlStatisticsIndexClient _indexClient;

        public UtilsController( TheStoreRepository repository, BackgroundWorks works, UrlStatisticsIndexClient indexClient ) =>
            ( _repository, _works, _indexClient ) = ( repository, works, indexClient );
        
        [ HttpGet ]
        public IActionResult CompareProjects( bool clean = true )
        {
            return _works.AddToQueue(
                new UtilsWorker( _repository ).ComparePages,
                new CompareProjectsContext(),
                QueuePriority.Low,
                clean );
        }
    }
}