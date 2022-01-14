using Common.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using TheStore.Api.Front.Data.Entities;
using TheStore.Api.Front.Data.Pub;

namespace RatingCalculator.Workers
{
    internal sealed class CTRHelper : BaseCTRHelper
    {

        private const decimal _bustCTR = 2m;
        // private const int _bustViews = -1;
        private const int _ctrCorrector = 1000;
        private Dictionary<string, ItemIds> _cache;
        private decimal _averageCtr;

        public CTRHelper(ICtrRepository repository) : base(repository) { }

        #region Initialize

        protected override int Initialize() {
            FillTheCache();
            CalculateAverageCtr();
            return _cache.Count;
        }

        private void FillTheCache() {
            var ctrs = Repository.GetCtrs();
            _cache = ctrs.ToDictionary(i => i.ProductId, i => i);
        }

        private void CalculateAverageCtr()
        {
            decimal totalViews = _cache.Sum(e => e.Value.Views);
            decimal totalClicks = _cache.Sum(e => e.Value.Clicks);

            var averageCtr = CalculatePercent(totalClicks, totalViews);

            _averageCtr = Math.Round(averageCtr, DecimalPlaces);
        }
        #endregion

        protected override int GetCtrFromChild(string itemId) {
            var (views, clics) = GetData(itemId);
            return CalculateNormalizeCtr(views, clics);
        }

        private int CalculateNormalizeCtr(decimal views, decimal clicks) {
            var ctr = views > BustViews ? CalculateCtr(views, clicks) : _bustCTR;

            return (int)(ctr * GetCtrCoefficient());
        }

        private decimal CalculateCtr(decimal views, decimal clicks)
        {
            var ctr = (clicks / _ctrCorrector) + ((_ctrCorrector - views) / _ctrCorrector) * _averageCtr;
            return Math.Round(ctr, DecimalPlaces);
        }

        private (int, int) GetData(string itemId)
        {
            if (_cache.ContainsKey(itemId))
            {
                var ctr = _cache[itemId];

                return (ctr.Views, ctr.Clicks);
            }

            return (0, 0);
        }

    }
}
