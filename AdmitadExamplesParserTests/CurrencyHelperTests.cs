// a.snegovoy@gmail.com

using System;

using AdmitadCommon.Entities;
using AdmitadCommon.Helpers;

using NUnit.Framework;

namespace AdmitadExamplesParserTests
{
    public sealed class CurrencyHelperTests
    {

        [ Test ]
        public void ConvertDecimalToInt()
        {
            decimal var1 = 1444.43m;
            decimal var2 = 13453m;
            var sdf = Convert.ToInt32( var1 );
            var dfg = Convert.ToInt32( var2 );
            Console.WriteLine( sdf );
            Console.WriteLine( dfg );
        }
        
        [ Test ]
        public void RubTest()
        {
            const Currency actual = Currency.RUB;
            var rub1 = CurrencyHelper.GetCurrency( "RUB" );
            var rub2 = CurrencyHelper.GetCurrency( "Rub" );
            var rub3 = CurrencyHelper.GetCurrency( "rub" );
            var rub4 = CurrencyHelper.GetCurrency( "RUR" );

            Assert.AreEqual( rub1, actual );
            Assert.AreEqual( rub2, actual );
            Assert.AreEqual( rub3, actual );
            Assert.AreEqual( rub4, actual );
        }
        
        [ Test ]
        public void UsdTest()
        {
            const Currency actual = Currency.USD;
            var usd1 = CurrencyHelper.GetCurrency( "USD" );
            Assert.AreEqual( usd1, actual );
            var usd2 = CurrencyHelper.GetCurrency( "Usd" );
            Assert.AreEqual( usd2, actual );
            var usd3 = CurrencyHelper.GetCurrency( "usd" );
            Assert.AreEqual( usd3, actual );
        }
        
        [ Test ]
        public void UndefinedTest()
        {
            var undefined = CurrencyHelper.GetCurrency( "sdfsdfsdf" );
            Assert.AreEqual( undefined, Currency.Undefined );
        }
    }
}