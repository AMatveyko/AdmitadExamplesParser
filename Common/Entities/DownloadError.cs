// a.snegovoy@gmail.com

using System.ComponentModel;

namespace Common.Entities
{
    public enum DownloadError
    {
        Ok,
        [ Description("Many requests")]
        ManyRequests,
        [ Description("Closed store")]
        ClosedStore,
        Unfinished,
        UnknownError
    }
}