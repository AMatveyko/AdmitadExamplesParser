// a.snegovoy@gmail.com

using System;

namespace Common.Entities
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