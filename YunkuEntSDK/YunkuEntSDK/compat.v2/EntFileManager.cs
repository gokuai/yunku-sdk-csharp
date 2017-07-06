using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using YunkuEntSDK.Net;
using YunkuEntSDK.UtilClass;

namespace YunkuEntSDK.compat.v2
{
    public class EntFileManager : HttpEngine
    {
        private const string Log_Tag = "EntFileManager_V2";

        private const long UploadSizeLimit = 52428800; //50MB
        private const string LibHost = HostConfig.ApiEntHostV2;
        private const string UrlApiFilelist = LibHost + "/1/file/ls";
        private const string UrlApiUpdateList = LibHost + "/1/file/updates";
        private const string UrlApiFileInfo = LibHost + "/1/file/info";
        private const string UrlApiCreateFolder = LibHost + "/1/file/create_folder";
        private const string UrlApiCreateFile = LibHost + "/1/file/create_file";
        private const string UrlApiDelFile = LibHost + "/1/file/del";
        private const string UrlApiMoveFile = LibHost + "/1/file/move";
        private const string UrlApiHistoryFile = LibHost + "/1/file/history";
        private const string UrlApiLinkFile = LibHost + "/1/file/link";
        private const string UrlApiSendmsg = LibHost + "/1/file/sendmsg";
        private const string UrlApiGetLink = LibHost + "/1/file/links";
        private const string UrlApiUpdateCount = LibHost + "/1/file/updates_count";
        private const string UrlApiGetServerSite = LibHost + "/1/file/servers";
        private const string UrlApiCreateFileByUrl = LibHost + "/1/file/create_file_by_url";
        private const string UrlApiUploadServers = LibHost + "/1/file/upload_servers";
        private const string UrlApiSearchFile = LibHost + "/1/file/search";
        private const string UrlApiGetPermission = LibHost + "/1/file/get_permission";
        private const string UrlApiSetPermission = LibHost + "/1/file/file_permission";
        private const string UrlApiAddTag = LibHost + "/1/file/add_tag";
        private const string UrlApiDelTag = LibHost + "/1/file/del_tag";

        public delegate void CompletedEventHandler(object sender, CompletedEventArgs e);

        public delegate void ProgressChangeEventHandler(object sender, ProgressEventArgs e);

        public EntFileManager(string orgClientId, string OrgClientScret) : base(orgClientId, OrgClientScret)
        {

        }

        /// <summary>
        /// 获取根目录文件列表
        /// </summary>
        /// <returns></returns>
        public string GetFileList()
        {
            return this.GetFileList(0, "", 100, false);
        }

        /// <summary>
        /// 获取文件列表
        /// </summary>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        public string GetFileList(string fullPath)
        {
            return this.GetFileList(0, fullPath, 100, false);
        }

