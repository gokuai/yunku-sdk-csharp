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
    public class EntManagerTests
    {

        public const string admin = "";
        public const string password = "";
        public const string clientId = "";
        public const string clientSecret = "";

        [TestMethod()]
        public void GetRolesTest()
        {
            var ent = new EntManager(clientId, clientSecret);
            ent.AccessToken(admin, password);
            ReturnResult result = ent.GetRoles();
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void GetMembersTest()
        {
            var ent = new EntManager(clientId, clientSecret);
            ent.AccessToken(admin, password);
            ReturnResult result = ent.GetMembers(0, 99);
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void GetMemberByIdTest()
        {
            var ent = new EntManager(clientId, clientSecret);
            ent.AccessToken(admin, password);
            ReturnResult result = ent.GetMemberById(504482);
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void GetMemberByOutIdTest()
        {
            var ent = new EntManager(clientId, clientSecret);
            ent.AccessToken(admin, password);
            ReturnResult result = ent.GetMemberByOutId("ac1d8e1f-6d67-4143-8494-4c864c5f3d31");
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void GetMemberByAccountTest()
        {
            var ent = new EntManager(clientId, clientSecret);
            ent.AccessToken(admin, password);
            ReturnResult result = ent.GetMemberByAccount("111");
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void GetGroupsTest()
        {
            var ent = new EntManager(clientId, clientSecret);
            ent.AccessToken(admin, password);
            ReturnResult result = ent.GetGroups();
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void GetMemberFileLinkTest()
        {
            var ent = new EntManager(clientId, clientSecret);
            ent.AccessToken(admin, password);
            ReturnResult result = ent.GetMemberFileLink(504482, true);
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void AddSyncMemberTest()
        {
            var ent = new EntManager(clientId, clientSecret);
            ent.AccessToken(admin, password);
            ReturnResult result = ent.AddSyncMember("MemberTest1", "Member1", "Member1", "1234", "111", "111");
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void SetSyncMemberStateTest()
        {
            var ent = new EntManager(clientId, clientSecret);
            ent.AccessToken(admin, password);
            ReturnResult result = ent.SetSyncMemberState("ac1d8e1f-6d67-4143-8494-4c864c5f3d31", true);
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void DelSyncMemberTest()
        {
            var ent = new EntManager(clientId, clientSecret);
            ent.AccessToken(admin, password);
            ReturnResult result = ent.DelSyncMember(new[] { "MemberTest1" });
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void AddSyncGroupTest()
        {
            var ent = new EntManager(clientId, clientSecret);
            ent.AccessToken(admin, password);
            ReturnResult result = ent.AddSyncGroup("ParentGroup", "ParentGroup", "");
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void DelSyncGroupTest()
        {
            var ent = new EntManager(clientId, clientSecret);
            ent.AccessToken(admin, password);
            ReturnResult result = ent.DelSyncGroup(new[] { "ParentGroup" });
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void AddSyncGroupMemberTest()
        {
            var ent = new EntManager(clientId, clientSecret);
            ent.AccessToken(admin, password);
            ReturnResult result = ent.AddSyncGroupMember("GroupTest", new[] { "MemberTest1" });
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void DelSyncGroupMemberTest()
        {
            var ent = new EntManager(clientId, clientSecret);
            ent.AccessToken(admin, password);
            ReturnResult result = ent.DelSyncGroupMember("GroupTest", new[] { "MemberTest1" });
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void GetGroupMembersTest()
        {
            var ent = new EntManager(clientId, clientSecret);
            ent.AccessToken(admin, password);
            ReturnResult result = ent.GetGroupMembers(154837, 0, 3, true);
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void DelSyncMemberGroupTest()
        {
            var ent = new EntManager(clientId, clientSecret);
            ent.AccessToken(admin, password);
            ReturnResult result = ent.DelSyncMemberGroup(new[] { "MemberTest2" });
            Assert.AreEqual(200, result.Code);
        }
    }
}