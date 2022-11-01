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
    public class BBdbR_WcBaseBll : RepositoryFactory<BBdbR_WcBase>
    {
        #region 全局变量定义区

        //定义本页面主要操作的表的表名，称为主表
        string tableName = "BBdbR_WcBase";//===复制时需要修改===
        #endregion

        #region 1.获取树，需要修改sql语句
        /// <summary>
        /// 基本信息：【工位】   --属于-->   【产线】  --属于-->  【工段】  --属于-->  【车间】
        /// </summary>
        /// <returns></returns>
        public DataTable GetTree()
        {
            StringBuilder strSql = new StringBuilder();
            //===复制时需要修改===（三级数）
            //车间节点
            strSql.Append(@"select c.WorkshopId As keys,
                                   c.WorkshopCd As code,
                                   c.WorkshopNm As name,
                                   c.Enabled As IsAvailable,
                                   '0' as parentId,
                                   '0' as sort             
                                   from BBdbR_WorkshopBase c where c.Enabled = '1'");
            //工段节点
            strSql.Append(@"union select 
                                    b.WorkSectionId AS keys,
                                    b.WorkSectionCd AS code, 
                                    b.WorkSectionNm AS name, 
                                    b.Enabled As IsAvailable,
                                    b.WorkshopId as parentId,
                                    '1' as sort 
                                    from BBdbR_WorkSectionBase b,BBdbR_WorkshopBase c where b.WorkshopId = c.WorkshopId and b.Enabled= '1'  ");
           //产线节点
            strSql.Append(@" union select    
                                    a.PlineId AS keys,
                                    a.PlineCd AS code,
                                    a.PlineNm AS name,
                                    a.Enabled As IsAvailable,
                                    a.WorkSectionId AS parentId,
                                    '2' as sort 
                             from  BBdbR_PlineBase a,BBdbR_WorkSectionBase b 
                             where a.WorkSectionId=b.WorkSectionId and a.Enabled = '1' and a.Enabled = '1' order by code asc");
            return Repository().FindTableBySql(strSql.ToString());
        }
        #endregion

        #region 2.点击树展示表格，需要修改sql语句
        /// <summary>
        /// 基本信息：
        ///     根据传入的参数不同写不同的sql语句进行查询           
        /// </summary>
        /// <param name="areaId">点击的节点的主键</param>
        /// <param name="parentId">节点的父级主键</param>
        /// <param name="jqgridparam">分页参数</param>
        /// <returns></returns>
        public DataTable GetList(string areaId,string parentId,string sort, ref JqGridParam jqgridparam) //===复制时需要修改===
        {
            string sql = "";
            DataTable dt = new DataTable();
            var a = string.IsNullOrEmpty(areaId) && string.IsNullOrEmpty(parentId);
            if (string.IsNullOrEmpty(areaId) && string.IsNullOrEmpty(parentId))
            {
                sql = "select a.*,b.PlineCd as PlineCd,b.PlineNm as PlineNm from " + tableName + " a join BBdbR_PlineBase b on a.PlineId=b.PlineId where a.Enabled=1 and b.Enabled=1 order by a.sort asc";     //===复制时需要修改===
            }
            else
            {
                if (parentId != "0")
                {
                    if (sort == "0")
                    {
                        sql = "select a.*,b.PlineCd as PlineCd,b.PlineNm as PlineNm from " + tableName + " a join BBdbR_PlineBase b on a.PlineId=b.PlineId join BBdbR_WorkSectionBase c on b.WorkSectionId=c.WorkSectionId where a.Enabled=1 and b.Enabled=1 and c.Enabled=1 and c.WorkSectionId='" + areaId + "' order by a.sort asc";     //===复制时需要修改===
                    }
                    else if (sort == "1")
                    {
                        sql = "select a.*,b.PlineCd as PlineCd,b.PlineNm as PlineNm from " + tableName + " a join BBdbR_PlineBase b on a.PlineId=b.PlineId join BBdbR_WorkSectionBase c on b.WorkSectionId=c.WorkSectionId where a.Enabled=1 and b.Enabled=1 and c.Enabled=1 and c.WorkSectionId='" + areaId + "' order by a.sort asc";     //===复制时需要修改===
                    }
                    else
                    {
                        sql = "select a.*,b.PlineCd as PlineCd,b.PlineNm as PlineNm from " + tableName + " a join BBdbR_PlineBase b on a.PlineId=b.PlineId where a.Enabled=1 and b.Enabled=1 and a.PlineId='" + areaId + "' order by a.sort asc";     //===复制时需要修改===要修改===
                    }
                }
                else
                {
                    sql = "select a.*,b.PlineCd as PlineCd,b.PlineNm as PlineNm from " + tableName + " a join BBdbR_PlineBase b on a.PlineId=b.PlineId join BBdbR_WorkSectionBase c on b.WorkSectionId=c.WorkSectionId join BBdbR_WorkshopBase d on c.WorkshopId=d.WorkshopId where a.Enabled=1 and b.Enabled=1 and c.Enabled=1 and d.Enabled=1 and d.WorkshopId='" + areaId + "' order by a.sort asc";     //===复制时需要修改===
                }
            }
            return Repository().FindTableBySql(sql.ToString(), false);
        }
        #endregion

        #region 3.编辑方法
        //将修改后的实体跟新到数据库中
        //返回值为1，或者0
        //1表示操作成功，0表示操作失败
        public int Update(BBdbR_WcBase entity) //===复制时需要修改===
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
            string sql = @"select * from " + tableName + " where Enabled = '1' and " + KeyName + " = '" + KeyValue + "'";
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

        #region 5.新增方法
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
        public int Insert(BBdbR_WcBase entity) //===复制时需要修改===
        {
            return Repository().Insert(entity);
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
            List<BBdbR_WcBase> listEntity = new List<BBdbR_WcBase>(); //===复制时需要修改===            
                //===复制时需要修改===
                BBdbR_WcBase entity = Repository().FindEntity(id);//根据主键（keyValue）在数据库中查找实体 //===复制时需要修改===
                entity.Enabled = "0";
                listEntity.Add(entity);
            return Repository().Update(listEntity);//修改数据库
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
        public DataTable GetPageListByCondition(string keywords, string Condition, JqGridParam jqgridparam) //===复制时需要修改===
        {
            string sql = "";
            if (Condition == "all")
            {
                sql = @"select a.*,b.PlineCd as PlineCd,b.PlineNm as PlineNm from " + tableName + " a join BBdbR_PlineBase b on a.PlineId=b.PlineId join BBdbR_WorkSectionBase c on b.WorkSectionId=c.WorkSectionId join BBdbR_WorkshopBase d on d.WorkshopId=c.WorkshopId where a.Enabled=1 and b.Enabled=1  order by a.sort asc";
                return Repository().FindTableBySql(sql,false);
            }
            else
            {
                if (keywords != "all")
                {
                    //根据条件查询
                    sql = @"select a.*,b.PlineCd as PlineCd,b.PlineNm as PlineNm from " + tableName + " a join BBdbR_PlineBase b on a.PlineId=b.PlineId join BBdbR_WorkSectionBase c on b.WorkSectionId=c.WorkSectionId join BBdbR_WorkshopBase d on d.WorkshopId=c.WorkshopId where a.Enabled=1 and b.Enabled=1 and a." + Condition + " like  '%" + keywords + "%' order by a.sort asc ";
                    return Repository().FindTableBySql(sql, false);
                }
                else
                {
                    sql = @"select a.*,b.PlineCd as PlineCd,b.PlineNm as PlineNm from " + tableName + " a join BBdbR_PlineBase b on a.PlineId=b.PlineId join BBdbR_WorkSectionBase c on b.WorkSectionId=c.WorkSectionId join BBdbR_WorkshopBase d on d.WorkshopId=c.WorkshopId where a.Enabled=1 and b.Enabled=1 order by a.sort asc";
                    return Repository().FindTableBySql(sql, false);
                }
            }
        }
        //11.获取所有人员
        public DataTable GetPlineNm(string DepartmentID)
        {
            string sql = @"select StfId as id, StfNm from BBdbR_StfBase where Enabled='1' and DepartmentID=" + "'" + DepartmentID + "'";
            return Repository().FindTableBySql(sql);
        }
        //获取人员信息
        public DataTable GetPlineNm2(string StfId)
        {
            string sql = @"select stfnm,phn,email from BBdbR_StfBase where Enabled='1' and StfId=" + "'" + StfId + "'";
            return Repository().FindTableBySql(sql);
        }
        //11.获取部门
        public DataTable GetPlineNm1()
        {
            string sql = @"select DepartmentID as id, DepartmentName from BBdbR_DepartmentBase where Enabled=1";
            return Repository().FindTableBySql(sql);
        }
        #endregion

        #region 8.编辑弹框中数据源
        /// <summary>
        /// 编辑弹框中数据源
        /// </summary>
        /// <returns></returns>
        public BBdbR_WcBase GetPlanList1(string KeyValue)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT  * FROM  BBdbR_WcBase where  WcId='" + KeyValue + "'");
            List<BBdbR_WcBase> dt = Repository().FindListBySql(strSql.ToString());
            BBdbR_WcBase Dvcentity = new BBdbR_WcBase();         
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

        #region 10.查找产线下工位数
        /// <summary>
        /// 查找车间下工艺段数量
        /// </summary>
        /// <returns></returns>
        public int GetWcCount(string KeyValue) //===复制时需要修改===
        {
            string sql = "";
            if (KeyValue != "")
            {
                sql = @"select * from BBdbR_WcBase where Enabled='1' and PlineId='" + KeyValue + "'";
                DataTable dt = Repository().FindTableBySql(sql);
                if (dt.Rows.Count > 0)
                {
                    return Repository().FindTableBySql(sql).Rows.Count;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 1;
            }
        }
        #endregion

        #region 质控点
        #region 1.获取树，需要修改sql语句
        /// <summary>
        /// 基本信息：【工位】   --属于-->   【产线】  --属于-->  【工段】  --属于-->  【车间】
        /// </summary>
        /// <returns></returns>
        public DataTable GetTreeQuality()
        {
            StringBuilder strSql = new StringBuilder();
            //===复制时需要修改===（三级数）
            //车间节点
            strSql.Append(@"select c.WorkshopId As keys,
                                   c.WorkshopCd As code,
                                   c.WorkshopNm As name,
                                   c.Enabled As IsAvailable,
                                   '0' as parentId,
                                   '0' as sort ,c.sort as que             
                                   from BBdbR_WorkshopBase c where c.Enabled = '1' and c.WorkshopCd='ZLCJXN01'  ");
            //工段节点
            strSql.Append(@"union select 
                                    b.WorkSectionId AS keys,
                                    b.WorkSectionCd AS code, 
                                    b.WorkSectionNm AS name, 
                                    b.Enabled As IsAvailable,
                                    b.WorkshopId as parentId,
                                    '1' as sort  ,b.sort as que
                                    from BBdbR_WorkSectionBase b,BBdbR_WorkshopBase c where b.WorkshopId = c.WorkshopId and b.Enabled= '1'  ");
            //产线节点
            strSql.Append(@" union select    
                                    a.PlineId AS keys,
                                    a.PlineCd AS code,
                                    a.PlineNm AS name,
                                    a.Enabled As IsAvailable,
                                    a.WorkSectionId AS parentId,
                                    '2' as sort ,a.sort as que
                             from  BBdbR_PlineBase a,BBdbR_WorkSectionBase b 
                             where a.WorkSectionId=b.WorkSectionId and a.Enabled = '1' and a.Enabled = '1' order by que asc");
            return Repository().FindTableBySql(strSql.ToString());
        }
        #endregion

        #region 2.点击树展示表格，需要修改sql语句
        /// <summary>
        /// 基本信息：
        ///     根据传入的参数不同写不同的sql语句进行查询           
        /// </summary>
        /// <param name="areaId">点击的节点的主键</param>
        /// <param name="parentId">节点的父级主键</param>
        /// <param name="jqgridparam">分页参数</param>
        /// <returns></returns>
        public DataTable GetListQuality(string areaId, string parentId, string sort, ref JqGridParam jqgridparam) //===复制时需要修改===
        {
            string sql = "";
            DataTable dt = new DataTable();
            var a = string.IsNullOrEmpty(areaId) && string.IsNullOrEmpty(parentId);
            if (areaId == "''" && parentId == "''")
            {
                sql = "select a.*,b.PlineCd as PlineCd,b.PlineNm as PlineNm from " + tableName + " a join BBdbR_PlineBase b on a.PlineId=b.PlineId join BBdbR_WorkSectionBase c on b.WorkSectionId=c.WorkSectionId join BBdbR_WorkshopBase d on d.WorkshopId=c.WorkshopId where a.Enabled=1 and b.Enabled=1 and d.WorkshopCd='ZLCJXN01' order by a.sort asc";     //===复制时需要修改===
            }
            else
            {
                if (parentId != "0")
                {
                    if (sort == "0")
                    {
                        sql = "select a.*,b.PlineCd as PlineCd,b.PlineNm as PlineNm from " + tableName + " a join BBdbR_PlineBase b on a.PlineId=b.PlineId join BBdbR_WorkSectionBase c on b.WorkSectionId=c.WorkSectionId where a.Enabled=1 and b.Enabled=1 and c.Enabled=1 and c.WorkSectionId='" + areaId + "' order by a.sort asc";     //===复制时需要修改===
                    }
                    else if (sort == "1")
                    {
                        sql = "select a.*,b.PlineCd as PlineCd,b.PlineNm as PlineNm from " + tableName + " a join BBdbR_PlineBase b on a.PlineId=b.PlineId join BBdbR_WorkSectionBase c on b.WorkSectionId=c.WorkSectionId where a.Enabled=1 and b.Enabled=1 and c.Enabled=1 and c.WorkSectionId='" + areaId + "' order by a.sort asc";     //===复制时需要修改===
                    }
                    else
                    {
                        sql = "select a.*,b.PlineCd as PlineCd,b.PlineNm as PlineNm from " + tableName + " a join BBdbR_PlineBase b on a.PlineId=b.PlineId where a.Enabled=1 and b.Enabled=1 and a.PlineId='" + areaId + "' order by a.sort asc";     //===复制时需要修改===要修改===
                    }
                }
                else
                {
                    sql = "select a.*,b.PlineCd as PlineCd,b.PlineNm as PlineNm from " + tableName + " a join BBdbR_PlineBase b on a.PlineId=b.PlineId join BBdbR_WorkSectionBase c on b.WorkSectionId=c.WorkSectionId join BBdbR_WorkshopBase d on c.WorkshopId=d.WorkshopId where a.Enabled=1 and b.Enabled=1 and c.Enabled=1 and d.Enabled=1 and d.WorkshopId='" + areaId + "' order by a.sort asc";     //===复制时需要修改===
                }
            }
            return Repository().FindTableBySql(sql.ToString(), false);
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
        public DataTable GetPageListByConditionQuality(string keywords, string Condition, JqGridParam jqgridparam) //===复制时需要修改===
        {
            string sql = "";
            if (Condition == "all")
            {
                sql = @"select a.*,b.PlineCd as PlineCd,b.PlineNm as PlineNm from " + tableName + " a join BBdbR_PlineBase b on a.PlineId=b.PlineId join BBdbR_WorkSectionBase c on b.WorkSectionId=c.WorkSectionId join BBdbR_WorkshopBase d on d.WorkshopId=c.WorkshopId where a.Enabled=1 and b.Enabled=1 and d.WorkshopCd='ZLCJXN01' order by a.sort asc";
                return Repository().FindTableBySql(sql, false);
            }
            else
            {
                if (keywords != "all")
                {
                    //根据条件查询
                    sql = @"select a.*,b.PlineCd as PlineCd,b.PlineNm as PlineNm from " + tableName + " a join BBdbR_PlineBase b on a.PlineId=b.PlineId join BBdbR_WorkSectionBase c on b.WorkSectionId=c.WorkSectionId join BBdbR_WorkshopBase d on d.WorkshopId=c.WorkshopId where a.Enabled=1 and b.Enabled=1 and d.WorkshopCd='ZLCJXN01' and a." + Condition + " like  '%" + keywords + "%' order by a.sort asc ";
                    return Repository().FindTableBySql(sql.ToString(), false);
                }
                else
                {
                    sql = @"select a.*,b.PlineCd as PlineCd,b.PlineNm as PlineNm from " + tableName + " a join BBdbR_PlineBase b on a.PlineId=b.PlineId join BBdbR_WorkSectionBase c on b.WorkSectionId=c.WorkSectionId join BBdbR_WorkshopBase d on d.WorkshopId=c.WorkshopId where a.Enabled=1 and b.Enabled=1 and d.WorkshopCd='ZLCJXN01' order by a.sort asc";
                    return Repository().FindTableBySql(sql.ToString(), false);
                }
            }
        }
        #endregion
        #endregion
    }
}