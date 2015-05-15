using System;
using System.Collections.Generic;
using System.Net;
using YunkuEntSDK.UtilClass;

namespace YunkuEntSDK.Data
{
    class FileOperationData:BaseData
    {
        private const string LogTag = "FileOperationData";
        public const int StateNoupload = 1;

        public const string KeyState = "state";
        public const string KeyHash = "hash";
        public const string KeyVersion = "version";
        public const string KeyServer = "server";

        public int Code { get; private set; }

        public int Status { get; set; }
        public string UuidHash { get; set; }
        public int FileVersion { get; private set; }
        public string Server { get; private set; }
        public string FileHash { get; private set; }


        public static FileOperationData Create(string jsonString,int code)
        {
            if (jsonString == null) return null;

            FileOperationData data = new FileOperationData();
            try
            {
                var json = (IDictionary<string, object>)SimpleJson.DeserializeObject(jsonString);
                data.Code = code;
                if (code == (int) HttpStatusCode.OK)
                {
                    data.Server = SimpleJson.TryStringValue(json, KeyServer);
                    data.UuidHash = SimpleJson.TryStringValue(json, KeyHash);
                    data.Status = SimpleJson.TryIntValue(json, KeyState);
                    data.FileVersion = SimpleJson.TryIntValue(json, KeyVersion);
                }
                else
                {
                    data.ErrorCode = SimpleJson.TryIntValue(json, KeyErrorCode);
                    data.ErrorMessage = SimpleJson.TryStringValue(json, KeyErrorMsg);
                }

                return data;

            }
            catch (Exception ex)
            {
                LogPrint.Print(LogTag + ":" + ex.StackTrace);
                return null;

            }
        }
    }


}
