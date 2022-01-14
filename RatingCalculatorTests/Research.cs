using Common.Api;
using Common.Entities.Rating;
using Common.Workers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RatingCalculator.Pub;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheStore.Api.Front.Data.Repositories;

namespace RatingCalculatorTests
{
    [TestClass]
    public sealed class Research
    {
        [TestMethod]
        public void ResearchTest() {
            var repository = new TheStoreRepository(SettingsBuilder.GetDbSettings());
            var builder = new CtrHelpersBuilder(CtrHelperType.Click);
            var calculator = new Calculator(repository, builder);
            var info = new ProductRatingData("bfb67ba1dd2628d8589e64dbd07c3fbb", "33", false);
            var rating = calculator.GetRating(info);
        }
    }
}
