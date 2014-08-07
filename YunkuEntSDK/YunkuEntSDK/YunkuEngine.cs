using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace YunkuEntSDK
{
     public class YunkuEngine:IYunkuMethod
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

         public YunkuEngine(string uesrname, string password, string client_id, string client_secret)
         {
             _manager = new YunkuManager(uesrname, password, client_id, client_secret);
             _username=uesrname;
             _password=password;
             _clientId = client_id;
             _clientSecret = client_secret;
         }

         private YunkuEngine(string uesrname, string password, string client_id, string client_secret,string token)
             : this(uesrname, password, client_id, client_secret)
         {
             _manager.Token = token;
             
         }

        public string AccessToken(bool isEnt)
        {
            return _manager.AccessToken(isEnt);
        }

        public string CreateLib(string orgName, int orgCapacity, string storagePointName, string orgDesc)
        {
            return _manager.CreateLib(orgName, orgCapacity, storagePointName, orgDesc);
        }

        public string GetLibList()
        {
            return _manager.GetLibList();
        }

        public string BindLib(int orgId, string title, string linkUrl)
        {
            return _manager.BindLib( orgId,  title,  linkUrl);
        }

        public string UnBindLib(string orgClientId)
        {
            return _manager.UnBindLib(orgClientId);
        }

        public string GetFileList(string orgClientId, string orgClientSecret, int dateline, int start, string fullPath)
        {
            return _manager.GetFileList( orgClientId,  orgClientSecret,  dateline,  start,  fullPath);
        }

        public string GetUpdateList(string orgClientId, string orgClientSecret, int dateline, bool isCompare, long fetchDateline)
        {
            return _manager.GetUpdateList( orgClientId,  orgClientSecret,  dateline,  isCompare,  fetchDateline);
        }

        public string GetFileInfo(string orgClientId, string orgClientSecret, int dateline, string fullPath)
        {
            return _manager.GetFileInfo( orgClientId,  orgClientSecret,  dateline,  fullPath);
        }

        public string CreateFolder(string orgClientId, string orgClientSecret, int dateline, string fullPath, string opName)
        {
            return _manager.CreateFolder( orgClientId,  orgClientSecret,  dateline,  fullPath,  opName);
        }

        public string CreateFile(string orgClientId, string orgClientSecret, int dateline, string fullPath, string opName, System.IO.Stream stream, string fileName)
        {
            return _manager.CreateFile( orgClientId,  orgClientSecret,  dateline,  fullPath,  opName,  stream,  fileName);
        }

        public string CreateFile(string orgClientId, string orgClientSecret, int dateline, string fullPath, string opName, string localPath)
        {
            return _manager.CreateFile( orgClientId,  orgClientSecret,  dateline,  fullPath,  opName,  localPath);
        }

        public string Del(string orgClientId, string orgClientSecret, int dateline, string fullPath, string opName)
        {
            return _manager.Del(orgClientId, orgClientSecret, dateline, fullPath, opName);
        }

        public string Move(string orgClientId, string orgClientSecret, int dateline, string fullPath, string destFullPath, string opName)
        {
            return _manager.Move( orgClientId,  orgClientSecret,  dateline,  fullPath,  destFullPath,  opName);
        }

        public string Link(string orgClientId, string orgClientSecret, int dateline, string fullPath)
        {
            return _manager.Link( orgClientId,  orgClientSecret,  dateline,  fullPath);
        }

        public string SendMsg(string orgClientId, string orgClientSecret, int dateline, string title, string text, string image, string linkUrl, string opName)
        {
            return _manager.SendMsg(orgClientId, orgClientSecret, dateline, title, text, image, linkUrl, opName);
        }

        public static bool LogPrintAvialable 
        {
            set { YunkuManager.LogPrintAvialable = value; }
            get { return YunkuManager.LogPrintAvialable; }
        }

         /// <summary>
         /// 复制一个Yunku对象
         /// </summary>
         /// <returns></returns>
        public YunkuEngine clone() {
            return new YunkuEngine(_username, _password, _clientId, _clientSecret,_manager.Token);
        }

    }
}
