using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YunkuEntSDK.Net
{
    internal interface IAuthRequest
    {
       string SendRequestWithAuth(string url, RequestType method, Dictionary<string, string> parameter, Dictionary<string, string> headParameter, List<string> ignoreKeys);
    }
}
