// a.snegovoy@gmail.com

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using Admitad.Converters.Handlers;

using AdmitadCommon;
using AdmitadCommon.Entities;
using AdmitadCommon.Extensions;
using AdmitadCommon.Helpers;

using AdmitadSqlData.Helpers;

namespace Admitad.Converters.Workers.ShopWorkers
{
    internal abstract class BaseShopWorker
    {
        private static readonly string[] RequiredParams = { Constants.Params.ColorName, Constants.Params.SizeName, Constants.Params.MaterialName };
        private const string SkipValue = "none";
        private const string CountryParam = "Страна-изготовитель";
        
        protected const string AgeParamName = "возраст";
        protected string[] GenderParamName = { "пол", "gender" };
        protected List<IOfferHandler> _handlers = new ();

        private static Regex _pricePattern = new Regex( @"(?<price>\d+(\.\d{2})?)", RegexOptions.Compiled );

        public Offer Convert( RawOffer rawOfer )
        {

            if( rawOfer == null ) {
                rawOfer = new RawOffer {
                    Description = "null"
                };
            }
            
            var offer = new Offer();
            
            if( rawOfer.IsDeleted ) {
                offer.SoldOut = true;
                return offer;
            }

            FillBaseOffer( offer, rawOfer );
            DoFillExtendedOffer( offer, rawOfer );
            var tunedOffer = GetTunedOffer( offer, rawOfer );
            var processedOffer = RunOfferHandlers( tunedOffer, rawOfer );
            return processedOffer;
        }

        private Offer RunOfferHandlers( Offer offer, RawOffer rawOffer )
        {
            foreach( var offerHandler in _handlers ) {
                offer = offerHandler.Process( offer, rawOffer );
            }

            return offer;
        }
        
        protected virtual Offer GetTunedOffer(
            Offer offer,
            RawOffer rawOffer ) =>
            offer;

        private static decimal GetPrice( string rawPrice ) {
            const decimal nullPrice = 0;
            if( rawPrice.IsNullOrWhiteSpace() ) {
                return nullPrice;
            }
            var m = _pricePattern.Match( rawPrice );
            
            if( m.Success == false ) {
                return nullPrice;
            }

            var cleanPrice = m.Groups[ "price" ].Value;
            return decimal.TryParse( cleanPrice, out var price ) ? price : nullPrice;
        }
        
        
        private void FillBaseOffer( IBaseOffer offer, RawOffer rawOffer )
        {
            var price = GetPrice( rawOffer.Price );
            var oldPrice = GetPrice( rawOffer.OldPriceClean );
            offer.Id = HashHelper.GetMd5Hash( rawOffer.ShopName, rawOffer.OfferId );
            offer.ProductId = HashHelper.GetMd5Hash( rawOffer.Pictures.FirstOrDefault() ?? rawOffer.Url );
            offer.Url = rawOffer.Url;
            offer.Currency = CurrencyHelper.GetCurrency( rawOffer.CurrencyId );
            offer.Description = rawOffer.Description;
            offer.Discount = DiscountHelper.CalculateDiscount( price, oldPrice );
            //offer.Enable = true;
            offer.Model = rawOffer.Model;
            offer.OldPrice = oldPrice;
            offer.TypePrefix = rawOffer.TypePrefix;
            offer.MarketCategory = rawOffer.MarketCategory;
            offer.CategoryPath = rawOffer.CategoryPath;
            offer.Name = rawOffer.Name;
            offer.Photos = rawOffer.Pictures;
            offer.Price = price;
            offer.ShopId = DbHelper.GetShopId( rawOffer.ShopNameLatin );
            offer.UpdateDate = rawOffer.UpdateTime;
            offer.Delivery = rawOffer.IsDelivered ?? false;
            offer.SalesNotes = rawOffer.SalesNotes;
        }

        
        
        private static Age AgeFromGender(
            Gender gender )
        {
            return gender switch {
                Gender.Boy => Age.Child,
                Gender.Child => Age.Child,
                Gender.Girl => Age.Child,
                Gender.Man => Age.Adult,
                Gender.Woman => Age.Adult,
                _ => Age.Undefined
            };
        }

        private static Gender GenderFromGenderAndAge( Age age, Gender gender ) {
            if( age == Age.Adult ) {
                if( gender == Gender.Man ) {
                    return Gender.Man;
                }

                if( gender == Gender.Woman ) {
                    return Gender.Woman;
                }
            }
            else if( age == Age.Child ) {
                if( gender == Gender.Man ) {
                    return Gender.Boy;
                }

                if( gender == Gender.Woman ) {
                    return Gender.Girl;
                }
            }

            return gender;
        }
        
        protected void DoFillExtendedOffer(
            IExtendedOffer extendedOffer,
            RawOffer rawOffer )
        {


            var gender = GetGenderFromParam( rawOffer.Params );
            var age = GetAgeFromParam( rawOffer.Params );

            if( gender == Gender.Undefined ) {
                gender = GetGenderFromCategory( rawOffer );
            }


            if( gender != Gender.Undefined &&
                age != Age.Undefined ) {
                gender = GenderFromGenderAndAge( age, gender );
            } else if( gender != Gender.Undefined &&
                       age == Age.Undefined ) {
                age = AgeFromGender( gender );
            }

            extendedOffer.Age = age;
            extendedOffer.Gender = gender;
            
            extendedOffer.CountryId = GetCountryId( rawOffer );
            extendedOffer.VendorNameClearly = GetClearlyVendor( rawOffer.Vendor );
            extendedOffer.CategoryId = rawOffer.CategoryId;

            FillParams( extendedOffer, rawOffer );
        }

        protected virtual int GetCountryId( RawOffer rawOffer )
        {
            var value = GetParamValueByName( rawOffer.Params, new [] { CountryParam } )
                        ?? rawOffer.Country?.ToLower();
            return DbHelper.GetCountryId( value );
        }

        protected virtual void FillParams( IExtendedOffer extendedOffer, RawOffer rawOffer ) {
            foreach( var param in rawOffer.Params ) {
                extendedOffer.AddParamIfNeed( param );
            }
        }

        protected virtual Age GetAgeFromParam( IEnumerable<RawParam> @params ) {
            var value = GetParamValueByName( @params, new [] { AgeParamName } );
            return AgeHelper.GetAge( value );
        }

        private Gender GetGender( RawOffer rawOffer )
        {
            var gender = GetGenderFromParam( rawOffer.Params );
            return gender == Gender.Undefined ? GetGenderFromCategory( rawOffer ) : gender;
        }

        private static Gender GetGenderFromCategory( RawOffer rawOffer ) =>
            GenderHelper.GetGender( new[] {rawOffer.CategoryPath, rawOffer.MarketCategory} );

        private Gender GetGenderFromParam( IEnumerable<RawParam> @params ) {
            var value = GetParamValueByName( @params, GenderParamName );
            return GenderHelper.GetGender( value );
        }

        protected static string GetParamValueByName(
            IEnumerable<RawParam> @params,
            string[] param ) =>
            @params.FirstOrDefault( p => param.Contains( p.Name.ToLower() ) )?.Value.ToLower();


        protected virtual string GetClearlyVendor( string vendor )
        {
            return BrandHelper.GetClearlyVendor( vendor );
        }

    }
}