// a.snegovoy@gmail.com

using System.Linq;

using AdmitadCommon.Entities;
using AdmitadCommon.Extensions;

using static System.Int32;

namespace Admitad.Converters.Workers.ShopWorkers
{
    internal sealed class GloriaJeansWorker : BaseShopWorker, IShopWorker
    {

        private const string SizeParam = "размер";
        private const string Boys = "мальчиков";
        private const string Girls = "девочек";
        private const string Years = "years";
        private const string Mounth = "mounth";
        private const string Month = "month";
        
        protected override Offer GetTunedOffer( Offer offer, RawOffer rawOffer )
        {

            if( rawOffer.CategoryPath.Contains( Boys ) ) {
                offer.Age = Age.Child;
                offer.Gender = Gender.Boy;
            } else if( rawOffer.CategoryPath.Contains( Girls ) ) {
                offer.Age = Age.Child;
                offer.Gender = Gender.Girl;
            }
            var param = rawOffer.Params.FirstOrDefault( p => p.Name == SizeParam );
            if( param != null ) {
                if( param.UnitFromXml == Mounth ||
                    param.UnitFromXml == Month ) {
                    offer.Age = Age.Child;
                    offer.Gender = Gender.Child;
                } else if( param.UnitFromXml == Years ) {
                    var year = GetYear( param.Value );
                    if( year <= 3 ) {
                        offer.Age = Age.Child;
                        offer.Gender = Gender.Child;
                    } else if( year <= 15) {
                        offer.Age = Age.Child;
                    }
                }
            }

            return offer;

        }

        private int GetYear( string data )
        {
            if( data.IsNotNullOrWhiteSpace() ) {
                return 0;
            }
            
            var parts = data.Split( "-" );
            if( parts.Any() ) {
                var forParsing = parts.Length > 1 ? parts[ 1 ] : parts[ 0 ];
                TryParse( forParsing, out var year );
                return year;
            }

            return 0;
        }
    }
}