using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YunkuEntSDK.Data
{
    public class UploadResult
    {
        public bool IsError { get; set; }

        public string ErrorMessage { get; set; }

        public FileInfo FileInfo { get; set; }
    }
}
