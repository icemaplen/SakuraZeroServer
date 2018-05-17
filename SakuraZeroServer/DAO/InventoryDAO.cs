using MySql.Data.MySqlClient;
using SakuraZeroServer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SakuraZeroServer.DAO
{
    class InventoryDAO
    {
        private MySqlConnection sqlConn;

        public InventoryDAO(MySqlConnection conn)
        {
            sqlConn = conn;
        }

        public List<Item> GetAllItems(int playerid)
        {
            List<Item> itemList = new List<Item>();
            MySqlDataReader reader = null;
            try
            {
                MySqlCommand cmd = new MySqlCommand("select * from inventory where playerid = @playerid", sqlConn);
                cmd.Parameters.AddWithValue("@playerid", playerid);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int itemid = reader.GetInt32("itemid");
                    int count = reader.GetInt32("count");
                    bool isDressed = reader.GetInt32("isdressed") == 1;
                    Item item = new Item(itemid, count, isDressed);
                    itemList.Add(item);
                }
                return itemList;
            }
            catch (Exception e)
            {
                Console.WriteLine("在 GetAllItems 出现异常：\n" + e);
            }
            finally
            {
                if (reader != null) reader.Close();
            }
            return null;
        }

        public Item FindItem(int playerid, int itemid)
        {
            MySqlDataReader reader = null;
            try
            {
                MySqlCommand cmd = new MySqlCommand("select * from inventory where playerid = @playerid and itemid = @itemid", sqlConn);
                cmd.Parameters.AddWithValue("@playerid", playerid);
                cmd.Parameters.AddWithValue("@itemid", itemid);
                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    int count = reader.GetInt32("count");
                    bool isDressed = reader.GetInt32("isdressed") == 1;
                    return new Item(itemid, count, isDressed);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("在 CanFindItem(int playerid, int itemid) 出现异常：\n" + e);
            }
            finally
            {
                if (reader != null) reader.Close();
            }
            return null;
        }

        public bool UpdateItem(int playerid, int itemid, int deltaCount)
        {
            try
            {
                MySqlCommand cmd;
                Item item = FindItem(playerid, itemid);
                if (item != null)
                {
                    if (item.Count + deltaCount <= 0)
                    {
                        cmd = new MySqlCommand("delete from inventory where playerid = @playerid and itemid = @itemid", sqlConn);
                        cmd.Parameters.AddWithValue("@playerid", playerid);
                        cmd.Parameters.AddWithValue("@itemid", itemid);
                    }
                    else
                    {
                        cmd = new MySqlCommand("update inventory set count = @count where playerid = @playerid and itemid = @itemid", sqlConn);
                        cmd.Parameters.AddWithValue("@playerid", playerid);
                        cmd.Parameters.AddWithValue("@itemid", itemid);
                        cmd.Parameters.AddWithValue("@count", item.Count + deltaCount);
                    }

                }
                else if (deltaCount > 0)
                {
                    cmd = new MySqlCommand("insert into inventory(playerid,itemid,count) values(@playerid,@itemid,@count)", sqlConn);
                    cmd.Parameters.AddWithValue("@playerid", playerid);
                    cmd.Parameters.AddWithValue("@itemid", itemid);
                    cmd.Parameters.AddWithValue("@count", deltaCount);
                }
                else
                {
                    return false;
                }

                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("在 UpdateItem 时候出现异常：" + e);
                return false;
            }
        }

        public bool UpdateEquipmentStatus(int playerid, int itemid, bool isDressed)
        {
            int tmpIsDressed = isDressed ? 1 : 0;
            MySqlDataReader reader = null;
            try
            {
                MySqlCommand cmd = new MySqlCommand("update inventory set isdressed = @isdressed where playerid = @playerid and itemid = @itemid", sqlConn);
                cmd.Parameters.AddWithValue("@playerid", playerid);
                cmd.Parameters.AddWithValue("@itemid", itemid);
                cmd.Parameters.AddWithValue("@isdressed", tmpIsDressed);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("在 CanFindItem(int playerid, int itemid) 出现异常：\n" + e);
            }
            finally
            {
                if (reader != null) reader.Close();
            }
            return false;
        }
    }
}
