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

namespace HfutIE.Business
{
    /// <summary>
    /// 检查模板基本信息表
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.08.25 22:00</date>
    /// </author>
    /// </summary>
    public class BBdbR_QualityCheckModelBaseBll : RepositoryFactory<BBdbR_QualityCheckModelBase>
    {
        #region 全局变量定义区

        //定义本页面主要操作的表的表名，称为主表
        string tableName = "BBdbR_QualityCheckModelBase";//===复制时需要修改===
        public readonly RepositoryFactory<BBdbR_QualityCheckModelBase> repository_User = new RepositoryFactory<BBdbR_QualityCheckModelBase>();
        #endregion

        #region 1.获取树，需要修改sql语句
        public DataTable GetTree()
        {
            StringBuilder strSql = new StringBuilder();
            //===复制时需要修改===
            strSql.Append(@"select    
                                    '10' AS keys,
                                    '10'AS code,
                                    '质控点' AS name,
                                    '1' As IsAvailable,
                                    '0' AS parentId,
                                    '1' as sort");
            strSql.Append(@" union  select    
                             QualityCheckPointId AS keys,
                             QualityCheckPointCd AS code,
                             QualityCheckPointNm AS name,
                             '1' As IsAvailable,
                             '10' AS parentId,
                             '1' as sort 
                          from BBdbR_QualityCheckPointBase where Enabled = '1'");
            return Repository().FindTableBySql(strSql.ToString());
        }
        #endregion
        #region 2.点击树展示表格，需要修改sql语句
        /// <summary>
        /// 基本信息：AVI  
        /// </summary>
        /// <param name="areaId">点击的节点的主键</param>
        /// <param name="parentId">节点的父级主键</param>
        /// <param name="jqgridparam">分页参数</param>
        /// <returns></returns>
        public List<BBdbR_QualityCheckModelBase> GetList(string areaId, string parentId, ref JqGridParam jqgridparam) //===复制时需要修改===
        {
            List<BBdbR_QualityCheckModelBase> listEntity = new List<BBdbR_QualityCheckModelBase>();
            string sql = "";
            if (parentId != "0")
            {
                //从本表中查询上级表主键与传入主键相同相等的数据，并返回列表
                sql = "select * from " + tableName + " where QualityCheckPointId ='" + areaId + "' and Enabled = '1'";     //===复制时需要修改===
                listEntity = Repository().FindListPageBySql(sql, ref jqgridparam); ; //执行sql语句
                for (int i = 0; i < listEntity.Count; i++)
                {
                    string sql1 = "select * from BBdbR_QualityCheckPointBase where QualityCheckPointId='" + listEntity[i].QualityCheckPointId + "'";
                    DataTable dt1 = Repository().FindTableBySql(sql1.ToString());
                    if (dt1.Rows.Count > 0)
                    {
                        listEntity[i].QualityCheckPointId = dt1.Rows[0]["QualityCheckPointNm"].ToString();
                    }
                }
            }
            else
            {
                sql = "select * from " + tableName + " where 1 = 1 and Enabled = '1'";     //===复制时需要修改===
                listEntity = Repository().FindListPageBySql(sql, ref jqgridparam); //执行sql语句
                for (int i = 0; i < listEntity.Count; i++)
                {
                    string sql1 = "select * from BBdbR_QualityCheckPointBase where QualityCheckPointId='" + listEntity[i].QualityCheckPointId + "'";
                    DataTable dt1 = Repository().FindTableBySql(sql1.ToString());
                    if (dt1.Rows.Count > 0)
                    {
                        listEntity[i].QualityCheckPointId = dt1.Rows[0]["QualityCheckPointNm"].ToString();
                    }
                }
            }
            return listEntity;
        }
        #endregion
        #region 1.展示表格
        /// <summary>
        /// 搜索表格中所有IsAvailable = true的数据
        /// </summary>
        /// <param name="jqgridparam">分页参数</param>
        /// <returns>返回搜索到的数据</returns>
        public List<BBdbR_QualityCheckModelBase> GetPageList(JqGridParam jqgridparam) //===复制时需要修改===
        {
            string sql = @"select * from " + tableName + " where Enabled = '1'";
            return Repository().FindListPageBySql(sql, ref jqgridparam);
        }
        #endregion
        #region 2.编辑方法
        //将修改后的实体更新到数据库中
        //返回值为1，或者0
        //1表示操作成功，0表示操作失败
        public int Update(BBdbR_QualityCheckModelBase entity) //===复制时需要修改===
        {
            
            return Repository().Update(entity); //将修改后的实体更新到数据库中
        }
        #endregion
        #region 3.检查某个字段的字段值是否存在
        /// <summary>
        ///   Enabled = 1的数据中某个字段（KeyName）的字段值（KeyValue）是否存在
        /// </summary>
        /// <param name="KeyName">字段名</param>
        /// <param name="KeyValue">字段值</param>
        /// <returns>返回的是搜索的表中包含该字段值的记录条数</returns>
        public int CheckCount(string KeyName, string KeyValue)
        {
            string sql = @"select * from " + tableName + " where Enabled = '1' and " + KeyName + " = '" + KeyValue + "'";
            DataTable count = Repository().FindTableBySql(sql);
            int a = count.Rows.Count;
            return a;
        }
        #endregion
        #region 4.新增方法
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
        public int Insert(BBdbR_QualityCheckModelBase entity) //===复制时需要修改===
        {
            
            return Repository().Insert(entity);
        }
        #endregion

        #region 5.删除方法
        //array 需要删除的信息的主键的数组
        //删除表中某一条数据表示将表中该条数据的Enabled设置为0，并不是真的删除该数据
        //返回值为1，或者0
        //1表示操作成功，0表示操作失败
        public int Delete(string[] array)
        {
            //创建一个表格的list，用于存储通过主键查询到的信息
            List<BBdbR_QualityCheckModelBase> listEntity = new List<BBdbR_QualityCheckModelBase>(); //===复制时需要修改===
            foreach (string keyValue in array)
            {
                //===复制时需要修改===
                BBdbR_QualityCheckModelBase entity = Repository().FindEntity(keyValue);//根据主键（keyValue）在数据库中查找实体 //===复制时需要修改===
                entity.Enabled = "0";//将该实体的IsAvailable属性改为false
                entity.Modify(entity.QualityCheckModelId);
                listEntity.Add(entity);
            }

            return Repository().Update(listEntity);//假删数据库
        }
        #endregion

        #region 6.查询方法，需要修改sql语句
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
        public List<BBdbR_QualityCheckModelBase> GetPageListByCondition(string keywords, string Condition, JqGridParam jqgridparam) //===复制时需要修改===
        {
            string sql = "";
            List<BBdbR_QualityCheckModelBase> dt;
            if (Condition == "all")
            {
                sql = @"select * from " + tableName + " where 1 = 1 and Enabled = '1'";
                dt = Repository().FindListBySql(sql.ToString());
            }
            else
            {
                //根据条件查询
                sql = @"select * from " + tableName + " where 1 = 1 and Enabled = '1' and " + Condition + " like  '%" + keywords + "%'";
                dt = Repository().FindListBySql(sql.ToString());
            }
            return dt;
        }
        #endregion

        //#region 7.页面表格
        ///// <summary>
        ///// 联合查询，展示页面表格
        ///// </summary>
        ///// <param name="CheckId"></param>
        ///// <returns></returns>
        //public List<BBdbR_MatBase> GetPlanList()
        //{
        //    StringBuilder strSql = new StringBuilder();
        //    strSql.Append(@"SELECT  * FROM  BBdbR_MatBase where Enabled = '1' and MatCatg = '产品'");
        //    List<BBdbR_MatBase> dt = Repository().FindListBySql(strSql.ToString());
        //    return dt;
        //}
        ///// <summary>
        ///// 联合查询，展示页面表格
        ///// </summary>
        ///// <param name="CheckId"></param>
        ///// <returns></returns>
        //public BBdbR_QualityCheckModelBase GetPlanList1(string StfId)
        //{
        //    StringBuilder strSql = new StringBuilder();
        //    strSql.Append(@"SELECT  * FROM  BBdbR_QualityCheckModelBase where 1=1 and StfId='" + StfId + "'");
        //    List<BBdbR_QualityCheckModelBase> dt = Repository().FindListBySql(strSql.ToString());
        //    return dt[0];
        //}
        //#endregion


    }
}