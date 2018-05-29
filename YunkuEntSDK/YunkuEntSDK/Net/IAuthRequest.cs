using System.Collections.Generic;
using YunkuEntSDK.Data;

namespace YunkuEntSDK.Net
{
    internal interface IAuthRequest
    {
       ReturnResult SendRequestWithAuth(string url, RequestType method, IDictionary<string, string> parameter, IDictionary<string, string> headParameter);
    }
}
