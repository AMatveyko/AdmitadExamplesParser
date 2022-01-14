using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheStore.Api.Front.Data.Entities;

namespace CategoryWorks
{
    internal sealed class CategoryPack
    {
        public ParentShopCategory ShopCategory { get; set; }
        public CategoryDb LocalCatetory { get; set; }
    }
}
