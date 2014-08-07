using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YunkuEntSDK.Data
{
    public class OauthErrMsg
    {
        private const string ERRORRESPONES_INVALID_REQUEST = "invalid_request";
        private const string ERRORRESPONES_INVALID_CLIENT = "invalid_client";
        private const string ERRORRESPONES_INVALID_GRANT = "invalid_grant";
        private const string ERRORRESPONES_UNAUTHORIED_CLIENT = "unauthorized_client";
        private const string ERRORRESPONES_ACCESS_DENIED = "access_denied";

        /// <summary>
        /// 
        /// 默认返回：请求错误，请重试
        /// 
        /// invalid_request 请求参数错误
        /// invalid_client 当前客户端版本已不能使用
        /// invalid_grant 邮箱或密码错误
        /// unauthorized_client 该设备已限制使用
        /// access_denied 您的客户端已被限制登录
        /// 
        /// 
        /// </summary>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public static string ConvertMsg(string errorMsg)
        {
            if (errorMsg.Equals(ERRORRESPONES_INVALID_REQUEST))
            {
                return "请求参数错误";
            }
            else if (errorMsg.Equals(ERRORRESPONES_INVALID_CLIENT))
            {
                return "当前客户端版本已不能使用";
            }
            else if (errorMsg.Equals(ERRORRESPONES_INVALID_GRANT))
            {
                return "邮箱或密码错误";
            }
            else if (errorMsg.Equals(ERRORRESPONES_UNAUTHORIED_CLIENT))
            {
                return "该设备已限制使用";
            }
            else if (errorMsg.Equals(ERRORRESPONES_ACCESS_DENIED))
            {
                return "您的客户端已被限制登录";
            }
            return "请求错误，请重试";
        }
    }
}
