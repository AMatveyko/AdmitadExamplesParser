using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Entities
{
    public interface IMinimalDownloadsInfo
    {
        int ShopWeight { get; }
        string NameLatin { get; }
        List<IFileInfo> Files { get; }
    }
}
