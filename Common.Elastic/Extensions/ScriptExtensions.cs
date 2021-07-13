// a.snegovoy@gmail.com

using System.Collections.Generic;

using Nest;

namespace Common.Elastic.Extensions
{
    internal static class ScriptExtensions
    {
        public static IScript RemoveCategoryScript( this ScriptDescriptor input, string categoryId )
        {
            const string categoryIdParam = "CategoryId";
            var @params = new Dictionary<string, object> {
                { categoryIdParam, categoryId }
            };
            return input
                //.Source( $"ctx._source.categories.remove( ctx._source.categories.indexOf( params[ '{categoryIdParam}' ] ) );" )
                //.Params( @params );
                .Source( $"ctx._source.categories.remove( ctx._source.categories.indexOf( {categoryId} ) );" );
        }
    }
}