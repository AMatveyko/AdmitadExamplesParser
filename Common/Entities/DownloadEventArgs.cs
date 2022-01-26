// a.snegovoy@gmail.com

using System;

namespace Common.Entities
{
    public sealed class DownloadEventArgs : EventArgs 
    {
        public DownloadEventArgs(
            DownloadsInfo info )
        {
            Info = info;
        }
        
        public DownloadsInfo Info { get; }
    }
}