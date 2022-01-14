using Admitad.Converters.Workers;
using Common.Api;
using Common.Elastic.Workers;
using Common.Settings;

namespace TheStore.Api.Core.Sources.Workers
{
    internal sealed class RatingsCalculationWorker
    {
        private readonly ProductRatingCalculation _productRatingCalculation;
        private readonly ProcessorSettings _settings;

        public RatingsCalculationWorker(ProductRatingCalculation productRatingCalculation, ProcessorSettings settings) =>
            ( _productRatingCalculation, _settings ) = (productRatingCalculation, settings);

        public void Calculate(RatingCalculationContext context) {
            _productRatingCalculation.ChangeContext(context);
            context.AddMessage("Start of calculations");
            context.AddMessage($"Type \"{_settings.CtrCalculationType}\"");
            var indexClient = IndexClient.CreateIndexClient(_settings.ElasticSearchClientSettings, context);
            var infos = indexClient.GetProductsForUpdatingRating();
            context.AddMessage($"Calculated ratings {infos.Count}");
            var datas = _productRatingCalculation.GetRatingUpdateDatas(infos);
            context.AddMessage($"Changed ratings {datas.Count}");
            indexClient.UpdateProductRating(datas);
            context.Content = "Calculations completed";
        }
    }
}
