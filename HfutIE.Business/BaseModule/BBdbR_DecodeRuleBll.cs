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
    /// BBdbR_DecodeRule
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.09.14 19:38</date>
    /// </author>
    /// </summary>
    public class BBdbR_DecodeRuleBll : RepositoryFactory<BBdbR_DecodeRule>
    {
        #region 全局变量定义区
        //定义本页面主要操作的表的表名，称为主表
        string tableName = "BBdbR_DecodeRule";//===复制时需要修改===
        #endregion
        #region 1.展示表格
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
                sql = @"select * from " + tableName + "  where BarId='" + keywords + "' and Enabled=1 order by "+ jqgridparam.sidx +" "+jqgridparam.sord;
                return (Repository().FindTableBySql(sql.ToString(), false));
            }
            else
            {
                return null;
            }
        }

        #endregion
        #region 2.新增方法
        public int Insert(BBdbR_DecodeRule entity) //===复制时需要修改===
        {
            return Repository().Insert(entity);
        }

        public int CheckCount(string Name, string Value)
        {
            string sql = @"select * from " + tableName + " where Enabled = '1' and BarId='" + Name + "' and CombineNm = '" + Value + "'";
            DataTable count = Repository().FindTableBySql(sql);
            int a = count.Rows.Count;
            return a;
        }
        #endregion
        #region 3.编辑方法
        public int Update(BBdbR_DecodeRule entity) //===复制时需要修改===
        {
            return Repository().Update(entity); //将修改后的实体跟新到数据库中
        }

        #endregion
        #region 4.删除方法
        //array 需要删除的信息的主键的数组
        //删除表中某一条数据表示将表中该条数据的Enabled设置为0，并不是真的删除该数据
        //返回值为1，或者0
        //1表示操作成功，0表示操作失败
        public int Delete(string[] array)
        {
            //创建一个表格的list，用于存储通过主键查询到的信息
            List<BBdbR_DecodeRule> listEntity = new List<BBdbR_DecodeRule>(); //===复制时需要修改===
            foreach (string DecodeRulesId in array)
            {
                //===复制时需要修改===
                BBdbR_DecodeRule entity = Repository().FindEntity(DecodeRulesId);//根据主键（keyValue）在数据库中查找实体 //===复制时需要修改===
                entity.Enabled = "0";//将该实体的IsAvailable属性改为false
                listEntity.Add(entity);
            }
            return Repository().Update(listEntity);//修改数据库
        }
        #endregion
        #region 5.查询方法，需要修改sql语句
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
        public List<BBdbR_DecodeRule> GetPageListByCondition(string keywords, string Condition, JqGridParam jqgridparam) //===复制时需要修改===
        {
            string sql = "";
            List<BBdbR_DecodeRule> dt;
            if (Condition == "all")
            {
                sql = @"select * from BBdbR_BarCodeBase where 1 = 1 and Enabled = '1'";
                dt = Repository().FindListBySql(sql.ToString());
            }
            else
            {
                //根据条件查询
                sql = @"select * from BBdbR_BarCodeBase where 1 = 1 and Enabled = '1' and " + Condition + " like  '%" + keywords + "%'";
                dt = Repository().FindListBySql(sql.ToString());
            }
            return dt;
        }
        #endregion
    }
}