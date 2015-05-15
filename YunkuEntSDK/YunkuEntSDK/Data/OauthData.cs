using System;
using System.Collections.Generic;
using YunkuEntSDK.UtilClass;

namespace YunkuEntSDK.Data
{
    class OauthData : BaseData
    {

        private const string LogTag="OauthData";

        private const string KeyAccessToken = "access_token";
        private const string KeyExpiresIn = "expires_in";
        private const string KeyError= "error";


        /// <summary>
        /// 登陆token
        /// </summary>
        public string Token
        {
            get;
            private set;
        }

        /// <summary>
        /// 用于重新获取token的参数
        /// </summary>
        public string RefreshToken
        {
            get;
            private set;
        }

        public int Expires
        {
            get;
            private set;
        }


        new public static OauthData Create(string jsonString)
        {
            if (jsonString == null) return null;

            OauthData data = new OauthData();
            try
            {
                var json = (IDictionary<string, object>)SimpleJson.DeserializeObject(jsonString);
                data.ErrorCode = SimpleJson.TryIntValue(json, KeyErrorCode);
                data.ErrorMessage = SimpleJson.TryStringValue(json, KeyError);
                data.ErrorMessage = SimpleJson.TryStringValue(json, KeyErrorMsg);

                data.Expires = SimpleJson.TryIntValue(json, KeyExpiresIn);
                data.Token = SimpleJson.TryStringValue(json, KeyAccessToken);
                return data;

            }
            catch (Exception ex)
            {
                LogPrint.Print(LogTag + ":" + ex.StackTrace);
                return null;

            }
        }
    }
}
