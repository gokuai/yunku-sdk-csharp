using YunkuEntSDK.UtilClass;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace YunkuEntSDK.Net
{
     class HttpRequestSyn    {
        #region 私有成员
        private const int BLOACK_SIZE = 4096;
        private string _request_url = null;
        private RequestType _request_type;
        IDictionary<string, string> _parameter;
        IDictionary<string, string> _headParameter;

        #endregion

         /// <summary>
         /// 
         /// </summary>
        public HttpStatusCode Code
        {
            private set;
            get;
        }

         /// <summary>
         /// 
         /// </summary>
        public List<byte> PostDataByte
        {
            get;
            set;
        }

        //public Stream FileContent
        //{
        //    get;
        //    set;
        //}

         /// <summary>
        /// 内容类型，post提交使用，默认为 application/x-www-form-urlencoded
         /// </summary>
        public string ContentType
        {
            get;
            set;
        }

         /// <summary>
         /// 请求返回结果
         /// </summary>
        public string Result
        {
            get;
            private set;
        }

        /// <summary>
        /// 参数排序，不包括签名
        /// </summary>
        public string[] SortedParamter
        {
            get
            {

                if (_parameter != null)
                {
                    List<KeyValuePair<string, string>> myList = new List<KeyValuePair<string, string>>();
                    foreach (KeyValuePair<string, string> key in _parameter)
                    {
                        myList.Add(key);
                    }
                    myList.Sort((firstPair, nextPair) =>
                    {
                        return firstPair.Key.CompareTo(nextPair.Key);
                    });
                    string[] arr = new string[myList.Count];
                    for (int i = 0; i < myList.Count; i++)
                    {
                        //list
                        arr[i] = myList[i].Value;
                    }

                    return arr;
                }
                else
                {
                    return new string[0];
                }
            }
        }

        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <remarks>
        /// 默认的请求方式的GET
        /// </remarks>
        public HttpRequestSyn()
        {
            _request_url = "";
            _parameter = new Dictionary<string, string>();
            _headParameter=new Dictionary<string,string>();
            _request_type = RequestType.GET; //默认请求方式为GET方式
        }

        /// <summary>
        /// 追加参数
        /// </summary>
        /// <param name="key">进行追加的键</param>
        /// <param name="value">键对应的值</param>
        public void AppendParameter(string key, string value)
        {
            _parameter.Add(key, value);
        }

        /// <summary>
        /// 追加头参数
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

        /// <summary>
        /// 请求URL地址
        /// </summary>
        public string RequestUrl
        {
            get { return _request_url; }
            set { _request_url = value; }
        }

        /// <summary>
        /// 请求方式
        /// </summary>
        public RequestType RequestMethod
        {
            get { return _request_type; }
            set { _request_type = value; }
        }

        public void Request()
        {
            switch (RequestMethod)
            {
                case RequestType.GET:
                   this.GetRequest();
                   break;
                default:
                    this.DefautRequest();
                    break;
            }
 
        }

         /// <summary>
         /// 默认请求方式
         /// </summary>
        private void DefautRequest()
        {
            Uri myurl = new Uri(this.RequestUrl);
            HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create(myurl);
            webRequest.UserAgent = "GoKuai_EntSDK";
            if (string.IsNullOrEmpty(ContentType))
            {
                webRequest.ContentType = "application/x-www-form-urlencoded";
            }
            else
            {
                webRequest.KeepAlive = true;
                webRequest.Timeout = 1000 * 60 * 60 * 24;
                webRequest.ContentType = ContentType;
            }
            webRequest.Method = RequestMethod.ToString();
            
            if (GetHeadParame() != null)
            {
                webRequest.Headers = GetHeadParame();
            }
            
            using (Stream stream = webRequest.GetRequestStream())
            {

                using (StreamWriter writer = new StreamWriter(stream))
                {
                    string parameterstring = this.GetParemeterString();
                    if (parameterstring.Length > 0)
                    {
                        writer.Write(this.GetParemeterString());
                        writer.Flush();
                    }

                    if (PostDataByte != null)
                    {
                        long count=0;
                        foreach (byte b in this.PostDataByte)
                        {
                            count++;
                            if (count %(1024*1024*100) == 0) LogPrint.Print("uploading" + count);
                            
                            stream.WriteByte(b);
                        }
                        stream.Flush();
                    }
                }
            }

            this.Result = GetResponeResult(webRequest);

            _parameter.Clear();
            _headParameter.Clear();
            
        }

        private string GetResponeResult(HttpWebRequest webRequest) 
        {
            string result="";
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse())
                {
                    // 服务器返回成功消息
                    Code = response.StatusCode;
                    using (Stream stream = response.GetResponseStream())
                    {
                        using (StreamReader sr = new StreamReader(stream))
                        {

                            result = sr.ReadToEnd();
                        }

                    }
                }
            }
            catch (WebException ex) 
            {
                HttpWebResponse response = ((HttpWebResponse)ex.Response);
                try
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            result = reader.ReadToEnd();
                        }
                    }
                }
                catch (WebException)
                {

                }
                this.Code = response.StatusCode;
            }
            return result;
        }

         /// <summary>
         /// Get请求
         /// </summary>
        private void GetRequest()
        {
            string strrequesturl = this.RequestUrl;
            string parastring = this.GetParemeterString();
            if (parastring.Length > 0)
            {
                strrequesturl += "?" + parastring;
            }
            Uri myurl = new Uri(strrequesturl);
            HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create(myurl);
            webRequest.UserAgent = "GoKuai_EntSDK";
            webRequest.Method = "GET";
            if (GetHeadParame() != null)
            {
                webRequest.Headers = GetHeadParame();
            }
            this.Result = GetResponeResult(webRequest);
            //清空参数列表
            _parameter.Clear();
            _headParameter.Clear();
        }

        /// <summary>
        /// 获得头参数
        /// </summary>
        /// <param name="webrequest"></param>
        /// <returns></returns>
        private WebHeaderCollection GetHeadParame()
        {
            WebHeaderCollection header = new WebHeaderCollection();
            if (_headParameter.Count > 0)
            {
                foreach (var item in _headParameter)
                {
                    header[item.Key] = item.Value;
                }
            }
            else
            {
                return null;
            }


            return header;
        }

        /// <summary>
        /// 获取传递参数的字符串
        /// </summary>
        /// <returns>字符串</returns>
        private string GetParemeterString()
        {
            string result = "";
            StringBuilder sb = new StringBuilder();
            bool hasParameter = false;
            string value = "";
            foreach (var item in _parameter)
            {
                if (!hasParameter)
                    hasParameter = true;

                value = Uri.EscapeUriString(item.Value); //对传递的字符串进行编码操作
                sb.Append(string.Format("{0}={1}&", item.Key, value));
            }
            if (hasParameter)
            {
                result = sb.ToString();
                int len = result.Length;
                result = result.Substring(0, --len); //将字符串尾的‘&’去掉
            }
            LogPrint.Print("-------result------------>"+result);
            return result;

        }
    }


}
