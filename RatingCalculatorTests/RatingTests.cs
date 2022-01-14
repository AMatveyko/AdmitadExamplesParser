using Common.Api;
using Common.Entities.Rating;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RatingCalculator.Pub;
using RatingCalculator.Workers;
using RatingCalculatorTests.Fakes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheStore.Api.Front.Data.Pub;

namespace RatingCalculatorTests
{
    [TestClass]
    public sealed class RatingTests
    {

        [TestMethod]
        [DataRow("f8a04f25284dac6ce5681bfe5a673841", "1", false, 1010000264, 1010000228, 1010000023)]
        [DataRow("f8a04f25284dac6ce5681bfe5a673841", "16", false, 1110006264, 1110006228, 1110006023)]
        [DataRow("f8a04f25284dac6ce5681bfe5a673841", "38", false, 1111000264, 1111000228, 1111000023)]
        [DataRow("f8a04f25284dac6ce5681bfe5a673841", "39", false, 1012000264, 1012000228, 1012000023)]
        [DataRow("2704087abb1913c8adbf2e9a73a0c0a2", "14", true, 1000005237, 1000005990, 1000005197)]
        [DataRow("2704087abb1913c8adbf2e9a73a0c0a2", "16", true, 1100006237, 1100006990, 1100006197)]
        [DataRow("AnyNotInTheDatabaseId", "16", true, 1100006020, 1100006671, 1100006000)]
        public void RatingTest(string productId, string shopId, bool soldOut, int ctr, int percentCtr, int clicksCtr) {
            var testSet = new List<(CtrHelperType, int)> {
                    ( CtrHelperType.Default, ctr ),
                    ( CtrHelperType.Percent, percentCtr ),
                    ( CtrHelperType.Click, clicksCtr )
                };
            foreach(var ( type, expectCtr) in testSet) {
                var builder = new CtrHelpersBuilder(type);
                AssertRatingTest(productId, shopId, soldOut, expectCtr, builder);
            }
        }

        private void AssertRatingTest(string productId, string shopId, bool soldOut, int ctr, CtrHelpersBuilder builder) {
            var info = new ProductRatingData(productId, shopId, soldOut);
            var calculator = new Calculator(new FakeTheStoreRepository(), builder);
            var ratingInfo = calculator.GetRating(info);

            Assert.AreEqual(ctr, ratingInfo.Rating);
        }

        [TestMethod]
        [DataRow("9b769cf7dfece9ddae39d8b08d04b18c", -73, 195, 245)]
        [DataRow("948c96d4252bb92e68e115a7e2c9634e", 20, 671, 10)]
        [DataRow("f8a04f25284dac6ce5681bfe5a673841", 264, 228, 23)]
        [DataRow("AnyNotInTheDatabaseId", 20, 671, 0)]
        [DataRow("a2cd4609229a177a96289327ff99c486", 234, 0, 0)]
        [DataRow("2704087abb1913c8adbf2e9a73a0c0a2", 237, 990, 197)]
        [DataRow("2a4ba9378b7a86d307a41c87f7e7a86a", 20, 671, 97)]
        public void CtrTests( string productId, int ctr, int percentCtr, int clicksCtr) {

            var repository = new FakeCtrRepository();
            var ctrHelper = new CTRHelper(repository);
            var ctrPercentHelper = new CTRByPercentHelper(repository);
            var ctrHelperByClicks = new CtrHelperByClicks(repository);

            AssertCtr(ctrHelper, ctr, productId);
            AssertCtr(ctrPercentHelper, percentCtr, productId);
            AssertCtr(ctrHelperByClicks, clicksCtr, productId);
        }

        private void AssertCtr(ICtrHelper helper, int ctr, string productId) {

            var actual = helper.GetCtr(productId);
            Assert.AreEqual(ctr, actual);
        }

        [TestMethod]
        public void MaxClicksTest() {
            var repository = new FakeCtrRepositoryForClicks();
            var helper = new CtrHelperByClicks(repository);
            var ctr = helper.GetCtr("ec0aadc4ebf2c8bea920e4a91e4d930e");

            Assert.AreEqual(999, ctr);
        }

    }
}
