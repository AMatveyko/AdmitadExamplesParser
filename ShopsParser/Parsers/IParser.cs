// a.snegovoy@gmail.com

using Common.Entities;

namespace ShopsParser.Parsers
{
    internal interface IParser
    {
        ( Age, Gender ) Parse(
            string data );
    }
}