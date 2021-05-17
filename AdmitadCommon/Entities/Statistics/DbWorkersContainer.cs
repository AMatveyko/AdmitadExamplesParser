// a.snegovoy@gmail.com

using System;
using System.Collections.Generic;

namespace AdmitadCommon.Entities.Statistics
{
    public sealed class DbWorkersContainer
    {

        public DbWorkersContainer(
            Action<string, List<ShopCategory>> updateShopCategory,
            Action<ShopProductStatistics> updateShopStatistics,
            Action<IShopStatisticsForDb> writeShopProcessLog,
            Action<int,DateTime> updateShopUpdateDate ) =>
            ( UpdateShopCategory, UpdateShopStatistics, WriteShopProcessLog, UpdateShopUpdateDate )
            = ( updateShopCategory, updateShopStatistics, writeShopProcessLog, updateShopUpdateDate );
        
        internal Action<string, List<ShopCategory>> UpdateShopCategory { get; }
        internal Action<ShopProductStatistics> UpdateShopStatistics { get; }
        internal Action<IShopStatisticsForDb> WriteShopProcessLog { get; }
        internal Action<int,DateTime> UpdateShopUpdateDate { get; }
    }
}