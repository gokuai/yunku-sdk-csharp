using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YunkuEntSDK
{
    internal interface IYunkuMethod
    {
         string AccessToken(bool isEnt);

         string CreateLib(string orgName, int orgCapacity, string storagePointName, string orgDesc);

         string GetLibList();

         string BindLib(int orgId, string title, string linkUrl);

         string UnBindLib(string orgClientId);

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

    }
}
