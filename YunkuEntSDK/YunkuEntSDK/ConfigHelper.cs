using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YunkuEntSDK
{
    public class ConfigHelper
    {
        /// <summary>
        /// 设置API服务器
        /// </summary>
        /// <param name="host"></param>
        public static void SetApiHost(string host)
        {
            Config.ApiHost = host;
        }

        /// <summary>
        /// 设置WEB服务器
        /// </summary>
        /// <param name="host"></param>
        public static void SetWebHost(string host)
        {
            Config.WebHost = host;
        }

        /// <summary>
        /// 设置分块上传时每块大小，默认10MB
        /// </summary>
        /// <param name="byteSize">分块大小，单位字节</param>
        public static void SetUploadBlockSize(int byteSize)
        {
            Config.BlockSize = byteSize;
        }

        /// <summary>
        /// 设置请求的语言环境，默认中文"zh-cn"
        /// </summary>
        /// <param name="language">语言环境</param>
        public static void SetLanguage(string language)
        {
            Config.Language = language;
        }

        /// <summary>
        /// 设置HTTP请求的User-Agent
        /// </summary>
        /// <param name="ua"></param>
        public static void SetUserAgent(string ua)
        {
            Config.UserAgent = ua;
        }

        /// <summary>
        /// HTTP请求超时时间，默认1小时
        /// </summary>
        /// <param name="seconds">超时时间，单位秒</param>
        public static void SetHttpTimeout(int seconds)
        {
            Config.HttpTimeout = seconds * 1000;
        }
    }
}
