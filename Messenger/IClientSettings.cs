// a.snegovoy@gmail.com

namespace Messenger
{
    public interface IClientSettings
    {
        MessengerType Type { get; }
        bool Enabled { get; }
    }
}