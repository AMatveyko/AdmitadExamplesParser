// a.snegovoy@gmail.com

using System.Collections.Generic;

using AdmitadCommon.Entities;

namespace AdmitadCommon.Helpers
{
    public static class CurrencyHelper
    {
        private static Dictionary<string, Currency> _cache = new();

        public static Currency GetCurrency(
            string input )
        {
            return input.ToLower() switch {
                "rub" => Currency.RUB,
                "rur" => Currency.RUB,
                "usd" => Currency.USD,
                _ => Currency.Undefined
            };
        }
    }
}