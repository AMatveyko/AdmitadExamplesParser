﻿// a.snegovoy@gmail.com

using System;
using System.Collections.Generic;
using System.Linq;

using AdmitadCommon.Entities;
using AdmitadCommon.Helpers;

using AdmitadSqlData.Helpers;

using Newtonsoft.Json;

namespace Admitad.Converters
{
    public static class ProductConverter
    {

        public static List<Product> GetProductsContainer( List<Offer> offers )
        {
            var groupedOffers = offers.GroupBy( o => o.ProductId ).ToList();
            var products =
                groupedOffers.Select( g => CollectProduct( g.ToList() ) ).ToList();

            return products;
        }

        public static Product CollectProduct( List<Offer> offers ) {
            if( offers.Any() == false ) {
                throw new Exception( "Offer list is empty" );
            }
            
            var updateDate = offers.Max( o => o.UpdateDate );
            var product = CreateNewProduct( offers.First(), updateDate );
            offers.ForEach( o => MergeOfferIntoProduct( product, o ) );
            product.Param = string.Join( " ; ", product.Params.Distinct() );
            product.JsonParams = GetJsonParams( offers );
            return product;
        }

        private static string GetJsonParams( List<Offer> offers )
        {
            var parameters = offers.SelectMany( o => o.Params ).ToList();
            var paramsDictionary = new Dictionary< string, Param >();
            foreach( var parameter in parameters ) {
                if( paramsDictionary.ContainsKey( parameter.Name ) == false ) {
                    var param = new Param( parameter.Name, parameter.Unit );
                    param.Values.AddRange( parameter.Values );
                    paramsDictionary[ parameter.Name ] = param;
                    continue;
                }

                foreach( var value in parameter.Values ) {
                    paramsDictionary[ parameter.Name ].AddValueIfNeed( value );
                }
            }

            var mergedParams = paramsDictionary.Select( pd => pd.Value ).ToList();
            return JsonConvert.SerializeObject( mergedParams );
        }
        
        private static void MergeOfferIntoProduct( Product product, Offer offer ) {
            product.Photos.AddRange( offer.Photos.Where( p => product.Photos.Contains( p ) == false ) );
            product.Params.AddRange( offer.Params.SelectMany( p => p.Values ) );
        }

        private static Product CreateNewProduct(
            Offer offer,
            DateTime updateDate )
        {
            return new Product {
                Id = offer.ProductId,
                Url = offer.Url,
                UpdateDate = updateDate,
                Name = offer.Name ?? string.Empty,
                Description = offer.Description ?? string.Empty,
                Model = offer.Model ?? string.Empty,
                Gender = GenderHelper.Convert( offer.Gender ),
                Age = AgeHelper.Convert( offer.Age ),
                ShopId = offer.ShopId.ToString(),
                Price = offer.Price,
                OldPrice = offer.OldPrice ?? 0m,
                TypePrefix = offer.TypePrefix ?? string.Empty,
                CategoryName = offer.CategoryPath ?? string.Empty,
                Discount = offer.Discount,
                Currency = offer.Currency.ToString(),
                CountryId = offer.CountryId.ToString(),
                VendorNameClearly = offer.VendorNameClearly,
                Photos = new List<string>(),
                Params = new List<string>(),
                Enable = 1,
                Soldout = 0,
                Delivery = 0,
                BrandId = DbHelper.GetBrandId( offer.VendorNameClearly ),
                SalesNotes = offer.SalesNotes
            };
        }
    }
}