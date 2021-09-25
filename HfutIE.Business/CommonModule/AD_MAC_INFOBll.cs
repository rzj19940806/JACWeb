//=====================================================================================
// All Rights Reserved , Copyright @ Learun 2017
// Software Developers @ Learun 2017
//=====================================================================================

using HfutIE.Entity;
using HfutIE.Repository;
using HfutIE.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace HfutIE.Business
{
    /// <summary>
    /// AD_MAC_INFO基础信息表
    /// <author>
    ///		<name>she</name>
    ///		<date>2017.11.18 15:09</date>
    /// </author>
    /// </summary>
    public class AD_MAC_INFOBll : RepositoryFactory<AD_MAC_INFO>
    {
        /// <summary>
        /// 根据mac码获取信息
        /// </summary>
        /// <returns></returns>
        public DataTable Getdt_mac(string maccode)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select * from AD_MAC_INFO where mac_code ='"+ maccode + "'");
            return Repository().FindTableBySql(strSql.ToString());
        }

        /// <summary>
        /// 根据主键获取mac信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetTable_Mac(string KeyValue)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select * from AD_MAC_INFO where mac_key='" + KeyValue + "'");
            return Repository().FindTableBySql(strSql.ToString());
        }

        
        /// <summary>
        /// mac信息检索
        /// </summary>
        /// <returns></returns>
        public DataTable Search_Mac(string type,string keywords)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select * from AD_MAC_INFO where 1=1");
            if (type!=""&& type!=null)
            {
                switch (type)
                {
                    case "1":
                        strSql.Append(@"");
                        break;
                    case "2":
                        strSql.Append(@" and mac_code like '%"+ keywords + "%'");
                        break;
                    case "3":
                        strSql.Append(@" and mac_name like '%" + keywords + "%'");
                        break;
                    case "4":
                        strSql.Append(@" and user_code like '%" + keywords + "%'");
                        break;
                    case "5":
                        strSql.Append(@" and user_name like '%" + keywords + "%'");
                        break;
                }
            }
            return Repository().FindTableBySql(strSql.ToString());
        }
    }
}