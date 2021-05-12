﻿// a.snegovoy@gmail.com

namespace AdmitadCommon.Entities
{
    public sealed class DownloadInfo
    {

        private const string ErrorStatusOk = "Ok";
        
        public DownloadInfo( int shopId, string nameLatin )
        {
            ShopId = shopId;
            NameLatin = nameLatin;
        } 
        
        public string NameLatin { get; }
        public int ShopId { get; }
        public string Url { get; set; }
        public string ShopName { get; set; }
        public string FilePath { get; set; }
        public long DownloadTime { get; set; }
        public DownloadError Error { get; set; }
        public bool HasError => Error != DownloadError.Ok;
    }
}