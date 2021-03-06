// a.snegovoy@gmail.com

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using AdmitadCommon.Entities.Queue;
using AdmitadCommon.Types;

using Common.Api;
using Common.Entities;

using Microsoft.AspNetCore.Mvc;

using NLog;

namespace TheStore.Api.Core.Sources.Workers
{
    public sealed class BackgroundWorks
    {

        private static readonly Logger Logger = LogManager.GetLogger( "ErrorLogger" );
        
        private static readonly ConcurrentDictionary<string, BackgroundBaseContext> Results = new ();
        private static bool _workInProgress;
        private static bool _parallelWorkInProgress;
        private static readonly object Locker = new ();

        private readonly PriorityQueue _queue;

        public BackgroundWorks( PriorityQueue queue ) => _queue = queue;

        public IActionResult ShowAllWorks()
        {
            var result = new QueueStatus();
            var singleContexts = Results.Values.Where( c => c is ParallelBackgroundContext == false ).ToList();
            var parallelContexts = Results.Values.Where( c => c is ParallelBackgroundContext ).ToList();
            if( singleContexts.Any() ) {
                result.Single = GetQueueState( singleContexts );
            }

            if( parallelContexts.Any() ) {
                result.Parallel = GetQueueState( parallelContexts );
            }
            return new JsonResult( result );
        }

        private static QueueState GetQueueState( List<BackgroundBaseContext> contexts )
        {
            return new QueueState {
                InWork = contexts.FirstOrDefault( c => c.WorkStatus == BackgroundStatus.InWork )?.Id ?? "empty",
                Awaiting =
                    contexts.Where( c => c.WorkStatus == BackgroundStatus.Awaiting ).Select( c => c.Id ).ToList(),
                Completed = contexts.Where( c => c.WorkStatus == BackgroundStatus.Completed ).Select( c => c.Id )
                    .ToList()
            };
        }
        
        public IActionResult AddToQueue< T >(
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

        private void AddWork<T>( Action<T> action, T context, QueuePriority priority ) 
            where T : BackgroundBaseContext {
            context.Prepare();
            Results[ context.Id ] = context;
            _queue.Enqueue( new BackgroundWork( () => action( context ), context.Id ), priority );
        }
        
        private void StartWorkIfNeed()
        {
            lock( Locker ) {
                if( _workInProgress == false ) {
                    _workInProgress = true;
                    
                    var thread = new Thread( () => DoWork(
                        _queue.Dequeue,
                        _queue.Any,
                        () => _workInProgress = false ) );
                    thread.Start();
                }

                if( _parallelWorkInProgress == false ) {
                    _parallelWorkInProgress = true;
                    
                    var thread = new Thread( () => DoWork(
                        _queue.ParallelDequeue,
                        _queue.HasParallel,
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
        
    }
}