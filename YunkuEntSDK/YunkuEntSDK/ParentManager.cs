using System;
using System.Net;
using YunkuEntSDK.Data;
using YunkuEntSDK.Net;
using YunkuEntSDK.UtilClass;

namespace YunkuEntSDK
{
    public abstract class ParentManager
    {
        private const string OauthHost = HostConfig.OauthHost;
        private const string UrlApiToken = OauthHost + "/oauth2/token";
        protected string ClientId;
        protected string ClientSecret;
        protected string Password;

        protected string Username;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <param name="clientId"></param>
    /// <param name="clientSecret"></param>
        public ParentManager(string username, string password, string clientId, string clientSecret)
        {
            Username = username;
            Password = MD5Core.GetHashString(password);
            ClientId = clientId;
            ClientSecret = clientSecret;
        }

        /// <summary>
        ///     获取到的身份验证token
        /// </summary>
        public  string Token { internal set; get; }

        public HttpStatusCode StatusCode { internal set;  get; }


        public string AccessToken(bool isEnt)
        {
            var request = new HttpRequestSyn();
            request.RequestUrl = UrlApiToken;
            request.AppendParameter("username", Username);
            request.AppendParameter("password", Password);
            request.AppendParameter("client_id", ClientId);
            request.AppendParameter("client_secret", ClientSecret);
            request.AppendParameter("grant_type", isEnt ? "ent_password" : "password");
            request.RequestMethod = RequestType.Post;
            request.Request();
            StatusCode = request.Code;
            string result = request.Result;

            OauthData data = OauthData.Create(result);
            if (request.Code == HttpStatusCode.OK)
            {
                Token = data.Token;
            }
            return result;
        }


        /// <summary>
        ///     生成签名
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        protected string GenerateSign(string[] array)
        {
            string stringSign = "";
            for (int i = 0; i < array.Length; i++)
            {
                stringSign += array[i] + (i == array.Length - 1 ? string.Empty : "\n");
            }

            return Uri.EscapeDataString(Util.EncodeToHMACSHA1(stringSign, ClientSecret));
        }
    }
}