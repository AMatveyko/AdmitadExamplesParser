// a.snegovoy@gmail.com

using Admitad.Converters.Workers;
using AdmitadCommon.Entities;

using AdmitadSqlData.Helpers;

using Common.Api;
using Common.Settings;

namespace TheStore.Api.Core.Sources.Workers
{
    internal sealed class CountryWorker : BaseLinkWorker
    {
        public CountryWorker( ElasticSearchClientSettings settings, BackgroundWorks works, DbHelper dbHelper, ProductRatingCalculation productRatingCalculation )
            : base( settings, works, dbHelper, productRatingCalculation ) { }

        public void LinkAll( CountriesLinkContext context )
        {
            var countries = Db.GetCountries();
            var linker = CreateLinker( context );
            linker.LinkCountries( countries );
        }
    }
}