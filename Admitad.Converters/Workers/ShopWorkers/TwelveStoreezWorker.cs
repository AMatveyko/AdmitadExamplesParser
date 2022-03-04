// a.snegovoy@gmail.com

using System.Text.RegularExpressions;

using AdmitadCommon;
using AdmitadCommon.Entities;

using AdmitadSqlData.Helpers;

using Common;
using Common.Entities;

namespace Admitad.Converters.Workers.ShopWorkers
{
    internal class TwelveStoreezWorker : BaseShopWorker, IShopWorker {
        
        private static readonly Regex DefineCountry = new ( @"Сделано в (?<country>[a-zA-Zа-яА-Я0-9_]+)\.", RegexOptions.Compiled );
        
        public TwelveStoreezWorker( DbHelper dbHelper ) : base( dbHelper ) { }
        
        protected override void FillParams(
            IExtendedOffer extendedOffer,
            RawOffer rawOffer )
        {
            extendedOffer.Age = Age.All;
            extendedOffer.Gender = Gender.Unisex;
            extendedOffer.CountryId = GetCountryName( rawOffer.Description );
            base.FillParams( extendedOffer, rawOffer );
        }

        private int GetCountryName( string description )
        {
            if( description == null ) {
                return Constants.UndefinedCountryId;
            }
            
            var m = DefineCountry.Match( description );
            return m.Success
                ? m.Groups[ "country" ].Value switch {
                    "Китае" => DbHelper.GetCountryId( "china" ),
                    "Киате" => DbHelper.GetCountryId( "china" ),
                    "России" => DbHelper.GetCountryId( "russia" ),
                    "Турции" => DbHelper.GetCountryId( "turkey" ),
                    _ => Constants.UndefinedCountryId
                }
                : Constants.UndefinedCountryId;
        }
    }
}