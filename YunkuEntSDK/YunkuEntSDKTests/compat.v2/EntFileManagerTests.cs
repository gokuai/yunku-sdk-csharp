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
            string returnString = entFile.GetFileList();
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }

        [TestMethod()]
        public void GetUpdateListTest()
        {
            var entFile = new EntFileManager(orgClientId, orgClientSecret);
            string returnString = entFile.GetUpdateList(false, 0);
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }

        [TestMethod()]
        public void GetUpdateCountTest()
        {

            long now = UnixTimestampConverter.ConvertLocalToTimestamp(DateTime.Now);
            long beigin = UnixTimestampConverter.ConvertLocalToTimestamp(DateTime.Now.AddDays(-1));

            var entFile = new EntFileManager(orgClientId, orgClientSecret);
            string returnString = entFile.GetUpdateCount(beigin, now, true);
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }

        [TestMethod()]
        public void GetFileInfoTest()
        {
            var entFile = new EntFileManager(orgClientId, orgClientSecret);
            string returnString = entFile.GetFileInfo("test", EntFileManager.NetType.Default, false);
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }

        [TestMethod()]
        public void CreateFolderTest()
        {
            var entFile = new EntFileManager(orgClientId, orgClientSecret);
            string returnString = entFile.CreateFolder("test", "Brandon");
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }

        [TestMethod()]
        public void CreateFileTest()
        {
            var entFile = new EntFileManager(orgClientId, orgClientSecret);
            string returnString = entFile.CreateFile("testqqq.txt", "Brandon", "D:\\test.txt");
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
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
            string returnString = entFile.Del("test", "Brandon");
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }

        [TestMethod()]
        public void MoveTest()
        {
            var entFile = new EntFileManager(orgClientId, orgClientSecret);
            string returnString = entFile.Move("qq.jpg", "test/qq.jpg", "Brandon");
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }

        [TestMethod()]
        public void LinkTest()
        {
            var entFile = new EntFileManager(orgClientId, orgClientSecret);
            string returnString = entFile.Link("qq.jpg", 0, EntFileManager.AuthType.Default, null);
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }

        [TestMethod()]
        public void SendMsgTest()
        {
            var entFile = new EntFileManager(orgClientId, orgClientSecret);
            string returnString = entFile.SendMsg("msgTest", "msg", "", "", "Brandon");
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }

        [TestMethod()]
        public void LinksTest()
        {
            var entFile = new EntFileManager(orgClientId, orgClientSecret);
            string returnString = entFile.Links(true);
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }

        [TestMethod()]
        public void CreateFileByUrlTest()
        {
            var entFile = new EntFileManager(orgClientId, orgClientSecret);
            string returnString = entFile.CreateFileByUrl("1q.jpg", 0, "Brandon", true,
                "http://www.sinaimg.cn/dy/slidenews/1_img/2015_27/2841_589214_521618.jpg");
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }

        [TestMethod()]
        public void GetUploadServersTest()
        {
            var entFile = new EntFileManager(orgClientId, orgClientSecret);
            string returnString = entFile.GetUploadServers();
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }

        [TestMethod()]
        public void GetServerSiteTest()
        {
            var entFile = new EntFileManager(orgClientId, orgClientSecret);
            string returnString = entFile.GetServerSite("");
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }

        [TestMethod()]
        public void HistoryTest()
        {
            var entFile = new EntFileManager(orgClientId, orgClientSecret);
            string returnString = entFile.History("test", 0, 100);
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }

        [TestMethod()]
        public void SearchTest()
        {
            var entFile = new EntFileManager(orgClientId, orgClientSecret);
            string returnString = entFile.Search("test", "", 0, 100);
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }

        [TestMethod()]
        public void GetPermissionTest()
        {
            var entFile = new EntFileManager(orgClientId, orgClientSecret);
            string returnString = entFile.GetPermission("test", 4);
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }

        [TestMethod()]
        public void SetPermissionTest()
        {
            var entFile = new EntFileManager(orgClientId, orgClientSecret);
            string returnString = entFile.SetPermission("test", 4, EntFileManager.FilePermissions.FileDelete,
                EntFileManager.FilePermissions.FilePreview);
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }

        [TestMethod()]
        public void AddTagTest()
        {
            var entFile = new EntFileManager(orgClientId, orgClientSecret);
            string returnString = entFile.AddTag("test", new[] { "test", "testTag" });
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }

        [TestMethod()]
        public void DelTagTest()
        {
            var entFile = new EntFileManager(orgClientId, orgClientSecret);
            string returnString = entFile.DelTag("test", new[] { "test", "testTag" });
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }
    }
}