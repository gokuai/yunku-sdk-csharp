using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using YunkuEntSDK.Data;
using YunkuEntSDK.Net;
using YunkuEntSDK.UtilClass;

namespace YunkuEntSDK
{

     internal class YunkuManager:ParentManager,IEntLibMethod,IEntMethod
    {
        const string OAUTH_HOST = "http://zka.goukuai.cn";
        const string LIB_HOST = "http://zka-lib.goukuai.cn";
        const string URL_API_TOKEN = OAUTH_HOST + "/oauth2/token";
        const string URL_API_CREATE_LIB = LIB_HOST + "/1/org/create";
        const string URL_API_GET_LIB_LIST = LIB_HOST + "/1/org/ls";
        const string URL_API_BIND = LIB_HOST + "/1/org/bind";
        const string URL_API_UNBIND = LIB_HOST + "/1/org/unbind";
        const string URL_API_FILELIST = LIB_HOST + "/1/file/ls";
        const string URL_API_UPDATE_LIST = LIB_HOST + "/1/file/updates";
        const string URL_API_FILE_INFO = LIB_HOST + "/1/file/info";
        const string URL_API_CREATE_FOLDER = LIB_HOST + "/1/file/create_folder";
        const string URL_API_CREATE_FILE = LIB_HOST + "/1/file/create_file";
        const string URL_API_DEL_FILE = LIB_HOST + "/1/file/del";
        const string URL_API_MOVE_FILE = LIB_HOST + "/1/file/move";
        const string URL_API_LINK_FILE = LIB_HOST + "/1/file/link";
        const string URL_API_SENDMSG = LIB_HOST + "/1/file/sendmsg";

        const string URL_API_GET_MEMBERS = LIB_HOST + "/1/org/get_members";
        const string URL_API_ADD_MEMBERS = LIB_HOST + "/1/org/add_member";
        const string URL_API_SET_MEMBER_ROLE = LIB_HOST + "/1/org/set_member_role";
        const string URL_API_DEL_MEMBER = LIB_HOST + "/1/org/del_member";
        const string URL_API_GET_GROUPS = LIB_HOST + "/1/org/get_groups";
        const string URL_API_ADD_GROUP = LIB_HOST + "/1/org/add_group";
        const string URL_API_DEL_GROUP = LIB_HOST + "/1/org/del_group";
        const string URL_API_SET_GROUP_ROLE = LIB_HOST + "/1/org/set_group_role";

        const string URL_API_ENT_GET_GROUPS = LIB_HOST + "/1/ent/get_groups";
        const string URL_API_ENT_GET_MEMBERS = LIB_HOST + "/1/ent/get_members";
        const string URL_API_ENT_GET_ROLES = LIB_HOST + "/1/ent/get_roles";
        const string URL_API_ENT_SYNC_MEMBER = LIB_HOST + "/1/ent/sync_member";

        public YunkuManager(string uesrname, string password, string client_id, string client_secret)
            : base(uesrname, password, client_id, client_secret)
        {
        }

        public HttpStatusCode StatusCode
        {
            private set;
            get;
        }


        public string AccessToken(bool isEnt)
        {
            HttpRequestSyn request = new HttpRequestSyn();
            request.RequestUrl = URL_API_TOKEN;
            request.AppendParameter("username", _username);
            request.AppendParameter("password", _password);
            request.AppendParameter("client_id", _clientId);
            request.AppendParameter("client_secret", _clientSecret);
            request.AppendParameter("grant_type", isEnt ? "ent_password" : "password");
            request.RequestMethod = RequestType.POST;
            request.Request();
            this.StatusCode = request.Code;
            string result = request.Result;

            OauthData data = OauthData.Create(result);
            if (request.Code == HttpStatusCode.OK)
            {
                _token = data.Token;
            }
            return result;
        }

        public string Create(string orgName, int orgCapacity, string storagePointName, string orgDesc)
        {
            HttpRequestSyn request = new HttpRequestSyn();
            request.RequestUrl = URL_API_CREATE_LIB;
            request.AppendParameter("token", _token);
            request.AppendParameter("token_type", "ent");
            request.AppendParameter("org_name", orgName);
            request.AppendParameter("org_capacity", orgCapacity + "");
            request.AppendParameter("storage_point_name", storagePointName);
            request.AppendParameter("org_desc", orgDesc);
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.POST;
            request.Request();
            this.StatusCode = request.Code;
            return request.Result;
        }


        public string GetLibList()
        {
            HttpRequestSyn request = new HttpRequestSyn();
            request.RequestUrl = URL_API_GET_LIB_LIST;
            request.AppendParameter("token", _token);
            request.AppendParameter("token_type", "ent");
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.GET;
            request.Request();
            this.StatusCode = request.Code;
            return request.Result;
        }

