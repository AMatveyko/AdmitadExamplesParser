// a.snegovoy@gmail.com

using System;
using System.Linq;

using Common.Entities;

using NLog;

namespace AdmitadCommon.Entities.Statistics
{
    public sealed class ShopProcessingStatistics : IShopStatisticsForDb
    {

        private readonly Action<string, bool> _addMessage;
        private readonly Logger _logger;
        private readonly DownloadsInfo _info;

        public ShopProcessingStatistics(
            DownloadsInfo info,
            Action<string, bool> messageAdder,
            Logger logger )
        {
            ( _addMessage, _info, _logger ) = ( messageAdder, info, logger );
            _product = new ShopProduct {
                ShopId = info.ShopId,
                ShopName = info.ShopName
            };
        }

        public DateTime StartDownloadFeed => _info.StartTime;
        public int ShopId => _info.ShopId;
        public long FileSize => _info.FeedsInfos.Sum( f => f.FileSize );
        public int OfferCount => _shopData?.NewOffers?.Count ?? 0;
        public int SoldOutOfferCount => _product?.SoldoutCount ?? 0;
        public int CategoryCount => _shopData?.Categories?.Count ?? 0;
        public long DownloadTime => _info.DownloadTime;
        private readonly ShopProduct _product;
        private ShopData _shopData;

        public ShopData GetShopData( Func<ShopData> action )
        {
            _shopData = action();
            return _shopData;
        }

        public void SetProductsStatistics( int count, int soldOut )
        {
            _product.TotalCount = count;
            _product.SoldoutCount = soldOut;
        }
        
        public void Write( DbWorkersContainer container )
        {
            SafeExecute( WriteCategories, container, "update category error" );
            SafeExecute( UpdateShopStatistics, container, "update shop statistics error");
            SafeExecute( WriteShopProcessLog, container, "update shop process log error" );
            SafeExecute( UpdateShopUpdateDate, container, "update shop update date error" );
        }

        private void UpdateShopUpdateDate( DbWorkersContainer container )
        {
            container.UpdateShopUpdateDate( _info.ShopId, _info.StartTime );
        }
        
        private void WriteShopProcessLog( DbWorkersContainer container )
        {
            container.WriteShopProcessLog( this );
        }
        
        private void UpdateShopStatistics( DbWorkersContainer container )
        {
            container.UpdateShopStatistics( _product );
        }

        private void WriteCategories( DbWorkersContainer container )
        {

                var shopCategories = _shopData.Categories.Values.ToList();
                if( shopCategories.Any() ) {
                    container.UpdateShopCategory( _shopData.Name, shopCategories );
                    _addMessage( $"Updated {shopCategories.Count} shop categories", false );
                }
                else {
                    _addMessage( "The shop has no categories", false );
                }

        }

        private void SafeExecute( Action<DbWorkersContainer> action, DbWorkersContainer container, string message )
        {
            try {
                action( container );
            }
            catch( Exception e ) {
                _addMessage( $"{DateTime.Now} { message }", false );
                _logger.Error( e );
            }
        }

    }
}