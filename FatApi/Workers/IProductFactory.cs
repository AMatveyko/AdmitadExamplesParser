// a.snegovoy@gmail.com

using AdmitadCommon.Entities;

using ElasticSearchData;

using FatApi.Model;

namespace FatApi.Workers
{
    public interface IProductFactory
    {
        Product Get(
            string id );

        ProductSearchResult ProductSearch(
            SearchParameters parameters,
            int offSet,
            int frameSize );
    }
}