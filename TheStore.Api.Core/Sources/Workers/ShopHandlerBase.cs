// a.snegovoy@gmail.com

using System.Collections.Generic;

using Admitad.Converters;
using Admitad.Converters.Workers;

using AdmitadCommon.Entities.Statistics;

using AdmitadSqlData.Helpers;

using Common.Api;
using Common.Entities;
using Common.Settings;
using Common.Workers;

using NLog;

namespace TheStore.Api.Core.Sources.Workers
{
    internal abstract class ShopHandlerBase
    {
        private static readonly Logger Logger = LogManager.GetLogger( "ErrorLogger" );

        protected readonly ProcessShopContext Context;
        protected readonly ElasticSearchClientSettings Settings;
        private readonly ShopProcessingStatistics _statistics;
        private readonly DbHelper _dbHelper;

        protected ShopHandlerBase(
            ProcessShopContext context,
            ElasticSearchClientSettings settings,
            DbHelper dbHelper )
        {
            ( Context, Settings, _statistics, _dbHelper ) = ( context, settings,
                new ShopProcessingStatistics( context.DownloadInfo, AddMessage, Logger ),
                dbHelper );
        }

        public void Process()
        {
            var shopData = GetShopData();
            DoProcess( shopData );
            Finish();
        }
        
        protected abstract void DoProcess( ShopData shopData );
        protected abstract IClientForShopStatistics GetClient();
        
        private ShopData GetShopData()
        {
           var data = _statistics.GetShopData( ParseShop );
           var message = $"new offers: {data.NewOffers.Count};";
           if( data.DeletedOffers.Count > 0 ) {
               message += $" deleted offers: {data.DeletedOffers.Count}";
           }
           AddMessage( message );
           return data;
        }

        protected IEnumerable<Offer> CleanOffers( IShopDataWithNewOffers shopData )
        {
            var cleaner = new OfferConverter( shopData, _dbHelper, Context ); 
            var cleanOffers = cleaner.GetCleanOffers();
            SetProgress( 40 );
            AddMessage( "Clearing offers complete" );
            return cleanOffers;
        }

        protected List<Product> ConvertOffers( IEnumerable<Offer> offers )
        {
            var calculation = new RatingCalculation( Context.DownloadInfo.ShopWeight );
            var products =
                new ProductConverter( _dbHelper, calculation ).GetProducts( offers ); 
            SetProgress( 80 );
            AddMessage( "Convert to products complete" );
            return products;
        }
        
        protected void SetProgress(
            int percents )
        {
            Context.SetProgress( percents, 100 );
        }

        protected void AddMessage(
            string text,
            bool isError = false )
        {
            Context.AddMessage( text, isError );
        }
        
        private void Finish() {
            SetProductsStatistics();
            _dbHelper.WriteShopStatistics( _statistics );
        }
        
        private void SetProductsStatistics()
        {
            var client = GetClient();
            var count = client.CountProductsForShop( Context.ShopId.ToString() );
            var soldOut = client.CountDisabledProductsByShop( Context.ShopId.ToString() );
            _statistics.SetProductsStatistics( (int)count, (int)soldOut );
        }

        private ShopData ParseShop()
        {
            var parser = new GeneralParser(
                Context.DownloadInfo,
                Context );
            var shopData = parser.Parse();
            SetProgress( 20 );
            AddMessage( "Parsing complete" );
            return shopData;
        }
    }
}