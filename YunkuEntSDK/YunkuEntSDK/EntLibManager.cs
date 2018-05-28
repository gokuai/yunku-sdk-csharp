using System.Collections.Generic;
using YunkuEntSDK.Data;
using YunkuEntSDK.Net;
using YunkuEntSDK.UtilClass;

namespace YunkuEntSDK
{
    public class EntLibManager : EntEngine
    {
        private static string ApiEntHost = Config.ApiHost;

        private static string UrlApiCreateLib = ApiEntHost + "/1/org/create";
        private static string UrlApiGetLibList = ApiEntHost + "/1/org/ls";
        private static string UrlApiBind = ApiEntHost + "/1/org/bind";
        private static string UrlApiUnbind = ApiEntHost + "/1/org/unbind";
        private static string UrlApiGetMembers = ApiEntHost + "/1/org/get_members";
        private static string UrlApiAddMembers = ApiEntHost + "/1/org/add_member";
        private static string UrlApiSetMemberRole = ApiEntHost + "/1/org/set_member_role";
        private static string UrlApiDelMember = ApiEntHost + "/1/org/del_member";
        private static string UrlApiGetGroups = ApiEntHost + "/1/org/get_groups";
        private static string UrlApiAddGroup = ApiEntHost + "/1/org/add_group";
        private static string UrlApiDelGroup = ApiEntHost + "/1/org/del_group";
        private static string UrlApiSetGroupRole = ApiEntHost + "/1/org/set_group_role";
        private static string UrlApiDestroy = ApiEntHost + "/1/org/destroy";
        private static string UrlApiGetMember = ApiEntHost + "/1/org/get_member";
        private static string UrlApiSet = ApiEntHost + "/1/org/set";
        private static string UrlApiGetInfo = ApiEntHost + "/1/org/info";
        private static string UrlApiGetLog = ApiEntHost + "/1/org/log";

        public EntLibManager(string clientId, string clientSecret) : base(clientId, clientSecret)
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
        public ReturnResult Create(string orgName, string orgCapacity, string storagePointName, string orgLogo)
        {
            string url = UrlApiCreateLib;
            var parameter = new Dictionary<string, string>();
            parameter.Add("org_name", orgName);
            parameter.Add("org_capacity", orgCapacity);
            parameter.Add("storage_point_name", storagePointName);
            parameter.Add("org_logo", orgLogo);
            return new RequestHelper(this).SetParams(parameter).SetUrl(url).SetMethod(RequestType.POST).ExecuteSync();
        }

        /// <summary>
        /// 获取企业库列表
        /// </summary>
        /// <returns></returns>
        public ReturnResult GetLibList()
        {
            return GetLibList(0, 0);
        }

        /// <summary>
        /// 获取库列表
        /// </summary>
        /// <returns></returns>
        public ReturnResult GetLibList(int memberId, int type)
        {
            string url = UrlApiGetLibList;
            var parameter = new Dictionary<string, string>();
            if (memberId > 0)
            {
                parameter.Add("member_id", memberId.ToString());
            }
            parameter.Add("type", type.ToString());
            return new RequestHelper(this).SetParams(parameter).SetUrl(url).SetMethod(RequestType.GET).ExecuteSync();
        }

        /// <summary>
        /// 获取库授权
        /// </summary>
        /// <param name="orgId"></param>
        /// <param name="title"></param>
        /// <param name="linkUrl"></param>
        /// <returns></returns>
        public ReturnResult Bind(int orgId, string title)
        {
            string url = UrlApiBind;
            var parameter = new Dictionary<string, string>();
            parameter.Add("org_id", orgId.ToString());
            parameter.Add("title", title);
            return new RequestHelper(this).SetParams(parameter).SetUrl(url).SetMethod(RequestType.POST).ExecuteSync();

        }

        /// <summary>
        /// 取消库授权
        /// </summary>
        /// <param name="orgClientId"></param>
        /// <returns></returns>
        public ReturnResult UnBind(string orgClientId)
        {
            string url = UrlApiUnbind;
            var parameter = new Dictionary<string, string>();
            parameter.Add("org_client_id", orgClientId);
            return new RequestHelper(this).SetParams(parameter).SetUrl(url).SetMethod(RequestType.POST).ExecuteSync();
        }

