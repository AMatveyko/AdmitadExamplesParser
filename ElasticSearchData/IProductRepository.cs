// a.snegovoy@gmail.com

using System.Collections.Generic;

using AdmitadCommon.Entities;

namespace ElasticSearchData
{
    public interface IProductRepository
    {
        Product GetProduct( string id );

        List<Product> ProductSearch( SearchParameters parameters, int offSet, int size );
    }
}