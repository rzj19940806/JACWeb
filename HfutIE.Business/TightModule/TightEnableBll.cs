//=====================================================================================
// All Rights Reserved , Copyright @ HfutIE 2021
// Software Developers @ HfutIE 2021
//=====================================================================================

using HfutIE.DataAccess;
using HfutIE.Entity;
using HfutIE.Repository;
using HfutIE.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Reflection.Emit;
using System.Text;

namespace HfutIE.Business
{
    /// <summary>
    /// 工位基础信息表
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.03.23 21:51</date>
    /// </author>
    /// </summary>
    public class TightEnableBll : RepositoryFactory<Tg_JobEnableConfig>
    {
        #region 全局变量定义区

        //定义本页面主要操作的表的表名，称为主表
        string tableName = "Tg_JobEnableConfig";//===复制时需要修改===
        #endregion

        #region 3.编辑方法
        //将修改后的实体跟新到数据库中
        //返回值为1，或者0
        //1表示操作成功，0表示操作失败
        public int Update(Tg_JobEnableConfig entity) //===复制时需要修改===
        {
            return Repository().Update(entity); //将修改后的实体跟新到数据库中
        }
        #endregion

        #region 4.检查是否已存在JOB使能配置关系
        /// <summary>
        ///   Enabled = 1的数据中某个字段（KeyName）的字段值（KeyValue）是否存在
        /// </summary>
        /// <param name="KeyName">字段名</param>
        /// <param name="KeyValue">字段值</param>
        /// <returns>返回的是搜索的表中包含该字段值的记录条数</returns>
        public int CheckCount(string KeyName, string KeyValue)
        {
            string sql = @"select * from Tg_JobEnableConfig where code='" + KeyName + "' and WJCId='" + KeyValue + "'";
            DataTable count = Repository().FindTableBySql(sql);
            int a = count.Rows.Count;
            return a;
        }
        public int CheckCount2(string KeyName, string KeyValue)
        {
            string sql = "";
            if(KeyName== "tg_wcjobconfig")
            {
                sql = @"select * from tg_wcjobconfig where wcjobcd='" + KeyValue + "'";

            }
            else if (KeyName == "物料")
            {
                sql = @"select * from BBdbR_MatBase where MatCd='" + KeyValue + "'";

            }
            else 
            {
                sql = @"select * from BBdbR_ProductBase where MatCd='" + KeyValue + "'";

            }
            DataTable count = Repository().FindTableBySql(sql);
            int a = count.Rows.Count;
            return a;
        }
        public int hasChildNode(string WcId)
        {
            string sql = @"select * from BBdbR_PostBase where WcId = '" + WcId + "'";
            DataTable count = Repository().FindTableBySql(sql);
            int a = count.Rows.Count;
            return a;
        }
        #endregion


        #region 6.删除方法
        //array 需要删除的信息的主键的数组
        //删除表中某一条数据表示将表中该条数据的Enabled设置为0，并不是真的删除该数据
        //返回值为1，或者0
        //1表示操作成功，0表示操作失败
        public int DeleteUseEnabled(string id)
        {
            //创建一个表格的list，用于存储通过主键查询到的信息
            List<Tg_JobEnableConfig> listEntity = new List<Tg_JobEnableConfig>(); //===复制时需要修改===            
                                                                    //===复制时需要修改===

            return Repository().Delete(listEntity);//修改数据库
        }
        #endregion

