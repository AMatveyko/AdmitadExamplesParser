// a.snegovoy@gmail.com

using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using Common.Elastic.Entities;
using Common.Entities;
using Common.Extensions;

using Nest;

namespace Common.Elastic.Helpers
{
    internal static class SearchParametersConverter
    {
        public static List<Func<QueryContainerDescriptor<Product>, QueryContainer>>GetTerms( SearchTerms terms )
        {
            var list = new List<Func<QueryContainerDescriptor<Product>, QueryContainer>>();
            
            if( terms == null ) {
                return list;
            }
            
            if( terms.CategoryId.IsNotNullOrWhiteSpace() ) {
                list.Add( GetTerm( p => p.Categories, terms.CategoryId ) );
            }

            if( terms.ProductId.IsNotNullOrWhiteSpace() ) {
                list.Add( GetTerm( p => p.Id, terms.ProductId ) );
            }

            return list;
        }

        private static Func<QueryContainerDescriptor<Product>, QueryContainer> GetTerm<TValue>(
            Expression<Func<Product, TValue>> field,
            object value ) =>
            i => i.Term( t => t.Field( field ).Value( value ) );

    }
}