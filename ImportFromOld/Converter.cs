// a.snegovoy@gmail.com

using System;
using System.Collections.Generic;
using System.Linq;

using Common.Entities;
using Common.Helpers;

using ImportFromOld.Data.Entity;

using static System.Boolean;

namespace ImportFromOld
{
    internal static class Converter {

        public static List<Product> GetProducts(
            IEnumerable<OfferOld> offers,
            Dictionary<long, OfferLinks> cash )
        {
            return offers.GroupBy( o => o.Photo1 )
                .Select( g => GetProduct( g.ToList(), cash ) ).ToList();
        }
        
        private static Product GetProduct( List<OfferOld> offers, Dictionary<long,OfferLinks> cash )
        {
            var offer = offers.First();

            var offerIds = offers.Select( o => o.Id ).ToList();

            var cached = offerIds.Where( cash.ContainsKey ).Select( id => cash[id] ).ToList();
            
            var photos = offers.SelectMany(
                o => new List<string> {
                    o.Photo1,
                    o.Photo2,
                    o.Photo3,
                    o.Photo4,
                    o.Photo5,
                    o.Photo6,
                    o.Photo7,
                    o.Photo8,
                    o.Photo9
                } ).Where( p => string.IsNullOrWhiteSpace(p) == false ).Distinct().ToList();
            var firstPhoto = offers.FirstOrDefault( o => string.IsNullOrWhiteSpace( o.Photo1 ) == false )?.Photo1;
            TryParse( offers.FirstOrDefault()?.Delivery, out var delivery );

            var product = new Product {
                View = offers.Sum( o => o.View ),
                Categories = offers.Select( o => o.CategoryId.ToString() ).Distinct().ToArray(),
                OriginalCategoryId = offers.FirstOrDefault( o => string.IsNullOrWhiteSpace( o.OriginalCategoryId ) == false )?.OriginalCategoryId,
                Tags =  GetPropertyArray( c => c.Tags, cached ),
                Colors = GetPropertyArray( c => c.Colors, cached ),
                Materials = GetPropertyArray( c => c.Materials, cached ),
                Sizes = GetPropertyArray( c => c.Sizes, cached ),
                Soldout = 1,
                Enable = 1,
                Click = offers.Sum( o => o.Click ),
                Id = HashHelper.GetMd5Hash( firstPhoto ?? "-1" ),
                Url = offers.FirstOrDefault( o => string.IsNullOrWhiteSpace( o.Url ) == false )?.Url ?? "-1",
                Delivery = delivery ? (byte)1 : (byte)0,
                UpdateDate = DateTime.Parse( "2020-08-09" ),
                Name = offer.Name,
                Model = offer.Model,
                TypePrefix = offer.TypePrefix,
                CategoryName = offer.CategoryName,
                Description = offer.Description,
                Photos = photos,
                Param = offer.ParamJson,
                JsonParams = offer.ParamJson,
                Gender = offer.Gender ?? "n",
                Age = offer.Adult == 1 ? "a" : "c",
                ShopId = offer.ShopId.ToString(),
                BrandId = GetProperty( c => c.BrandId, cached ),
                VendorNameClearly = offer.ClearlyVendorName,
                CountryId = GetProperty( c => c.CountryId, cached ),
                Price = offer.Price ?? 0,
                OldPrice = offer.OldPrice ?? 0,
                Discount = (short)(offer.Discount ?? 0),
                Currency = offer.CurrencyId,
                SalesNotes = offer.SalesNotes
            };

            var offersAllIds = offers.Select( o => o.Id.ToString() ).ToList();
            offersAllIds.AddRange( offers.Select( o => o.OfferId ) );

            product.OfferIds = offersAllIds.ToArray();
            
            return product;
        }
        
        private static string[] GetPropertyArray(
            Func<OfferLinks, List<string>> func,
            List<OfferLinks> list ) =>
            list.SelectMany( func ).Distinct().ToArray();

        private static string GetProperty(
            Func<OfferLinks, string> func,
            List<OfferLinks> list ) =>
            list.Select( func ).FirstOrDefault() ?? "-1";

    }
}