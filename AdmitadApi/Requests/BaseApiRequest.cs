// a.snegovoy@gmail.com

namespace AdmitadApi.Requests
{
    internal abstract class BaseApiRequest : IApiRequest
    {

        private readonly string _url = "https://api.admitad.com/";
        protected abstract string EntryPoint { get; }

        public string Get() => _url + EntryPoint;
    }
}