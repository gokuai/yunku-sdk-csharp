using System.Collections.Generic;
using YunkuEntSDK.UtilClass;

namespace YunkuEntSDK.Data
{
    public class ReturnResult
    {
        public string Body { get; set; }
        public int Code { get; set; }

        public string ToJsonString()
        {
            return SimpleJson.SerializeObject(this);
        }
    }
}
