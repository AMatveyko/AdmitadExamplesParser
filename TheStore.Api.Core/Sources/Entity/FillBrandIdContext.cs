// a.snegovoy@gmail.com

using AdmitadCommon.Entities;

namespace TheStore.Api.Core.Sources.Entity
{
    public class FillBrandIdContext : BackgroundBaseContext
    {
        public FillBrandIdContext(
            string clearlyName )
            : base( $"{nameof(FillBrandIdContext)}:{clearlyName}" )
        {
            ClearlyName = clearlyName;
        }
        
        public string ClearlyName { get; }
    }
}