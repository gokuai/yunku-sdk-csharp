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
    public class ThirdPartyManagerTests
    {

        public const string ClientId = "";
        public const string ClientSecret = "";
        public const string OutId = "";

        [TestMethod()]
        public void CreateEntTest()
        {
            ThirdPartyManager thirdParty = new ThirdPartyManager(ClientId, ClientSecret, OutId);
            ReturnResult result = thirdParty.CreateEnt("yunku3", "yunku3", "", "", "");
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void GetEntInfoTest()
        {
            ThirdPartyManager thirdParty = new ThirdPartyManager(ClientId, ClientSecret, OutId);
            ReturnResult result = thirdParty.GetEntInfo();
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void OrderSubscribeTest()
        {
            ThirdPartyManager thirdParty = new ThirdPartyManager(ClientId, ClientSecret, OutId);
            ReturnResult result = thirdParty.OrderSubscribe(-1, 1, 12);
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void OrderUpgradeTest()
        {
            ThirdPartyManager thirdParty = new ThirdPartyManager(ClientId, ClientSecret, OutId);
            ReturnResult result = thirdParty.OrderUpgrade(-1,1);
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void OrderRenewTest()
        {
            ThirdPartyManager thirdParty = new ThirdPartyManager(ClientId, ClientSecret, OutId);
            ReturnResult result = thirdParty.OrderRenew(12);
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void OrderUnsubscribeTest()
        {
            ThirdPartyManager thirdParty = new ThirdPartyManager(ClientId, ClientSecret, OutId);
            ReturnResult result = thirdParty.OrderUnsubscribe();
            Assert.AreEqual(200, result.Code);
        }

        [TestMethod()]
        public void GetSsoUrlTest()
        {
            ThirdPartyManager thirdParty = new ThirdPartyManager(ClientId, ClientSecret, OutId);
            ReturnResult result = thirdParty.GetSsoUrl("");
            Assert.AreEqual(200, result.Code);
        }
    }
}