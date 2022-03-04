// a.snegovoy@gmail.com

using System;
using System.Collections.Generic;

using Common.Elastic.Helpers;
using Common.Entities;

using Nest;

namespace Common.Elastic.Entities
{
    internal sealed class SearchProperties
    {
        public SearchTerms Must { get; set; }
        public SearchTerms MustNot { get; set; }

        public List<Func<QueryContainerDescriptor<Product>, QueryContainer>> GetMustTerms() =>
            SearchParametersConverter.GetTerms( Must );
        public List<Func<QueryContainerDescriptor<Product>, QueryContainer>> GetMustNotTerms() =>
            SearchParametersConverter.GetTerms( MustNot );
    }
}