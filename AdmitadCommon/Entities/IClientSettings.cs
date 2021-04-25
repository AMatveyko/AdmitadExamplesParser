// a.snegovoy@gmail.com

namespace AdmitadCommon.Entities
{
    public interface IClientSettings
    {
        MessengerType Type { get; }
        bool Enabled { get; }
    }
}