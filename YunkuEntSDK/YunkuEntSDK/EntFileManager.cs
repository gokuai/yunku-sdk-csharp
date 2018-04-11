using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using YunkuEntSDK.Net;
using YunkuEntSDK.UtilClass;
using static YunkuEntSDK.HttpEngine.RequestHelper;

namespace YunkuEntSDK
{
    public class EntFileManager : HttpEngine
    {
        private const string Log_Tag = "EntFileManager";

        private static long UploadSizeLimit = 52428800; //50MB
        private static int BlockSize = Config.BlockSize;
        private static string LibHost = Config.ApiHost;
        private static string UrlApiFilelist = LibHost + "/1/file/ls";
        private static string UrlApiUpdateList = LibHost + "/1/file/updates";
        private static string UrlApiFileInfo = LibHost + "/1/file/info";
        private static string UrlApiCreateFolder = LibHost + "/1/file/create_folder";
        private static string UrlApiCreateFile = LibHost + "/1/file/create_file";
        private static string UrlApiDelFile = LibHost + "/1/file/del";
        private static string UrlApiMoveFile = LibHost + "/1/file/move";
        private static string UrlApiLinkFile = LibHost + "/1/file/link";
        private static string UrlApiGetLink = LibHost + "/1/file/links";
        private static string UrlApiUpdateCount = LibHost + "/1/file/updates_count";
        private static string UrlApiGetServerSite = LibHost + "/1/file/servers";
        private static string UrlApiCreateFileByUrl = LibHost + "/1/file/create_file_by_url";
        private static string UrlApiUploadServers = LibHost + "/1/file/upload_servers";
        private static string UrlApiCopyFile = LibHost + "/1/file/copy";
        private static string UrlApiRecycleFile = LibHost + "/1/file/recycle";
        private static string UrlApiRecoverFile = LibHost + "/1/file/recover";
        private static string UrlApiCompletelyDelFile = LibHost + "/1/file/del_completely";
        private static string UrlApiHistoryFile = LibHost + "/1/file/history";
        private static string UrlApiGetUploadUrl = LibHost + "/1/file/download_url";
        private static string UrlApiSearchFile = LibHost + "/1/file/search";
        private static string UrlApiPreviewUrl = LibHost + "/1/file/preview_url";
        private static string UrlApiGetPermission = LibHost + "/1/file/get_permission";
        private static string UrlApiSetPermission = LibHost + "/1/file/file_permission";
        private static string UrlApiAddTag = LibHost + "/1/file/add_tag";
        private static string UrlApiDelTag = LibHost + "/1/file/del_tag";

        public delegate void CompletedEventHandler(object sender, CompletedEventArgs e);

        public delegate void ProgressChangeEventHandler(object sender, ProgressEventArgs e);

        public EntFileManager(string orgClientId, string orgClientSecret) : base(orgClientId, orgClientSecret)
        {
        }

        /// <summary>
        /// 获取根目录文件列表
        /// </summary>
        /// <returns></returns>
        public string GetFileList()
        {
            return GetFileList("", 0, 100, false);
        }

        /// <summary>
        /// 获取文件列表异步异步请求示例
        /// </summary>
        /// <param name="fullpath"></param>
        /// <param name="start"></param>
        /// <param name="size"></param>
        /// <param name="dirOnly"></param>
        /// <param name="handler"></param>
        /// <returns></returns>
        public Thread GetFileListAsync(string fullpath, int start, int size, bool dirOnly, RequestEventHanlder handler)
        {

            string url = UrlApiFilelist;
            var parameter = new Dictionary<string, string>();
            parameter.Add("org_client_id", _clientId);
            parameter.Add("dateline", Util.GetUnixDataline() + "");
            parameter.Add("start", start + "");
            parameter.Add("fullpath", fullpath);
            parameter.Add("size", size + "");
            if (dirOnly)
            {
                parameter.Add("dir", "1");
            }
            parameter.Add("sign", GenerateSign(parameter));
            return new RequestHelper().SetParams(parameter).SetUrl(url).SetMethod(RequestType.Get).ExecuteAsync(1, handler);

        }

        /// <summary>
        /// 获取文件列表
        /// </summary>
        /// <param name="fullpath"></param>
        /// <returns></returns>
        public string GetFileList(string fullpath)
        {
            return GetFileList(fullpath, 0, 100, false);
        }

