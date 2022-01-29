using System.Collections.Generic;
using System.Linq;

using AdmitadApi.Entities;
using AdmitadApi.Pub;

namespace AdmitadApiWorker
{
    class Program
    {

        static void Main( string[] args )
        {

            var api = new AdmitadApiClient();
            var sites = api.GetSites();
            var theStoreId = GetTheShoreSite( sites );
            var compaigns = api.GetCompaigns( theStoreId );
        }

        private static int GetTheShoreSite(
            IEnumerable<WebSite> sites ) =>
            sites.First( s => s.Name.ToLower() == "thestore" && s.IsActive ).Id;

    }
}