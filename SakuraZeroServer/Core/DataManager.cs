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

        /// <summary>
        /// 匹配用户名和密码，登录时调用
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public User VerifyUser(string username, string password)
        {
            return userDao.VerifyUser(sqlConn, username, password);
        }

        /// <summary>
        /// 查询用户名是否存在，注册时调用
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public bool CanGetUser(string username)
        {
            return userDao.GetUserByUsername(sqlConn, username);
        }

        /// <summary>
        /// 创建新用户
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool CreateUser(string username, string password)
        {
            return userDao.AddUser(sqlConn, username, password);
        }

        public bool SavaPlayer(Player player)
        {
            return true;
        }
    }
}
