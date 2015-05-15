namespace YunkuEntSDK.Data
{
    public class Person
    {
        public string account;
        public string email;
        public string name;
        public string phone;
        public int pid;

        public Person(int pid, string account, string name, string email, string phone)
        {
            this.pid = pid;
            this.account = account;
            this.name = name;
            this.email = email;
            this.phone = phone;
        }
    }
}