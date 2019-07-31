using System;
using System.Collections.Generic;
using System.IO;
using YunkuEntSDK.Data;
using YunkuEntSDK.Net;
using YunkuEntSDK.UtilClass;

namespace YunkuEntSDK
{
    public class EntFileManager : EntEngine
    {
        private const string Log_Tag = "EntFileManager";
        
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
        private static string UrlApiLinkClose = LibHost + "/1/file/link_close";
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
        private static string UrlApiGetDownloadUrl = LibHost + "/1/file/download_url";
        private static string UrlApiSearchFile = LibHost + "/1/file/search";
        private static string UrlApiPreviewUrl = LibHost + "/1/file/preview_url";
        private static string UrlApiGetPermission = LibHost + "/1/file/get_permission";
        private static string UrlApiSetPermission = LibHost + "/1/file/file_permission";
        private static string UrlApiAddTag = LibHost + "/1/file/add_tag";
        private static string UrlApiDelTag = LibHost + "/1/file/del_tag";

        public delegate void CompletedEventHandler(object sender, CompletedEventArgs e);

        public delegate void ProgressChangeEventHandler(object sender, ProgressEventArgs e);

        public EntFileManager(string orgClientId, string secret) : base(orgClientId, secret)
        {
            this._clientIdKey = "org_client_id";
        }

        /// <summary>
        /// 获取根目录文件列表
        /// </summary>
        /// <returns></returns>
        public ReturnResult GetFileList()
        {
            return GetFileList("", 0, 100, false);
        }

        /// <summary>
        /// 获取文件列表
        /// </summary>
        /// <param name="fullpath"></param>
        /// <returns></returns>
        public ReturnResult GetFileList(string fullpath)
        {
            return GetFileList(fullpath, 0, 100, false);
        }

        /// <summary>
        /// 获取文件列表
        /// </summary>
        /// <param name="start"></param>
        /// <param name="fullpath"></param>
        /// <returns></returns>
        public ReturnResult GetFileList(string fullpath, int start, int size, bool dirOnly)
        {
            string url = UrlApiFilelist;
            var parameter = new Dictionary<string, string>();
            parameter.Add("start", start.ToString());
            parameter.Add("fullpath", fullpath);
            parameter.Add("size", size.ToString());
            if (dirOnly)
            {
                parameter.Add("dir", "1");
            }
            return new RequestHelper(this).SetParams(parameter).SetUrl(url).SetMethod(RequestType.GET).ExecuteSync();
        }

        /// <summary>
        /// 获取更新列表
        /// </summary>
        /// <param name="isCompare"></param>
        /// <param name="fetchDateline"></param>
        /// <returns></returns>
        public ReturnResult GetUpdateList(bool isCompare, long fetchDateline)
        {
            string url = UrlApiUpdateList;
            var parameter = new Dictionary<string, string>();
            if (isCompare)
            {
                parameter.Add("mode", "compare");
            }
            parameter.Add("fetch_dateline", fetchDateline.ToString());
            return new RequestHelper(this).SetParams(parameter).SetUrl(url).SetMethod(RequestType.GET).ExecuteSync();
        }

        /// <summary>
        /// 文件更新数量
        /// </summary>
        /// <param name="beginDateline"></param>
        /// <param name="endDateline"></param>
        /// <param name="showDelete"></param>
        /// <returns></returns>
        public ReturnResult GetUpdateCount(long beginDateline, long endDateline, bool showDelete)
        {
            string url = UrlApiUpdateCount;
            var parameter = new Dictionary<string, string>();
            parameter.Add("begin_dateline", beginDateline.ToString());
            parameter.Add("end_dateline", endDateline.ToString());
            parameter.Add("showdel", (showDelete ? "1" : "0"));
            return new RequestHelper(this).SetParams(parameter).SetUrl(url).SetMethod(RequestType.GET).ExecuteSync();
        }

        /// <summary>
        /// 获取文件信息
        /// </summary>
        /// <param name="fullpath"></param>
        /// <returns></returns>
        public ReturnResult GetFileInfoByFullpath(string fullpath)
        {
            return this.GetFileInfoByFullpath(fullpath, NetType.Default, false);
        }