        /// <summary>
        /// 获取文件列表
        /// </summary>
        /// <param name="start"></param>
        /// <param name="fullpath"></param>
        /// <returns></returns>
        public string GetFileList(string fullpath, int start, int size, bool dirOnly)
        {
            string url = UrlApiFilelist;
            var parameter = new Dictionary<string, string>();
            parameter.Add("org_client_id", _clientId);
            parameter.Add("dateline", Util.GetUnixDataline() + "");
            parameter.Add("start", start + "");
            parameter.Add("fullpath", fullpath);
            parameter.Add("size", size + "");
            if (dirOnly)
            {
                parameter.Add("dir", "1");
            }
            parameter.Add("sign", GenerateSign(parameter));
            return new RequestHelper().SetParams(parameter).SetUrl(url).SetMethod(RequestType.Get).ExecuteSync();
        }

        /// <summary>
        /// 获取更新列表
        /// </summary>
        /// <param name="isCompare"></param>
        /// <param name="fetchDateline"></param>
        /// <returns></returns>
        public string GetUpdateList(bool isCompare, long fetchDateline)
        {
            string url = UrlApiUpdateList;
            var parameter = new Dictionary<string, string>();
            parameter.Add("org_client_id", _clientId);
            parameter.Add("dateline", Util.GetUnixDataline() + "");
            if (isCompare)
            {
                parameter.Add("mode", "compare");
            }
            parameter.Add("fetch_dateline", fetchDateline + "");
            parameter.Add("sign", GenerateSign(parameter));
            return new RequestHelper().SetParams(parameter).SetUrl(url).SetMethod(RequestType.Get).ExecuteSync();
        }

        /// <summary>
        /// 文件更新数量
        /// </summary>
        /// <param name="beginDateline"></param>
        /// <param name="endDateline"></param>
        /// <param name="showDelete"></param>
        /// <returns></returns>
        public string GetUpdateCount(long beginDateline, long endDateline, bool showDelete)
        {
            string url = UrlApiUpdateCount;
            var parameter = new Dictionary<string, string>();
            parameter.Add("org_client_id", _clientId);
            parameter.Add("dateline", Util.GetUnixDataline() + "");
            parameter.Add("begin_dateline", beginDateline + "");
            parameter.Add("end_dateline", endDateline + "");
            parameter.Add("showdel", (showDelete ? 1 : 0) + "");
            parameter.Add("sign", GenerateSign(parameter));
            return new RequestHelper().SetParams(parameter).SetUrl(url).SetMethod(RequestType.Get).ExecuteSync();
        }

        /// <summary>
        /// 获取文件信息
        /// </summary>
        /// <param name="fullpath"></param>
        /// <returns></returns>
        public string GetFileInfoByFullpath(string fullpath)
        {
            return this.GetFileInfoByFullpath(fullpath, NetType.Default, false);
        }

        /// <summary>
        /// 获取文件信息
        /// </summary>
        /// <param name="fullpath"></param>
        /// <returns></returns>
        public string GetFileInfoByFullpath(string fullpath, bool getAttribute)
        {
            return this.GetFileInfoByFullpath(fullpath, NetType.Default, getAttribute);
        }

        /// <summary>
        /// 获取文件信息
        /// </summary>
        /// <param name="fullpath"></param>
        /// <param name="type"></param>
        /// <param name="getAttribute"></param>
        /// <returns></returns>
        public string GetFileInfoByFullpath(string fullpath, NetType type, bool getAttribute)
        {
            return this.GetFileInfo(null, fullpath, type, getAttribute);
        }

        /// <summary>
        /// 获取文件信息
        /// </summary>
        /// <param name="hash"></param>
        /// <returns></returns>
        public string GetFileInfoByHash(string hash)
        {
            return this.GetFileInfoByHash(hash, NetType.Default, false);
        }

        /// <summary>
        /// 获取文件信息
        /// </summary>
        /// <param name="hash"></param>
        /// <returns></returns>
        public string GetFileInfoByHash(string hash, bool getAttribute)
        {
            return this.GetFileInfoByHash(hash, NetType.Default, getAttribute);
        }

