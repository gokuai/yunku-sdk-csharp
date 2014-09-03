using YunkuEntSDK.Net;
using YunkuEntSDK.UtilClass;

namespace YunkuEntSDK
{
    public class EntLibManager : ParentManager
    {
        private const string LibHost = HostConfig.LibHost;

        private const string UrlApiCreateLib = LibHost + "/1/org/create";
        private const string UrlApiGetLibList = LibHost + "/1/org/ls";
        private const string UrlApiBind = LibHost + "/1/org/bind";
        private const string UrlApiUnbind = LibHost + "/1/org/unbind";
        private const string UrlApiGetMembers = LibHost + "/1/org/get_members";
        private const string UrlApiAddMembers = LibHost + "/1/org/add_member";
        private const string UrlApiSetMemberRole = LibHost + "/1/org/set_member_role";
        private const string UrlApiDelMember = LibHost + "/1/org/del_member";
        private const string UrlApiGetGroups = LibHost + "/1/org/get_groups";
        private const string UrlApiAddGroup = LibHost + "/1/org/add_group";
        private const string UrlApiDelGroup = LibHost + "/1/org/del_group";
        private const string UrlApiSetGroupRole = LibHost + "/1/org/set_group_role";
        private const string UrlApiDestroy = LibHost + "/1/org/destroy";


        public EntLibManager(string uesrname, string password, string client_id, string client_secret)
            : base(uesrname, password, client_id, client_secret)
        {
        }

        /// <summary>
        /// 创建库
        /// </summary>
        /// <param name="orgName"></param>
        /// <param name="orgCapacity"></param>
        /// <param name="storagePointName"></param>
        /// <param name="orgDesc"></param>
        /// <returns></returns>
        public string Create(string orgName, int orgCapacity, string storagePointName, string orgDesc)
        {
            var request = new HttpRequestSyn();
            request.RequestUrl = UrlApiCreateLib;
            request.AppendParameter("token", Token);
            request.AppendParameter("token_type", "ent");
            request.AppendParameter("org_name", orgName);
            request.AppendParameter("org_capacity", orgCapacity + "");
            request.AppendParameter("storage_point_name", storagePointName);
            request.AppendParameter("org_desc", orgDesc);
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.Post;
            request.Request();
            StatusCode = request.Code;
            return request.Result;
        }

        /// <summary>
        /// 获取库列表
        /// </summary>
        /// <returns></returns>
        public string GetLibList()
        {
            var request = new HttpRequestSyn();
            request.RequestUrl = UrlApiGetLibList;
            request.AppendParameter("token", Token);
            request.AppendParameter("token_type", "ent");
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.Get;
            request.Request();
            StatusCode = request.Code;
            return request.Result;
        }

        /// <summary>
        /// 获取库授权
        /// </summary>
        /// <param name="orgId"></param>
        /// <param name="title"></param>
        /// <param name="linkUrl"></param>
        /// <returns></returns>
        public string Bind(int orgId, string title, string linkUrl)
        {
            var request = new HttpRequestSyn();
            request.RequestUrl = UrlApiBind;
            request.AppendParameter("token", Token);
            request.AppendParameter("token_type", "ent");
            request.AppendParameter("org_id", orgId + "");
            request.AppendParameter("title", title);
            request.AppendParameter("url", linkUrl);
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.Post;
            request.Request();
            StatusCode = request.Code;
            return request.Result;
        }

        /// <summary>
        /// 取消库授权
        /// </summary>
        /// <param name="orgClientId"></param>
        /// <returns></returns>
        public string UnBind(string orgClientId)
        {
            var request = new HttpRequestSyn();
            request.RequestUrl = UrlApiUnbind;
            request.AppendParameter("token", Token);
            request.AppendParameter("token_type", "ent");
            request.AppendParameter("org_client_id", orgClientId);
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.Post;
            request.Request();
            StatusCode = request.Code;
            return request.Result;
        }

        /// <summary>
        /// 获取库成员列表
        /// </summary>
        /// <param name="start"></param>
        /// <param name="size"></param>
        /// <param name="orgId"></param>
        /// <returns></returns>
        public string GetMembers(int start, int size, int orgId)
        {
            var request = new HttpRequestSyn();
            request.RequestUrl = UrlApiGetMembers;
            request.AppendParameter("token", Token);
            request.AppendParameter("token_type", "ent");
            request.AppendParameter("start", start + "");
            request.AppendParameter("size", size + "");
            request.AppendParameter("org_id", orgId + "");
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.Get;
            request.Request();
            StatusCode = request.Code;
            return request.Result;
        }

