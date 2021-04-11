﻿// a.snegovoy@gmail.com

namespace Messenger
{
    public class TelegramSettings : IClientSettings
    {
        public MessengerType Type => MessengerType.Telegram;
        public bool Enabled { get; set; }
        public string Token { get; set; }
        public string ChatId { get; set; }
    }
}