using MySql.Data.MySqlClient;
using SakuraZeroCommon.Property;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SakuraZeroServer.Model;

namespace SakuraZeroServer.DAO
{
    class PlayerDAO
    {
        public bool CreatePlayer(MySqlConnection conn,int userid, string name, EPlayerJob playerJob)
        {
            int jobnum = 0;
            switch (playerJob)
            {
                case EPlayerJob.None:
                    jobnum = 0;
                    break;
                case EPlayerJob.Saber:
                    jobnum = 1;
                    break;
                default:
                    jobnum = 0;
                    break;
            }

            try
            {
                MySqlCommand cmd = new MySqlCommand("insert into player(userid,name,playerjob) values(@userid,@name,@playerjob)", conn);
                cmd.Parameters.AddWithValue("@userid", userid);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@playerjob", jobnum);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("在创建角色的时候出现异常：" + e);
                return false;
            }
        }

        public Player GetPlayer(MySqlConnection conn, int playerid)
        {
            MySqlDataReader reader = null;
            try
            {
                MySqlCommand cmd = new MySqlCommand("select * from player where playerid = @playerid", conn);
                cmd.Parameters.AddWithValue("@playerid", playerid);
                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    string name = reader.GetString("name");
                    int userid = reader.GetInt32("userid");
                    // TODO
                    Player player = new Player();
                    return player;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("在创建角色的时候出现异常：" + e);
            }
            finally
            {
                if (reader != null) reader.Close();
            }
            return null;
        }

        public Player[] GetPlayers(MySqlConnection conn, int userid)
        {
            Player[] players;
            //try
            //{
            //    MySqlCommand cmd = new MySqlCommand("insert into player(userid,name,playerjob) values(@userid,@name,@playerjob)", conn);
            //    cmd.Parameters.AddWithValue("@userid", userid);
            //    cmd.Parameters.AddWithValue("@name", name);
            //    cmd.Parameters.AddWithValue("@playerjob", jobnum);
            //    cmd.ExecuteNonQuery();
            //    return true;
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine("在创建角色的时候出现异常：" + e);
            //    return false;
            //}
        }
    }
}
