// a.snegovoy@gmail.com

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

using AdmitadCommon.Entities;
using AdmitadCommon.Types;

using Microsoft.AspNetCore.Mvc;

namespace TheStore.Api.Core.Sources.Workers
{
    public static class BackgroundWorks
    {
        private static BackgroundBaseContext _work;
        private static readonly ConcurrentDictionary<string, BackgroundBaseContext> Results = new ();
        private static bool _workInProgress;
        private static readonly object Locker = new ();

        public static IActionResult AddToQueue< T >(
            Action<T> action,
            T context,
            QueuePriority priority,
            bool clean )
            where T : BackgroundBaseContext
        {
            if( Results.ContainsKey( context.Id ) == false ) {
                AddWork( action, context, priority );
            }
            
            StartWorkIfNeed();
            
            var result = Results[ context.Id ];
            if( result.WorkStatus == BackgroundStatus.Completed && clean ) {
                Results.TryRemove( result.Id, out var res );
            }
            return new JsonResult( result );
        }

        private static void StartWorkIfNeed()
        {
            lock( Locker ) {
                if( _workInProgress == false ) {
                    _workInProgress = true;
                    
                    var thread = new Thread( DoWork );
                    thread.Start();
                }
            }
        }
        
        private static void AddWork<T>( Action<T> action, T context, QueuePriority priority ) 
            where T : BackgroundBaseContext {
            context.WorkStatus = BackgroundStatus.Awaiting;
            Results[ context.Id ] = context;
            PriorityQueue.Enqueue( new BackgroundWork( () => action( context ), context.Id ), priority );
        }
        
        private static void DoWork()
        {
            while( PriorityQueue.Any() ) {
                var (action, id) = PriorityQueue.Dequeue();
                var context = Results[ id ];
                context.WorkStatus = BackgroundStatus.InWork;
                RunAction( action, context );
                context.PercentFinished = 100;
                context.WorkStatus = BackgroundStatus.Completed;
            }

            _workInProgress = false;
        }
        
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
            
            var thread = new Thread( () => RunAction( () => action( context ), context ) );
            thread.Start();
            
            return GetResult( "В работе" );
        }

        private static void RunAction(
            Action action,
            BackgroundBaseContext context )
        {
            var sw = new Stopwatch();
            sw.Start();
            try {
                action();
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
            var context = _work ?? new BackgroundBaseContext("1");
            if( text != null ) {
                context.Content = text;
            }
            return new JsonResult( context );
        }
    }
}