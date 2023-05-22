using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace TODOLIST.Models
{
    public class Person
    {
        public int id { get; set; }
        public string name { get; set; }

        public List<RequestData> PersonRequests { get; set; }

        private byte[] Password { get; set; }
        public string PasswordHash
        {
            get
            {
                var sb = new StringBuilder();
                foreach (var b in MD5.HashData(Password))
                    sb.Append(b.ToString("x2"));
                return sb.ToString();
            }
            set { Password = Encoding.UTF8.GetBytes(value); }
        }

        public bool IsAdmin => name == "admin";

        public bool CheckPassword(string password) => PasswordHash == password;

        //default constructor

        public Person()
        {

        }

        public Person(string name, string password)
        {
            this.name = name;
            this.PasswordHash = password;
        }
    }
}