        /// <summary>
        /// 获取文件信息
        /// </summary>
        /// <param name="fullpath"></param>
        /// <returns></returns>
        public ReturnResult GetFileInfoByFullpath(string fullpath, bool getAttribute)
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
        public ReturnResult GetFileInfoByFullpath(string fullpath, NetType type, bool getAttribute)
        {
            return this.GetFileInfo(null, fullpath, type, getAttribute);
        }

        /// <summary>
        /// 获取文件信息
        /// </summary>
        /// <param name="hash"></param>
        /// <returns></returns>
        public ReturnResult GetFileInfoByHash(string hash)
        {
            return this.GetFileInfoByHash(hash, NetType.Default, false);
        }

        /// <summary>
        /// 获取文件信息
        /// </summary>
        /// <param name="hash"></param>
        /// <returns></returns>
        public ReturnResult GetFileInfoByHash(string hash, bool getAttribute)
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
        public ReturnResult GetFileInfoByHash(string hash, NetType type, bool getAttribute)
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
        private ReturnResult GetFileInfo(string hash, string fullpath, NetType type, bool getAttribute)
        {
            string url = UrlApiFileInfo;
            var parameter = new Dictionary<string, string>();
            parameter.Add("fullpath", fullpath);
            parameter.Add("attribute", (getAttribute ? "1" : "0"));
            switch (type)
            {
                case NetType.Default:
                    break;
                case NetType.In:
                    parameter.Add("net", type.ToString().ToLower());
                    break;
            }
            return new RequestHelper(this).SetParams(parameter).SetUrl(url).SetMethod(RequestType.GET).ExecuteSync();
        }

        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="fullpath"></param>
        /// <param name="opName"></param>
        /// <returns></returns>
        public ReturnResult CreateFolder(string fullpath, string opName)
        {
            string url = UrlApiCreateFolder;
            var parameter = new Dictionary<string, string>();
            parameter.Add("fullpath", fullpath);
            parameter.Add("op_name", opName);
            return new RequestHelper(this).SetParams(parameter).SetUrl(url).SetMethod(RequestType.POST).ExecuteSync();
        }

        /// <summary>
        /// 请求上传文件
        /// </summary>
        /// <param name="fullpath"></param>
        /// <param name="fileHash"></param>
        /// <param name="fileSize"></param>
        /// <param name="opId"></param>
        /// <param name="opName"></param>
        /// <param name="overwrite"></param>
        /// <returns></returns>
        public ReturnResult CreateFile(string fullpath, string fileHash, long fileSize, int opId, string opName, bool overwrite)
        {
            string url = UrlApiCreateFile;
            var parameter = new Dictionary<string, string>();
            parameter.Add("fullpath", fullpath);
            parameter.Add("filehash", fileHash);
            parameter.Add("filesize", fileSize.ToString());
            if (opId > 0)
            {
                parameter.Add("op_id", opId.ToString());
            }
            else if (!String.IsNullOrEmpty(opName))
            {
                parameter.Add("op_name", opName);
            }
            parameter.Add("overwrite", (overwrite ? "1" : "0"));
            return new RequestHelper(this).SetParams(parameter).SetUrl(url).SetMethod(RequestType.POST).ExecuteSync();
        }

        private UploadManager InitUploadManager(string opName, int opId, int blockSize)
        {
            UploadManager manager = new UploadManager(blockSize, this);
            if (opId > 0)
            {
                manager.SetOperator(opId);
            }
            if (!string.IsNullOrEmpty(opName))
            {
                manager.SetOperator(opName);
            }
            return manager;
        }

        /// <summary>
        /// 异步分块上传本地文件，默认分块上传大小10MB
        /// </summary>
        /// <param name="fullpath"></param>
        /// <param name="opName"></param>
        /// <param name="opId"></param>
        /// <param name="localFilepath"></param>
        /// <param name="overwrite"></param>
        /// <param name="completedEventHandler"></param>
        /// <param name="progressChangeEventHandler"></param>
        /// <returns></returns>
        public bool UploadByBlockAsync(string fullpath, string opName, int opId, string localFilepath, bool overwrite
            , CompletedEventHandler completedEventHandler, ProgressChangeEventHandler progressChangeEventHandler)
        {
            return UploadByBlockAsync(fullpath, opName, opId, localFilepath, overwrite, BlockSize, completedEventHandler, progressChangeEventHandler);
        }

