// a.snegovoy@gmail.com

using System.Collections.Generic;

namespace AdmitadCommon.Entities
{
    public sealed class ColorProperty : BaseProperty
    {
        public string ParentId { get; set; }
        public string Name { get; set; }
        public string LatinName { get; set; }
        public override string FieldName => "colors";
        public override List<string> FieldsForSearch =>
            new() {
                "name",
                "model",
                "typeprefix",
                "categoryName",
                "param"
            };
    }
}