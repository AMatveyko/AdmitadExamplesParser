// a.snegovoy@gmail.com

using System;

using AdmitadCommon.Entities.Api;

using AdmitadSqlData.Helpers;

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
            DbHelper db )
            : base( settings, works, db )
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

        private void CheckContextType< T >( T context )
        {
            if( ReferenceEquals( context, _context ) == false ) {
                throw new ArgumentException( "Разные контексты!" );
            }
        }
        
    }
}