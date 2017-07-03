using System.Collections.Generic;
using YunkuEntSDK.Net;
using YunkuEntSDK.UtilClass;

namespace YunkuEntSDK
{
    public class EntLibManager : OauthEngine
    {
        private const string ApiEntHost = HostConfig.ApiEntHost;

        private const string UrlApiCreateLib = ApiEntHost + "/1/org/create";
        private const string UrlApiGetLibList = ApiEntHost + "/1/org/ls";
        private const string UrlApiBind = ApiEntHost + "/1/org/bind";
        private const string UrlApiUnbind = ApiEntHost + "/1/org/unbind";
        private const string UrlApiGetMembers = ApiEntHost + "/1/org/get_members";
        private const string UrlApiAddMembers = ApiEntHost + "/1/org/add_member";
        private const string UrlApiSetMemberRole = ApiEntHost + "/1/org/set_member_role";
        private const string UrlApiDelMember = ApiEntHost + "/1/org/del_member";
        private const string UrlApiGetGroups = ApiEntHost + "/1/org/get_groups";
        private const string UrlApiAddGroup = ApiEntHost + "/1/org/add_group";
        private const string UrlApiDelGroup = ApiEntHost + "/1/org/del_group";
        private const string UrlApiSetGroupRole = ApiEntHost + "/1/org/set_group_role";
        private const string UrlApiDestroy = ApiEntHost + "/1/org/destroy";
        private const string UrlApiGetMember = ApiEntHost + "/1/org/get_member";
        private const string UrlApiSet = ApiEntHost + "/1/org/set";
        private const string UrlApiGetInfo = ApiEntHost + "/1/org/info";
        private const string UrlApiGetLog = ApiEntHost + "/1/org/log";

        public EntLibManager(string clientId, string clientSecret) : base(clientId, clientSecret, true)
        {
        }

        public EntLibManager(string clientId, string clientSecret, bool isEnt, string token) : base(clientId, clientSecret, isEnt, token)
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
        public string Create(string orgName, string orgCapacity, string storagePointName, string orgLogo)
        {
            string url = UrlApiCreateLib;
            var parameter = new Dictionary<string, string>();
            AddAuthParams(parameter);
            parameter.Add("org_name", orgName);
            parameter.Add("org_capacity", orgCapacity);
            parameter.Add("storage_point_name", storagePointName);
            parameter.Add("org_logo", orgLogo);
            parameter.Add("sign", GenerateSign(parameter));
            return new RequestHelper().SetParams(parameter).SetUrl(url).SetMethod(RequestType.Post).ExecuteSync();
        }

        /// <summary>
        /// 获取企业库列表
        /// </summary>
        /// <returns></returns>
        public string GetLibList()
        {
            return GetLibList(0, 0);
        }

        /// <summary>
        /// 获取库列表
        /// </summary>
        /// <returns></returns>
        public string GetLibList(int memberId, int type)
        {
            string url = UrlApiGetLibList;
            var parameter = new Dictionary<string, string>();
            AddAuthParams(parameter);
            if (memberId > 0)
            {
                parameter.Add("member_id", memberId + "");
            }
            parameter.Add("type", type + "");
            parameter.Add("sign", GenerateSign(parameter));
            return new RequestHelper().SetParams(parameter).SetUrl(url).SetMethod(RequestType.Get).ExecuteSync();
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
            string url = UrlApiBind;
            var parameter = new Dictionary<string, string>();
            AddAuthParams(parameter);
            parameter.Add("org_id", orgId + "");
            parameter.Add("title", title);
            parameter.Add("url", linkUrl);
            parameter.Add("sign", GenerateSign(parameter));
            return new RequestHelper().SetParams(parameter).SetUrl(url).SetMethod(RequestType.Post).ExecuteSync();

        }

