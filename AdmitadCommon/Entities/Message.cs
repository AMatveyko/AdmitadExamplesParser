// a.snegovoy@gmail.com

using System;

namespace AdmitadCommon.Entities
{
    internal sealed record Message( bool Important, DateTime Time, string Text ) {
        public override string ToString() =>
            $"{Time.ToString()} {Text}";
    };
}