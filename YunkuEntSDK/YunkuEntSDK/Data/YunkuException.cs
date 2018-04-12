using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YunkuEntSDK.Data
{
    public class YunkuException:Exception
    {
        public YunkuException(string message, ReturnResult result) :base(message)
        {
            Result = result;
        }

        public ReturnResult Result { get; private set; }
    }
}