        public bool UploadByBlockAsync(string fullpath, string opName, int opId, string localFilepath, bool overwrite
            , int blockSize, CompletedEventHandler completedEventHandler, ProgressChangeEventHandler progressChangeEventHandler)
        {
            FileStream stream = File.OpenRead(localFilepath);
            return UploadByBlockAsync(fullpath, opName, opId, stream, overwrite, blockSize, completedEventHandler, progressChangeEventHandler);
        }

        /// <summary>
        /// 异步分块上传文件流，默认分块上传大小10MB
        /// </summary>
        /// <param name="fullpath"></param>
        /// <param name="opName"></param>
        /// <param name="opId"></param>
        /// <param name="stream"></param>
        /// <param name="overwrite"></param>
        /// <param name="completedEventHandler"></param>
        /// <param name="progressChangeEventHandler"></param>
        /// <returns></returns>
        public bool UploadByBlockAsync(string fullpath, string opName, int opId, Stream stream, bool overwrite
            , CompletedEventHandler completedEventHandler, ProgressChangeEventHandler progressChangeEventHandler)
        {
            return UploadByBlockAsync(fullpath, opName, opId, stream, overwrite, BlockSize, completedEventHandler, progressChangeEventHandler);
        }

        public bool UploadByBlockAsync(string fullpath, string opName, int opId, Stream stream, bool overwrite
            , int blockSize, CompletedEventHandler completedEventHandler, ProgressChangeEventHandler progressChangeEventHandler)
        {
            UploadManager manager = this.InitUploadManager(opName, opId, blockSize);
            manager.Completed += new UploadManager.CompletedEventHandler(completedEventHandler);
            manager.ProgresChanged += new UploadManager.ProgressChangeEventHandler(progressChangeEventHandler);
            return manager.UploadAsync(stream, fullpath, overwrite);
        }

        /// <summary>
        /// 分块上传本地文件，默认分块上传大小10MB
        /// </summary>
        /// <param name="fullpath"></param>
        /// <param name="opName"></param>
        /// <param name="opId"></param>
        /// <param name="localFilepath"></param>
        /// <param name="overwrite"></param>
        /// <returns></returns>
        public Data.FileInfo UploadByBlock(string fullpath, string opName, int opId, string localFilepath, bool overwrite)
        {
            return UploadByBlock(fullpath, opName, opId, localFilepath, overwrite, BlockSize);
        }

        public Data.FileInfo UploadByBlock(string fullpath, string opName, int opId, string localFilepath, bool overwrite, int blockSize)
        {
            FileStream stream = File.OpenRead(localFilepath);
            return UploadByBlock(fullpath, opName, opId, stream, overwrite, BlockSize);
        }

        /// <summary>
        /// 分块上传文件流，默认分块上传大小10MB
        /// </summary>
        /// <param name="fullpath"></param>
        /// <param name="opName"></param>
        /// <param name="opId"></param>
        /// <param name="localFilePath"></param>
        /// <param name="overwrite"></param>
        /// <param name="blockSize"></param>
        /// <returns></returns>
        public Data.FileInfo UploadByBlock(string fullpath, string opName, int opId, Stream stream, bool overwrite)
        {
            return UploadByBlock(fullpath, opName, opId, stream, overwrite, BlockSize);
        }

