using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YunkuEntSDK
{
    internal interface IEntLibMethod
    {
         string AccessToken(bool isEnt);

         string Create(string orgName, int orgCapacity, string storagePointName, string orgDesc);

         string GetLibList();

         string Bind(int orgId, string title, string linkUrl);

         string UnBind(string orgClientId);

         string GetFileList(string orgClientId, string orgClientSecret, int dateline, int start, string fullPath);

         string GetUpdateList(string orgClientId, string orgClientSecret, int dateline, bool isCompare, long fetchDateline);

         string GetFileInfo(string orgClientId, string orgClientSecret, int dateline, string fullPath);

         string CreateFolder(string orgClientId, string orgClientSecret, int dateline, string fullPath, string opName);

         string CreateFile(string orgClientId, string orgClientSecret, int dateline, string fullPath, string opName, Stream stream, string fileName);

         string CreateFile(string orgClientId, string orgClientSecret, int dateline, string fullPath, string opName, string localPath);

         string Del(string orgClientId, string orgClientSecret, int dateline, string fullPath, string opName);

         string Move(string orgClientId, string orgClientSecret, int dateline, string fullPath, string destFullPath, string opName);

         string Link(string orgClientId, string orgClientSecret, int dateline, string fullPath);

         string SendMsg(string orgClientId, string orgClientSecret, int dateline, string title, string text, string image, string linkUrl, string opName);

         string GetMembers(int start, int size, int orgId);

         string AddMembers(int orgId, int roleId, int[] memberIds);

         string SetMemberRole(int orgId, int roleId, int[] memberIds);

         string DelMember(int orgId, int[] memberIds);

         string GetGroups(int orgId);

         string AddGroup(int orgId, int groupId, int roleId);

         string DelGroup(int orgId, int groupId);

         string SetGroupRole(int orgId, int groupId, int roleId);




    }
}
