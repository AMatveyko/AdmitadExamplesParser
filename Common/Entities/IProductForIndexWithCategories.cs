using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Entities
{
    public interface IProductForIndexWithCategories : IProductForIndex
    {
        [JsonProperty("categories")] string[] Categories { get; }
    }
}
