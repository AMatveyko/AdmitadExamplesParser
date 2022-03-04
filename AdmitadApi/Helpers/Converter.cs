// a.snegovoy@gmail.com

using System;
using System.Collections.Generic;
using System.Linq;

using AdmitadApi.Entities;

namespace AdmitadApi.Helpers
{
    internal static class Converter
    {
        public static WebSite Convert(
            WebSiteResponse response ) =>
            new() {
                Id = response.Id,
                Name = response.Name,
                IsActive = IsActive( response.Status )
            };

        public static Compaign Convert( CompaignResponse response ) =>
            new() {
                Id = response.Id,
                Name = response.Name,
                NameAliases = response.NameAliases,
                Currency = response.Currency,
                EPC = response.EPC,
                ECPC = response.ECPC,
                Feeds = response.Feeds.Select( Convert ).ToList(),
                IsActive = IsActive( response.Status ),
                IsConnected = IsActive( response.ConnectionStatus ),
                ModifiedDate = response.ModifiedDate
            };
        
        public static AdmitadFeedInfo Convert(
            AdmitadFeedInfoResponse response ) =>
            new() {
                Name = response.Name,
                XmlFeed = response.XmlLink,
                LastUpdate = new List<DateTime>() {
                    DateTime.Parse( response.AdmitadLastUpdate.Replace( "+00:00", string.Empty ) ),
                    DateTime.Parse( response.AdvertiserLastUpdate.Replace( "+00:00", string.Empty ) )
                }.Max()
            };

        private static bool IsActive( string data ) => data == "active";

    }
}