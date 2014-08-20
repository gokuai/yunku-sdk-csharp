using System;
using System.IO;
using System.Net;
using YunkuEntSDK.Net;
using YunkuEntSDK.UtilClass;

namespace YunkuEntSDK
{
    public class EntFileManager
    {
        private const long UploadSizeLimit = 52428800;//50MB
        private const string LibHost = HostConfig.LibHost;
        private const string UrlApiFilelist = LibHost + "/1/file/ls";
        private const string UrlApiUpdateList = LibHost + "/1/file/updates";
        private const string UrlApiFileInfo = LibHost + "/1/file/info";
        private const string UrlApiCreateFolder = LibHost + "/1/file/create_folder";
        private const string UrlApiCreateFile = LibHost + "/1/file/create_file";
        private const string UrlApiDelFile = LibHost + "/1/file/del";
        private const string UrlApiMoveFile = LibHost + "/1/file/move";
        private const string UrlApiLinkFile = LibHost + "/1/file/link";
        private const string UrlApiSendmsg = LibHost + "/1/file/sendmsg";
        private const string UrlApiGetLink = LibHost + "/1/file/links";

        private readonly string _orgClientId;
        private readonly string _orgClientSecret;

        public EntFileManager(string orgClientId, string orgClientSecret)
        {
            _orgClientId = orgClientId;
            _orgClientSecret = orgClientSecret;
        }

        public HttpStatusCode StatusCode { get; internal set; }

        /// <summary>
        /// 获取文件列表
        /// </summary>
        /// <param name="dateline"></param>
        /// <param name="start"></param>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        public string GetFileList(int dateline, int start, string fullPath)
        {
            var request = new HttpRequestSyn();
            request.RequestUrl = UrlApiFilelist;
            request.AppendParameter("org_client_id", _orgClientId);
            request.AppendParameter("dateline", dateline + "");
            request.AppendParameter("start", start + "");
            request.AppendParameter("fullpath", fullPath);
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.Get;
            request.Request();
            StatusCode = request.Code;
            return request.Result;
        }

        /// <summary>
        /// 获取更新列表
        /// </summary>
        /// <param name="dateline"></param>
        /// <param name="isCompare"></param>
        /// <param name="fetchDateline"></param>
        /// <returns></returns>
        public string GetUpdateList(int dateline, bool isCompare, long fetchDateline)
        {
            var request = new HttpRequestSyn();
            request.RequestUrl = UrlApiUpdateList;
            request.AppendParameter("org_client_id", _orgClientId);
            request.AppendParameter("dateline", dateline + "");
            if (isCompare)
            {
                request.AppendParameter("mode", "compare");
            }
            request.AppendParameter("fetch_dateline", fetchDateline + "");
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.Get;
            request.Request();
            StatusCode = request.Code;
            return request.Result;
        }

        /// <summary>
        /// 获取文件信息
        /// </summary>
        /// <param name="dateline"></param>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        public string GetFileInfo(int dateline, string fullPath)
        {
            var request = new HttpRequestSyn();
            request.RequestUrl = UrlApiFileInfo;
            request.AppendParameter("org_client_id", _orgClientId);
            request.AppendParameter("dateline", dateline + "");
            request.AppendParameter("fullpath", fullPath);
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.Get;
            request.Request();
            StatusCode = request.Code;
            return request.Result;
        }

        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="dateline"></param>
        /// <param name="fullPath"></param>
        /// <param name="opName"></param>
        /// <returns></returns>
        public string CreateFolder(int dateline, string fullPath, string opName)
        {
            var request = new HttpRequestSyn();
            request.RequestUrl = UrlApiCreateFolder;
            request.AppendParameter("org_client_id", _orgClientId);
            request.AppendParameter("dateline", dateline + "");
            request.AppendParameter("fullpath", fullPath);
            request.AppendParameter("op_name", opName);
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.Post;
            request.Request();
            StatusCode = request.Code;
            return request.Result;
        }

