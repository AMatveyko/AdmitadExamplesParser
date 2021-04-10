// a.snegovoy@gmail.com

using System;
using System.Collections.Generic;

using AdmitadCommon.Entities;

namespace AdmitadExamplesParser.Workers.Components
{
    internal interface IElasticClient<T>
    {
        void IndexMany( IEnumerable<T> entities );
        void Bulk( IEnumerable<T> entities );
        void BulkAll( IEnumerable<T> entities );
        string UpdateProductsForCategory( Category category );
        string UpdateProductsForCategoryNew( Category category );
        long CountProductsForCategory( Category category );
        long CountProductsForCategoryNew( Category category );

        string UpdateProductsForCategoryFieldNameModel(
            Category category );

        long CountProductsForCategoryFieldNameModel(
            Category category );
        long CountProductsWithCategory( string categoryId );
        string UpdateProductsForTag( Tag tag );

        string LinkProductsByProperty(
            BaseProperty property );
        long CountProductsWithTag( string tagId );
        long CountProductsForTag( Tag tag );
        void BulkLinkedData( List<LinkedData> data );
        string DisableOldProducts( DateTime indexTime );
        string UnlinkProductsByProperty( BaseProperty property );
    }
}