        #region 7.查询方法，需要修改sql语句
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
        public DataTable GetPageListByCondition(string Condition, string keywords,string WcCode, JqGridParam jqgridparam) //===复制时需要修改===查询JOB工位信息
        {
            string sql = "";
            if (Condition == "All")
            {
                sql = @"select * from tg_wcjobconfig  order by wcJobcd asc";
                return Repository().FindTableBySql(sql,false);
            }
            else
            {
                if(Condition=="VIN")//通过JOB使能配置查询JOB信息
                {
                    string str = $"select MatCd,CarType from P_ProducePlan_Pro where VIN like '%{keywords}'";
                    DataTable dt = Repository().FindTableBySql(str, false);
                    if (dt.Rows.Count > 0)
                    {
                        string ProCd = dt.Rows[0]["MatCd"].ToString();
                        string CarType = (string)dt.Rows[0][1];
                        //综合查找
                        StringBuilder sb = new StringBuilder();
                        sb.Append(@"SELECT DISTINCT WcJobCd FROM TightConfigView WHERE Type='产品' AND CHARINDEX(Code,'" + ProCd + "')>0 AND WcCd='" + WcCode + "' UNION ALL ");
                        sb.Append(@"SELECT DISTINCT WcJobCd FROM TightConfigView a JOIN produceBom b ON a.Code=b.MatCd  WHERE Type='图号' AND b.ProductCd='" + ProCd + "' AND a.WcCd='" + WcCode + "' UNION ALL ");
                        sb.Append(@"SELECT DISTINCT WcJobCd FROM TightConfigView  WHERE Type='车型' AND Code='" + CarType + "' AND WcCd='" + WcCode + "'  UNION ALL ");
                        sb.Append(@"SELECT DISTINCT WcJobCd from TightConfigView c JOIN dbo.produceBom d ON c.RsvFld1=d.MatCd WHERE Type='车型+图号' AND d.ProductCd='" + ProCd + "' AND c.Code='" + CarType + "' AND c.WcCd='" + WcCode + "' ");
                        DataTable Configtable = Repository().FindTableBySql(sb.ToString(), false);
                        if(Configtable.Rows.Count > 0)
                        {
                            string str1 = "";
                            for (int i = 0; i < Configtable.Rows.Count; i++)
                            {
                                if(i!= Configtable.Rows.Count - 1)
                                {
                                    str1 += $"'{Configtable.Rows[i][0]}',";
                                }
                                else
                                {
                                    str1 += $"'{Configtable.Rows[i][0]}'";
                                }
                            }
                            string str2 = "select * from tg_wcjobconfig where WcJobCd in ( ";
                            str2 += str1;
                            str2 += " ) order by wcJobcd asc";
                            return Repository().FindTableBySql(str2, false);
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        return null;
                    }
                 
                }
                else
                {
                    sql = @"select * from tg_wcjobconfig where "+ Condition + " like '%"+ keywords + "%' order by wcJobcd asc";
                    return Repository().FindTableBySql(sql, false);
                }
            }
        }
        public DataTable GetPageListByConditionEnable(string Condition, string keywords,string WcCode, JqGridParam jqgridparam) //===复制时需要修改===查询JOB使能信息
        {
            string sql = "";
            if (Condition == "VIN")
            {
                string str = $"select MatCd,CarType from P_ProducePlan_Pro where VIN like '%{keywords}'";
                DataTable dt = Repository().FindTableBySql(str, false);
                if (dt.Rows.Count > 0)
                {
                    string ProCd = dt.Rows[0]["MatCd"].ToString();
                    string CarType = (string)dt.Rows[0][1];
                    //综合查找
                    StringBuilder sb = new StringBuilder();
                    sb.Append(@"SELECT DISTINCT WcJobCd AS WJCId,Type,Code,RsvFld1 FROM TightConfigView WHERE Type='产品' AND CHARINDEX(Code,'" + ProCd + "')>0 AND WcCd='" + WcCode + "' UNION ALL ");
                    sb.Append(@"SELECT DISTINCT WcJobCd AS WJCId,Type,Code,a.RsvFld1 FROM TightConfigView a JOIN produceBom b ON a.Code=b.MatCd  WHERE Type='图号' AND b.ProductCd='" + ProCd + "' AND a.WcCd='" + WcCode + "' UNION ALL ");
                    sb.Append(@"SELECT DISTINCT WcJobCd AS WJCId,Type,Code,RsvFld1 FROM TightConfigView  WHERE Type='车型' AND Code='" + CarType + "' AND WcCd='" + WcCode + "'  UNION ALL ");
                    sb.Append(@"SELECT DISTINCT WcJobCd AS WJCId,Type,Code,c.RsvFld1 from TightConfigView c JOIN dbo.produceBom d ON c.RsvFld1=d.MatCd WHERE Type='车型+图号' AND d.ProductCd='" + ProCd + "' AND c.Code='" + CarType + "' AND c.WcCd='" + WcCode + "' ");
                    return Repository().FindTableBySql(sb.ToString(), false);
                    
                }
                else
                {
                    return null;
                }
            }
            else
            {
                sql = @"select * from Tg_JobEnableConfig where WJCId = '" + keywords + "' order by CHARINDEX(RTRIM(CAST(Type as NCHAR)),'产品,车型+图号,车型,图号') ";
                return Repository().FindTableBySql(sql, false);
            }
        }
        #endregion

        #region 8.编辑弹框中数据源
        /// <summary>
        /// 编辑弹框中数据源
        /// </summary>
        /// <returns></returns>
        public Tg_JobEnableConfig GetPlanList1(string KeyValue)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT  * FROM  BBdbR_WcBase where  WcId='" + KeyValue + "'");
            List<Tg_JobEnableConfig> dt = Repository().FindListBySql(strSql.ToString());
            Tg_JobEnableConfig Dvcentity = new Tg_JobEnableConfig();         
            Dvcentity = dt[0];
            return Dvcentity;
        }
        #endregion

