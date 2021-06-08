// a.snegovoy@gmail.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using Admitad.Converters;
using Admitad.Converters.Workers;

using AdmitadCommon.Entities;
using AdmitadCommon.Entities.Api;
using AdmitadCommon.Entities.Statistics;

using AdmitadSqlData.Helpers;

using Common.Settings;
using Common.Workers;

using NLog;

namespace TheStore.Api.Core.Sources.Workers
{
    internal sealed class ShopHandler
    {
        
        private static readonly Logger Logger = LogManager.GetLogger( "ErrorLogger" );

        private readonly ProcessShopContext _context;
        private readonly ProcessorSettings _settings;
        private readonly ShopProcessingStatistics _statistics;
        private readonly DateTime _startDate;
        private readonly DbHelper _dbHelper;
        
        public ShopHandler( ProcessShopContext context, ProcessorSettings settings, DbHelper dbHelper ) {
            _context = context;
            _settings = settings;
            _statistics = new ShopProcessingStatistics( context.DownloadInfo, AddMessage, Logger );
            _startDate = DateTime.Now;
            _dbHelper = dbHelper;
        }

        public void Process()
        {
            var shopData = _statistics.GetShopData( ParseShop );
            var cleanOffers = CleanOffers( shopData );
            var products = ConvertOffers( cleanOffers, shopData.Weight );
            UpdateProducts( products );
            DisableProductsIfNeed();
            Finish();
        }

        private void Finish()
        {
            SetProductsStatistics();
            _dbHelper.WriteShopStatistics( _statistics );
        }
        
        private void DisableProductsIfNeed()
        {
            if( _context.NeedSoldOut == false ) {
                return;
            }
            var client = CreateElasticClient( _context );
            // > TODO
            //var result = client.DisableOldProducts( _startDate, _context.ShopId.ToString() );
            var result = client.DisableOldProducts( DateTime.Now.AddDays( -1 ).Date, _context.ShopId.ToString() );
            AddMessage( $"Disable { result.Pretty } products", result.IsError );
        }
        
        private ShopData ParseShop() {
            var parser = new GeneralParser(
                //_context.DownloadInfo.FilePath,
                //_context.DownloadInfo.NameLatin,
                _context.DownloadInfo,
                _context,
                _settings.EnableExtendedStatistics );
            var shopData = parser.Parse();
            SetProgress( 20 );
            AddMessage( "Parsing complete" );
            return shopData;
        }

        private IEnumerable<Offer> CleanOffers( ShopData shopData )
        {
            var cleaner = new OfferConverter( shopData, _dbHelper, _context ); 
            var cleanOffers = cleaner.GetCleanOffers();
            SetProgress( 40 );
            AddMessage( "Clearing offers complete" );
            return cleanOffers;
        }

        private List<Product> ConvertOffers( IEnumerable<Offer> offers, int shopWeight )
        {
            var calculation = new RatingCalculation( shopWeight );
            var products =
                new ProductConverter( _dbHelper, calculation ).GetProductsContainer( offers ); 
            SetProgress( 80 );
            AddMessage( "Convert to products complete" );
            return products;
        }

        private void UpdateProducts( IEnumerable<Product> products )
        {
            var iProducts = products.Select( p => ( IProductForIndex ) p ).ToList();
            var client = CreateElasticClient( _context );
            client.BulkAll( iProducts );
            SetProgress( 100 );
            Thread.Sleep( 5000 );
            AddMessage( "Update products complete" );
            _context.Content = $"{ iProducts.Count } products";
        }

        private void SetProductsStatistics()
        {
            var client = CreateElasticClient( _context );
            var count = client.CountProductsForShop( _context.ShopId.ToString() );
            var soldout = client.CountDisabledProductsByShop( _context.ShopId.ToString() );
            _statistics.SetProductsStatistics( (int)count, (int)soldout );
        }
        
        private void SetProgress( int percents ) => _context.SetProgress( percents, 100 );

        private void AddMessage( string text, bool isError = false ) => _context.AddMessage( text, isError ); 
        
        private ElasticSearchClient<IIndexedEntities> CreateElasticClient( BackgroundBaseContext context ) =>
            new ( _settings.ElasticSearchClientSettings, context );
    }
}