        /// <summary>
        /// 获取库成员列表
        /// </summary>
        /// <param name="start"></param>
        /// <param name="size"></param>
        /// <param name="orgId"></param>
        /// <returns></returns>
        public ReturnResult GetMembers(int start, int size, int orgId)
        {
            string url = UrlApiGetMembers;
            var parameter = new Dictionary<string, string>();
            parameter.Add("org_id", orgId.ToString());
            parameter.Add("start", start.ToString());
            parameter.Add("size", size.ToString());
            return new RequestHelper(this).SetParams(parameter).SetUrl(url).SetMethod(RequestType.GET).ExecuteSync();
        }

        /// <summary>
        /// 查询库成员信息
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="type"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public ReturnResult GetMember(int orgId, MemberType type, string[] ids)
        {
            string url = UrlApiGetMembers;
            var parameter = new Dictionary<string, string>();
            parameter.Add("org_id", orgId.ToString());
            parameter.Add("type", type.ToString().ToLower());
            parameter.Add("ids", Util.StrArrayToString(ids, ","));
            return new RequestHelper(this).SetParams(parameter).SetUrl(url).SetMethod(RequestType.GET).ExecuteSync();
        }

        /// <summary>
        /// 添加库成员
        /// </summary>
        /// <param name="orgId"></param>
        /// <param name="roleId"></param>
        /// <param name="memberIds"></param>
        /// <returns></returns>
        public ReturnResult AddMembers(int orgId, int roleId, int[] memberIds)
        {
            string url = UrlApiAddMembers;
            var parameter = new Dictionary<string, string>();
            parameter.Add("org_id", orgId.ToString());
            parameter.Add("role_id", roleId.ToString());
            parameter.Add("member_ids", Util.IntArrayToString(memberIds, ","));
            return new RequestHelper(this).SetParams(parameter).SetUrl(url).SetMethod(RequestType.POST).ExecuteSync();
        }

        /// <summary>
        /// 修改库成员角色
        /// </summary>
        /// <param name="orgId"></param>
        /// <param name="roleId"></param>
        /// <param name="memberIds"></param>
        /// <returns></returns>
        public ReturnResult SetMemberRole(int orgId, int roleId, int[] memberIds)
        {
            string url = UrlApiSetMemberRole;
            var parameter = new Dictionary<string, string>();
            parameter.Add("org_id", orgId.ToString());
            parameter.Add("role_id", roleId.ToString());
            parameter.Add("member_ids", Util.IntArrayToString(memberIds, ","));
            return new RequestHelper(this).SetParams(parameter).SetUrl(url).SetMethod(RequestType.POST).ExecuteSync();
        }

        /// <summary>
        /// 删除库成员
        /// </summary>
        /// <param name="orgId"></param>
        /// <param name="memberIds"></param>
        /// <returns></returns>
        public ReturnResult DelMember(int orgId, int[] memberIds)
        {
            string url = UrlApiDelMember;
            var parameter = new Dictionary<string, string>();
            parameter.Add("org_id", orgId.ToString());
            parameter.Add("member_ids", Util.IntArrayToString(memberIds, ","));
            return new RequestHelper(this).SetParams(parameter).SetUrl(url).SetMethod(RequestType.POST).ExecuteSync();
        }

        /// <summary>
        /// 获取库分组列表
        /// </summary>
        /// <param name="orgId"></param>
        /// <returns></returns>
        public ReturnResult GetGroups(int orgId)
        {
            string url = UrlApiGetGroups;
            var parameter = new Dictionary<string, string>();
            parameter.Add("org_id", orgId.ToString());
            return new RequestHelper(this).SetParams(parameter).SetUrl(url).SetMethod(RequestType.GET).ExecuteSync();
        }

        /// <summary>
        /// 库上添加分组
        /// </summary>
        /// <param name="orgId"></param>
        /// <param name="groupId"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public ReturnResult AddGroup(int orgId, int groupId, int roleId)
        {
            string url = UrlApiAddGroup;
            var parameter = new Dictionary<string, string>();
            parameter.Add("org_id", orgId.ToString());
            parameter.Add("role_id", roleId.ToString());
            parameter.Add("group_id", groupId.ToString());
            return new RequestHelper(this).SetParams(parameter).SetUrl(url).SetMethod(RequestType.POST).ExecuteSync();
        }

