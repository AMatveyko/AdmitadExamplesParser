// a.snegovoy@gmail.com

using System;
using System.Diagnostics;

namespace Common.Helpers
{
    public static class DebugHelper {

        public static T MeasureExecuteTime<T>( Func<T> action, out long executeTime ) {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var result = action();
            stopWatch.Stop();
            executeTime = stopWatch.ElapsedMilliseconds;
            return result;
        }
        
        public static void MeasureExecuteTime( Action action, out long executeTime ) {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            action();
            stopWatch.Stop();
            executeTime = stopWatch.ElapsedMilliseconds;
        }
        
    }
}