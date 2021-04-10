// a.snegovoy@gmail.com

using System.Collections.Generic;

using AdmitadCommon.Entities;

namespace FatApi.Model
{
    public sealed class ProductSearchResult
    {
        public int OffSet { get; set; }
        public int FrameSize { get; set; }
        public int Count => Products.Count;
        public int TotalCount { get; set; }
        public List<Product> Products { get; set; }
    }
}