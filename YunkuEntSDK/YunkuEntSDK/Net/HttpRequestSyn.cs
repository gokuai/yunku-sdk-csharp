using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using YunkuEntSDK.UtilClass;

namespace YunkuEntSDK.Net
{
    internal class HttpRequestSyn
    {
        #region 私有成员

        private readonly IDictionary<string, string> _headParameter;
        private readonly IDictionary<string, string> _parameter;

        #endregion

        /// <summary>
        ///     默认构造函数
        /// </summary>
        /// <remarks>
        ///     默认的请求方式的GET
        /// </remarks>
        public HttpRequestSyn()
        {
            RequestUrl = "";
            _parameter = new Dictionary<string, string>();
            _headParameter = new Dictionary<string, string>();
            RequestMethod = RequestType.Get; //默认请求方式为GET方式
        }

        /// <summary>
        /// </summary>
        public HttpStatusCode Code { private set; get; }

        /// <summary>
        /// </summary>
        public List<byte> PostDataByte { get; set; }

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
        ///     参数排序，不包括签名
        /// </summary>
        public string[] SortedParamter
        {
            get
            {
                if (_parameter != null)
                {
                    var myList = new List<KeyValuePair<string, string>>();
                    foreach (var key in _parameter)
                    {
                        myList.Add(key);
                    }
                    myList.Sort((firstPair, nextPair) => { return firstPair.Key.CompareTo(nextPair.Key); });
                    var arr = new string[myList.Count];
                    for (int i = 0; i < myList.Count; i++)
                    {
                        //list
                        arr[i] = myList[i].Value;
                    }

                    return arr;
                }
                return new string[0];
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

        /// <summary>
        ///     追加参数
        /// </summary>
        /// <param name="key">进行追加的键</param>
        /// <param name="value">键对应的值</param>
        public void AppendParameter(string key, string value)
        {
            _parameter.Add(key, value);
        }

        /// <summary>
        ///     追加头参数
        /// </summary>
        /// <param name="key">追加键</param>
        /// <param name="value">键对应的值</param>
        public void AppendHeaderParameter(string key, string value)
        {
            if (!value.Equals(string.Empty))
            {
                _headParameter.Add(key, value);
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
            var webRequest = (HttpWebRequest) WebRequest.Create(myurl);
            webRequest.UserAgent = HostConfig.UserAgent;
            if (string.IsNullOrEmpty(ContentType))
            {
                webRequest.ContentType = "application/x-www-form-urlencoded";
            }
            else
            {
                webRequest.KeepAlive = true;
                webRequest.Timeout = 1000*60*60*24;
                webRequest.ContentType = ContentType;
            }
            webRequest.Method = RequestMethod.ToString();

            if (GetHeadParame() != null)
            {
                webRequest.Headers = GetHeadParame();
            }

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

                    if (PostDataByte != null)
                    {
                        long count = 0;
                        foreach (byte b in PostDataByte)
                        {
                            count++;
                            if (count%(1024*1024*100) == 0) LogPrint.Print("uploading" + count);

                            stream.WriteByte(b);
                        }
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
            try
            {
                using (var response = (HttpWebResponse) webRequest.GetResponse())
                {
                    // 服务器返回成功消息
                    Code = response.StatusCode;
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
                var response = ((HttpWebResponse) ex.Response);
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
                Code = response.StatusCode;
            }
            return result;
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
                strrequesturl += "?" + parastring;
            }
            var myurl = new Uri(strrequesturl);
            var webRequest = (HttpWebRequest) WebRequest.Create(myurl);
            webRequest.UserAgent = HostConfig.UserAgent;
            webRequest.Method = "GET";
            if (GetHeadParame() != null)
            {
                webRequest.Headers = GetHeadParame();
            }
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
                value = Uri.EscapeUriString(item.Value); //对传递的字符串进行编码操作
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
        private WebHeaderCollection GetHeadParame()
        {
            var header = new WebHeaderCollection();
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
            else
            {
                return null;
            }


            return header;
        }
    }
}