// a.snegovoy@gmail.com

using System.Collections.Generic;
using System.Linq;

namespace Common.Entities
{
    public static class StatisticsContainer
    {

        private static List<StatisticsBlock> _blocks = new List<StatisticsBlock>();

        public static StatisticsBlock GetNewBlock( string componentName ) {
            var block = new StatisticsBlock( componentName );
            _blocks.Add( block );
            return block;
        }

        public static StatisticsBlock GetSumBlockByName( string componentName )
        {
            var sumBlock = new StatisticsBlock( componentName );
            var blocks = _blocks.Where( b => b.ComponentName == componentName );
            foreach( var block in blocks ) {
                block.Lines.ForEach( l => sumBlock.AddLine( l ) );
                sumBlock.WorkTime += block.WorkTime;
            }

            return sumBlock;
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