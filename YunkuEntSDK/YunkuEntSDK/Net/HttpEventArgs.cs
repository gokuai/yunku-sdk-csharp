using System;
using System.Net;

namespace YunkuEntSDK.Net
{
    /// <summary>
    /// Http请求参数类 unused
    /// </summary>
    public class HttpEventArgs : EventArgs
    {
        #region 私有成员

        private string _result;
        private bool _is_error;

        #endregion

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public HttpEventArgs()
        {
            this.Result = "";
            this.IsError = false;
        }

        //public WP7HttpEventArgs(string result)
        //{
        //    this.Result = result;
        //    this.IsError = false;
        //}

        /// <summary>
        /// 结果字符串
        /// 如果是接口报的错误或者正确数据,格式为json
        /// </summary>
        public string Result
        {
            get { return _result; }
            set { _result = value; }
        }

        private HttpStatusCode _code;
        public HttpStatusCode StatusCode
        {
            get { return _code; }
            set { _code = value; }

        }


        ///// <summary>
        /////  提供给文件列表、更新列表、收藏列表
        ///// </summary>
        //public bool IsRefreshRequest
        //{
        //    set;
        //    get;
        //}

        /// <summary>
        /// 是否错误
        /// </summary>
        public bool IsError
        {
            get { return _is_error; }
            set { _is_error = value; }
        }
    }
}
