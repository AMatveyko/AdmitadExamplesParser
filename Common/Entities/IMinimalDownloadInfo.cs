using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Entities
{
    public interface IMinimalDownloadInfo
    {
        string FilePath { get; }
        int ShopWeight { get; }
        string NameLatin { get; }
    }
}
