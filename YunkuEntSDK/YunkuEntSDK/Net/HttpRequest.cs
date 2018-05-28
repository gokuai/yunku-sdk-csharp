using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using YunkuEntSDK.Data;
using YunkuEntSDK.UtilClass;

namespace YunkuEntSDK.Net
{
    internal class HttpRequest
    {
        private IDictionary<string, string> _headers;
        private IDictionary<string, string> _parameters;

        public string Url { get; set; }

        public RequestType Method { get; set; }

        public Stream ContentBody { get; set; }

        public string ContentType { get; set; }

        public HttpRequest(string url)
        {
            Url = url;
            Method = RequestType.GET;
        }

        public void SetParameters(IDictionary<string, string> parameters)
        {
            if (parameters != null)
            {
                _parameters = parameters;
            }
        }

        public void SetHeaders(IDictionary<string, string> headers)
        {
            if (headers != null)
            {
                _headers = headers;
            }
        }

        public ReturnResult Request()
        {
            if (Method == RequestType.GET)
            {
                return CreateGetRequest();
            }
            else
            {
                return CreateDefautRequest();
            }
        }

        private HttpWebRequest CreateRequest()
        {
            string url = this.Url;
            if (this.Method == RequestType.GET)
            {
                string query = this.GetParemeterString();
                if (query.Length > 0)
                {
                    url += (url.IndexOf("?") > 0 ? "&" : "?") + query;
                }
            }
            var request = (HttpWebRequest)WebRequest.Create(new Uri(url));
            request.Method = Method.ToString();
            request.Timeout = Config.HttpTimeout;
            request.ReadWriteTimeout = Config.HttpTimeout;
            request.UserAgent = Config.UserAgent;
            request.Headers.Set("Accept-Language", Config.Language);
            if (!string.IsNullOrEmpty(ContentType))
            {
                request.ContentType = ContentType;
            }
            else if (Method.Equals(RequestType.POST))
            {
                request.ContentType = "application/x-www-form-urlencoded";
            }
            if (Method == RequestType.PUT)
            {
                request.KeepAlive = true;
            }

            if (this._headers != null && this._headers.Count > 0)
            {
                foreach (var head in this._headers)
                {
                    if (head.Value == null)
                    {
                        continue;
                    }
                    request.Headers.Set(head.Key, Uri.EscapeDataString(head.Value));
                }
            }
            
            return request;
        }

        private ReturnResult CreateDefautRequest()
        {
            var request = this.CreateRequest();
            string parameterstring = this.GetParemeterString();
            int retry = Config.Retry;
            while (true)
            {
                try
                {
                    using (Stream stream = request.GetRequestStream())
                    {
                        if (parameterstring.Length > 0)
                        {
                            using (var writer = new StreamWriter(stream))
                            {
                                writer.Write(parameterstring);
                                writer.Flush();
                            }
                        }

                        if (ContentBody != null)
                        {
                            byte[] buffer = new byte[4096];
                            int read;
                            while ((read = ContentBody.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                stream.Write(buffer, 0, read);
                            }
                            stream.Flush();
                        }
                    }

                    break;
                }
                catch (WebException ex)
                {
                    if (ex.Response == null)
                    {
                        if (retry-- > 0)
                        {
                            Thread.Sleep(1000);
                        }
                        else
                        {
                            throw;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }

            ReturnResult result = GetResponeResult(request);
            return result;
        }

        private WebResponse GetResponse(WebRequest request)
        {
            try
            {
                return request.GetResponse();
            }
            catch (WebException ex)
            {
                if (ex.Response == null)
                {
                    throw;
                }
                return ex.Response;
            }
        }

        private ReturnResult GetResponeResult(HttpWebRequest request)
        {
            ReturnResult result = new ReturnResult();
            int retry = Config.Retry;
            while (true)
            {
                try
                {
                    using (var response = this.GetResponse(request) as HttpWebResponse)
                    {
                        // 服务器返回成功消息
                        result.Code = (int)response.StatusCode;
                        using (Stream stream = response.GetResponseStream())
                        {
                            using (var sr = new StreamReader(stream))
                            {
                                result.Body = sr.ReadToEnd();
                            }
                        }
                    }

                    return result;
                }
                catch(WebException)
                {
                    if (retry-- > 0)
                    {
                        Thread.Sleep(1000);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            
        }

        private ReturnResult CreateGetRequest()
        {
            var request = this.CreateRequest();
            ReturnResult result = GetResponeResult(request);
            return result;
        }

        private string GetParemeterString()
        {
            string result = "";
            if (this._parameters == null || this._parameters.Count == 0)
            {
                return result;
            }
            var sb = new StringBuilder();
            string value = "";
            foreach (var item in _parameters)
            {
                if (item.Value == null)
                {
                    continue;
                }
                value = Uri.EscapeDataString(item.Value); //对传递的字符串进行编码操作
                sb.Append(string.Format("&{0}={1}", item.Key, value));
            }
            if (sb.Length > 0)
            {
                result = sb.ToString().Substring(1);
            }
            LogPrint.Print("-------httprequest------------>" + result);
            return result;
        }
    }
}