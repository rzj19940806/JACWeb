//=====================================================================================
// All Rights Reserved , Copyright @ HfutIE 2021
// Software Developers @ HfutIE 2021
//=====================================================================================

using HfutIE.DataAccess;
using HfutIE.Entity;
using HfutIE.Repository;
using HfutIE.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace HfutIE.Business
{
    /// <summary>
    /// 班制基础信息表
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.08.29 16:19</date>
    /// </author>
    /// </summary>
    public class ClassConfigViewBll : RepositoryFactory<ClassConfigView>
    {
        #region 全局变量定义区
        //定义本页面主要操作的表的表名，称为主表
        string tableName = "ClassConfigView";//===复制时需要修改===
        #endregion


        #region 1.搜索
        /// <summary>
        ///     查询时提供了两个关键字，一个是Condition，另一个是keywords
        ///     
        ///     Condition是关键字，即查询条件，对应数据库中的一个字段
        ///     keywords是查询值，即查询条件的具体值，对应数据库中查询条件字段的值
        ///     查询的时候传递了Condition和keywords
        /// 
        /// </summary>
        /// <param name="keywords">查询值</param>
        /// <param name="Condition">关键字（查询条件）</param>
        /// <param name="jqgridparam">分页参数</param>
        /// <returns></returns>
        public List<ClassConfigView> GetPageListByCondition(string keywords, JqGridParam jqgridparam) //===复制时需要修改===
        {
            string sql = "";
            if (keywords != "")
            {
                sql = @"select * from " + tableName + " where Enabled=1 and ClassId='"+ keywords+"'";
                return Repository().FindListBySql(sql);
            }
            else
            {
                sql = @"select * from " + tableName + " where Enabled=1";
                return Repository().FindListBySql(sql);
            }
        }
        public ClassConfigView GetByCondition(string keywords) //===复制时需要修改===
        {
            string sql = "";
            if (keywords != "")
            {
                sql = @"select * from " + tableName + " where Enabled=1 and ClassId='" + keywords + "'";
                return Repository().FindEntityBySql(sql);
            }
            else
            {
                sql = @"select * from " + tableName + " where Enabled=1";
                return Repository().FindEntityBySql(sql);
            }
        }
        #endregion
    }
}