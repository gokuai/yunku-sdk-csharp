using System.Collections.Generic;
using YunkuEntSDK.Data;
using YunkuEntSDK.Net;
using YunkuEntSDK.UtilClass;

namespace YunkuEntSDK
{
    public class EntManager : EntEngine
    {
        private static string ApiEntHost = Config.ApiHost;
        private static string UrlApiEntGetGroups = ApiEntHost + "/1/ent/get_groups";
        private static string UrlApiEntGetMembers = ApiEntHost + "/1/ent/get_members";
        private static string UrlApiGetMember = ApiEntHost + "/1/ent/get_member";
        private static string UrlApiEntGetRoles = ApiEntHost + "/1/ent/get_roles";
        private static string UrlApiAddSyncMember = ApiEntHost + "/1/ent/add_sync_member";
        private static string UrlApiDelSyncMember = ApiEntHost + "/1/ent/del_sync_member";
        private static string UrlApiAddSyncGroup = ApiEntHost + "/1/ent/add_sync_group";
        private static string UrlApiDelSyncGroup = ApiEntHost + "/1/ent/del_sync_group";
        private static string UrlApiAddSyncGroupMember = ApiEntHost + "/1/ent/add_sync_group_member";
        private static string UrlApiDelSyncGroupMember = ApiEntHost + "/1/ent/del_sync_group_member";
        private static string UrlApiDelSyncMemberGroup = ApiEntHost + "/1/ent/del_sync_member_group";
        private static string UrlApiGetGroupMembers = ApiEntHost + "/1/ent/get_group_members";
        private static string UrlApiAddAdmin = ApiEntHost + "/1/ent/add_sync_admin";
        private static string UrlApiMemberLoginReport = ApiEntHost + "/1/ent/member_login_report";

        public EntManager(string clientId, string secret) : base(clientId, secret)
        {
        }

        /// <summary>
        /// 获取成员
        /// </summary>
        /// <param name="start"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public ReturnResult GetMembers(int start, int size)
        {
            string url = UrlApiEntGetMembers;
            var parameter = new Dictionary<string, string>();
            parameter.Add("start", start.ToString());
            parameter.Add("size", size.ToString());
            return new RequestHelper(this).SetParams(parameter).SetUrl(url).SetMethod(RequestType.GET).ExecuteSync();
        }

        private ReturnResult GetMember(int memberId, string outId, string account)
        {
            string url = UrlApiGetMember;
            var parameter = new Dictionary<string, string>();
            if (memberId > 0)
            {
                parameter.Add("member_id", memberId.ToString());
            }
            parameter.Add("out_id", outId);
            parameter.Add("account", account);
            return new RequestHelper(this).SetParams(parameter).SetUrl(url).SetMethod(RequestType.GET).ExecuteSync();
        }

        /// <summary>
        /// 根据成员id获取企业成员信息
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public ReturnResult GetMemberById(int memberId)
        {
            return GetMember(memberId, null, null);
        }

        /// <summary>
        ///  根据外部id获取企业成员信息
        /// </summary>
        /// <param name="outId"></param>
        /// <returns></returns>

        public ReturnResult GetMemberByOutId(string outId)
        {
            return GetMember(0, outId, null);
        }

        /// <summary>
        /// 根据帐号获取企业成员信息
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public ReturnResult GetMemberByAccount(string account)
        {
            return GetMember(0, null, account);
        }

        /// <summary>
        ///     获取分组
        /// </summary>
        /// <returns></returns>
        public ReturnResult GetGroups()
        {
            string url = UrlApiEntGetGroups;
            return new RequestHelper(this).SetUrl(url).SetMethod(RequestType.GET).ExecuteSync();
        }

        /// <summary>
        /// 获取角色
        /// </summary>
        /// <returns></returns>
        public ReturnResult GetRoles()
        {
            string url = UrlApiEntGetRoles;
            return new RequestHelper(this).SetUrl(url).SetMethod(RequestType.GET).ExecuteSync();

        }

        /// <summary>
        /// 添加或修改同步成员
        /// </summary>
        /// <param name="outId"></param>
        /// <param name="memberName"></param>
        /// <param name="account"></param>
        /// <param name="memberEmail"></param>
        /// <param name="memberPhone"></param>
        /// <returns></returns>
        public ReturnResult AddSyncMember(string outId, string memberName,
            string account, string memberEmail, string memberPhone, string password, bool state)
        {
            string url = UrlApiAddSyncMember;
            var parameter = new Dictionary<string, string>();
            parameter.Add("out_id", outId);
            parameter.Add("member_name", memberName);
            parameter.Add("account", account);
            parameter.Add("member_email", memberEmail);
            parameter.Add("member_phone", memberPhone);
            parameter.Add("password", password);
            parameter.Add("state", state ? "1" : "0");
            return new RequestHelper(this).SetParams(parameter).SetUrl(url).SetMethod(RequestType.POST).ExecuteSync();
        }

        /// <summary>
        /// 设置成员状态
        /// </summary>
        /// <param name="outId"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public ReturnResult SetSyncMemberState(string outId, bool state)
        {
            string url = UrlApiAddSyncMember;
            var parameter = new Dictionary<string, string>();
            parameter.Add("out_id", outId);
            parameter.Add("state", state ? "1" : "0");
            return new RequestHelper(this).SetParams(parameter).SetUrl(url).SetMethod(RequestType.POST).ExecuteSync();
        }

