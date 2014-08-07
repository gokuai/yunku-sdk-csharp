using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace YunkuEntSDK.UtilClass
{
    class LogPrint
    {
        private const string PRO_NAME = "GoKuai_EntSDK";

        public static void Print(string log)
        {
            if (ParentManager.LogPrint) {
                Debug.WriteLine(PRO_NAME + "GoKuai_EntSDK==>" + log);
            }
            
        }
        
    }
}
