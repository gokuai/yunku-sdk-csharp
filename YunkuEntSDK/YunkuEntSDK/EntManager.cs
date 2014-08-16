using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YunkuEntSDK.Net;

namespace YunkuEntSDK
{
    public class EntManager:ParentManager
    {
        const string LIB_HOST = HostConfig.LIB_HOST;
        const string URL_API_ENT_GET_GROUPS = LIB_HOST + "/1/ent/get_groups";
        const string URL_API_ENT_GET_MEMBERS = LIB_HOST + "/1/ent/get_members";
        const string URL_API_ENT_GET_ROLES = LIB_HOST + "/1/ent/get_roles";
        const string URL_API_ENT_SYNC_MEMBER = LIB_HOST + "/1/ent/sync_member";

       public EntManager(string username, string password, string clientId, string clientSecret)
           :base(username,password,clientId,clientSecret)
       {
       }

       public string GetMembers(int start, int size)
       {
           HttpRequestSyn request = new HttpRequestSyn();
           request.RequestUrl = URL_API_ENT_GET_MEMBERS;
           request.AppendParameter("token", _token);
           request.AppendParameter("token_type", "ent");
           request.AppendParameter("start", start + "");
           request.AppendParameter("size", size + "");
           request.AppendParameter("sign", GenerateSign(request.SortedParamter));
           request.RequestMethod = RequestType.GET;
           request.Request();
           this.StatusCode = request.Code;
           return request.Result;
       }

       public string GetGroups()
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

       public string GetRoles()
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

       public string SyncMembers(string structStr)
       {
           HttpRequestSyn request = new HttpRequestSyn();
           request.RequestUrl = URL_API_ENT_GET_GROUPS;
           request.AppendParameter("token", _token);
           request.AppendParameter("token_type", "ent");
           request.AppendParameter("members", structStr);
           request.AppendParameter("sign", GenerateSign(request.SortedParamter));
           request.RequestMethod = RequestType.GET;
           request.Request();
           this.StatusCode = request.Code;
           return request.Result;
       }
    }
}
