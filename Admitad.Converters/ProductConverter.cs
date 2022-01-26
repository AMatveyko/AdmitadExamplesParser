// a.snegovoy@gmail.com

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Admitad.Converters.Workers;
using AdmitadSqlData.Helpers;

using Common.Entities;
using Common.Helpers;
using Common.Workers;

using Newtonsoft.Json;

namespace Admitad.Converters
{
    public sealed class ProductConverter
    {

        private readonly DbHelper _dbHelper;
        private readonly ProductRatingCalculation _calculator;
        
        public ProductConverter( DbHelper dbHelper, ProductRatingCalculation calculator ) {
            _dbHelper = dbHelper;
            _calculator = calculator;
        }
        
        public List<Product> GetProducts( IEnumerable<Offer> offers )
        {
            var groupedOffers = offers.GroupBy( o => o.ProductId ).ToList();
            var products =
                groupedOffers.AsParallel().Select( g => CollectProduct( g.ToList() ) );
            var filteredProducts = FilterBrokenProducts( products ).ToList();

            return filteredProducts;
        }

        public static void MergePartInToProduct( Product product, ProductPart part )
        {
            var parameters = JsonConvert.DeserializeObject<List<Param>>( product.JsonParams ) ?? new List<Param>();
            var moreParameters = JsonConvert.DeserializeObject<List<Param>>( part.JsonParams ) ?? new List<Param>();

            var mergedParameters = MergeParameters( parameters, moreParameters );
            
            product.JsonParams = GetJsonParams( mergedParameters );
            product.Params = GetStringParams( mergedParameters );
            product.Param = GetDistinctProductParam( product.Params );
            product.OfferIds = product.OfferIds.ToList().Concat( part.OfferIds ).Distinct().ToArray();

        }

        private static List<Param> MergeParameters( List<Param> product, IReadOnlyCollection<Param> part )
        {
            foreach( var partParameter in part ) {
                if( product.Count( p => p.Name == partParameter.Name ) > 0 ) {
                    var param = product.First( p => p.Name == partParameter.Name );
                    partParameter.Values.ForEach( param.AddValueIfNeed );
                }
                else {
                    product.Add( partParameter );
                }
            }

            return product;
        }
        
        private Product CollectProduct( List<Offer> offers ) {
            if( offers.Any() == false ) {
                throw new Exception( "Offer list is empty" );
            }
            
            var updateDate = offers.Max( o => o.UpdateDate );
            var product = CreateNewProduct( offers, updateDate );
            offers.ForEach( o => MergeOfferIntoProduct( product, o ) );
            product.Param = GetDistinctProductParam( product.Params );
            product.JsonParams = GetJsonParams( offers );
            return product;
        }

        private static string GetDistinctProductParam( List<string> parameters ) => string.Join( " ; ", parameters.Distinct() ); 
        
        private static IEnumerable<Product> FilterBrokenProducts( IEnumerable<Product> products )
        {
            return products.Where( p => p.Price > 0 );
        }

        private static string GetJsonParams( IEnumerable<Offer> offers )
        {
            var parameters = offers.SelectMany( o => o.Params );
            return GetJsonParams( parameters );
        }

        private static string GetJsonParams( IEnumerable<Param> parameters )
        {
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
            product.Params.AddRange( GetStringParams( offer.Params ) );
        }

        private static List<string> GetStringParams( IEnumerable<Param> parameters ) =>
            parameters.SelectMany( p => p.Values ).ToList(); 
        
        private Product CreateNewProduct(
            List<Offer> offers,
            DateTime updateDate )
        {
            var offer = offers.First();
            var soldOut = offers.All( o => o.SoldOut );
            var ageRange = AgeRange.GetMaxRange( offers.Select( o => o.AgeRange ).ToList() );
            
            // костыль для перехода на новую схему возраста и пола
            var gender = offer.Gender switch {
                Gender.Boy => Gender.Man,
                Gender.Girl => Gender.Woman,
                _ => offer.Gender
            };
            
            _dbHelper.RememberVendorIfUnknown( offer.VendorNameClearly, offer.OriginalVendor );

            var product = new Product {
                Id = offer.ProductId,
                Url = offer.Url,
                UpdateDate = updateDate,
                Name = offer.Name ?? string.Empty,
                Description = offer.Description ?? string.Empty,
                Model = offer.Model ?? string.Empty,
                Gender = GenderHelper.Convert( gender ),
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
                Type = offer.Type.ToString(),
                AgeFromIntFact = ( int? ) ageRange?.From,
                AgeFromTxt = ageRange?.From.ToString( CultureInfo.InvariantCulture ),
                AgeToIntFact = ( int? ) ageRange?.To,
                AgeToTxt = ageRange?.To.ToString( CultureInfo.InvariantCulture )
            };

            CalculateAndSetRating(product);

            return product;
        }

        private void CalculateAndSetRating(Product product) => _calculator.SetRating(product, false);
    }
}