// a.snegovoy@gmail.com

using System.Collections.Generic;

using AdmitadCommon.Entities;

namespace ElasticSearchData
{
    public interface IOfferRepository
    {
        List<Offer> GetByProductId( string id );

        List<Offer> OffersSearch( SearchParameters parameters );
    }
}