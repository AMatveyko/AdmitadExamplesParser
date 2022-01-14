using Microsoft.VisualStudio.TestTools.UnitTesting;
using RatingCalculator.Workers;
using RatingCalculatorTests.Fakes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RatingCalculatorTests
{
    [TestClass]
    public class ShopsHelperTests
    {

        private ShopRatingHelper _helper = new ShopRatingHelper(new FakeShopRepository());

        [TestMethod]
        [DataRow(0, "1")]
        [DataRow(0, "2")]
        [DataRow(0, "3")]
        [DataRow(1, "4")]
        [DataRow(1, "5")]
        [DataRow(2, "6")]
        [DataRow(2, "7")]
        [DataRow(2, "8")]
        [DataRow(3, "9")]
        [DataRow(3, "10")]
        [DataRow(4, "11")]
        [DataRow(4, "12")]
        [DataRow(4, "13")]
        [DataRow(5, "14")]
        [DataRow(5, "15")]
        [DataRow(6, "16")]
        [DataRow(6, "17")]
        [DataRow(7, "18")]
        [DataRow(7, "19")]
        [DataRow(7, "20")]
        [DataRow(8, "21")]
        [DataRow(8, "22")]
        [DataRow(9, "23")]
        [DataRow(9, "24")]
        [DataRow(8, "33")]
        [DataRow(0, "41")]
        [DataRow(5, "27")]
        public void EcpcLevel0Test( int expect, string shopId) {

            var actual = _helper.GetEcpc(shopId);
            Assert.AreEqual(expect, actual);

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void NotFoundShopTest() {
            _helper.GetEcpc("200");
        }

        [TestMethod]
        public void EnabledProgramTest() {
            var actual = _helper.IsOpeningProgramm("38");
            Assert.AreEqual(true, actual);
        }

        [TestMethod]
        public void DisabledProgramTest() {
            var actual = _helper.IsOpeningProgramm("39");
            Assert.AreEqual(false, actual);
        }

        [TestMethod]
        public void WeightTest() {
            var actual10 = _helper.GetInternalRating("38");
            var actual20 = _helper.GetInternalRating("39");
            Assert.AreEqual(10, actual10);
            Assert.AreEqual(20, actual20);
        }
    }
}
