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
        
        public ShopHandler( ProcessShopContext context, ProcessorSettings settings ) {
            _context = context;
            _settings = settings;
            _statistics = new ShopProcessingStatistics( context.DownloadInfo, AddMessage, Logger );
            _startDate = DateTime.Now;
        }

        public void Process()
        {
            SetProductsStatistics( "Before" );
            var shopData = _statistics.GetShopData( ParseShop );
            var cleanOffers = CleanOffers( shopData );
            var products = ConvertOffers( cleanOffers );
            UpdateProducts( products );
            DisableProductsIfNeed();
            Finish();
            // Finish( products );
        }

        private void Finish()
        // private void Finish( List<Product> products )
        {
            // _statistics.FillCategories( products );
            SetProductsStatistics( "After" );
            DbHelper.WriteShopStatistics( _statistics );
        }
        
        private void DisableProductsIfNeed()
        {
            if( _context.NeedSoldOut == false ) {
                return;
            }
            var client = CreateElasticClient( _context );
            var result = client.DisableOldProducts( _startDate, _context.ShopId.ToString() );
            AddMessage( $"Disable { result.Pretty } products", result.IsError );
        }
        
        private ShopData ParseShop() {
            var parser = new GeneralParser(
                _context.DownloadInfo.FilePath,
                _context.DownloadInfo.NameLatin,
                _context,
                _settings.EnableExtendedStatistics );
            var shopData = parser.Parse();
            SetProgress( 20 );
            AddMessage( "Parsing complete" );
            return shopData;
        }

        private IEnumerable<Offer> CleanOffers( ShopData shopData )
        {
            var cleaner = new OfferConverter( shopData, _context ); 
            var cleanOffers = cleaner.GetCleanOffers();
            SetProgress( 40 );
            AddMessage( "Clearing offers complete" );
            return cleanOffers;
        }

        private List<Product> ConvertOffers( IEnumerable<Offer> offers )
        {
            var products = ProductConverter.GetProductsContainer( offers ); 
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

        private void SetProductsStatistics( string condition )
        {
            var client = CreateElasticClient( _context );
            var count = client.CountProductsForShop( _context.ShopId.ToString() );
            var soldout = client.CountDisabledProductsByShop( _context.ShopId.ToString() );
            switch( condition ) {
                case "Before" :
                    _statistics.SetProductStatisticsBefore( (int)count, (int)soldout );
                    break;
                case "After" :
                    _statistics.SetProductsStatisticsAfter( (int)count, (int)soldout );
                    break;
            }
        }
        
        private void SetProgress( int percents ) => _context.SetProgress( percents, 100 );

        private void AddMessage( string text, bool isError = false ) => _context.AddMessage( text, isError ); 
        
        private ElasticSearchClient<IIndexedEntities> CreateElasticClient( BackgroundBaseContext context ) =>
            new ( _settings.ElasticSearchClientSettings, context );
    }
}