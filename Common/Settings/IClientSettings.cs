// a.snegovoy@gmail.com

using Common.Entities;

namespace Common.Settings
{
    public interface IClientSettings
    {
        MessengerType Type { get; }
        bool Enabled { get; }
    }
}