        /// <summary>
        /// 删除同步成员
        /// </summary>
        /// <param name="groups"></param>
        /// <returns></returns>
        public ReturnResult DelSyncMember(string[] members)
        {
            string url = UrlApiDelSyncMember;
            var parameter = new Dictionary<string, string>();
            parameter.Add("members", Util.StrArrayToString(members, ","));
            return new RequestHelper(this).SetParams(parameter).SetUrl(url).SetMethod(RequestType.POST).ExecuteSync();
        }

        /// <summary>
        /// 添加或修改同步分组
        /// </summary>
        /// <param name="outId"></param>
        /// <param name="name"></param>
        /// <param name="parentOutId"></param>
        /// <returns></returns>
        public ReturnResult AddSyncGroup(string outId, string name, string parentOutId)
        {
            string url = UrlApiAddSyncGroup;
            var parameter = new Dictionary<string, string>();
            parameter.Add("out_id", outId);
            parameter.Add("name", name);
            parameter.Add("parent_out_id", parentOutId);
            return new RequestHelper(this).SetParams(parameter).SetUrl(url).SetMethod(RequestType.POST).ExecuteSync();
        }

        /// <summary>
        /// 删除同步分组
        /// </summary>
        /// <param name="groups"></param>
        /// <returns></returns>
        public ReturnResult DelSyncGroup(string[] groups)
        {
            string url = UrlApiDelSyncGroup;
            var parameter = new Dictionary<string, string>();
            parameter.Add("groups", Util.StrArrayToString(groups, ","));
            return new RequestHelper(this).SetParams(parameter).SetUrl(url).SetMethod(RequestType.POST).ExecuteSync();
        }

        /// <summary>
        /// 添加同步分组的成员
        /// </summary>
        /// <param name="groupOutId"></param>
        /// <param name="members"></param>
        /// <returns></returns>
        public ReturnResult AddSyncGroupMember(string groupOutId, string[] members)
        {
            string url = UrlApiAddSyncGroupMember;
            var parameter = new Dictionary<string, string>();
            parameter.Add("group_out_id", groupOutId);
            parameter.Add("members", Util.StrArrayToString(members, ","));
            return new RequestHelper(this).SetParams(parameter).SetUrl(url).SetMethod(RequestType.POST).ExecuteSync();
        }

        /// <summary>
        /// 删除同步分组的成员
        /// </summary>
        /// <param name="groupOutId"></param>
        /// <param name="members"></param>
        /// <returns></returns>
        public ReturnResult DelSyncGroupMember(string groupOutId, string[] members)
        {
            string url = UrlApiDelSyncGroupMember;
            var parameter = new Dictionary<string, string>();
            parameter.Add("group_out_id", groupOutId);
            parameter.Add("members", Util.StrArrayToString(members, ","));
            return new RequestHelper(this).SetParams(parameter).SetUrl(url).SetMethod(RequestType.POST).ExecuteSync();
        }

        /// <summary>
        /// 分组成员列表
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="start"></param>
        /// <param name="size"></param>
        /// <param name="showChild"></param>
        /// <returns></returns>
        public ReturnResult GetGroupMembers(int groupId, int start, int size, bool showChild)
        {
            string url = UrlApiGetGroupMembers;
            var parameter = new Dictionary<string, string>();
            parameter.Add("group_id", groupId.ToString());
            parameter.Add("start", start.ToString());
            parameter.Add("size", size.ToString());
            parameter.Add("show_child", (showChild ? "1" : "0"));
            return new RequestHelper(this).SetParams(parameter).SetUrl(url).SetMethod(RequestType.GET).ExecuteSync();
        }

        /// <summary>
        /// 删除成员的所有同步分组
        /// </summary>
        /// <param name="members"></param>
        /// <returns></returns>
        public ReturnResult DelSyncMemberGroup(string[] members)
        {
            string url = UrlApiDelSyncMemberGroup;
            var parameter = new Dictionary<string, string>();
            parameter.Add("members", Util.StrArrayToString(members, ","));
            return new RequestHelper(this).SetParams(parameter).SetUrl(url).SetMethod(RequestType.POST).ExecuteSync();
        }



        /// <summary>
        /// 添加管理员
        /// </summary>
        /// <param name="outId"></param>
        /// <param name="memberEmail"></param>
        /// <param name="isSuperAdmin"></param>
        /// <returns></returns>
        public ReturnResult AddSyncAdmin(string outId, string memberEmail, bool isSuperAdmin)
        {
            string url = UrlApiAddAdmin;
            var parameter = new Dictionary<string, string>();
            parameter.Add("out_id", outId);
            parameter.Add("member_email", memberEmail);
            parameter.Add("type", (isSuperAdmin ? "1" : "0"));
            return new RequestHelper(this).SetParams(parameter).SetUrl(url).SetMethod(RequestType.POST).ExecuteSync();
        }

        /// <summary>
        /// 成员登录情况统计
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="enDdate"></param>
        /// <returns></returns>
        public ReturnResult MemberLoginReport(string startDate, string endDate)
        {
            string url = UrlApiMemberLoginReport;
            var parameter = new Dictionary<string, string>();
            parameter.Add("start_date", startDate);
            parameter.Add("end_date", endDate);
            return new RequestHelper(this).SetParams(parameter).SetUrl(url).SetMethod(RequestType.GET).ExecuteSync();
        }

        public EntManager Clone()
        {
            return new EntManager(_clientId, _secret);
        }
    }
}