        /// <summary>
        /// 取消库授权
        /// </summary>
        /// <param name="orgClientId"></param>
        /// <returns></returns>
        public string UnBind(string orgClientId)
        {
            string url = UrlApiUnbind;
            var parameter = new Dictionary<string, string>();
            AddAuthParams(parameter);
            parameter.Add("org_client_id", orgClientId);
            parameter.Add("sign", GenerateSign(parameter));
            return new RequestHelper().SetParams(parameter).SetUrl(url).SetMethod(RequestType.Post).ExecuteSync();
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
            string url = UrlApiGetMembers;
            var parameter = new Dictionary<string, string>();
            AddAuthParams(parameter);
            parameter.Add("start", start + "");
            parameter.Add("size", size + "");
            parameter.Add("org_id", orgId + "");
            parameter.Add("sign", GenerateSign(parameter));
            return new RequestHelper().SetParams(parameter).SetUrl(url).SetMethod(RequestType.Get).ExecuteSync();
        }

        /// <summary>
        /// 查询库成员信息
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="type"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public string GetMember(int orgId, MemberType type, string[] ids)
        {
            string url = UrlApiGetMembers;
            var parameter = new Dictionary<string, string>();
            AddAuthParams(parameter);
            parameter.Add("ids", Util.StrArrayToString(ids, ","));
            parameter.Add("type", type.ToString().ToLower());
            parameter.Add("org_id", orgId + "");
            parameter.Add("sign", GenerateSign(parameter));
            return new RequestHelper().SetParams(parameter).SetUrl(url).SetMethod(RequestType.Get).ExecuteSync();
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
            string url = UrlApiAddMembers;
            var parameter = new Dictionary<string, string>();
            AddAuthParams(parameter);
            parameter.Add("role_id", roleId + "");
            parameter.Add("member_ids", Util.IntArrayToString(memberIds, ","));
            parameter.Add("org_id", orgId + "");
            parameter.Add("sign", GenerateSign(parameter));
            return new RequestHelper().SetParams(parameter).SetUrl(url).SetMethod(RequestType.Post).ExecuteSync();
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
            string url = UrlApiSetMemberRole;
            var parameter = new Dictionary<string, string>();
            AddAuthParams(parameter);
            parameter.Add("role_id", roleId + "");
            parameter.Add("member_ids", Util.IntArrayToString(memberIds, ","));
            parameter.Add("org_id", orgId + "");
            parameter.Add("sign", GenerateSign(parameter));
            return new RequestHelper().SetParams(parameter).SetUrl(url).SetMethod(RequestType.Post).ExecuteSync();
        }

        /// <summary>
        /// 删除库成员
        /// </summary>
        /// <param name="orgId"></param>
        /// <param name="memberIds"></param>
        /// <returns></returns>
        public string DelMember(int orgId, int[] memberIds)
        {
            string url = UrlApiDelMember;
            var parameter = new Dictionary<string, string>();
            AddAuthParams(parameter);
            parameter.Add("member_ids", Util.IntArrayToString(memberIds, ","));
            parameter.Add("org_id", orgId + "");
            parameter.Add("sign", GenerateSign(parameter));
            return new RequestHelper().SetParams(parameter).SetUrl(url).SetMethod(RequestType.Post).ExecuteSync();
        }

        /// <summary>
        /// 获取库分组列表
        /// </summary>
        /// <param name="orgId"></param>
        /// <returns></returns>
        public string GetGroups(int orgId)
        {
            string url = UrlApiGetGroups;
            var parameter = new Dictionary<string, string>();
            AddAuthParams(parameter);
            parameter.Add("org_id", orgId + "");
            parameter.Add("sign", GenerateSign(parameter));
            return new RequestHelper().SetParams(parameter).SetUrl(url).SetMethod(RequestType.Get).ExecuteSync();
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
            string url = UrlApiAddGroup;
            var parameter = new Dictionary<string, string>();
            AddAuthParams(parameter);
            parameter.Add("org_id", orgId + "");
            parameter.Add("role_id", roleId + "");
            parameter.Add("group_id", groupId + "");
            parameter.Add("sign", GenerateSign(parameter));
            return new RequestHelper().SetParams(parameter).SetUrl(url).SetMethod(RequestType.Post).ExecuteSync();
        }

