using Microsoft.VisualStudio.TestTools.UnitTesting;
using YunkuEntSDK.compat.v2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YunkuEntSDK.Data;
using YunkuEntSDK.UtilClass;
using YunkuEntSDK.Net;

namespace YunkuEntSDK.compat.v2.Tests
{
    [TestClass()]
    public class EntFileManagerTests
    {

        public const string orgClientId = "";
        public const string orgClientSecret = "";

        [TestMethod()]
        public void GetFileListTest()
        {
            var entFile = new EntFileManager(orgClientId, orgClientSecret);
            ReturnResult result = entFile.GetFileList();
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void GetUpdateListTest()
        {
            var entFile = new EntFileManager(orgClientId, orgClientSecret);
            ReturnResult result = entFile.GetUpdateList(false, 0);
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void GetUpdateCountTest()
        {

            long now = UnixTimestampConverter.ConvertLocalToTimestamp(DateTime.Now);
            long beigin = UnixTimestampConverter.ConvertLocalToTimestamp(DateTime.Now.AddDays(-1));

            var entFile = new EntFileManager(orgClientId, orgClientSecret);
            ReturnResult result = entFile.GetUpdateCount(beigin, now, true);
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void GetFileInfoTest()
        {
            var entFile = new EntFileManager(orgClientId, orgClientSecret);
            ReturnResult result = entFile.GetFileInfo("test", EntFileManager.NetType.Default, false);
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void CreateFolderTest()
        {
            var entFile = new EntFileManager(orgClientId, orgClientSecret);
            ReturnResult result = entFile.CreateFolder("test", "Brandon");
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void CreateFileTest()
        {
            var entFile = new EntFileManager(orgClientId, orgClientSecret);
            ReturnResult result = entFile.CreateFile("testqqq.txt", "Brandon", "D:\\test.txt");
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void UploadByBlockTest()
        {
            var entLib = new EntFileManager(orgClientId, orgClientSecret);
            entLib.UploadByBlockAsync("testUPp.txt", "Brandon", 0, "D:\\test.txt", true, UploadCompeleted, ProgressChanged);
        }

        private void ProgressChanged(object sender, ProgressEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void UploadCompeleted(object sender, CompletedEventArgs e)
        {
            throw new NotImplementedException();
        }

        [TestMethod()]
        public void DelTest()
        {
            var entFile = new EntFileManager(orgClientId, orgClientSecret);
            ReturnResult result = entFile.Del("test", "Brandon");
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void MoveTest()
        {
            var entFile = new EntFileManager(orgClientId, orgClientSecret);
            ReturnResult result = entFile.Move("qq.jpg", "test/qq.jpg", "Brandon");
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void LinkTest()
        {
            var entFile = new EntFileManager(orgClientId, orgClientSecret);
            ReturnResult result = entFile.Link("qq.jpg", 0, EntFileManager.AuthType.Default, null);
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void SendMsgTest()
        {
            var entFile = new EntFileManager(orgClientId, orgClientSecret);
            ReturnResult result = entFile.SendMsg("msgTest", "msg", "", "", "Brandon");
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void LinksTest()
        {
            var entFile = new EntFileManager(orgClientId, orgClientSecret);
            ReturnResult result = entFile.Links(true);
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void CreateFileByUrlTest()
        {
            var entFile = new EntFileManager(orgClientId, orgClientSecret);
            ReturnResult result = entFile.CreateFileByUrl("1q.jpg", 0, "Brandon", true,
                "http://www.sinaimg.cn/dy/slidenews/1_img/2015_27/2841_589214_521618.jpg");
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void GetUploadServersTest()
        {
            var entFile = new EntFileManager(orgClientId, orgClientSecret);
            ReturnResult result = entFile.GetUploadServers();
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void GetServerSiteTest()
        {
            var entFile = new EntFileManager(orgClientId, orgClientSecret);
            ReturnResult result = entFile.GetServerSite("");
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void HistoryTest()
        {
            var entFile = new EntFileManager(orgClientId, orgClientSecret);
            ReturnResult result = entFile.History("test", 0, 100);
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void SearchTest()
        {
            var entFile = new EntFileManager(orgClientId, orgClientSecret);
            ReturnResult result = entFile.Search("test", "", 0, 100);
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void GetPermissionTest()
        {
            var entFile = new EntFileManager(orgClientId, orgClientSecret);
            ReturnResult result = entFile.GetPermission("test", 4);
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void SetPermissionTest()
        {
            var entFile = new EntFileManager(orgClientId, orgClientSecret);
            ReturnResult result = entFile.SetPermission("test", 4, EntFileManager.FilePermissions.FileDelete,
                EntFileManager.FilePermissions.FilePreview);
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void AddTagTest()
        {
            var entFile = new EntFileManager(orgClientId, orgClientSecret);
            ReturnResult result = entFile.AddTag("test", new[] { "test", "testTag" });
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void DelTagTest()
        {
            var entFile = new EntFileManager(orgClientId, orgClientSecret);
            ReturnResult result = entFile.DelTag("test", new[] { "test", "testTag" });
            Assert.AreEqual(200, result.Code);
        }
    }
}