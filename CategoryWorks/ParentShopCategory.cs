using Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CategoryWorks
{
    internal sealed class ParentShopCategory : ShopCategory {
        public List<ParentShopCategory> Children { get; set; }

        public static ParentShopCategory Create(ShopCategory category) => new ParentShopCategory {
                ParentId = category.ParentId,
                Name = category.Name,
                Id = category.Id
            };
    }
}
