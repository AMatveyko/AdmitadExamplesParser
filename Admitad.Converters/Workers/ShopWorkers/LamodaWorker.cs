// a.snegovoy@gmail.com

using System;
using System.Collections.Generic;
using System.Linq;

using AdmitadCommon;
using AdmitadCommon.Entities;

using AdmitadSqlData.Helpers;

namespace Admitad.Converters.Workers.ShopWorkers
{
    internal sealed class LamodaWorker : BaseShopWorker, IShopWorker
    {

        private const char ParamSplitter = ',';

        public LamodaWorker(
            DbHelper dbHelper )
            : base( dbHelper ) { }
        
        protected override void FillParams(
            IExtendedOffer extendedOffer,
            RawOffer rawOffer )
        {
            base.FillParams( extendedOffer, rawOffer );
            var materialParams = rawOffer.Params.Where( p => p.Name.Contains( Constants.Params.MaterialName ) );
            foreach( var param in materialParams ) {
                var materials = param.Value.Split(
                    ParamSplitter,
                    StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries );
                foreach( var material in materials ) {
                    extendedOffer.AddParamIfNeed( new RawParam{ NameFromXml = Constants.Params.MaterialName, ValueFromXml = material } );
                }
            }
        }

        protected override Age GetAgeFromParam( IEnumerable<RawParam> @params )
        {
            var value = GetParamValueByName( @params, new []{ AgeParamName } );
            return value switch {
                "от 55 лет" => Age.Adult,
                "от 35 лет" => Age.Adult,
                "от 25 лет" => Age.Adult,
                "от 40 лет" => Age.Adult,
                "от 45 лет" => Age.Adult,
                "от 30 лет" => Age.Adult,
                "от 50 лет" => Age.Adult,
                "любой" => Age.All,
                _ => Age.Undefined
            };
        }
        
    }
}