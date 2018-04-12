using Microsoft.VisualStudio.TestTools.UnitTesting;
using YunkuEntSDK.compat.v2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YunkuEntSDK.Data;

namespace YunkuEntSDK.compat.v2.Tests
{
    [TestClass()]
    public class EntLibManagerTests
    {

        public const string admin = "";
        public const string password = "";
        public const string clientId = "";
        public const string clientSecret = "";

        [TestMethod()]
        public void CreateTest()
        {
            var entLib = new EntLibManager(clientId, clientSecret);
            entLib.AccessToken(admin, password);
            ReturnResult result = entLib.Create("vitali", "1073741824", "destroy", "test lib");
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void GetLibListTest()
        {
            var entLib = new EntLibManager(clientId, clientSecret);
            entLib.AccessToken(admin, password);
            ReturnResult result = entLib.GetLibList();
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void BindTest()
        {
            var entLib = new EntLibManager(clientId, clientSecret);
            entLib.AccessToken(admin, password);
            ReturnResult result = entLib.Bind(1262696, "YunkuJavaSDKDemo", null);
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void UnBindTest()
        {
            var entLib = new EntLibManager(clientId, clientSecret);
            entLib.AccessToken(admin, password);
            ReturnResult result = entLib.UnBind("AxT0EUEjtYc8za41xXl1dKFJ40");
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void GetMembersTest()
        {
            var entLib = new EntLibManager(clientId, clientSecret);
            entLib.AccessToken(admin, password);
            ReturnResult result = entLib.GetMembers(0, 10, 1262679);
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void GetMemberTest()
        {
            var entLib = new EntLibManager(clientId, clientSecret);
            entLib.AccessToken(admin, password);
            ReturnResult result = entLib.GetMember(1262679, EntLibManager.MemberType.MemberId, new[] { "239931" });
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void AddMembersTest()
        {
            var entLib = new EntLibManager(clientId, clientSecret);
            entLib.AccessToken(admin, password);
            ReturnResult result = entLib.AddMembers(1262679, 13862, new int[] { 885371 });
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void SetMemberRoleTest()
        {
            var entLib = new EntLibManager(clientId, clientSecret);
            entLib.AccessToken(admin, password);
            ReturnResult result = entLib.SetMemberRole(1262679, 13862, new int[] { 885371 });
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void DelMemberTest()
        {
            var entLib = new EntLibManager(clientId, clientSecret);
            entLib.AccessToken(admin, password);
            ReturnResult result = entLib.DelMember(1262679, new int[] { 885371 });
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void GetGroupsTest()
        {
            var entLib = new EntLibManager(clientId, clientSecret);
            entLib.AccessToken(admin, password);
            ReturnResult result = entLib.GetGroups(1262679);
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void AddGroupTest()
        {
            var entLib = new EntLibManager(clientId, clientSecret);
            entLib.AccessToken(admin, password);
            ReturnResult result = entLib.AddGroup(1262679, 154837, 13862);
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void DelGroupTest()
        {
            var entLib = new EntLibManager(clientId, clientSecret);
            entLib.AccessToken(admin, password);
            ReturnResult result = entLib.DelGroup(1262679, 154837);
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void SetGroupRoleTest()
        {
            var entLib = new EntLibManager(clientId, clientSecret);
            entLib.AccessToken(admin, password);
            ReturnResult result = entLib.SetGroupRole(1262679, 154837, 13862);
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void DestroyTest()
        {
            var entLib = new EntLibManager(clientId, clientSecret);
            entLib.AccessToken(admin, password);
            ReturnResult result = entLib.Destroy("AxT0EUEjtYc8za41xXl1dKFJ40");
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void SetTest()
        {
            var entLib = new EntLibManager(clientId, clientSecret);
            entLib.AccessToken(admin, password);
            ReturnResult result = entLib.Set(1262696, "ttt", "1073741824", "");
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void GetInfoTest()
        {
            var entLib = new EntLibManager(clientId, clientSecret);
            entLib.AccessToken(admin, password);
            ReturnResult result = entLib.GetInfo(1262679);
            Assert.AreEqual(200, result.Code);
        }
    }
}