using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using YunkuEntSDK.Net;
using YunkuEntSDK.UtilClass;

namespace YunkuEntSDK
{
    public class EntFileManager
    {
        const string LIB_HOST = HostConfig.LIB_HOST;
        const string URL_API_FILELIST = LIB_HOST + "/1/file/ls";
        const string URL_API_UPDATE_LIST = LIB_HOST + "/1/file/updates";
        const string URL_API_FILE_INFO = LIB_HOST + "/1/file/info";
        const string URL_API_CREATE_FOLDER = LIB_HOST + "/1/file/create_folder";
        const string URL_API_CREATE_FILE = LIB_HOST + "/1/file/create_file";
        const string URL_API_DEL_FILE = LIB_HOST + "/1/file/del";
        const string URL_API_MOVE_FILE = LIB_HOST + "/1/file/move";
        const string URL_API_LINK_FILE = LIB_HOST + "/1/file/link";
        const string URL_API_SENDMSG = LIB_HOST + "/1/file/sendmsg";

        private string _orgClientId, _orgClientSecret;

        public EntFileManager(string orgClientId, string orgClientSecret)
        {
            _orgClientId = orgClientId;
            _orgClientSecret = orgClientSecret;
        }

        public HttpStatusCode StatusCode
        {
            get;
            set;
        }

        public string GetFileList(int dateline, int start, string fullPath)
        {
            HttpRequestSyn request = new HttpRequestSyn();
            request.RequestUrl = URL_API_FILELIST;
            request.AppendParameter("org_client_id", _orgClientId);
            request.AppendParameter("dateline", dateline + "");
            request.AppendParameter("start", start + "");
            request.AppendParameter("fullpath", fullPath);
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.GET;
            request.Request();
            this.StatusCode = request.Code;
            return request.Result;
        }

        public string GetUpdateList(int dateline, bool isCompare, long fetchDateline)
        {
            HttpRequestSyn request = new HttpRequestSyn();
            request.RequestUrl = URL_API_FILELIST;
            request.AppendParameter("org_client_id", _orgClientId);
            request.AppendParameter("dateline", dateline + "");
            if (isCompare)
            {
                request.AppendParameter("mode", "compare");
            }
            request.AppendParameter("fetch_dateline", fetchDateline + "");
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.GET;
            request.Request();
            this.StatusCode = request.Code;
            return request.Result;
        }

        public string GetFileInfo(int dateline, string fullPath)
        {
            HttpRequestSyn request = new HttpRequestSyn();
            request.RequestUrl = URL_API_FILE_INFO;
            request.AppendParameter("org_client_id", _orgClientId);
            request.AppendParameter("dateline", dateline + "");
            request.AppendParameter("fullpath", fullPath);
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.GET;
            request.Request();
            this.StatusCode = request.Code;
            return request.Result;
        }

        public string CreateFolder(int dateline, string fullPath, string opName)
        {
            HttpRequestSyn request = new HttpRequestSyn();
            request.RequestUrl = URL_API_CREATE_FOLDER;
            request.AppendParameter("org_client_id", _orgClientId);
            request.AppendParameter("dateline", dateline + "");
            request.AppendParameter("fullpath", fullPath);
            request.AppendParameter("op_name", opName);
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.POST;
            request.Request();
            this.StatusCode = request.Code;
            return request.Result;
        }

        public string CreateFile(int dateline, string fullPath, string opName, System.IO.Stream stream, string fileName)
        {
            if (stream.Length > 51200)
            {
                LogPrint.Print("文件大小超过50MB");
                return "";
            }

            HttpRequestSyn request = new HttpRequestSyn();
            request.RequestUrl = URL_API_CREATE_FILE;
            string[] arr = new string[] { dateline + "", "file", fullPath, opName, _orgClientId };
            MsMultiPartFormData data = new MsMultiPartFormData();
            request.ContentType = "multipart/form-data;boundary=" + data.Boundary;
            data.AddStreamFile("file", fileName, Util.ReadToEnd(stream));
            data.AddParams("org_client_id", _orgClientSecret);
            data.AddParams("dateline", dateline + "");
            data.AddParams("fullpath", fullPath);
            data.AddParams("op_name", opName);
            data.AddParams("filefield", "file");
            data.AddParams("sign", GenerateSign(arr));
            data.PrepareFormData();
            request.PostDataByte = data.GetFormData();
            request.RequestMethod = RequestType.POST;
            LogPrint.Print("------------->Begin to Upload<------------------");
            request.Request();
            LogPrint.Print("--------------------->Upload Request Compeleted<--------------");
            this.StatusCode = request.Code;
            return request.Result;
        }

        public string CreateFile(int dateline, string fullPath, string opName, string localPath)
        {
            using (FileStream FS = new FileStream(localPath, FileMode.Open))
            {
                Stream stream = FS;
                if (stream != null)
                {
                    return CreateFile(dateline, fullPath, opName, stream, Util.GetFileNameFromPath(localPath));
                }
                else
                {
                    LogPrint.Print("file not exist");
                }

            }
            return "";
        }

        public string Del(int dateline, string fullPath, string opName)
        {
            HttpRequestSyn request = new HttpRequestSyn();
            request.RequestUrl = URL_API_DEL_FILE;
            request.AppendParameter("org_client_id", _orgClientId);
            request.AppendParameter("dateline", dateline + "");
            request.AppendParameter("fullpath", fullPath);
            request.AppendParameter("op_name", opName);
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.POST;
            request.Request();
            this.StatusCode = request.Code;
            return request.Result;
        }

        public string Move(int dateline, string fullPath, string destFullPath, string opName)
        {
            HttpRequestSyn request = new HttpRequestSyn();
            request.RequestUrl = URL_API_MOVE_FILE;
            request.AppendParameter("org_client_id", _orgClientId);
            request.AppendParameter("dateline", dateline + "");
            request.AppendParameter("fullpath", fullPath);
            request.AppendParameter("dest_fullpath", destFullPath);
            request.AppendParameter("op_name", opName);
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.POST;
            request.Request();
            this.StatusCode = request.Code;
            return request.Result;
        }

        public string Link(int dateline, string fullPath)
        {
            HttpRequestSyn request = new HttpRequestSyn();
            request.RequestUrl = URL_API_LINK_FILE;
            request.AppendParameter("org_client_id", _orgClientId);
            request.AppendParameter("dateline", dateline + "");
            request.AppendParameter("fullpath", fullPath);
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.POST;
            request.Request();
            this.StatusCode = request.Code;
            return request.Result;
        }

        public string SendMsg(int dateline, string title, string text, string image, string linkUrl, string opName)
        {
            HttpRequestSyn request = new HttpRequestSyn();
            request.RequestUrl = URL_API_SENDMSG;
            request.AppendParameter("org_client_id", _orgClientId);
            request.AppendParameter("dateline", dateline + "");
            request.AppendParameter("title", title);
            request.AppendParameter("text", text);
            request.AppendParameter("image", image);
            request.AppendParameter("url", linkUrl);
            request.AppendParameter("op_name", opName);
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.POST;
            request.Request();
            this.StatusCode = request.Code;
            return request.Result;
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

            return Uri.EscapeDataString(Util.EncodeToHMACSHA1(string_sign, _orgClientSecret));
        }

    }
}
