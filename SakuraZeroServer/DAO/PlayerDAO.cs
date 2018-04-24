using MySql.Data.MySqlClient;
using SakuraZeroCommon.Property;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
