using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YunkuEntSDK.Net;
using YunkuEntSDK.UtilClass;

namespace YunkuEntSDK
{
    public class ThirdPartyManager : HttpEngine
    {
        private const string UrlApiCreateEnt = HostConfig.OauthHost + "/1/thirdparty/create_ent";
        private const string UrlApiEntInfo = HostConfig.OauthHost + "/1/thirdparty/ent_info";
        private const string UrlApiOrder = HostConfig.OauthHost + "/1/thirdparty/order";
        private const string UrlApiGetToken = HostConfig.OauthHost + "/1/thirdparty/get_token";
        private const string UrlApiGetSsO = HostConfig.OauthHost + "/1/thirdparty/get_sso_url";

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
        public string CreateEnt(string entName, string contactName, string contactMobile, string contactEmail, string contactAddress)
        {
            var request = new HttpRequestSyn { RequestUrl = UrlApiCreateEnt };
            request.AppendParameter("client_id", "");
            request.AppendParameter("out_id", _outId);
            request.AppendParameter("dateline", Util.GetUnixDataline() + "");
            request.AppendParameter("ent_name", entName);
            request.AppendParameter("contact_name", entName);
            request.AppendParameter("contact_mobile", contactMobile);
            request.AppendParameter("contact_email", contactEmail);
            request.AppendParameter("contact_address", contactAddress);
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.Post;
            request.Request();
            return request.Result;
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
        public string CreateEnt( IDictionary<string, string> dic, string entName, string contactName, string contactMobile, string contactEmail, string contactAddress)
        {
            var request = new HttpRequestSyn { RequestUrl = UrlApiCreateEnt };
            request.AppendParameter("client_id", "");
            request.AppendParameter("out_id", _outId);
            request.AppendParameter("dateline", Util.GetUnixDataline() + "");
            request.AppendParameter("ent_name", entName);
            request.AppendParameter("contact_name", entName);
            request.AppendParameter("contact_mobile", contactMobile);
            request.AppendParameter("contact_email", contactEmail);
            request.AppendParameter("contact_address", contactAddress);
            if(dic != null)
            {
                // TODO
                foreach(var c in dic)
                {
                    request.AppendParameter(c.Key, c.Value);
                }
            }
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.Post;
            request.Request();
            return request.Result;
        }

        /// <summary>
        /// 获取企业信息
        /// </summary>
        /// <returns></returns>
        public string GetEntInfo()
        {
            var request = new HttpRequestSyn { RequestUrl = UrlApiEntInfo };
            request.AppendParameter("client_id", "");
            request.AppendParameter("out_id", _outId);
            request.AppendParameter("dateline", Util.GetUnixDataline() + "");
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.Post;
            request.Request();
            return request.Result;
        }

        /// <summary>
        /// 购买
        /// </summary>
        /// <param name="memberCount"></param>
        /// <param name="space"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public string OrderSubscribe(int memberCount, int space, int month)
        {
            return Order(Subscribe, memberCount, space, month);
        }

        /// <summary>
        /// 升级
        /// </summary>
        /// <param name="memberCount"></param>
        /// <param name="space"></param>
        /// <returns></returns>
        public string OrderUpgrade(int memberCount, int space)
        {
            return Order(Upgrade, memberCount, space, 0);
        }

        /// <summary>
        /// 续费
        /// </summary>
        /// <param name="month"></param>
        /// <returns></returns>
        public string OrderRenew(int month)
        {
            return Order(Renew, -1, 0, month);
        }

        /// <summary>
        /// 退订
        /// </summary>
        /// <returns></returns>
        public string OrderUnsubscribe()
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
        public string Order(string type, int memberCount, int space, int month)
        {
            var request = new HttpRequestSyn { RequestUrl = UrlApiOrder };
            request.AppendParameter("client_id", "");
            request.AppendParameter("out_id", _outId);
            if (!string.IsNullOrEmpty(type))
            {
                request.AppendParameter("type", type);
                switch (type)
                {
                    case Subscribe:
                        request.AppendParameter("member_count", memberCount + "");
                        request.AppendParameter("space", space + "");
                        request.AppendParameter("month", month + "");
                        break;
                    case Upgrade:
                        request.AppendParameter("member_count", memberCount + "");
                        request.AppendParameter("space", space + "");
                        break;
                    case Renew:
                        request.AppendParameter("month", month + "");
                        break;
                }
            }
            request.AppendParameter("dateline", Util.GetUnixDataline() + "");
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.Post;
            request.Request();
            return request.Result;
        }

        /// <summary>
        /// 获取企业token
        /// </summary>
        /// <returns></returns>
        public string GetEntToken()
        {
            var request = new HttpRequestSyn { RequestUrl = UrlApiGetToken };
            request.AppendParameter("client_id", "");
            request.AppendParameter("out_id", _outId);
            request.AppendParameter("dateline", Util.GetUnixDataline() + "");
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.Post;
            request.Request();
            return request.Result;
        }

        /// <summary>
        /// 获取单点登录地址
        /// </summary>
        /// <param name="ticket"></param>
        /// <returns></returns>
        public string GetSsoUrl(string ticket)
        {
            var request = new HttpRequestSyn { RequestUrl = UrlApiGetSsO };
            request.AppendParameter("client_id", "");
            request.AppendParameter("out_id", _outId);
            request.AppendParameter("ticket", ticket);
            request.AppendParameter("dateline", Util.GetUnixDataline() + "");
            request.AppendParameter("sign", GenerateSign(request.SortedParamter));
            request.RequestMethod = RequestType.Post;
            request.Request();
            return request.Result;
        }
    }
}
