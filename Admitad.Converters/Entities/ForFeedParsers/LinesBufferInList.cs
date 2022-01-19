// a.snegovoy@gmail.com

using System.Collections.Generic;

namespace Admitad.Converters.Entities
{
    internal sealed class LinesBufferInList : ILinesBuffer
    {

        private readonly List<string> _buffer = new ();

        public void Add( string line ) => _buffer.Add( line );
        public List<string> GetList() => _buffer;
    }
}