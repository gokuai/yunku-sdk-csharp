using System.Collections.Generic;
using YunkuEntSDK.UtilClass;

namespace YunkuEntSDK
{
    public abstract class SignAbility
    {
        static List<string> IGNORE_KEYS = new List<string>() { "filehash", "filesize", "sign" };
        protected string _clientId;
        protected string _secret;

        public SignAbility(string clientId, string secret)
        {
            this._clientId = clientId;
            this._secret = secret;
        }

        public string GetClientId()
        {
            return this._clientId;
        }

        public string GetSecret()
        {
            return this._secret;
        }

        /// <summary>
        ///  生成签名
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        public string GenerateSign(IDictionary<string, string> parameter, string secret)
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
                    if (IGNORE_KEYS.Contains(key))
                    {
                        continue;
                    }

                    stringSign += parameter[key] + "\n";
                }
                stringSign += parameter[myList[size - 1].Key];
            }
            return Util.EncodeToHMACSHA1(stringSign, secret);
        }

        public string GenerateSign(IDictionary<string, string> parameter)
        {
            return this.GenerateSign(parameter, this._secret);
        }
    }
}