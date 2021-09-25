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
    /// 缺陷明细表
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.08.31 16:55</date>
    /// </author>
    /// </summary>
    public class BBdbR_DefectCatgDeTailBll : RepositoryFactory<BBdbR_DefectCatgDeTail>
    {
        #region 全局变量定义区

        //定义本页面主要操作的表的表名，称为主表
        string tableName = "BBdbR_DefectCatgDeTail";//===复制时需要修改===

        //定义子表表名
        //string subTableName = "";//===复制时需要修改===

        //定义上级表表名
        string superTableName = "BBdbR_DefectCatgBase";//===复制时需要修改===

        //定义上级表主键
        string superID = "DefectCatgId";

        //定义上级表名称
        string superName = "DefectCatgNm";
        #endregion

        #region 1.获取树，需要修改sql语句
        /// <summary>
        ///     keys-------------本节点主键
        ///     code-------------本节点编号
        ///     name-------------本节点名称
        ///     myId-------------本节点编码
        ///     parentId---------父节点编码
        ///     sort-------------节点层级
        /// </summary>
        /// <returns></returns>
        public DataTable GetTree()
        {
            StringBuilder strSql = new StringBuilder();
            //===复制时需要修改===
            strSql.Append(@"select  DefectCatgId AS keys,     
                        DefectCatgCd AS code,
                        DefectCatgNm AS name,
                        Enabled As Enabled,
                        DefectCatgId As myId,
                        '0' as parentId,  
                        '0' as sort    
                        from BBdbR_DefectCatgBase where Enabled = '1' ");
            return Repository().FindTableBySql(strSql.ToString(),false);
        }
        #endregion

        #region 2.点击树展示表格，需要修改sql语句
        /// <summary>
        /// 基本信息：【缺陷类别】   --属于-->   【缺陷明细】
        ///     根据传入的参数不同写不同的sql语句进行查询           
        /// </summary>
        /// <param name="areaId">点击的节点的主键</param>
        /// <param name="sort">点击的节点的层级</param>
        /// <param name="jqgridparam">分页参数</param>
        /// <returns></returns>
        public List<BBdbR_DefectCatgDeTail> GetList(string areaId, string sort, ref JqGridParam jqgridparam) //===复制时需要修改===
        {
            List<BBdbR_DefectCatgDeTail> listEntity = new List<BBdbR_DefectCatgDeTail>();//===复制时需要修改===
            string sql = "";
            if (sort == "30")
            {
                //从本表中查询上级表主键与传入主键相同相等的数据，并返回列表
                sql = "select * from " + tableName + " where " + superID + " = " + areaId + " and Enabled = '1' ";     //===复制时需要修改===
                listEntity = Repository().FindList(sql); //执行sql语句
            }
            return Repository().FindListPageBySql(sql, ref jqgridparam);
        }
        #endregion

        #region 3.展示表格
        /// <summary>
        /// 搜索表格中所有IsAvailable = true的数据
        /// </summary>
        /// <param name="jqgridparam">分页参数</param>
        /// <returns>返回搜索到的数据</returns>
        public List<BBdbR_DefectCatgDeTail> GetPageList(JqGridParam jqgridparam) //===复制时需要修改===
        {
            return Repository().FindList("Enabled", "1");
        }
        #endregion

        #region 4.检查某个字段的字段值是否存在
        /// <summary>
        ///   检查表中IsAvailable = true的数据中某个字段（KeyName）的字段值（KeyValue）是否存在
        /// </summary>
        /// <param name="KeyName">字段名</param>
        /// <param name="KeyValue">字段值</param>
        /// <returns>返回的是搜索的表中包含该字段值的记录条数</returns>
        public int CheckCount(string KeyName, string KeyValue)
        {
            string sql = @"select * from " + tableName + " where Enabled = '1' and " + KeyName + " = " + KeyValue + "";
            int count = Repository().FindCountBySql(sql);
            return count;
        }
        #endregion

        #region 5.查询方法，需要修改sql语句
        //查询时提供了两个关键字，一个是Condition，另一个是keywords
        //Condition是关键字，即查询条件，对应数据库中的一个字段
        //keywords是查询值，即查询条件的具体值，对应数据库中查询条件字段的值
        //根据不同的关键字(Condition)，用字符串拼接的方式拼接不同的sql语句，进行近似查询（like）。

        //该方法在查询和展示表格数据的时候使用
        //查询的时候传递了Condition和keywords，展示表格时未传递参数
        public List<BBdbR_DefectCatgDeTail> GetPageListByCondition(string keywords, string Condition, JqGridParam jqgridparam) //===复制时需要修改===
        {
            string sql = "";
            if (Condition == "all")
            {
                sql = @"select * from " + tableName + " where IsAvailable = 'true'";
                return Repository().FindListBySql(sql);
            }
            else
            {
                //根据条件查询
                sql = @"select * from " + tableName + " where Enabled = '1' and " + Condition + " like  '%" + keywords + "%'";
                return Repository().FindListPageBySql(sql.ToString(), ref jqgridparam);
            }


        }
        #endregion

        #region 6.编辑方法

        #endregion

        #region 7.新增方法

        #endregion

        #region 8.删除时使用，查找本表中某一条数据下级表中是否绑定了数据

        #endregion

        #region 9.删除方法

        #endregion

    }
}