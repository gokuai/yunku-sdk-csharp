using System;
using System.Net;
using YunkuEntSDK.Data;
using YunkuEntSDK.Net;
using YunkuEntSDK.UtilClass;

namespace YunkuEntSDK
{
    public abstract class ParentManager : SignAbility
    {
        private const string OauthHost = HostConfig.OauthHost;
        private const string UrlApiToken = OauthHost + "/oauth2/token2";

        protected string _clientId;
        protected bool _isEnt;
        protected string _tokenType;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="clientId"></param>
        /// <param name="clientSecret"></param>
        internal ParentManager(string clientId, string clientSecret, bool isEnt)
        {
            _clientId = clientId;
            _clientSecret = clientSecret;
            _isEnt = isEnt;
            _tokenType = isEnt ? "ent" : "";
        }

        /// <summary>
        ///     获取到的身份验证token
        /// </summary>
        public string Token { internal set; get; }


        public string AccessToken(string username,string password)
        {
            var request = new HttpRequestSyn {RequestUrl = UrlApiToken};
            request.AppendParameter("username", username);

            string passwordEncode;
            if (username.Contains("/") || username.Contains("\\"))
            {
                passwordEncode = Util.EncodeBase64(password);
            }
            else
            {
                passwordEncode = MD5Core.GetHashString(password);
            }

            request.AppendParameter("password", passwordEncode);
            request.AppendParameter("client_id", _clientId);
            request.AppendParameter("dateline", Util.GetUnixDataline() + "");
            request.AppendParameter("grant_type", _isEnt ? "ent_password" : "password");
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.Post;
            request.Request();
            ReturnResult returnResult = ReturnResult.Create(request.Result);
            string result = ReturnResult.Create(request.Result).Result;

            OauthData data = OauthData.Create(result);
            if (returnResult.Code == (int) HttpStatusCode.OK)
            {
                Token = data.Token;
            }
            return result;
        }
    }
}