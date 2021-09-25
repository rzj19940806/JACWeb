//=====================================================================================
// All Rights Reserved , Copyright @ HfutIE 2021
// Software Developers @ HfutIE 2021
//=====================================================================================

using HfutIE.Entity;
using HfutIE.Repository;
using HfutIE.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.Mvc;

namespace HfutIE.Business
{
    /// <summary>
    /// AVI去向配置信息表
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.08.31 12:49</date>
    /// </author>
    /// </summary>
    public class BBdbR_AVIGroupConfigBll : RepositoryFactory<BBdbR_AVIGroupConfig>
    {
        #region 全局变量定义区

        //定义本页面主要操作的表的表名，称为主表
        string tableName = "BBdbR_AVIGroupConfig";//===复制时需要修改===
        //BBdbR_AVIAcitonConfig
        #endregion

        #region 方法区


        #region 1.页面表格
        /// <summary>
        /// 联合查询，展示页面表格
        /// </summary>
        /// <param name="CheckId"></param>
        /// <returns></returns>
        public DataTable GetPlanList()
        {
            StringBuilder strSql = new StringBuilder();
            DataTable dt;
            strSql.Append(@"SELECT * FROM  " + tableName + " where AVIId is NULL and Enabled=1 ");
            dt = Repository().FindTableBySql(strSql.ToString(), false);
            return dt;
        }
        #endregion

        #region 2.AVI分组配置信息表格-未在该组
        /// <summary>
        /// 点击AVI站点基础信息，查询AVI去向配置信息表
        /// 查询的时候传递了keywords
        /// </summary>
        /// <param name="keywords">查询值</param>
        /// <param name="jqgridparam">分页参数</param>
        /// <returns></returns>
        public DataTable ReGetConfigList(string keywords, JqGridParam jqgridparam) //===复制时需要修改===
        {
            string sql = "";
            sql = @"select AviId as AVIId,AviCd as AVICd,AviNm as AVINm from BBdbR_AVIBase where Enabled=1 and AviId not in (select ISNULL(AVIId,'') from BBdbR_AVIGroupConfig where Enabled=1 and AVIGroupCd in (select AVIGroupCd from BBdbR_AVIGroupConfig where Enabled=1 and AVIGroupId='" + keywords + "'))";
            return (Repository().FindTableBySql(sql.ToString(), false));
        }
        #endregion

