// a.snegovoy@gmail.com

using System;
using System.Linq;
using System.Text.RegularExpressions;

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

        private const string MonthsConst = "months";
        private const string YearsConst = "years";

        private const string AgeParam = "возраст";
        private const string AlternateAgeParam = "возраст ребенка";
        
        private static readonly Regex Digits = new ( @"\d+", RegexOptions.Compiled );
        private static readonly Regex StartRange = new("(рождения|для новорожденных)", RegexOptions.Compiled);
        private static readonly Regex Month =
            new( @"(месяца|месяцев|месяц|months|month|мес(\.|\+)?)", RegexOptions.Compiled );
        private static readonly Regex Years =
            new( @"(года|год|years|лет|л|г|\+)", RegexOptions.Compiled );
        private static readonly Regex FromYear = new(@"от\s+года", RegexOptions.Compiled);
        private static readonly Regex ToYear = new(@"до\s+года", RegexOptions.Compiled);
        private static readonly Regex Coma = new(",", RegexOptions.Compiled);
        private static readonly Regex Remove = new("(от |c |с )", RegexOptions.Compiled);
        private static readonly Regex Split = new("(?: до )|-", RegexOptions.Compiled);


        public DochkiSinochkiWorker( DbHelper dbHelper, Func<RawOffer, int, string> idGetter = null )
            : base( dbHelper, idGetter )
        {
            Handlers.Add( new ProcessPropertiesCategory( new DochkiSinochkiCategoryContainer() ) );
        }

        protected override Offer GetTunedOffer( Offer offer, RawOffer rawOffer )
        {
            var agedOffer = GetAgeGender( offer );
            return GetAgeRangeOffer( agedOffer );
        }

        private static Offer GetAgeRangeOffer( Offer offer )
        {
            var ageParams = offer.Params.Where( CheckParam );
            var convertedParams = ageParams
                    .SelectMany( p => p.Values.Select( v => $"{p.Unit} {v}" ) )
                    .Select( p => FromYear.Replace( p, "от 1 года" ) )
                    .Select( p => ToYear.Replace( p, "до 1 года" ) )
                    .Select( p => StartRange.Replace( p, "0" ) )
                    .Select( p => Coma.Replace( p, "." ) )
                    .Select( p => Month.Replace( p, MonthsConst ) )
                    .Select( p => Years.Replace( p, YearsConst ) )
                    .Select( p => Split.Split( p ) );
            
            var ranges = convertedParams.Select( Convert ).ToList();

            offer.AgeRange = AgeRange.GetMaxRange( ranges );
            
            return offer;
        }

        private static AgeRange Convert( string[] rawRange )
        {
            var result = new AgeRangeExtended { From = AgeRange.Min, To = AgeRange.Max };
            
            var withTo = rawRange.Any() && ( Remove.IsMatch( rawRange[0] ) || rawRange.Length == 2 );
            
            var clearedRanges =
                rawRange.Select( r => Remove.Replace( r, string.Empty ) ).ToList();
            
            if( clearedRanges.Any() == false ) {
                return result;
            }

            ( result.From, result.FromUnit ) = GetAgeParameters( clearedRanges.First() );

            if( clearedRanges.Count > 1 ) {
                ( result.To, result.ToUnit ) = GetAgeParameters( clearedRanges[ 1 ] );
            }

            result.FromUnit ??= result.ToUnit ?? YearsConst;
            result.ToUnit ??= result.FromUnit ?? YearsConst;

            result.From = GetAgeValue( result.From, result.FromUnit );
            result.To = withTo
                ? GetAgeValue( result.To, result.ToUnit )
                : result.From;
            
            return result;

        }

        private static decimal GetAgeValue( decimal value, string unit ) =>
            unit == YearsConst ? value * 12 : value;

        private static ( decimal, string ) GetAgeParameters( string rawAge )
        {
            ( decimal value, string unit ) result = ( 0, null );
            if( rawAge.Contains( YearsConst ) ) {
                result.unit = YearsConst;
            }
            else if( rawAge.Contains( MonthsConst ) ) {
                result.unit = MonthsConst;
            }

            var text =
                Regex.Replace( rawAge, $"({YearsConst}|{MonthsConst})", string.Empty ).Trim();

            if( decimal.TryParse( text, out var decimalValue ) ) {
                result.value = decimalValue;
            }

            return result;
        }
        
        private static bool CheckParam( Param param )
        {
            var name = param.Name.ToLower();
            return name is AgeParam or AlternateAgeParam;
        }
        
        private static Offer GetAgeGender( Offer offer )
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