        /// <summary>
        /// 删除库上的分组
        /// </summary>
        /// <param name="orgId"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public ReturnResult DelGroup(int orgId, int groupId)
        {
            string url = UrlApiDelGroup;
            var parameter = new Dictionary<string, string>();
            parameter.Add("org_id", orgId.ToString());
            parameter.Add("group_id", groupId.ToString());
            return new RequestHelper(this).SetParams(parameter).SetUrl(url).SetMethod(RequestType.POST).ExecuteSync();
        }

        /// <summary>
        /// 修改库上分组的角色
        /// </summary>
        /// <param name="orgId"></param>
        /// <param name="groupId"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public ReturnResult SetGroupRole(int orgId, int groupId, int roleId)
        {
            string url = UrlApiSetGroupRole;
            var parameter = new Dictionary<string, string>();
            parameter.Add("org_id", orgId.ToString());
            parameter.Add("group_id", groupId.ToString());
            parameter.Add("role_id", roleId.ToString());
            return new RequestHelper(this).SetParams(parameter).SetUrl(url).SetMethod(RequestType.POST).ExecuteSync();
        }

        /// <summary>
        /// 删除库
        /// </summary>
        /// <param name="orgId"></param>
        /// <returns></returns>
        public ReturnResult Destroy(int orgId)
        {
            string url = UrlApiDestroy;
            var parameter = new Dictionary<string, string>();
            parameter.Add("org_id", orgId.ToString());
            return new RequestHelper(this).SetParams(parameter).SetUrl(url).SetMethod(RequestType.POST).ExecuteSync();
        }

        /// <summary>
        /// 删除库
        /// </summary>
        /// <param name="orgClientId"></param>
        /// <returns></returns>
        public ReturnResult DestroyByOrgClientId(string orgClientId)
        {
            string url = UrlApiDestroy;
            var parameter = new Dictionary<string, string>();
            parameter.Add("org_client_id", orgClientId);
            return new RequestHelper(this).SetParams(parameter).SetUrl(url).SetMethod(RequestType.POST).ExecuteSync();
        }

        /// <summary>
        /// 修改库信息
        /// </summary>
        /// <param name="orgId"></param>
        /// <param name="orgName"></param>
        /// <param name="orgCapacity"></param>
        /// <param name="orgDesc"></param>
        /// <param name="orgLogo"></param>
        /// <returns></returns>
        public ReturnResult Set(int orgId, string orgName, string orgCapacity, string orgLogo)
        {
            string url = UrlApiSet;
            var parameter = new Dictionary<string, string>();
            parameter.Add("org_id", orgId.ToString());

            if (!string.IsNullOrEmpty(orgName))
            {
                parameter.Add("org_name", orgName);
            }
            if (!string.IsNullOrEmpty(orgCapacity))
            {
                parameter.Add("org_capacity", orgCapacity);
            }
            if (!string.IsNullOrEmpty(orgLogo))
            {
                parameter.Add("org_logo", orgLogo);
            }
            return new RequestHelper(this).SetParams(parameter).SetUrl(url).SetMethod(RequestType.POST).ExecuteSync();
        }

        /// <summary>
        /// 获取库信息
        /// </summary>
        /// <param name="orgId"></param>
        /// <returns></returns>
        public ReturnResult GetInfo(int orgId)
        {
            string url = UrlApiGetInfo;
            var parameter = new Dictionary<string, string>();
            parameter.Add("org_id", orgId.ToString());
            return new RequestHelper(this).SetParams(parameter).SetUrl(url).SetMethod(RequestType.GET).ExecuteSync();
        }

        /// <summary>
        /// 获取库日志
        /// </summary>
        /// <param name="orgId"></param>
        /// <param name="act"></param>
        /// <param name="startDateline"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public ReturnResult GetLog(int orgId, string[] act, int startDateline, int size)
        {
            string url = UrlApiGetLog;
            var parameter = new Dictionary<string, string>();
            parameter.Add("org_id", orgId.ToString());
            if (act != null)
            {
                parameter.Add("act", Util.StrArrayToString(act, ","));
            }
            parameter.Add("start_dateline", startDateline.ToString());
            parameter.Add("size", size.ToString());
            return new RequestHelper(this).SetParams(parameter).SetUrl(url).SetMethod(RequestType.GET).ExecuteSync();
        }

        /// <summary>
        /// 复制EntLibManager对象
        /// </summary>
        /// <returns></returns>
        public EntLibManager Clone()
        {
            return new EntLibManager(_clientId, _secret);
        }

        public enum MemberType
        {
            Account,
            OutId,
            MemberId
        }

    }
}