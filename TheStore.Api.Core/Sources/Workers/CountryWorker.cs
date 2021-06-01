// a.snegovoy@gmail.com

using AdmitadCommon.Entities;
using AdmitadCommon.Entities.Api;

using AdmitadSqlData.Helpers;

namespace TheStore.Api.Core.Sources.Workers
{
    internal sealed class CountryWorker : BaseLinkWorker
    {
        public CountryWorker( ElasticSearchClientSettings settings, BackgroundWorks works, DbHelper dbHelper )
            : base( settings, works, dbHelper ) { }

        public void LinkAll( CountriesLinkContext context )
        {
            var countries = Db.GetCountries();
            var linker = CreateLinker( context );
            linker.LinkCountries( countries );
        }
    }
}