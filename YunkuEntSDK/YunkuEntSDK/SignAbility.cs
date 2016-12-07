using System;
using YunkuEntSDK.UtilClass;

namespace YunkuEntSDK
{
    public abstract class SignAbility
    {
        internal SignAbility()
        {
        
        }

        protected string ClientSecret;
        /// <summary>
        ///     生成签名
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        protected string GenerateSign(string[] array)
        {
            string stringSign = "";
            for (int i = 0; i < array.Length; i++)
            {
                stringSign += array[i] + (i == array.Length - 1 ? string.Empty : "\n");
            }

            return Util.EncodeToHMACSHA1(stringSign, ClientSecret);
        }
    }
}
