using System.Collections.Generic;
using System.Net;
using YunkuEntSDK.Data;
using YunkuEntSDK.Net;
using YunkuEntSDK.UtilClass;

namespace YunkuEntSDK
{
    public abstract class OauthEngine : HttpEngine, IAuthRequest
    {
        private const string Log_Tag = "OauthEngine";

        private static string OauthHost = Config.WebHost;
        private static string UrlApiToken = OauthHost + "/oauth2/token2";
        protected string _tokenType;
        protected string _refreshToken;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="clientId"></param>
        /// <param name="secret"></param>
        internal OauthEngine(string clientId, string secret) : base(clientId, secret)
        {
        }

        internal OauthEngine(string clientId, string secret, string token) : base(clientId, secret)
        {
            Token = token;
        }

        /// <summary>
        ///     获取到的身份验证token
        /// </summary>
        public string Token { internal set; get; }


        public ReturnResult AccessToken(string username, string password)
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
            parameter.Add("grant_type", "password");
            ReturnResult result = new RequestHelper(this).SetParams(parameter).SetUrl(url).SetMethod(RequestType.POST).ExecuteSync();
            LogPrint.Print(Log_Tag + "==>accessToken:==>result:" + result.ToJsonString());

            if (result.Code == (int)HttpStatusCode.OK)
            {
                LogPrint.Print(Log_Tag + "==>accessToken:==>StatusCode:200");
                OauthData data = OauthData.Create(result.Body);
                Token = data.Token;
            }
            return result;
        }

        /// <summary>
        /// 添加认证参数
        /// </summary>
        /// <param name="parameter"></param>
        internal void AddAuthParams(IDictionary<string, string> parameter)
        {
            if (Token == null)
            {
                parameter.Add("client_id", _clientId);
                parameter.Add("dateline", Util.GetUnixDataline().ToString());
            }
            else
            {
                parameter.Add("token", Token);
                parameter.Add("token_type", _tokenType);
            }
        }

        /// <summary>
        /// 重新获得 token
        /// </summary>
        /// <returns></returns>
        private bool RefreshToken()
        {
            if (string.IsNullOrEmpty(_refreshToken))
            {
                return false;
            }
            string url = UrlApiToken;
            var parameter = new Dictionary<string, string>();
            parameter.Add("grant_type", "refresh_token");
            parameter.Add("refresh_token", _refreshToken);
            ReturnResult result = new RequestHelper(this).SetParams(parameter).SetUrl(url).SetMethod(RequestType.POST).ExecuteSync();
            OauthData data = OauthData.Create(result.Body);
            if (data != null)
            {
                data.Code = result.Code;
                if (data.Code == (int)HttpStatusCode.OK)
                {
                    Token = data.Token;
                    _refreshToken = data.RefreshToken;
                    return true;
                }
                LogPrint.Print(Log_Tag + "==>token:" + Token + "==>refreshToken:" + _refreshToken);
            }
            return false;
        }

        /// <summary>
        /// 重新进行签名
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="ignoreKeys"></param>
        private void ReSignParams(IDictionary<string, string> parameter)
        {
            ReSignParams(parameter, _secret);
        }

        /// <summary>
        /// 重新根据参数进行签名
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="secret"></param>
        /// <param name="ignoreKeys"></param>
        protected void ReSignParams(IDictionary<string, string> parameter, string secret)
        {
            parameter.Add("token", Token);
            parameter.Add("sign", GenerateSign(parameter, secret));
        }

        ReturnResult IAuthRequest.SendRequestWithAuth(string url, RequestType method, IDictionary<string, string> parameter, IDictionary<string, string> headParameter)
        {
            var request = new HttpRequest(url);
            request.SetParameters(parameter);
            request.SetHeaders(headParameter);
            request.Method = method;
            ReturnResult result = request.Request();
            
            if (result.Code == (int)HttpStatusCode.Unauthorized)
            {
                RefreshToken();
                ReSignParams(parameter);

                var requestAgain = new HttpRequest(url);
                requestAgain.SetParameters(parameter);
                requestAgain.SetHeaders(headParameter);
                requestAgain.Method = method;
                result = requestAgain.Request();
            }
            return result;
        }
    }
}