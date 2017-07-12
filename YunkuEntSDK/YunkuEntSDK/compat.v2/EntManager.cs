using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YunkuEntSDK.Net;
using YunkuEntSDK.UtilClass;

namespace YunkuEntSDK.compat.v2
{
    public class EntManager : OauthEngine
    {
        private const string ApiEntHost = HostConfig.ApiEntHostV2;
        private const string UrlApiEntGetGroups = ApiEntHost + "/1/ent/get_groups";
        private const string UrlApiEntGetMembers = ApiEntHost + "/1/ent/get_members";
        private const string UrlApiGetMember = ApiEntHost + "/1/ent/get_member";
        private const string UrlApiEntGetRoles = ApiEntHost + "/1/ent/get_roles";
        //private const string UrlApiEntSyncMember = ApiEntHost + "/1/ent/sync_member";
        private const string UrlApiGetMemberFileLink = ApiEntHost + "/1/ent/get_member_file_link";
        private const string UrlApiAddSyncMember = ApiEntHost + "/1/ent/add_sync_member";
        private const string UrlApiDelSyncMember = ApiEntHost + "/1/ent/del_sync_member";
        private const string UrlApiAddSyncGroup = ApiEntHost + "/1/ent/add_sync_group";
        private const string UrlApiDelSyncGroup = ApiEntHost + "/1/ent/del_sync_group";
        private const string UrlApiAddSyncGroupMember = ApiEntHost + "/1/ent/add_sync_group_member";
        private const string UrlApiDelSyncGroupMember = ApiEntHost + "/1/ent/del_sync_group_member";
        private const string UrlApiDelSyncMemberGroup = ApiEntHost + "/1/ent/del_sync_member_group";
        private const string UrlApiGetGroupMembers = ApiEntHost + "/1/ent/get_group_members";

        public EntManager(string clientId, string clientSecret) : base(clientId, clientSecret, true)
        {
        }

        public EntManager(string clientId, string clientSecret, bool isEnt, string token) : base(clientId, clientSecret, isEnt, token)
        {
            Token = token;
        }

        /// <summary>
        /// 获取角色
        /// </summary>
        /// <returns></returns>
        public string GetRoles()
        {
            string url = UrlApiEntGetRoles;
            var parameter = new Dictionary<string, string>();
            AddAuthParams(parameter);
            parameter.Add("sign", GenerateSign(parameter));
            return new RequestHelper().SetParams(parameter).SetUrl(url).SetMethod(RequestType.Get).ExecuteSync();

        }

        /// <summary>
        /// 获取成员
        /// </summary>
        /// <param name="start"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public string GetMembers(int start, int size)
        {
            string url = UrlApiEntGetMembers;
            var parameter = new Dictionary<string, string>();
            AddAuthParams(parameter);
            parameter.Add("start", start + "");
            parameter.Add("size", size + "");
            parameter.Add("sign", GenerateSign(parameter));
            return new RequestHelper().SetParams(parameter).SetUrl(url).SetMethod(RequestType.Get).ExecuteSync();
        }

        private string GetMember(int memberId, string outId, string account)
        {
            string url = UrlApiGetMember;
            var parameter = new Dictionary<string, string>();
            AddAuthParams(parameter);
            if (memberId > 0)
            {
                parameter.Add("member_id", memberId + "");
            }
            parameter.Add("out_id", outId);
            parameter.Add("account", account);
            parameter.Add("sign", GenerateSign(parameter));
            return new RequestHelper().SetParams(parameter).SetUrl(url).SetMethod(RequestType.Get).ExecuteSync();
        }

        /// <summary>
        /// 根据成员id获取企业成员信息
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public string GetMemberById(int memberId)
        {
            return GetMember(memberId, null, null);
        }

        /// <summary>
        ///  根据外部id获取企业成员信息
        /// </summary>
        /// <param name="outId"></param>
        /// <returns></returns>

        public string GetMemberByOutId(string outId)
        {
            return GetMember(0, outId, null);
        }

        /// <summary>
        /// 根据帐号获取企业成员信息
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public string GetMemberByAccount(string account)
        {
            return GetMember(0, null, account);
        }

        /// <summary>
        ///     获取分组
        /// </summary>
        /// <returns></returns>
        public string GetGroups()
        {
            string url = UrlApiEntGetGroups;
            var parameter = new Dictionary<string, string>();
            AddAuthParams(parameter);
            parameter.Add("sign", GenerateSign(parameter));
            return new RequestHelper().SetParams(parameter).SetUrl(url).SetMethod(RequestType.Get).ExecuteSync();
        }

