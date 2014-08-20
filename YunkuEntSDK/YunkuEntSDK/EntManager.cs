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
            var request = new HttpRequestSyn();
            request.RequestUrl = UrlApiEntGetMembers;
            request.AppendParameter("token", Token);
            request.AppendParameter("token_type", "ent");
            request.AppendParameter("start", start + "");
            request.AppendParameter("size", size + "");
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.Get;
            request.Request();
            StatusCode = request.Code;
            return request.Result;
        }

        /// <summary>
        ///     获取分组
        /// </summary>
        /// <returns></returns>
        public string GetGroups()
        {
            var request = new HttpRequestSyn();
            request.RequestUrl = UrlApiEntGetGroups;
            request.AppendParameter("token", Token);
            request.AppendParameter("token_type", "ent");
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.Get;
            request.Request();
            StatusCode = request.Code;
            return request.Result;
        }

        /// <summary>
        /// 获取角色
        /// </summary>
        /// <returns></returns>
        public string GetRoles()
        {
            var request = new HttpRequestSyn();
            request.RequestUrl = UrlApiEntGetRoles;
            request.AppendParameter("token", Token);
            request.AppendParameter("token_type", "ent");
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.Get;
            request.Request();
            StatusCode = request.Code;
            return request.Result;
        }

        /// <summary>
        ///     获取角色
        /// </summary>
        /// <returns></returns>
        public string GetEntRoles()
        {
            var request = new HttpRequestSyn();
            request.RequestUrl = UrlApiEntGetRoles;
            request.AppendParameter("token", Token);
            request.AppendParameter("token_type", "ent");
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.Get;
            request.Request();
            StatusCode = request.Code;
            return request.Result;
        }

        /// <summary>
        ///     根据成员id获取成员个人库外链
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public string GetMemberFileLink(int memberId)
        {
            var request = new HttpRequestSyn();
            request.RequestUrl = UrlApiGetMemberFileLink;
            request.AppendParameter("token", Token);
            request.AppendParameter("token_type", "ent");
            request.AppendParameter("member_id", memberId + "");
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.Get;
            request.Request();
            StatusCode = request.Code;
            return request.Result;
        }

        /// <summary>
        /// 根据外部成员id获取成员信息
        /// </summary>
        /// <param name="outIds"></param>
        /// <returns></returns>
        public string GetMemberByOutid(string[] outIds)
        {
            var request = new HttpRequestSyn();
            request.RequestUrl = UrlApiGetMemberByOutId;
            request.AppendParameter("token", Token);
            request.AppendParameter("token_type", "ent");
            request.AppendParameter("out_ids", Util.StrArrayToString(outIds, ","));
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.Get;
            request.Request();
            StatusCode = request.Code;
            return request.Result;
        }
    }
}