using Microsoft.VisualStudio.TestTools.UnitTesting;
using YunkuEntSDK.Data;

namespace YunkuEntSDK.Tests
{
    [TestClass()]
    public class EntLibManagerTests
    {

        public const string ClientId = "";
        public const string ClientSecret = "";

        [TestMethod()]
        public void CreateTest()
        {
            EntLibManager entLib = new EntLibManager(ClientId, ClientSecret);
            ReturnResult result = entLib.Create("lib", "1073741824", "destroy", "test lib");
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void GetLibListTest()
        {
            EntLibManager entLib = new EntLibManager(ClientId, ClientSecret);
            ReturnResult result = entLib.GetLibList();
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void BindTest()
        {
            EntLibManager entLib = new EntLibManager(ClientId, ClientSecret);
            ReturnResult result = entLib.Bind(1317448, "");
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void UnBindTest()
        {
            EntLibManager entLib = new EntLibManager(ClientId, ClientSecret);
            ReturnResult result = entLib.UnBind("l56SYNzK6N9z2ZDMhYqC1Oo");
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void GetMembersTest()
        {
            EntLibManager entLib = new EntLibManager(ClientId, ClientSecret);
            ReturnResult result = entLib.GetMembers(0, 20, 1317448);
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void GetMemberTest()
        {
            EntLibManager entLib = new EntLibManager(ClientId, ClientSecret);
            ReturnResult result = entLib.GetMember(1317448, EntLibManager.MemberType.Account, new[] { "qwdqwdq1" });
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void AddMembersTest()
        {
            EntLibManager entLib = new EntLibManager(ClientId, ClientSecret);
            ReturnResult result = entLib.AddMembers(1317448, 3208, new int[] { 216144 });
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void SetMemberRoleTest()
        {
            EntLibManager entLib = new EntLibManager(ClientId, ClientSecret);
            ReturnResult result = entLib.SetMemberRole(1317448, 3208, new int[] { 216144 });
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void DelMemberTest()
        {
            EntLibManager entLib = new EntLibManager(ClientId, ClientSecret);
            ReturnResult result = entLib.DelMember(1317448, new int[] { 216144 });
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void GetGroupsTest()
        {
            EntLibManager entLib = new EntLibManager(ClientId, ClientSecret);
            ReturnResult result = entLib.GetGroups(1317448);
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void AddGroupTest()
        {
            EntLibManager entLib = new EntLibManager(ClientId, ClientSecret);
            ReturnResult result = entLib.AddGroup(1317448, 4448, 3208);
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void DelGroupTest()
        {
            EntLibManager entLib = new EntLibManager(ClientId, ClientSecret);
            ReturnResult result = entLib.DelGroup(1317448, 4448);
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void SetGroupRoleTest()
        {
            EntLibManager entLib = new EntLibManager(ClientId, ClientSecret);
            ReturnResult result = entLib.SetGroupRole(1317448, 4448, 1194);
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void DestroyTest()
        {
            EntLibManager entLib = new EntLibManager(ClientId, ClientSecret);
            ReturnResult result = entLib.Destroy(1317448);
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void SetTest()
        {
            EntLibManager entLib = new EntLibManager(ClientId, ClientSecret);
            ReturnResult result = entLib.Set(1258748, "ttt", "1073741824", "");
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void GetInfoTest()
        {
            EntLibManager entLib = new EntLibManager(ClientId, ClientSecret);
            ReturnResult result = entLib.GetInfo(1317448);
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void GetLogTest()
        {
            EntLibManager entLib = new EntLibManager(ClientId, ClientSecret);
            ReturnResult result = entLib.GetLog(1317448, null, 0, 100);
            Assert.AreEqual(200, result.Code);
        }
    }
}