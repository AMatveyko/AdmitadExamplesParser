using System;
using System.Linq;
using HtmlAgilityPack;

namespace SearchEngineIndexChecking.Parsers
{
    internal static class YandexParser
    {
        public static bool IsIndexed(string data) {
            var document = new HtmlDocument();
            document.LoadHtml(data);
            if (IsUrlNotFound(document)) {
                return false;
            }

            return IsIndexed(document);
        }

        private static bool IsUrlNotFound(HtmlDocument document) {
            var element = document.DocumentNode.Descendants().FirstOrDefault(n => n.HasClass("misspell__message") && n.InnerText == "По вашему запросу ничего не нашлось");
            return element != null;
        }

        private static bool IsIndexed(HtmlDocument document) {
            var element = document.DocumentNode.Descendants()
                .FirstOrDefault(n => n.HasClass("serp-item") && n.HasClass("desktop-card"));
            if (element == null) {
                throw new ArgumentException("wrong data");
            }

            return true;
        }
    }
}