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
    public class OauthEngine : HttpEngine
    {
        private const string Log_Tag = "OauthEngine_V2";

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

        internal OauthEngine(string clientId, string clientSecret, bool isEnt, string token) : this(clientId, clientSecret, isEnt)
        {
            Token = token;
        }

        /// <summary>
        ///     获取到的身份验证token
        /// </summary>
        public string Token { internal set; get; }


        /// <summary>
        /// 使用帐号用户名获取token
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public string AccessToken(string username, string password)
        {
            string url = UrlApiToken;
            var parameter = new Dictionary<string, string>();
            parameter.Add("username", username);
            string passwordEncode;
            if (username.IndexOf("/") > 0 || username.IndexOf("\\") > 0)
            {
                passwordEncode = Util.EncodeBase64(password);
            }
            else
            {
                passwordEncode = MD5Core.GetHashString(password);
            }
            parameter.Add("password", passwordEncode);
            parameter.Add("client_id", _clientId);
            parameter.Add("grant_type", _isEnt ? "ent_password" : "password");
            parameter.Add("dateline", Util.GetUnixDataline() + "");
            parameter.Add("sign", GenerateSign(parameter));

            string result = new RequestHelper().SetParams(parameter).SetUrl(url).SetMethod(RequestType.Post).ExecuteSync();
            ReturnResult returnResult = ReturnResult.Create(result);
            LogPrint.Print(Log_Tag + "==>accessToken:==>result:" + result);

            if (returnResult.Code == (int)HttpStatusCode.OK)
            {
                LogPrint.Print(Log_Tag + "==>accessToken:==>StatusCode:200");
                OauthData data = OauthData.Create(returnResult.Result);
                Token = data.Token;
            }
            return result;
        }

        protected void AddAuthParams(Dictionary<string, string> parameter)
        {
            parameter.Add("token", Token);
            parameter.Add("token_type", _tokenType);
        }
    }
}
