using System;
using System.Collections.Generic;
using System.Text;

using Common.Entities.Rating;

using TheStore.Api.Front.Data.Entities;

namespace TheStore.Api.Front.Data.Pub
{
    public interface ICtrRepository
    {
        List<ItemCtrInfo> GetCtrs();
    }
}
