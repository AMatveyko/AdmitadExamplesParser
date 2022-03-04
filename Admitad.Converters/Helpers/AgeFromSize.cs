// a.snegovoy@gmail.com

using System;

using Admitad.Converters.Entities;

using Common.Entities;

namespace Admitad.Converters.Helpers
{
    internal sealed class AgeFromSize
    {

        private readonly Func<Offer, ProductType> _productTypeGetter;
        private readonly Func<Offer, bool> _isBaby;
        private readonly Func<Offer, SizeOptions> _optionsGetter;

        public AgeFromSize(
            Func<Offer, ProductType> productTypeGetter,
            Func<Offer, bool> isBaby,
            Func<Offer, SizeOptions> optionsGetter ) =>
            ( _productTypeGetter, _isBaby, _optionsGetter ) = ( productTypeGetter, isBaby, optionsGetter );

        public void Fill( Offer offer )
        {
            if( _productTypeGetter( offer ) != ProductType.Clothing ) {
                return;
            }

            var options = _optionsGetter( offer );

            offer.AgeRange ??= SizeTable.GetRange( options.Value, options.Type, _isBaby( offer ) );

        }


    }
}