﻿// a.snegovoy@gmail.com

using System.ComponentModel.DataAnnotations.Schema;

namespace AdmitadSqlData.Entities
{
    [ Table( "country" ) ]
    public class Country {
        [ Column( "id" ) ]
        public int Id { get; set; }
        [ Column( "name" ) ]
        public string Name { get; set; }
        [ Column( "name2" ) ]
        public string From { get; set; }
        [ Column( "name_sin" ) ]
        public string Synonym { get; set; }
        [ Column( "name_lat" ) ]
        public string LatinName { get; set; }
    }
}