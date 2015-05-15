﻿using System;
using System.Data;
using YunkuEntSDK.Net;
using YunkuEntSDK.UtilClass;

namespace YunkuEntSDK
{
    public class EntManager : ParentManager
    {
        private const string LibHost = HostConfig.LibHost;
        private const string UrlApiEntGetGroups = LibHost + "/1/ent/get_groups";
        private const string UrlApiEntGetMembers = LibHost + "/1/ent/get_members";
        private const string UrlApiEntGetRoles = LibHost + "/1/ent/get_roles";
        private const string UrlApiEntSyncMember = LibHost + "/1/ent/sync_member";
        private const string UrlApiGetMemberFileLink = LibHost + "/1/ent/get_member_file_link";
        private const string UrlApiGetMemberByOutId = LibHost + "/1/ent/get_member_by_out_id";
        private const string UrlApiAddSyncMember = LibHost + "/1/ent/add_sync_member";
        private const string UrlApiDelSyncMember = LibHost + "/1/ent/del_sync_member";
        private const string UrlApiAddSyncGroup = LibHost + "/1/ent/add_sync_group";
        private const string UrlApiDelSyncGroup = LibHost + "/1/ent/del_sync_group";
        private const string UrlApiAddSyncGroupMember = LibHost + "/1/ent/add_sync_group_member";
        private const string UrlApiDelSyncGroupMember = LibHost + "/1/ent/del_sync_group_member";
        private const string UrlApiGetGroupMembers = LibHost + "/1/ent/get_group_members";

        public EntManager(string username, string password, string clientId, string clientSecret)
            : base(username, password, clientId, clientSecret)
        {
        }

//        /// <summary>
//        ///   同步成员和组织架构  
//        /// </summary>
//        /// <param name="structStr"></param>
//        /// <returns></returns>
//        public string SyncMembers(string structStr)
//        {
//            var request = new HttpRequestSyn();
//            request.RequestUrl = UrlApiEntSyncMember;
//            request.AppendParameter("token", Token);
//            request.AppendParameter("token_type", "ent");
//            request.AppendParameter("members", structStr);
//            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
//            request.RequestMethod = RequestType.Post;
//            request.Request();
//            StatusCode = request.Code;
//            return request.Result;
//        }

        /// <summary>
        ///     获取成员
        /// </summary>
        /// <param name="start"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public string GetMembers(int start, int size)
        {
            var request = new HttpRequestSyn {RequestUrl = UrlApiEntGetMembers};
            request.AppendParameter("token", Token);
            request.AppendParameter("token_type", "ent");
            request.AppendParameter("start", start + "");
            request.AppendParameter("size", size + "");
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.Get;
            request.Request();
            return request.Result;
        }


        /// <summary>
        ///     获取分组
        /// </summary>
        /// <returns></returns>
        public string GetGroups()
        {
            var request = new HttpRequestSyn {RequestUrl = UrlApiEntGetGroups};
            request.AppendParameter("token", Token);
            request.AppendParameter("token_type", "ent");
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.Get;
            request.Request();
            return request.Result;
        }

        /// <summary>
        /// 获取角色
        /// </summary>
        /// <returns></returns>
        public string GetRoles()
        {
            var request = new HttpRequestSyn {RequestUrl = UrlApiEntGetRoles};
            request.AppendParameter("token", Token);
            request.AppendParameter("token_type", "ent");
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
            var request = new HttpRequestSyn {RequestUrl = UrlApiEntGetRoles};
            request.AppendParameter("token", Token);
            request.AppendParameter("token_type", "ent");
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
        public string GetMemberFileLink(int memberId,bool fileOnly)
        {
            var request = new HttpRequestSyn {RequestUrl = UrlApiGetMemberFileLink};
            request.AppendParameter("token", Token);
            request.AppendParameter("token_type", "ent");
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
        /// 根据外部成员id获取成员信息
        /// </summary>
        /// <param name="outIds"></param>
        /// <returns></returns>
        public string GetMemberByOutid(string[] outIds)
        {
            if (outIds == null)
            {
                throw new NoNullAllowedException("outIds is null");
            }
            return GetMemberByIds(null,outIds);
        }

        /// <summary>
        /// 根据外部成员登陆帐号获取成员信息
        /// </summary>
        /// <param name="userIds"></param>
        /// <returns></returns>
        public string GetMemberByUserIds(string[] userIds)
        {
            if (userIds == null)
            {
                throw new NoNullAllowedException("userIds is null");
            }
            return GetMemberByIds(userIds, null);
        }


        private string GetMemberByIds(string[] userIds,string[] outIds)
        {
            var request = new HttpRequestSyn {RequestUrl = UrlApiGetMemberByOutId};
            request.AppendParameter("token", Token);
            request.AppendParameter("token_type", "ent");
            if (outIds != null)
            {
                request.AppendParameter("out_ids", Util.StrArrayToString(outIds, ","));
            }
            else
            {
                request.AppendParameter("user_ids", Util.StrArrayToString(userIds, ","));
            }
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.Request();
            request.RequestMethod = RequestType.Get;
            return request.Result;
        }

        /// <summary>
        /// 添加或修改同步成员
        /// </summary>
        /// <param name="oudId"></param>
        /// <param name="memberName"></param>
        /// <param name="account"></param>
        /// <param name="memberEmail"></param>
        /// <param name="memberPhone"></param>
        /// <returns></returns>
        public string AddSyncMember(string oudId,string memberName,
            string account,string memberEmail,string memberPhone,string password)
        {
            var request = new HttpRequestSyn {RequestUrl = UrlApiAddSyncMember};
            request.AppendParameter("token", Token);
            request.AppendParameter("token_type", "ent");
            request.AppendParameter("out_id",oudId);
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
        /// 删除同步成员
        /// </summary>
        /// <param name="groups"></param>
        /// <returns></returns>
        public string DelSyncMember(string[] members)
        {
            var request = new HttpRequestSyn {RequestUrl = UrlApiDelSyncMember};
            request.AppendParameter("token", Token);
            request.AppendParameter("token_type", "ent");
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
        public string AddSyncGroup(string outId,string name,string parentOutId)
        {
            var request = new HttpRequestSyn {RequestUrl = UrlApiAddSyncGroup};
            request.AppendParameter("token", Token);
            request.AppendParameter("token_type", "ent");
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
        public string DelSyncGroup(string[]groups)
        {
            var request = new HttpRequestSyn {RequestUrl = UrlApiDelSyncGroup};
            request.AppendParameter("token", Token);
            request.AppendParameter("token_type", "ent");
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
        public string AddSyncGroupMember(string groupOutId,string[] members)
        {
            var request = new HttpRequestSyn {RequestUrl = UrlApiAddSyncGroupMember};
            request.AppendParameter("token", Token);
            request.AppendParameter("token_type", "ent");
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
            var request = new HttpRequestSyn {RequestUrl = UrlApiDelSyncGroupMember};
            request.AppendParameter("token", Token);
            request.AppendParameter("token_type", "ent");
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
            var request = new HttpRequestSyn {RequestUrl = UrlApiGetGroupMembers};
            request.AppendParameter("token", Token);
            request.AppendParameter("token_type", "ent");
            request.AppendParameter("group_id", groupId+"");
            request.AppendParameter("start", start + "");
            request.AppendParameter("size", size + "");
            request.AppendParameter("show_child", (showChild?1:0) + "");
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.Post;
            request.Request();
            return request.Result;
        }



    }


    

}