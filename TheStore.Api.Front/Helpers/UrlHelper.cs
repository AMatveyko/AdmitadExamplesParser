// a.snegovoy@gmail.com

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using TheStore.Api.Front.Entity;

namespace TheStore.Api.Front.Helpers
{
    internal class UrlHelper
    {

        //private const string PathToCsv = "/var/www/thestore/www/utils_v2/top_listings_full.csv";
        private const string PathToCsv = @"o:\AdmitadExamplesParser\TheStore.Api.Front\bin\Debug\netcoreapp3.1\top_listings_full.csv";
        
        private const string OldSite = "https://thestore.ru/";
        private const string NewSite = "https://thestore.matveyko.su/";
        private const string UrlTemplate = "{0}{1}";

        private const string Flag = "dlya";

        private List<string> _brokenLines = new List<string>();
        private List<string> _notSuitable = new List<string>();

        public IEnumerable<string> GetBroken()
        {
            return new List<string> {
                "Broken:"
            }.Concat( _brokenLines ).Concat(
                new List<string> {
                    "Not suitable:"
                } ).Concat( _notSuitable );
        }
        
        public List<UrlInfo> GetInfos( int? visits )
        {
            if( File.Exists( PathToCsv ) == false ) {
                throw new FileNotFoundException( PathToCsv );
            }

            var strings = File.ReadAllLines( PathToCsv );
            return strings.Select( GetUrlTuple )
                .Select( Convert )
                .Where( i => i != null )
                .Where( i => visits == null || i.Visits >= visits.Value ).ToList();
        }

        private static UrlInfo Convert( (string, string )? urlTuple )
        {
            if( urlTuple == null ) {
                return null;
            }

            var uri = urlTuple.Value.Item1.Replace( OldSite, string.Empty ).Replace( NewSite, string.Empty );
            int.TryParse( urlTuple.Value.Item2, out var visits );
            return new UrlInfo(
                visits,
                String.Format( UrlTemplate, OldSite, uri ),
                String.Format( UrlTemplate, NewSite, uri ) );
        }

        private ( string, string )? GetUrlTuple( string str )
        {
            if( str.Contains( Flag ) == false ) {
                _notSuitable.Add( str );
                return null;
            }
            
            var infoPath = str.Split( "," );
            
            if( infoPath.Length != 2 ) {
                _brokenLines.Add( str );
                return null;
            }

            for( var i = 0; i < 2; i++ ) {
                infoPath[ i ] = infoPath[ i ].Trim('"');
            }

            return ( infoPath[ 0 ], infoPath[ 1 ] );
        }
        
    }
}