        /// <summary>
        /// 获取文件信息
        /// </summary>
        /// <param name="hash"></param>
        /// <param name="type"></param>
        /// <param name="getAttribute"></param>
        /// <returns></returns>
        public string GetFileInfoByHash(string hash, NetType type, bool getAttribute)
        {
            return this.GetFileInfo(hash, null, type, getAttribute);
        }

        /// <summary>
        /// 获取文件信息
        /// </summary>
        /// <param name="hash"></param>
        /// <param name="fullpath"></param>
        /// <param name="type"></param>
        /// <param name="getAttribute"></param>
        /// <returns></returns>
        private string GetFileInfo(string hash, string fullpath, NetType type, bool getAttribute)
        {
            string url = UrlApiFileInfo;
            var parameter = new Dictionary<string, string>();
            parameter.Add("org_client_id", _clientId);
            parameter.Add("dateline", Util.GetUnixDataline() + "");
            parameter.Add("fullpath", fullpath);
            parameter.Add("attribute", (getAttribute ? 1 : 0) + "");
            switch (type)
            {
                case NetType.Default:
                    break;
                case NetType.In:
                    parameter.Add("net", type.ToString().ToLower());
                    break;
            }
            parameter.Add("sign", GenerateSign(parameter));
            return new RequestHelper().SetParams(parameter).SetUrl(url).SetMethod(RequestType.Get).ExecuteSync();
        }

        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="fullpath"></param>
        /// <param name="opName"></param>
        /// <returns></returns>
        public string CreateFolder(string fullpath, string opName)
        {
            string url = UrlApiCreateFolder;
            var parameter = new Dictionary<string, string>();
            parameter.Add("org_client_id", _clientId);
            parameter.Add("dateline", Util.GetUnixDataline() + "");
            parameter.Add("fullpath", fullpath);
            parameter.Add("op_name", opName);
            parameter.Add("sign", GenerateSign(parameter));
            return new RequestHelper().SetParams(parameter).SetUrl(url).SetMethod(RequestType.Post).ExecuteSync();
        }

        /// <summary>
        /// 通过文件流上传
        /// </summary>
        /// <param name="fullpath"></param>
        /// <param name="opName"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        public string CreateFile(string fullpath, string opName, Stream stream, bool overWrite)
        {
            if (stream.Length > UploadSizeLimit)
            {
                LogPrint.Print("文件大小超过50MB");
                return "";
            }
            long dateline = Util.GetUnixDataline();


            string url = UrlApiCreateFile;
            var parameter = new Dictionary<string, string>();
            parameter.Add("org_client_id", _clientId);
            parameter.Add("dateline", dateline + "");
            parameter.Add("fullpath", fullpath);
            parameter.Add("op_name", opName);
            parameter.Add("overwrite", (overWrite ? 1 : 0) + "");
            parameter.Add("filefield", "file");

            var data = new MsMultiPartFormData();
            string fileName = Util.GetFileNameFromPath(fullpath);
            data.AddStreamFile("file", fileName, Util.ReadToEnd(stream));
            data.AddParams("org_client_id", _clientId);
            data.AddParams("dateline", dateline + "");
            data.AddParams("fullpath", fullpath);
            data.AddParams("op_name", opName);
            data.AddParams("overwrite", (overWrite ? 1 : 0) + "");
            data.AddParams("filefield", "file");
            data.AddParams("sign", GenerateSign(parameter));
            data.PrepareFormData();

            string contentType = "multipart/form-data;boundary=" + data.Boundary;
            var postDataByte = data.GetFormData();

            return new RequestHelper().SetUrl(url).SetContentType(contentType).SetPostDataByte(postDataByte).SetMethod(RequestType.Post).ExecuteSync();
        }

        /// <summary>
        /// 通过本地路径上传
        /// </summary>
        /// <param name="fullpath"></param>
        /// <param name="opName"></param>
        /// <param name="localPath"></param>
        /// <returns></returns>
        public string CreateFile(string fullpath, string opName, string localPath)
        {
            if (File.Exists(localPath))
            {
                using (var fs = new FileStream(localPath, FileMode.Open))
                {
                    Stream stream = fs;
                    return CreateFile(fullpath, opName, stream, true);
                }
            }
            else
            {
                LogPrint.Print(fullpath + " file not exist");
                return "";
            }
        }