        #region 3.AVI分组配置信息表格-已在该组
        /// <summary>
        /// 点击AVI站点基础信息，查询AVI去向配置信息表
        /// 查询的时候传递了keywords
        /// </summary>
        /// <param name="keywords">查询值</param>
        /// <param name="jqgridparam">分页参数</param>
        /// <returns></returns>
        public DataTable GetConfigList(string keywords, JqGridParam jqgridparam) //===复制时需要修改===
        {
            string sql = "";
            if (keywords != "")
            {
                sql = @"select * from " + tableName + " where AVIId is not null and Enabled=1 and AVIGroupCd in (select AVIGroupCd from " + tableName + " where AVIGroupId='" + keywords + "')";
                return (Repository().FindTableBySql(sql.ToString(), false));
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region 4.删除
        //array 需要删除的信息的主键的数组
        //删除表中某一条数据表示将表中该条数据的Enabled设置为0，并不是真的删除该数据
        //返回值为1，或者0
        //1表示操作成功，0表示操作失败
        public int Delete(string keyvalue)//删除组内站点dt.Rows[0]["AVIGroupCd"]
        {
            BBdbR_AVIGroupConfig entity = Repository().FindEntity(keyvalue);
            entity.AVIGroupCount = "0";
            Repository().Update(entity);
            StringBuilder deletesql = new StringBuilder();
            deletesql.Append(@"update " + tableName + " set Enabled = 0 where AVIId is not null and AVIGroupCd='" + entity.AVIGroupCd + "'");
            return Repository().ExecuteBySql(deletesql);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
         public int Delete1(string keyvalue)//删整个组
        {
            DataTable dt = new DataTable();
            string sql = @"select * from "+tableName+" where Enabled=1 and AVIGroupId='"+keyvalue+"'";
            dt = Repository().FindTableBySql(sql);
            StringBuilder deletesql = new StringBuilder();
            deletesql.Append(@"update " + tableName + " set Enabled = 0 where AVIGroupCd='" + dt.Rows[0]["AVIGroupCd"] + "'");
            return Repository().ExecuteBySql(deletesql);
        }
        #endregion

        #region 5.新增方法
        //entity实体中的数据是从页面中传来的，它是用户填写的数据
        //entity实体中只有部分字段有值，因为页面中只提供给部分字段赋值
        //将页面中填写的数据以实体（entity）的方式新增到数据库
        //其中，实体中的IsAvailable字段没有在页面中填写
        //IsAvailable字段的作用是做假删除，即数据库中的某一条数据的IsAvailable字段的字段值为true表示该数据存在
        //字段值为false表示该数据被删除
        //在删除数据库中的某一条数据时只要修改IsAvailable字段的字段值为false，并不删除该条数据
        //在新增时将实体的IsAvailable字段的值修改为true
        //返回值为1，或者0
        //1表示操作成功，0表示操作失败
        public int Insert(BBdbR_AVIGroupConfig entity,string keyvalue,int i) //===复制时需要修改===
        {
            List<BBdbR_AVIGroupConfig> listEntity = new List<BBdbR_AVIGroupConfig>(); //===复制时需要修改===
            //i = i + 1;
            //BBdbR_AVIGroupConfig entity1 = Repository().FindEntity(keyvalue);//根据主键（keyValue）在数据库中查找实体 //===复制时需要修改===
            //entity1.AVICount = i.ToString();//将该实体的IsAvailable属性改为false
            //Repository().Update(entity1);
            return Repository().Insert(entity);
        }
        public int Insert1(BBdbR_AVIGroupConfig entity, int i) //===复制时需要修改===
        {
            return Repository().Insert(entity);
        }
        #endregion

        #region 6.检查字段是否唯一
        public int CheckCount(string KeyName, string KeyValue)
        {
            string sql = @"select * from " + tableName + " where Enabled='1' and " + KeyName + " = '" + KeyValue + "'";
            DataTable count = Repository().FindTableBySql(sql);
            int a = count.Rows.Count;
            return a;
        }

        public int CheckCount2(string KeyName, string KeyValue,string KeyName2,string KeyValue2)
        {
            string sql = @"select * from " + tableName + " where Enabled='1' and " + KeyName + " = '" + KeyValue + "'";
            DataTable count = Repository().FindTableBySql(sql);
            int a = count.Rows.Count;
            if (a == 0)
            {
                sql = @"select * from " + tableName + " where Enabled='1' and  " + KeyName2 + " = '" + KeyValue2 + "'";
                count = Repository().FindTableBySql(sql);
                if (count.Rows.Count > 0)
                {
                    a = 2;
                    return a;
                }
            }
            return a;
        }
        #endregion

        #region 7.编辑方法
        //将修改后的实体跟新到数据库中
        //返回值为1，或者0
        //1表示操作成功，0表示操作失败
        public int Update(BBdbR_AVIGroupConfig entity,string KeyValue) //===复制时需要修改===
        {
            StringBuilder updatesql = new StringBuilder();
            DataTable dt = new DataTable();
            string sql=@"select AVIGroupCd from "+tableName+" where AVIGroupId='"+ KeyValue + "'";
            dt = Repository().FindTableBySql(sql.ToString(), false);
             updatesql.Append(@"update " + tableName + " set AVIGroupCd='" + entity.AVIGroupCd + "',AVIGroupNm='" + entity.AVIGroupNm + "' where AVIGroupCd='" + dt.Rows[0][0] + "' and Enabled=1");
             int a=Repository().ExecuteBySql(updatesql);
            
             return Repository().Update(entity);  //将修改后的实体跟新到数据库中
           
        }
        #endregion

        #region 8.查询方法，需要修改sql语句
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
        public DataTable GetPageListByCondition(string keywords, string Condition, JqGridParam jqgridparam) //===复制时需要修改===
        {
            string sql = "";
            DataTable dt;
            if (Condition == "all")
            {
                sql = @"SELECT * FROM  " + tableName + " where AVIId is NULL and Enabled=1 ";
                dt = Repository().FindTableBySql(sql.ToString(), false);
            }
            
            else 
            {
                //根据条件查询
                //string sql1 = @"select ";
                sql = @"SELECT * FROM  " + tableName + " where AVIId is NULL and Enabled = '1' and " + Condition + " like  '%" + keywords + "%'";
                dt = Repository().FindTableBySql(sql.ToString(), false);
            }
            return dt;
        }
        #endregion


        #endregion
    }
}