// a.snegovoy@gmail.com

using System.Linq;

using AdmitadCommon.Entities;
using AdmitadCommon.Helpers;

namespace Admitad.Converters.Workers.ShopWorkers
{
    internal sealed class AnabelWorker : BaseShopWorker, IShopWorker
    {
        protected override void FillParams(
            IExtendedOffer extendedOffer,
            RawOffer rawOffer ) {
            extendedOffer.Age = Age.Adult;
            extendedOffer.Gender = Gender.Woman;
            rawOffer.Params = RawParamHelper.GetRawParamFromNameAnabel( rawOffer.Name );
            base.FillParams( extendedOffer, rawOffer );
        }

        protected override Offer GetTunedOffer(
            Offer offer,
            RawOffer rawOffer )
        {
            //делаем из
            //https://anabel24.ru/image/cache/catalog/public/b5/4d/a3/5447e1acd270687b4ab66a9393098226d4-200x200.jpg
            //вот такое
            //https://anabel24.ru/image/catalog/public/b5/4d/a3/5447e1acd270687b4ab66a9393098226d4.jpg
            if( offer.Photos != null &&
                offer.Photos.Any() ) {
                offer.Photos = offer.Photos
                    .Select( p => p.Replace( "/cache", string.Empty ) )
                    .Select( p => p.Replace( "-200x200", string.Empty ) ).ToList();
            }

            return offer;
        }
    }
}