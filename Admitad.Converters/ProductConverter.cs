// a.snegovoy@gmail.com

using System;
using System.Collections.Generic;
using System.Linq;

using AdmitadCommon.Entities;
using AdmitadCommon.Helpers;

using AdmitadSqlData.Helpers;

using Common.Workers;

using Newtonsoft.Json;

namespace Admitad.Converters
{
    public sealed class ProductConverter
    {

        private readonly DbHelper _dbHelper;
        private readonly RatingCalculation _calculation;
        
        public ProductConverter( DbHelper dbHelper, RatingCalculation calculation )
        {
            _dbHelper = dbHelper;
            _calculation = calculation;
        }
        
        public List<Product> GetProductsContainer( IEnumerable<Offer> offers )
        {
            var groupedOffers = offers.GroupBy( o => o.ProductId ).ToList();
            var products =
                groupedOffers.Select( g => CollectProduct( g.ToList() ) );
            var filteredProducts = FilterBrokenProducts( products ).ToList();

            return filteredProducts;
        }

        private Product CollectProduct( List<Offer> offers ) {
            if( offers.Any() == false ) {
                throw new Exception( "Offer list is empty" );
            }
            
            var updateDate = offers.Max( o => o.UpdateDate );
            var product = CreateNewProduct( offers, updateDate );
            offers.ForEach( o => MergeOfferIntoProduct( product, o ) );
            product.Param = string.Join( " ; ", product.Params.Distinct() );
            product.JsonParams = GetJsonParams( offers );
            return product;
        }

        private static IEnumerable<Product> FilterBrokenProducts( IEnumerable<Product> products )
        {
            return products.Where( p => p.Price > 0 );
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

        private Product CreateNewProduct(
            List<Offer> offers,
            DateTime updateDate )
        {
            var offer = offers.First();
            var soldOut = offers.All( o => o.SoldOut );
            _dbHelper.RememberVendorIfUnknown( offer.VendorNameClearly, offer.OriginalVendor );
            
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
                Price = offers.Max( o => o.Price ),
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
                Soldout = (byte)( soldOut ? 1 : 0 ),
                Delivery = 0,
                BrandId = _dbHelper.GetBrandId( offer.VendorNameClearly ),
                SalesNotes = offer.SalesNotes,
                OriginalCategoryId = offer.CategoryId,
                OfferIds = offers.Select( o => o.OriginalId ).ToArray(),
                Vendor = offer.OriginalVendor,
                Rating = _calculation.Calculate()
            };
        }
    }
}