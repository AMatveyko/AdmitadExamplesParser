using System.IO;
using NUnit.Framework;
using SearchEngineIndexChecking.Parsers;

namespace SearchEngineIndexCheckingTests
{
    public class ParsingTests
    {
        [Test]
        public void YandexHasResultTest() {
            var filePath = @"o:\admitad\tests\parsingSearchEngine\yandexResult.txt";
            var data = File.ReadAllText(filePath);
            var result = YandexParser.IsIndexed(data);
            Assert.AreEqual(true, result);
        }

        [Test]
        public void YandexNotFoundTest() {
            var filePath = @"o:\admitad\tests\parsingSearchEngine\emptyResult.txt";
            var data = File.ReadAllText(filePath);
            var result = YandexParser.IsIndexed(data);
            Assert.AreEqual( false, result );
        }
    }
}