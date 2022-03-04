using Common.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CategoryWorks
{
    internal class ReducedDownloadsInfo : IMinimalDownloadsInfo
    {
        public List<string> FilePaths { get; set; } 

        public int ShopWeight { get; set; }

        public string NameLatin { get; set; }
        public List<IFileInfo> Files => FilePaths.Select( fp => new FeedInfo( "1", "" ) ).Cast<IFileInfo>().ToList();
    }
}
