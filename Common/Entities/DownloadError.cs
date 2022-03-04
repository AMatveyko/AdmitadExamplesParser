// a.snegovoy@gmail.com

using System.ComponentModel;

namespace Common.Entities
{
    public enum DownloadError
    {
        NoAction,
        Ok,
        [ Description("Many requests")]
        ManyRequests,
        [ Description("Closed store")]
        ClosedStore,
        Unfinished,
        UnknownError
    }
}