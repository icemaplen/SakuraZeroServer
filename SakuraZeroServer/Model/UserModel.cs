using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SakuraZeroServer.Model
{
    public class UserModel
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public UserModel(int id, string username, string password)
        {
            ID = id;
            Username = username;
            Password = password;
        }
    }
}
