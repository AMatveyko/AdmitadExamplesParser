// a.snegovoy@gmail.com

using System;
using System.Collections.Generic;

namespace Common.Entities
{
    public sealed class ShopInfo
    {
        public ShopInfo(
            string name,
            string nameLatin,
            List<FeedInfo> feeds,
            int shopId,
            int weight,
            byte versionProcessing,
            DateTime? lastUpdate )
        {
            Name = name;
            NameLatin = nameLatin;
            Feeds = feeds;
            ShopId = shopId;
            Weight = weight;
            VersionProcessing = versionProcessing;
            LastUpdate = lastUpdate.GetValueOrDefault();
        }
        
        public string Name { get; }
        public string NameLatin { get; }
        public List<FeedInfo> Feeds { get; }
        public int ShopId { get; }
        public int Weight { get; }
        public byte VersionProcessing { get; set; }
        public DateTime LastUpdate { get; }
    }
}