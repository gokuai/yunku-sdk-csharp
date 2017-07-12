using Microsoft.VisualStudio.TestTools.UnitTesting;
using YunkuEntSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            String returnString = entLib.Create("lib", "1073741824", "destroy", "test lib");
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }

        [TestMethod()]
        public void GetLibListTest()
        {
            EntLibManager entLib = new EntLibManager(ClientId, ClientSecret);
            String returnString = entLib.GetLibList();
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }

        [TestMethod()]
        public void BindTest()
        {
            EntLibManager entLib = new EntLibManager(ClientId, ClientSecret);
            String returnString = entLib.Bind(1317448, "", "");
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }

        [TestMethod()]
        public void UnBindTest()
        {
            EntLibManager entLib = new EntLibManager(ClientId, ClientSecret);
            String returnString = entLib.UnBind("l56SYNzK6N9z2ZDMhYqC1Oo");
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }

        [TestMethod()]
        public void GetMembersTest()
        {
            EntLibManager entLib = new EntLibManager(ClientId, ClientSecret);
            String returnString = entLib.GetMembers(0, 20, 1317448);
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }

        [TestMethod()]
        public void GetMemberTest()
        {
            EntLibManager entLib = new EntLibManager(ClientId, ClientSecret);
            String returnString = entLib.GetMember(1317448, EntLibManager.MemberType.Account, new[] { "qwdqwdq1" });
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }

        [TestMethod()]
        public void AddMembersTest()
        {
            EntLibManager entLib = new EntLibManager(ClientId, ClientSecret);
            String returnString = entLib.AddMembers(1317448, 3208, new int[] { 216144 });
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }

        [TestMethod()]
        public void SetMemberRoleTest()
        {
            EntLibManager entLib = new EntLibManager(ClientId, ClientSecret);
            String returnString = entLib.SetMemberRole(1317448, 3208, new int[] { 216144 });
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }

        [TestMethod()]
        public void DelMemberTest()
        {
            EntLibManager entLib = new EntLibManager(ClientId, ClientSecret);
            String returnString = entLib.DelMember(1317448, new int[] { 216144 });
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }

        [TestMethod()]
        public void GetGroupsTest()
        {
            EntLibManager entLib = new EntLibManager(ClientId, ClientSecret);
            String returnString = entLib.GetGroups(1317448);
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }

        [TestMethod()]
        public void AddGroupTest()
        {
            EntLibManager entLib = new EntLibManager(ClientId, ClientSecret);
            String returnString = entLib.AddGroup(1317448, 4448, 3208);
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }

        [TestMethod()]
        public void DelGroupTest()
        {
            EntLibManager entLib = new EntLibManager(ClientId, ClientSecret);
            String returnString = entLib.DelGroup(1317448, 4448);
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }

        [TestMethod()]
        public void SetGroupRoleTest()
        {
            EntLibManager entLib = new EntLibManager(ClientId, ClientSecret);
            String returnString = entLib.SetGroupRole(1317448, 4448, 1194);
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }

        [TestMethod()]
        public void DestroyTest()
        {
            EntLibManager entLib = new EntLibManager(ClientId, ClientSecret);
            String returnString = entLib.Destroy("jvj4DYQFFPoV98to7wu4ZUQ");
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }

        [TestMethod()]
        public void SetTest()
        {
            EntLibManager entLib = new EntLibManager(ClientId, ClientSecret);
            String returnString = entLib.Set(1258748, "ttt", "1073741824", "");
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }

        [TestMethod()]
        public void GetInfoTest()
        {
            EntLibManager entLib = new EntLibManager(ClientId, ClientSecret);
            String returnString = entLib.GetInfo(1317448);
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }

        [TestMethod()]
        public void GetLogTest()
        {
            EntLibManager entLib = new EntLibManager(ClientId, ClientSecret);
            String returnString = entLib.GetLog(1317448, null, 0, 100);
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }
    }
}