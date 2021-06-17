// a.snegovoy@gmail.com

using System;
using System.Linq;

using Admitad.Converters.Entities;
using Admitad.Converters.Handlers;

using AdmitadSqlData.Helpers;

using Common.Entities;

namespace Admitad.Converters.Workers.ShopWorkers
{
    internal sealed class DochkiSinochkiWorker : BaseShopWorker, IShopWorker
    {

        private const string GenderParamNameThis = "пол ребенка";
        private const string GenderMan = "для мальчиков";
        private const string GenderWoman = "для девочек";
        
        public DochkiSinochkiWorker( DbHelper dbHelper, Func<RawOffer, int, string> idGetter = null )
            : base( dbHelper, idGetter )
        {
            Handlers.Add( new ProcessPropertiesCategory( new DochkiSinochkiCategoryContainer() ) );
        }

        protected override Offer GetTunedOffer(
            Offer offer,
            RawOffer rawOffer )
        {
            offer.Age = Age.Child;
            var param = offer.Params.FirstOrDefault( p => p.Name == GenderParamNameThis );
            
            if( param == null ) {
                return offer;
            }

            if( param.Values.Contains( GenderMan ) &&
                param.Values.Contains( GenderWoman ) ) {
                offer.Gender = Gender.Unisex;
                return offer;
            }

            if( param.Values.Contains( GenderMan ) ) {
                offer.Gender = Gender.Man;
                return offer;
            }

            if( param.Values.Contains( GenderWoman ) ) {
                offer.Gender = Gender.Woman;
                return offer;
            }

            return offer;
        }
    }
}