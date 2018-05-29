using System;

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
