// a.snegovoy@gmail.com

using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Entities
{
    public sealed class DownloadsInfo : IMinimalDownloadsInfo
    {
        public DownloadsInfo( ShopInfo shopInfo )
        {
            ShopId = shopInfo.ShopId;
            NameLatin = shopInfo.NameLatin;
            ShopWeight = shopInfo.Weight;
            VersionProcessing = shopInfo.VersionProcessing;
            LastUpdate = shopInfo.LastUpdate;
            FeedsInfos = shopInfo.Feeds;
        } 
        
        public List<FeedInfo> FeedsInfos { get; }
        public DateTime StartTime { get; set; }
        public string NameLatin { get; }
        public List<IFileInfo> Files => FeedsInfos.Cast<IFileInfo>().ToList();
        public int ShopId { get; }
        public string ShopName { get; set; }
        public long DownloadTime { get; set; }
        public bool HasErrors => FeedsInfos.Any( f => f.Error != DownloadError.Ok );
        public int ShopWeight { get; }
        public int VersionProcessing { get; }
        public DateTime LastUpdate { get; }
    }
}