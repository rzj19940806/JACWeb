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
    /// 推送任务生成规则配置表
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.08.26 15:14</date>
    /// </author>
    /// </summary>
    public class BBdbR_PushRuleBll : RepositoryFactory<BBdbR_PushRule>
    {
        #region 全局变量定义区
        //定义本页面主要操作的表的表名，称为主表
        string tableName = "BBdbR_PushRule";//===复制时需要修改===
        #endregion

        #region 1.搜索
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
        /// <returns></returns>
        public DataTable  GetPageListByCondition(string keywords, string Condition, JqGridParam jqgridparam) //===复制时需要修改===
        {
            string sql = "";
            if (Condition == "all")
            {
                sql = @"select a.*,b.PlineCd as PlineCd,b.PlineNm as PlineNm,c.WcCd as WcCd,c.WcNm as WcNm from " + tableName + " a left join BBdbR_PlineBase b on a.PlineId=b.PlineId left join BBdbR_WcBase c on a.WcId=c.WcId where a.Enabled=1";
            }
            else
            {
                if (keywords != "all")
                {
                    sql = @"select a.*,b.PlineCd as PlineCd,b.PlineNm as PlineNm,c.WcCd as WcCd,c.WcNm as WcNm from " + tableName + " a left join BBdbR_PlineBase b on a.PlineId=b.PlineId left join BBdbR_WcBase c on a.WcId=c.WcId where a.Enabled=1 and b.Enabled=1 and c.Enabled=1 and a." + Condition + " like  '%" + keywords + "%'";
                    //根据条件查询
                }
                else
                {
                    sql = @"select a.*,b.PlineCd as PlineCd,b.PlineNm as PlineNm,c.WcCd as WcCd,c.WcNm as WcNm from " + tableName + " a left join BBdbR_PlineBase b on a.PlineId=b.PlineId left join BBdbR_WcBase c on a.WcId=c.WcId where a.Enabled=1";

                }              
            }
            return Repository().FindTableBySql(sql.ToString(), false);
        }

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
        /// <returns></returns>
        public DataTable GetPageList() //===复制时需要修改===
        {
            string sql = @"select a.*,b.PlineCd as PlineCd,b.PlineNm as PlineNm,c.WcCd as WcCd,c.WcNm as WcNm from " + tableName + " a left join BBdbR_PlineBase b on a.PlineId=b.PlineId left join BBdbR_WcBase c on a.WcId=c.WcId where a.Enabled=1";
            return Repository().FindTableBySql(sql.ToString(), false);
        }
        #endregion

        #region 2.编辑
        //将修改后的实体跟新到数据库中
        //返回值为1，或者0
        //1表示操作成功，0表示操作失败
        public int Update(BBdbR_PushRule entity) //===复制时需要修改===
        {
            return Repository().Update(entity); //将修改后的实体跟新到数据库中
        }
        #endregion

        #region 3.检查某个字段的值是否存在
        /// <summary>
        ///   检查表中IsAvailable = true的数据中某个字段（KeyName）的字段值（KeyValue）是否存在
        /// </summary>
        /// <param name="KeyName">字段名</param>
        /// <param name="KeyValue">字段值</param>
        /// <returns>返回的是搜索的表中包含该字段值的记录条数</returns>
        public int CheckCount(string tableName, string KeyName, string KeyValue)
        {
            string sql = @"select * from " + tableName + " where  " + KeyName + " = '" + KeyValue + "'";
            DataTable count = Repository().FindTableBySql(sql);
            int a = count.Rows.Count;
            return a;
        }
        #endregion

        #region 4.新增
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
        public int Insert(BBdbR_PushRule entity) //===复制时需要修改===
        {
            return Repository().Insert(entity);
        }
        #endregion

        #region 5.区域下拉框

        //产线
        public DataTable GetPlineNm()
        {
            string sql = @"select PlineId as id, PlineNm from BBdbR_PlineBase where 1=1";
            return Repository().FindTableBySql(sql);
        }

        public DataTable GetWcNm()
        {
            string sql = @"select WcId as id, WcNm from BBdbR_WcBase where 1=1";
            return Repository().FindTableBySql(sql);
        }
        #endregion

        #region 6.删除方法
        //array 需要删除的信息的主键的数组
        //删除表中某一条数据表示将表中该条数据的Enabled设置为0，并不是真的删除该数据
        //返回值为1，或者0
        //1表示操作成功，0表示操作失败
        public int Delete(string[] array)
        {
            //创建一个表格的list，用于存储通过主键查询到的信息
            List<BBdbR_PushRule> listEntity = new List<BBdbR_PushRule>(); //===复制时需要修改===
            foreach (string keyValue in array)
            {
                //===复制时需要修改===
                BBdbR_PushRule entity = Repository().FindEntity(keyValue);//根据主键（keyValue）在数据库中查找实体 //===复制时需要修改===
                entity.Enabled = "0";//将该实体的IsAvailable属性改为false
                listEntity.Add(entity);
            }
            return Repository().Update(listEntity);//修改数据库
        }
        #endregion
    }
}