// a.snegovoy@gmail.com
using AdmitadCommon.Entities;

namespace AdmitadExamplesParser.Entities
{
    internal sealed class DownloadInfo
    {
        private const string ErrorStatusOk = "Ok";
        public string Url { get; set; }
        public string ShopName { get; set; }
        public string FilePath { get; set; }
        public long DownloadTime { get; set; }
        public DownloadError Error { get; set; }
        public bool HasError => Error != DownloadError.Ok;
    }
}