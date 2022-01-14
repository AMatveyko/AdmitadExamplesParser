// a.snegovoy@gmail.com

using System;

namespace Common.Entities
{
    public sealed class DownloadInfo : IMinimalDownloadInfo
    {
        //public DownloadInfo( int shopId, string nameLatin, int shopWeight, byte versionProcessing, DateTime lastUpdate )
        public DownloadInfo( XmlFileInfo info )
        {
            ShopId = info.ShopId;
            NameLatin = info.NameLatin;
            ShopWeight = info.Weight;
            VersionProcessing = info.VersionProcessing;
            LastUpdate = info.LastUpdate;
        } 
        
        public DateTime StartTime { get; set; }
        public string NameLatin { get; }
        public int ShopId { get; }
        public string Url { get; set; }
        public string ShopName { get; set; }
        public string FilePath { get; set; }
        public long DownloadTime { get; set; }
        public DownloadError Error { get; set; }
        public long FileSize { get; set; }
        public bool HasError => Error != DownloadError.Ok;
        public int ShopWeight { get; }
        public int VersionProcessing { get; }
        public DateTime LastUpdate { get; }
    }
}