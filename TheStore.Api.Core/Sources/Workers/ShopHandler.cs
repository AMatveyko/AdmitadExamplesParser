// a.snegovoy@gmail.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using Admitad.Converters;
using Admitad.Converters.Workers;

using AdmitadCommon.Entities;
using AdmitadCommon.Entities.Api;

using AdmitadSqlData.Helpers;

namespace TheStore.Api.Core.Sources.Workers
{
    internal sealed class ShopHandler
    {

        private readonly ProcessShopContext _context;
        private readonly ProcessorSettings _settings;
        
        public ShopHandler( ProcessShopContext context, ProcessorSettings settings ) {
            _context = context;
            _settings = settings;
        }

        public void Process()
        {
            var startDate = DateTime.Now;
            SetStatistics( "Before" );
            var shopData = ParseShop();
            AddMessage( "Parsing complete" );
            var cleanOffers = CleanOffers( shopData );
            AddMessage( "Clearing offers complete" );
            var products = ConvertOffers( cleanOffers );
            AddMessage( "Convert to products complete" );
            UpdateProducts( products );
            AddMessage( "Update products complete" );
            Thread.Sleep( 5000 );
            DisableProducts( startDate );
            SetStatistics( "After" );
            _context.Content = $"{products.Count} products";
        }

        private void DisableProducts( DateTime startDate )
        {
            var client = CreateElasticClient( _context );
            var result = client.DisableOldProducts( startDate, _context.ShopId.ToString() );
            AddMessage( $"Disable { result.Pretty } products", result.IsError );
        }
        
        private ShopData ParseShop() {
            var parser = new GeneralParser(
                _context.FilePath,
                _context.ShopName,
                _context,
                _settings.EnableExtendedStatistics );
            var shopData = parser.Parse();
            SetProgress( 20 );
            return shopData;
        }

        private IEnumerable<Offer> CleanOffers( ShopData shopData )
        {
            var cleaner = new OfferConverter( shopData, _context ); 
            var cleanOffers = cleaner.GetCleanOffers();
            SetProgress( 40 );
            return cleanOffers;
        }

        private List<Product> ConvertOffers( IEnumerable<Offer> offers )
        {
            var products = ProductConverter.GetProductsContainer( offers ); 
            SetProgress( 80 );
            return products;
        }

        private void UpdateProducts( IEnumerable<Product> products )
        {
            var iProducts = products.Select( p => ( IProductForIndex ) p ).ToList();
            var client = CreateElasticClient( _context );
            client.BulkAll( iProducts );
            SetProgress( 100 );
        }

        private void SetStatistics( string condition )
        {
            var client = CreateElasticClient( _context );
            var statistics = DbHelper.GetShopStatistics( _context.ShopId );
            if( condition == "Before" ) {
                var count = client.CountProductsForShop( _context.ShopId.ToString() );
                var soldout = client.CountDisabledProductsByShop( _context.ShopId.ToString() );
                statistics.TotalBefore = (int) count;
                statistics.SoldoutBefore = ( int ) soldout;
            }

            if( condition == "After" ) {
                var count = client.CountProductsForShop( _context.ShopId.ToString() );
                var soldout = client.CountDisabledProductsByShop( _context.ShopId.ToString() );
                statistics.TotalAfter = (int) count;
                statistics.SoldoutAfter = ( int ) soldout;
            }
            
            DbHelper.UpdateShopStatistics( statistics );
        }
        
        private void SetProgress( int percents ) => _context.SetProgress( percents, 100 );

        private void AddMessage( string text, bool isError = false ) => _context.AddMessage( text, isError ); 
        
        private ElasticSearchClient<IIndexedEntities> CreateElasticClient( BackgroundBaseContext context ) =>
            new ( _settings.ElasticSearchClientSettings, context );
    }
}