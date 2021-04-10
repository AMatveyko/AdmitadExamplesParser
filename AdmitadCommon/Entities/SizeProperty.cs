// a.snegovoy@gmail.com

using System.Collections.Generic;

namespace AdmitadCommon.Entities
{
    public class SizeProperty : BaseProperty
    {
        public override string FieldName => "sizes";
        public override List<string> FieldsForSearch =>
            new() {
                "param"
            };
    }
}