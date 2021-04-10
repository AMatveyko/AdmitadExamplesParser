// a.snegovoy@gmail.com

using System;
using System.Collections.Generic;
using System.Linq;

using AdmitadCommon;
using AdmitadCommon.Entities;

namespace AdmitadExamplesParser.Workers.ShopWorkers
{
    internal sealed class LamodaWorker : BaseShopWorker, IShopWorker
    {

        private const char ParamSplitter = ',';

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

        // protected override List<Param> GetParams( RawOffer rawOffer )
        // {
        //     var @params = base.GetParams( rawOffer );
        //     @params.AddRange( GetMaterialParam( rawOffer ) );
        //     return @params;
        // }

        // private static IEnumerable<Param> GetMaterialParam( RawOffer rawOffer )
        // {
        //     var materialParams = rawOffer.Params.Where( p => p.Name.Contains( Constants.Params.MaterialName ) );
        //     var cleanParams = new List<Param>();
        //     foreach( var param in materialParams ) {
        //         if( param.Value.Contains( ParamSplitter ) ) {
        //             cleanParams.AddRange( SplitMaterialParam( param ) );
        //         }
        //         else {
        //             cleanParams.Add( new Param( Constants.Params.MaterialName, param.Unit, param.Value ) );
        //         }
        //     }
        //
        //     return cleanParams;
        // }

        // private static IEnumerable<Param> SplitMaterialParam( RawParam param )
        // {
        //     var values = param.Value.Split( ParamSplitter );
        //     foreach( var value in values ) {
        //         yield return new Param( Constants.Params.MaterialName, param.Unit, value );
        //     }
        // }

        protected override Age GetAgeFromParam( IEnumerable<RawParam> @params )
        {
            var value = GetParamValueByName( AgeParamName, @params );
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