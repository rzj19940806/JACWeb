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
    public class DepartmentStaffConfigViewBll : RepositoryFactory<DepartmentStaffConfigView>
    {
        #region 全局变量定义区
        //定义本页面主要操作的表的表名，称为主表
        string tableName = "DepartmentStaffConfigView";//===复制时需要修改===
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
        public List<DepartmentStaffConfigView> GetTeamStaffList(string keywords, JqGridParam jqgridparam) //===复制时需要修改===
        {
            string sql = "";
            if (keywords != "")
            {
                sql = @"select * from " + tableName + " where Enabled=1 and StfId='" + keywords+"'";
                return Repository().FindListBySql(sql);
            }
            else
            {
                sql = @"select * from " + tableName + " where Enabled=1";
                return Repository().FindListBySql(sql);
            }
        }
        #endregion

        #region 2.点击树展示表格，需要修改sql语句
        /// <summary>
        /// 基本信息：【人员】   --属于-->   【部门】  --属于-->  【父部门】
        ///     根据传入的参数不同写不同的sql语句进行查询           
        /// </summary>
        /// <param name="areaId">点击的节点的主键</param>
        /// <param name="parentId">节点的父级主键</param>
        /// <param name="jqgridparam">分页参数</param>
        /// <returns></returns>
        public List<DepartmentStaffConfigView> GetList(string areaId, string parentId, ref JqGridParam jqgridparam) //===复制时需要修改===
        {
            string sql = "";
            if (string.IsNullOrEmpty(areaId) && string.IsNullOrEmpty(parentId))
            {
                sql = "select * from "+ tableName+" where  Enabled=1";     //===复制时需要修改===           
            }
            //未点击树，默认加载表格
            else
            {
                //点击树，展示对应节点表格
                if (parentId != "0")
                {
                    //从本表中查询上级表主键与传入主键相同相等的数据，并返回列表
                    sql = "select * from " + tableName + " where DepartmentID ='" + areaId + "' and  Enabled=1";     //===复制时需要修改===               
                }
                else
                {
                    sql = "select * from  " + tableName + "  where DepartmentID='" + areaId + "'  and  Enabled=1  or ParentDepartmentID='" + areaId + "'";     //===复制时需要修改===             
                }
            }        
            return Repository().FindListBySql(sql);
        }
        #endregion

        #region 3.查询方法，需要修改sql语句
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
        /// <returns>查询的数据（列表）</returns>
        public List<DepartmentStaffConfigView> GetPageListByCondition(string keywords, string Condition, JqGridParam jqgridparam) //===复制时需要修改===
        {
            string sql = "";
            if (Condition == "all")
            {
                sql = "select * from " + tableName + " where  Enabled=1";     //===复制时需要修改===         
            }
            else
            {
                if (keywords == "all")
                {
                    sql = "select * from " + tableName + " where  Enabled=1";     //===复制时需要修改===       
                }
                else
                {
                    sql = "select * from " + tableName + " where  Enabled=1 and " + Condition + " like  '%" + keywords + "%'";   //===复制时需要修改===       
                }          
            }
            return Repository().FindListBySql(sql);
        }
        #endregion


    }
}