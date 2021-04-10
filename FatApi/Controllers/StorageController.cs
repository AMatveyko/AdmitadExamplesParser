// a.snegovoy@gmail.com

using AdmitadCommon.Entities;

using ElasticSearchData;

using FatApi.Model;
using FatApi.Workers;

using Microsoft.AspNetCore.Mvc;

namespace FatApi.Controllers
{
    [ ApiController ]
    [ Route( "Api/[controller]/[action]" ) ]
    public class StorageController : ControllerBase
    {

        private readonly IProductFactory _factory;

        public StorageController( IProductFactory factory ) {
            _factory = factory;
        }
        
        [ HttpGet ]
        [ HttpPost ]
        public Product GetProduct( string id )
        {
            return _factory.Get( id );
        }



        [ HttpGet ]
        [ HttpPost ]
        public ProductSearchResult ProductSearch( SearchParameters parameters, int offSet, int frameSize )
        {
            return _factory.ProductSearch( parameters, offSet, frameSize );
        }
        
    }
}