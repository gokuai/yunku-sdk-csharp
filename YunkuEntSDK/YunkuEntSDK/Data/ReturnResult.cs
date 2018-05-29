using System.Net;
using YunkuEntSDK.UtilClass;

namespace YunkuEntSDK.Data
{
    public class ReturnResult
    {
        public string Body { get; set; }
        public int Code { get; set; }

        public bool IsOK()
        {
            return this.Code == (int)HttpStatusCode.OK || this.Code == (int)HttpStatusCode.PartialContent;
        }

        public string ToJsonString()
        {
            return SimpleJson.SerializeObject(this);
        }
    }
}
