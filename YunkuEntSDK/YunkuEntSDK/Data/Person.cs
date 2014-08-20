using System;

namespace YunkuEntSDK.Data
{
    public class Person
    {
        public string Account;
        public string Email;
        public string Name;
        public string Phone;
        public int PId;

        public Person(int pid, String account, String name, String email, String phone)
        {
            PId = pid;
            Account = account;
            Name = name;
            Email = email;
            Phone = phone;
        }
    }
}