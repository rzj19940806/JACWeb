//=====================================================================================
// All Rights Reserved , Copyright @ HfutIE 2021
// Software Developers @ HfutIE 2021
//=====================================================================================

using HfutIE.Entity;
using HfutIE.Repository;
using HfutIE.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.Mvc;

namespace HfutIE.Business
{
    /// <summary>
    /// AVI站点基础信息表
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.03.26 10:57</date>
    /// </author>
    /// </summary>
    public class BBdbR_AVIBaseBll : RepositoryFactory<BBdbR_AVIBase>
    {
        #region 全局变量定义区

        //定义本页面主要操作的表的表名，称为主表
        string tableName = "BBdbR_AVIBase";//===复制时需要修改===
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
            StringBuilder sql1 = new StringBuilder();
            StringBuilder sql2 = new StringBuilder();
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            sql1.Append(@"SELECT a.*,b.PlineNm as PlineNm FROM  " + tableName + " a join BBdbR_PlineBase b on a.PlineId=b.PlineId where a.IsIndependence='0' and a.Enabled=1 ");
            dt1 = Repository().FindTableBySql(sql1.ToString(), false);
            sql2.Append(@"SELECT *,'无' as PlineNm FROM  " + tableName + " where IsIndependence='1' and Enabled=1 ");
            dt2 = Repository().FindTableBySql(sql2.ToString(), false);
            DataTable dt = dt1.Clone();
            object[] obj = new object[dt.Columns.Count];
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                dt1.Rows[i].ItemArray.CopyTo(obj, 0);
                dt.Rows.Add(obj);
            }
            for (int i = 0; i < dt2.Rows.Count; i++)
            {
                dt2.Rows[i].ItemArray.CopyTo(obj, 0);
                dt.Rows.Add(obj);
            }
            return dt;
        }
        #endregion

        #region 2.新增方法
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
        public int Insert(BBdbR_AVIBase entity) //===复制时需要修改===
        {
            //entity.Enabled = "1";

            return Repository().Insert(entity);
        }
        #endregion

        #region 3.编辑方法
        //将修改后的实体跟新到数据库中
        //返回值为1，或者0
        //1表示操作成功，0表示操作失败
        public int Update(BBdbR_AVIBase entity) //===复制时需要修改===
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
            string sql = @"select * from " + tableName + " where Enabled = '1' and " + KeyName + " = '" + KeyValue + "' ";
            DataTable count = Repository().FindTableBySql(sql);
            int a = count.Rows.Count;
            return a;
        }
        public int CheckCount2(string KeyName, string KeyValue, string KeyName2, string KeyValue2)
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

        #region 5.删除方法
        //array 需要删除的信息的主键的数组
        //删除表中某一条数据表示将表中该条数据的Enabled设置为0，并不是真的删除该数据
        //返回值为1，或者0
        //1表示操作成功，0表示操作失败
        public int Delete(string[] array)
        {
            //创建一个表格的list，用于存储通过主键查询到的信息
            List<BBdbR_AVIBase> listEntity = new List<BBdbR_AVIBase>(); //===复制时需要修改===
            foreach (string keyValue in array)
            {
                //===复制时需要修改===
                BBdbR_AVIBase entity = Repository().FindEntity(keyValue);//根据主键（keyValue）在数据库中查找实体 //===复制时需要修改===
                entity.Enabled = "0";//将该实体的IsAvailable属性改为false
                listEntity.Add(entity);
            }
            return Repository().Update(listEntity);//修改数据库
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
        public List<BBdbR_AVIBase> GetPageListByCondition(string keywords, string Condition, JqGridParam jqgridparam) //===复制时需要修改===
        {
            string sql = "";
            List<BBdbR_AVIBase> dt;

            if (Condition == "all" || keywords == "")
            {
                sql = @"select * from " + tableName + " where Enabled = '1' order by AviCd asc";
                dt = Repository().FindListBySql(sql.ToString());
            }
            else
            {
                //根据条件查询
                sql = @"select * from " + tableName + " where Enabled = '1' and " + Condition + " like  '%" + keywords + "%' order by AviCd asc";
                dt = Repository().FindListBySql(sql.ToString());
            }

            return dt;
        }
        #endregion

        #region 7.下拉框产线
        //11.获取所有下拉框产线
        public DataTable GetPlineNm()
        {
            string sql = @"select PlineId as id, PlineNm as plinenm from BBdbR_PlineBase where Enabled='1'";
            return Repository().FindTableBySql(sql);
        }
        #endregion

        #endregion
    }
}