// a.snegovoy@gmail.com

using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Threading;

using AdmitadCommon.Entities;
using AdmitadCommon.Entities.Api;
using AdmitadCommon.Types;

using Microsoft.AspNetCore.Mvc;

using NLog;

namespace TheStore.Api.Core.Sources.Workers
{
    public static class BackgroundWorks
    {

        private static readonly Logger Logger = LogManager.GetLogger( "ErrorLogger" );
        
        private static BackgroundBaseContext _work;
        private static readonly ConcurrentDictionary<string, BackgroundBaseContext> Results = new ();
        private static bool _workInProgress;
        private static bool _parallelWorkInProgress;
        private static readonly object Locker = new ();

        public static IActionResult ShowAllWorks()
        {
            var works = ( Work: _work, Works: Results.Select( i => i.Value ).OrderBy( i => i.Id ) );
            return new JsonResult( works );
        }
        
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
                if( result is ParallelBackgroundContext parallelContext ) {
                    foreach( var childContext in parallelContext.Contexts ) {
                        Results.TryRemove( childContext.Id, out var childRes );
                    }
                }
                Results.TryRemove( result.Id, out var res );
            }
            return new JsonResult( result );
        }

        private static void AddWork<T>( Action<T> action, T context, QueuePriority priority ) 
            where T : BackgroundBaseContext {
            context.Prepare();
            Results[ context.Id ] = context;
            PriorityQueue.Enqueue( new BackgroundWork( () => action( context ), context.Id ), priority );
        }
        
        private static void StartWorkIfNeed()
        {
            lock( Locker ) {
                if( _workInProgress == false ) {
                    _workInProgress = true;
                    
                    var thread = new Thread( () => DoWork(
                        PriorityQueue.Dequeue,
                        PriorityQueue.Any,
                        () => _workInProgress = false ) );
                    thread.Start();
                }

                if( _parallelWorkInProgress == false ) {
                    _parallelWorkInProgress = true;
                    
                    var thread = new Thread( () => DoWork(
                        PriorityQueue.ParallelDequeue,
                        PriorityQueue.HasParallel,
                        () => _parallelWorkInProgress = false ) );
                    thread.Start();
                }
            }
        }

        private static void DoWork( Func<BackgroundWork> workGetter, Func<bool> workChecker, Action finisher )
        {
            while( workChecker() ) {
                var (action, id) = workGetter();
                var context = Results[ id ];
                RunAction( action, context );
            }

            finisher();
        }

        private static void RunAction( Action action, BackgroundBaseContext context )
        {
            try {
                context.Start();
                action();
            }
            catch( Exception e ) {
                context.Content = e.Message;
                context.AddMessage( e.Message, true );
                context.IsError = true;
                Logger.Error( e );
            }
            finally {
                context.Finish();
            }
        }
        
        private static IActionResult NotFinishedResult()
        {
            return GetResult( $"Фоновая задача ещё не завершилась, выполненно {_work.PercentFinished}%" );
        }
        
        private static IActionResult GetResult( string text = null )
        {
            var context = _work ?? new BackgroundBaseContext("1", "empty" );
            if( text != null ) {
                context.Content = text;
            }
            return new JsonResult( context );
        }
    }
}