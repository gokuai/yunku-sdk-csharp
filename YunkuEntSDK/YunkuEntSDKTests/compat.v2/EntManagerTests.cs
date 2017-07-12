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
            string returnString = ent.GetRoles();
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }

        [TestMethod()]
        public void GetMembersTest()
        {
            var ent = new EntManager(clientId, clientSecret);
            ent.AccessToken(admin, password);
            string returnString = ent.GetMembers(0, 99);
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }

        [TestMethod()]
        public void GetMemberByIdTest()
        {
            var ent = new EntManager(clientId, clientSecret);
            ent.AccessToken(admin, password);
            string returnString = ent.GetMemberById(504482);
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }

        [TestMethod()]
        public void GetMemberByOutIdTest()
        {
            var ent = new EntManager(clientId, clientSecret);
            ent.AccessToken(admin, password);
            string returnString = ent.GetMemberByOutId("ac1d8e1f-6d67-4143-8494-4c864c5f3d31");
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }

        [TestMethod()]
        public void GetMemberByAccountTest()
        {
            var ent = new EntManager(clientId, clientSecret);
            ent.AccessToken(admin, password);
            string returnString = ent.GetMemberByAccount("111");
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }

        [TestMethod()]
        public void GetGroupsTest()
        {
            var ent = new EntManager(clientId, clientSecret);
            ent.AccessToken(admin, password);
            string returnString = ent.GetGroups();
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }

        [TestMethod()]
        public void GetMemberFileLinkTest()
        {
            var ent = new EntManager(clientId, clientSecret);
            ent.AccessToken(admin, password);
            string returnString = ent.GetMemberFileLink(504482, true);
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }

        [TestMethod()]
        public void AddSyncMemberTest()
        {
            var ent = new EntManager(clientId, clientSecret);
            ent.AccessToken(admin, password);
            string returnString = ent.AddSyncMember("MemberTest1", "Member1", "Member1", "1234", "111", "111");
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }

        [TestMethod()]
        public void SetSyncMemberStateTest()
        {
            var ent = new EntManager(clientId, clientSecret);
            ent.AccessToken(admin, password);
            string returnString = ent.SetSyncMemberState("ac1d8e1f-6d67-4143-8494-4c864c5f3d31", true);
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }

        [TestMethod()]
        public void DelSyncMemberTest()
        {
            var ent = new EntManager(clientId, clientSecret);
            ent.AccessToken(admin, password);
            string returnString = ent.DelSyncMember(new[] { "MemberTest1" });
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }

        [TestMethod()]
        public void AddSyncGroupTest()
        {
            var ent = new EntManager(clientId, clientSecret);
            ent.AccessToken(admin, password);
            string returnString = ent.AddSyncGroup("ParentGroup", "ParentGroup", "");
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }

        [TestMethod()]
        public void DelSyncGroupTest()
        {
            var ent = new EntManager(clientId, clientSecret);
            ent.AccessToken(admin, password);
            string returnString = ent.DelSyncGroup(new[] { "ParentGroup" });
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }

        [TestMethod()]
        public void AddSyncGroupMemberTest()
        {
            var ent = new EntManager(clientId, clientSecret);
            ent.AccessToken(admin, password);
            string returnString = ent.AddSyncGroupMember("GroupTest", new[] { "MemberTest1" });
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }

        [TestMethod()]
        public void DelSyncGroupMemberTest()
        {
            var ent = new EntManager(clientId, clientSecret);
            ent.AccessToken(admin, password);
            string returnString = ent.DelSyncGroupMember("GroupTest", new[] { "MemberTest1" });
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }

        [TestMethod()]
        public void GetGroupMembersTest()
        {
            var ent = new EntManager(clientId, clientSecret);
            ent.AccessToken(admin, password);
            string returnString = ent.GetGroupMembers(154837, 0, 3, true);
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }

        [TestMethod()]
        public void DelSyncMemberGroupTest()
        {
            var ent = new EntManager(clientId, clientSecret);
            ent.AccessToken(admin, password);
            string returnString = ent.DelSyncMemberGroup(new[] { "MemberTest2" });
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }
    }
}