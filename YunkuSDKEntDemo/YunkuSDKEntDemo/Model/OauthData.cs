using YunkuEntSDK.Data;
using YunkuEntSDK.Net;
using YunkuEntSDK.UtilClass;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YunkuSDKEntDemo.Model
{
    public class OauthData
    {

        private const string LOG_TAG="OauthData";

        private const string KEY_ACCESS_TOKEN = "access_token";
        private const string KEY_EXPIRES_IN = "expires_in";
        private const string KEY_ERROR = "error";

        /// <summary>
        /// 登陆token
        /// </summary>
        public string Token
        {
            get;
            private set;
        }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string Error
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

        /// <summary>
        /// 解析认证jsonstring
        /// </summary>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public static OauthData Create(string jsonString)
        {
            if (jsonString == null) return null;

            OauthData data = new OauthData();
            try
            {
                var json = (IDictionary<string, object>)SimpleJson.DeserializeObject(jsonString);

                string msg = SimpleJson.TryStringValue(json, KEY_ERROR);
                data.Error = OauthErrMsg.ConvertMsg(msg);//转化错误信息

                data.Expires = SimpleJson.TryIntValue(json, KEY_EXPIRES_IN);
                data.Token = SimpleJson.TryStringValue(json, KEY_ACCESS_TOKEN);
                return data;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(LOG_TAG + "==>" + ex.StackTrace);
                return null;

            }
        }
    }
}
