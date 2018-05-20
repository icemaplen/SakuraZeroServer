using MySql.Data.MySqlClient;
using SakuraZeroCommon.Property;
using System;
using System.Collections.Generic;
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

        public bool CreatePlayer(int userid, string name, EPlayerJob playerJob, ECharacter character)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand("insert into player(userid,name,playerjob,playercharacter) values(@userid,@name,@playerjob,@character)", sqlConn);
                cmd.Parameters.AddWithValue("@userid", userid);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@playerjob", playerJob.ToString());
                cmd.Parameters.AddWithValue("@character", character.ToString());
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("在创建角色的时候出现异常：" + e);
                return false;
            }
        }

        public bool DeletePlayer(int playerid)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand("delete from player where playerid = @playerid", sqlConn);
                cmd.Parameters.AddWithValue("@playerid", playerid);
                cmd.ExecuteNonQuery();

                MySqlCommand cmd2 = new MySqlCommand("delete from inventory where playerid = @playerid", sqlConn);
                cmd2.Parameters.AddWithValue("@playerid", playerid);
                cmd2.ExecuteNonQuery();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("在删除角色的时候出现异常：" + e);
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
                    return GetPlayerByReader(reader);
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
                    Player player = GetPlayerByReader(reader);
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
       
        public Pos GetPlayerPos(int playerid)
        {
            MySqlDataReader reader = null;
            try
            {
                MySqlCommand cmd = new MySqlCommand("select mapnum, posx, posy, posz from player where playerid = @playerid", sqlConn);
                cmd.Parameters.AddWithValue("@playerid", playerid);
                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return new Pos(reader.GetInt32(0), reader.GetFloat(1), reader.GetFloat(2), reader.GetFloat(3));
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("在 GetPlayerPos(int playerid) 出现异常：\n" + e);
            }
            finally
            {
                if (reader != null) reader.Close();
            }
            return null;
        }

        public bool SavePlayerPos(int playerid, Pos pos)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand("update player set mapnum = @mapnum, posx = @posx, posy = @posy, posz = @posz where playerid = @playerid", sqlConn);
                cmd.Parameters.AddWithValue("@mapnum", pos.MapNum);
                cmd.Parameters.AddWithValue("@posx", pos.X);
                cmd.Parameters.AddWithValue("@posy", pos.Y);
                cmd.Parameters.AddWithValue("@posz", pos.Z);
                cmd.Parameters.AddWithValue("@playerid", playerid);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("在 SavePlayerPos(Player player) 出现异常：\n" + e);
                return false;
            }
        }

        private Player GetPlayerByReader(MySqlDataReader reader)
        {
            int playerid = reader.GetInt32("playerid");
            int userid = reader.GetInt32("userid");
            string name = reader.GetString("name");
            ESex sex = (ESex)Enum.Parse(typeof(ESex), reader.GetString("sex"));
            EPlayerJob job = (EPlayerJob)Enum.Parse(typeof(EPlayerJob), reader.GetString("playerjob"));
            ECharacter character = (ECharacter)Enum.Parse(typeof(ECharacter), reader.GetString("playercharacter"));
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
    }
}
