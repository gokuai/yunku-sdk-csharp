using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YunkuEntSDK.UtilClass;

namespace YunkuEntSDK
{

    internal class ParentManager
    {

        protected string _clientId;
        protected string _clientSecret;
        protected string _username;
        protected string _password;

        protected string _token;
        /// <summary>
        /// 获取到的身份验证token
        /// </summary>
        public string Token
        {
            internal set { _token=value;}
            get { return _token; }
        }

         /// <summary>
        /// 
        /// </summary>
        /// <param name="account">帐号</param>
        /// <param name="password">密码</param>
        /// <param name="key">序列号</param>
        /// <param name="secrectkey">密钥</param>
        public ParentManager(string username, string password, string clientId, string clientSecret) 
        {
            _username = username;
            _password = MD5Core.GetHashString(password);
            _clientId = clientId;
            _clientSecret = clientSecret;

        }

        

        /// <summary>
        /// 生成签名
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        protected string GenerateSign(string[] array)
        {
            string string_sign = "";
            for (int i = 0; i < array.Length; i++)
            {
                string_sign += array[i] + (i == array.Length - 1 ? string.Empty : "\n");
            }

            return Uri.EscapeDataString(Util.EncodeToHMACSHA1(string_sign, _clientSecret));
        }

        /// <summary>
        /// 生成签名
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        protected string GenerateSign(string[] array, string orgClientSecret)
        {
            string string_sign = "";
            for (int i = 0; i < array.Length; i++)
            {
                string_sign += array[i] + (i == array.Length - 1 ? string.Empty : "\n");
            }

            return Uri.EscapeDataString(Util.EncodeToHMACSHA1(string_sign, orgClientSecret));
        }
    }
}
