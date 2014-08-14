using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace YunkuEntSDK
{
     public class EntLibManager:IEntLibMethod
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

         public EntLibManager(string uesrname, string password, string client_id, string client_secret)
         {
             _manager = new YunkuManager(uesrname, password, client_id, client_secret);
             _username=uesrname;
             _password=password;
             _clientId = client_id;
             _clientSecret = client_secret;
         }

         private EntLibManager(string uesrname, string password, string client_id, string client_secret,string token)
             : this(uesrname, password, client_id, client_secret)
         {
             _manager.Token = token;
             
         }

         /// <summary>
         /// 获取认证
         /// </summary>
         /// <param name="isEnt"></param>
         /// <returns></returns>
        public string AccessToken(bool isEnt)
        {
            return _manager.AccessToken(isEnt);
        }
         /// <summary>
         /// 创建库
         /// </summary>
         /// <param name="orgName"></param>
         /// <param name="orgCapacity"></param>
         /// <param name="storagePointName"></param>
         /// <param name="orgDesc"></param>
         /// <returns></returns>
        public string Create(string orgName, int orgCapacity, string storagePointName, string orgDesc)
        {
            return _manager.Create(orgName, orgCapacity, storagePointName, orgDesc);
        }
         /// <summary>
         /// 获取库里欸包
         /// </summary>
         /// <returns></returns>
        public string GetLibList()
        {
            return _manager.GetLibList();
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
            return _manager.Bind( orgId,  title,  linkUrl);
        }
         /// <summary>
         /// 取消库授权
         /// </summary>
         /// <param name="orgClientId"></param>
         /// <returns></returns>
        public string UnBind(string orgClientId)
        {
            return _manager.UnBind(orgClientId);
        }
         /// <summary>
         /// 获取文件列表
         /// </summary>
         /// <param name="orgClientId"></param>
         /// <param name="orgClientSecret"></param>
         /// <param name="dateline"></param>
         /// <param name="start"></param>
         /// <param name="fullPath"></param>
         /// <returns></returns>
        public string GetFileList(string orgClientId, string orgClientSecret, int dateline, int start, string fullPath)
        {
            return _manager.GetFileList( orgClientId,  orgClientSecret,  dateline,  start,  fullPath);
        }
         /// <summary>
         /// 获取更新列表
         /// </summary>
         /// <param name="orgClientId"></param>
         /// <param name="orgClientSecret"></param>
         /// <param name="dateline"></param>
         /// <param name="isCompare"></param>
         /// <param name="fetchDateline"></param>
         /// <returns></returns>
        public string GetUpdateList(string orgClientId, string orgClientSecret, int dateline, bool isCompare, long fetchDateline)
        {
            return _manager.GetUpdateList( orgClientId,  orgClientSecret,  dateline,  isCompare,  fetchDateline);
        }
         /// <summary>
         /// 获取文件信息
         /// </summary>
         /// <param name="orgClientId"></param>
         /// <param name="orgClientSecret"></param>
         /// <param name="dateline"></param>
         /// <param name="fullPath"></param>
         /// <returns></returns>
        public string GetFileInfo(string orgClientId, string orgClientSecret, int dateline, string fullPath)
        {
            return _manager.GetFileInfo( orgClientId,  orgClientSecret,  dateline,  fullPath);
        }
         /// <summary>
         /// 创建文件夹
         /// </summary>
         /// <param name="orgClientId"></param>
         /// <param name="orgClientSecret"></param>
         /// <param name="dateline"></param>
         /// <param name="fullPath"></param>
         /// <param name="opName"></param>
         /// <returns></returns>
        public string CreateFolder(string orgClientId, string orgClientSecret, int dateline, string fullPath, string opName)
        {
            return _manager.CreateFolder( orgClientId,  orgClientSecret,  dateline,  fullPath,  opName);
        }
         /// <summary>
         /// 上传文件（流方式）
         /// </summary>
         /// <param name="orgClientId"></param>
         /// <param name="orgClientSecret"></param>
         /// <param name="dateline"></param>
         /// <param name="fullPath"></param>
         /// <param name="opName"></param>
         /// <param name="stream"></param>
         /// <param name="fileName"></param>
         /// <returns></returns>
        public string CreateFile(string orgClientId, string orgClientSecret, int dateline, string fullPath, string opName, System.IO.Stream stream, string fileName)
        {
            return _manager.CreateFile( orgClientId,  orgClientSecret,  dateline,  fullPath,  opName,  stream,  fileName);
        }
         /// <summary>
         ///上传文件
         /// </summary>
         /// <param name="orgClientId"></param>
         /// <param name="orgClientSecret"></param>
         /// <param name="dateline"></param>
         /// <param name="fullPath"></param>
         /// <param name="opName"></param>
         /// <param name="localPath"></param>
         /// <returns></returns>
        public string CreateFile(string orgClientId, string orgClientSecret, int dateline, string fullPath, string opName, string localPath)
        {
            return _manager.CreateFile( orgClientId,  orgClientSecret,  dateline,  fullPath,  opName,  localPath);
        }
         /// <summary>
         /// 删除文件
         /// </summary>
         /// <param name="orgClientId"></param>
         /// <param name="orgClientSecret"></param>
         /// <param name="dateline"></param>
         /// <param name="fullPath"></param>
         /// <param name="opName"></param>
         /// <returns></returns>
        public string Del(string orgClientId, string orgClientSecret, int dateline, string fullPath, string opName)
        {
            return _manager.Del(orgClientId, orgClientSecret, dateline, fullPath, opName);
        }
         /// <summary>
         /// 移动文件
         /// </summary>
         /// <param name="orgClientId"></param>
         /// <param name="orgClientSecret"></param>
         /// <param name="dateline"></param>
         /// <param name="fullPath"></param>
         /// <param name="destFullPath"></param>
         /// <param name="opName"></param>
         /// <returns></returns>
        public string Move(string orgClientId, string orgClientSecret, int dateline, string fullPath, string destFullPath, string opName)
        {
            return _manager.Move( orgClientId,  orgClientSecret,  dateline,  fullPath,  destFullPath,  opName);
        }
         /// <summary>
         /// 文件链接
         /// </summary>
         /// <param name="orgClientId"></param>
         /// <param name="orgClientSecret"></param>
         /// <param name="dateline"></param>
         /// <param name="fullPath"></param>
         /// <returns></returns>
        public string Link(string orgClientId, string orgClientSecret, int dateline, string fullPath)
        {
            return _manager.Link( orgClientId,  orgClientSecret,  dateline,  fullPath);
        }
         /// <summary>
         ///发送消息
         /// </summary>
         /// <param name="orgClientId"></param>
         /// <param name="orgClientSecret"></param>
         /// <param name="dateline"></param>
         /// <param name="title"></param>
         /// <param name="text"></param>
         /// <param name="image"></param>
         /// <param name="linkUrl"></param>
         /// <param name="opName"></param>
         /// <returns></returns>
        public string SendMsg(string orgClientId, string orgClientSecret, int dateline, string title, string text, string image, string linkUrl, string opName)
        {
            return _manager.SendMsg(orgClientId, orgClientSecret, dateline, title, text, image, linkUrl, opName);
        }



        

         /// <summary>
         /// 复制一个Yunku对象
         /// </summary>
         /// <returns></returns>
        public EntLibManager clone() {
            return new EntLibManager(_username, _password, _clientId, _clientSecret,_manager.Token);
        }




         /// <summary>
         /// 获取库成员
         /// </summary>
         /// <param name="start"></param>
         /// <param name="size"></param>
         /// <param name="orgId"></param>
         /// <returns></returns>
        public string GetMembers(int start, int size, int orgId)
        {
            return _manager.GetMembers(start, size, orgId);
        }
         /// <summary>
         /// 添加成员
         /// </summary>
         /// <param name="orgId"></param>
         /// <param name="roleId"></param>
         /// <param name="memberIds"></param>
         /// <returns></returns>
        public string AddMembers(int orgId, int roleId, int[] memberIds)
        {
            return _manager.AddMembers(orgId, roleId, memberIds);
        }
         /// <summary>
         /// 设置成员角色
         /// </summary>
         /// <param name="orgId"></param>
         /// <param name="roleId"></param>
         /// <param name="memberIds"></param>
         /// <returns></returns>
        public string SetMemberRole(int orgId, int roleId, int[] memberIds)
        {
            return _manager.SetMemberRole(orgId, roleId, memberIds);
        }
         /// <summary>
         /// 删除成员
         /// </summary>
         /// <param name="orgId"></param>
         /// <param name="memberIds"></param>
         /// <returns></returns>
        public string DelMember(int orgId, int[] memberIds)
        {
            return _manager.DelMember(orgId, memberIds);
        }
         /// <summary>
         /// 获取库分组
         /// </summary>
         /// <param name="orgId"></param>
         /// <returns></returns>
        public string GetGroups(int orgId)
        {
            return _manager.GetGroups(orgId);
        }
         /// <summary>
         /// 添加分组
         /// </summary>
         /// <param name="orgId"></param>
         /// <param name="groupId"></param>
         /// <param name="roleId"></param>
         /// <returns></returns>
        public string AddGroup(int orgId, int groupId, int roleId)
        {
            return _manager.AddGroup(orgId, groupId, roleId);
        }
         /// <summary>
         /// 删除分组
         /// </summary>
         /// <param name="orgId"></param>
         /// <param name="groupId"></param>
         /// <returns></returns>
        public string DelGroup(int orgId, int groupId)
        {
            return _manager.DelGroup(orgId, groupId);
        }
         /// <summary>
         /// 设置分组橘色
         /// </summary>
         /// <param name="orgId"></param>
         /// <param name="groupId"></param>
         /// <param name="roleId"></param>
         /// <returns></returns>
        public string SetGroupRole(int orgId, int groupId, int roleId)
        {
            return _manager.SetGroupRole(orgId, groupId, roleId);
        }
    }
}
