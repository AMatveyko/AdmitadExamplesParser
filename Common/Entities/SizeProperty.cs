// a.snegovoy@gmail.com

using System.Collections.Generic;

namespace Common.Entities
{
    public sealed class SizeProperty : BaseProperty
    {
        public override string FieldName => "sizes";
        public override List<string> FieldsForSearch =>
            new List<string> {
                "param"
            };
    }
}