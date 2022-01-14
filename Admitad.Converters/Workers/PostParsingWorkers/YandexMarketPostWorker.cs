// a.snegovoy@gmail.com

using System;
using System.Text.RegularExpressions;

using Common.Entities;

namespace Admitad.Converters.Workers
{
    public sealed class YandexMarketPostWorker : IPostWorker
    {

        private static string ParameterDiagonal = "diagonal";
        private static string ParameterTechnology = "technology";
        private static string Model = "model";
        private static string ParameterResolution = "resolution"; 
        
        private static Regex _tvPattern = new Regex(
            @"(Телевизор\s+)?((?<diagonal>\d+)""\s+)?(Телевизор\s+)?(?<vendor>\S+)\s+(?<model>\S+)(?<params>.*)",
            RegexOptions.Compiled );
        private static Regex _yearPattern = new Regex( @"((?<year>\d{4}))", RegexOptions.Compiled );
        private static Regex _resolutionPattern = new Regex( @"(?<resulotion>\d{2,5}(x|X)\d{2,5})", RegexOptions.Compiled );
        
        public void Process(
            Product product )
        {
            var match = _tvPattern.Match( product.Name );
            if( match.Success == false ) {
                return;
            }

            SetPrimaryParameters( product, match );
            ActionIfNotEmpty( value => ParseAndSetParameters( product, value ), match.Groups[ "params" ].Value );
        }

        private void ParseAndSetParameters( Product product, string data )
        {
            var resolutionMatch = _resolutionPattern.Match( data );
            var yearMatch = _yearPattern.Match( data );

            if( resolutionMatch.Success ) {
                product.Params.Add($"resolution={resolutionMatch.Groups["resolution"].Value}");
                data = _resolutionPattern.Replace( data, string.Empty );
            }

            if( yearMatch.Success ) {
                product.Params.Add($"year={yearMatch.Groups["year"].Value}");
                data = _yearPattern.Replace( data, string.Empty );
            }
            
            product.Params.Add($"other={data}");
            
        }
        
        private void SetPrimaryParameters( Product product, Match match ) {
            ActionIfNotEmpty( 
                value => product.Model = value,
                match.Groups[ "model" ].Value);
            ActionIfNotEmpty( 
                value => product.Params.Add($"{ParameterDiagonal}={value}"),
                match.Groups[ "diagonal" ].Value);

        }

        private static void ActionIfNotEmpty( Action<string> action, string value ) {
            if( string.IsNullOrEmpty( value ) ||
                value == string.Empty ) {
                return;
            }

            action( value );
        }

    }
}