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
            ReturnResult result = entFile.GetFileList();
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void GetUpdateListTest()
        {
            EntFileManager entFile = new EntFileManager(orgClientId, orgClientSecret);
            ReturnResult result = entFile.GetUpdateList(false, 0);
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void GetUpdateCountTest()
        {
            long now = UnixTimestampConverter.ConvertLocalToTimestamp(DateTime.Now);
            long beigin = UnixTimestampConverter.ConvertLocalToTimestamp(DateTime.Now.AddDays(-1));

            EntFileManager entFile = new EntFileManager(orgClientId, orgClientSecret);
            ReturnResult result = entFile.GetUpdateCount(beigin, now, true);
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void GetFileInfoTest()
        {
            EntFileManager entFile = new EntFileManager(orgClientId, orgClientSecret);
            ReturnResult result = entFile.GetFileInfoByFullpath("test", EntFileManager.NetType.Default, false);
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void CreateFolderTest()
        {
            EntFileManager entFile = new EntFileManager(orgClientId, orgClientSecret);
            ReturnResult result = entFile.CreateFolder("testV", "Vitali");
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void CreateFileTest()
        {
            EntFileManager entFile = new EntFileManager(orgClientId, orgClientSecret);
            ReturnResult result = entFile.CreateFile("testVV.txt", "Brandon", "D:\\test.txt");
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void UploadByBlockTest()
        {
            EntFileManager entFile = new EntFileManager(orgClientId, orgClientSecret);
            entFile.UploadByBlockAsync("testABC.txt", "Brandon", 0,
                 "D:\\test.txt", true, UploadCompeleted, ProgressChanged);
        }

        private void ProgressChanged(object sender, ProgressEventArgs e)
        {
            LogPrint.Print(" onProgress:" + e.ProgressPercent);
        }

        private void UploadCompeleted(object sender, CompletedEventArgs e)
        {
            Assert.IsFalse(e.IsError);
        }

        [TestMethod()]
        public void DelTest()
        {
            EntFileManager entFile = new EntFileManager(orgClientId, orgClientSecret);
            ReturnResult result = entFile.Del("test", "Brandon", true);
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void MoveTest()
        {
            EntFileManager entFile = new EntFileManager(orgClientId, orgClientSecret);
            ReturnResult result = entFile.Move("qq.jpg", "test/qq.jpg", "Brandon");
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void LinkTest()
        {
            EntFileManager entFile = new EntFileManager(orgClientId, orgClientSecret);
            ReturnResult result = entFile.Link("qq.jpg", 0, EntFileManager.AuthType.Default, null);
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void LinksTest()
        {
            EntFileManager entFile = new EntFileManager(orgClientId, orgClientSecret);
            ReturnResult result = entFile.Links(true);
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void CreateFileByUrlTest()
        {
            EntFileManager entFile = new EntFileManager(orgClientId, orgClientSecret);
            ReturnResult result = entFile.CreateFileByUrl("1q.jpg", 0, "Brandon", true,
                "http://www.sinaimg.cn/dy/slidenews/1_img/2015_27/2841_589214_521618.jpg");
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void GetUploadServersTest()
        {
            EntFileManager entFile = new EntFileManager(orgClientId, orgClientSecret);
            ReturnResult result = entFile.GetUploadServers();
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void GetServerSiteTest()
        {
            EntFileManager entFile = new EntFileManager(orgClientId, orgClientSecret);
            ReturnResult result = entFile.GetServerSite("");
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void CopyTest()
        {
            EntFileManager entFile = new EntFileManager(orgClientId, orgClientSecret);
            ReturnResult result = entFile.Copy("test/qq.jpg", "qq.jpg", "qp");
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void RecyleTest()
        {
            EntFileManager entFile = new EntFileManager(orgClientId, orgClientSecret);
            ReturnResult result = entFile.Recyle(0, 100);
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void RecoverTest()
        {
            EntFileManager entFile = new EntFileManager(orgClientId, orgClientSecret);
            ReturnResult result = entFile.Recover("test", "qp");
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void CompletelyDelFileTest()
        {
            EntFileManager entFile = new EntFileManager(orgClientId, orgClientSecret);
            ReturnResult result = entFile.CompletelyDelFile(new[] { "aaa.jpg" }, "qp");
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void HistoryTest()
        {
            EntFileManager entFile = new EntFileManager(orgClientId, orgClientSecret);
            ReturnResult result = entFile.History("test", 0, 100);
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void GetDownloadUrlByHashTest()
        {
            EntFileManager entFile = new EntFileManager(orgClientId, orgClientSecret);
            ReturnResult result = entFile.GetDownloadUrlByHash("5ef2b3b8449cf3440b8a3b1874da5e4236b06dd8",
                false, EntFileManager.NetType.Default);
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void GetDownloadUrlByFullPathTest()
        {
            EntFileManager entFile = new EntFileManager(orgClientId, orgClientSecret);
            ReturnResult result = entFile.GetDownloadUrlByFullpath("test/qq.jpg", false, EntFileManager.NetType.Default);
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void SearchTest()
        {
            EntFileManager entFile = new EntFileManager(orgClientId, orgClientSecret);
            ReturnResult result = entFile.Search("test", "", 0, 100, EntFileManager.ScopeType.FileName, EntFileManager.ScopeType.Tag);
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void PreviewUrlTest()
        {
            EntFileManager entFile = new EntFileManager(orgClientId, orgClientSecret);
            ReturnResult result = entFile.GetPreviewUrlByFullpath("test/qq.jpg", false, "", false);
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void GetPermissionTest()
        {
            EntFileManager entFile = new EntFileManager(orgClientId, orgClientSecret);
            ReturnResult result = entFile.GetPermission("test", 4);
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void SetPermissionTest()
        {
            EntFileManager entFile = new EntFileManager(orgClientId, orgClientSecret);
            ReturnResult result = entFile.SetPermission("test", 4, EntFileManager.FilePermissions.FileDelete,
                EntFileManager.FilePermissions.FilePreview);
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void AddTagTest()
        {
            EntFileManager entFile = new EntFileManager(orgClientId, orgClientSecret);
            ReturnResult result = entFile.AddTag("test", new[] { "test", "testTag" });
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void DelTagTest()
        {
            EntFileManager entFile = new EntFileManager(orgClientId, orgClientSecret);
            ReturnResult result = entFile.DelTag("test", new[] { "test", "testTag" });
            Assert.AreEqual(200, result.Code);
        }
    }
}