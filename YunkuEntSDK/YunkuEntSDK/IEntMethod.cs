using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YunkuEntSDK
{
    interface IEntMethod
    {
        string GetEntRoles();

        string GetEntMembers(int start, int size);

        string GetEntGroups();

        string SyncMembers(string structStr);

    }
}
