// a.snegovoy@gmail.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using Admitad.Converters.Workers;

using AdmitadSqlData.Helpers;

using Common.Api;
using Common.Entities;
using Common.Settings;

namespace TheStore.Api.Core.Sources.Workers
{
    internal sealed class ShopHandler : ShopHandlerBase
    {
        
        private readonly DateTime _startDate;
        private readonly ElasticSearchClient<IIndexedEntities> _client;

        public ShopHandler( ProcessShopContext context, ProcessorSettings settings, DbHelper dbHelper )
            :base( context, settings.ElasticSearchClientSettings, dbHelper ) {
            _startDate = DateTime.Now;
            _client = new ElasticSearchClient<IIndexedEntities>( Settings, context );
        }

        protected override void DoProcess( ShopData shopData )
        {
            var cleanOffers = CleanOffers( shopData );
            var products = ConvertOffers( cleanOffers );
            UpdateProducts( products );
            DisableProductsIfNeed();
        }

        protected override IClientForShopStatistics GetClient() => _client;

        private void DisableProductsIfNeed()
        {
            if( Context.NeedSoldOut == false ) {
                return;
            }
            
            var result = _client.DisableOldProducts( DateTime.Now.AddDays( -1 ).Date, Context.ShopId.ToString() );
            AddMessage( $"Disable { result.Pretty } products", result.IsError );
        }

        private void UpdateProducts( IEnumerable<Product> products )
        {
            var iProducts = products.Select( p => ( IProductForIndex ) p ).ToList();
            _client.BulkAll( iProducts );
            SetProgress( 100 );
            Thread.Sleep( 5000 );
            AddMessage( "Update products complete" );
            Context.Content = $"{ iProducts.Count } products";
        }

    }
}