// a.snegovoy@gmail.com

using System;

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
    }
}