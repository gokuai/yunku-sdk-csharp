using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using YunkuEntSDK.UtilClass;

namespace YunkuSDKEntDemo.Model
{
   /// <summary>
   /// 网络返回数据
   /// </summary>
    public class BaseData
    {
        private const string LOG_TAG = "BaseData";
        protected const string KEY_ERROR_CODE = "error_code";
        protected const string KEY_ERROR_MSG = "error_msg";
        
        /// <summary>
        /// 错误码
        /// </summary>
        public int ErrorCode
        {
             get;
            protected set;
        }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMessage
        {
             get;
            protected set;
        }


        public static BaseData Create(string jsonString)
        {
            if (jsonString == null) return null;

            BaseData data = new BaseData();
            try
            {
                var json =  (IDictionary<string, object>)SimpleJson.DeserializeObject(jsonString);
                data.ErrorCode = SimpleJson.TryIntValue(json, KEY_ERROR_CODE);
                
                data.ErrorMessage = SimpleJson.TryStringValue(json, KEY_ERROR_MSG);
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
