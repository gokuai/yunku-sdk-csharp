using System;

namespace YunkuEntSDK.Net
{

    public class ProgressEventArgs:EventArgs
    {
        public int ProgressPercent { get; set; }

        public string LocalFullpath { get; set; }

    }
}
