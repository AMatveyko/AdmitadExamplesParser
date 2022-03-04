using System.Collections.Generic;
using SearchEngineIndexChecking.Entities;
using SearchEngineIndexChecking.Helpers;

namespace SearchEngineIndexChecking.Workers
{
    internal sealed class RandomTypesSetBuilder : IBrowserTypesSetBuilder
    {

        private readonly List<BrowserType> _types;
        private readonly RandomElementGetter<BrowserType> _randomElementGetter =
            new RandomElementGetter<BrowserType>();

        public RandomTypesSetBuilder(List<BrowserType> types) => _types = types;

        public BrowserType GetNextType() => _randomElementGetter.Get(_types);
    }
}