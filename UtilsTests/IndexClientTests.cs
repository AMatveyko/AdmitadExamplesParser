using Admitad.Converters.Workers;
using Common.Api;
using Common.Elastic.Workers;
using Common.Workers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheStore.Api.Front.Data.Repositories;

namespace UtilsTests
{
    [TestClass]
    public sealed class IndexClientTests
    {
        [TestMethod]
        public void ScrolingProductsForUpdateRatingTest() {
            var repository = new TheStoreRepository(SettingsBuilder.GetDbSettings());
            var builder = new SettingsBuilder(repository);
            var elasticSettings = builder.GetSettings().ElasticSearchClientSettings;
            var client = IndexClient.CreateIndexClient(elasticSettings,new BackgroundBaseContext("id","name"));
            var result = client.GetProductsForUpdatingRating();

           var settings = new SettingsBuilder(repository).GetSettings();

            var calculation = new ProductRatingCalculation(repository, settings.CtrCalculationType);

            var result2 = calculation.GetRatingUpdateDatas(result);

            client.UpdateProductRating(result2);

        }
    }
}
