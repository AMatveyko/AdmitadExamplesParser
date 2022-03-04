using Common.Api;
using RatingCalculator.Workers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheStore.Api.Front.Data.Pub;

namespace RatingCalculator.Pub
{
    public sealed class CtrHelpersBuilder
    {
        private readonly CtrHelperType _type;

        public CtrHelpersBuilder(CtrHelperType type ) => (_type) = (type);

        internal ICtrHelper Get(ICtrRepository repository) => _type switch {
                CtrHelperType.Percent => new CTRByPercentHelper(repository),
                CtrHelperType.Click => new CtrHelperByClicks(repository),
                _ => new CTRHelper(repository)
            };
    }
}
