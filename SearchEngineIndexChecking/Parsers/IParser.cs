using System.Collections.Generic;

namespace SearchEngineIndexChecking.Parsers
{
    internal interface IParser
    {
        List<string> GetUrls(string data);
    }
}