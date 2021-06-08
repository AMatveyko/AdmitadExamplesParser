// a.snegovoy@gmail.com

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using NLog;

using TheStore.Api.Core.Sources.Entities;

using Web.Common.Entities;

namespace TheStore.Api.Core.Sources.Helpers
{
    internal sealed class UrlHelper
    {

        private const string OldSite = "https://thestore.ru/";
        private const string NewSite = "https://thestore.matveyko.su/";
        private const string UrlTemplate = "{0}{1}";

        private const string Flag = "/dlya/";
        private const string Flag2 =  "/brand-";

        private List<string> _brokenLines = new ();
        private List<string> _notSuitable = new ();

        public IEnumerable<string> GetBroken()
        {
            return new List<string> {
                "Broken:"
            }.Concat( _brokenLines ).Concat(
                new List<string> {
                    "Not suitable:"
                } ).Concat( _notSuitable );
        }

        public List<UrlInfo> GetInfos()
        {
            
            var settings = new RequestSettings("https://thestore.ru", LogManager.GetLogger( "ErrorLogger" ) );
            var request = new PageStatisticsRequest( settings );

            var response = request.Execute(); 
            var strings = response.Lines;
            return strings.Select( GetUrlTuple )
                .Select( Convert )
                .Where( i => i != null )
                .ToList();
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
            if( str.Contains( Flag ) == false && str.Contains( Flag2 ) == false ) {
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