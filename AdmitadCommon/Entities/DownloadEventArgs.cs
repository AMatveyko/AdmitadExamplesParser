// a.snegovoy@gmail.com

using System;

namespace AdmitadCommon.Entities
{
    public sealed class DownloadEventArgs : EventArgs 
    {
        public DownloadEventArgs(
            DownloadInfo info )
        {
            Info = info;
        }
        
        public DownloadInfo Info { get; }
    }
}