// a.snegovoy@gmail.com

using System.Collections.Generic;

namespace AdmitadCommon.Entities
{
    public abstract class BaseProperty
    {
        public string Id { get; set; }
        public List<string> Names { get; set; }
        public abstract string FieldName { get; }
        public abstract List<string> FieldsForSearch { get; }
    }
}