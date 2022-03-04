using System;
using System.IO;

namespace DeleteEdgeTemporaryFilesTool
{
    class Program
    {
        
        private const string Path = @"E:\";
        
        static void Main(string[] args) {
            var directory = new DirectoryInfo(Path);
            var directories = directory.GetDirectories();
            var files = directory.GetFiles();
            foreach (var dir in directories) {
                try {
                    dir.Delete(true);
                }
                catch { }
            }

            foreach (var file in files) {
                try {
                    file.Delete();
                }
                catch { }
            }
        }
    }
}