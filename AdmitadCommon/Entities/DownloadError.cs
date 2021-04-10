// a.snegovoy@gmail.com

using System.ComponentModel;

namespace AdmitadCommon.Entities
{
    public enum DownloadError
    {
        Ok,
        [ Description("Many requests")]
        ManyRequests,
        [ Description("Closed store")]
        ClosedStore,
        UnknownError
    }
}