// a.snegovoy@gmail.com
using System;
namespace AdmitadExamplesParser.Entities
{
    internal sealed class DownloadInfo
    {
        private const string ErrorStatusOk = "Ok";
        public string Url { get; set; }
        public string ShopName { get; set; }
        public string FilePath { get; set; }
        public long DownloadTime { get; set; }
        public string Error { get; set; } = ErrorStatusOk;
        public bool HasError => Error != ErrorStatusOk;
    }
}