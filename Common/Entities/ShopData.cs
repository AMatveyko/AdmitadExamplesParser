// a.snegovoy@gmail.com

using System.Collections.Generic;

using Common.Helpers;

namespace Common.Entities
{
    public sealed class ShopData : IShopDataWithNewOffers, IShopDataWithDeletedOffers
    {

        public ShopData( int weight ) =>
            Weight = weight;
        
        public string Name { get; set; }
        public List<RawOffer> NewOffers { get; } = new List<RawOffer>();
        public List<RawOffer> DeletedOffers { get; } = new List<RawOffer>();

        public Dictionary<string, ShopCategory> Categories { get; } = new Dictionary<string, ShopCategory>();
        private bool CategoryLoop { get; set; }
        private int Weight { get; }

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

        public List<ShopCategory> GetCategories( string categoryId )
        {
            var categories = new List<ShopCategory>();
            var rootId = categoryId;
            while( rootId != null && Categories.ContainsKey( rootId ) ) {
                var category = Categories[ rootId ];
                categories.Insert( 0, category );
                rootId = category.ParentId;
                if( category.Id == category.ParentId ) {
                    CategoryLoop = true;
                    break;
                }
            }

            return categories;
        }

        public void InsertOffers( ShopData shopData ) {
            NewOffers.AddRange( shopData.NewOffers);
            DeletedOffers.AddRange( shopData.DeletedOffers);
        }
        
    }
}