        public string Bind(int orgId, string title, string linkUrl)
        {
            HttpRequestSyn request = new HttpRequestSyn();
            request.RequestUrl = URL_API_BIND;
            request.AppendParameter("token", _token);
            request.AppendParameter("token_type", "ent");
            request.AppendParameter("org_id", orgId + "");
            request.AppendParameter("title", title);
            request.AppendParameter("url", linkUrl);
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.POST;
            request.Request();
            this.StatusCode = request.Code;
            return request.Result;
        }

        public string UnBind(string orgClientId)
        {
            HttpRequestSyn request = new HttpRequestSyn();
            request.RequestUrl = URL_API_UNBIND;
            request.AppendParameter("token", _token);
            request.AppendParameter("token_type", "ent");
            request.AppendParameter("org_client_id", orgClientId);
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.POST;
            request.Request();
            this.StatusCode = request.Code;
            return request.Result;
        }

        public string GetFileList(string orgClientId, string orgClientSecret, int dateline, int start, string fullPath)
        {
            HttpRequestSyn request = new HttpRequestSyn();
            request.RequestUrl = URL_API_FILELIST;
            request.AppendParameter("org_client_id", orgClientId);
            request.AppendParameter("dateline", dateline + "");
            request.AppendParameter("start", start + "");
            request.AppendParameter("fullpath", fullPath);
            request.AppendParameter("sign", GenerateSign(request.SortedParamter,orgClientSecret));
            request.RequestMethod = RequestType.GET;
            request.Request();
            this.StatusCode = request.Code;
            return request.Result;
        }

        public string GetUpdateList(string orgClientId, string orgClientSecret, int dateline, bool isCompare, long fetchDateline)
        {
            HttpRequestSyn request = new HttpRequestSyn();
            request.RequestUrl = URL_API_FILELIST;
            request.AppendParameter("org_client_id", orgClientId);
            request.AppendParameter("dateline", dateline + "");
            if (isCompare)
            {
                request.AppendParameter("mode", "compare");
            }
            request.AppendParameter("fetch_dateline", fetchDateline+"");
            request.AppendParameter("sign", GenerateSign(request.SortedParamter, orgClientSecret));
            request.RequestMethod = RequestType.GET;
            request.Request();
            this.StatusCode = request.Code;
            return request.Result;
        }

        public string GetFileInfo(string orgClientId, string orgClientSecret, int dateline, string fullPath)
        {
            HttpRequestSyn request = new HttpRequestSyn();
            request.RequestUrl = URL_API_FILE_INFO;
            request.AppendParameter("org_client_id", orgClientId);
            request.AppendParameter("dateline", dateline + "");
            request.AppendParameter("fullpath", fullPath);
            request.AppendParameter("sign", GenerateSign(request.SortedParamter, orgClientSecret));
            request.RequestMethod = RequestType.GET;
            request.Request();
            this.StatusCode = request.Code;
            return request.Result;
        }

        public string CreateFolder(string orgClientId, string orgClientSecret, int dateline, string fullPath, string opName)
        {
            HttpRequestSyn request = new HttpRequestSyn();
            request.RequestUrl = URL_API_CREATE_FOLDER;
            request.AppendParameter("org_client_id", orgClientId);
            request.AppendParameter("dateline", dateline + "");
            request.AppendParameter("fullpath", fullPath);
            request.AppendParameter("op_name", opName);
            request.AppendParameter("sign", GenerateSign(request.SortedParamter, orgClientSecret));
            request.RequestMethod = RequestType.POST;
            request.Request();
            this.StatusCode = request.Code;
            return request.Result;
        }

        public string CreateFile(string orgClientId, string orgClientSecret, int dateline, string fullPath, string opName, System.IO.Stream stream, string fileName)
        {
            if (stream.Length > 51200)
            {
                LogPrint.Print("文件大小超过50MB");
                return "";
            }

            HttpRequestSyn request = new HttpRequestSyn();
            request.RequestUrl = URL_API_CREATE_FILE;
            string[] arr = new string[] { dateline + "", "file", fullPath, opName, orgClientId, _token };
            MsMultiPartFormData data = new MsMultiPartFormData();
            request.ContentType = "multipart/form-data;boundary=" + data.Boundary;
            data.AddStreamFile("file", fileName, Util.ReadToEnd(stream));
            data.AddParams("token", _token);
            data.AddParams("org_client_id", orgClientId);
            data.AddParams("dateline", dateline+"");
            data.AddParams("fullpath", fullPath);
            data.AddParams("op_name", opName);
            data.AddParams("filefield", "file");
            data.AddParams("sign", GenerateSign(arr,orgClientSecret));
            data.PrepareFormData();
            request.PostDataByte = data.GetFormData();
            request.RequestMethod = RequestType.POST;
            LogPrint.Print("------------->Begin to Upload<------------------");
            request.Request();
            LogPrint.Print("--------------------->Upload Request Compeleted<--------------");
            this.StatusCode = request.Code;
            return request.Result;
        }

