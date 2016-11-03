using YunkuEntSDK.Net;
using YunkuEntSDK.UtilClass;

namespace YunkuEntSDK
{
    public class EntLibManager : ParentManager
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

        public EntLibManager(string clientId, string clientSecret) : base(clientId, clientSecret, true)
        {
        }

        public EntLibManager(string clientId, string clientSecret, bool isEnt) : base(clientId, clientSecret, isEnt)
        {
        }

        public EntLibManager(string clientId, string clientSecret, bool isEnt, string token) : base(clientId, clientSecret, isEnt)
        {
            Token = token;
        }

        /// <summary>
        /// 创建库
        /// </summary>
        /// <param name="orgName"></param>
        /// <param name="orgCapacity"></param>
        /// <param name="storagePointName"></param>
        /// <param name="orgDesc"></param>
        /// <returns></returns>
        public string Create(string orgName, string orgCapacity, string storagePointName, string orgDesc,string orgLogo)
        {
            var request = new HttpRequestSyn {RequestUrl = UrlApiCreateLib};
            if (Token == null)
            {
                request.AppendParameter("client_id", _clientId);
                request.AppendParameter("dateline", Util.GetUnixDataline() + "");
            }
            else
            {
                request.AppendParameter("token", Token);
                request.AppendParameter("token_type", _tokenType);
            }
            request.AppendParameter("org_name", orgName);
            request.AppendParameter("org_capacity", orgCapacity );
            request.AppendParameter("storage_point_name", storagePointName);
            request.AppendParameter("org_desc", orgDesc);
            request.AppendParameter("org_logo", orgLogo);
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.Post;
            request.Request();
            return request.Result;
        }

        /// <summary>
        /// 获取库列表
        /// </summary>
        /// <returns></returns>
        public string GetLibList()
        {
            var request = new HttpRequestSyn {RequestUrl = UrlApiGetLibList};
            if (Token == null)
            {
                request.AppendParameter("client_id", _clientId);
                request.AppendParameter("dateline", Util.GetUnixDataline() + "");
            }
            else
            {
                request.AppendParameter("token", Token);
                request.AppendParameter("token_type", _tokenType);
            }
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.Get;
            request.Request();
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
            var request = new HttpRequestSyn {RequestUrl = UrlApiBind};
            if (Token == null)
            {
                request.AppendParameter("client_id", _clientId);
                request.AppendParameter("dateline", Util.GetUnixDataline() + "");
            }
            else
            {
                request.AppendParameter("token", Token);
                request.AppendParameter("token_type", _tokenType);
            }
            request.AppendParameter("org_id", orgId + "");
            request.AppendParameter("title", title);
            request.AppendParameter("url", linkUrl);
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.Post;
            request.Request();
            return request.Result;
        }

        /// <summary>
        /// 取消库授权
        /// </summary>
        /// <param name="orgClientId"></param>
        /// <returns></returns>
        public string UnBind(string orgClientId)
        {
            var request = new HttpRequestSyn {RequestUrl = UrlApiUnbind};
            if (Token == null)
            {
                request.AppendParameter("client_id", _clientId);
                request.AppendParameter("dateline", Util.GetUnixDataline() + "");
            }
            else
            {
                request.AppendParameter("token", Token);
                request.AppendParameter("token_type", _tokenType);
            }
            request.AppendParameter("org_client_id", orgClientId);
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.Post;
            request.Request();
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
            var request = new HttpRequestSyn {RequestUrl = UrlApiGetMembers};
            if (Token == null)
            {
                request.AppendParameter("client_id", _clientId);
                request.AppendParameter("dateline", Util.GetUnixDataline() + "");
            }
            else
            {
                request.AppendParameter("token", Token);
                request.AppendParameter("token_type", _tokenType);
            }
            request.AppendParameter("start", start + "");
            request.AppendParameter("size", size + "");
            request.AppendParameter("org_id", orgId + "");
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.Get;
            request.Request();
            return request.Result;
        }

        /// <summary>
        /// 查询库成员信息
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="type"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public string GetMember(int orgid, MemberType type, string[] ids)
        {
            var request = new HttpRequestSyn {RequestUrl = UrlApiGetMember};
            if (Token == null)
            {
                request.AppendParameter("client_id", _clientId);
                request.AppendParameter("dateline", Util.GetUnixDataline() + "");
            }
            else
            {
                request.AppendParameter("token", Token);
                request.AppendParameter("token_type", _tokenType);
            }
            request.AppendParameter("org_id", orgid + "");
            request.AppendParameter("type", type.ToString().ToLower() + "");
            request.AppendParameter("ids", Util.StrArrayToString(ids,","));
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.Get;
            request.Request();
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
            var request = new HttpRequestSyn {RequestUrl = UrlApiAddMembers};
            if (Token == null)
            {
                request.AppendParameter("client_id", _clientId);
                request.AppendParameter("dateline", Util.GetUnixDataline() + "");
            }
            else
            {
                request.AppendParameter("token", Token);
                request.AppendParameter("token_type", _tokenType);
            }
            request.AppendParameter("role_id", roleId + "");
            request.AppendParameter("org_id", orgId + "");
            request.AppendParameter("member_ids", Util.IntArrayToString(memberIds, ","));
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.Post;
            request.Request();
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
            var request = new HttpRequestSyn {RequestUrl = UrlApiSetMemberRole};
            if (Token == null)
            {
                request.AppendParameter("client_id", _clientId);
                request.AppendParameter("dateline", Util.GetUnixDataline() + "");
            }
            else
            {
                request.AppendParameter("token", Token);
                request.AppendParameter("token_type", _tokenType);
            }
            request.AppendParameter("role_id", roleId + "");
            request.AppendParameter("org_id", orgId + "");
            request.AppendParameter("member_ids", Util.IntArrayToString(memberIds, ","));
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.Post;
            request.Request();
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
            var request = new HttpRequestSyn {RequestUrl = UrlApiDelMember};
            if (Token == null)
            {
                request.AppendParameter("client_id", _clientId);
                request.AppendParameter("dateline", Util.GetUnixDataline() + "");
            }
            else
            {
                request.AppendParameter("token", Token);
                request.AppendParameter("token_type", _tokenType);
            }
            request.AppendParameter("org_id", orgId + "");
            request.AppendParameter("member_ids", Util.IntArrayToString(memberIds, ","));
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.Post;
            request.Request();
            return request.Result;
        }

