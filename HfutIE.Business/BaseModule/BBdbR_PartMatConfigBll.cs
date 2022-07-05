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
using System.Data.SqlClient;
using System.Text;

namespace HfutIE.Business
{
    /// <summary>
    /// 组件物料配置
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.12.22 20:42</date>
    /// </author>
    /// </summary>
    public class BBdbR_PartMatConfigBll : RepositoryFactory<BBdbR_PartMatConfig>
    {
        #region 全局变量定义区
        //定义本页面主要操作的表的表名，称为主表
        string tableName = "BBdbR_PartMatConfig";//===复制时需要修改===
        #endregion


        #region 1.页面表格
        /// <summary>
        /// 联合查询，展示页面表格
        /// </summary>
        /// <param name="CheckId"></param>
        /// <returns></returns>
        public List<BBdbR_PartMatConfig> GetList(string KeyValue)
        {
            StringBuilder strSql = new StringBuilder();
            List<BBdbR_PartMatConfig> dt;
            strSql.Append(@"SELECT  * FROM  "+tableName+ " where  PartId='" + KeyValue + "' and Enabled='1' ");
            dt = Repository().FindListBySql(strSql.ToString());
            return dt;
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
            string sql = @"select * from " + tableName + " where Enabled = '1' and " + KeyName + "  = '" + KeyValue + "'";
            DataTable count = Repository().FindTableBySql(sql);
            int a = count.Rows.Count;
            return a;
        }
        #endregion

        #region 2.1 检查某个组件编号和物料编号的字段值是否同时存在
        
        public int CheckCount2(string PartCd, string MatCd)
        {
            string sql = @"select * from BBdbR_PartMatConfig where Enabled = '1' and  PartCd = '" + PartCd + "' and MatCd = '" + MatCd + "' ";
            DataTable count = Repository().FindTableBySql(sql);
            int a = count.Rows.Count;
            return a;
        }
        #endregion

        #region 3.新增方法
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
        public int Insert(BBdbR_PartMatConfig entity) //===复制时需要修改===
        {
            //entity.Enabled = "1";

            return Repository().Insert(entity);
        }
        public int update(BBdbR_PartMatConfig entity) //===复制时需要修改===
        {
            //entity.Enabled = "1";

            return Repository().Update(entity);
        }
        #endregion

        #region 4.删除方法
        //array 需要删除的信息的主键的数组
        //删除表中某一条数据表示将表中该条数据的Enabled设置为0，并不是真的删除该数据
        //返回值为1，或者0
        //1表示操作成功，0表示操作失败
        public int Delete(string[] array)
        {
            int isok = 0;
            List<BBdbR_PartMatConfig> listEntity = new List<BBdbR_PartMatConfig>();
            //创建一个表格的list，用于存储通过主键查询到的信息
            //List<BBdbR_CarPartBase> listEntity = new List<BBdbR_CarPartBase>(); //===复制时需要修改===
            foreach (string keyValue in array)
            {
                //===复制时需要修改===
                BBdbR_PartMatConfig entity = Repository().FindEntity(keyValue);//根据主键（keyValue）在数据库中查找实体 //===复制时需要修改===
                    entity.Enabled = "0";
                    listEntity.Add(entity);
            }
            return Repository().Update(listEntity);//修改数据库
        }
        #endregion
    }
}