        /// <summary>
        /// 分块上传本地文件，默认分块上传大小10MB
        /// </summary>
        /// <param name="fullpath"></param>
        /// <param name="opName"></param>
        /// <param name="opId"></param>
        /// <param name="localFilePath"></param>
        /// <param name="overWrite"></param>
        /// <param name="completedEventHandler"></param>
        /// <param name="progressChangeEventHandler"></param>
        /// <returns></returns>
        public bool UploadByBlockAsync(string fullpath, string opName, int opId, string localFilePath, bool overWrite
            , CompletedEventHandler completedEventHandler, ProgressChangeEventHandler progressChangeEventHandler)
        {
            return UploadByBlockAsync(fullpath, opName, opId, localFilePath, overWrite, BlockSize, completedEventHandler, progressChangeEventHandler);
        }

        public bool UploadByBlockAsync(string fullpath, string opName, int opId, string localFilePath, bool overWrite
            , int blockSize, CompletedEventHandler completedEventHandler, ProgressChangeEventHandler progressChangeEventHandler)
        {
            FileStream stream = File.OpenRead(localFilePath);
            return UploadByBlockAsync(fullpath, opName, opId, stream, overWrite, blockSize, completedEventHandler, progressChangeEventHandler);
        }

        /// <summary>
        /// 分块上传文件流，默认分块上传大小10MB
        /// </summary>
        /// <param name="fullpath"></param>
        /// <param name="opName"></param>
        /// <param name="opId"></param>
        /// <param name="stream"></param>
        /// <param name="overWrite"></param>
        /// <param name="completedEventHandler"></param>
        /// <param name="progressChangeEventHandler"></param>
        /// <returns></returns>
        public bool UploadByBlockAsync(string fullpath, string opName, int opId, Stream stream, bool overWrite
            , CompletedEventHandler completedEventHandler, ProgressChangeEventHandler progressChangeEventHandler)
        {
            return UploadByBlockAsync(fullpath, opName, opId, stream, overWrite, BlockSize, completedEventHandler, progressChangeEventHandler);
        }

