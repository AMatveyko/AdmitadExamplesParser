// a.snegovoy@gmail.com

using System;
using System.Linq;
using System.Text.RegularExpressions;

using AdmitadCommon.Entities;

using AdmitadSqlData.Helpers;

using Common.Entities;

using SixLabors.ImageSharp;

namespace Admitad.Converters.Workers.ShopWorkers
{
    internal sealed class YooxWorker : BaseShopWorker, IShopWorker
    {


        private static Regex Half = new( " ½", RegexOptions.Compiled );
        
        private const string MonthsUnit = "months";
        private const string YearsUnit = "years";

        public YooxWorker(
            DbHelper dbHelper )
            : base( dbHelper ) { }

        protected override Offer GetTunedOffer( Offer offer, RawOffer rawOffer )
        {
            var genderedOffer = FillAgeAndGender( offer, rawOffer );
            return FillAgeRange( genderedOffer );
        }

        private static Offer FillAgeRange( Offer offer )
        {
            var ageParams = offer.Params.Where( p => p.Unit is MonthsUnit or YearsUnit ).ToList();
            if( ageParams.Count > 1 ) {
                foreach( var param in ageParams ) {
                    Console.WriteLine( $"{offer.Id} {param.Name} {param.Unit}: { string.Join( ",",param.Values ) }" );
                }
            }

            if( ageParams.Any() == false ) {
                return offer;
            }

            var @params = ageParams.First();

            offer.AgeRange = GetRange( string.Join( ",", @params.Values ), @params.Unit );
            
            return offer;

        }

        private static AgeRange GetRange( string values, string type )
        {
            var processedValues = Half.Replace( values, ".5" ).Replace( "/", "," );
            var intValues = processedValues.Split( "," ).Select(
                v => {
                    var result = decimal.TryParse( v, out var number );
                    return ( Result: result, Number: number );
                } ).Where( i => i.Result )
                .Select( i => type == YearsUnit ? i.Number * 12 : i.Number ).ToList();
            return new AgeRange {
                From = intValues.Min(),
                To = intValues.Max()
            };
        }
        
        private Offer FillAgeAndGender(
            Offer offer,
            RawOffer rawOffer )
        {
            var ageParam = rawOffer.Params.FirstOrDefault( p => p.Name == AgeParamName );
            var genderParam = rawOffer.Params.FirstOrDefault( p => GenderParamName.Contains( p.Name ) );
            if( ageParam != null ) {
                offer.Age = ageParam.Value switch {
                    "взрослый" => Age.Adult,
                    "none" => Age.All,
                    "дети" => Age.Child,
                    "младший" => Age.Child,
                    "ребенок" => Age.Child,
                    _ => Age.All
                };
            }

            if( genderParam != null ) {
                var gender = genderParam.Value switch {
                    "женщина" => Gender.Woman,
                    "мужчина" => Gender.Man,
                    "none" => Gender.Unisex,
                    _ => throw new ArgumentOutOfRangeException()
                };
                if( offer.Age == Age.Adult ) {
                    offer.Gender = gender;
                }

                if( offer.Age == Age.Child ) {
                    if( gender == Gender.Man ) {
                        offer.Gender = Gender.Boy;
                    }

                    if( gender == Gender.Woman ) {
                        offer.Gender = Gender.Girl;
                    }
                }
            }

            return offer;
        }
    }
}