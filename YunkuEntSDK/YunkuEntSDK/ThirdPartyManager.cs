using System.Collections.Generic;
using YunkuEntSDK.Data;
using YunkuEntSDK.Net;

namespace YunkuEntSDK
{
    public class ThirdPartyManager : EntEngine
    {
        private static string UrlApiCreateEnt = Config.WebHost + "/1/thirdparty/create_ent";
        private static string UrlApiEntInfo = Config.WebHost + "/1/thirdparty/ent_info";
        private static string UrlApiOrder = Config.WebHost + "/1/thirdparty/order";
        private static string UrlApiGetSsO = Config.WebHost + "/1/thirdparty/get_sso_url";

        public const string Subscribe = "orderSubscribe";
        public const string Upgrade = "orderUpgrade";
        public const string Renew = "orderRenew";
        public const string Unsubscribe = "orderUnsubscribe";

        private readonly string _outId;

        public ThirdPartyManager(string clientId, string clientSecret, string outId) : base(clientId, clientSecret)
        {
            _outId = outId;
        }

        /// <summary>
        /// 开通企业
        /// </summary>
        /// <param name="entName"></param>
        /// <param name="contactName"></param>
        /// <param name="contactMobile"></param>
        /// <param name="contactEmail"></param>
        /// <param name="contactAddress"></param>
        /// <returns></returns>
        public ReturnResult CreateEnt(string entName, string contactName, string contactMobile, string contactEmail, string contactAddress)
        {
            return CreateEnt(null, entName, contactName, contactMobile, contactEmail, contactAddress);
        }

        /// <summary>
        /// 扩展参数
        /// </summary>
        /// <param name="dic"></param>
        /// <param name="entName"></param>
        /// <param name="contactName"></param>
        /// <param name="contactMobile"></param>
        /// <param name="contactEmail"></param>
        /// <param name="contactAddress"></param>
        /// <returns></returns>
        public ReturnResult CreateEnt(IDictionary<string, string> dic, string entName, string contactName, string contactMobile, string contactEmail, string contactAddress)
        {
            string url = UrlApiCreateEnt;
            var parameter = new Dictionary<string, string>();
            parameter.Add("out_id", _outId);
            parameter.Add("ent_name", entName);
            parameter.Add("contact_name", contactName);
            parameter.Add("contact_mobile", contactMobile);
            parameter.Add("contact_email", contactEmail);
            parameter.Add("contact_address", contactAddress);
            if (dic != null)
            {
                foreach (var dics in dic)
                {
                    parameter.Add(dics.Key, dics.Value);
                }
            }
            return new RequestHelper(this).SetParams(parameter).SetUrl(url).SetMethod(RequestType.POST).ExecuteSync();

        }

        /// <summary>
        /// 获取企业信息
        /// </summary>
        /// <returns></returns>
        public ReturnResult GetEntInfo()
        {
            string url = UrlApiEntInfo;
            var parameter = new Dictionary<string, string>();
            parameter.Add("out_id", _outId);
            return new RequestHelper(this).SetParams(parameter).SetUrl(url).SetMethod(RequestType.POST).ExecuteSync();
        }

        /// <summary>
        /// 购买
        /// </summary>
        /// <param name="memberCount"></param>
        /// <param name="space"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public ReturnResult OrderSubscribe(int memberCount, int space, int month)
        {
            return Order(Subscribe, memberCount, space, month);
        }

        /// <summary>
        /// 升级
        /// </summary>
        /// <param name="memberCount"></param>
        /// <param name="space"></param>
        /// <returns></returns>
        public ReturnResult OrderUpgrade(int memberCount, int space)
        {
            return Order(Upgrade, memberCount, space, 0);
        }

        /// <summary>
        /// 续费
        /// </summary>
        /// <param name="month"></param>
        /// <returns></returns>
        public ReturnResult OrderRenew(int month)
        {
            return Order(Renew, -1, 0, month);
        }

        /// <summary>
        /// 退订
        /// </summary>
        /// <returns></returns>
        public ReturnResult OrderUnsubscribe()
        {
            return Order(Unsubscribe, -1, 0, 0);
        }

        /// <summary>
        /// 订单处理
        /// </summary>
        /// <param name="type"></param>
        /// <param name="memberCount"></param>
        /// <param name="space"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public ReturnResult Order(string type, int memberCount, int space, int month)
        {
            string url = UrlApiOrder;
            var parameter = new Dictionary<string, string>();
            parameter.Add("out_id", _outId);
            if (!string.IsNullOrEmpty(type))
            {
                parameter.Add("type", type);
                switch (type)
                {
                    case Subscribe:
                        parameter.Add("member_count", memberCount.ToString());
                        parameter.Add("space", space.ToString());
                        parameter.Add("month", month.ToString());
                        break;
                    case Upgrade:
                        parameter.Add("member_count", memberCount.ToString());
                        parameter.Add("space", space.ToString());
                        break;
                    case Renew:
                        parameter.Add("month", month.ToString());
                        break;
                }
            }
            return new RequestHelper(this).SetParams(parameter).SetUrl(url).SetMethod(RequestType.POST).ExecuteSync();
        }

        /// <summary>
        /// 获取单点登录地址
        /// </summary>
        /// <param name="ticket"></param>
        /// <returns></returns>
        public ReturnResult GetSsoUrl(string ticket)
        {
            string url = UrlApiGetSsO;
            var parameter = new Dictionary<string, string>();
            parameter.Add("out_id", _outId);
            parameter.Add("ticket", ticket);
            return new RequestHelper(this).SetParams(parameter).SetUrl(url).SetMethod(RequestType.POST).ExecuteSync();
        }
    }
}
