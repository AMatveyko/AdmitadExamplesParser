// a.snegovoy@gmail.com

using System.Collections.Generic;

namespace AdmitadCommon.Entities
{
    public sealed class SizeProperty : BaseProperty
    {
        public override string FieldName => "sizes";
        public override List<string> FieldsForSearch =>
            new() {
                "param"
            };
    }
}