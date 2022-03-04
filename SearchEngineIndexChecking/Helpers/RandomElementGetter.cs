using System;
using System.Collections.Generic;

namespace SearchEngineIndexChecking.Helpers
{
    internal class RandomElementGetter<T>
    {

        private readonly Random _random = new Random();

        public T Get(List<T> set) {
            var index = _random.Next(set.Count);
            return set[index];
        }
            
    }
}