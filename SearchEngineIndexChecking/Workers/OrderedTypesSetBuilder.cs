using System.Collections.Generic;
using SearchEngineIndexChecking.Entities;

namespace SearchEngineIndexChecking.Workers
{
    internal sealed class OrderedTypesSetBuilder : IBrowserTypesSetBuilder
    {

        private readonly Queue<BrowserType> _types = new Queue<BrowserType>();

        public OrderedTypesSetBuilder(List<BrowserType> types) =>
            types.ForEach( _types.Enqueue );

        public BrowserType GetNextType() {
            var type = _types.Dequeue();
            _types.Enqueue( type );
            return type;
        }
    }
}