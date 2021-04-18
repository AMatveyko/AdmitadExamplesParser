// a.snegovoy@gmail.com

using System;

namespace AdmitadCommon.Helpers
{
    public static class TimeHelper
    {
        public static string MillisecondsPretty( long ms ) {
            var t = TimeSpan.FromMilliseconds( ms );

            var answer = string.Format("{0:D2}h:{1:D2}m:{2:D2}s:{3:D3}ms", 
                t.Hours,
                t.Minutes, 
                t.Seconds, 
                t.Milliseconds);
            return answer;
        }
    }
}