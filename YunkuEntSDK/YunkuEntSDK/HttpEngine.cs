using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using YunkuEntSDK.Net;
using YunkuEntSDK.UtilClass;

namespace YunkuEntSDK
{
    public abstract class HttpEngine : SignAbility
    {

        private const string Log_Tag = "HttpEngine";

        protected string _clientId;
        protected string _clientSecret;


        /// <summary>
        /// 
        /// </summary>
        public class RequestEvents
        {
            public int ApiID { get; set; }
            public string Result { get; set; }
        }

        public delegate void RequestEventHanlder(object sender, RequestEvents e);

        public HttpEngine(string clientId, string clientSecret)
        {
            _clientId = clientId;
            _clientSecret = clientSecret;
        }

        public string GenerateSign(Dictionary<string, string> parameter)
        {
            return GenerateSign(parameter, _clientSecret);
        }

        public string GenerateSign(Dictionary<string, string> parameter, List<string> ignoreKeys)
        {
            return GenerateSign(parameter, _clientSecret, ignoreKeys);
        }


        internal class RequestHelper
        {
            RequestType _method;
            Dictionary<string, string> _params;
            Dictionary<string, string> _headParams;
            string _url;
            bool _checkAuth;

            List<string> _ignoreKeys;
            List<byte> _postDateByte;
            Stream _stream;
            string _contentType;

            public event RequestEventHanlder RequestCompleted;

            internal RequestHelper SetMethod(RequestType method)
            {
                this._method = method;
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
                this._checkAuth = checkAuth;
                return this;
            }

            public RequestHelper SetUrl(string url)
            {
                this._url = url;
                return this;
            }

            public RequestHelper SetIgnoreKeys(List<string> ignoreKeys)
            {
                this._ignoreKeys = ignoreKeys;
                return this;
            }

            public RequestHelper SetPostDataByte(List<byte> postDataByte)
            {
                this._postDateByte = postDataByte;
                return this;
            }

            public RequestHelper SetContent(Stream stream)
            {
                this._stream = stream;
                return this;
            }
 

            public RequestHelper SetContentType(string contentType)
            {
                this._contentType = contentType;
                return this;
            }

            public string ExecuteSync()
            {
                CheckNecessaryParams(_url, _method);

                if (!Util.IsNetworkAvailableEx())
                {
                    return "";
                }

                if (_checkAuth)
                {
                    if (this.GetType().Equals(typeof(IAuthRequest)))
                    {
                        return ((IAuthRequest)this).SendRequestWithAuth(_url, _method, _params, _headParams, _ignoreKeys);
                    }
                }

                var request = new HttpRequest { RequestUrl = _url };
                request.AppendParameter(_params);
                request.AppendHeaderParameter(_headParams);
                request.ContentType = _contentType;
                request.PostDataByte = _postDateByte;
                request.RequestMethod = _method;
                request.Request();
                return request.Result;
            }


            public Thread ExecuteAsync(int ApiID, RequestEventHanlder hanlder)
            {
                RequestCompleted += hanlder;
                SynchronizationContext context = SynchronizationContext.Current;

                Thread thread = new Thread(() =>
                {
                    string result = ExecuteSync();
                    context.Post(state =>
                    {
                        RequestCompleted?.Invoke(this, new RequestEvents() { Result = result, ApiID = ApiID });
                    }, null);
                });
                thread.Start();
                return thread;
            }

            private void CheckNecessaryParams(string url, RequestType method)
            {
                if (string.IsNullOrEmpty(url))
                {
                    throw new ArgumentException("url must not be null");
                }
            }

        }
    }


}
