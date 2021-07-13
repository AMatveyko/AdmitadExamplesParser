// a.snegovoy@gmail.com

using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Entities
{
    public sealed class Offer : IIndexedEntities, IExtendedOffer, IBaseOffer
    {
        public string Id { get; set; }
        public string OriginalId { get; set; }
        public string RoutingId { get; }

        public const string IndexNameConst = "offers";
        public DateTime AddDate => UpdateDate;
        public bool SoldOut { get; set; }
        public string IndexName => IndexNameConst;
        public bool Delivery { get; set; }
        public string ProductId { get; set; }
        public string Url { get; set; }
        public DateTime UpdateDate { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> Photos { get; set; }
        public string Model { get; set; }
        public string TypePrefix { get; set; }
        public string MarketCategory { get; set; }
        public string CategoryPath { get; set; }
        public decimal? OldPrice { get; set; }
        public int ShopId { get; set; }
        public decimal Price { get; set; }
        public byte Discount { get; set; }
        public Currency Currency { get; set; }
        public List<Param> Params { get; set; } = new List<Param>();
        //public List<string> AllParams { get; set; } = new();
        public string SalesNotes { get; set; }
        public Gender Gender { get; set; }
        public Age Age { get; set; }
        public string CategoryId { get; set; }
        public int BrandId { get; set; }
        public string VendorNameClearly { get; set; }
        public int CountryId { get; set; }
        public ProductType Type { get; set; }
        public string OriginalVendor { get; set; }
        public AgeRange AgeRange { get; set; }

        public void AddParamIfNeed( RawParam raw ) {
            AddParamIfNeed( raw.Name.ToLower(), raw.Value.ToLower(), raw.Unit?.ToLower() );
        }

        private void AddParamIfNeed(
            string name,
            string value,
            string unit )
        {
            var param = Params.FirstOrDefault( p => p.Name == name );
            if( param == null ) {
                param = new Param( name, unit ?? string.Empty );
                Params.Add( param );
            }

            param.AddValueIfNeed( value );
        }
    }
}