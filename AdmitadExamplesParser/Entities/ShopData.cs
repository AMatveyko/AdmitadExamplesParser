// a.snegovoy@gmail.com

using System;
using System.Collections.Generic;
using System.Linq;

using AdmitadCommon.Entities;
using AdmitadCommon.Helpers;

namespace AdmitadExamplesParser.Entities
{
    public sealed class ShopData {
        public string Name { get; set; }
        public List<RawOffer> Offers { get; set; } = new();
        public Dictionary<string, ShopCategory> Categories { get; } = new ();
        public bool CategoryLoop { get; private set; }

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
            return string.Join( "\\", path );
        }

    }
}