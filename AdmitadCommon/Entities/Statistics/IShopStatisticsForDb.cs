// a.snegovoy@gmail.com

using System;

namespace AdmitadCommon.Entities.Statistics
{
    public interface IShopStatisticsForDb
    {
        DateTime StartDownloadFeed { get; }
        int ShopId { get; }
        long FileSize { get; }
        int OfferCount { get; }
        int SoldOutOfferCount { get; }
        int CategoryCount { get; }
        long DownloadTime { get; }
    }
}