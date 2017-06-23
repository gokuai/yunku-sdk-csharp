using System;
using System.IO;
using System.Net;
using System.Threading;
using YunkuEntSDK.Net;
using YunkuEntSDK.UtilClass;

namespace YunkuEntSDK
{
    public class EntFileManager : HttpEngine
    {
        private const long UploadSizeLimit = 52428800; //50MB
        private const string LibHost = HostConfig.ApiEntHost;
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
        private const string UrlApiUpdateCount = LibHost + "/1/file/updates_count";
        private const string UrlApiGetServerSite = LibHost + "/1/file/servers";
        private const string UrlApiCreateFileByUrl = LibHost + "/1/file/create_file_by_url";
        private const string UrlApiUploadServers = LibHost + "/1/file/upload_servers";
        private const string UrlApiCopyFile = LibHost + "/1/file/copy";
        private const string UrlApiRecycleFile = LibHost + "/1/file/recycle";
        private const string UrlApiRecoverFile = LibHost + "/1/file/recover";
        private const string UrlApiCompletelyDelFile = LibHost + "/1/file/del_completely";
        private const string UrlApiHistoryFile = LibHost + "/1/file/history";
        private const string UrlApiGetUploadUrl = LibHost + "/1/file/download_url";
        private const string UrlApiSearchFile = LibHost + "/1/file/search";
        private const string UrlApiPreviewUrl = LibHost + "/1/file/preview_url";
        private const string UrlApiGetPermission = LibHost + "/1/file/get_permission";
        private const string UrlApiSetPermission = LibHost + "/1/file/file_permission";
        private const string UrlApiAddTag = LibHost + "/1/file/add_tag";
        private const string UrlApiDelTag = LibHost + "/1/file/del_tag";

        public delegate void CompletedEventHandler(object sender, CompletedEventArgs e);

        public delegate void ProgressChangeEventHandler(object sender, ProgressEventArgs e);

        public EntFileManager(string orgClientId, string orgClientSecret) : base(orgClientId, orgClientSecret)
        {
        }


