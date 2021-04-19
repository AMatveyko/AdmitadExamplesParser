// a.snegovoy@gmail.com

using System;
using System.Net;

using AdmitadCommon.Entities;
using AdmitadCommon.Helpers;

using RestSharp;

namespace Messenger.Clients
{
    internal class Telegram : IMessenger
    {

        private readonly TelegramSettings _settings;

        private const string _urlTemplate = "https://api.telegram.org/bot{0}/sendMessage";

        public Telegram( IClientSettings settings )
        {
            if( settings is TelegramSettings telegramSettings ) {
                _settings = telegramSettings;
            }
            else {
                throw new ArgumentException( "Wrong settings type for telegram" );
            }
        }

        public void Send(
            string message )
        {
            var client = new RestClient( string.Format( _urlTemplate, _settings.Token ) );
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddParameter(
                "application/x-www-form-urlencoded", 
                $"chat_id={_settings.ChatId}&text={message}",
                ParameterType.RequestBody );
            var response = client.Execute(request);
            if (response.StatusCode != HttpStatusCode.OK) {
                LogWriter.Log( $"Sending a message via telegram { response.StatusCode }" );
            }
        }
    }
}