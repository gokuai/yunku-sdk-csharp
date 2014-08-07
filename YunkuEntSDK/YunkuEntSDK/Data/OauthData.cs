using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YunkuEntSDK.UtilClass;

namespace YunkuEntSDK.Data
{
    class OauthData : BaseData
    {

        private const string LOG_TAG="OauthData";

        private const string KEY_ACCESS_TOKEN = "access_token";
        private const string KEY_EXPIRES_IN = "expires_in";
        private const string KEY_REFRESH_TOKEN = "refresh_token";
        private const string KEY_ERROR= "error";


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
                data.ErrorCode = SimpleJson.TryIntValue(json, KEY_ERROR_CODE);
                data.ErrorMessage = SimpleJson.TryStringValue(json, KEY_ERROR);
                data.ErrorMessage = SimpleJson.TryStringValue(json, KEY_ERROR_MSG);

                data.Expires = SimpleJson.TryIntValue(json, KEY_EXPIRES_IN);
                data.Token = SimpleJson.TryStringValue(json, KEY_ACCESS_TOKEN);
                data.RefreshToken = SimpleJson.TryStringValue(json, KEY_REFRESH_TOKEN);
                return data;

            }
            catch (Exception ex)
            {
                LogPrint.Print(LOG_TAG + ":" + ex.StackTrace);
                return null;

            }
        }
    }
}
