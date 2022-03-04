// a.snegovoy@gmail.com

namespace Common.Api
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