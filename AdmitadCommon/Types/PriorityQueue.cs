// a.snegovoy@gmail.com

using System;
using System.Collections.Generic;
using System.Linq;

using AdmitadCommon.Entities;

namespace AdmitadCommon.Types
{
    public sealed class PriorityQueue
    {
        private static Dictionary<QueuePriority, Queue<BackgroundWork>> _queues = new() {
            {QueuePriority.Low, new Queue<BackgroundWork>()},
            {QueuePriority.Medium, new Queue<BackgroundWork>()},
            {QueuePriority.Hight, new Queue<BackgroundWork>()}
        };

        private static Queue<BackgroundWork> _parallelQueue = new ();
        
        public bool Any() => _queues.Any( d => d.Value.Any() );
        public bool HasParallel() => _parallelQueue.Any();

        public void Enqueue( BackgroundWork work, QueuePriority priority ) {
            if( priority == QueuePriority.Parallel ) {
                _parallelQueue.Enqueue( work );
                return;
            }
            _queues[ priority ].Enqueue( work );
        }

        public BackgroundWork ParallelDequeue() => _parallelQueue.Dequeue();

        public BackgroundWork Dequeue()
        {
            foreach( var queue in _queues.OrderBy( q => q.Key ).Select( q => q.Value) ) {
                if( queue.Any() ) {
                    return queue.Dequeue();
                }
            }

            throw new ArgumentException( "Queue empty" );
        } 
    }
}