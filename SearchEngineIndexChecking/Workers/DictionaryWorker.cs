using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SearchEngineIndexChecking.Helpers;

namespace SearchEngineIndexChecking.Workers
{
    internal sealed class DictionaryWorker : IWordsSet
    {

        private const string FilePath = "wordSet.txt";

        private static readonly List<string> WordSet;
        private static readonly Range Range = new Range(1, 3);
        private readonly RandomElementGetter<string> _rand = new RandomElementGetter<string>();

        static DictionaryWorker() => WordSet = CreateSet();

        public string GetPhrase(int length = 1) {
            
            ValidateLength(length);

            return string.Join(" ", Enumerable.Range(0, length).Select( i => _rand.Get(WordSet)));
        }

        public string GetPhraseRandomLength() {
            var length = new Random().Next(Range.Start.Value, Range.End.Value + 1);
            return GetPhrase(length);
        }

        private static void ValidateLength(int length) {
            if (length < Range.Start.Value || length > Range.End.Value) {
                throw new ArgumentException($"lenght {length} out of range 1-3");
            }
        }
        
        private static List<string> CreateSet() =>
            File.ReadAllLines(FilePath).Select(w => w.ToLower()).Distinct().ToList();
    }
}