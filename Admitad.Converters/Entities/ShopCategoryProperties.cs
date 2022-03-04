// a.snegovoy@gmail.com

using Common.Entities;

namespace Admitad.Converters.Entities
{
    internal sealed class ShopCategoryProperties
    {
        public ShopCategoryProperties( ProductType type, Gender? gender = null, Age? age = null, AgeRange range = null ) =>
            ( Gender, Age, Type, AgeRange ) = ( gender, age, type, range );
        
        public Gender? Gender { get; }
        public Age? Age { get; }
        public ProductType Type { get; }
        public AgeRange AgeRange { get; }
    }
}