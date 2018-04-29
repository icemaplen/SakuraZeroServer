using MySql.Data.MySqlClient;
using SakuraZeroCommon.Property;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SakuraZeroServer.Model;
using SakuraZeroCommon.Tool;

namespace SakuraZeroServer.DAO
{
    class PlayerDAO
    {
        private MySqlConnection sqlConn;

        public PlayerDAO(MySqlConnection conn)
        {
            sqlConn = conn;
        }

        public bool CreatePlayer(int userid, string name, EPlayerJob playerJob)
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
                MySqlCommand cmd = new MySqlCommand("insert into player(userid,name,playerjob) values(@userid,@name,@playerjob)", sqlConn);
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

        public int GetPlayerIDByName(string name)
        {
            MySqlDataReader reader = null;
            try
            {
                MySqlCommand cmd = new MySqlCommand("select * from player where name = @name", sqlConn);
                cmd.Parameters.AddWithValue("@name", name);
                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    int id = reader.GetInt32("playerid");
                    return id;
                }
                else
                {
                    return -1;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("在 GetPlayerIDByName(string name) 出现异常：\n" + e);
            }
            finally
            {
                if (reader != null) reader.Close();
            }
            return -1;
        }

        public Player GetPlayer(int playerid)
        {
            MySqlDataReader reader = null;
            try
            {
                MySqlCommand cmd = new MySqlCommand("select * from player where playerid = @playerid", sqlConn);
                cmd.Parameters.AddWithValue("@playerid", playerid);
                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    string name = reader.GetString("name");
                    int userid = reader.GetInt32("userid");
                    ESex sex = (ESex)Enum.Parse(typeof(ESex), reader.GetInt32("sex").ToString());
                    EPlayerJob job = (EPlayerJob)Enum.Parse(typeof(EPlayerJob), reader.GetInt32("playerjob").ToString());
                    ECharacter character = (ECharacter)Enum.Parse(typeof(ECharacter), reader.GetInt32("character").ToString());
                    int gold = reader.GetInt32("gold");
                    int level = reader.GetInt32("level");
                    int exp = reader.GetInt32("exp");
                    int mapNum = reader.GetInt32("mapnum");
                    float posx = reader.GetFloat("posx");
                    float posy = reader.GetFloat("posy");
                    float posz = reader.GetFloat("posz");
                    Pos pos = new Pos(posx, posy, posz);
                    Player player = new Player(playerid, name, userid, sex, job, character, gold, level, exp, mapNum, pos);
                    return player;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("在 GetPlayer(int playerid) 出现异常：\n" + e);
            }
            finally
            {
                if (reader != null) reader.Close();
            }
            return null;
        }

        public List<Player> GetPlayers(int userid)
        {
            List<Player> playerList = new List<Player>();
            MySqlDataReader reader = null;
            try
            {
                MySqlCommand cmd = new MySqlCommand("select * from player where userid = @userid", sqlConn);
                cmd.Parameters.AddWithValue("@userid", userid);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int playerid = reader.GetInt32("playerid");
                    string name = reader.GetString("name");
                    ESex sex = (ESex)Enum.Parse(typeof(ESex), reader.GetInt32("sex").ToString());
                    EPlayerJob job = (EPlayerJob)Enum.Parse(typeof(EPlayerJob), reader.GetInt32("playerjob").ToString());
                    ECharacter character = (ECharacter)Enum.Parse(typeof(ECharacter), reader.GetInt32("character").ToString());
                    int gold = reader.GetInt32("gold");
                    int level = reader.GetInt32("level");
                    int exp = reader.GetInt32("exp");
                    int mapNum = reader.GetInt32("mapnum");
                    float posx = reader.GetFloat("posx");
                    float posy = reader.GetFloat("posy");
                    float posz = reader.GetFloat("posz");
                    Pos pos = new Pos(posx,posy,posz);
                    Player player = new Player(playerid, name, userid, sex, job, character, gold, level, exp, mapNum, pos);
                    playerList.Add(player);
                }
                return playerList;
            }
            catch (Exception e)
            {
                Console.WriteLine("在 GetPlayers(int userid) 出现异常：\n" + e);
            }
            finally
            {
                if (reader != null) reader.Close();
            }
            return null;
        }

        public bool SavePlayer(Player player)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand("update player set name = @name, gold = @gold, level = @level, exp = @exp, mapnum = @mapnum, pos = @pos where playerid = @playerid", sqlConn);
                cmd.Parameters.AddWithValue("@name", player.Name);
                cmd.Parameters.AddWithValue("@gold", player.Gold);
                cmd.Parameters.AddWithValue("@level", player.Level);
                cmd.Parameters.AddWithValue("@exp", player.Exp);
                cmd.Parameters.AddWithValue("@mapnum", player.MapNum);
                cmd.Parameters.AddWithValue("@pos", player.Pos.ToString());
                cmd.Parameters.AddWithValue("@playerid", player.ID);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("在 SavePlayer(Player player) 出现异常：\n" + e);
                return false;
            }
        }

        public bool SavePlayerPos(Player player)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand("update player set mapnum = @mapnum, pos = @pos where playerid = @playerid", sqlConn);
                cmd.Parameters.AddWithValue("@mapnum", player.MapNum);
                cmd.Parameters.AddWithValue("@pos", player.Pos.ToString());
                cmd.Parameters.AddWithValue("@playerid", player.ID);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("在 SavePlayerPos(Player player) 出现异常：\n" + e);
                return false;
            }
        }
    }
}
