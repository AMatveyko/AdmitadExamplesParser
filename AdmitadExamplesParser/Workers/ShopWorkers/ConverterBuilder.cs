// a.snegovoy@gmail.com

using System;

namespace AdmitadExamplesParser.Workers.ShopWorkers
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
                _ => new DefaultShopWorker()
            };
        }
    }
}