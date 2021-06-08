// a.snegovoy@gmail.com

using System.Collections.Generic;

using AdmitadCommon;
using AdmitadCommon.Entities;

using AdmitadSqlData.Helpers;

namespace Admitad.Converters.Workers.ShopWorkers
{
    internal sealed class AdidasWorker : BaseShopWorker, IShopWorker
    {
        
        public AdidasWorker(
            DbHelper dbHelper )
            : base( dbHelper ) { }
        
        // protected override string GetClearlyVendor(
        //     string vendor ) =>
        //     "adidas";

        protected override void FillParams(
            IExtendedOffer extendedOffer,
            RawOffer rawOffer )
        {
            rawOffer.Params = FixSizeParams( rawOffer.Params );
            base.FillParams( extendedOffer, rawOffer );
            if( extendedOffer.Gender == Gender.Undefined ) {
                extendedOffer.Gender = Gender.Unisex;
            }
        }

        private static List<RawParam> FixSizeParams( List<RawParam> @params ) {
            var fixedSize = new List<RawParam>();
            foreach( var param in @params ) {
                if( param.Name == Constants.Params.SizeName && param.Value.Contains( "-" ) ) {
                    fixedSize.AddRange( SplitSize( param ) );
                }
                else {
                    fixedSize.Add( param );
                }
            }

            return fixedSize;
        }

        private static IEnumerable<RawParam> SplitSize( RawParam param )
        {
            var sizes = param.Value.Split( "-" );
            if( int.TryParse( sizes[ 0 ], out var from ) &&
                int.TryParse( sizes[ 1 ], out var to ) ) {
                for( var i = from; i <= to; i++ ) {
                    yield return new RawParam{ NameFromXml = param.Name.ToLower(), ValueFromXml = i.ToString() };
                }
            }
            else {
                yield return param;
            }
        }
    }
}