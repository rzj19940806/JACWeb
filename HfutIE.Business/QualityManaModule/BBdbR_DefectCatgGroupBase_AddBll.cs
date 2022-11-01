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
    /// 缺陷类别分组基本信息表
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.10.09 20:06</date>
    /// </author>
    /// </summary>
    public class BBdbR_DefectCatgGroupBase_AddBll : RepositoryFactory<BBdbR_DefectCatgGroupBase_Add>
    {
        #region 全局变量定义区

        //定义本页面主要操作的表的表名，称为主表
        string tableName = "BBdbR_DefectCatgGroupBase_Add";//===复制时需要修改===
        #endregion

        #region 方法区
        #region 1.获取树，需要修改sql语句
        public DataTable GetTree(string Type)
        {
            StringBuilder strSql = new StringBuilder();
            if (Type == "CH")//冲焊
            {
                //===复制时需要修改===
                strSql.Append(@"select DefectCatgId as keys,
                                   DefectCatgCd as code,
                                   DefectCatgNm as name,
                                   '0'as parentId,
                                    '0'as sort 
                            from BBdbR_DefectCatgBase_Add 
                            where Enabled=1 and type='CH' order by code asc ");
            }
            else//涂装
            {        
                //===复制时需要修改===
                strSql.Append(@"select DefectCatgId as keys,
                                   DefectCatgCd as code,
                                   DefectCatgNm as name,
                                   '0'as parentId,
                                    '0'as sort 
                            from BBdbR_DefectCatgBase_Add 
                            where Enabled=1 and type='TZ'order by code asc ");

            }
            return Repository().FindTableBySql(strSql.ToString());
        }
        #endregion

        #region 2.点击树展示表格，需要修改sql语句
        /// <summary>
        ///     根据传入的参数不同写不同的sql语句进行查询
        ///             
        /// </summary>
        /// <param name="areaId">点击的节点的主键</param>
        /// <param name="sort">点击的节点的层级编号</param>
        /// <param name="jqgridparam">分页参数</param>
        /// <returns></returns>
        public DataTable GetList(string areaId, string text, string value, string sort, ref JqGridParam jqgridparam)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select * from BBdbR_DefectCatgGroupBase_Add where  DefectCatgId='" + areaId + "' and Enabled=1 order by DefectCatgGroupCd asc");
            return Repository().FindTableBySql(strSql.ToString(),false);
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
            string sql = @"select * from " + tableName + " where Enabled = '1' and  " + KeyName + "  = '" + KeyValue + "'";
            DataTable count = Repository().FindTableBySql(sql);
            int a = count.Rows.Count;
            return a;
        }
        #endregion

        #region 4.新增方法
        public int Insert(BBdbR_DefectCatgGroupBase_Add entity) //===复制时需要修改===
        {
            return Repository().Insert(entity);
        }
        #endregion

        #region 5.编辑方法
        public int Update(BBdbR_DefectCatgGroupBase_Add entity) //===复制时需要修改===
        {
            return Repository().Update(entity);
        }
        #endregion

        #region 6.查询方法
        public List<BBdbR_DefectCatgGroupBase_Add> GetGridList(string Condition,string  keywords,string  Nodeareaid, JqGridParam jqgridparam) //===复制时需要修改===
        {
            string sql = "";
            if (Condition != "all")
            {
                 sql = @"Select * from BBdbR_DefectCatgGroupBase_Add where Enabled=1 and " + Condition + " like '%" + keywords + "%' and DefectCatgId='" + Nodeareaid + "'";
            }
            else
            {
                sql = @"Select * from BBdbR_DefectCatgGroupBase_Add where Enabled=1 and  DefectCatgId='" + Nodeareaid + "'";
            }
            return Repository().FindListBySql(sql);
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
            List<BBdbR_DefectCatgGroupBase_Add> listEntity = new List<BBdbR_DefectCatgGroupBase_Add>(); //===复制时需要修改===
            foreach (string keyValue in array)
            {
                //===复制时需要修改===
                BBdbR_DefectCatgGroupBase_Add entity = Repository().FindEntity(keyValue);//根据主键（keyValue）在数据库中查找实体 //===复制时需要修改===
                entity.Enabled = "0";//将该实体的IsAvailable属性改为false
                listEntity.Add(entity);
            }
            return Repository().Update(listEntity);//修改数据库
        }
        #endregion
        #endregion
    }
}