        public string CreateFile(string orgClientId, string orgClientSecret, int dateline, string fullPath, string opName, string localPath)
        {
            using (FileStream FS = new FileStream(localPath, FileMode.Open))
            {
                Stream stream = FS;
                if (stream != null)
                {
                    return CreateFile(orgClientId, orgClientSecret, dateline, fullPath, opName, stream, Util.GetFileNameFromPath(localPath));
                }
                else 
                {
                    LogPrint.Print("file not exist");
                }

            }
            return "";
        }

        public string Del(string orgClientId, string orgClientSecret, int dateline, string fullPath, string opName)
        {
            HttpRequestSyn request = new HttpRequestSyn();
            request.RequestUrl = URL_API_DEL_FILE;
            request.AppendParameter("org_client_id", orgClientId);
            request.AppendParameter("dateline", dateline + "");
            request.AppendParameter("fullpath", fullPath);
            request.AppendParameter("op_name", opName);
            request.AppendParameter("sign", GenerateSign(request.SortedParamter, orgClientSecret));
            request.RequestMethod = RequestType.POST;
            request.Request();
            this.StatusCode = request.Code;
            return request.Result;
        }

        public string Move(string orgClientId, string orgClientSecret, int dateline, string fullPath, string destFullPath, string opName)
        {
            HttpRequestSyn request = new HttpRequestSyn();
            request.RequestUrl = URL_API_MOVE_FILE;
            request.AppendParameter("org_client_id", orgClientId);
            request.AppendParameter("dateline", dateline + "");
            request.AppendParameter("fullpath", fullPath);
            request.AppendParameter("dest_fullpath", destFullPath);
            request.AppendParameter("op_name", opName);
            request.AppendParameter("sign", GenerateSign(request.SortedParamter, orgClientSecret));
            request.RequestMethod = RequestType.POST;
            request.Request();
            this.StatusCode = request.Code;
            return request.Result;
        }

        public string Link(string orgClientId, string orgClientSecret, int dateline, string fullPath)
        {
            HttpRequestSyn request = new HttpRequestSyn();
            request.RequestUrl = URL_API_LINK_FILE;
            request.AppendParameter("org_client_id", orgClientId);
            request.AppendParameter("dateline", dateline + "");
            request.AppendParameter("fullpath", fullPath);
            request.AppendParameter("sign", GenerateSign(request.SortedParamter, orgClientSecret));
            request.RequestMethod = RequestType.POST;
            request.Request();
            this.StatusCode = request.Code;
            return request.Result;
        }

        public string SendMsg(string orgClientId, string orgClientSecret, int dateline, string title, string text, string image, string linkUrl, string opName)
        {
            HttpRequestSyn request = new HttpRequestSyn();
            request.RequestUrl = URL_API_SENDMSG;
            request.AppendParameter("org_client_id", orgClientId);
            request.AppendParameter("dateline", dateline + "");
            request.AppendParameter("title", title);
            request.AppendParameter("text", text);
            request.AppendParameter("image", image);
            request.AppendParameter("url", linkUrl);
            request.AppendParameter("op_name", opName);
            request.AppendParameter("sign", GenerateSign(request.SortedParamter, orgClientSecret));
            request.RequestMethod = RequestType.POST;
            request.Request();
            this.StatusCode = request.Code;
            return request.Result;
        }


        public string GetEntRoles()
        {
            HttpRequestSyn request = new HttpRequestSyn();
            request.RequestUrl = URL_API_ENT_GET_ROLES;
            request.AppendParameter("token", _token);
            request.AppendParameter("token_type", "ent");
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.GET;
            request.Request();
            this.StatusCode = request.Code;
            return request.Result;
        }

        public string GetEntMembers(int start, int size)
        {
            HttpRequestSyn request = new HttpRequestSyn();
            request.RequestUrl = URL_API_ENT_GET_MEMBERS;
            request.AppendParameter("token", _token);
            request.AppendParameter("token_type", "ent");
            request.AppendParameter("start", start+"");
            request.AppendParameter("size", size + "");
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.GET;
            request.Request();
            this.StatusCode = request.Code;
            return request.Result;
        }

        public string GetEntGroups()
        {
            HttpRequestSyn request = new HttpRequestSyn();
            request.RequestUrl = URL_API_ENT_GET_GROUPS;
            request.AppendParameter("token", _token);
            request.AppendParameter("token_type", "ent");
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.GET;
            request.Request();
            this.StatusCode = request.Code;
            return request.Result;
        }

