// a.snegovoy@gmail.com

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ImportFromOld.Data;
using ImportFromOld.Data.Entity;

namespace ImportFromOld
{
    internal static class Cash
    {
        public static Dictionary<long, OfferLinks> Values = new();

        private static Dictionary<long,IItemProperty> GetPropertyCash( IEnumerable<IItemProperty> properties )
        {
            var cash = new Dictionary<long, IItemProperty>();
            var entities = properties.ToList();
            foreach( var entity in entities ) {
                cash[ entity.OfferId ] = entity;
            }

            return cash;
        }

        private static Dictionary<long,List<IItemProperty>> GetPropertiesCash( IEnumerable<IItemProperty> properties )
        {
            var cash = new Dictionary<long, List<IItemProperty>>();
            var entities = properties.ToList();
            var grouped = entities.GroupBy( p => p.OfferId );
            foreach( var group in grouped ) {
                cash[ group.Key ] = group.ToList();
            }
            return cash;
        }
        
        public static void FillCash()
        {
            var brandTask = Task.Run( () => GetPropertyCash( new ImportDbContext().Brands ) );
            var countryTask = Task.Run( () => GetPropertyCash( new ImportDbContext().Countries ) );
            var colorsTask = Task.Run( () => GetPropertiesCash( new ImportDbContext().Colors ) );
            var materialsTask = Task.Run( () => GetPropertiesCash( new ImportDbContext().Materials ) );
            var sizeTask = Task.Run( () => GetPropertiesCash( new ImportDbContext().Sizes ) );
            var tagsTask = Task.Run( () => GetPropertiesCash( new ImportDbContext().Tags ) );
            
            Task.WaitAll( brandTask, countryTask, colorsTask, materialsTask, sizeTask, tagsTask );

            var cashBrand = brandTask.Result;
            var cashCountries = countryTask.Result;
            var cashColors = colorsTask.Result;
            var cashMaterials = materialsTask.Result;
            var cashSize = sizeTask.Result;
            var cashTags = tagsTask.Result;

            var ids = new List<long>();
            ids.AddRange( cashBrand.Keys );
            ids.AddRange( cashCountries.Keys );
            ids.AddRange( cashColors.Keys );
            ids.AddRange( cashMaterials.Keys );
            ids.AddRange( cashSize.Keys );
            ids.AddRange( cashTags.Keys );
            var hashedIds = ids.ToHashSet();
            

            foreach( var id in hashedIds ) {
                var links = new OfferLinks( id );
                Values[ id ] = links;
                links.Colors = GetProperties( id, cashColors );
                links.Materials = GetProperties( id, cashMaterials );
                links.Sizes = GetProperties( id, cashSize );
                links.Tags = GetProperties( id, cashTags );
                links.BrandId = GetProperty( id, cashBrand );
                links.CountryId = GetProperty( id, cashCountries );
            }
            
        }


        private static List<string> GetProperties(
            long key,
            IReadOnlyDictionary<long, List<IItemProperty>> dictionary ) =>
            dictionary.ContainsKey( key )
                ? dictionary[ key ].Select( v => v.Value.ToString() ).Distinct().ToList()
                : new List<string>();

        private static string GetProperty(
            long key,
            IReadOnlyDictionary<long, IItemProperty> dictionary ) =>
            dictionary.ContainsKey( key ) ? dictionary[ key ].Value.ToString() : "-1";

    }
}