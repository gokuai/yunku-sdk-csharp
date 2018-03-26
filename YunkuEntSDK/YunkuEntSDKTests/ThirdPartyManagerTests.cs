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
            String returnString = thirdParty.CreateEnt("yunku3", "yunku3", "", "", "");
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }

        [TestMethod()]
        public void GetEntInfoTest()
        {
            ThirdPartyManager thirdParty = new ThirdPartyManager(ClientId, ClientSecret, OutId);
            String returnString = thirdParty.GetEntInfo();
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }

        [TestMethod()]
        public void OrderSubscribeTest()
        {
            ThirdPartyManager thirdParty = new ThirdPartyManager(ClientId, ClientSecret, OutId);
            String returnString = thirdParty.OrderSubscribe(-1, 1, 12);
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }

        [TestMethod()]
        public void OrderUpgradeTest()
        {
            ThirdPartyManager thirdParty = new ThirdPartyManager(ClientId, ClientSecret, OutId);
            String returnString = thirdParty.OrderUpgrade(-1,1);
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }

        [TestMethod()]
        public void OrderRenewTest()
        {
            ThirdPartyManager thirdParty = new ThirdPartyManager(ClientId, ClientSecret, OutId);
            String returnString = thirdParty.OrderRenew(12);
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }

        [TestMethod()]
        public void OrderUnsubscribeTest()
        {
            ThirdPartyManager thirdParty = new ThirdPartyManager(ClientId, ClientSecret, OutId);
            String returnString = thirdParty.OrderUnsubscribe();
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }

        [TestMethod()]
        public void GetSsoUrlTest()
        {
            ThirdPartyManager thirdParty = new ThirdPartyManager(ClientId, ClientSecret, OutId);
            String returnString = thirdParty.GetSsoUrl("");
            ReturnResult resultString = ReturnResult.Create(returnString);
            Assert.AreEqual(200, resultString.Code);
        }
    }
}