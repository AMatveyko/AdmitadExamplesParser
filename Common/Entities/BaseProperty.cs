// a.snegovoy@gmail.com

using System.Collections.Generic;

using Common.Entities.ForElastic;

namespace Common.Entities
{
    public abstract class BaseProperty : ISearchNames
    {
        public string Id { get; set; }
        public List<string> Names { get; set; }
        public abstract string FieldName { get; }
        public List<string> SearchNames { get; set; }
        public abstract List<string> FieldsForSearch { get; }
    }
}