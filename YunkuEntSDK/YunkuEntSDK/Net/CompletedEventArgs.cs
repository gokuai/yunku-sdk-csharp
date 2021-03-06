﻿using System;
using YunkuEntSDK.Data;

namespace YunkuEntSDK.Net
{
    public class CompletedEventArgs:EventArgs
    {
        public bool IsError { get; set; }

        public string ErrorMessage { get; set; }

        public string Fullpath { get; set; }

        public FileInfo FileInfo { get; set; }
    }
}
