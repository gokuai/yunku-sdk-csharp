using System;
using System.Collections.Generic;

namespace YunkuEntSDK.Data
{
    public class Group
    {
        public  List<Object> Children;
        public int GId;
        public String Name;

        public Group(int gid, String name, List<object> children)
        {
            GId = gid;
            Name = name;
            Children = children;
        }

        public void AddChildren(Object o)
        {
            Children.Add(o);
        }
    }
}