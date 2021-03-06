// a.snegovoy@gmail.com

using System;
using System.Threading.Tasks;
using Admitad.Converters.Workers;
using AdmitadCommon.Entities;

using AdmitadSqlData.Helpers;

using Common.Api;
using Common.Elastic.Workers;
using Common.Entities;
using Common.Settings;

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
        private readonly DbHelper _dbHelper;
        private readonly ProductRatingCalculation _productRatingCalculation;

        public IndexController( ProcessorSettings settings, BackgroundWorks works, DbHelper dbHelper, ProductRatingCalculation productRatingCalculation )
        {
            _works = works;
            _settings = settings;
            _dbHelper = dbHelper;
            _productRatingCalculation = productRatingCalculation;
        }

        [ HttpGet ]
        [ Route( "FillBrandId" ) ]
        public IActionResult FillBrandId( string clearlyName, bool clean = true )
        {
            var context = new FillBrandIdContext( clearlyName );
            var worker = new BrandWorker( _settings, _works, _dbHelper, _productRatingCalculation);
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
        public IActionResult GetShopStatistics() =>
            new StatisticsWorker( _dbHelper ).GetShopStatistics();

        [HttpGet]
        [Route("CalculateProductsRatings")]
        public IActionResult CalculateProductsRatings( bool clean = true ) {
            var context = new RatingCalculationContext();
            var worker = new RatingsCalculationWorker(_productRatingCalculation, _settings);
            return _works.AddToQueue(worker.Calculate, context, QueuePriority.Medium, clean);
        }
        
        [ HttpGet ]
        [ Route( "IndexShop" ) ]
        public IActionResult IndexShop(
            int id,
            bool downloadFresh = false,
            bool needLink = false,
            bool needSoldOut = true,
            bool clean = true )
        {
            var context = new IndexShopContext( id, downloadFresh, needLink, needSoldOut );
            var worker = CreateIndexWorker(context);
            return _works.AddToQueue( worker.Index, context, QueuePriority.Parallel, clean );
        }

        [ HttpGet ] 
        [ Route( "IndexAllShops" ) ]
        public IActionResult IndexAllShops( bool clean = true )
        {
            var context = new IndexAllShopsContext();
            var worker = CreateIndexWorker(context);
            return _works.AddToQueue( worker.IndexAll, context, QueuePriority.Parallel, clean );
        }

        [ HttpGet ]
        [ Route( "SellShopProducts" ) ]
        public IActionResult SellShopProducts( int shopId, bool clean = true )
        {
            var context = new SellShopProductsContext( shopId );
            var worker = new ProductsHandler( _settings.ElasticSearchClientSettings, context, _works, _dbHelper, _productRatingCalculation);
            return _works.AddToQueue( worker.SellShopProducts, context, QueuePriority.Low, clean );
        }

        [ HttpGet ]
        [ Route( "UnlinkShop" ) ]
        public IActionResult UnlinkShop( int shopId, bool clean = true )
        {
            var context = new UnlinkShopContext( shopId );
            var worker = new ProductsHandler( _settings.ElasticSearchClientSettings, context, _works, _dbHelper, _productRatingCalculation);
            return _works.AddToQueue( worker.UnlinkShop, context, QueuePriority.Low, clean );
        }
        
        [ HttpGet ]
        [ Route("RelinkTag") ]
        public IActionResult RelinkTag( int id, bool relink = false, bool clean = true )
        {
            var context = new RelinkTagContext( id.ToString(), relink );
            var worker = new TagsWorker( _settings.ElasticSearchClientSettings, _works, _dbHelper, _productRatingCalculation);
            return _works.AddToQueue( worker.RelinkTag, context, QueuePriority.Low, clean );
        }
        
        [ HttpGet ]
        [ Route("RelinkCategory") ]
        public IActionResult RelinkCategory( string id, bool relink = false, bool clean = true )
        {
            var context = new RelinkCategoryContext( id, relink );
            var worker = new CategoryWorker( _settings.ElasticSearchClientSettings, _works, _dbHelper, _productRatingCalculation);
            return _works.AddToQueue( worker.RelinkCategory, context, QueuePriority.Low, clean );
        }

        // [ HttpGet ]
        // [ Route( "RelinkAllCategories" ) ]
        // public IActionResult RelinkAllCategories( bool clean = true )
        // {
        //     var context = new RelinkAllCategories();
        //     var worker = new CategoryWorker( _settings.ElasticSearchClientSettings, _works );
        //     return _works.AddToQueue( worker.RelinkAllCategories, context, QueuePriority.Parallel, clean );
        // }

        [ HttpGet ]
        [ Route( "LinkTags" ) ]
        public IActionResult LinkTags( bool clean = false )
        {
            var context = new LinkTagsContext( "" );
            var worker = new TagsWorker( _settings.ElasticSearchClientSettings, _works, _dbHelper, _productRatingCalculation);
            return _works.AddToQueue( worker.LinkTags, context, QueuePriority.Low, clean );
        }

        [ HttpGet ]
        [ Route( "LinkAll" ) ]
        public IActionResult LinkAll( bool clean = false )
        {
            var context = new LinkAllContext();
            return _works.AddToQueue(
                CreateIndexWorker(context).LinkAll,
                context, QueuePriority.Parallel,
                clean );
        }

        private IndexWorker CreateIndexWorker(ParallelBackgroundContext context) =>
            new IndexWorker(_settings, context, _works, _dbHelper, _productRatingCalculation);

    }
}