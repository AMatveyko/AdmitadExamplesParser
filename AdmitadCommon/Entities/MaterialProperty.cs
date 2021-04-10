// a.snegovoy@gmail.com

using System.Collections.Generic;

namespace AdmitadCommon.Entities
{
    public class MaterialProperty : BaseProperty
    {
        public override string FieldName => "materials";
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