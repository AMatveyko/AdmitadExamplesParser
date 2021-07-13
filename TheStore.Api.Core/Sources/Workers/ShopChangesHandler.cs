// a.snegovoy@gmail.com

using System.Collections.Generic;
using System.Linq;

using Admitad.Converters;

using AdmitadSqlData.Helpers;

using Common.Api;
using Common.Elastic.Workers;
using Common.Entities;
using Common.Settings;

namespace TheStore.Api.Core.Sources.Workers
{
    internal sealed class ShopChangesHandler : ShopHandlerBase
    {

        private readonly IIndexClient _client;

        public ShopChangesHandler(
            ProcessShopContext context,
            ElasticSearchClientSettings settings,
            DbHelper dbHelper )
            : base( context, settings, dbHelper )
        {
            _client = IndexClient.CreateIndexClient( settings, context );
        }
            
        
        protected override void DoProcess( ShopData shopData )
        {
            //var shopData = GetShopData();
            DeleteOffers( shopData );
            var newOffers = CleanOffers( shopData ).ToList();
            InsertOffers( newOffers );
        }

        protected override IClientForShopStatistics GetClient() => _client;

        private void InsertOffers( IReadOnlyCollection<Offer> newOffers )
        {
            
            if( newOffers.Any() == false ) {
                return;
            }
            
            var products = ConvertOffers( newOffers );
            var productsIds = products.Select( p => p.Id ).ToArray();
            var productsParamsFromIndex = _client.GetProductsByIds( productsIds );
            var mergedProducts = MergeProducts( products, productsParamsFromIndex );
            _client.UpsertProducts( mergedProducts );
        }

        private List<Product> MergeProducts(
            List<Product> products,
            IReadOnlyList<ProductPart> productsFromIndex )
        {
            if( productsFromIndex.Any() == false ) {
                return products;
            }

            var partDictionary = productsFromIndex.ToDictionary( k => k.Id, v => v );
            foreach( var product in products.Where( p => partDictionary.ContainsKey( p.Id ) ) ) {
                ProductConverter.MergePartInToProduct( product, partDictionary[product.Id] );
            }

            CalculateProducts( 
                products.Select( p => p.Id ).ToHashSet(), 
                partDictionary.Keys.ToHashSet() );
            
            return products;
            
        }

        private void CalculateProducts( IReadOnlyCollection<string> productsFromOffers, IReadOnlySet<string> productsFromDb )
        {
            var newProducts = productsFromOffers.Count( p => productsFromDb.Contains( p ) == false );
            var updatedProducts = productsFromOffers.Count - newProducts;
            Context.AddMessage( $"New products: {newProducts}, Updated products { updatedProducts} " );
        }

        private void DeleteOffers( IShopDataWithDeletedOffers shopData )
        {
            if( shopData.DeletedOffers.Any() == false ) {
                return;
            }
            
            
            var deletedIds = shopData.DeletedOffers.Select( o => o.OfferId ).ToHashSet();
            var productsForDelete = _client.SearchProductsByOffersIds( deletedIds.ToArray() );
            foreach( var product in productsForDelete ) {
                DeleteOffersFromProduct( product, deletedIds );
            }

            var numberEnded = productsForDelete.Count( p => p.Soldout == 1 );
            AddMessage( $"Number of times sold: { numberEnded }" );
            AddMessage( "Updating products after deleting offers" );
            
            if( productsForDelete.Any() ) {
                _client.UpdateProductsAfterDeletingOffers( productsForDelete );
            }
        }

        private static void DeleteOffersFromProduct( ProductPart product, IReadOnlySet<string> deletedIds )
        {
            product.OfferIds = product.OfferIds.Where( o => deletedIds.Contains( o ) == false ).ToArray();
            var soldOut = product.OfferIds.Any() ? 0 : 1;
            product.Soldout = ( byte ) soldOut;
        }
 
    }
}