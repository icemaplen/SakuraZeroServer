using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SakuraZeroServer.Model
{
    public class User
    {
        public int ID { get; set; }
        public string Username { get; set; }
        //public string Password { get; set; }

        public User(int id, string username)
        {
            ID = id;
            Username = username;
            //Password = password;
        }
    }
}
