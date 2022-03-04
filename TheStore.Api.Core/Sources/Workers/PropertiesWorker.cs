// a.snegovoy@gmail.com

using Admitad.Converters.Workers;
using AdmitadCommon.Entities;

using AdmitadSqlData.Helpers;

using Common.Api;
using Common.Settings;

namespace TheStore.Api.Core.Sources.Workers
{
    internal sealed class PropertiesWorker : BaseLinkWorker
    {
        public PropertiesWorker(
            ElasticSearchClientSettings settings, BackgroundWorks works, DbHelper dbHelper, ProductRatingCalculation productRatingCalculation )
            : base( settings, works, dbHelper, productRatingCalculation ) { }

        public void LinkProperties( LinkPropertiesContext context )
        {
            var colors = Db.GetColors();
            var materials = Db.GetMaterials();
            var sizes = Db.GetSizes();
            var linker = CreateLinker( context );
            linker.LinkProperties( colors: colors, materials: materials, sizes: sizes );
        }
        
        public void UnlinkProperties( UnlinkPropertiesContext context )
        {
            var colors = Db.GetColors();
            var materials = Db.GetMaterials();
            var sizes = Db.GetSizes();
            var linker = CreateLinker( context );
            linker.UnlinkProperties( colors: colors, materials: materials, sizes: sizes );
        }
    }
}