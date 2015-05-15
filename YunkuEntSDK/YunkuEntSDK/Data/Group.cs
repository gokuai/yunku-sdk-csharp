using System;
using System.Collections.Generic;

namespace YunkuEntSDK.Data
{
    public class Group
    {
        public  List<Object> children;
        public int gid;
        public String name;

        public Group(int gid, string name, List<object> children)
        {
            this.gid = gid;
            this.name = name;
            this.children = children;
        }

        public void AddChildren(Object o)
        {
            this.children.Add(o);
        }
    }
}