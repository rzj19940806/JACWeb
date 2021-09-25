using HfutIE.DataAccess;
using HfutIE.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HfutIE.Repository
{
    /// <summary>
    /// 操作数据库工厂
    /// </summary>
    public class DataFactory
    {
        /// <summary>
        /// 当前数据库类型
        /// </summary>
        private static string DbType = ConfigHelper.AppSettings("ComponentDbType");

        /// <summary>
        /// 获取指定的数据库连接
        /// </summary>
        /// <param name="connString"></param>
        /// <returns></returns>
        public static IDatabase Database(string connString)
        {
            return new Database(connString);
        }
        /// <summary>
        /// 获取指定的数据库连接
        /// </summary>
        /// <returns></returns>
        public static IDatabase Database()
        {
            switch (DbType)
            {
                case "SqlServer":
                    return Database("HfutIEFramework_SqlServer");
                case "MySql":
                    return Database("HfutIEFramework_MySql");
                case "Oracle":
                    return Database("HfutIEFramework_Oracle");
                default:
                    return null;
            }
        }
    }
}
