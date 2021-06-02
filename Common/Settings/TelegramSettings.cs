// a.snegovoy@gmail.com

using Common.Entities;

namespace Common.Settings
{
    public sealed class TelegramSettings : IClientSettings
    {
        public MessengerType Type => MessengerType.Telegram;
        public bool Enabled { get; set; }
        public string Token { get; set; }
        public string ChatId { get; set; }
    }
}