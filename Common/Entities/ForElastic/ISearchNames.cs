// a.snegovoy@gmail.com

using System.Collections.Generic;

namespace Common.Entities.ForElastic
{
    public interface ISearchNames
    {
        List<string> SearchNames { get; set; }
        List<string> FieldsForSearch { get; }
    }
}