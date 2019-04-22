using SharpCompress.Archives.Zip;
using SharpCompress.Readers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDemoList.Models
{
    public class VlcStream
    {
        public ZipArchive Stream { get; set; }
        public FileStream fileStream { get; set; }
        public ReaderOptions Option { get; set; }
        public string ZipPath { get; set; }
        public void Dispose()
        {
            Stream.Dispose();
            Stream = null;
            fileStream.Dispose();
            fileStream = null;
            Option = null;
            ZipPath = null;
            check = null;
        }
        public Func<ZipArchiveEntry, bool> check { get; set; }
    }
}
