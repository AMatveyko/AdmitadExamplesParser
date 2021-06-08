// a.snegovoy@gmail.com

using System.Collections.Generic;

using Common.Helpers;

namespace Common.Entities
{
    public sealed class ShopData
    {

        public ShopData(
            int weight ) =>
            Weight = weight;
        
        public string Name { get; set; }
        public List<RawOffer> Offers { get; set; } = new List<RawOffer>();
        public Dictionary<string, ShopCategory> Categories { get; } = new Dictionary<string, ShopCategory>();
        public bool CategoryLoop { get; private set; }
        public int Weight { get; }

        public void AddCategories( IEnumerable<ShopCategory> categories ) {
            foreach( var category in categories ) {
                if( Categories.ContainsKey( category.Id ) ) {
                    var duplicate = Categories[ category.Id ];
                    LogWriter.Log( $"Category duplicate: in dictionary { duplicate.Id } - { duplicate.Name }; duplicate { category.Id } - { category.Name }" );
                }
                else {
                    Categories[ category.Id ] = category;
                }
            }
        }

        public string GetCategoryPath( string categoryId )
        {
            var path = new List<string>();
            var rootId = categoryId;
            while( rootId != null &&
                   Categories.ContainsKey( rootId ) ) {
                var category = Categories[ rootId ];
                path.Insert( 0, category.Name );
                rootId = category.ParentId;
                if( category.Id == category.ParentId ) {
                    CategoryLoop = true;
                    break;
                }
            }
            return string.Join( " \\ ", path );
        }
    }
}