        /// <summary>
        ///     根据成员id获取成员个人库外链
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public string GetMemberFileLink(int memberId, bool fileOnly)
        {
            string url = UrlApiGetMemberFileLink;
            var parameter = new Dictionary<string, string>();
            AddAuthParams(parameter);
            parameter.Add("member_id", memberId + "");
            if (fileOnly)
            {
                parameter.Add("file", "1");
            }

            parameter.Add("sign", GenerateSign(parameter));
            return new RequestHelper().SetParams(parameter).SetUrl(url).SetMethod(RequestType.Get).ExecuteSync();
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
        public string AddSyncMember(string outId, string memberName,
            string account, string memberEmail, string memberPhone, string password)
        {
            string url = UrlApiAddSyncMember;
            var parameter = new Dictionary<string, string>();
            AddAuthParams(parameter);
            parameter.Add("out_id", outId);
            parameter.Add("member_name", memberName);
            parameter.Add("account", account);
            parameter.Add("member_email", memberEmail);
            parameter.Add("member_phone", memberPhone);
            parameter.Add("password", password);
            parameter.Add("sign", GenerateSign(parameter));
            return new RequestHelper().SetParams(parameter).SetUrl(url).SetMethod(RequestType.Post).ExecuteSync();
        }

        /// <summary>
        /// 设置成员状态
        /// </summary>
        /// <param name="outId"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public string SetSyncMemberState(string outId, bool state)
        {
            string url = UrlApiAddSyncMember;
            var parameter = new Dictionary<string, string>();
            AddAuthParams(parameter);
            parameter.Add("out_id", outId);
            parameter.Add("state", state ? "1" : "0");
            parameter.Add("sign", GenerateSign(parameter));
            return new RequestHelper().SetParams(parameter).SetUrl(url).SetMethod(RequestType.Post).ExecuteSync();
        }

        /// <summary>
        /// 删除同步成员
        /// </summary>
        /// <param name="groups"></param>
        /// <returns></returns>
        public string DelSyncMember(string[] members)
        {
            string url = UrlApiDelSyncMember;
            var parameter = new Dictionary<string, string>();
            AddAuthParams(parameter);
            parameter.Add("members", Util.StrArrayToString(members, ","));
            parameter.Add("sign", GenerateSign(parameter));
            return new RequestHelper().SetParams(parameter).SetUrl(url).SetMethod(RequestType.Post).ExecuteSync();
        }

        /// <summary>
        /// 添加或修改同步分组
        /// </summary>
        /// <param name="outId"></param>
        /// <param name="name"></param>
        /// <param name="parentOutId"></param>
        /// <returns></returns>
        public string AddSyncGroup(string outId, string name, string parentOutId)
        {
            string url = UrlApiAddSyncGroup;
            var parameter = new Dictionary<string, string>();
            AddAuthParams(parameter);
            parameter.Add("out_id", outId);
            parameter.Add("name", name);
            parameter.Add("parent_out_id", parentOutId);
            parameter.Add("sign", GenerateSign(parameter));
            return new RequestHelper().SetParams(parameter).SetUrl(url).SetMethod(RequestType.Post).ExecuteSync();
        }

        /// <summary>
        /// 删除同步分组
        /// </summary>
        /// <param name="groups"></param>
        /// <returns></returns>
        public string DelSyncGroup(string[] groups)
        {
            string url = UrlApiDelSyncGroup;
            var parameter = new Dictionary<string, string>();
            AddAuthParams(parameter);
            parameter.Add("groups", Util.StrArrayToString(groups, ","));
            parameter.Add("sign", GenerateSign(parameter));
            return new RequestHelper().SetParams(parameter).SetUrl(url).SetMethod(RequestType.Post).ExecuteSync();
        }

        /// <summary>
        /// 添加同步分组的成员
        /// </summary>
        /// <param name="groupOutId"></param>
        /// <param name="members"></param>
        /// <returns></returns>
        public string AddSyncGroupMember(string groupOutId, string[] members)
        {
            string url = UrlApiAddSyncGroupMember;
            var parameter = new Dictionary<string, string>();
            AddAuthParams(parameter);
            parameter.Add("group_out_id", groupOutId);
            parameter.Add("members", Util.StrArrayToString(members, ","));
            parameter.Add("sign", GenerateSign(parameter));
            return new RequestHelper().SetParams(parameter).SetUrl(url).SetMethod(RequestType.Post).ExecuteSync();
        }

        /// <summary>
        /// 删除同步分组的成员
        /// </summary>
        /// <param name="groupOutId"></param>
        /// <param name="members"></param>
        /// <returns></returns>
        public string DelSyncGroupMember(string groupOutId, string[] members)
        {
            string url = UrlApiDelSyncGroupMember;
            var parameter = new Dictionary<string, string>();
            AddAuthParams(parameter);
            parameter.Add("group_out_id", groupOutId);
            parameter.Add("members", Util.StrArrayToString(members, ","));
            parameter.Add("sign", GenerateSign(parameter));
            return new RequestHelper().SetParams(parameter).SetUrl(url).SetMethod(RequestType.Post).ExecuteSync();
        }

        /// <summary>
        /// 分组成员列表
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="start"></param>
        /// <param name="size"></param>
        /// <param name="showChild"></param>
        /// <returns></returns>
        public string GetGroupMembers(int groupId, int start, int size, bool showChild)
        {
            string url = UrlApiGetGroupMembers;
            var parameter = new Dictionary<string, string>();
            AddAuthParams(parameter);
            parameter.Add("group_id", groupId + "");
            parameter.Add("start", start + "");
            parameter.Add("size", size + "");
            parameter.Add("show_child", (showChild ? 1 : 0) + "");
            parameter.Add("sign", GenerateSign(parameter));
            return new RequestHelper().SetParams(parameter).SetUrl(url).SetMethod(RequestType.Get).ExecuteSync();
        }

        /// <summary>
        /// 删除成员的所有同步分组
        /// </summary>
        /// <param name="members"></param>
        /// <returns></returns>
        public string DelSyncMemberGroup(string[] members)
        {
            {
                string url = UrlApiDelSyncMemberGroup;
                var parameter = new Dictionary<string, string>();
                AddAuthParams(parameter);
                parameter.Add("members", Util.StrArrayToString(members, ","));
                parameter.Add("sign", GenerateSign(parameter));
                return new RequestHelper().SetParams(parameter).SetUrl(url).SetMethod(RequestType.Post).ExecuteSync();
            }
        }

        /// <summary>
        /// 复制一个EntManager对象
        /// </summary>
        /// <returns></returns>
        public EntManager Clone()
        {
            return new EntManager(_clientId, _clientSecret, _isEnt, Token);
        }
    }
}
