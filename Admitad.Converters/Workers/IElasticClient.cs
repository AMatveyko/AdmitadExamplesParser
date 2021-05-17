// a.snegovoy@gmail.com

using System;
using System.Collections.Generic;

using AdmitadCommon;
using AdmitadCommon.Entities;
using AdmitadCommon.Entities.Responses;

namespace Admitad.Converters.Workers
{
    public interface IElasticClient<T>
    {
        void IndexMany( IEnumerable<T> entities );
        void Bulk( IEnumerable<T> entities );
        void BulkAll( IEnumerable<T> entities );
        UpdateResult UpdateProductsForCategory( Category category );
        UpdateResult UpdateProductsForCategoryNew( Category category );
        long CountProductsForCategory( Category category );
        long CountProductsForCategoryNew( Category category );

        UpdateResult UpdateProductsForCategoryFieldNameModel(
            Category category );

        long CountProductsForCategoryFieldNameModel(
            Category category );
        long CountProductsWithCategory( string categoryId );
        UpdateResult UpdateProductsForTag( Tag tag );

        UpdateResult LinkProductsByProperty(
            BaseProperty property );
        long CountProductsWithTag( string tagId );
        long CountProductsForTag( Tag tag );
        void BulkLinkedData( List<LinkedData> data );
        UpdateResult DisableOldProducts( DateTime indexTime, string shopId );
        UpdateResult UnlinkProductsByProperty( BaseProperty property );
        public long GetCountAllDocuments();

        UpdateResult UnlinkCategory(
            Category category );

        UpdateResult UnlinkTag(
            Tag tag );

        long CountForDisableProducts(
            DateTime indexTime,
            string shopId );

        long CountProductsForShop(
            string shopId );

        long CountDisabledProductsByShop(
            string shopId );

        long CountProducts();
        long CountSoldOutProducts();

        ProductResponse GetProduct(
            string id );

        UpdateResult UpdateProductsFroCountry(
            Country country );
    }
}