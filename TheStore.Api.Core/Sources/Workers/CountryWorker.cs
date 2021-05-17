// a.snegovoy@gmail.com

using AdmitadCommon.Entities;
using AdmitadCommon.Entities.Api;

using AdmitadSqlData.Helpers;

namespace TheStore.Api.Core.Sources.Workers
{
    internal sealed class CountryWorker : BaseLinkWorker
    {
        public CountryWorker( ElasticSearchClientSettings settings, BackgroundWorks works )
            : base( settings, works ) { }

        public void LinkAll( CountriesLinkContext context )
        {
            var countries = DbHelper.GetCountries();
            var linker = CreateLinker( context );
            linker.LinkCounties( countries );
        }
    }
}