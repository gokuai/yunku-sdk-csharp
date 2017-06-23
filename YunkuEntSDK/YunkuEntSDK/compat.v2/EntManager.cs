using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YunkuEntSDK.Net;
using YunkuEntSDK.UtilClass;

namespace YunkuEntSDK.compat.v2
{
    class EntManager : OauthEngine
    {
        private const string ApiEntHost = HostConfig.ApiEntHostV2;
        private const string UrlApiEntGetGroups = ApiEntHost + "/1/ent/get_groups";
        private const string UrlApiEntGetMembers = ApiEntHost + "/1/ent/get_members";
        private const string UrlApiGetMember = ApiEntHost + "/1/ent/get_member";
        private const string UrlApiEntGetRoles = ApiEntHost + "/1/ent/get_roles";
        private const string UrlApiEntSyncMember = ApiEntHost + "/1/ent/sync_member";
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

        public EntManager(string clientId, string clientSecret, bool isEnt, string token) : base(clientId, clientSecret, isEnt)
        {
            Token = token;
        }

        /// <summary>
        /// 获取角色
        /// </summary>
        /// <returns></returns>
        public string GetRoles()
        {
            var request = new HttpRequestSyn { RequestUrl = UrlApiEntGetRoles };
            AddAuthParams(request);
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.Get;
            request.Request();
            return request.Result;
        }

        /// <summary>
        /// 获取成员
        /// </summary>
        /// <param name="start"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public string GetMembers(int start, int size)
        {
            var request = new HttpRequestSyn { RequestUrl = UrlApiEntGetMembers };
            AddAuthParams(request);
            request.AppendParameter("start", start + "");
            request.AppendParameter("size", size + "");
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.Get;
            request.Request();
            return request.Result;
        }

        private string GetMember(int memberId, string outId, string account)
        {
            var request = new HttpRequestSyn { RequestUrl = UrlApiGetMember };
            AddAuthParams(request);
            if (memberId > 0)
            {
                request.AppendParameter("member_id", memberId + "");
            }
            request.AppendParameter("out_id", outId);
            request.AppendParameter("account", account);
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.Get;
            request.Request();
            return request.Result;

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
            var request = new HttpRequestSyn { RequestUrl = UrlApiEntGetGroups };
            AddAuthParams(request);
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.Get;
            request.Request();
            return request.Result;
        }



        /// <summary>
        ///     获取角色
        /// </summary>
        /// <returns></returns>
        public string GetEntRoles()
        {
            var request = new HttpRequestSyn { RequestUrl = UrlApiEntGetRoles };
            AddAuthParams(request);
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.Get;
            request.Request();
            return request.Result;
        }

        /// <summary>
        ///     根据成员id获取成员个人库外链
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public string GetMemberFileLink(int memberId, bool fileOnly)
        {
            var request = new HttpRequestSyn { RequestUrl = UrlApiGetMemberFileLink };
            AddAuthParams(request);
            if (fileOnly)
            {
                request.AppendParameter("file", "1");
            }
            request.AppendParameter("member_id", memberId + "");
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.Get;
            request.Request();
            return request.Result;
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
            var request = new HttpRequestSyn { RequestUrl = UrlApiAddSyncMember };
            AddAuthParams(request);
            request.AppendParameter("out_id", outId);
            request.AppendParameter("member_name", memberName);
            request.AppendParameter("account", account);
            request.AppendParameter("member_email", memberEmail);
            request.AppendParameter("member_phone", memberPhone);
            request.AppendParameter("password", password);
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.Post;
            request.Request();
            return request.Result;
        }

        /// <summary>
        /// 设置成员状态
        /// </summary>
        /// <param name="outId"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public string SetSyncMemberState(string outId, bool state)
        {
            var request = new HttpRequestSyn { RequestUrl = UrlApiAddSyncMember };
            AddAuthParams(request);
            request.AppendParameter("out_id", outId);
            request.AppendParameter("state", state ? "1" : "0");
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.Post;
            request.Request();
            return request.Result;
        }

        /// <summary>
        /// 删除同步成员
        /// </summary>
        /// <param name="groups"></param>
        /// <returns></returns>
        public string DelSyncMember(string[] members)
        {
            var request = new HttpRequestSyn { RequestUrl = UrlApiDelSyncMember };
            AddAuthParams(request);
            request.AppendParameter("members", Util.StrArrayToString(members, ","));
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.Post;
            request.Request();
            return request.Result;
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
            var request = new HttpRequestSyn { RequestUrl = UrlApiAddSyncGroup };
            AddAuthParams(request);
            request.AppendParameter("out_id", outId);
            request.AppendParameter("name", name);
            if (!string.IsNullOrEmpty(parentOutId))
            {
                request.AppendParameter("parent_out_id", parentOutId);
            }

            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.Post;
            request.Request();
            return request.Result;
        }

        /// <summary>
        /// 删除同步分组
        /// </summary>
        /// <param name="groups"></param>
        /// <returns></returns>
        public string DelSyncGroup(string[] groups)
        {
            var request = new HttpRequestSyn { RequestUrl = UrlApiDelSyncGroup };
            AddAuthParams(request);
            request.AppendParameter("groups", Util.StrArrayToString(groups, ","));
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.Post;
            request.Request();
            return request.Result;
        }

        /// <summary>
        /// 添加同步分组的成员
        /// </summary>
        /// <param name="groupOutId"></param>
        /// <param name="members"></param>
        /// <returns></returns>
        public string AddSyncGroupMember(string groupOutId, string[] members)
        {
            var request = new HttpRequestSyn { RequestUrl = UrlApiAddSyncGroupMember };
            AddAuthParams(request);
            request.AppendParameter("group_out_id", groupOutId);
            request.AppendParameter("members", Util.StrArrayToString(members, ","));
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.Post;
            request.Request();
            return request.Result;
        }

        /// <summary>
        /// 删除同步分组的成员
        /// </summary>
        /// <param name="groupOutId"></param>
        /// <param name="members"></param>
        /// <returns></returns>
        public string DelSyncGroupMember(string groupOutId, string[] members)
        {
            var request = new HttpRequestSyn { RequestUrl = UrlApiDelSyncGroupMember };
            AddAuthParams(request);
            request.AppendParameter("group_out_id", groupOutId);
            request.AppendParameter("members", Util.StrArrayToString(members, ","));
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.Post;
            request.Request();
            return request.Result;
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
            var request = new HttpRequestSyn { RequestUrl = UrlApiGetGroupMembers };
            AddAuthParams(request);
            request.AppendParameter("group_id", groupId + "");
            request.AppendParameter("start", start + "");
            request.AppendParameter("size", size + "");
            request.AppendParameter("show_child", (showChild ? 1 : 0) + "");
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.Get;
            request.Request();
            return request.Result;
        }

        /// <summary>
        /// 删除成员的所有同步分组
        /// </summary>
        /// <param name="members"></param>
        /// <returns></returns>
        public string DelSyncMemberGroup(string[] members)
        {
            var request = new HttpRequestSyn { RequestUrl = UrlApiDelSyncMemberGroup };
            AddAuthParams(request);
            request.AppendParameter("members", Util.StrArrayToString(members, ","));
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.Post;
            request.Request();
            return request.Result;
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
