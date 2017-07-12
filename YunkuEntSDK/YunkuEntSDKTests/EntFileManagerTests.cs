using Microsoft.VisualStudio.TestTools.UnitTesting;
using YunkuEntSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YunkuEntSDK.Data;
using YunkuEntSDK.UtilClass;
using YunkuEntSDK.Net;

namespace YunkuEntSDK.Tests
{
    [TestClass()]
    public class EntFileManagerTests
    {

        public const string orgClientId = "";
        public const string orgClientSecret = "";

        [TestMethod()]
        public void GetFileListTest()
        {
            EntFileManager entFile = new EntFileManager(orgClientId, orgClientSecret);
            String returnString = entFile.GetFileList();
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }

        [TestMethod()]
        public void GetUpdateListTest()
        {
            EntFileManager entFile = new EntFileManager(orgClientId, orgClientSecret);
            String returnString = entFile.GetUpdateList(false, 0);
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }

        [TestMethod()]
        public void GetUpdateCountTest()
        {
            long now = UnixTimestampConverter.ConvertLocalToTimestamp(DateTime.Now);
            long beigin = UnixTimestampConverter.ConvertLocalToTimestamp(DateTime.Now.AddDays(-1));

            EntFileManager entFile = new EntFileManager(orgClientId, orgClientSecret);
            String returnString = entFile.GetUpdateCount(beigin, now, true);
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }

        [TestMethod()]
        public void GetFileInfoTest()
        {
            EntFileManager entFile = new EntFileManager(orgClientId, orgClientSecret);
            String returnString = entFile.GetFileInfo("test", EntFileManager.NetType.Default, false);
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }

        [TestMethod()]
        public void CreateFolderTest()
        {
            EntFileManager entFile = new EntFileManager(orgClientId, orgClientSecret);
            String returnString = entFile.CreateFolder("test", "Vitali");
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }

        [TestMethod()]
        public void CreateFileTest()
        {
            EntFileManager entFile = new EntFileManager(orgClientId, orgClientSecret);
            String returnString = entFile.CreateFile("testqqq.txt", "Brandon", "D:\\test.txt");
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }

        [TestMethod()]
        public void UploadByBlockTest()
        {
            EntFileManager entFile = new EntFileManager(orgClientId, orgClientSecret);
            entFile.UploadByBlock("testUPp.txt", "Brandon", 0,
                 "D:\\test.txt", true, UploadCompeleted, ProgressChanged);
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
            EntFileManager entFile = new EntFileManager(orgClientId, orgClientSecret);
            String returnString = entFile.Del("test", "Brandon");
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }

        [TestMethod()]
        public void MoveTest()
        {
            EntFileManager entFile = new EntFileManager(orgClientId, orgClientSecret);
            String returnString = entFile.Move("qq.jpg", "test/qq.jpg", "Brandon");
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }

        [TestMethod()]
        public void LinkTest()
        {
            EntFileManager entFile = new EntFileManager(orgClientId, orgClientSecret);
            String returnString = entFile.Link("qq.jpg", 0, EntFileManager.AuthType.Default, null);
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }

        [TestMethod()]
        public void SendMsgTest()
        {
            EntFileManager entFile = new EntFileManager(orgClientId, orgClientSecret);
            String returnString = entFile.SendMsg("msgTest", "msg", "", "", "Brandon");
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }

        [TestMethod()]
        public void LinksTest()
        {
            EntFileManager entFile = new EntFileManager(orgClientId, orgClientSecret);
            String returnString = entFile.Links(true);
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }

        [TestMethod()]
        public void CreateFileByUrlTest()
        {
            EntFileManager entFile = new EntFileManager(orgClientId, orgClientSecret);
            String returnString = entFile.CreateFileByUrl("1q.jpg", 0, "Brandon", true,
                "http://www.sinaimg.cn/dy/slidenews/1_img/2015_27/2841_589214_521618.jpg");
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }

        [TestMethod()]
        public void GetUploadServersTest()
        {
            EntFileManager entFile = new EntFileManager(orgClientId, orgClientSecret);
            String returnString = entFile.GetUploadServers();
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }

        [TestMethod()]
        public void GetServerSiteTest()
        {
            EntFileManager entFile = new EntFileManager(orgClientId, orgClientSecret);
            String returnString = entFile.GetServerSite("");
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }

        [TestMethod()]
        public void CopyTest()
        {
            EntFileManager entFile = new EntFileManager(orgClientId, orgClientSecret);
            String returnString = entFile.Copy("test/qq.jpg", "qq.jpg", "qp");
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }

        [TestMethod()]
        public void RecyleTest()
        {
            EntFileManager entFile = new EntFileManager(orgClientId, orgClientSecret);
            String returnString = entFile.Recyle(0, 100);
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }

        [TestMethod()]
        public void RecoverTest()
        {
            EntFileManager entFile = new EntFileManager(orgClientId, orgClientSecret);
            String returnString = entFile.Recover("test", "qp");
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }

        [TestMethod()]
        public void CompletelyDelFileTest()
        {
            EntFileManager entFile = new EntFileManager(orgClientId, orgClientSecret);
            String returnString = entFile.CompletelyDelFile(new[] { "aaa.jpg" }, "qp");
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }

        [TestMethod()]
        public void HistoryTest()
        {
            EntFileManager entFile = new EntFileManager(orgClientId, orgClientSecret);
            String returnString = entFile.History("test", 0, 100);
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }

        [TestMethod()]
        public void GetDownloadUrlByHashTest()
        {
            EntFileManager entFile = new EntFileManager(orgClientId, orgClientSecret);
            String returnString = entFile.GetDownloadUrlByHash("5ef2b3b8449cf3440b8a3b1874da5e4236b06dd8",
                false, EntFileManager.NetType.Default);
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }

        [TestMethod()]
        public void GetDownloadUrlByFullPathTest()
        {
            EntFileManager entFile = new EntFileManager(orgClientId, orgClientSecret);
            String returnString = entFile.GetDownloadUrlByFullPath("test/qq.jpg", false, EntFileManager.NetType.Default);
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }

        [TestMethod()]
        public void SearchTest()
        {
            EntFileManager entFile = new EntFileManager(orgClientId, orgClientSecret);
            String returnString = entFile.Search("test", "", 0, 100, EntFileManager.ScopeType.FileName, EntFileManager.ScopeType.Tag);
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }

        [TestMethod()]
        public void PreviewUrlTest()
        {
            EntFileManager entFile = new EntFileManager(orgClientId, orgClientSecret);
            String returnString = entFile.PreviewUrl("test/qq.jpg", false, "");
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }

        [TestMethod()]
        public void GetPermissionTest()
        {
            EntFileManager entFile = new EntFileManager(orgClientId, orgClientSecret);
            String returnString = entFile.GetPermission("test", 4);
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }

        [TestMethod()]
        public void SetPermissionTest()
        {
            EntFileManager entFile = new EntFileManager(orgClientId, orgClientSecret);
            String returnString = entFile.SetPermission("test", 4, EntFileManager.FilePermissions.FileDelete,
                EntFileManager.FilePermissions.FilePreview);
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }

        [TestMethod()]
        public void AddTagTest()
        {
            EntFileManager entFile = new EntFileManager(orgClientId, orgClientSecret);
            String returnString = entFile.AddTag("test", new[] { "test", "testTag" });
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }

        [TestMethod()]
        public void DelTagTest()
        {
            EntFileManager entFile = new EntFileManager(orgClientId, orgClientSecret);
            String returnString = entFile.DelTag("test", new[] { "test", "testTag" });
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }
    }
}