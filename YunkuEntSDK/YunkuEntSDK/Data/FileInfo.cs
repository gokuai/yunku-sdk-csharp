using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YunkuEntSDK.Data
{
    public class FileInfo
    {
        public string Fullpath { get; set; }

        public string Filename { get; set; }

        public string Filehash { get; set; }

        public long Filesize { get; set; }

        public string Hash { get; set; }

        public string UploadServer { get; set; }
    }
}
