// a.snegovoy@gmail.com

using System;
using Admitad.Converters.Workers;
using AdmitadSqlData.Helpers;

using Common.Api;
using Common.Settings;

namespace TheStore.Api.Core.Sources.Workers
{
    internal sealed class ProductsHandler : BaseLinkWorker
    {

        private readonly BackgroundBaseContext _context;

        public ProductsHandler(
            ElasticSearchClientSettings settings,
            BackgroundBaseContext context,
            BackgroundWorks works,
            DbHelper db,
            ProductRatingCalculation productRatingCalculation)
            : base( settings, works, db, productRatingCalculation )
        {
            _context = context;
        }
        
        public void SellShopProducts( SellShopProductsContext context )
        {
            CheckContextType( context );
            var client = CreateElasticClient( context );
            var result = client.DisableShopProducts( context.ShopId.ToString() );
            context.Content = $"Disabled {result.Pretty}";
        }

        public void UnlinkShop( UnlinkShopContext context )
        {
            CheckContextType( context );
            var client = CreateElasticClient( context );
            var result = client.UnlinkShop( context.ShopId );
            context.Content = $"Unlinked {result.Pretty} products";
        }
        
        private void CheckContextType< T >( T context )
        {
            if( ReferenceEquals( context, _context ) == false ) {
                throw new ArgumentException( "Разные контексты!" );
            }
        }
        
    }
}