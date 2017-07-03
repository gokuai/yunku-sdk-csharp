using System;
using System.Collections.Generic;
using System.Net;
using YunkuEntSDK.Data;
using YunkuEntSDK.Net;
using YunkuEntSDK.UtilClass;

namespace YunkuEntSDK
{
    public abstract class OauthEngine : HttpEngine
    {
        private const string Log_Tag = "OauthEngine";

        private const string OauthHost = HostConfig.OauthHost;
        private const string UrlApiToken = OauthHost + "/oauth2/token2";


        protected bool _isEnt;
        protected string _tokenType;
        protected string _refreshToken;

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

        /// <summary>
        /// 添加认证参数
        /// </summary>
        /// <param name="request"></param>
        internal void AddAuthParams(HttpRequestSyn request)
        {
            if (Token == null)
            {
                request.AppendParameter("client_id", _clientId);
                request.AppendParameter("dateline", Util.GetUnixDataline() + "");
            }
            else
            {
                request.AppendParameter("token", Token);
                request.AppendParameter("token_type", _tokenType);
            }
        }

        internal void AddAuthParams(Dictionary<string, string> parameter)
        {
            if (Token == null)
            {
                parameter.Add("client_id", _clientId);
                parameter.Add("dateline", Util.GetUnixDataline() + "");
            }
            else
            {
                parameter.Add("token", Token);
                parameter.Add("token_type", _tokenType);
            }
        }

        /// <summary>
        /// 使用第三方API OUTID 登录
        /// </summary>
        /// <param name="outId"></param>
        /// <returns></returns>
        public string AccessTokenWithThirdPartyOutId(string outId)
        {
            return new ThirdPartyManager(_clientId, _clientSecret, outId).GetEntToken();
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
            parameter.Add("client_id", _clientId);
            parameter.Add("sign", GenerateSign(parameter));

            string result = new RequestHelper().SetParams(parameter).SetUrl(url).SetMethod(RequestType.Post).ExecuteSync();
            ReturnResult returnResult = ReturnResult.Create(result);
            OauthData data = OauthData.Create(returnResult.Result);
            if(data != null)
            {
                data.Code = returnResult.Code;
                if(data.Code == (int)HttpStatusCode.OK)
                {
                    Token = data.Token;
                    _refreshToken = data.RefreshToken;
                    return true;
                }
                LogPrint.Print(Log_Tag + "==>token:" + Token + "==>refreshToken:" + _refreshToken);
            }
            return false;
        }

        //异步请求待解决

        //internal string SendRequestWithAuth(string url, RequestType requestType, Dictionary<string, string> parameter,
        //    Dictionary<string, string> headParameter, List<string> ignoreKeys)
        //{
        //    string returnString = new RequestHelper().SetParams(parameter).SetHeadParams(headParameter).SetUrl(url).SetMethod(RequestType.Post).ExecuteSync();

        //    var request = new HttpRequestAsync { RequestUrl = url };
        //    request.RequestMethod = requestType;
        //    request.Request();

        //    ReturnResult returnResult = ReturnResult.Create(request.Result);
        //    string returnString = ReturnResult.Create(returnResult.Result).Result;
        //    if (returnResult.Code == (int)HttpStatusCode.Unauthorized)
        //    {
        //        RefreshToken();
        //        request.RemoveParamter();
        //        request.AppendParameter("token", Token);
        //        request.AppendParameter("sign", GenerateSign(request.SortedParamter, _clientSecret));
        //    }
        //}
    }
}