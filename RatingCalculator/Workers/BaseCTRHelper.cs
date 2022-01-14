using Common.Api;
using System;
using TheStore.Api.Front.Data.Pub;

namespace RatingCalculator.Workers
{
    internal abstract class BaseCTRHelper: ICtrHelper
    {
        protected const int BustViews = 100;
        protected const int DecimalPlaces = 1;

        protected readonly ICtrRepository Repository;
        private BackgroundBaseContext _context = new BackgroundBaseContext("empty","empty");

        private DateTime _lastInitialize;

        private const int CacheLifeTime = 6;

        protected BaseCTRHelper(ICtrRepository repository) => (Repository) = (repository);

        public int GetCtr(string itemId) {
            WorkWithCache();
            return GetCtrFromChild(itemId);
        }

        protected void WorkWithCache() {
            if (_lastInitialize < DateTime.Now.AddHours(-CacheLifeTime)) {
                DetermineCauseCacheRenew();
                var newEntries = Initialize();
                _context.AddMessage($"{newEntries} new entries in the cache.");
            }
            _lastInitialize = DateTime.Now;
        }

        protected abstract int Initialize();
        protected abstract int GetCtrFromChild(string itemId);
        protected decimal CalculatePercent(decimal clicks, decimal views) => clicks / views * 100;
        protected int GetCtrCoefficient() => (int)Math.Pow(10, DecimalPlaces);

        private void DetermineCauseCacheRenew() {
            var cause = _lastInitialize == default(DateTime) ? "Cache is empty" : "Cache is out of date";
            _context.AddMessage($"{cause}. Start updating the cache.");
        }

        public void ChangeContext(BackgroundBaseContext context) {
            _context = context;
        }
    }
}
