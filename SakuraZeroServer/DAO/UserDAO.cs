using MySql.Data.MySqlClient;
using SakuraZeroServer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SakuraZeroServer.Tool;

namespace SakuraZeroServer.DAO
{
    public class UserDAO
    {
        private MySqlConnection sqlConn;

        public UserDAO(MySqlConnection conn)
        {
            sqlConn = conn;
        }

        public User VerifyUser(string username, string password)
        {
            MySqlDataReader reader = null;
            try
            {
                MySqlCommand cmd = new MySqlCommand("select * from user where username = @username and password = @password", sqlConn);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", MD5Hash.GetHashCode(password));
                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    Console.WriteLine(reader.GetString("username"));
                    Console.WriteLine(reader.GetString("password"));
                    int id = reader.GetInt32("id");
                    User user = new User(id, username);
                    return user;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("在VerifyUser的时候出现异常：" + e);
            }
            finally
            {
                if (reader != null) reader.Close();
            }
            return null;
        }

        public bool GetUserByUsername(string username)
        {
            MySqlDataReader reader = null;
            try
            {
                MySqlCommand cmd = new MySqlCommand("select * from user where username = @username", sqlConn);
                cmd.Parameters.AddWithValue("@username", username);
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("在GetUserByUsername的时候出现异常：" + e);
            }
            finally
            {
                if (reader != null) reader.Close();
            }
            return false;
        }

        public bool AddUser(string username, string password)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand("insert into user set username = @username , password = @password", sqlConn);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", MD5Hash.GetHashCode(password));
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("在AddUser的时候出现异常：" + e);
                return false;
            }
        }
    }
}
