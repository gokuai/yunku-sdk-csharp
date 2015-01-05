using System;
using System.Net;
using YunkuEntSDK.Data;
using YunkuEntSDK.Net;
using YunkuEntSDK.UtilClass;

namespace YunkuEntSDK
{
    public abstract class ParentManager:SignAbility
    {
        private const string OauthHost = HostConfig.OauthHost;
        private const string UrlApiToken = OauthHost + "/oauth2/token";

        private string _passsword;
        private string _username;
        private string _clientId;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <param name="clientId"></param>
    /// <param name="clientSecret"></param>
        internal ParentManager(string username, string password, string clientId, string clientSecret)
        {
            _username = username;
            _passsword = MD5Core.GetHashString(password);
            _clientId = clientId;
            ClientSecret = clientSecret;
        }

        /// <summary>
        ///     获取到的身份验证token
        /// </summary>
        public  string Token { internal  set; get; }

        public HttpStatusCode StatusCode {  set;  get; }


        public string AccessToken(bool isEnt)
        {
            var request = new HttpRequestSyn();
            request.RequestUrl = UrlApiToken;
            request.AppendParameter("username", _username);
            request.AppendParameter("password", _passsword);
            request.AppendParameter("client_id", _clientId);
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

        


    }
}