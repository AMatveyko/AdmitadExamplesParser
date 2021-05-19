// a.snegovoy@gmail.com

using System;
using System.Linq;

using NLog;

namespace AdmitadCommon.Entities.Statistics
{
    public sealed class ShopProcessingStatistics : IShopStatisticsForDb
    {

        private readonly Action<string, bool> _addMessage;
        private readonly Logger _logger;
        private readonly DownloadInfo _info;

        public ShopProcessingStatistics( DownloadInfo info, Action<string, bool> messageAdder, Logger logger )
        {
            ( _addMessage, _info, _logger ) = ( messageAdder, info, logger );
            _productStatistics = new ShopProductStatistics {
                ShopId = info.ShopId,
                ShopName = info.ShopName
            };
        }

        public DateTime StartDownloadFeed => _info.StartTime;
        public int ShopId => _info.ShopId;
        public long FileSize => _info.FileSize;
        public int OfferCount => _shopData?.Offers?.Count ?? 0;
        public int SoldOutOfferCount => _productStatistics?.SoldoutAfter ?? 0;
        public int CategoryCount => _shopData?.Categories?.Count ?? 0;
        public long DownloadTime => _info.DownloadTime;
        private readonly ShopProductStatistics _productStatistics;
        private ShopData _shopData;

        public ShopData GetShopData( Func<ShopData> action )
        {
            _shopData = action();
            return _shopData;
        }

        public void SetProductStatisticsBefore( int count, int soldOut ) {
            _productStatistics.TotalBefore = count;
            _productStatistics.SoldoutBefore = soldOut;
        }

        public void SetProductsStatisticsAfter( int count, int soldOut )
        {
            _productStatistics.TotalAfter = count;
            _productStatistics.SoldoutAfter = soldOut;
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
            container.UpdateShopUpdateDate( _info.ShopId, DateTime.Now );
        }
        
        private void WriteShopProcessLog( DbWorkersContainer container )
        {
            container.WriteShopProcessLog( this );
        }
        
        private void UpdateShopStatistics( DbWorkersContainer container )
        {
            container.UpdateShopStatistics( _productStatistics );
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
        
        // public void FillCategories( List<Product> products )
        // {
        //     if( products != null &&
        //         products.Any() &&
        //         _shopData.Categories != null &&
        //         _shopData.Categories.Any() ) {
        //         var groupedProducts = products.Where( p => p.OriginalCategoryId.IsNotNullOrWhiteSpace() )
        //             .GroupBy( p => p.OriginalCategoryId ).ToArray();
        //         
        //         if( groupedProducts.Any() == false ) {
        //             return;
        //         }
        //
        //         foreach( var group in groupedProducts ) {
        //             FillCategory( group );
        //         }
        //         
        //     }
        // }

        // private void FillCategory( IGrouping<string, Product> group )
        // {
        //     if( _shopData.Categories.ContainsKey( group.Key ) == false ) {
        //         return;
        //     }
        //
        //     var category = _shopData.Categories[ group.Key ];
        //     category.TotalProductsNumber = group.Count();
        //     category.MenProductsNumber = group.Count( p => p.Gender == "m" );
        //     category.WomenProductsNumber = group.Count( p => p.Gender == "w" );
        // }

    }
}