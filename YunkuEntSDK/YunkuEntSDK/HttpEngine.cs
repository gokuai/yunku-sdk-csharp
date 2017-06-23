using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YunkuEntSDK.Net;
using YunkuEntSDK.UtilClass;

namespace YunkuEntSDK
{
    public abstract class HttpEngine : SignAbility
    {

        private const string Log_Tag = "ParentManager";

        protected string _clientId;
        protected string _clientSecret;

        public HttpEngine(string clientId, string clientSecret)
        {
            _clientId = clientId;
            _clientSecret = clientSecret;
        }

        public string GenerateSign(string[] array)
        {
            return GenerateSign(array, _clientSecret);
        }
    }

    public class RequestHelper
    {
        RequestType method;
        IDictionary<string, string> _params;
        IDictionary<string, string> _headParams;
        string url;
        bool checkAuth;

        List<string> ignoreKeys;


        internal RequestHelper SetMethod(RequestType method)
        {
            this.method = method;
            return this;
        }

        internal RequestHelper SetParams(Dictionary<string, string> parameter)
        {
            this._params = parameter;
            return this;
        }

        internal RequestHelper SetHeadParams(Dictionary<string, string> headParams)
        {
            this._headParams = headParams;
            return this;
        }

        RequestHelper SetCheckAuth(bool checkAuth)
        {
            this.checkAuth = checkAuth;
            return this;
        }

        public RequestHelper SetUrl(string url)
        {
            this.url = url;
            return this;
        }

        public RequestHelper SetIgnoreKeys(List<string> ignoreKeys)
        {
            this.ignoreKeys = ignoreKeys;
            return this;
        }

        public string ExecuteSync()
        {
            CheckNecessaryParams(url, method);

            var request = new HttpRequestSyn { RequestUrl = url };
            request.AppendParameter("", "");
            request.AppendHeaderParameter("", "");
            request.RequestMethod = method;
            request.Request();
            return request.Result;
        }

        private void CheckNecessaryParams(string url, RequestType method)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentException("url must not be null");
            }
            
            if (method == null)
            {
                throw new ArgumentException("method must not be null");
            }
        }
    }
}