        #region 9.获得导出模板
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
        #region 4.在表单中加载及搜索没配置过的实体清单
        public DataTable GetNotConfig(string WcId, string Condition, string keywords, ref JqGridParam jqgridparam)//在表单中加载没配置过的组件清单
        {
            StringBuilder strSql = new StringBuilder();
            if (Condition == "" || Condition == null)//加载物料图号（从BOM表中获取所有物料）
            {
                strSql.Append(@"select MatId,a.Code ,a.EtName ,a.type as Type,'' as Rem from 
( select  distinct MatCd as Code,MatId,MatNm as EtName,'图号' as type from produceBom )
as a where a.Code not in (select Code from  Tg_JobEnableConfig b  where b.WJCId='" + WcId + "')");
                if (keywords != "" && keywords != null)
                {
                    strSql.Append(" and  a.Code like '%" + keywords + "%'");
                }

            }
            else if (Condition == "Num")//加载物料图号
            {
                strSql.Append(@"select MatId,a.Code ,a.EtName ,a.type as Type,'' as Rem from ( select MatId, MatCd as Code,MatNm as EtName,'图号' as type,Rem as Rem  from BBdbR_MatBase where Enabled='1' )as a where a.Code not in (select Code from  Tg_JobEnableConfig b  where b.WJCId='" + WcId + "') ");
                if (keywords != "" && keywords != null)
                {
                    strSql.Append(" and  a.Code like '%" + keywords + "%'");
                }

            }
            else if (Condition == "Prod")
            {
                strSql.Append(@"select MatId,a.Code ,a.EtName ,a.type as Type,''as Rem from (  select MatId, MatCd as Code,MatNm as EtName,'产品' as type,Notification as Rem  from BBdbR_ProductBase where Enabled='1' )as a where a.Code not in (select Code from  Tg_JobEnableConfig b  where b.WJCId='" + WcId + "') ");
                if (keywords != "" && keywords != null)
                {
                    strSql.Append(" and  a.Code like '%" + keywords + "%'");
                }
            }
            else //if (Condition == "车型"|| Condition == "车型&图号")
            {
                if (Condition == "车型 图号")
                {
                    Condition = "车型+图号";
                }
                strSql.Append(@"select ROW_NUMBER() over (order by code ) MatId,Code ,EtName ,type as Type,''as Rem from 
                                 (SELECT  DISTINCT CarType as Code,CarType as EtName, '"+ Condition + "' as Type,'' as Rem from dbo.P_ProducePlan_Pro   ) as a order by code ");
            }
            return Repository().FindTableBySql(strSql.ToString(), false);
        }


        #endregion

        #region 5.提交配置
        public int JOBEnableInsert(Tg_JobEnableConfig entity)
        {
            return Repository().Insert(entity);
            //return 1;
        }
        #endregion

        #region 6.删除配置
        public int ConfigDelete(string KeyValue)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(@"delete Tg_JobEnableConfig where ID ='" + KeyValue + "'");
            int isok = Repository().ExecuteBySql(sql);
            return isok;
        }

        #endregion


    }
}