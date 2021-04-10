// a.snegovoy@gmail.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using AdmitadCommon.Entities;

namespace AdmitadCommon.Helpers
{
    public static class RawParamHelper
    {

        private static Regex _colorPattern = new( "В цвете:(?<colors>[а-яА-Я, ]+)", RegexOptions.Compiled );
        private static Regex _sizePattern = new( @"Размеры: (?<sizes>[\d,]+)", RegexOptions.Compiled );
        
        public static List<RawParam> GetRawParamFromNameAnabel( string name )
        {
            var @params = new List<RawParam>();
            var colorMatch = _colorPattern.Match( name );
            var sizeMatch = _sizePattern.Match( name );

            if( colorMatch.Success ) {
                var colorParams = colorMatch.Groups[ "colors" ].Value.Split(
                    ",",
                    StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries ).Select(
                    c => new RawParam {
                        NameFromXml = Constants.Params.ColorName,
                        ValueFromXml = c
                    } );
                @params.AddRange( colorParams );
            }

            if( sizeMatch.Success ) {
                var sizeParams = sizeMatch.Groups[ "sizes" ].Value.Split(
                    ",",
                    StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries ).Select(
                    s => new RawParam {
                        NameFromXml = Constants.Params.SizeName,
                        ValueFromXml = s
                    } );
                @params.AddRange( sizeParams );
            }

            return @params;
        }
        
        
    }
}