        /// <summary>
        /// 获取文件列表
        /// </summary>
        /// <param name="start"></param>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        public string GetFileList(int start, string fullPath)
        {
            var request = new HttpRequestSyn { RequestUrl = UrlApiFilelist };
            request.AppendParameter("org_client_id", _clientId);
            request.AppendParameter("dateline", Util.GetUnixDataline() + "");
            request.AppendParameter("start", start + "");
            request.AppendParameter("fullpath", fullPath);
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
            if (showDelete)
            {
                request.AppendParameter("showdel", "1");
            }
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
        /// 复制文件
        /// </summary>
        /// <param name="originFullPath"></param>
        /// <param name="targetFullPath"></param>
        /// <param name="opName"></param>
        /// <returns></returns>
        public string Copy(string originFullpath, string targetFullpath, string opName)
        {
            var request = new HttpRequestSyn { RequestUrl = UrlApiCopyFile };
            request.AppendParameter("org_client_id", _clientId);
            request.AppendParameter("from_fullpath", originFullpath);
            request.AppendParameter("fullpath", targetFullpath);
            request.AppendParameter("op_name", opName);
            request.AppendParameter("dateline", Util.GetUnixDataline() + "");
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.Post;
            request.Request();
            return request.Result;
        }

        /// <summary>
        /// 回收站
        /// </summary>
        /// <param name="start"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public string Recyle(int start, int size)
        {
            var request = new HttpRequestSyn { RequestUrl = UrlApiRecycleFile };
            request.AppendParameter("org_client_id", _clientId);
            request.AppendParameter("start", start + "");
            request.AppendParameter("size", size + "");
            request.AppendParameter("dateline", Util.GetUnixDataline() + "");
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.Get;
            request.Request();
            return request.Result;
        }

        /// <summary>
        /// 恢复删除文件
        /// </summary>
        /// <param name="fullpaths"></param>
        /// <param name="opName"></param>
        /// <returns></returns>
        public string Recover(string fullpaths, string opName)
        {
            var request = new HttpRequestSyn { RequestUrl = UrlApiRecoverFile };
            request.AppendParameter("org_client_id", _clientId);
            request.AppendParameter("fullpaths", fullpaths);
            request.AppendParameter("op_name", opName);
            request.AppendParameter("dateline", Util.GetUnixDataline() + "");
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.Post;
            request.Request();
            return request.Result;
        }

        /// <summary>
        /// 彻底删除文件（夹）
        /// </summary>
        /// <param name="fullpaths"></param>
        /// <param name="opName"></param>
        /// <returns></returns>
        public string CompletelyDelFile(string[] fullpaths, string opName)
        {
            var request = new HttpRequestSyn { RequestUrl = UrlApiCompletelyDelFile };
            request.AppendParameter("org_client_id", _clientId);
            request.AppendParameter("fullpaths", Util.StrArrayToString(fullpaths, "|") + "");
            request.AppendParameter("op_name", opName);
            request.AppendParameter("dateline", Util.GetUnixDataline() + "");
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.Post;
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
        /// <param name="fullPath"></param>
        /// <param name="isOpen"></param>
        /// <param name="net"></param>
        /// <returns></returns>
        public string GetDownloadUrlByFullPath(string fullPath, bool isOpen, NetType net)
        {
            return GetDownloadUrl(null, fullPath, isOpen, net, "");
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
            var request = new HttpRequestSyn { RequestUrl = UrlApiGetUploadUrl };
            request.AppendParameter("org_client_id", _clientId);
            request.AppendParameter("fullpath", fullpath);
            request.AppendParameter("hash", hash);
            request.AppendParameter("open", (isOpen ? 1 : 0) + "");
            switch (net)
            {
                case NetType.Default:
                    break;
                case NetType.In:
                    request.AppendHeaderParameter("net", net.ToString().ToLower());
                    break;
            }
            request.AppendParameter("dateline", Util.GetUnixDataline() + "");
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.Get;
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
        public string Search(string keyWords, string path, int start, int size, params ScopeType[] scopes)
        {
            var request = new HttpRequestSyn { RequestUrl = UrlApiSearchFile };
            request.AppendParameter("org_client_id", _clientId);
            request.AppendParameter("keywords", keyWords);
            request.AppendParameter("path", path);
            //if (scopes != null)
            //{
            //    // TODO 需要解决组json
            //   JsonArray array =  new JsonArray();
            //    if(scopes != null)
            //    {
            //        foreach (var s in Enum.GetValues(typeof(ScopeType)))
            //        {
            //            string name = Enum.GetName(typeof(ScopeType), scopes);
            //            //array.Add(name);
            //        }
            //    }
            //    request.AppendParameter("scope", array.ToString().ToLower());
            //}
            request.AppendParameter("start", start + "");
            request.AppendParameter("size", size + "");
            request.AppendParameter("dateline", Util.GetUnixDataline() + "");
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.Get;
            request.Request();
            return request.Result;
        }

        /// <summary>
        /// 文件预览地址
        /// </summary>
        /// <param name="fullpath"></param>
        /// <param name="showWaterMark"></param>
        /// <param name="memberName"></param>
        /// <returns></returns>
        public string PreviewUrl(string fullpath, bool showWaterMark, string memberName)
        {
            var request = new HttpRequestSyn { RequestUrl = UrlApiPreviewUrl };
            request.AppendParameter("org_client_id", _clientId);
            request.AppendParameter("fullpath", fullpath);
            request.AppendParameter("member_name", memberName);
            request.AppendParameter("watermark", (showWaterMark ? 1 : 0) + "");
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
        public string SetPermission(string fullpath, int memberId, params FilePermissions[] permissions)
        {
            var request = new HttpRequestSyn { RequestUrl = UrlApiSetPermission };
            request.AppendParameter("org_client_id", _clientId);
            request.AppendParameter("fullpath", fullpath);
            request.AppendParameter("member_id", memberId + "");
            if (permissions != null)
            {
                var jsonArray = new JsonArray();
                var jsonObject = new JsonObject();
                foreach (var p in permissions)
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
        public enum ScopeType : int 
        {
            Tag = 1,
            Content = 2,
            FileName = 4
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