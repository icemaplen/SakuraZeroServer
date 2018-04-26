using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using SakuraZeroServer.Tool;
using SakuraZeroServer.Model;
using SakuraZeroServer.DAO;

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

        public DataManager()
        {
            sqlConn = SqlConnHelper.Connect();
            userDao = new UserDAO();
        }

        public bool Register(string username, string password)
        {
            return userDao.AddUser(sqlConn, username, password);
        }

        public UserModel VerifyUser(string username, string password)
        {
            return userDao.VerifyUser(sqlConn, username, password);
        }
        
    }
}
