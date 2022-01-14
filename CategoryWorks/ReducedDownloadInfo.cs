using Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CategoryWorks
{
    internal class ReducedDownloadInfo : IMinimalDownloadInfo
    {
        public string FilePath { get; set; }

        public int ShopWeight { get; set; }

        public string NameLatin { get; set; }
    }
}
