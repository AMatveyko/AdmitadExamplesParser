// a.snegovoy@gmail.com

using System;
using System.Collections.Generic;
using System.Linq;

using AdmitadCommon.Entities;

namespace AdmitadCommon.Types
{
    public static class PriorityQueue
    {
        private static Dictionary<QueuePriority, Queue<BackgroundWork>> _queues = new() {
            {QueuePriority.Low, new Queue<BackgroundWork>()},
            {QueuePriority.Medium, new Queue<BackgroundWork>()},
            {QueuePriority.Hight, new Queue<BackgroundWork>()}
        };

        public static bool Any() => _queues.Any( d => d.Value.Any() );

        public static void Enqueue( BackgroundWork work, QueuePriority priority ) =>
            _queues[ priority ].Enqueue( work );

        public static BackgroundWork Dequeue()
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