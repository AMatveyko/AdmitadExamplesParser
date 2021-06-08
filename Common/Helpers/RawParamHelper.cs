// a.snegovoy@gmail.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using Common.Entities;

namespace Common.Helpers
{
    public static class RawParamHelper
    {

        private static Regex _colorPattern = new Regex( "В цвете:(?<colors>[а-яА-Я, ]+)", RegexOptions.Compiled );
        private static Regex _sizePattern = new Regex( @"Размеры: (?<sizes>[\d,]+)", RegexOptions.Compiled );
        
        public static List<RawParam> GetRawParamFromNameAnabel( string name )
        {
            var @params = new List<RawParam>();
            var colorMatch = _colorPattern.Match( name );
            var sizeMatch = _sizePattern.Match( name );

            if( colorMatch.Success ) {
                var colorParams = colorMatch.Groups[ "colors" ].Value.Split(
                    ",",
                    StringSplitOptions.RemoveEmptyEntries ).Select(
                    c => new RawParam {
                        NameFromXml = Constants.Params.ColorName,
                        ValueFromXml = c.Trim()
                    } );
                @params.AddRange( colorParams );
            }

            if( sizeMatch.Success ) {
                var sizeParams = sizeMatch.Groups[ "sizes" ].Value.Split(
                    ",",
                    StringSplitOptions.RemoveEmptyEntries ).Select(
                    s => new RawParam {
                        NameFromXml = Constants.Params.SizeName,
                        ValueFromXml = s.Trim()
                    } );
                @params.AddRange( sizeParams );
            }

            return @params;
        }
        
        
    }
}