        /// <summary>
        /// 添加库成员
        /// </summary>
        /// <param name="orgId"></param>
        /// <param name="roleId"></param>
        /// <param name="memberIds"></param>
        /// <returns></returns>
        public string AddMembers(int orgId, int roleId, int[] memberIds)
        {
            var request = new HttpRequestSyn();
            request.RequestUrl = UrlApiAddMembers;
            request.AppendParameter("token", Token);
            request.AppendParameter("token_type", "ent");
            request.AppendParameter("role_id", roleId + "");
            request.AppendParameter("org_id", orgId + "");
            request.AppendParameter("member_ids", Util.IntArrayToString(memberIds, ","));
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.Post;
            request.Request();
            StatusCode = request.Code;
            return request.Result;
        }

        /// <summary>
        /// 修改库成员角色
        /// </summary>
        /// <param name="orgId"></param>
        /// <param name="roleId"></param>
        /// <param name="memberIds"></param>
        /// <returns></returns>
        public string SetMemberRole(int orgId, int roleId, int[] memberIds)
        {
            var request = new HttpRequestSyn();
            request.RequestUrl = UrlApiSetMemberRole;
            request.AppendParameter("token", Token);
            request.AppendParameter("token_type", "ent");
            request.AppendParameter("role_id", roleId + "");
            request.AppendParameter("org_id", orgId + "");
            request.AppendParameter("member_ids", Util.IntArrayToString(memberIds, ","));
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.Post;
            request.Request();
            StatusCode = request.Code;
            return request.Result;
        }

        /// <summary>
        /// 删除库成员
        /// </summary>
        /// <param name="orgId"></param>
        /// <param name="memberIds"></param>
        /// <returns></returns>
        public string DelMember(int orgId, int[] memberIds)
        {
            var request = new HttpRequestSyn();
            request.RequestUrl = UrlApiDelMember;
            request.AppendParameter("token", Token);
            request.AppendParameter("token_type", "ent");
            request.AppendParameter("org_id", orgId + "");
            request.AppendParameter("member_ids", Util.IntArrayToString(memberIds, ","));
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.Post;
            request.Request();
            StatusCode = request.Code;
            return request.Result;
        }

        /// <summary>
        /// 获取库分组列表
        /// </summary>
        /// <param name="orgId"></param>
        /// <returns></returns>
        public string GetGroups(int orgId)
        {
            var request = new HttpRequestSyn();
            request.RequestUrl = UrlApiGetGroups;
            request.AppendParameter("token", Token);
            request.AppendParameter("token_type", "ent");
            request.AppendParameter("org_id", orgId + "");
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.Get;
            request.Request();
            StatusCode = request.Code;
            return request.Result;
        }

        /// <summary>
        /// 库上添加分组
        /// </summary>
        /// <param name="orgId"></param>
        /// <param name="groupId"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public string AddGroup(int orgId, int groupId, int roleId)
        {
            var request = new HttpRequestSyn();
            request.RequestUrl = UrlApiAddGroup;
            request.AppendParameter("token", Token);
            request.AppendParameter("token_type", "ent");
            request.AppendParameter("org_id", orgId + "");
            request.AppendParameter("role_id", roleId + "");
            request.AppendParameter("group_id", groupId + "");
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.Post;
            request.Request();
            StatusCode = request.Code;
            return request.Result;
        }

        /// <summary>
        /// 删除库上的分组
        /// </summary>
        /// <param name="orgId"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public string DelGroup(int orgId, int groupId)
        {
            var request = new HttpRequestSyn();
            request.RequestUrl = UrlApiDelGroup;
            request.AppendParameter("token", Token);
            request.AppendParameter("token_type", "ent");
            request.AppendParameter("org_id", orgId + "");
            request.AppendParameter("group_id", groupId + "");
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.Post;
            request.Request();
            StatusCode = request.Code;
            return request.Result;
        }

        /// <summary>
        /// 修改库上分组的角色
        /// </summary>
        /// <param name="orgId"></param>
        /// <param name="groupId"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public string SetGroupRole(int orgId, int groupId, int roleId)
        {
            var request = new HttpRequestSyn();
            request.RequestUrl = UrlApiSetGroupRole;
            request.AppendParameter("token", Token);
            request.AppendParameter("token_type", "ent");
            request.AppendParameter("org_id", orgId + "");
            request.AppendParameter("group_id", groupId + "");
            request.AppendParameter("role_id", roleId + "");
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.Post;
            request.Request();
            StatusCode = request.Code;
            return request.Result;
        }

        /// <summary>
        /// 删除库
        /// </summary>
        /// <param name="orgClientId"></param>
        /// <returns></returns>
        public string Destroy(string orgClientId)
        {
            var request = new HttpRequestSyn();
            request.RequestUrl = UrlApiDestroy;
            request.AppendParameter("token", Token);
            request.AppendParameter("token_type", "ent");
            request.AppendParameter("org_client_id", "" + orgClientId);
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.Post;
            request.Request();
            StatusCode = request.Code;
            return request.Result;
        }
    }
}