using System.Collections.Generic;
using YunkuEntSDK.UtilClass;

namespace YunkuEntSDK
{
    public class EntEngine : HttpEngine
    {
        private const string Log_Tag = "EntEngine";
        protected string _clientIdKey = "client_id";

        public EntEngine(string clientId, string secret) : base(clientId, secret)
        {
        }

        protected internal class RequestHelper : HttpEngine.RequestHelper
        {
            public RequestHelper(EntEngine engine) : base(engine)
            {
            }
            
            protected override void SetCommonParams(bool disableSign)
            {
                if (disableSign)
                {
                    return;
                }
                if (this._params == null)
                {
                    this._params = new Dictionary<string, string>();
                }
                this._params.Add(((EntEngine)this._engine)._clientIdKey, this._engine.GetClientId());
                this._params.Add("dateline", Util.GetUnixDataline().ToString());
                this._params.Add("sign", this._engine.GenerateSign(this._params));
            }
        }
    }
}
