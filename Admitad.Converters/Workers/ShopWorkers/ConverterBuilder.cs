// a.snegovoy@gmail.com

using AdmitadSqlData.Helpers;

namespace Admitad.Converters.Workers.ShopWorkers
{
    internal static class ConverterBuilder
    {

        public static IShopWorker GetConverterByShop( string shopName, DbHelper dbHelper )
        {

            return shopName switch {
                "yoox" => new YooxWorker( dbHelper ),
                "lamoda" => new LamodaWorker( dbHelper ),
                "adidas" => new AdidasWorker( dbHelper ),
                "asos" => new AsosWorker( dbHelper ),
                "12storeez" => new TwelveStoreezWorker( dbHelper ),
                "anabel" => new AnabelWorker( dbHelper ),
                "vmeha" => new VmehaWorker( dbHelper ),
                "brandshop" => new BrandshopWorker( dbHelper ),
                "gretta" => new GrettaWorker( dbHelper ),
                "goods" => new GoodsWorker( dbHelper ),
                "gloriajeans" => new GloriaJeansWorker( dbHelper ),
                "tamaris" => new TamarisWorker( dbHelper ),
                "Incanto" => new IncantoShopWorker( dbHelper ),
                "gullivermarket" => new GulliverMarketWorker( dbHelper ),
                "newchic" => new NewchicWorker( dbHelper ),
                "belleyou" => new BelleyouWorker( dbHelper ),
                "intimshop" => new IntimShopWorker( dbHelper ),
                "svmoscow" => new SvMoscowWorker( dbHelper ),
                "smartcasual" => new SmartcasualWorker( dbHelper ),
                "goldenline" => new GoldenLineWorker( dbHelper ),
                "yoins" => new YoinsWorker( dbHelper ),
                "vipavenue" => new VipAvenueWorker( dbHelper ),
                "gerryweber" => new GerryWeberWorker( dbHelper ),
                "gate31" => new Gate31Worker( dbHelper ),
                "freeage" => new FreeageWorker( dbHelper ),
                "vassaco" => new VassaCoWorker( dbHelper ),
                "camper" => new CamperWorker( dbHelper ),
                "inavokich" => new InavokichWorker( dbHelper ),
                _ => new DefaultShopWorker( dbHelper )
            };
        }
    }
}