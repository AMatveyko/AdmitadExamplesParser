// a.snegovoy@gmail.com

using System.Collections.Generic;

namespace AdmitadExamplesParser.Entities
{
    internal static class StatisticsContainer
    {

        private static List<StatisticsBlock> _blocks = new();

        public static StatisticsBlock GetBlock( string componentName ) {
            var block = new StatisticsBlock( componentName );
            _blocks.Add( block );
            return block;
        }

        public static IEnumerable<string> GetAllLines()
        {
            foreach( var block in _blocks ) {
                yield return $"Component: { block.ComponentName }";
                foreach( var (line, time) in block.Lines ) {
                    yield return time == null ? line : $"{line}, {GetTimeValue(time.Value)}";
                }

                yield return $"Component work time: {GetTimeValue( block.WorkTime )}";
                yield return string.Empty;
            }
        }

        private static string GetTimeValue(
            long timeInMs ) =>
            $"{timeInMs / 1000}.{timeInMs % 1000}s";

    }
}