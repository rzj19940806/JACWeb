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
    /// 调度规则基本信息表
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.08.27 11:19陈克利</date>
    /// </author>
    /// </summary>
    public class BBdbR_SchedulRuleBaseBll : RepositoryFactory<BBdbR_SchedulRuleBase>
    {
        #region 全局变量定义区

        //定义本页面主要操作的表的表名，称为主表
        string tableName = "BBdbR_SchedulRuleBase";//===复制时需要修改===
        #endregion

        #region 1.查询方法，需要修改sql语句
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
        public List<BBdbR_SchedulRuleBase> GetPageListByCondition(string keywords, string Condition, JqGridParam jqgridparam) //===复制时需要修改===
        {
            string sql = "";
            List<BBdbR_SchedulRuleBase> listEntity = new List<BBdbR_SchedulRuleBase>();
            if (Condition == "all")
            {
                sql = @"select * from " + tableName + " where Enabled='1'";
                listEntity = Repository().FindListBySql(sql);

            }
            else
            {
                if (keywords == "all")
                {
                    sql = @"select * from " + tableName + " where Enabled='1'";
                    listEntity = Repository().FindListBySql(sql);

                }
                else
                {
                    //根据条件查询
                    sql = @"select * from " + tableName + " where Enabled='1' and " + Condition + " like  '%" + keywords + "%'";
                    listEntity = Repository().FindListBySql(sql);

                }
            }
            return listEntity;
        }

        /// <summary>
        /// 编辑弹框中数据源
        /// </summary>
        /// <returns></returns>
        public BBdbR_SchedulRuleBase GetPlanList(string KeyValue)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT  * FROM  BBdbR_SchedulRuleBase where  SchedulRuleId='" + KeyValue + "'");
            List<BBdbR_SchedulRuleBase> dt = Repository().FindListBySql(strSql.ToString());
            BBdbR_SchedulRuleBase Dvcentity = new BBdbR_SchedulRuleBase();
            Dvcentity = dt[0];
            return Dvcentity;
        }
        #endregion

        #region 2.编辑
        //将修改后的实体跟新到数据库中
        //返回值为1，或者0
        //1表示操作成功，0表示操作失败
        public int Update(BBdbR_SchedulRuleBase entity) //===复制时需要修改===
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
        public int CheckCount(string KeyName, string KeyValue, string KeyName1, string KeyValue1)
        {
            string sql = @"select * from " + tableName + " where Enabled='1' and " + KeyName + " = '" + KeyValue + "'";
            DataTable count = Repository().FindTableBySql(sql);
            int a = count.Rows.Count;
            if (a == 0)
            {
                sql = @"select * from " + tableName + " where Enabled='1' and  " + KeyName1 + " = '" + KeyValue1 + "'";
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
        public int Insert(BBdbR_SchedulRuleBase entity) //===复制时需要修改===
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
            List<BBdbR_SchedulRuleBase> listEntity = new List<BBdbR_SchedulRuleBase>(); //===复制时需要修改===
            foreach (string keyValue in array)
            {
                //===复制时需要修改===
                BBdbR_SchedulRuleBase entity = Repository().FindEntity(keyValue);//根据主键（keyValue）在数据库中查找实体 //===复制时需要修改===
                entity.Enabled = "0";//将该实体的IsAvailable属性改为false
                listEntity.Add(entity);
            }
            return Repository().Update(listEntity);//修改数据库
        }
        #endregion

        #region 6.AVI站点车身调度规则配置信息表格-未配置
        /// <summary>
        /// 点击AVI站点基础信息，查询AVI去向配置信息表
        /// 查询的时候传递了keywords
        /// </summary>
        /// <param name="keywords">查询值</param>
        /// <param name="jqgridparam">分页参数</param>
        /// <returns></returns>
        public DataTable ReGetConfigList(string keywords, JqGridParam jqgridparam) //===复制时需要修改===
        {
            string sql = "";
            sql = @"select * from BBdbR_SchedulRuleBase where Enabled=1 and SchedulRuleId not in (select distinct(SchedulRuleId) from BBdbR_CarSchedulRuleConfig where Enabled=1 and AviId='" + keywords + "')";
            return (Repository().FindTableBySql(sql.ToString(), false));
        }
        #endregion

        #region 7.AVI车身调度规则配置信息表格-已配置
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
                sql = @"select a.* from BBdbR_SchedulRuleBase a join BBdbR_CarSchedulRuleConfig b on a.SchedulRuleId=b.SchedulRuleId where a.Enabled=1 and b.Enabled=1 and b.AviId='" + keywords + "'";
                return (Repository().FindTableBySql(sql.ToString(), false));
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 查询车身对应的产线
        /// 查询的时候传递了keywords
        /// </summary>
        /// <param name="keywords">查询值</param>
        /// <param name="jqgridparam">分页参数</param>
        /// <returns></returns>
        public DataTable GetPageConfigList(string keywords, JqGridParam jqgridparam) //===复制时需要修改===
        {
            string sql = "";
            if (keywords != "")
            {
                sql = @"select a.*,b.AVIWhereId as AVIWhereId,b.PlineMark as PlineMark,c.AviCd as AviCd,c.AviNm as AviNm from BBdbR_PlineBase a join " + tableName + " b on a.PlineId=b.PlineId join BBdbR_AVIBase c on b.AviId=c.AviId where a.Enabled=1 and b.Enabled=1 and c.Enabled=1 and b.AviId='" + keywords + "'";
                return (Repository().FindTableBySql(sql.ToString(), false));
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region 8.导出模板
        public void GetExcellTemperature(string ImportId, out DataTable data, out string DataColumn, out string fileName)
        {
            DataColumn = "";
            data = new DataTable();
            Base_ExcelImport base_excelimport = DataFactory.Database().FindEntity<Base_ExcelImport>(ImportId);
            fileName = base_excelimport.ImportFileName;
            List<Base_ExcelImportDetail> listBase_ExcelImportDetail = DataFactory.Database().FindList<Base_ExcelImportDetail>("ImportId", ImportId);
            object[] rows = new object[listBase_ExcelImportDetail.Count];
            int i = 0;
            foreach (Base_ExcelImportDetail excelImportDetail in listBase_ExcelImportDetail)
            {
                if (DataColumn == "")
                {
                    DataColumn = DataColumn + excelImportDetail.ColumnName;
                }
                else
                {
                    DataColumn = DataColumn + "|" + excelImportDetail.ColumnName;
                }
                switch (excelImportDetail.DataType)
                {
                    //字符串
                    case "0":
                        data.Columns.Add(excelImportDetail.ColumnName, typeof(string));
                        rows[i] = "";
                        break;
                    //数字
                    case "1":
                        data.Columns.Add(excelImportDetail.ColumnName, typeof(decimal));
                        rows[i] = "";
                        break;
                    //日期
                    case "2":
                        data.Columns.Add(excelImportDetail.ColumnName, typeof(DateTime));
                        rows[i] = DateTime.Now;
                        break;
                    //外键
                    case "3":
                        data.Columns.Add(excelImportDetail.ColumnName, typeof(string));
                        rows[i] = "";
                        break;
                    //唯一识别
                    case "4":
                        data.Columns.Add(excelImportDetail.ColumnName, typeof(string));
                        rows[i] = "";
                        break;
                    default:
                        break;
                }
                i++;
            }
            data.Rows.Add(rows);

        }
        #endregion

    }
}