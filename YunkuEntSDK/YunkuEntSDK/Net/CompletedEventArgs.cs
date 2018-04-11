﻿using System;

namespace YunkuEntSDK.Net
{
    public class CompletedEventArgs:EventArgs
    {
        public bool IsError { get; set; }

        public string ErrorMessage { get; set; }

        public string Fullpath { get; set; }
    }
}
