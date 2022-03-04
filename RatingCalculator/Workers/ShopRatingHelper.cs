using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheStore.Api.Front.Data.Entities;
using TheStore.Api.Front.Data.Pub;

namespace RatingCalculator.Workers
{
    internal class ShopRatingHelper
    {

        private const decimal RatingLevels = 10;

        private Dictionary<string, int> _ecpcRaitings;
        private List<ShopDb> _shops;


        public ShopRatingHelper(IShopRepository repository) => Initialize(repository);

        public int GetEcpc( string shopId) =>
            _ecpcRaitings.ContainsKey(shopId) ? _ecpcRaitings[shopId] : throw new ArgumentOutOfRangeException("Магазин не найден");

        public int GetInternalRating(string shopId) => GetShop(shopId).Weight;

        public bool IsOpeningProgramm(string shopId) => GetShop(shopId).Enabled;

        private void Initialize(IShopRepository repository) {
            if( _shops == null) {
                FillShops(repository);
            }
            if( _ecpcRaitings == null) {
                FillRaitings();
            }
        }

        private void FillRaitings() {
            var ecpcs = GetEcpcs();
            var levels = GetLevels(ecpcs.Count);
            _ecpcRaitings = _shops.ToDictionary(s => s.Id.ToString(), s => GetRatingByEcpc(s.ECPC, ecpcs, levels));
        }

        private void FillShops(IShopRepository repository) => _shops = repository.GetShops();

        private static int GetRatingByEcpc( double ecpc, List<double> ecpcs, Dictionary<int,decimal> levels)        {
            var index = ecpcs.IndexOf(ecpc);
            return levels.First(l => index < l.Value || index == l.Value).Key;
        }

        private List<double> GetEcpcs() => _shops.Select(s => s.ECPC).Distinct().OrderBy(e => e).ToList();

        private static Dictionary<int,decimal> GetLevels( int ecpcCount) {
            decimal step = ecpcCount / RatingLevels;
            var levels = new Dictionary<int, decimal>();

            for (var i = 1; i <= RatingLevels; i++) {
                levels.Add(i - 1, step * i);
            }

            return levels;
        }

        private ShopDb GetShop(string shopId) => _shops.First(s => s.Id.ToString() == shopId);
    }
}
