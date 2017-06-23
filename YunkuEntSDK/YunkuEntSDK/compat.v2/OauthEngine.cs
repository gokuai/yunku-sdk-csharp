using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using YunkuEntSDK.Data;
using YunkuEntSDK.Net;
using YunkuEntSDK.UtilClass;

namespace YunkuEntSDK.compat.v2
{
    class OauthEngine : HttpEngine
    {
        private const string OauthHost = HostConfig.OauthHostV2;
        private const string UrlApiToken = OauthHost + "/oauth2/token2";

        protected bool _isEnt;
        protected string _tokenType;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="clientId"></param>
        /// <param name="clientSecret"></param>
        internal OauthEngine(string clientId, string clientSecret, bool isEnt) : base(clientId, clientSecret)
        {
            _isEnt = isEnt;
            _tokenType = isEnt ? "ent" : "";
        }

        /// <summary>
        ///     获取到的身份验证token
        /// </summary>
        public string Token { internal set; get; }


        public string AccessToken(string username, string password)
        {
            var request = new HttpRequestSyn { RequestUrl = UrlApiToken };
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
            LogPrint.Print("accessToken:==>result:" + result);

            if (returnResult.Code == (int)HttpStatusCode.OK)
            {
                LogPrint.Print("accessToken:==>StatusCode:200");
                OauthData data = OauthData.Create(result);
                Token = data.Token;
            }
            return result;
        }

        protected void AddAuthParams(HttpRequestSyn request)
        {
            request.AppendParameter("token", Token);
            request.AppendParameter("token_type", _tokenType);
        }
    }
}
