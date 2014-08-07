using System;
using System.Net;

using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using YunkuEntSDK.UtilClass;
using System.IO.IsolatedStorage;
using YunkuEntSDK.Net;
using System.Threading;
namespace YunkuEntSDK.Net
{
    /// <summary>
    /// 异步获取 unused
    /// </summary>
    public class HttpRequestAsync
    {
        #region 私有成员
        private string _request_url = null;
        private RequestType _request_type;
        IDictionary<string, string> _parameter;
        IDictionary<string, string> _headParameter;

        /// <summary>
        /// 不包括签名
        /// </summary>
        public string [] SortedParamter 
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
                    string []arr=new string[myList.Count];
                    for (int i = 0; i < myList.Count; i++) {
                        //list
                        arr[i] = myList[i].Value;
                    }

                    return arr;
                }else{
                    return new string[0];
                }
            }
        }
        #endregion

        ///// <summary>
        ///// 上传流
        ///// </summary>
        //public Stream Content
        //{
        //    set;
        //    get;
        //}

        /// <summary>
        /// 请求id
        /// </summary>
        public int ApiId
        {
            set;
            get;
        }

        public string ContentType
        {
            get;
            set;
        }

        public List<byte> PostDataByte
        {
            get;
            set;
        }



        /// <summary>
        /// Http请求指代
        /// </summary>
        /// <param name="sender">发送者</param>
        /// <param name="e">发送所带的参数</param>
        public delegate void HttpResquestEventHandler(object sender, HttpEventArgs e);

        /// <summary>
        /// Http请求完成事件
        /// </summary>
        public event HttpResquestEventHandler HttpCompleted;


        ///// <summary>
        ///// Http回调事件
        ///// </summary>
        //public Action CallbackAction
        //{
        //    get;
        //    set;
        //}


        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <remarks>
        /// 默认的请求方式的GET
        /// </remarks>
        public HttpRequestAsync()
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
        public void AppendHeaderParameter(string key,string value) {
            if (!value.Equals(string.Empty))
            {
                _headParameter.Add(key, value);
            }
            
        }



        /// <summary>
        /// 触发HTTP请求完成方法
        /// </summary>
        /// <param name="e">事件参数</param>
        public void OnHttpCompleted(HttpEventArgs e)
        {
            if (this.HttpCompleted != null)
            {
                this.HttpCompleted(this, e);
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
        internal RequestType RequestMethod
        {
            get { return _request_type; }
            set { _request_type = value; }
        }

        /// <summary>
        /// 进行HTTP请求
        /// </summary>
        public void Request()
        {

            switch(RequestMethod){
                case RequestType.GET:
                    this.GetRequest();
                    break;
                default :
                      DefautRequest();
                    break;
            }
        }

        /// <summary>
        /// HTTP方式的GET请求
        /// </summary>
        /// <returns></returns>
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

            webRequest.BeginGetResponse(new AsyncCallback(HandleResponse), webRequest); //直接获取响应

            //清空参数列表
            _parameter.Clear(); 
            _headParameter.Clear();
        }



        /// <summary>
        ///  默认请求
        /// </summary>
        private void DefautRequest() {
            Uri myurl = new Uri(this.RequestUrl);
            HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create(myurl);
            webRequest.UserAgent = "GoKuai_EntSDK";
            if (string.IsNullOrEmpty(ContentType))
            {
                webRequest.ContentType = "application/x-www-form-urlencoded";
            }
            else 
            {
                webRequest.ContentType = ContentType;
            }
            
            webRequest.Method = RequestMethod.ToString();

            if (GetHeadParame() != null)
            {
                webRequest.Headers = GetHeadParame();
            }

            webRequest.BeginGetRequestStream(new AsyncCallback(HandleRequestReady), webRequest);
        
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
            return result;

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
        /// 异步请求回调函数
        /// </summary>
        /// <param name="asyncResult">异步请求参数</param>
        private void HandleRequestReady(IAsyncResult asyncResult)
        {
            HttpWebRequest webRequest = asyncResult.AsyncState as HttpWebRequest;
            using (Stream stream = webRequest.EndGetRequestStream(asyncResult))
            {

                using (StreamWriter writer = new StreamWriter(stream))
                {
                    string parameterstring = this.GetParemeterString();
                    if (parameterstring.Length > 0)
                    {
                        writer.Write(this.GetParemeterString());
                        LogPrint.Print(this.GetParemeterString());
                        writer.Flush();
                    }

                    if (PostDataByte != null)
                    {
                        foreach (byte b in this.PostDataByte)
                        {
                            stream.WriteByte(b);
                        }
                        stream.Flush();
                    }
                    
                    
                }

                
            }

            webRequest.BeginGetResponse(new AsyncCallback(HandleResponse), webRequest);
            //清空参数列表
            _parameter.Clear();
            _headParameter.Clear();
        }

        /// <summary>
        /// 异步响应回调函数
        /// </summary>
        /// <param name="asyncResult">异步请求参数</param>
        private void HandleResponse(IAsyncResult asyncResult) 
        {
            string result = "";
            HttpStatusCode code=HttpStatusCode.NotFound;
            bool iserror = false;
            try
            {
                HttpWebRequest webRequest = asyncResult.AsyncState as HttpWebRequest;
                HttpWebResponse webResponse = (HttpWebResponse)webRequest.EndGetResponse(asyncResult);
                code = webResponse.StatusCode;
                Stream streamResult = webResponse.GetResponseStream(); //获取响应流
                StreamReader reader = new StreamReader(streamResult);
                result = reader.ReadToEnd();

            }
            catch (WebException ex)
            {
                //抓获错误之后页面上的错误信息 流数据
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
                        //UtilDebug.ShowException(e,null,null);
                    }
                code = response.StatusCode;
                iserror = true;
            }
            finally
            {
                HttpEventArgs e = new HttpEventArgs();
                e.IsError = iserror;
                e.Result = result;
                e.StatusCode = code;

                //进行异步回调操作
                SynchronizationContext context = SynchronizationContext.Current;

                new Thread(() =>
                {
                    context.Post((state) =>
                    {
                        OnHttpCompleted(e);
                    }, null);

                }).Start();

                
                
            }
        }

    }

    
}
