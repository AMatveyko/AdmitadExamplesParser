// a.snegovoy@gmail.com

namespace AdmitadApi.Requests
{
    internal sealed class CampaignsList : BaseApiRequest
    {

        private readonly int _siteId;

        public CampaignsList(
            int siteId ) =>
            _siteId = siteId;

        protected override string EntryPoint => $"advcampaigns/website/{_siteId}/";
    }
}