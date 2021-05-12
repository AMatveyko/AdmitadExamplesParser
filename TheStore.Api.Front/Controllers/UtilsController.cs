// a.snegovoy@gmail.com

using Microsoft.AspNetCore.Mvc;

using TheStore.Api.Front.Data.Repositories;
using TheStore.Api.Front.Workers;

namespace TheStoreApi.Controllers
{
    [ ApiController ]
    [ Route( "[controller]" ) ]
    public class UtilsController : ControllerBase
    {

        private readonly TheStoreRepository _repository;

        public UtilsController(
            TheStoreRepository repository ) =>
            _repository = repository;
        
        [ HttpGet ]
        public IActionResult ComparePages( int? visits = null )
        {
            return new UtilsWorker( _repository ).ComparePages( visits );
        }
    }
}