        /// <summary>
        /// 获取文件列表
        /// </summary>
        /// <param name="start"></param>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        public string GetFileList(int start, string fullPath, int size, bool dirOnly)
        {
            string url = UrlApiFilelist;
            var parameter = new Dictionary<string, string>();
            parameter.Add("org_client_id", _clientId);
            parameter.Add("dateline", Util.GetUnixDataline() + "");
            parameter.Add("start", start + "");
            parameter.Add("fullpath", fullPath);
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
        /// <param name="fullPath"></param>
        /// <returns></returns>
        public string GetFileInfo(string fullPath, NetType type, bool getAttribute)
        {
            string url = UrlApiFileInfo;
            var parameter = new Dictionary<string, string>();
            parameter.Add("org_client_id", _clientId);
            parameter.Add("dateline", Util.GetUnixDataline() + "");
            parameter.Add("fullpath", fullPath);
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
        /// <param name="fullPath"></param>
        /// <param name="opName"></param>
        /// <returns></returns>
        public string CreateFolder(string fullPath, string opName)
        {
            string url = UrlApiCreateFolder;
            var parameter = new Dictionary<string, string>();
            parameter.Add("org_client_id", _clientId);
            parameter.Add("dateline", Util.GetUnixDataline() + "");
            parameter.Add("fullpath", fullPath);
            parameter.Add("op_name", opName);
            parameter.Add("sign", GenerateSign(parameter));
            return new RequestHelper().SetParams(parameter).SetUrl(url).SetMethod(RequestType.Post).ExecuteSync();
        }

        /// <summary>
        /// 通过文件流上传
        /// </summary>
        /// <param name="fullPath"></param>
        /// <param name="opName"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        public string CreateFile(string fullPath, string opName, Stream stream, bool overWrite)
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
            parameter.Add("fullpath", fullPath);
            parameter.Add("op_name", opName);
            parameter.Add("overwrite", (overWrite ? 1 : 0) + "");
            parameter.Add("filefield", "file");

            var data = new MsMultiPartFormData();
            string fileName = Util.GetFileNameFromPath(fullPath);
            data.AddStreamFile("file", fileName, Util.ReadToEnd(stream));
            data.AddParams("org_client_id", _clientId);
            data.AddParams("dateline", dateline + "");
            data.AddParams("fullpath", fullPath);
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
        /// <param name="fullPath"></param>
        /// <param name="opName"></param>
        /// <param name="localPath"></param>
        /// <returns></returns>
        public string CreateFile(string fullPath, string opName, string localPath)
        {
            if (File.Exists(localPath))
            {
                using (var fs = new FileStream(localPath, FileMode.Open))
                {
                    Stream stream = fs;
                    return CreateFile(fullPath, opName, stream, true);
                }
            }
            else
            {
                LogPrint.Print(fullPath + " file not exist");
                return "";
            }
        }


        public Thread UploadByBlock(string fullPath, string opName, int opId, string localFilePath, bool overWrite
            , CompletedEventHandler completedEventHandler, ProgressChangeEventHandler progressChangeEventHandler)
        {
            UploadManager uploadManager = new UploadManager(UrlApiCreateFile, localFilePath,
                fullPath, opName, opId, _clientId, Util.GetUnixDataline(), _clientSecret, overWrite);
            uploadManager.Completed += new UploadManager.CompletedEventHandler(completedEventHandler);
            uploadManager.ProgresChanged += new UploadManager.ProgressChangeEventHandler(progressChangeEventHandler);

            Thread thread = new Thread(uploadManager.DoUpload);
            thread.Start();
            return thread;
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="fullPaths"></param>
        /// <param name="opName"></param>
        /// <returns></returns>
        public string Del(string fullPaths, string opName)
        {
            string url = UrlApiDelFile;
            var parameter = new Dictionary<string, string>();
            parameter.Add("org_client_id", _clientId);
            parameter.Add("dateline", Util.GetUnixDataline() + "");
            parameter.Add("fullpaths", fullPaths);
            parameter.Add("op_name", opName);
            parameter.Add("sign", GenerateSign(parameter));
            return new RequestHelper().SetParams(parameter).SetUrl(url).SetMethod(RequestType.Post).ExecuteSync();
        }

        /// <summary>
        /// 移动文件
        /// </summary>
        /// <param name="fullPath"></param>
        /// <param name="destFullPath"></param>
        /// <param name="opName"></param>
        /// <returns></returns>
        public string Move(string fullPath, string destFullPath, string opName)
        {
            string url = UrlApiMoveFile;
            var parameter = new Dictionary<string, string>();
            parameter.Add("org_client_id", _clientId);
            parameter.Add("dateline", Util.GetUnixDataline() + "");
            parameter.Add("fullpath", fullPath);
            parameter.Add("dest_fullpath", destFullPath);
            parameter.Add("op_name", opName);
            parameter.Add("sign", GenerateSign(parameter));
            return new RequestHelper().SetParams(parameter).SetUrl(url).SetMethod(RequestType.Post).ExecuteSync();
        }

        /// <summary>
        /// 获取文件链接
        /// </summary>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        public string Link(string fullPath, int deadline, AuthType authType, string password)
        {
            string url = UrlApiLinkFile;
            var parameter = new Dictionary<string, string>();
            parameter.Add("org_client_id", _clientId);
            parameter.Add("dateline", Util.GetUnixDataline() + "");
            parameter.Add("fullpath", fullPath);

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
        /// 发送消息
        /// </summary>
        /// <param name="title"></param>
        /// <param name="text"></param>
        /// <param name="image"></param>
        /// <param name="linkUrl"></param>
        /// <param name="opName"></param>
        /// <returns></returns>
        public string SendMsg(string title, string text, string image, string linkUrl, string opName)
        {
            string url = UrlApiSendmsg;
            var parameter = new Dictionary<string, string>();
            parameter.Add("org_client_id", _clientId);
            parameter.Add("dateline", Util.GetUnixDataline() + "");
            parameter.Add("title", title);
            parameter.Add("text", text);
            parameter.Add("image", image);
            parameter.Add("url", linkUrl);
            parameter.Add("op_name", opName);
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
        /// <param name="fullPath"></param>
        /// <param name="opId"></param>
        /// <param name="opName"></param>
        /// <param name="overwrite"></param>
        /// <param name="fileUrl"></param>
        /// <returns></returns>
        public string CreateFileByUrl(string fullPath, int opId, string opName, bool overwrite, string fileUrl)
        {
            string url = UrlApiCreateFileByUrl;
            var parameter = new Dictionary<string, string>();
            parameter.Add("org_client_id", _clientId);
            parameter.Add("fullpath", fullPath);
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
        /// 文件搜索
        /// </summary>
        /// <param name="keyWords"></param>
        /// <param name="path"></param>
        /// <param name="start"></param>
        /// <param name="size"></param>
        /// <param name="scopes"></param>
        /// <returns></returns>
        public string Search(string keyWords, string path, int start, int size)
        {
            string url = UrlApiSearchFile;
            var parameter = new Dictionary<string, string>();
            parameter.Add("org_client_id", _clientId);
            parameter.Add("dateline", Util.GetUnixDataline() + "");
            parameter.Add("keywords", keyWords);
            parameter.Add("path", path);
            parameter.Add("start", start + "");
            parameter.Add("size", size + "");
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

        /// <summary>
        /// 复制一个EntFileManager对象
        /// </summary>
        /// <returns></returns>
        public EntFileManager Clone()
        {
            return new EntFileManager(_clientId, _clientSecret);
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

        public enum FilePermissions
        {
            FileRead,
            FilePreview,
            FileWrite,
            FileDelete
        }
    }
}