        public bool UploadByBlockAsync(string fullpath, string opName, int opId, Stream stream, bool overWrite
            , int blockSize, CompletedEventHandler completedEventHandler, ProgressChangeEventHandler progressChangeEventHandler)
        {
            UploadManager uploadManager = new UploadManager(UrlApiCreateFile, stream,
                fullpath, opName, opId, _clientId, Util.GetUnixDataline(), _clientSecret, overWrite, blockSize);
            uploadManager.Completed += new UploadManager.CompletedEventHandler(completedEventHandler);
            uploadManager.ProgresChanged += new UploadManager.ProgressChangeEventHandler(progressChangeEventHandler);

            WaitCallback callback = new WaitCallback(uploadManager.DoUpload);
            return ThreadPool.QueueUserWorkItem(callback);
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="fullpaths"></param>
        /// <param name="opName"></param>
        /// <param name="destroy"></param>
        /// <returns></returns>
        public string Del(string fullpaths, string opName, bool destroy)
        {
            string url = UrlApiDelFile;
            var parameter = new Dictionary<string, string>();
            parameter.Add("org_client_id", _clientId);
            parameter.Add("dateline", Util.GetUnixDataline() + "");
            parameter.Add("fullpaths", fullpaths);
            parameter.Add("op_name", opName);
            if (destroy)
            {
                parameter.Add("destroy", "1");
            }
            parameter.Add("sign", GenerateSign(parameter));
            return new RequestHelper().SetParams(parameter).SetUrl(url).SetMethod(RequestType.Post).ExecuteSync();
        }

        /// <summary>
        /// 移动文件
        /// </summary>
        /// <param name="fullpath"></param>
        /// <param name="destFullpath"></param>
        /// <param name="opName"></param>
        /// <returns></returns>
        public string Move(string fullpath, string destFullpath, string opName)
        {
            string url = UrlApiMoveFile;
            var parameter = new Dictionary<string, string>();
            parameter.Add("org_client_id", _clientId);
            parameter.Add("dateline", Util.GetUnixDataline() + "");
            parameter.Add("fullpath", fullpath);
            parameter.Add("dest_fullpath", destFullpath);
            parameter.Add("op_name", opName);
            parameter.Add("sign", GenerateSign(parameter));
            return new RequestHelper().SetParams(parameter).SetUrl(url).SetMethod(RequestType.Post).ExecuteSync();
        }

        /// <summary>
        /// 获取文件链接
        /// </summary>
        /// <param name="fullpath"></param>
        /// <returns></returns>
        public string Link(string fullpath, int deadline, AuthType authType, string password)
        {
            string url = UrlApiLinkFile;
            var parameter = new Dictionary<string, string>();
            parameter.Add("org_client_id", _clientId);
            parameter.Add("dateline", Util.GetUnixDataline() + "");
            parameter.Add("fullpath", fullpath);

            if (deadline != 0)
            {
                parameter.Add("deadline", deadline + "");
            }

            if (!authType.Equals(AuthType.Default))
            {
                parameter.Add("auth", authType.ToString().ToLower());
            }
            parameter.Add("password", password);
            parameter.Add("sign", GenerateSign(parameter));
            return new RequestHelper().SetParams(parameter).SetUrl(url).SetMethod(RequestType.Post).ExecuteSync();
        }

        /// <summary>
        /// 获取当前库所有外链
        /// </summary>
        /// <param name="fileOnly"></param>
        /// <returns></returns>
        public string Links(bool fileOnly)
        {
            string url = UrlApiGetLink;
            var parameter = new Dictionary<string, string>();
            parameter.Add("org_client_id", _clientId);
            parameter.Add("dateline", Util.GetUnixDataline() + "");
            if (fileOnly)
            {
                parameter.Add("file", "1");
            }

            parameter.Add("sign", GenerateSign(parameter));
            return new RequestHelper().SetParams(parameter).SetUrl(url).SetMethod(RequestType.Get).ExecuteSync();
        }

        /// <summary>
        /// 通过链接上传文件
        /// </summary>
        /// <param name="fullpath"></param>
        /// <param name="opId"></param>
        /// <param name="opName"></param>
        /// <param name="overwrite"></param>
        /// <param name="fileUrl"></param>
        /// <returns></returns>
        public string CreateFileByUrl(string fullpath, int opId, string opName, bool overwrite, string fileUrl)
        {
            string url = UrlApiCreateFileByUrl;
            var parameter = new Dictionary<string, string>();
            parameter.Add("org_client_id", _clientId);
            parameter.Add("fullpath", fullpath);
            parameter.Add("dateline", Util.GetUnixDataline() + "");
            if (opId > 0)
            {
                parameter.Add("op_id", opId + "");
            }
            else
            {
                parameter.Add("op_name", opName);
            }
            parameter.Add("overwrite", (overwrite ? 1 : 0) + "");
            parameter.Add("url", fileUrl);
            parameter.Add("sign", GenerateSign(parameter));
            return new RequestHelper().SetParams(parameter).SetUrl(url).SetMethod(RequestType.Post).ExecuteSync();
        }

        /// <summary>
        /// 获取上传地址
        /// (支持50MB以上文件的上传)
        /// </summary>
        /// <returns></returns>
        public string GetUploadServers()
        {
            string url = UrlApiUploadServers;
            var parameter = new Dictionary<string, string>();
            parameter.Add("org_client_id", _clientId);
            parameter.Add("dateline", Util.GetUnixDataline() + "");
            parameter.Add("sign", GenerateSign(parameter));
            return new RequestHelper().SetParams(parameter).SetUrl(url).SetMethod(RequestType.Get).ExecuteSync();
        }

        /// <summary>
        /// 获取服务器地址
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public string GetServerSite(string type)
        {
            string url = UrlApiGetServerSite;
            var parameter = new Dictionary<string, string>();
            parameter.Add("org_client_id", _clientId);
            parameter.Add("dateline", Util.GetUnixDataline() + "");
            parameter.Add("type", type);
            parameter.Add("sign", GenerateSign(parameter));
            return new RequestHelper().SetParams(parameter).SetUrl(url).SetMethod(RequestType.Get).ExecuteSync();
        }

        /// <summary>
        /// 复制文件
        /// </summary>
        /// <param name="originFullpath"></param>
        /// <param name="targetFullpath"></param>
        /// <param name="opName"></param>
        /// <returns></returns>
        public string Copy(string originFullpath, string targetFullpath, string opName)
        {
            string url = UrlApiCopyFile;
            var parameter = new Dictionary<string, string>();
            parameter.Add("org_client_id", _clientId);
            parameter.Add("dateline", Util.GetUnixDataline() + "");
            parameter.Add("from_fullpath", originFullpath);
            parameter.Add("fullpath", targetFullpath);
            parameter.Add("op_name", opName);
            parameter.Add("sign", GenerateSign(parameter));
            return new RequestHelper().SetParams(parameter).SetUrl(url).SetMethod(RequestType.Post).ExecuteSync();
        }

        /// <summary>
        /// 回收站
        /// </summary>
        /// <param name="start"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public string Recyle(int start, int size)
        {
            string url = UrlApiRecycleFile;
            var parameter = new Dictionary<string, string>();
            parameter.Add("org_client_id", _clientId);
            parameter.Add("dateline", Util.GetUnixDataline() + "");
            parameter.Add("start", start + "");
            parameter.Add("size", size + "");
            parameter.Add("sign", GenerateSign(parameter));
            return new RequestHelper().SetParams(parameter).SetUrl(url).SetMethod(RequestType.Get).ExecuteSync();
        }

        /// <summary>
        /// 恢复删除文件
        /// </summary>
        /// <param name="fullpaths"></param>
        /// <param name="opName"></param>
        /// <returns></returns>
        public string Recover(string fullpaths, string opName)
        {
            string url = UrlApiRecoverFile;
            var parameter = new Dictionary<string, string>();
            parameter.Add("org_client_id", _clientId);
            parameter.Add("dateline", Util.GetUnixDataline() + "");
            parameter.Add("fullpaths", fullpaths);
            parameter.Add("op_name", opName);
            parameter.Add("sign", GenerateSign(parameter));
            return new RequestHelper().SetParams(parameter).SetUrl(url).SetMethod(RequestType.Post).ExecuteSync();
        }

        /// <summary>
        /// 彻底删除文件（夹）
        /// </summary>
        /// <param name="fullpaths"></param>
        /// <param name="opName"></param>
        /// <returns></returns>
        public string CompletelyDelFile(string[] fullpaths, string opName)
        {
            string url = UrlApiCompletelyDelFile;
            var parameter = new Dictionary<string, string>();
            parameter.Add("org_client_id", _clientId);
            parameter.Add("dateline", Util.GetUnixDataline() + "");
            parameter.Add("fullpaths", Util.StrArrayToString(fullpaths, "|") + "");
            parameter.Add("op_name", opName);
            parameter.Add("sign", GenerateSign(parameter));
            return new RequestHelper().SetParams(parameter).SetUrl(url).SetMethod(RequestType.Post).ExecuteSync();
        }

        /// <summary>
        /// 获取文件历史
        /// </summary>
        /// <param name="fullpath"></param>
        /// <param name="start"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public string History(string fullpath, int start, int size)
        {
            string url = UrlApiHistoryFile;
            var parameter = new Dictionary<string, string>();
            parameter.Add("org_client_id", _clientId);
            parameter.Add("dateline", Util.GetUnixDataline() + "");
            parameter.Add("fullpath", fullpath);
            parameter.Add("start", start + "");
            parameter.Add("size", size + "");
            parameter.Add("sign", GenerateSign(parameter));
            return new RequestHelper().SetParams(parameter).SetUrl(url).SetMethod(RequestType.Post).ExecuteSync();

        }

        /// <summary>
        /// 通过文件唯一标识获取下载地址
        /// </summary>
        /// <param name="hash"></param>
        /// <returns></returns>
        public string GetDownloadUrlByHash(string hash)
        {
            return GetDownloadUrl(hash, null, false, NetType.Default, "");
        }

        /// <summary>
        /// 通过文件唯一标识获取下载地址
        /// </summary>
        /// <param name="hash"></param>
        /// <param name="isOpen"></param>
        /// <param name="net"></param>
        /// <returns></returns>
        public string GetDownloadUrlByHash(string hash, bool isOpen, NetType net)
        {
            return GetDownloadUrl(hash, null, isOpen, net, "");
        }

        /// <summary>
        /// 通过文件路径获取下载地址
        /// </summary>
        /// <param name="fullpath"></param>
        /// <returns></returns>
        public string GetDownloadUrlByFullpath(string fullpath)
        {
            return GetDownloadUrl(null, fullpath, false, NetType.Default, "");
        }

        /// <summary>
        /// 通过文件路径获取下载地址
        /// </summary>
        /// <param name="fullpath"></param>
        /// <param name="isOpen"></param>
        /// <param name="net"></param>
        /// <returns></returns>
        public string GetDownloadUrlByFullpath(string fullpath, bool isOpen, NetType net)
        {
            return GetDownloadUrl(null, fullpath, isOpen, net, "");
        }

        /// <summary>
        /// 获取下载地址
        /// </summary>
        /// <param name="hash"></param>
        /// <param name="fullpath"></param>
        /// <param name="isOpen"></param>
        /// <param name="net"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private string GetDownloadUrl(string hash, string fullpath, bool isOpen, NetType net, string fileName)
        {
            string url = UrlApiGetUploadUrl;
            var parameter = new Dictionary<string, string>();
            parameter.Add("org_client_id", _clientId);
            parameter.Add("dateline", Util.GetUnixDataline() + "");
            parameter.Add("fullpath", fullpath);
            parameter.Add("hash", hash);
            parameter.Add("open", (isOpen ? 1 : 0) + "");
            switch (net)
            {
                case NetType.Default:
                    break;
                case NetType.In:
                    parameter.Add("net", net.ToString().ToLower());
                    break;
            }
            parameter.Add("sign", GenerateSign(parameter));
            return new RequestHelper().SetParams(parameter).SetUrl(url).SetMethod(RequestType.Get).ExecuteSync();
        }

        /// <summary>
        /// 文件搜索
        /// </summary>
        /// <param name="keyWords"></param>
        /// <param name="path"></param>
        /// <param name="start"></param>
        /// <param name="size"></param>
        /// <param name="scopes"></param>
        /// <returns></returns>
        public string Search(string keyWords, string path, int start, int size, params ScopeType[] scopes)
        {
            string url = UrlApiSearchFile;
            var parameter = new Dictionary<string, string>();
            parameter.Add("org_client_id", _clientId);
            parameter.Add("dateline", Util.GetUnixDataline() + "");
            parameter.Add("keywords", keyWords);
            parameter.Add("path", path);
            JsonArray array = new JsonArray();
            foreach (var s in scopes)
            {
                string scope = Enum.GetName(typeof(ScopeType), s);
                array.Add(scope);
            }
            parameter.Add("scope", array.ToString().ToLower());
            parameter.Add("start", start + "");
            parameter.Add("size", size + "");
            parameter.Add("sign", GenerateSign(parameter));
            return new RequestHelper().SetParams(parameter).SetUrl(url).SetMethod(RequestType.Get).ExecuteSync();
        }

        /// <summary>
        /// 文件预览地址
        /// </summary>
        /// <param name="fullpath"></param>
        /// <param name="showWaterMark"></param>
        /// <param name="memberName"></param>
        /// <param name="returnThumbnail"></param>
        /// <returns></returns>
        public string GetPreviewUrlByFullpath(string fullpath, bool showWatermark, string memberName, bool returnThumbnail)
        {
            return this.PreviewUrl(null, fullpath, showWatermark, memberName, returnThumbnail);
        }

        /// <summary>
        /// 文件预览地址
        /// </summary>
        /// <param name="hash"></param>
        /// <param name="showWaterMark"></param>
        /// <param name="memberName"></param>
        /// <param name="returnThumbnail"></param>
        /// <returns></returns>
        public string GetPreviewUrlByHash(string hash, bool showWatermark, string memberName, bool returnThumbnail)
        {
            return this.PreviewUrl(hash, null, showWatermark, memberName, returnThumbnail);
        }

        /// <summary>
        /// 文件预览地址
        /// </summary>
        /// <param name="hash"></param>
        /// <param name="fullpath"></param>
        /// <param name="showWaterMark"></param>
        /// <param name="memberName"></param>
        /// <param name="returnThumbnail"></param>
        /// <returns></returns>
        private string PreviewUrl(string hash, string fullpath, bool showWatermark, string memberName, bool returnThumbnail)
        {
            string url = UrlApiPreviewUrl;
            var parameter = new Dictionary<string, string>();
            parameter.Add("org_client_id", _clientId);
            parameter.Add("dateline", Util.GetUnixDataline() + "");
            parameter.Add("hash", fullpath);
            parameter.Add("fullpath", fullpath);
            if (showWatermark)
            {
                parameter.Add("watermark", "1");
            }
            parameter.Add("member_name", memberName);
            if (returnThumbnail)
            {
                parameter.Add("thumbnail", "1");
            }
            parameter.Add("sign", GenerateSign(parameter));
            return new RequestHelper().SetParams(parameter).SetUrl(url).SetMethod(RequestType.Get).ExecuteSync();
        }

        /// <summary>
        /// 获取文件夹权限
        /// </summary>
        /// <param name="fullpath"></param>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public string GetPermission(string fullpath, int memberId)
        {
            string url = UrlApiGetPermission;
            var parameter = new Dictionary<string, string>();
            parameter.Add("org_client_id", _clientId);
            parameter.Add("dateline", Util.GetUnixDataline() + "");
            parameter.Add("fullpath", fullpath);
            parameter.Add("member_id", memberId + "");
            parameter.Add("sign", GenerateSign(parameter));
            return new RequestHelper().SetParams(parameter).SetUrl(url).SetMethod(RequestType.Post).ExecuteSync();
        }

        /// <summary>
        /// 修改文件夹权限
        /// </summary>
        /// <param name="fullpath"></param>
        /// <param name="memberId"></param>
        /// <param name="permissions"></param>
        /// <returns></returns>
        public string SetPermission(string fullpath, int memberId, params FilePermissions[] permissions)
        {
            string url = UrlApiSetPermission;
            var parameter = new Dictionary<string, string>();
            parameter.Add("org_client_id", _clientId);
            parameter.Add("dateline", Util.GetUnixDataline() + "");
            parameter.Add("fullpath", fullpath);

            var jsonArray = new JsonArray();
            var jsonObject = new JsonObject();
            foreach (var s in permissions)
            {
                string scope = Enum.GetName(typeof(FilePermissions), s);
                jsonArray.Add(scope);
            }

            jsonObject.Add(memberId + "", jsonArray);
            parameter.Add("permissions", jsonObject.ToString().ToLower());
            parameter.Add("sign", GenerateSign(parameter));
            return new RequestHelper().SetParams(parameter).SetUrl(url).SetMethod(RequestType.Post).ExecuteSync();
        }

        /// <summary>
        /// 添加标签
        /// </summary>
        /// <param name="fullpath"></param>
        /// <param name="tags"></param>
        /// <returns></returns>
        public string AddTag(string fullpath, string[] tags)
        {
            string url = UrlApiAddTag;
            var parameter = new Dictionary<string, string>();
            parameter.Add("org_client_id", _clientId);
            parameter.Add("dateline", Util.GetUnixDataline() + "");
            parameter.Add("fullpath", fullpath);
            parameter.Add("tag", Util.StrArrayToString(tags, ";") + "");
            parameter.Add("sign", GenerateSign(parameter));
            return new RequestHelper().SetParams(parameter).SetUrl(url).SetMethod(RequestType.Post).ExecuteSync();
        }

        /// <summary>
        /// 删除标签
        /// </summary>
        /// <param name="fullpath"></param>
        /// <param name="tags"></param>
        /// <returns></returns>
        public string DelTag(string fullpath, string[] tags)
        {
            string url = UrlApiDelTag;
            var parameter = new Dictionary<string, string>();
            parameter.Add("org_client_id", _clientId);
            parameter.Add("dateline", Util.GetUnixDataline() + "");
            parameter.Add("fullpath", fullpath);
            parameter.Add("tag", Util.StrArrayToString(tags, ";") + "");
            parameter.Add("sign", GenerateSign(parameter));
            return new RequestHelper().SetParams(parameter).SetUrl(url).SetMethod(RequestType.Post).ExecuteSync();
        }

        public enum AuthType
        {
            Default,
            Preview,
            Download,
            Upload
        }

        public enum NetType
        {
            Default,
            In
        }

        public enum ScopeType
        {
            Tag,
            Content,
            FileName
        }

        public enum FilePermissions
        {
            FileRead,
            FilePreview,
            FileWrite,
            FileDelete
        }
    }
}