        /// <summary>
        /// 通过文件流上传
        /// </summary>
        /// <param name="dateline"></param>
        /// <param name="fullPath"></param>
        /// <param name="opName"></param>
        /// <param name="stream"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string CreateFile(int dateline, string fullPath, string opName, Stream stream, string fileName)
        {
            if (stream.Length > UploadSizeLimit)
            {
                LogPrint.Print("文件大小超过50MB");
                return "";
            }

            var request = new HttpRequestSyn();
            request.RequestUrl = UrlApiCreateFile;
            string[] arr = {dateline + "", "file", fullPath, opName, _orgClientId};
            var data = new MsMultiPartFormData();
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
            request.RequestMethod = RequestType.Post;
            LogPrint.Print("------------->Begin to Upload<------------------");
            request.Request();
            LogPrint.Print("--------------------->Upload Request Compeleted<--------------");
            StatusCode = request.Code;
            return request.Result;
        }

        /// <summary>
        /// 通过本地路径上传
        /// </summary>
        /// <param name="dateline"></param>
        /// <param name="fullPath"></param>
        /// <param name="opName"></param>
        /// <param name="localPath"></param>
        /// <returns></returns>
        public string CreateFile(int dateline, string fullPath, string opName, string localPath)
        {
            using (var fs = new FileStream(localPath, FileMode.Open))
            {
                Stream stream = fs;
                return CreateFile(dateline, fullPath, opName, stream, Util.GetFileNameFromPath(localPath));
            }
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="dateline"></param>
        /// <param name="fullPath"></param>
        /// <param name="opName"></param>
        /// <returns></returns>
        public string Del(int dateline, string fullPath, string opName)
        {
            var request = new HttpRequestSyn();
            request.RequestUrl = UrlApiDelFile;
            request.AppendParameter("org_client_id", _orgClientId);
            request.AppendParameter("dateline", dateline + "");
            request.AppendParameter("fullpath", fullPath);
            request.AppendParameter("op_name", opName);
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.Post;
            request.Request();
            StatusCode = request.Code;
            return request.Result;
        }

        /// <summary>
        /// 移动文件
        /// </summary>
        /// <param name="dateline"></param>
        /// <param name="fullPath"></param>
        /// <param name="destFullPath"></param>
        /// <param name="opName"></param>
        /// <returns></returns>
        public string Move(int dateline, string fullPath, string destFullPath, string opName)
        {
            var request = new HttpRequestSyn();
            request.RequestUrl = UrlApiMoveFile;
            request.AppendParameter("org_client_id", _orgClientId);
            request.AppendParameter("dateline", dateline + "");
            request.AppendParameter("fullpath", fullPath);
            request.AppendParameter("dest_fullpath", destFullPath);
            request.AppendParameter("op_name", opName);
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.Post;
            request.Request();
            StatusCode = request.Code;
            return request.Result;
        }

        /// <summary>
        /// 获取文件链接
        /// </summary>
        /// <param name="dateline"></param>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        public string Link(int dateline, string fullPath)
        {
            var request = new HttpRequestSyn();
            request.RequestUrl = UrlApiLinkFile;
            request.AppendParameter("org_client_id", _orgClientId);
            request.AppendParameter("dateline", dateline + "");
            request.AppendParameter("fullpath", fullPath);
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.Post;
            request.Request();
            StatusCode = request.Code;
            return request.Result;
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="dateline"></param>
        /// <param name="title"></param>
        /// <param name="text"></param>
        /// <param name="image"></param>
        /// <param name="linkUrl"></param>
        /// <param name="opName"></param>
        /// <returns></returns>
        public string SendMsg(int dateline, string title, string text, string image, string linkUrl, string opName)
        {
            var request = new HttpRequestSyn();
            request.RequestUrl = UrlApiSendmsg;
            request.AppendParameter("org_client_id", _orgClientId);
            request.AppendParameter("dateline", dateline + "");
            request.AppendParameter("title", title);
            request.AppendParameter("text", text);
            request.AppendParameter("image", image);
            request.AppendParameter("url", linkUrl);
            request.AppendParameter("op_name", opName);
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.Post;
            request.Request();
            StatusCode = request.Code;
            return request.Result;
        }

        /// <summary>
        /// 获取当前库所有外链
        /// </summary>
        /// <param name="dateline"></param>
        /// <returns></returns>
        public string Links(int dateline)
        {
            var request = new HttpRequestSyn();
            request.RequestUrl = UrlApiGetLink;
            request.AppendParameter("org_client_id", _orgClientId);
            request.AppendParameter("dateline", dateline + "");
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.Get;
            request.Request();
            StatusCode = request.Code;
            return request.Result;
        }

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

            return Uri.EscapeDataString(Util.EncodeToHMACSHA1(stringSign, _orgClientSecret));
        }
    }
}