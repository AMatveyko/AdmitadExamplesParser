// a.snegovoy@gmail.com

namespace Admitad.Converters.Workers
{
    public sealed class ParsingInfo
    {

        public ParsingInfo(
            string filePath,
            int shopWeight,
            string shopName ) =>
            ( FilePath, ShopWeight, ShopName ) = ( filePath, shopWeight, shopName );
        
        public string FilePath { get; }
        public int ShopWeight { get; }
        public string ShopName { get; }
    }
}