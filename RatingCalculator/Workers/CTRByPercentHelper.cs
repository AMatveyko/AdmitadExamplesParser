using Common.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using TheStore.Api.Front.Data.Pub;

namespace RatingCalculator.Workers
{
    internal class CTRByPercentHelper : BaseCTRHelper
    {

        private Dictionary<string, int> _calculatedCtrs;
        private int _bustCtr;

        public CTRByPercentHelper(ICtrRepository repository) : base(repository) {}

        private int CalculateCtr(decimal views, decimal clicks) {
            var percent = Math.Round(CalculatePercent(clicks, views), DecimalPlaces);
            
            if( percent == 100) {
                percent = 99;
            }

            return (int)(percent * GetCtrCoefficient());
        }

        private Dictionary<string, int> GetCalculatedCtrsFromDb(ICtrRepository repository) {
            
            var ctrs = repository.GetCtrs();

            return ctrs.Where(c => c.Views >= BustViews).ToDictionary(i => i.ProductId, i => CalculateCtr(i.Views, i.Clicks));
        }

        private int GetCalculatedBurstCtr() {
            var maxCtr = _calculatedCtrs.Max(i => i.Value);
            var avgCtr = (int)_calculatedCtrs.Average(i => i.Value);
            return avgCtr + ((maxCtr - avgCtr) / 2);
        }

        protected override int Initialize() {
            _calculatedCtrs = GetCalculatedCtrsFromDb(Repository);
            _bustCtr = GetCalculatedBurstCtr();
            return _calculatedCtrs.Count;
        }

        protected override int GetCtrFromChild(string itemId) => _calculatedCtrs.ContainsKey(itemId) ? _calculatedCtrs[itemId] : _bustCtr;
    }
}
