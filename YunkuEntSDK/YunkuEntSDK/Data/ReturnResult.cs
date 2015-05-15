using System.Collections.Generic;
using YunkuEntSDK.UtilClass;

namespace YunkuEntSDK.Data
{
    public class ReturnResult
    {
        private const string KeyCode = "Code";
        private const string KeyResult = "Result";
        public string Result { get; set; }

        public int Code { get; set; }


        public string ToJsonString()
        {
            return SimpleJson.SerializeObject(this);

        }

        public static ReturnResult Create(string jsonString)
        {
            var json = (IDictionary<string, object>)SimpleJson.DeserializeObject(jsonString);
            var statusCode = SimpleJson.TryIntValue(json, KeyCode);
            string result = SimpleJson.TryStringValue(json, KeyResult);

            return new ReturnResult()
            {
                Code = statusCode,
                Result = result
            };
        }
    }
}
