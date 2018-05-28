using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            ReturnResult result = ent.GetMembers(0, 99);
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void GetMemberByIdTest()
        {
            var ent = new EntManager(ClientId, ClientSecret);
            ReturnResult result = ent.GetMemberById(4);
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void GetMemberByOutIdTest()
        {
            var ent = new EntManager(ClientId, ClientSecret);
            ReturnResult result = ent.GetMemberByOutId("$:LWCP_v1:$ypc3i0Op0Tn0Ge2GvyShWA==");
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void GetMemberByAccountTest()
        {
            var ent = new EntManager(ClientId, ClientSecret);
            ReturnResult result = ent.GetMemberByAccount("6905656124312207");
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void GetGroupsTest()
        {
            var ent = new EntManager(ClientId, ClientSecret);
            ReturnResult result = ent.GetGroups();
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void GetRolesTest()
        {
            var ent = new EntManager(ClientId, ClientSecret);
            ReturnResult result = ent.GetRoles();
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void AddSyncMemberTest()
        {
            var ent = new EntManager(ClientId, ClientSecret);
            ReturnResult result = ent.AddSyncMember("MemberTest1", "Member1", "Member1", "", "", "", true);
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void SetSyncMemberStateTest()
        {
            var ent = new EntManager(ClientId, ClientSecret);
            ReturnResult result = ent.SetSyncMemberState("", true);
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void DelSyncMemberTest()
        {
            var ent = new EntManager(ClientId, ClientSecret);
            ReturnResult result = ent.DelSyncMember(new[] { "MemberTest", "MemberTest1", "MemberTest2" });
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void AddSyncGroupTest()
        {
            var ent = new EntManager(ClientId, ClientSecret);
            ReturnResult result = ent.AddSyncGroup("ParentGroup", "ParentGroup", "");
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void DelSyncGroupTest()
        {
            var ent = new EntManager(ClientId, ClientSecret);
            ReturnResult result = ent.DelSyncGroup(new[] { "ParentGroup", "GroupTest" });
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void AddSyncGroupMemberTest()
        {
            var ent = new EntManager(ClientId, ClientSecret);
            ReturnResult result = ent.AddSyncGroupMember("GroupTest", new[] { "MemberTest1" });
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void DelSyncGroupMemberTest()
        {
            var ent = new EntManager(ClientId, ClientSecret);
            ReturnResult result = ent.DelSyncGroupMember("ParentGroup", new[] { "MemberTest2", "MemberTest3" });
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void GetGroupMembersTest()
        {
            var ent = new EntManager(ClientId, ClientSecret);
            ReturnResult result = ent.GetGroupMembers(1317448, 0, 3, true);
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void DelSyncMemberGroupTest()
        {
            var ent = new EntManager(ClientId, ClientSecret);
            ReturnResult result = ent.DelSyncMemberGroup(new[] { "MemberTest2" });
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void AddSyncAdminTest()
        {
            var ent = new EntManager(ClientId, ClientSecret);
            ReturnResult result = ent.AddSyncAdmin("$:LWCP_v1:$ypc3i0Op0Tn0Ge2GvyShWA==", "", false);
            Assert.AreEqual(200, result.Code);
        }

    }
}