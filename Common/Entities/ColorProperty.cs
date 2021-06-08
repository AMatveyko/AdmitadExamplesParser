// a.snegovoy@gmail.com

using System.Collections.Generic;

namespace Common.Entities
{
    public sealed class ColorProperty : BaseProperty
    {
        public string ParentId { get; set; }
        public string Name { get; set; }
        public string LatinName { get; set; }
        public override string FieldName => "colors";
        public override List<string> FieldsForSearch =>
            new List<string>() {
                "name",
                "model",
                "typeprefix",
                "categoryName",
                "param"
            };
    }
}