        /// <summary>
        /// 获取库分组列表
        /// </summary>
        /// <param name="orgId"></param>
        /// <returns></returns>
        public string GetGroups(int orgId)
        {
            var request = new HttpRequestSyn {RequestUrl = UrlApiGetGroups};
            if (Token == null)
            {
                request.AppendParameter("client_id", _clientId);
                request.AppendParameter("dateline", Util.GetUnixDataline() + "");
            }
            else
            {
                request.AppendParameter("token", Token);
                request.AppendParameter("token_type", _tokenType);
            }
            request.AppendParameter("org_id", orgId + "");
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.Get;
            request.Request();
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
            var request = new HttpRequestSyn {RequestUrl = UrlApiAddGroup};
            if (Token == null)
            {
                request.AppendParameter("client_id", _clientId);
                request.AppendParameter("dateline", Util.GetUnixDataline() + "");
            }
            else
            {
                request.AppendParameter("token", Token);
                request.AppendParameter("token_type", _tokenType);
            }
            request.AppendParameter("org_id", orgId + "");
            request.AppendParameter("role_id", roleId + "");
            request.AppendParameter("group_id", groupId + "");
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.Post;
            request.Request();
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
            var request = new HttpRequestSyn {RequestUrl = UrlApiDelGroup};
            if (Token == null)
            {
                request.AppendParameter("client_id", _clientId);
                request.AppendParameter("dateline", Util.GetUnixDataline() + "");
            }
            else
            {
                request.AppendParameter("token", Token);
                request.AppendParameter("token_type", _tokenType);
            }
            request.AppendParameter("org_id", orgId + "");
            request.AppendParameter("group_id", groupId + "");
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.Post;
            request.Request();
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
            var request = new HttpRequestSyn {RequestUrl = UrlApiSetGroupRole};
            if (Token == null)
            {
                request.AppendParameter("client_id", _clientId);
                request.AppendParameter("dateline", Util.GetUnixDataline() + "");
            }
            else
            {
                request.AppendParameter("token", Token);
                request.AppendParameter("token_type", _tokenType);
            }
            request.AppendParameter("org_id", orgId + "");
            request.AppendParameter("group_id", groupId + "");
            request.AppendParameter("role_id", roleId + "");
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.Post;
            request.Request();
            return request.Result;
        }

        /// <summary>
        /// 删除库
        /// </summary>
        /// <param name="orgClientId"></param>
        /// <returns></returns>
        public string Destroy(string orgClientId)
        {
            var request = new HttpRequestSyn {RequestUrl = UrlApiDestroy};
            if (Token == null)
            {
                request.AppendParameter("client_id", _clientId);
                request.AppendParameter("dateline", Util.GetUnixDataline() + "");
            }
            else
            {
                request.AppendParameter("token", Token);
                request.AppendParameter("token_type", _tokenType);
            }
            request.AppendParameter("org_client_id", "" + orgClientId);
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.Post;
            request.Request();
            return request.Result;
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
        public string Set(int orgId, string orgName, string orgCapacity, string orgDesc, string orgLogo)
        {
            var request = new HttpRequestSyn {RequestUrl = UrlApiSet};
            if (Token == null)
            {
                request.AppendParameter("client_id", _clientId);
                request.AppendParameter("dateline", Util.GetUnixDataline() + "");
            }
            else
            {
                request.AppendParameter("token", Token);
                request.AppendParameter("token_type", _tokenType);
            }
            request.AppendParameter("org_id", "" + orgId);
            if (!string.IsNullOrEmpty(orgName))
            {
                request.AppendParameter("org_name", "" + orgName);
            }
            if (!string.IsNullOrEmpty(orgCapacity))
            {
                request.AppendParameter("org_capacity", "" + orgCapacity);
            }
            if (!string.IsNullOrEmpty(orgDesc))
            {
                request.AppendParameter("org_desc", "" + orgDesc);
            }
            if (!string.IsNullOrEmpty(orgDesc))
            {
                request.AppendParameter("org_logo", "" + orgLogo);
            }
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.Post;
            request.Request();
            return request.Result;

        }

        /// <summary>
        /// 获取库信息
        /// </summary>
        /// <param name="orgId"></param>
        /// <returns></returns>
 
        public string GetInfo(int orgId)
        {
            var request = new HttpRequestSyn {RequestUrl = UrlApiGetInfo};
            if (Token == null)
            {
                request.AppendParameter("client_id", _clientId);
                request.AppendParameter("dateline", Util.GetUnixDataline() + "");
            }
            else
            {
                request.AppendParameter("token", Token);
                request.AppendParameter("token_type", _tokenType);
            }
            request.AppendParameter("org_id", "" + orgId);
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.Get;
            request.Request();
            return request.Result;
        }

        public enum MemberType
        {
            Account ,
            OutId,
            MemberId
        }

    }
}