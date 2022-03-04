// a.snegovoy@gmail.com

using AdmitadSqlData.Helpers;
using ShopsNames = Common.Constants.ShopsNames;

namespace Admitad.Converters.Workers.ShopWorkers
{
    internal static class ConverterBuilder
    {

        public static IShopWorker GetConverterByShop( string shopName, DbHelper dbHelper )
        {

            return shopName switch {
                ShopsNames.Yoox => new YooxWorker( dbHelper ),
                ShopsNames.Lamoda => new LamodaWorker( dbHelper ),
                ShopsNames.Adidas => new AdidasWorker( dbHelper ),
                ShopsNames.Asos => new AsosWorker( dbHelper ),
                ShopsNames.TwelveStoreez => new TwelveStoreezWorker( dbHelper ),
                ShopsNames.Anabel => new AnabelWorker( dbHelper ),
                ShopsNames.VMeha => new VmehaWorker( dbHelper ),
                ShopsNames.BrandShop => new BrandshopWorker( dbHelper ),
                ShopsNames.Gretta => new GrettaWorker( dbHelper ),
                ShopsNames.Goods => new GoodsWorker( dbHelper ),
                ShopsNames.GloriaJeans => new GloriaJeansWorker( dbHelper ),
                ShopsNames.Tamaris => new TamarisWorker( dbHelper ),
                ShopsNames.Incanto => new IncantoShopWorker( dbHelper ),
                ShopsNames.GulliverMarket => new GulliverMarketWorker( dbHelper ),
                ShopsNames.NewChic => new NewchicWorker( dbHelper ),
                ShopsNames.BelleYou => new BelleyouWorker( dbHelper ),
                ShopsNames.IntimShop => new IntimShopWorker( dbHelper ),
                ShopsNames.SVMoscow => new SvMoscowWorker( dbHelper ),
                ShopsNames.SmartCasual => new SmartcasualWorker( dbHelper ),
                ShopsNames.GoldenLine => new GoldenLineWorker( dbHelper ),
                ShopsNames.Yoins => new YoinsWorker( dbHelper ),
                ShopsNames.VipAvenue => new VipAvenueWorker( dbHelper ),
                ShopsNames.GerryWeber => new GerryWeberWorker( dbHelper ),
                ShopsNames.Gate31 => new Gate31Worker( dbHelper ),
                ShopsNames.FreeAge => new FreeageWorker( dbHelper ),
                ShopsNames.VassaCo => new VassaCoWorker( dbHelper ),
                ShopsNames.Camper => new CamperWorker( dbHelper ),
                ShopsNames.Inavokich => new InavokichWorker( dbHelper ),
                ShopsNames.DochkiSinochki => new DochkiSinochkiWorker( dbHelper ),
                ShopsNames.Lassie => new LassieWorker( dbHelper ),
                ShopsNames.Shein => new SheinWorker( dbHelper ),
                ShopsNames.FarFetch => new FarfetchWorker( dbHelper ),
                ShopsNames.BebaKids => new BebaKidsWorker( dbHelper ),
                ShopsNames.Akusherstvo => new AkusherstvoWorker( dbHelper ),
                _ => new DefaultShopWorker( dbHelper )
            };
        }
    }
}