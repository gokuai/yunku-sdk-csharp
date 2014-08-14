using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace YunkuEntSDK
{
    public class EntManager:IEntMethod
    {
        private YunkuManager _manager;
         private string _username;
         private string _password;
         private string _clientId;
         private string _clientSecret;

         public HttpStatusCode StatusCode 
         {
             get { return _manager.StatusCode; }
         }

         public EntManager(string uesrname, string password, string client_id, string client_secret)
         {
             _manager = new YunkuManager(uesrname, password, client_id, client_secret);
             _username=uesrname;
             _password=password;
             _clientId = client_id;
             _clientSecret = client_secret;
         }

         private EntManager(string uesrname, string password, string client_id, string client_secret, string token)
             : this(uesrname, password, client_id, client_secret)
         {
             _manager.Token = token;
             
         }

        /// <summary>
        /// 获取角色
        /// </summary>
        /// <returns></returns>
        public string GetEntRoles()
        {
            return _manager.GetEntRoles();
        }
        /// <summary>
        /// 获取成员
        /// </summary>
        /// <param name="start"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public string GetEntMembers(int start, int size)
        {
            return _manager.GetEntMembers(start,size);
        }
        /// <summary>
        /// 获取分组
        /// </summary>
        /// <returns></returns>
        public string GetEntGroups()
        {
            return _manager.GetEntGroups();
        }

        public string SyncMembers(string structStr)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 复制一个Yunku对象
        /// </summary>
        /// <returns></returns>
        public EntManager clone()
        {
            return new EntManager(_username, _password, _clientId, _clientSecret, _manager.Token);
        }

        public string AccessToken(bool isEnt)
        {
            return _manager.AccessToken(isEnt);
        }
    }
}
