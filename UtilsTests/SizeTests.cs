// a.snegovoy@gmail.com

using System.Collections.Generic;
using System.Linq;

using Admitad.Converters.Entities;

using Common.Entities;

using NUnit.Framework;

namespace UtilsTests
{
    public class SizeTests
    {
        [ Test ]
        public void SizesTests()
        {
            var values = new List<(string, SizeType, bool)>() {
                ( "28", SizeType.Ru, true ),
                ( "28", SizeType.Ru, false ),
                ( "177", SizeType.Eu, false ),
                ( "74", SizeType.Eu, true ),
                ( "104", SizeType.Eu, true ),
                ( "104", SizeType.Eu, false ),
                ( "105", SizeType.Eu, true )
            };

            var ranges = 
                values.Select( v => SizeTable.GetRange( v.Item1, v.Item2, v.Item3 ) ).ToList();
        }
    }
}