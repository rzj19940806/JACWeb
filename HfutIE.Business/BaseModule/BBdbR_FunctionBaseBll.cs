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
    /// 功能模块基本信息表
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.10.09 19:50</date>
    /// </author>
    /// </summary>
    public class BBdbR_FunctionBaseBll : RepositoryFactory<BBdbR_FunctionBase>
    {
        #region 全局变量定义区

        //定义本页面主要操作的表的表名，称为主表
        string tableName = "BBdbR_FunctionBase";//===复制时需要修改===
        #endregion

        #region 1.展示表格
        /// <summary>
        /// 搜索表格中所有IsAvailable = true的数据
        /// </summary>
        /// <param name="jqgridparam">分页参数</param>
        /// <returns>返回搜索到的数据</returns>
        public List<BBdbR_FunctionBase> GetPageList(JqGridParam jqgridparam) //===复制时需要修改===
        {
            string sql = @"select * from " + tableName + " where Enable = '1'";
            return Repository().FindListPageBySql(sql, ref jqgridparam);
        }
        #endregion
        #region 2.检查某个字段的字段值是否存在
        /// <summary>
        ///   Enabled = 1的数据中某个字段（KeyName）的字段值（KeyValue）是否存在
        /// </summary>
        /// <param name="KeyName">字段名</param>
        /// <param name="KeyValue">字段值</param>
        /// <returns>返回的是搜索的表中包含该字段值的记录条数</returns>
        public int CheckCount(string KeyName, string KeyValue)
        {
            string sql = @"select * from " + tableName + " where Enable = '1' and  " + KeyName + "  = '" + KeyValue + "'";
            DataTable count = Repository().FindTableBySql(sql);
            int a = count.Rows.Count;
            return a;
        }
        #endregion

        #region 3.新增方法
        public int Insert(BBdbR_FunctionBase entity) //===复制时需要修改===
        {
            return Repository().Insert(entity);
        }
        #endregion

        #region 4.编辑方法
        public int Update(BBdbR_FunctionBase entity) //===复制时需要修改===
        {
            return Repository().Update(entity);
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
            List<BBdbR_FunctionBase> listEntity = new List<BBdbR_FunctionBase>(); //===复制时需要修改===
            foreach (string keyValue in array)
            {
                //===复制时需要修改===
                BBdbR_FunctionBase entity = FindEntity(keyValue);//根据主键（keyValue）在数据库中查找实体 //===复制时需要修改===
                entity.Enable = "0";//将该实体的IsAvailable属性改为false
                listEntity.Add(entity);
            }
            return Repository().Update(listEntity);//修改数据库
        }
        #endregion

        #region 6.编辑界面填充
        /// <summary>
        /// 点击AVI站点基础信息，查询AVI去向配置信息表
        /// 查询的时候传递了keywords
        /// </summary>
        /// <param name="keywords">查询值</param>
        /// <param name="jqgridparam">分页参数</param>
        /// <returns></returns>
        public BBdbR_FunctionBase SetConfigInfor(string KeyValue) //===复制时需要修改===
        {
            string sql = "";
            BBdbR_FunctionBase entity = new BBdbR_FunctionBase();
            sql = @"select * from " + tableName + " where Enable=1 and FunctionId='" + KeyValue + "'";
            DataTable dt = Repository().FindTableBySql(sql.ToString(), false);
            if (dt.Rows.Count > 0)
            {
                entity.FunctionId = dt.Rows[0]["FunctionId"].ToString();
                entity.FunctionCd = dt.Rows[0]["FunctionCd"].ToString();
                entity.FunctionNm = dt.Rows[0]["FunctionNm"].ToString();
                entity.FunctionType = int.Parse(dt.Rows[0]["FunctionType"].ToString());
                entity.FunctionDsc = dt.Rows[0]["FunctionDsc"].ToString();
                entity.Rem = dt.Rows[0]["Rem"].ToString();
            }
            return entity;
        }

        #endregion
        #region 7.查找要编辑的项
        public BBdbR_FunctionBase FindEntity(string KeyValue)
        {
            string sql = "";
            BBdbR_FunctionBase entity = new BBdbR_FunctionBase();
            sql = @"select * from " + tableName + " where Enable=1 and FunctionId='" + KeyValue + "'";
            return Repository().FindEntityBySql(sql);
        }
        #endregion
        #region 7.条件查询
        public DataTable GridPageByCondition(string keywords, string Condition, JqGridParam jqgridparam) //===复制时需要修改===
        {
            string sql = "";
            DataTable dt;
            if (Condition=="all")
            {
                 sql = @"select * from " + tableName + " where Enable = '1'";
                dt = Repository().FindTableBySql(sql.ToString(), false);
            }
            else
            {
                //根据条件查询
                sql = @"select * from " + tableName + " where Enable = '1' and " + Condition + " like  '%" + keywords + "%'";
                dt = Repository().FindTableBySql(sql.ToString(), false);
            }
            return dt;
        }
        #endregion
    }
}