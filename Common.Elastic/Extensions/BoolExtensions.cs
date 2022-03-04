// a.snegovoy@gmail.com

using System.Linq;

using Common.Elastic.Entities;
using Common.Entities;

using Nest;

namespace Common.Elastic.Extensions
{
    internal static class BoolExtensions
    {
        public static IBoolQuery FromSearchParameters( this BoolQueryDescriptor<Product> input, SearchProperties properties )
        {
            var must = properties.GetMustTerms();
            var mustNot = properties.GetMustNotTerms();
            
            if( must.Any() ) {
                input.Must( must.ToArray() );
            }

            if( mustNot.Any() ) {
                input.MustNot( mustNot );
            }

            return input;
        }
    }
}