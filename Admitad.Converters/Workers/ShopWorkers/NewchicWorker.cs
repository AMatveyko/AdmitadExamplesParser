// a.snegovoy@gmail.com

using System;
using System.Collections.Generic;
using System.Linq;

using AdmitadCommon;
using AdmitadCommon.Entities;
using AdmitadCommon.Extensions;

using AdmitadSqlData.Helpers;

namespace Admitad.Converters.Workers.ShopWorkers
{
    internal sealed class NewchicWorker : BaseShopWorker, IShopWorker
    {

        private Dictionary<string, string> _colors;
        
        public NewchicWorker()
        {
            //_handlers.Add( new CurrencyUSD() );
            var colors = DbHelper.GetColors();
            _colors = colors.Distinct( new ColorComparer() )
                .ToDictionary( c => c.LatinName.Trim(), c => c.Name.Trim() );
        }

        protected override Offer GetTunedOffer( Offer offer, RawOffer rawOffer )
        {

            var rawParams = new[] {
                ( rawOffer.Colors, Constants.Params.ColorName ),
                ( rawOffer.Size, Constants.Params.SizeName )
            };

            foreach( var rawParam in rawParams ) {
                FillParams( offer, rawParam.Item1, rawParam.Item2 );
            }

            return offer;
        }

        private void FillParams(
            Offer offer,
            string rawParams,
            string paramName ) {
            if( rawParams.IsNullOrWhiteSpace() ) {
                return;
            }
            foreach( var value in rawParams.ToLower().Split(',') ) {
                
                if( paramName == Constants.Params.ColorName ) {
                    var modifiedValue = value;
                    foreach( var key in _colors.Keys ) {
                        if( modifiedValue.Contains( key ) ) {
                            modifiedValue = modifiedValue.Replace( key, _colors[ key ] );
                        }
                    }
                    offer.AddParamIfNeed(  new RawParam {
                        NameFromXml = paramName,
                        ValueFromXml = modifiedValue
                    } );
                    
                    continue;
                }
                
                offer.AddParamIfNeed(  new RawParam {
                    NameFromXml = paramName,
                    ValueFromXml = value
                } );
            }
            
        }
        
        private sealed class ColorComparer : IEqualityComparer<ColorProperty>
        {
            public bool Equals(
                ColorProperty x,
                ColorProperty y )
            {
                return x.LatinName == y.LatinName;
            }

            public int GetHashCode(
                ColorProperty obj )
            {
                return HashCode.Combine( obj.LatinName );
            }
        }
    }
}