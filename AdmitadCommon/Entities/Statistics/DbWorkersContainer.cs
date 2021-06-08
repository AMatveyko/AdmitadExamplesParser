// a.snegovoy@gmail.com

using System;
using System.Collections.Generic;

namespace AdmitadCommon.Entities.Statistics
{
    public sealed class DbWorkersContainer
    {

        public DbWorkersContainer(
            Action<string, List<ShopCategory>> updateShopCategory,
            Action<ShopProduct> updateShopStatistics,
            Action<IShopStatisticsForDb> writeShopProcessLog,
            Action<int,DateTime> updateShopUpdateDate ) =>
            ( UpdateShopCategory, UpdateShopStatistics, WriteShopProcessLog, UpdateShopUpdateDate )
            = ( updateShopCategory, updateShopStatistics, writeShopProcessLog, updateShopUpdateDate );
        
        internal Action<string, List<ShopCategory>> UpdateShopCategory { get; }
        internal Action<ShopProduct> UpdateShopStatistics { get; }
        internal Action<IShopStatisticsForDb> WriteShopProcessLog { get; }
        internal Action<int,DateTime> UpdateShopUpdateDate { get; }
    }
}