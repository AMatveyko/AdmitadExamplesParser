// a.snegovoy@gmail.com

using AdmitadCommon.Helpers;


using NUnit.Framework;

namespace AdmitadExamplesParserTests
{
    public sealed class DiscountHelperTests
    {
        [ Test ]
        public void DiscountsIntegerTest() {
            decimal? oldPrice = 1000.00m;
            const decimal price = 800.00m;
            const int actual = 20;
            var discount = DiscountHelper.CalculateDiscount( price, oldPrice );
            Assert.AreEqual( discount, actual );
        }
        
        [ Test ]
        public void DiscountsFractionalTest1() {
            decimal? oldPrice = 1856.00m;
            const decimal price = 1324.00m;
            const int actual = 28;
            var discount = DiscountHelper.CalculateDiscount( price, oldPrice );
            Assert.AreEqual( discount, actual );
        }
        
        [ Test ]
        public void DiscountsFractionalTest2() {
            decimal? oldPrice = 1248.34m;
            const decimal price = 793.65m;
            const int actual = 36;
            var discount = DiscountHelper.CalculateDiscount( price, oldPrice );
            Assert.AreEqual( discount, actual );
        }
        
        [ Test ]
        public void PriceGreaterThanOldPriceTest()
        {
            decimal? oldPrice = 1000.00m;
            const decimal price = 2000.00m;
            const int actual = 0;
            var discount = DiscountHelper.CalculateDiscount( price, oldPrice );
            Assert.AreEqual( discount, actual );
        }
        
        [ Test ]
        public void OldPriceNullTest()
        {
            decimal? oldPrice = null;
            const decimal price = 2000.00m;
            const int actual = 0;
            var discount = DiscountHelper.CalculateDiscount( price, oldPrice );
            Assert.AreEqual( discount, actual );
        }
    }
}