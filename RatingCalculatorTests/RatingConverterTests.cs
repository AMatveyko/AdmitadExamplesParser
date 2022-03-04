using Common.Entities.Rating;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RatingCalculator.Common;
using RatingCalculator.Workers;

namespace RatingCalculatorTests
{
    [TestClass]
    public class RatingConverterTests
    {
        [TestMethod]
        public void OpenProgrammTest() {
            const int expectedRating = 1110109050;
            var data = new RatingCalculator.Common.ProductRatingWorkData("", true) {
                CTR = 50,
                ECPCRating = 09,
                ShopInternalRating = 01,
                IsOpenProgramm = true
            };

            var info = RatingConverter.Convert(data);

            Compare(expectedRating, info);
        }

        [TestMethod]
        public void CloseProgrammTest() {
            const int expectedRating = 1010109050;
            var data = new RatingCalculator.Common.ProductRatingWorkData("", true)
            {
                CTR = 50,
                ECPCRating = 09,
                ShopInternalRating = 01,
                IsOpenProgramm = false
            };

            var info = RatingConverter.Convert(data);

            Compare(expectedRating, info);
        }

        [TestMethod]
        public void OutOfStockProduct() {
            const int expectedRating = 1100109050;
            var data = new RatingCalculator.Common.ProductRatingWorkData("", false) {
                CTR = 50,
                ECPCRating = 09,
                ShopInternalRating = 01,
                IsOpenProgramm = true
            };

            var info = RatingConverter.Convert(data);

            Compare(expectedRating, info);
        }

        [TestMethod]
        public void CloseProgramOutOfStockTest() {
            const int expectedRating = 1000109070;
            var data = new RatingCalculator.Common.ProductRatingWorkData("", false)
            {
                CTR = 70,
                ECPCRating = 09,
                ShopInternalRating = 01,
                IsOpenProgramm = false
            };

            var info = RatingConverter.Convert(data);

            Compare(expectedRating, info);
        }

        public void Compare( int expectRating, ProductRating actualInfo) {
            Assert.AreEqual(expectRating, actualInfo.Rating);
        }
    }
}
