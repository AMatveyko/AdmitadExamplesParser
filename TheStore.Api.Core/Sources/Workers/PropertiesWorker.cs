// a.snegovoy@gmail.com

using AdmitadCommon.Entities;
using AdmitadCommon.Entities.Api;

using AdmitadSqlData.Helpers;

using Common.Settings;

namespace TheStore.Api.Core.Sources.Workers
{
    internal sealed class PropertiesWorker : BaseLinkWorker
    {
        public PropertiesWorker(
            ElasticSearchClientSettings settings, BackgroundWorks works, DbHelper dbHelper )
            : base( settings, works, dbHelper ) { }

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