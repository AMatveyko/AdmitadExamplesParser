// a.snegovoy@gmail.com

using System.ComponentModel.DataAnnotations.Schema;

namespace TheStore.Api.Front.Data.Entities
{
    [ Table( "category" ) ]
    public sealed class CategoryDb
    {
        [ Column( "id" ) ]
        public int Id { get; set; }
        [ Column( "id_parent" ) ]
        public int ParentId { get; set; }
        [ Column( "enabled" ) ]
        public bool Enabled { get; set; }
        [ Column( "lvl" ) ]
        public int Level { get; set; }
        [ Column( "name" ) ]
        public string Name { get; set; }
        [ Column( "name_h1" ) ]
        public string H1 { get; set; }
        [ Column( "name_h1_vin" ) ]
        public string H1Vin { get; set; }
        [ Column( "name2" ) ]
        public string Name2 { get; set; }
        [ Column( "name3" ) ]
        public string Name3 { get; set; }
        [ Column( "unit" ) ]
        public string Unit { get; set; }
        [ Column( "name_lat" ) ]
        public string LatinName { get; set; }
        [ Column( "name_lat2" ) ]
        public string LatinName2 { get; set; }
        [ Column( "prefix_type" ) ]
        public int PrefixType { get; set; }
        [ Column( "sex" ) ]
        public string Sex { get; set; }
        [ Column( "img" ) ]
        public string Image { get; set; }
        [ Column( "search" ) ]
        public string Search { get; set; }
        [ Column( "search_specify" ) ]
        public string SearchSpecify { get; set; }
        [ Column( "search_minus" ) ]
        public string SearchMinus { get; set; }
        [ Column( "search_fields" ) ]
        public string SearchFields { get; set; }
        [ Column( "search_minus_fields" ) ]
        public string SearchMinusFields { get; set; }
        [ Column( "search_gender" ) ]
        public string SearchGender { get; set; }
        [ Column( "search_age" ) ]
        public int SearchAge { get; set; }
        [ Column( "cnt" ) ]
        public int Count { get; set; }
        [ Column( "cnt2" ) ]
        public int Count2 { get; set; }
        [ Column( "take_unisex" ) ]
        public bool TageUnisex { get; set; }
    }
}