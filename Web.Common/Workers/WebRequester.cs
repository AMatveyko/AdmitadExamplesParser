﻿// a.snegovoy@gmail.com

using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using Web.Common.Entities;
using Web.Common.Helpers;

namespace Web.Common.Workers
{
    public static class WebRequester
    {
        public static async Task<byte[]> Request(
            string url,
            ProxyInfo proxyInfo )
        {
            var uri = new Uri( url );
            using var client = proxyInfo != null ? GetClientWithProxy( proxyInfo ) : new HttpClient();
            client.DefaultRequestHeaders.Add( "User-Agent", UserAgents.Get() );
            return await client.GetByteArrayAsync( uri );
        }

        private static HttpClient GetClientWithProxy(
            ProxyInfo proxyInfo )
        {
            var httpClientHandler = new HttpClientHandler {
                Proxy = new WebProxy {
                    Address = new Uri( proxyInfo.Url ),
                    BypassProxyOnLocal = false,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential( proxyInfo.UserName, proxyInfo.Password )
                }
            };

            return new HttpClient( httpClientHandler );
        }
    }
}