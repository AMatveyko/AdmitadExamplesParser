// a.snegovoy@gmail.com

using System.Collections.Generic;
using System.Linq;

using AdmitadApi.Entities;
using AdmitadApi.Helpers;
using AdmitadApi.Requests;
using AdmitadApi.Workers;

namespace AdmitadApi.Pub
{
    public sealed class AdmitadApiClient
    {

        private readonly Executor _executor = new ();

        public List<WebSite> GetSites()
        {
            var result = _executor.Execute<List<WebSiteResponse>>( new WebSitesList() );
            return result.Select( Converter.Convert ).ToList();
        }

        public List<Compaign> GetCompaigns(
            int siteId ) =>
            _executor.Execute<CompaingListResponse>( new CampaignsList( siteId ) ).Result
                .Select( Converter.Convert ).ToList();
    }
}