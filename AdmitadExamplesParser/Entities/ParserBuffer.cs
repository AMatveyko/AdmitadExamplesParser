// a.snegovoy@gmail.com

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace AdmitadExamplesParser.Entities
{
    internal static class ParserBuffer
    {
        private static readonly ConcurrentDictionary<int, StringBuilder> _buffers = new();

        public static StringBuilder GetBuffer( int key )
        {
            if( _buffers.ContainsKey( key ) == false ) {
                _buffers[ key ] = new StringBuilder();
            }

            return _buffers[ key ];
        }

        public static void DestroyBuffer( int key ) {
            if( _buffers.ContainsKey( key ) ) {
                _buffers.Remove( key , out var str );
            }
        }
    }
}