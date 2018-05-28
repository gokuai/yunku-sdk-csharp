using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YunkuEntSDK.Data;

namespace YunkuEntSDK.Net
{
    internal interface IAuthRequest
    {
       ReturnResult SendRequestWithAuth(string url, RequestType method, IDictionary<string, string> parameter, IDictionary<string, string> headParameter);
    }
}
