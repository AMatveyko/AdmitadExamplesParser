using Common.Api;
using RatingCalculator.Workers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheStore.Api.Front.Data.Pub;

namespace RatingCalculator.Workers
{
    internal sealed class CtrHelperByClicks : BaseCTRHelper
    {

        private Dictionary<string, int> _cache;

        public CtrHelperByClicks(ICtrRepository repository) : base(repository) { }

        protected override int GetCtrFromChild(string itemId) => _cache.ContainsKey(itemId) ? GetClicksFromCache(itemId) : 0;

        protected override int Initialize() {
            _cache = Repository.GetCtrs().ToDictionary(c => c.ProductId, c => c.Clicks);
            return _cache.Count;
        }

        private int GetClicksFromCache(string itemId) {
            var value = _cache[itemId];
            return value > 999 ? 999 : value;
        }
    }
}
