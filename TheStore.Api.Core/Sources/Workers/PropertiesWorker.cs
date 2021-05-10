// a.snegovoy@gmail.com

using Admitad.Converters;

using AdmitadCommon.Entities;
using AdmitadCommon.Entities.Api;

using AdmitadSqlData.Helpers;

namespace TheStore.Api.Core.Sources.Workers
{
    internal sealed class PropertiesWorker : BaseLinkWorker
    {
        public PropertiesWorker(
            ElasticSearchClientSettings settings, BackgroundWorks works )
            : base( settings, works ) { }

        public void LinkProperties( LinkPropertiesContext context )
        {
            var colors = DbHelper.GetColors();
            var materials = DbHelper.GetMaterials();
            var sizes = DbHelper.GetSizes();
            var linker = CreateLinker( context );
            linker.LinkProperties( colors: colors, materials: materials, sizes: sizes );
        }
        
        public void UnlinkProperties( UnlinkPropertiesContext context )
        {
            var colors = DbHelper.GetColors();
            var materials = DbHelper.GetMaterials();
            var sizes = DbHelper.GetSizes();
            var linker = CreateLinker( context );
            linker.UnlinkProperties( colors: colors, materials: materials, sizes: sizes );
        }
    }
}