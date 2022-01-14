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
using TheStoreRepositoryFromFront = TheStore.Api.Front.Data.Repositories.TheStoreRepository;

namespace TheStore.Api.Core.Sources.Workers
{
    internal sealed class ShopHandler : ShopHandlerBase
    {
        
        private readonly DateTime _startDate;
        private readonly ElasticSearchClient<IIndexedEntities> _client;

        public ShopHandler( ProcessShopContext context, ProcessorSettings settings, DbHelper dbHelper, ProductRatingCalculation productRatingCalculation )
            :base( context, settings.ElasticSearchClientSettings, dbHelper, productRatingCalculation ) {
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

        private void UpdateProducts( List<Product> products )
        {

            WriteProducts(products);
            
            SetProgress( 100 );
            Thread.Sleep( 5000 );
            AddMessage( "Update all products complete" );
            Context.Content = $"{ products.Count } products";
        }

        private void WriteProducts(List<Product> products) {

            var productsCategoryMapping = GetProductsWithCategoryMapping(products);
            var clearedProductList = GetClearedProductList(products, productsCategoryMapping);

            DoWriteProducts(clearedProductList);
            DoWriteProductsWithCategoryMapping(productsCategoryMapping);
        }

        private Dictionary<string,int> GetCategoryMappingDictionary() => GetCategoryMapping().ToDictionary(k => k.ShopCategoryId, v => v.LocalCategoryId);

        private void DoWriteProductsWithCategoryMapping(List<Product> products) {
            if(products.Any()) {
                var iProducts = products.Select(p => (IProductForIndexWithCategories)p).ToList();
                _client.BulkAllWithCategories(iProducts);
                AddMessage($"Update products with categorymapping complete");
            }
        }

        private void DoWriteProducts(List<Product> products) {
            var iProducts = products.Select(p => (IProductForIndex)p).ToList();
            _client.BulkAllTyped(iProducts);
            AddMessage($"Update products without categorymapping complete");
        }

        private List<Product> GetClearedProductList(List<Product> allProducts, List<Product> productsWithCategoryMapping) {
            var withCategoryMappingIds = productsWithCategoryMapping.Select(p => p.Id).ToHashSet();
            return allProducts.Where(p => withCategoryMappingIds.Contains(p.Id) == false).ToList();
        }

        private List<Product> GetProductsWithCategoryMapping(List<Product> products) {

            var maps = GetCategoryMappingDictionary();

            var productsCategoryMapping = new List<Product>();

            foreach(var product in products) {
                if( maps.ContainsKey(product.OriginalCategoryId) == false) {
                    continue;
                }

                product.Categories = new[] { maps[product.OriginalCategoryId].ToString() };

                productsCategoryMapping.Add(product);
            }

            return productsCategoryMapping;
        }
    }
}