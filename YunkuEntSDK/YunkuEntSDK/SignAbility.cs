using System;
using System.Collections.Generic;
using YunkuEntSDK.UtilClass;

namespace YunkuEntSDK
{
    public abstract class SignAbility
    {
        internal SignAbility()
        {

        }

        /// <summary>
        ///  生成签名
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        protected string GenerateSign(Dictionary<string, string> parameter, string secret)
        {
            return GenerateSign(parameter, secret, new List<string>());
        }

        protected string GenerateSign(Dictionary<string, string> parameter, string secret, List<string> ignoreKeys)
        {
            var myList = new List<KeyValuePair<string, string>>();
            foreach (var keys in parameter)
            {
                if (keys.Value == null)
                {
                    continue;
                }
                myList.Add(keys);
            }
            myList.Sort((firstPair, nextPair) => { return firstPair.Key.CompareTo(nextPair.Key); });
            int size = myList.Count;
            string stringSign = "";

            if (size > 0)
            {
                for (int i = 0; i < size - 1; i++)
                {
                    string key = myList[i].Key;
                    if (ignoreKeys != null && ignoreKeys.Contains(key))
                    {
                        continue;
                    }

                    stringSign += parameter[key] + "\n";
                }
                stringSign += parameter[myList[size - 1].Key];
            }
            return Util.EncodeToHMACSHA1(stringSign, secret);
        }
    }
}