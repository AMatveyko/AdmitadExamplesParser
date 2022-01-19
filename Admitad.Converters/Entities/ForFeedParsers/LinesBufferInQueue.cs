// a.snegovoy@gmail.com

using System.Collections.Concurrent;

namespace Admitad.Converters.Entities
{
    internal sealed class LinesBufferInQueue : ILinesBuffer
    {

        private readonly ConcurrentQueue<string> _buffer = new ();

        public void Add( string line ) {
            _buffer.Enqueue( line );
        }

        public string Dequeue() {
            string line;
            _buffer.TryDequeue( out var newLine );
            line = string.IsNullOrEmpty( newLine ) ? "queue empty" : newLine;
            
            return line;
                
        }
        public bool IsNotEmpty() => _buffer.Count > 0;
    }
}