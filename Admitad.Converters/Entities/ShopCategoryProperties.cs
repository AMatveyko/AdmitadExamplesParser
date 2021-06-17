// a.snegovoy@gmail.com

using Common.Entities;

namespace Admitad.Converters.Entities
{
    internal sealed class ShopCategoryProperties
    {
        public ShopCategoryProperties( ProductType type, Gender? gender = null, Age? age = null ) =>
            ( Gender, Age, Type ) = ( gender, age, type );
        
        public Gender? Gender { get; }
        public Age? Age { get; }
        public ProductType Type { get; }
    }
}