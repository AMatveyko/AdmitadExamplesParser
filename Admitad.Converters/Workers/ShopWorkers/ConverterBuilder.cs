// a.snegovoy@gmail.com

namespace Admitad.Converters.Workers.ShopWorkers
{
    internal static class ConverterBuilder
    {
        //private const string Lamoda = "lamoda.ru";
        
        public static IShopWorker GetConverterByShop(
            string shopName )
        {
            return shopName switch {
                "yoox" => new YooxWorker(),
                "lamoda" => new LamodaWorker(),
                "adidas" => new AdidasWorker(),
                "asos" => new AsosWorker(),
                "12storeez" => new TwelveStoreezWorker(),
                "anabel" => new AnabelWorker(),
                "vmeha" => new VmehaWorker(),
                "brandshop" => new BrandshopWorker(),
                "gretta" => new GrettaWorker(),
                "goods" => new GoodsWorker(),
                "gloriajeans" => new GloriaJeansWorker(),
                "tamaris" => new TamarisWorker(),
                "Incanto" => new IncantoShopWorker(),
                "gullivermarket" => new GulliverMarketWorker(),
                _ => new DefaultShopWorker()
            };
        }
    }
}