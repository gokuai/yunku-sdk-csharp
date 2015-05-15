using System;
using YunkuEntSDK.UtilClass;

namespace YunkuEntSDK.Data
{
    //同步成员和组织架构

  
//            Person person = new Person(0, "认证帐号", "显示名称", "邮箱", "联系电话");
//            List<object> list = new List<object>();
//            list.Add(person); //添加成员
//            list.Add(person); //添加成员
//            list.Add(person); //添加成员
//
//            Group group = new Group(0, "分组名称", list);
//            List<object> list2 = new List<object>();
//            list2.Add(group); //添加组
//            list2.Add(person); //添加成员
//            Group group2 = new Group(0, "分组名称", list2);
//
//            DeserializeReturn(entManager.SyncMembers(new SyncMemberData(person, group2).ToJsonString()),
//                entManager.StatusCode);
    public class SyncMemberData
    {
        public Person person;
        public Group group;

        public SyncMemberData(Person person, Group group)
        {
            this.person = person;
            this.group = group;
        }


        public String ToJsonString()
        {
            return SimpleJson.SerializeObject(this);

        }

    }
}
