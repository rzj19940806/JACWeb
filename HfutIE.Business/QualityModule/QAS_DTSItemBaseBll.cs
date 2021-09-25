//=====================================================================================
// All Rights Reserved , Copyright @ HfutIE 2021
// Software Developers @ HfutIE 2021
//=====================================================================================

using HfutIE.DataAccess;
using HfutIE.Entity;
using HfutIE.Repository;
using HfutIE.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace HfutIE.Business
{
    /// <summary>
    /// DTS检测项基础信息表
    /// <author>
    ///		<name>CHFAS</name>
    ///		<date>2021.07.06 12:00</date>
    /// </author>
    /// </summary>
    public class QAS_DTSItemBaseBll : RepositoryFactory<QAS_DTSItemBase>
    {
        #region
        #endregion
        #region 全局变量定义区
        //定义本页面主要操作的表的表名，称为主表
        string tableName = "QAS_DTSItemBase";
        #endregion

        #region 方法区
        /// <summary>
        /// 单条件查询
        /// </summary>
        /// <param name="PropertyName"></param>
        /// <param name="PropertyValue"></param>
        /// <returns></returns>
        public List<QAS_DTSItemBase> GetPageList(string PropertyName, string PropertyValue) 
        {
            List<QAS_DTSItemBase> dt;
            string sql = "";
            if (PropertyName == "")
            {
                sql = "select * from " + tableName + " where 1=1 order by DTSItemID";
                dt = Repository().FindListBySql(sql);
            }
            else
            {
                //根据条件查询
                sql = "select * from " + tableName + " where " + PropertyName + " like  '%" + PropertyValue + "%' order by DTSItemID";
                dt = Repository().FindListBySql(sql);
            }
            return dt;
        }

        #endregion
    }
}