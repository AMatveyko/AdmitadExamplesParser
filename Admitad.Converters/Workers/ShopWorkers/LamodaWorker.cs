// a.snegovoy@gmail.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using Admitad.Converters.Entities;

using AdmitadSqlData.Helpers;

using Common;
using Common.Entities;

namespace Admitad.Converters.Workers.ShopWorkers
{
    internal sealed class LamodaWorker : BaseShopWorker, IShopWorker
    {

        private static Regex _ageLetters = new("(m|t|y)", RegexOptions.Compiled);
        private static Regex _digits = new(@"(?<digits>\d+)", RegexOptions.Compiled);
        private const char ParamSplitter = ',';

        public LamodaWorker(
            DbHelper dbHelper )
            : base( dbHelper ) { }

        protected override Offer GetTunedOffer( Offer offer, RawOffer rawOffer )
        {
            FillAgeRange( offer );
            return offer;
        }

        private static void FillAgeRange( Offer offer )
        {
            var parameters = offer.Params.Where( p => p.Name.ToLower() == "размер" ).ToList();
            if( parameters.Any() == false ) {
                return;
            }

            var isBaby = offer.CategoryPath.Contains( "Новорожденным" );

            var ranges = parameters.SelectMany( p => p.Values.Select( v => CreateRange( p.Unit, v, isBaby ) ) );
            var range = AgeRange.GetMaxRange( ranges );
            
            offer.AgeRange = range;
            
        }

        private static AgeRange CreateRange( string unit, string value, bool isBaby )
        {
            var clearedValue = value.Replace( ",5", string.Empty );
            return unit.ToLower() switch {
                "cm" => SizeSm( clearedValue, isBaby ),
                "сm" => SizeSm( clearedValue, isBaby ),
                "eu" => SizeTable.SizeEu( clearedValue, isBaby ),
                "еu" => SizeTable.SizeEu( clearedValue, isBaby ),
                "ru" => SizeTable.SizeRu( clearedValue, isBaby ),
                "age" => SizeAge( clearedValue, isBaby ),
                _ => OtherSizes( value, isBaby )
            };
        }

        private static AgeRange OtherSizes( string value, bool isBaby )
        {
            var m = _digits.Match( value );
            if( m.Success == false ||
                int.TryParse( m.Groups[ "digits" ].Value, out var digits ) == false ) {
                return null;
            }
            
            
            return digits < SizeTable.SizeSmThreshold ? SizeTable.SizeRu( value, isBaby ) : SizeSm( value, isBaby );
        }
        
        private static AgeRange SizeAge( string value, bool isBaby )
        {
            var m = _ageLetters.Match( value );
            return m.Success 
                ? SizeAgeFromYears( value ) 
                : SizeAgeEu( value, isBaby );
        }

        private static AgeRange SizeAgeEu( string value, bool isBaby )
        {
            if( int.TryParse( value, out var digits ) == false ) {
                return null;
            }

            return digits < SizeTable.SizeSmThreshold ? SizeTable.SizeRu( value, isBaby ) : SizeSm( value, isBaby );
        }
        
        private static AgeRange SizeAgeFromYears( string value )
        {
            var processedValue = value.ToLower().Replace( "t", "y" );
            var isYears = processedValue.Contains( "y" );
            var m = _digits.Match( processedValue );
            if( m.Success == false || int.TryParse( m.Groups["digits"].Value, out var digits ) == false ) {
                return null;
            }

            return isYears ? AgeRange.GetFromYear( digits ) : AgeRange.GetFromMonths( digits );
        }

        private static AgeRange SizeSm( string value, bool isBaby ) => SizeTable.SizeEu( value, isBaby );

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