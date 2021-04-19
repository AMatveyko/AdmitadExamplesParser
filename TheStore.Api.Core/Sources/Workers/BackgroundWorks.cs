// a.snegovoy@gmail.com

using System;
using System.Diagnostics;
using System.Threading;

using AdmitadCommon.Entities;

using Microsoft.AspNetCore.Mvc;

namespace TheStore.Api.Core.Sources.Workers
{
    public static class BackgroundWorks
    {
        private static BackgroundBaseContext _work;
        
        public static IActionResult Run<T>( Action<T> action, T context, bool clean ) where T: BackgroundBaseContext
        {
            
            if( _work != null ) {
                
                if( _work.IsFinished ) {
                    var result = GetResult();
                    if( clean ) {
                        _work = null;
                    }
                    return result;
                }

                return NotFinishedResult();
            }

            _work = context;
            
            var thread = new Thread( () => RunAction( action, context ) );
            thread.Start();
            
            return GetResult( "В работе" );
        }

        private static void RunAction<T>(
            Action<T> action,
            T context ) where T: BackgroundBaseContext
        {
            var sw = new Stopwatch();
            sw.Start();
            try {
                action( context );
            }
            catch( Exception e ) {
                context.Content = e.Message;
                context.IsError = true;
            }
            sw.Stop();
            context.Time = sw.ElapsedMilliseconds;
            context.IsFinished = true;
        }
        
        private static IActionResult NotFinishedResult()
        {
            return GetResult( $"Фоновая задача ещё не завершилась, выполненно {_work.PercentFinished}%" );
        }
        
        private static IActionResult GetResult( string text = null )
        {
            var context = _work ?? new BackgroundBaseContext();
            if( text != null ) {
                context.Content = text;
            }
            return new JsonResult( context );
        }
    }
}