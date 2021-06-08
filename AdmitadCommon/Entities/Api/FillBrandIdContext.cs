// a.snegovoy@gmail.com

namespace AdmitadCommon.Entities.Api
{
    public sealed class FillBrandIdContext : BackgroundBaseContext
    {
        public FillBrandIdContext(
            string clearlyName )
            : base( GetCollectedId( nameof(FillBrandIdContext), clearlyName ), "FillBrandId" )
        {
            ClearlyName = clearlyName;
        }
        
        public string ClearlyName { get; }
    }
}