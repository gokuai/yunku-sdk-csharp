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
        #region 私有成员

        private IDictionary<string, string> _headParameter;
        private IDictionary<string, string> _parameter;

        #endregion

        /// <summary>
        ///     默认构造函数
        /// </summary>
        /// <remarks>
        ///     默认的请求方式的GET
        /// </remarks>
        public HttpRequest()
        {
            RequestUrl = "";
            _parameter = new Dictionary<string, string>();
            _headParameter = new Dictionary<string, string>();
            RequestMethod = RequestType.Get; //默认请求方式为GET方式
        }

        /// <summary>
        /// </summary>
        public List<byte> PostDataByte { get; set; }


        public Stream Content { get; set; }

        //public Stream FileContent
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        ///     内容类型，post提交使用，默认为 application/x-www-form-urlencoded
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        ///     请求返回结果
        /// </summary>
        public string Result { get; private set; }


        /// <summary>
        ///     Http请求指代
        /// </summary>
        /// <param name="sender">发送者</param>
        /// <param name="e">发送所带的参数</param>
        public delegate void HttpResquestEventHandler(object sender, HttpEventArgs e);

        /// <summary>
        ///     Http请求完成事件
        /// </summary>
        public event HttpResquestEventHandler HttpCompleted;

        /// <summary>
        ///     触发HTTP请求完成方法
        /// </summary>
        /// <param name="e">事件参数</param>
        public void OnHttpCompleted(HttpEventArgs e)
        {
            if (HttpCompleted != null)
            {
                HttpCompleted(this, e);
            }
        }

        /// <summary>
        ///     请求URL地址
        /// </summary>
        public string RequestUrl { get; set; }

        /// <summary>
        ///     请求方式
        /// </summary>
        public RequestType RequestMethod { get; set; }

        public void AppendParameter(Dictionary<string, string> paramter)
        {
            if (paramter != null)
            {
                _parameter = paramter;
            }

        }

        /// <summary>
        ///     追加头参数
        /// </summary>
        /// <param name="key">追加键</param>
        /// <param name="value">键对应的值</param>
        public void AppendHeaderParameter(Dictionary<string, string> headParamter)
        {
            if (headParamter != null)
            {
                _headParameter = headParamter;
            }
        }

        public void Request()
        {
            
                switch (RequestMethod)
            {
                case RequestType.Get:
                    GetRequest();
                    break;
                default:
                    DefautRequest();
                    break;
            }
        }


        /// <summary>
        ///     默认请求方式
        /// </summary>
        private void DefautRequest()
        {
            var myurl = new Uri(RequestUrl);
            var webRequest = (HttpWebRequest)WebRequest.Create(myurl);
            webRequest.Method = RequestMethod.ToString();
            webRequest.Timeout = Config.HttpTimeout;
            webRequest.ReadWriteTimeout = Config.HttpTimeout;
            webRequest.UserAgent = Config.UserAgent;
            webRequest.Headers.Add("Accept-Language", Config.Language);

            if (!string.IsNullOrEmpty(ContentType))
            {
                webRequest.ContentType = ContentType;
            } else if (RequestMethod.Equals(RequestType.Post))
            {
                webRequest.ContentType = "application/x-www-form-urlencoded";
            }

            if (RequestMethod == RequestType.Put)
            {
                webRequest.KeepAlive = true;
            }
            
            setHeaderCollection(webRequest.Headers);

            using (Stream stream = webRequest.GetRequestStream())
            {
                using (var writer = new StreamWriter(stream))
                {
                    string parameterstring = GetParemeterString();
                    if (parameterstring.Length > 0)
                    {
                        writer.Write(GetParemeterString());
                        writer.Flush();
                    }

                    if (PostDataByte != null) //msmutipart
                    {
                        long count = 0;
                        foreach (byte b in PostDataByte)
                        {
                            count++;
                            if (count % (1024 * 1024 * 100) == 0) LogPrint.Print("uploading" + count);

                            stream.WriteByte(b);
                        }
                        stream.Flush();
                    }

                    if (Content != null)
                    {
                        byte[] bytes = Util.ReadToEnd(Content);
                        stream.Write(bytes, 0, bytes.Length);
                        stream.Flush();
                    }
                }
            }

            Result = GetResponeResult(webRequest);
           
            _parameter.Clear();
            _headParameter.Clear();

        }

        private string GetResponeResult(HttpWebRequest webRequest)
        {
            string result = "";
            int code;
            try
            {
                using (var response = (HttpWebResponse)webRequest.GetResponse())
                {
                    // 服务器返回成功消息
                    code = (int)response.StatusCode;
                    using (Stream stream = response.GetResponseStream())
                    {
                        using (var sr = new StreamReader(stream))
                        {
                            result = sr.ReadToEnd();
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                var response = ((HttpWebResponse)ex.Response);
                try
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            result = reader.ReadToEnd();
                        }
                    }
                }
                catch (WebException)
                {
                }
                code = (int)response.StatusCode;



            }
            return new ReturnResult()
            {
                Code = code,
                Result = result

            }.ToJsonString(); ;
        }

        /// <summary>
        ///     Get请求
        /// </summary>
        private void GetRequest()
        {
            string strrequesturl = RequestUrl;
            string parastring = GetParemeterString();
            if (parastring.Length > 0)
            {
                strrequesturl += (RequestUrl.IndexOf("?") > 0 ? "&" : "?") + parastring;
            }
            var myurl = new Uri(strrequesturl);
            var webRequest = (HttpWebRequest)WebRequest.Create(myurl);
            webRequest.Timeout = Config.HttpTimeout;
            webRequest.ReadWriteTimeout = Config.HttpTimeout;
            webRequest.UserAgent = Config.UserAgent;
            webRequest.Headers.Add("Accept-Language", Config.Language);
            webRequest.Method = "GET";
            setHeaderCollection(webRequest.Headers);

            Result = GetResponeResult(webRequest);

            
            //清空参数列表
            _parameter.Clear();
            _headParameter.Clear();
        }

        /// <summary>
        ///     获取传递参数的字符串
        /// </summary>
        /// <returns>字符串</returns>
        private string GetParemeterString()
        {
            string result = "";
            var sb = new StringBuilder();
            bool hasParameter = false;
            string value = "";
            foreach (var item in _parameter)
            {
                if (!hasParameter)
                    hasParameter = true;

                if (item.Value == null)
                {
                    continue;
                }
                value = Uri.EscapeDataString(item.Value); //对传递的字符串进行编码操作
                sb.Append(string.Format("{0}={1}&", item.Key, value));
            }
            if (hasParameter)
            {
                result = sb.ToString();
                int len = result.Length;
                result = result.Substring(0, --len); //将字符串尾的‘&’去掉
            }
            LogPrint.Print("-------httprequest------------>" + result);
            return result;
        }

        /// <summary>
        ///     获得头参数
        /// </summary>
        /// <param name="webrequest"></param>
        /// <returns></returns>
        private void setHeaderCollection(WebHeaderCollection header)
        {
            if (_headParameter != null)
            {
                if (_headParameter.Count > 0)
                {
                    foreach (var item in _headParameter)
                    {
                        if (item.Value == null)
                        {
                            continue;
                        }
                        header[item.Key] = item.Value;
                    }
                }
            }


        }

      


    }
}