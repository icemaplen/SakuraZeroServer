using System.Collections.Generic;
using MySql.Data.MySqlClient;
using SakuraZeroServer.Tool;
using SakuraZeroServer.Model;
using SakuraZeroServer.DAO;
using SakuraZeroCommon.Property;
using SakuraZeroCommon.Tool;

namespace SakuraZeroServer.Core
{
    /// <summary>
    /// 数据库封装，操作MySQL数据库.
    /// </summary>
    public class DataManager
    {
        #region 单例模式

        private static DataManager instance;
        public static DataManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DataManager();
                }
                return instance;
            }
        }
        #endregion

        private MySqlConnection sqlConn;
        private UserDAO userDao;
        private PlayerDAO playerDAO;
        private InventoryDAO inventoryDAO;

        public DataManager()
        {
            //sqlConn = SqlConnHelper.Connect();
            userDao = new UserDAO(SqlConnHelper.Connect());
            playerDAO = new PlayerDAO(SqlConnHelper.Connect());
            inventoryDAO = new InventoryDAO(SqlConnHelper.Connect());
        }

        /// <summary>
        /// 匹配用户名和密码，登录时调用
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public User VerifyUser(string username, string password)
        {
            return userDao.VerifyUser(username, password);
        }

        /// <summary>
        /// 查询用户名是否存在，注册时调用
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public bool CanGetUser(string username)
        {
            return userDao.GetUserByUsername(username);
        }

        /// <summary>
        /// 创建新用户
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool CreateUser(string username, string password)
        {
            return userDao.AddUser(username, password);
        }

        public Player GetPlayer(int playerid)
        {
            return playerDAO.GetPlayer(playerid);
        }

        public List<Player> GetPlayers(int userid)
        {
            return playerDAO.GetPlayers(userid);
        }

        public bool CreateRole(int userid,string name,EPlayerJob job, ECharacter character)
        {
            return playerDAO.CreatePlayer(userid, name, job, character);
        }

        public bool DeleteRole(int playerid)
        {
            return playerDAO.DeletePlayer(playerid);
        }

        public int GetPlayerIDByName(string name)
        {
            return playerDAO.GetPlayerIDByName(name);
        }

        public bool SavaPlayer(Player player)
        {
            return playerDAO.SavePlayer(player);
        }

        public bool SavePlayerPos(int playerid, Pos pos)
        {
            return playerDAO.SavePlayerPos(playerid, pos);
        }

        public Pos GetPlayerPos(int playerid)
        {
            return playerDAO.GetPlayerPos(playerid);
        }

        public List<Item> GetAllItems(int playerid)
        {
            return inventoryDAO.GetAllItems(playerid);
        }

        public Item FindItem(int playerid, int itemid)
        {
            return inventoryDAO.FindItem(playerid, itemid);
        }

        public bool UpdateItem(int playerid, int itemid, int deltaCount)
        {
            return inventoryDAO.UpdateItem(playerid, itemid, deltaCount);
        }

        public bool UpdateEquipmentStatus(int playerid, int itemid, bool isDressed)
        {
            return inventoryDAO.UpdateEquipmentStatus(playerid, itemid, isDressed);
        }

        public bool UpdateGold(int playerid, int gold)
        {
            return inventoryDAO.UpdateGold(playerid, gold);
        }
    }
}
