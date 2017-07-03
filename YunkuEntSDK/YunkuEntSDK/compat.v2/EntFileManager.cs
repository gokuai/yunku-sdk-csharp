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
            var request = new HttpRequestSyn { RequestUrl = UrlApiFilelist };
            request.AppendParameter("org_client_id", _clientId);
            request.AppendParameter("dateline", Util.GetUnixDataline() + "");
            request.AppendParameter("start", start + "");
            request.AppendParameter("size", size + "");
            request.AppendParameter("fullpath", fullPath);
            if (dirOnly)
            {
                request.AppendParameter("dir", "1");
            }
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.Get;
            request.Request();
            return request.Result;
        }

        /// <summary>
        /// 获取更新列表
        /// </summary>
        /// <param name="isCompare"></param>
        /// <param name="fetchDateline"></param>
        /// <returns></returns>
        public string GetUpdateList(bool isCompare, long fetchDateline)
        {
            var request = new HttpRequestSyn { RequestUrl = UrlApiUpdateList };
            request.AppendParameter("org_client_id", _clientId);
            request.AppendParameter("dateline", Util.GetUnixDataline() + "");
            if (isCompare)
            {
                request.AppendParameter("mode", "compare");
            }
            request.AppendParameter("fetch_dateline", fetchDateline + "");
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.Get;
            request.Request();
            return request.Result;
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
            var request = new HttpRequestSyn { RequestUrl = UrlApiUpdateCount };
            request.AppendParameter("org_client_id", _clientId);
            request.AppendParameter("dateline", Util.GetUnixDataline() + "");
            request.AppendParameter("begin_dateline", beginDateline + "");
            request.AppendParameter("end_dateline", endDateline + "");
            request.AppendParameter("showdel", (showDelete ? 1 : 0) + "");
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.Get;
            request.Request();
            return request.Result;
        }

        /// <summary>
        /// 获取文件信息
        /// </summary>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        public string GetFileInfo(string fullPath, NetType type, bool getAttribute)
        {
            var request = new HttpRequestSyn { RequestUrl = UrlApiFileInfo };
            request.AppendParameter("org_client_id", _clientId);
            request.AppendParameter("dateline", Util.GetUnixDataline() + "");
            request.AppendParameter("fullpath", fullPath);
            request.AppendParameter("attribute", (getAttribute ? 1 : 0) + "");
            switch (type)
            {
                case NetType.Default:
                    break;
                case NetType.In:
                    request.AppendHeaderParameter("net", type.ToString().ToLower());
                    break;
            }
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.Get;
            request.Request();
            return request.Result;
        }

        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="fullPath"></param>
        /// <param name="opName"></param>
        /// <returns></returns>
        public string CreateFolder(string fullPath, string opName)
        {
            var request = new HttpRequestSyn { RequestUrl = UrlApiCreateFolder };
            request.AppendParameter("org_client_id", _clientId);
            request.AppendParameter("dateline", Util.GetUnixDataline() + "");
            request.AppendParameter("fullpath", fullPath);
            request.AppendParameter("op_name", opName);
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.Post;
            request.Request();
            return request.Result;
        }

        /// <summary>
        /// 通过文件流上传
        /// </summary>
        /// <param name="fullPath"></param>
        /// <param name="opName"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        public string CreateFile(string fullPath, string opName, Stream stream)
        {
            if (stream.Length > UploadSizeLimit)
            {
                LogPrint.Print("文件大小超过50MB");
                return "";
            }
            long dateline = Util.GetUnixDataline();
            var request = new HttpRequestSyn { RequestUrl = UrlApiCreateFile };
            string[] arr = { dateline + "", "file", fullPath, opName, _clientId };
            var data = new MsMultiPartFormData();
            request.ContentType = "multipart/form-data;boundary=" + data.Boundary;
            string filename = Util.GetFileNameFromPath(fullPath);
            data.AddStreamFile("file", filename, Util.ReadToEnd(stream));
            data.AddParams("org_client_id", _clientId);
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
            return request.Result;
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
            if (File.Exists(fullPath))
            {
                using (var fs = new FileStream(localPath, FileMode.Open))
                {
                    Stream stream = fs;
                    return CreateFile(fullPath, opName, stream);
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
            var request = new HttpRequestSyn { RequestUrl = UrlApiDelFile };
            request.AppendParameter("org_client_id", _clientId);
            request.AppendParameter("dateline", Util.GetUnixDataline() + "");
            request.AppendParameter("fullpaths", fullPaths);
            request.AppendParameter("op_name", opName);
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.Post;
            request.Request();
            return request.Result;
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
            var request = new HttpRequestSyn { RequestUrl = UrlApiMoveFile };
            request.AppendParameter("org_client_id", _clientId);
            request.AppendParameter("dateline", Util.GetUnixDataline() + "");
            request.AppendParameter("fullpath", fullPath);
            request.AppendParameter("dest_fullpath", destFullPath);
            request.AppendParameter("op_name", opName);
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.Post;
            request.Request();
            return request.Result;
        }

        /// <summary>
        /// 获取文件链接
        /// </summary>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        public string Link(string fullPath, int deadline, AuthType authType, string password)
        {
            var request = new HttpRequestSyn { RequestUrl = UrlApiLinkFile };
            request.AppendParameter("org_client_id", _clientId);
            request.AppendParameter("dateline", Util.GetUnixDataline() + "");
            request.AppendParameter("fullpath", fullPath);
            request.AppendParameter("password", password);

            if (deadline != 0)
            {
                request.AppendParameter("deadline", deadline + "");
            }

            if (!authType.Equals(AuthType.Default))
            {
                request.AppendParameter("auth", authType.ToString().ToLower());
            }

            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.Post;
            request.Request();
            return request.Result;
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
            var request = new HttpRequestSyn { RequestUrl = UrlApiSendmsg };
            request.AppendParameter("org_client_id", _clientId);
            request.AppendParameter("dateline", Util.GetUnixDataline() + "");
            request.AppendParameter("title", title);
            request.AppendParameter("text", text);
            request.AppendParameter("image", image);
            request.AppendParameter("url", linkUrl);
            request.AppendParameter("op_name", opName);
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.Post;
            request.Request();
            return request.Result;
        }

        /// <summary>
        /// 获取当前库所有外链
        /// </summary>
        /// <param name="fileOnly"></param>
        /// <returns></returns>
        public string Links(bool fileOnly)
        {
            var request = new HttpRequestSyn { RequestUrl = UrlApiGetLink };
            request.AppendParameter("org_client_id", _clientId);
            request.AppendParameter("dateline", Util.GetUnixDataline() + "");
            if (fileOnly)
            {
                request.AppendParameter("file", "1");
            }
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.Get;
            request.Request();
            return request.Result;
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
            var request = new HttpRequestSyn { RequestUrl = UrlApiCreateFileByUrl };
            request.AppendParameter("org_client_id", _clientId);
            request.AppendParameter("fullpath", fullPath);
            request.AppendParameter("dateline", Util.GetUnixDataline() + "");
            if (opId > 0)
            {
                request.AppendParameter("op_id", opId + "");
            }
            else
            {
                request.AppendParameter("op_name", opName);
            }
            request.AppendParameter("overwrite", (overwrite ? 1 : 0) + "");
            request.AppendParameter("url", fileUrl);
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.Post;
            request.Request();
            return request.Result;
        }

        /// <summary>
        /// 获取上传地址
        /// (支持50MB以上文件的上传)
        /// </summary>
        /// <returns></returns>
        public string GetUploadServers()
        {
            var request = new HttpRequestSyn { RequestUrl = UrlApiUploadServers };
            request.AppendParameter("org_client_id", _clientId);
            request.AppendParameter("dateline", Util.GetUnixDataline() + "");
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.Get;
            request.Request();
            return request.Result;
        }

        public string GetServerSite(string type)
        {
            var request = new HttpRequestSyn { RequestUrl = UrlApiGetServerSite };
            request.AppendParameter("org_client_id", _clientId);
            request.AppendParameter("type", type);
            request.AppendParameter("dateline", Util.GetUnixDataline() + "");
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.Get;
            request.Request();
            return request.Result;
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
            var request = new HttpRequestSyn { RequestUrl = UrlApiHistoryFile };
            request.AppendParameter("org_client_id", _clientId);
            request.AppendParameter("fullpath", fullpath);
            request.AppendParameter("start", start + "");
            request.AppendParameter("size", size + "");
            request.AppendParameter("dateline", Util.GetUnixDataline() + "");
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.Post;
            request.Request();
            return request.Result;
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
            var request = new HttpRequestSyn { RequestUrl = UrlApiSearchFile };
            request.AppendParameter("org_client_id", _clientId);
            request.AppendParameter("keywords", keyWords);
            request.AppendParameter("path", path);
            request.AppendParameter("start", start + "");
            request.AppendParameter("size", size + "");
            request.AppendParameter("dateline", Util.GetUnixDataline() + "");
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.Get;
            request.Request();
            return request.Result;
        }

        /// <summary>
        /// 获取文件夹权限
        /// </summary>
        /// <param name="fullpath"></param>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public string GetPermission(string fullpath, int memberId)
        {
            var request = new HttpRequestSyn { RequestUrl = UrlApiGetPermission };
            request.AppendParameter("org_client_id", _clientId);
            request.AppendParameter("fullpath", fullpath);
            request.AppendParameter("member_id", memberId + "");
            request.AppendParameter("dateline", Util.GetUnixDataline() + "");
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.Post;
            request.Request();
            return request.Result;
        }

        /// <summary>
        /// 修改文件夹权限
        /// </summary>
        /// <param name="fullpath"></param>
        /// <param name="memberId"></param>
        /// <param name="permissions"></param>
        /// <returns></returns>
        public string SetPermission(string fullpath, int memberId, FilePermissions permissions)
        {
            var request = new HttpRequestSyn { RequestUrl = UrlApiSetPermission };
            request.AppendParameter("org_client_id", _clientId);
            request.AppendParameter("fullpath", fullpath);
            if (permissions != null)
            {
                var jsonArray = new JsonArray();
                var jsonObject = new JsonObject();
                foreach (string p in Enum.GetNames(typeof(FilePermissions)))
                {
                    jsonArray.Add(p);
                }
                jsonObject.Add(memberId + "", jsonArray);
                request.AppendParameter("permissions", jsonObject.ToString().ToLower());
            }
            request.AppendParameter("dateline", Util.GetUnixDataline() + "");
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.Post;
            request.Request();
            return request.Result;
        }

        /// <summary>
        /// 添加标签
        /// </summary>
        /// <param name="fullpath"></param>
        /// <param name="tags"></param>
        /// <returns></returns>
        public string AddTag(string fullpath, string[] tags)
        {
            var request = new HttpRequestSyn { RequestUrl = UrlApiAddTag };
            request.AppendParameter("org_client_id", _clientId);
            request.AppendParameter("fullpath", fullpath);
            request.AppendParameter("tag", Util.StrArrayToString(tags, ";") + "");
            request.AppendParameter("dateline", Util.GetUnixDataline() + "");
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.Post;
            request.Request();
            return request.Result;
        }

        /// <summary>
        /// 删除标签
        /// </summary>
        /// <param name="fullpath"></param>
        /// <param name="tags"></param>
        /// <returns></returns>
        public string DelTag(string fullpath, string[] tags)
        {
            var request = new HttpRequestSyn { RequestUrl = UrlApiDelTag };
            request.AppendParameter("org_client_id", _clientId);
            request.AppendParameter("fullpath", fullpath);
            request.AppendParameter("tag", Util.StrArrayToString(tags, ";") + "");
            request.AppendParameter("dateline", Util.GetUnixDataline() + "");
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.Post;
            request.Request();
            return request.Result;
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

        [Flags]
        public enum FilePermissions
        {
            FileRead = 1,
            FilePreview = 2,
            FileWrite = 4,
            FileDelete = 8
        }
    }
}
