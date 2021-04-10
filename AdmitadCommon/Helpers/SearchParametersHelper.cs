// a.snegovoy@gmail.com

using System;
using System.Linq;

using AdmitadCommon.Entities;

namespace AdmitadCommon.Helpers
{
    public static class SearchParametersHelper
    {

        private const string ParamField = "param";
        
        public static Age ConvertAgeFromDb( int fromDb ) =>
            fromDb switch {
                0 => Age.All,
                1 => Age.Adult,
                2 => Age.Child,
                _ => throw new ArgumentException( "Wrong age from db" )
            };

        public static Gender ConvertGenderFromDb( string fromDb ) =>
            fromDb switch {
                "w" => Gender.Woman,
                "m" => Gender.Man,
                "u" => Gender.Unisex,
                "b" => Gender.Boy,
                "g" => Gender.Girl,
                "c" => Gender.Child,
                _ => Gender.Unisex
            };

        public static string[] ConvertFieldsFromDb( string[] fromDb )
        {
            var fields = fromDb.ToList();
            if( fields.Contains( ParamField ) ) {
                fields.Remove( ParamField );
                fields.AddRange( new [] { "mergedColors","mergedSizes","mergedMaterials" } ) ;
            }

            return fields.ToArray();
        }
    }
}