        public Data.FileInfo UploadByBlock(string fullpath, string opName, int opId, Stream stream, bool overwrite, int blockSize)
        {
            UploadManager manager = this.InitUploadManager(opName, opId, blockSize);
            return manager.Upload(stream, fullpath, overwrite);
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="fullpaths"></param>
        /// <param name="opName"></param>
        /// <param name="destroy"></param>
        /// <returns></returns>
        public ReturnResult Del(string fullpaths, string opName, bool destroy)
        {
            string url = UrlApiDelFile;
            var parameter = new Dictionary<string, string>();
            parameter.Add("fullpaths", fullpaths);
            parameter.Add("op_name", opName);
            if (destroy)
            {
                parameter.Add("destroy", "1");
            }
            return new RequestHelper(this).SetParams(parameter).SetUrl(url).SetMethod(RequestType.POST).ExecuteSync();
        }

        /// <summary>
        /// 移动文件
        /// </summary>
        /// <param name="fullpath"></param>
        /// <param name="destFullpath"></param>
        /// <param name="opName"></param>
        /// <returns></returns>
        public ReturnResult Move(string fullpath, string destFullpath, string opName)
        {
            string url = UrlApiMoveFile;
            var parameter = new Dictionary<string, string>();
            parameter.Add("fullpath", fullpath);
            parameter.Add("dest_fullpath", destFullpath);
            parameter.Add("op_name", opName);
            return new RequestHelper(this).SetParams(parameter).SetUrl(url).SetMethod(RequestType.POST).ExecuteSync();
        }

        /// <summary>
        /// 获取文件链接
        /// </summary>
        /// <param name="fullpath"></param>
        /// <returns></returns>
        public ReturnResult Link(string fullpath, int deadline, AuthType authType, string password)
        {
            string url = UrlApiLinkFile;
            var parameter = new Dictionary<string, string>();
            parameter.Add("fullpath", fullpath);

            if (deadline != 0)
            {
                parameter.Add("deadline", deadline.ToString());
            }

            if (!authType.Equals(AuthType.Default))
            {
                parameter.Add("auth", authType.ToString().ToLower());
            }
            parameter.Add("password", password);
            return new RequestHelper(this).SetParams(parameter).SetUrl(url).SetMethod(RequestType.POST).ExecuteSync();
        }

        /// <summary>
        /// 关闭文件外链
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public ReturnResult LinkCloseByCode(string code)
        {
            string url = UrlApiLinkClose;
            var parameter = new Dictionary<string, string>();
            parameter.Add("code", code);
            return new RequestHelper(this).SetParams(parameter).SetUrl(url).SetMethod(RequestType.POST).ExecuteSync();
        }

        /// <summary>
        /// 关闭文件外链
        /// </summary>
        /// <param name="fullpath"></param>
        /// <returns></returns>
        public ReturnResult LinkCloseByFullpath(string fullpath)
        {
            string url = UrlApiLinkClose;
            var parameter = new Dictionary<string, string>();
            parameter.Add("fullpath", fullpath);
            return new RequestHelper(this).SetParams(parameter).SetUrl(url).SetMethod(RequestType.POST).ExecuteSync();
        }

        /// <summary>
        /// 获取当前库所有外链
        /// </summary>
        /// <param name="fileOnly"></param>
        /// <returns></returns>
        public ReturnResult GetLinks(bool fileOnly)
        {
            string url = UrlApiGetLink;
            var parameter = new Dictionary<string, string>();
            if (fileOnly)
            {
                parameter.Add("file", "1");
            }
            return new RequestHelper(this).SetParams(parameter).SetUrl(url).SetMethod(RequestType.GET).ExecuteSync();
        }

        /// <summary>
        /// 获取上传地址和临时key
        /// (为网页上传提供上传地址和key)
        /// </summary>
        /// <param name="fullpath"></param>
        /// <param name="timeout">临时key失效时长, 单位秒</param>
        /// <returns></returns>
        public ReturnResult GetUploadServers(string fullpath, int timeout)
        {
            string url = UrlApiUploadServers;
            var parameter = new Dictionary<string, string>();
            parameter.Add("fullpath", fullpath);
            parameter.Add("timeout", timeout.ToString());
            parameter.Add("rand", (new Random()).Next(100000, 999999).ToString());
            return new RequestHelper(this).SetUrl(url).SetMethod(RequestType.GET).ExecuteSync();
        }

        /// <summary>
        /// 获取服务器地址
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public ReturnResult GetServers(string type)
        {
            string url = UrlApiGetServerSite;
            var parameter = new Dictionary<string, string>();
            parameter.Add("type", type);
            return new RequestHelper(this).SetParams(parameter).SetUrl(url).SetMethod(RequestType.GET).ExecuteSync();
        }

        /// <summary>
        /// 复制文件
        /// </summary>
        /// <param name="originFullpath"></param>
        /// <param name="targetFullpath"></param>
        /// <param name="opName"></param>
        /// <returns></returns>
        public ReturnResult Copy(string originFullpath, string targetFullpath, string opName)
        {
            string url = UrlApiCopyFile;
            var parameter = new Dictionary<string, string>();
            parameter.Add("from_fullpath", originFullpath);
            parameter.Add("fullpath", targetFullpath);
            parameter.Add("op_name", opName);
            return new RequestHelper(this).SetParams(parameter).SetUrl(url).SetMethod(RequestType.POST).ExecuteSync();
        }

        /// <summary>
        /// 回收站
        /// </summary>
        /// <param name="start"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public ReturnResult Recyle(int start, int size)
        {
            string url = UrlApiRecycleFile;
            var parameter = new Dictionary<string, string>();
            parameter.Add("start", start.ToString());
            parameter.Add("size", size.ToString());
            return new RequestHelper(this).SetParams(parameter).SetUrl(url).SetMethod(RequestType.GET).ExecuteSync();
        }

        /// <summary>
        /// 恢复删除文件
        /// </summary>
        /// <param name="fullpaths"></param>
        /// <param name="opName"></param>
        /// <returns></returns>
        public ReturnResult Recover(string fullpaths, string opName)
        {
            string url = UrlApiRecoverFile;
            var parameter = new Dictionary<string, string>();
            parameter.Add("fullpaths", fullpaths);
            parameter.Add("op_name", opName);
            return new RequestHelper(this).SetParams(parameter).SetUrl(url).SetMethod(RequestType.POST).ExecuteSync();
        }

        /// <summary>
        /// 彻底删除文件（夹）
        /// </summary>
        /// <param name="fullpaths"></param>
        /// <param name="opName"></param>
        /// <returns></returns>
        public ReturnResult CompletelyDelFile(string[] fullpaths, string opName)
        {
            string url = UrlApiCompletelyDelFile;
            var parameter = new Dictionary<string, string>();
            parameter.Add("fullpaths", Util.StrArrayToString(fullpaths, "|"));
            parameter.Add("op_name", opName);
            return new RequestHelper(this).SetParams(parameter).SetUrl(url).SetMethod(RequestType.POST).ExecuteSync();
        }

        /// <summary>
        /// 获取文件历史
        /// </summary>
        /// <param name="fullpath"></param>
        /// <param name="start"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public ReturnResult History(string fullpath, int start, int size)
        {
            string url = UrlApiHistoryFile;
            var parameter = new Dictionary<string, string>();
            parameter.Add("fullpath", fullpath);
            parameter.Add("start", start.ToString());
            parameter.Add("size", size.ToString());
            return new RequestHelper(this).SetParams(parameter).SetUrl(url).SetMethod(RequestType.GET).ExecuteSync();

        }

        /// <summary>
        /// 通过文件唯一标识获取下载地址
        /// </summary>
        /// <param name="hash"></param>
        /// <returns></returns>
        public ReturnResult GetDownloadUrlByHash(string hash)
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
        public ReturnResult GetDownloadUrlByHash(string hash, bool isOpen, NetType net)
        {
            return GetDownloadUrl(hash, null, isOpen, net, "");
        }

        /// <summary>
        /// 通过文件路径获取下载地址
        /// </summary>
        /// <param name="fullpath"></param>
        /// <returns></returns>
        public ReturnResult GetDownloadUrlByFullpath(string fullpath)
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
        public ReturnResult GetDownloadUrlByFullpath(string fullpath, bool isOpen, NetType net)
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
        private ReturnResult GetDownloadUrl(string hash, string fullpath, bool isOpen, NetType net, string fileName)
        {
            string url = UrlApiGetDownloadUrl;
            var parameter = new Dictionary<string, string>();
            parameter.Add("fullpath", fullpath);
            parameter.Add("hash", hash);
            parameter.Add("open", (isOpen ? "1" : "0"));
            switch (net)
            {
                case NetType.Default:
                    break;
                case NetType.In:
                    parameter.Add("net", net.ToString().ToLower());
                    break;
            }
            return new RequestHelper(this).SetParams(parameter).SetUrl(url).SetMethod(RequestType.GET).ExecuteSync();
        }

        /// <summary>
        /// 文件搜索
        /// </summary>
        /// <param name="keywords"></param>
        /// <param name="path"></param>
        /// <param name="start"></param>
        /// <param name="size"></param>
        /// <param name="scopes"></param>
        /// <returns></returns>
        public ReturnResult Search(string keywords, string path, int start, int size, params ScopeType[] scopes)
        {
            string url = UrlApiSearchFile;
            var parameter = new Dictionary<string, string>();
            parameter.Add("keywords", keywords);
            parameter.Add("path", path);
            JsonArray array = new JsonArray();
            foreach (var s in scopes)
            {
                string scope = Enum.GetName(typeof(ScopeType), s);
                array.Add(scope);
            }
            parameter.Add("scope", array.ToString().ToLower());
            parameter.Add("start", start.ToString());
            parameter.Add("size", size.ToString());
            return new RequestHelper(this).SetParams(parameter).SetUrl(url).SetMethod(RequestType.GET).ExecuteSync();
        }

        /// <summary>
        /// 文件预览地址
        /// </summary>
        /// <param name="fullpath"></param>
        /// <param name="showWaterMark"></param>
        /// <param name="memberName"></param>
        /// <param name="returnThumbnail"></param>
        /// <returns></returns>
        public ReturnResult GetPreviewUrlByFullpath(string fullpath, bool showWatermark, string memberName, bool returnThumbnail)
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
        public ReturnResult GetPreviewUrlByHash(string hash, bool showWatermark, string memberName, bool returnThumbnail)
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
        private ReturnResult PreviewUrl(string hash, string fullpath, bool showWatermark, string memberName, bool returnThumbnail)
        {
            string url = UrlApiPreviewUrl;
            var parameter = new Dictionary<string, string>();
            parameter.Add("hash", hash);
            parameter.Add("fullpath", fullpath);
            if (showWatermark)
            {
                parameter.Add("watermark", "1");
                parameter.Add("member_name", memberName);
            }
            if (returnThumbnail)
            {
                parameter.Add("thumbnail", "1");
            }
            return new RequestHelper(this).SetParams(parameter).SetUrl(url).SetMethod(RequestType.GET).ExecuteSync();
        }

        /// <summary>
        /// 获取文件夹权限
        /// </summary>
        /// <param name="fullpath"></param>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public ReturnResult GetPermission(string fullpath, int memberId)
        {
            string url = UrlApiGetPermission;
            var parameter = new Dictionary<string, string>();
            parameter.Add("fullpath", fullpath);
            parameter.Add("member_id", memberId.ToString());
            return new RequestHelper(this).SetParams(parameter).SetUrl(url).SetMethod(RequestType.GET).ExecuteSync();
        }

        /// <summary>
        /// 修改文件夹权限
        /// </summary>
        /// <param name="fullpath"></param>
        /// <param name="memberId"></param>
        /// <param name="permissions"></param>
        /// <returns></returns>
        public ReturnResult SetPermission(string fullpath, int memberId, params FilePermissions[] permissions)
        {
            string url = UrlApiSetPermission;
            var parameter = new Dictionary<string, string>();
            parameter.Add("fullpath", fullpath);

            var jsonArray = new JsonArray();
            var jsonObject = new JsonObject();
            foreach (var s in permissions)
            {
                string scope = Enum.GetName(typeof(FilePermissions), s);
                jsonArray.Add(scope);
            }

            jsonObject.Add(memberId.ToString(), jsonArray);
            parameter.Add("permissions", jsonObject.ToString().ToLower());
            return new RequestHelper(this).SetParams(parameter).SetUrl(url).SetMethod(RequestType.POST).ExecuteSync();
        }

        /// <summary>
        /// 添加标签
        /// </summary>
        /// <param name="fullpath"></param>
        /// <param name="tags"></param>
        /// <returns></returns>
        public ReturnResult AddTag(string fullpath, string[] tags)
        {
            string url = UrlApiAddTag;
            var parameter = new Dictionary<string, string>();
            parameter.Add("fullpath", fullpath);
            parameter.Add("tag", Util.StrArrayToString(tags, ";"));
            return new RequestHelper(this).SetParams(parameter).SetUrl(url).SetMethod(RequestType.POST).ExecuteSync();
        }

        /// <summary>
        /// 删除标签
        /// </summary>
        /// <param name="fullpath"></param>
        /// <param name="tags"></param>
        /// <returns></returns>
        public ReturnResult DelTag(string fullpath, string[] tags)
        {
            string url = UrlApiDelTag;
            var parameter = new Dictionary<string, string>();
            parameter.Add("fullpath", fullpath);
            parameter.Add("tag", Util.StrArrayToString(tags, ";"));
            return new RequestHelper(this).SetParams(parameter).SetUrl(url).SetMethod(RequestType.POST).ExecuteSync();
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