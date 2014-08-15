using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using YunkuEntSDK.Data;
using YunkuEntSDK.Net;
using YunkuEntSDK.UtilClass;

namespace YunkuEntSDK
{

     public class EntlibManager:ParentManager
    {
         
         const string LIB_HOST = HostConfig.LIB_HOST;
         
         const string URL_API_CREATE_LIB = LIB_HOST + "/1/org/create";
         const string URL_API_GET_LIB_LIST = LIB_HOST + "/1/org/ls";
         const string URL_API_BIND = LIB_HOST + "/1/org/bind";
         const string URL_API_UNBIND = LIB_HOST + "/1/org/unbind";


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

        public EntlibManager(string uesrname, string password, string client_id, string client_secret)
            : base(uesrname, password, client_id, client_secret)
        {
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
