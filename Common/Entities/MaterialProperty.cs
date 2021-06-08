// a.snegovoy@gmail.com

using System.Collections.Generic;

namespace Common.Entities
{
    public sealed class MaterialProperty : BaseProperty
    {
        public override string FieldName => "materials";
        public override List<string> FieldsForSearch =>
            new List<string> {
                "name",
                "model",
                "typeprefix",
                "categoryName",
                "param"
            };
    }
}