        public string SyncMembers(string structStr)
        {
            HttpRequestSyn request = new HttpRequestSyn();
            request.RequestUrl = URL_API_ENT_GET_GROUPS;
            request.AppendParameter("token", _token);
            request.AppendParameter("token_type", "ent");
            request.AppendParameter("members",structStr);
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.GET;
            request.Request();
            this.StatusCode = request.Code;
            return request.Result;
        }



        public string GetMembers(int start, int size, int orgId)
        {
            HttpRequestSyn request = new HttpRequestSyn();
            request.RequestUrl = URL_API_GET_MEMBERS;
            request.AppendParameter("token", _token);
            request.AppendParameter("token_type", "ent");
            request.AppendParameter("start", start + "");
            request.AppendParameter("size", size + "");
            request.AppendParameter("org_id", orgId + "");
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.GET;
            request.Request();
            this.StatusCode = request.Code;
            return request.Result;
        }

        public string AddMembers(int orgId, int roleId, int[] memberIds)
        {
            HttpRequestSyn request = new HttpRequestSyn();
            request.RequestUrl = URL_API_ADD_MEMBERS;
            request.AppendParameter("token", _token);
            request.AppendParameter("token_type", "ent");
            request.AppendParameter("role_id", roleId + "");
            request.AppendParameter("org_id", orgId + "");
            request.AppendParameter("member_ids", Util.intArrayToString(memberIds,","));
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.POST;
            request.Request();
            this.StatusCode = request.Code;
            return request.Result;
        }

        public string SetMemberRole(int orgId, int roleId, int[] memberIds)
        {
            HttpRequestSyn request = new HttpRequestSyn();
            request.RequestUrl = URL_API_SET_MEMBER_ROLE;
            request.AppendParameter("token", _token);
            request.AppendParameter("token_type", "ent");
            request.AppendParameter("role_id", roleId + "");
            request.AppendParameter("org_id", orgId + "");
            request.AppendParameter("member_ids", Util.intArrayToString(memberIds, ","));
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.POST;
            request.Request();
            this.StatusCode = request.Code;
            return request.Result;
        }

        public string DelMember(int orgId, int[] memberIds)
        {
            HttpRequestSyn request = new HttpRequestSyn();
            request.RequestUrl = URL_API_DEL_MEMBER;
            request.AppendParameter("token", _token);
            request.AppendParameter("token_type", "ent");
            request.AppendParameter("org_id", orgId + "");
            request.AppendParameter("member_ids", Util.intArrayToString(memberIds, ","));
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.POST;
            request.Request();
            this.StatusCode = request.Code;
            return request.Result;
        }

        public string GetGroups(int orgId)
        {
            HttpRequestSyn request = new HttpRequestSyn();
            request.RequestUrl = URL_API_GET_GROUPS;
            request.AppendParameter("token", _token);
            request.AppendParameter("token_type", "ent");
            request.AppendParameter("org_id", orgId + "");
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.GET;
            request.Request();
            this.StatusCode = request.Code;
            return request.Result;
        }

        public string AddGroup(int orgId, int groupId, int roleId)
        {
            HttpRequestSyn request = new HttpRequestSyn();
            request.RequestUrl = URL_API_ADD_GROUP;
            request.AppendParameter("token", _token);
            request.AppendParameter("token_type", "ent");
            request.AppendParameter("org_id", orgId + "");
            request.AppendParameter("role_id", roleId + "");
            request.AppendParameter("group_id", groupId + "");
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.POST;
            request.Request();
            this.StatusCode = request.Code;
            return request.Result;
        }

        public string DelGroup(int orgId, int groupId)
        {
            HttpRequestSyn request = new HttpRequestSyn();
            request.RequestUrl = URL_API_DEL_GROUP;
            request.AppendParameter("token", _token);
            request.AppendParameter("token_type", "ent");
            request.AppendParameter("org_id", orgId + "");
            request.AppendParameter("group_id", groupId + "");
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.POST;
            request.Request();
            this.StatusCode = request.Code;
            return request.Result;
        }

        public string SetGroupRole(int orgId, int groupId, int roleId)
        {
            HttpRequestSyn request = new HttpRequestSyn();
            request.RequestUrl = URL_API_SET_GROUP_ROLE;
            request.AppendParameter("token", _token);
            request.AppendParameter("token_type", "ent");
            request.AppendParameter("org_id", orgId + "");
            request.AppendParameter("group_id", groupId + "");
            request.AppendParameter("role_id", roleId + "");
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.POST;
            request.Request();
            this.StatusCode = request.Code;
            return request.Result;
        }
    }
}
