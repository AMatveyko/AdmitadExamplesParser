// a.snegovoy@gmail.com

using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheStore.Api.Front.Data.Entities
{
    [ Table( "tag" ) ]
    public sealed class TagDb : ICloneable
    {
        [ Column( "id" ) ]
        public int Id { get; set; }
        [ Column( "date_add" ) ]
        public int AddDate { get; set; }
        [ Column( "date_update" ) ]
        public int UpdateDate { get; set; }
        [ Column( "name" ) ]
        public string Name { get; set; }
        [ Column( "exclude_phrase" ) ]
        public string Exclude { get; set; }
        [ Column( "name2" ) ]
        public string Name2 { get; set; }
        [ Column( "name_menu" ) ]
        public string Menu { get; set; }
        [ Column( "name_h1" ) ]
        public string H1 { get; set; }
        [ Column( "name_title" ) ]
        public string Title { get; set; }
        [ Column( "name_lat" ) ]
        public string LatinName { get; set; }
        [ Column( "name_lat2" ) ]
        public string LatinName2 { get; set; }
        [ Column( "pol" ) ]
        public string Sex { get; set; }
        [ Column( "id_category" ) ]
        public int CategoryId { get; set; }
        [ Column( "search_fields" ) ]
        public string SearchFields { get; set; }
        [ Column( "enabled" ) ]
        public bool Enabled { get; set; }
        [ Column( "important" ) ]
        public bool Important { get; set; }
        [ Column( "name_specify" ) ]
        public string Specify { get; set; }
        
        
        public override bool Equals( object obj )
        {
            var item = obj as TagDb;

            if (item == null)
            {
                return false;
            }

            return LatinName.Equals( item.LatinName );
        }

        public override int GetHashCode() => LatinName.GetHashCode();

        public object Clone() =>
            new TagDb {
                AddDate = int.Parse( DateTime.Now.ToString( "yyyyMMdd" ) ),
                Name = Name,
                Exclude = Exclude,
                Name2 = Name2,
                Menu = Menu,
                H1 = H1,
                Title = Title,
                LatinName = LatinName,
                LatinName2 = LatinName2,
                Sex = Sex,
                CategoryId = CategoryId,
                SearchFields = SearchFields,
                Enabled = Enabled,
                Important = Important,
                Specify = Specify
            };
    }
}