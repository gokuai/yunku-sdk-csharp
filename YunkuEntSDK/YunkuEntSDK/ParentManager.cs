using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using YunkuEntSDK.Data;
using YunkuEntSDK.Net;
using YunkuEntSDK.UtilClass;

namespace YunkuEntSDK
{

    public abstract class ParentManager
    {

        const string OAUTH_HOST = HostConfig.OAUTH_HOST;
        const string URL_API_TOKEN = OAUTH_HOST + "/oauth2/token";
        protected string _clientId;
        protected string _clientSecret;
        protected string _username;
        protected string _password;

        protected string _token;
        /// <summary>
        /// 获取到的身份验证token
        /// </summary>
        public string Token
        {
            internal set { _token=value;}
            get { return _token; }
        }

         /// <summary>
        /// 
        /// </summary>
        /// <param name="account">帐号</param>
        /// <param name="password">密码</param>
        /// <param name="key">序列号</param>
        /// <param name="secrectkey">密钥</param>
        public ParentManager(string username, string password, string clientId, string clientSecret) 
        {
            _username = username;
            _password = MD5Core.GetHashString(password);
            _clientId = clientId;
            _clientSecret = clientSecret;

        }

        public HttpStatusCode StatusCode
        {
            set;
            get;
        }


        public string AccessToken(bool isEnt)
        {
            HttpRequestSyn request = new HttpRequestSyn();
            request.RequestUrl = URL_API_TOKEN;
            request.AppendParameter("username", _username);
            request.AppendParameter("password", _password);
            request.AppendParameter("client_id", _clientId);
            request.AppendParameter("client_secret", _clientSecret);
            request.AppendParameter("grant_type", isEnt ? "ent_password" : "password");
            request.RequestMethod = RequestType.POST;
            request.Request();
            this.StatusCode = request.Code;
            string result = request.Result;

            OauthData data = OauthData.Create(result);
            if (request.Code == HttpStatusCode.OK)
            {
                _token = data.Token;
            }
            return result;
        }

        

        /// <summary>
        /// 生成签名
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        protected string GenerateSign(string[] array)
        {
            string string_sign = "";
            for (int i = 0; i < array.Length; i++)
            {
                string_sign += array[i] + (i == array.Length - 1 ? string.Empty : "\n");
            }

            return Uri.EscapeDataString(Util.EncodeToHMACSHA1(string_sign, _clientSecret));
        }


    }
}
