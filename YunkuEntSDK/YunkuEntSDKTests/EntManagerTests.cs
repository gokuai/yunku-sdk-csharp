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
    public class EntManagerTests
    {
        public const string ClientId = "";
        public const string ClientSecret = "";

        [TestMethod()]
        public void GetMembersTest()
        {
            var ent = new EntManager(ClientId, ClientSecret);
            string returnString = ent.GetMembers(0, 99);
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }

        [TestMethod()]
        public void GetMemberByIdTest()
        {
            var ent = new EntManager(ClientId, ClientSecret);
            string returnString = ent.GetMemberById(4);
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }

        [TestMethod()]
        public void GetMemberByOutIdTest()
        {
            var ent = new EntManager(ClientId, ClientSecret);
            string returnString = ent.GetMemberByOutId("$:LWCP_v1:$ypc3i0Op0Tn0Ge2GvyShWA==");
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }

        [TestMethod()]
        public void GetMemberByAccountTest()
        {
            var ent = new EntManager(ClientId, ClientSecret);
            string returnString = ent.GetMemberByAccount("6905656124312207");
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }

        [TestMethod()]
        public void GetGroupsTest()
        {
            var ent = new EntManager(ClientId, ClientSecret);
            string returnString = ent.GetGroups();
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }

        [TestMethod()]
        public void GetRolesTest()
        {
            var ent = new EntManager(ClientId, ClientSecret);
            string returnString = ent.GetRoles();
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }

        [TestMethod()]
        public void GetMemberFileLinkTest()
        {
            var ent = new EntManager(ClientId, ClientSecret);
            string returnString = ent.GetMemberFileLink(4, true);
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }

        [TestMethod()]
        public void AddSyncMemberTest()
        {
            var ent = new EntManager(ClientId, ClientSecret);
            string returnString = ent.AddSyncMember("MemberTest1", "Member1", "Member1", "", "", "");
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }

        [TestMethod()]
        public void SetSyncMemberStateTest()
        {
            var ent = new EntManager(ClientId, ClientSecret);
            string returnString = ent.SetSyncMemberState("", true);
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }

        [TestMethod()]
        public void DelSyncMemberTest()
        {
            var ent = new EntManager(ClientId, ClientSecret);
            string returnString = ent.DelSyncMember(new[] { "MemberTest", "MemberTest1", "MemberTest2" });
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }

        [TestMethod()]
        public void AddSyncGroupTest()
        {
            var ent = new EntManager(ClientId, ClientSecret);
            string returnString = ent.AddSyncGroup("ParentGroup", "ParentGroup", "");
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }

        [TestMethod()]
        public void DelSyncGroupTest()
        {
            var ent = new EntManager(ClientId, ClientSecret);
            string returnString = ent.DelSyncGroup(new[] { "ParentGroup", "GroupTest" });
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }

        [TestMethod()]
        public void AddSyncGroupMemberTest()
        {
            var ent = new EntManager(ClientId, ClientSecret);
            string returnString = ent.AddSyncGroupMember("GroupTest", new[] { "MemberTest1" });
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }

        [TestMethod()]
        public void DelSyncGroupMemberTest()
        {
            var ent = new EntManager(ClientId, ClientSecret);
            string returnString = ent.DelSyncGroupMember("ParentGroup", new[] { "MemberTest2", "MemberTest3" });
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }

        [TestMethod()]
        public void GetGroupMembersTest()
        {
            var ent = new EntManager(ClientId, ClientSecret);
            string returnString = ent.GetGroupMembers(1317448, 0, 3, true);
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }

        [TestMethod()]
        public void DelSyncMemberGroupTest()
        {
            var ent = new EntManager(ClientId, ClientSecret);
            string returnString = ent.DelSyncMemberGroup(new[] { "MemberTest2" });
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }

        [TestMethod()]
        public void AddSyncAdminTest()
        {
            var ent = new EntManager(ClientId, ClientSecret);
            string returnString = ent.AddSyncAdmin("$:LWCP_v1:$ypc3i0Op0Tn0Ge2GvyShWA==", "", false);
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }

    }
}