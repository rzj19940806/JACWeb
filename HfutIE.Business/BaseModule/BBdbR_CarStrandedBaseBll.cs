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
    /// 滞留区域管理基本信息表
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.08.27 15:54</date>
    /// </author>
    /// </summary>
    public class BBdbR_CarStrandedBaseBll : RepositoryFactory<BBdbR_CarStrandedBase>
    {
        #region 全局变量定义区
        //定义本页面主要操作的表的表名，称为主表
        string tableName = "BBdbR_CarStrandedBase";//===复制时需要修改===
        #endregion
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
            strSql.Append(@"SELECT a.*,b.AviNm as StaAVINm,c.AviNm as EndAVINm FROM  " + tableName + " a join BBdbR_AVIBase b on a.StaAVIId=b.AviId join BBdbR_AVIBase c on a.EndAVIId=c.AviId where a.Enabled=1 ");
            dt = Repository().FindTableBySql(strSql.ToString(), false);
            return dt;
        }
        #endregion
        
        #region 3.编辑方法
        //将修改后的实体跟新到数据库中
        //返回值为1，或者0
        //1表示操作成功，0表示操作失败
        public int Update(BBdbR_CarStrandedBase entity) //===复制时需要修改===
        {
            return Repository().Update(entity); //将修改后的实体跟新到数据库中
        }
        #endregion
        #region 4.检查某个字段的字段值是否存在
        /// <summary>
        ///   Enabled = 1的数据中某个字段（KeyName）的字段值（KeyValue）是否存在
        /// </summary>
        /// <param name="KeyName">字段名</param>
        /// <param name="KeyValue">字段值</param>
        /// <returns>返回的是搜索的表中包含该字段值的记录条数</returns>
        public int CheckCount(string KeyName, string KeyValue)
        {
            string sql = @"select * from " + tableName + " where Enabled = '1' and'" + KeyName + "' = '" + KeyValue + "'";
            DataTable count = Repository().FindTableBySql(sql);
            int a = count.Rows.Count;
            return a;
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
        public int Insert(BBdbR_CarStrandedBase entity) //===复制时需要修改===
        {
            entity.Enabled = "1";

            return Repository().Insert(entity);
        }
        #endregion
        #region 7.删除方法
        //array 需要删除的信息的主键的数组
        //删除表中某一条数据表示将表中该条数据的Enabled设置为0，并不是真的删除该数据
        //返回值为1，或者0
        //1表示操作成功，0表示操作失败
        public int Delete(string[] array)
        {
            //创建一个表格的list，用于存储通过主键查询到的信息
            List<BBdbR_CarStrandedBase> listEntity = new List<BBdbR_CarStrandedBase>(); //===复制时需要修改===
            foreach (string keyValue in array)
            {
                //===复制时需要修改===
                BBdbR_CarStrandedBase entity = Repository().FindEntity(keyValue);//根据主键（keyValue）在数据库中查找实体 //===复制时需要修改===
                entity.Enabled = "0";//将该实体的IsAvailable属性改为false
                listEntity.Add(entity);
            }
            return Repository().Update(listEntity);//修改数据库
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
        public List<BBdbR_CarStrandedBase> GetPageListByCondition(string keywords, string Condition, JqGridParam jqgridparam) //===复制时需要修改===
        {
            string sql = "";
            List<BBdbR_CarStrandedBase> dt;
            if (Condition == "all")
            {
                sql = @"select * from " + tableName + " where Enabled = '1'";
                dt = Repository().FindListBySql(sql.ToString());
            }
            else
            {
                //根据条件查询
                sql = @"select * from " + tableName + " where Enabled = '1' and " + Condition + " like  '%" + keywords + "%'";
                dt = Repository().FindListBySql(sql.ToString());
            }
            return dt;
        }
        #endregion
        //public DataTable GetPlineNm()
        //{
        //    string sql = @"select Expr2 as id, Expr3 from dbo.View1 where Enabled='1'";
        //    return Repository().FindTableBySql(sql);
        //}
        public DataTable GetPlineNm()
        {
            string sql = @"select AviId , AviNm from BBdbR_AVIBase where Enabled='1'";
            return Repository().FindTableBySql(sql);
        }
        public DataTable GetPlineNm1()
        {
            string sql = @"select Expr2 as id, Expr3 from dbo.View1 where Enabled='1'";
            return Repository().FindTableBySql(sql);
        }
    }
}
