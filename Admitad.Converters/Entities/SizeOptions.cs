// a.snegovoy@gmail.com

using Common.Entities;

namespace Admitad.Converters.Entities
{
    internal sealed class SizeOptions
    {

        public SizeOptions( SizeType type, string value ) =>
            ( Type, Value ) = ( type, value );
        
        public SizeType Type { get; }
        public string Value { get; }
    }
}