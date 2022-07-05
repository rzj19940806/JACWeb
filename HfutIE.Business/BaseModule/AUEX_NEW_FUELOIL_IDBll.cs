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

namespace HfutIE.Business
{
    /// <summary>
    /// 燃油标识信息表
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.09.14 17:00</date>
    /// </author>
    /// </summary>
    public class AUEX_NEW_FUELOIL_IDBll : RepositoryFactory<AUEX_NEW_FUELOIL_ID>
    {
        #region 全局变量定义区

        //定义本页面主要操作的表的表名，称为主表
        string tableName = "AUEX_NEW_FUELOIL_ID";//===复制时需要修改===
        public readonly RepositoryFactory<Base_User> repository_User = new RepositoryFactory<Base_User>();
        #endregion

        #region 1.展示表格
        /// <summary>
        /// 搜索表格中所有Enabled = 1的数据
        /// </summary>
        /// <param name="jqgridparam">分页参数</param>
        /// <returns>返回搜索到的数据</returns>
        public List<AUEX_NEW_FUELOIL_ID> GetPageList(JqGridParam jqgridparam) //===复制时需要修改===
        {
            string sql = @"Select * From " + tableName + "  Where Enabled = '1' order by OPENING_DATE desc";
            List<AUEX_NEW_FUELOIL_ID> list = Repository().FindListBySql(sql);
            return list;
        }
        #endregion

        #region 2.编辑方法
        //将修改后的实体跟新到数据库中
        //返回值为1，或者0
        //1表示操作成功，0表示操作失败
        public int Update(AUEX_NEW_FUELOIL_ID entity) //===复制时需要修改===
        {
            return Repository().Update(entity); //将修改后的实体跟新到数据库中
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
        public int Insert(AUEX_NEW_FUELOIL_ID entity) //===复制时需要修改===
        {
            entity.Enabled = "1";

            return Repository().Insert(entity);
        }
        #endregion

        #region 5.删除方法
        //array 需要删除的信息的主键的数组
        //删除表中某一条数据表示将表中该条数据的Enabled设置为0，并不是真的删除该数据
        //返回值为1，或者0
        //1表示操作成功，0表示操作失败
        public  int Delete(string[] array)
        {
            //创建一个表格的list，用于存储通过主键查询到的信息
            List<AUEX_NEW_FUELOIL_ID> listEntity = new List<AUEX_NEW_FUELOIL_ID>(); //===复制时需要修改===
            foreach (string keyValue in array)
            {
                //===复制时需要修改===
                AUEX_NEW_FUELOIL_ID entity = Repository().FindEntity(keyValue);//根据主键（keyValue）在数据库中查找实体 //===复制时需要修改===
                entity.Enabled = "0";//将该实体的IsAvailable属性改为false
                listEntity.Add(entity);
            }
            return Repository().Update(listEntity);//更新数据库
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
        public List<AUEX_NEW_FUELOIL_ID> GetPageListByCondition(string keywords, string Condition, JqGridParam jqgridparam) //===复制时需要修改===
        {
            string sql = "";
            List<AUEX_NEW_FUELOIL_ID> dt;
            if (Condition == "all")
            {
                sql = @"select * from " + tableName + " where Enabled = 1 order by OPENING_DATE desc";
                dt = Repository().FindListBySql(sql.ToString());
            }
            else
            {
                //根据条件查询
                sql = @"select * from " + tableName + " where 1 = Enabled and " + Condition + " like  '%" + keywords + "%' order by OPENING_DATE desc";
                dt = Repository().FindListBySql(sql.ToString());
            }
            return dt;
        }
        #endregion
    }
}