        /// <summary>
        /// 删除库上的分组
        /// </summary>
        /// <param name="orgId"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public string DelGroup(int orgId, int groupId)
        {
            string url = UrlApiDelGroup;
            var parameter = new Dictionary<string, string>();
            AddAuthParams(parameter);
            parameter.Add("org_id", orgId + "");
            parameter.Add("group_id", groupId + "");
            parameter.Add("sign", GenerateSign(parameter));
            return new RequestHelper().SetParams(parameter).SetUrl(url).SetMethod(RequestType.Post).ExecuteSync();
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
            string url = UrlApiSetGroupRole;
            var parameter = new Dictionary<string, string>();
            AddAuthParams(parameter);
            parameter.Add("org_id", orgId + "");
            parameter.Add("group_id", groupId + "");
            parameter.Add("role_id", roleId + "");
            parameter.Add("sign", GenerateSign(parameter));
            return new RequestHelper().SetParams(parameter).SetUrl(url).SetMethod(RequestType.Post).ExecuteSync();
        }

        /// <summary>
        /// 删除库
        /// </summary>
        /// <param name="orgClientId"></param>
        /// <returns></returns>
        public string Destroy(string orgClientId)
        {
            string url = UrlApiDestroy;
            var parameter = new Dictionary<string, string>();
            AddAuthParams(parameter);
            parameter.Add("org_client_id", orgClientId + "");
            parameter.Add("sign", GenerateSign(parameter));
            return new RequestHelper().SetParams(parameter).SetUrl(url).SetMethod(RequestType.Post).ExecuteSync();
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
        public string Set(int orgId, string orgName, string orgCapacity, string orgLogo)
        {
            string url = UrlApiSet;
            var parameter = new Dictionary<string, string>();
            AddAuthParams(parameter);
            parameter.Add("org_id", orgId + "");

            if (!string.IsNullOrEmpty(orgName))
            {
                parameter.Add("org_name", "" + orgName);
            }
            if (!string.IsNullOrEmpty(orgCapacity))
            {
                parameter.Add("org_capacity", "" + orgCapacity);
            }
            if (!string.IsNullOrEmpty(orgLogo))
            {
                parameter.Add("org_logo", "" + orgLogo);
            }
            parameter.Add("sign", GenerateSign(parameter));
            return new RequestHelper().SetParams(parameter).SetUrl(url).SetMethod(RequestType.Post).ExecuteSync();
        }

        /// <summary>
        /// 获取库信息
        /// </summary>
        /// <param name="orgId"></param>
        /// <returns></returns>

        public string GetInfo(int orgId)
        {
            string url = UrlApiGetInfo;
            var parameter = new Dictionary<string, string>();
            AddAuthParams(parameter);
            parameter.Add("org_id", orgId + "");
            parameter.Add("sign", GenerateSign(parameter));
            return new RequestHelper().SetParams(parameter).SetUrl(url).SetMethod(RequestType.Get).ExecuteSync();
        }

        /// <summary>
        /// 获取库日志
        /// </summary>
        /// <param name="orgId"></param>
        /// <param name="act"></param>
        /// <param name="startDateline"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public string GetLog(int orgId, string[] act, int startDateline, int size)
        {
            string url = UrlApiGetLog;
            var parameter = new Dictionary<string, string>();
            AddAuthParams(parameter);
            parameter.Add("org_id", orgId + "");
            if (act != null)
            {
                parameter.Add("act", Util.StrArrayToString(act, ",") + "");
            }
            parameter.Add("start_dateline", startDateline + "");
            parameter.Add("size", size + "");
            parameter.Add("sign", GenerateSign(parameter));
            return new RequestHelper().SetParams(parameter).SetUrl(url).SetMethod(RequestType.Post).ExecuteSync();
        }

        /// <summary>
        /// 复制EntLibManager对象
        /// </summary>
        /// <returns></returns>
        public EntLibManager Clone()
        {
            return new EntLibManager(_clientId, _clientSecret, _isEnt, Token);
        }

        public enum MemberType
        {
            Account,
            OutId,
            MemberId
        }

    }
}