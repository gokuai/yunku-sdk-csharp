using System;
using System.Collections.Generic;
using System.IO;
using YunkuEntSDK.Data;
using YunkuEntSDK.Net;

namespace YunkuEntSDK
{
    public abstract class HttpEngine : SignAbility
    {
        private const string Log_Tag = "HttpEngine";

        public HttpEngine(string clientId, string secret) : base(clientId, secret)
        {
        }

        public ReturnResult Call(string url, RequestType method, IDictionary<string, string> headers, IDictionary<string, string> data, Stream stream, bool disableSign)
        {
            RequestHelper request = new RequestHelper(this).SetUrl(url).SetMethod(method);
            if (headers != null && headers.Count > 0)
            {
                request.SetHeaders(headers);
            }
            if (data != null && data.Count > 0)
            {
                request.SetParams(data);
            }
            if (stream != null)
            {
                request.SetContent(stream);
            }
            return request.ExecuteSync(disableSign);
        }

        /// <summary>
        /// 
        /// </summary>
        public class RequestEvents
        {
            public int ApiID { get; set; }
            public ReturnResult Result { get; set; }
        }

        public delegate void RequestEventHanlder(object sender, RequestEvents e);

        protected internal class RequestHelper
        {
            protected HttpEngine _engine;
            protected RequestType _method;
            protected IDictionary<string, string> _params;
            protected IDictionary<string, string> _headers;
            protected string _url;
            protected bool _checkAuth;
            
            protected Stream _stream;
            protected string _contentType;

            public RequestHelper(HttpEngine engine)
            {
                this._engine = engine;
            }

            //public event RequestEventHanlder RequestCompleted;

            public RequestHelper SetMethod(RequestType method)
            {
                this._method = method;
                return this;
            }

            public RequestHelper SetParams(IDictionary<string, string> parameter)
            {
                this._params = parameter;
                return this;
            }

            public RequestHelper SetHeaders(IDictionary<string, string> headers)
            {
                this._headers = headers;
                return this;
            }

            public RequestHelper SetCheckAuth(bool checkAuth)
            {
                this._checkAuth = checkAuth;
                return this;
            }

            public RequestHelper SetUrl(string url)
            {
                this._url = url;
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

            public ReturnResult ExecuteSync()
            {
                return this.ExecuteSync(false);
            }

            public ReturnResult ExecuteSync(bool disableSign)
            {
                this.CheckNecessaryParams(_url);
                this.SetCommonParams(disableSign);

                if (_checkAuth)
                {
                    if (this.GetType().Equals(typeof(IAuthRequest)))
                    {
                        return ((IAuthRequest)this).SendRequestWithAuth(_url, _method, _params, _headers);
                    }
                }

                var request = new HttpRequest(this._url);
                request.Method = _method;
                if (_params != null && _params.Count > 0)
                {
                    request.SetParameters(_params);
                }
                if (_headers != null && _headers.Count > 0)
                {
                    request.SetHeaders(_headers);
                }
                if (_contentType != null && _contentType.Length > 0)
                {
                    request.ContentType = _contentType;
                }
                if (_stream != null)
                {
                    request.ContentBody = _stream;
                }
                return request.Request();
            }

            protected virtual void SetCommonParams(bool disableSign)
            {
                if (disableSign)
                {
                    return;
                }
                if (this._params == null)
                {
                    this._params = new Dictionary<string, string>();
                }
                this._params.Add("sign", this._engine.GenerateSign(this._params));
            }

            private void CheckNecessaryParams(string url)
            {
                if (string.IsNullOrEmpty(url))
                {
                    throw new ArgumentException("url must not be null");
                }
            }

        }
    }


}
