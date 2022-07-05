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
using System.Linq;
using System.Text;

namespace HfutIE.Business
{
    /// <summary>
    /// 车身质量检查销项过程表
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.12.25 10:08</date>
    /// </author>
    /// </summary>
    public class Q_CarQualityOutput_ProBll : RepositoryFactory<Q_CarQualityOutput_Pro>
    {
        #region 全局变量定义区
        string tableName = "Q_CarQualityOutput_Pro";
        #endregion

        #region 搜索
        public DataTable GetPageListByCondition(string QualityCheckPointGroupNm,string QualityCheckPointNm, string CarComponentNm, string DefectNm,  string OutputResult, string ReinspectionNumber,string VIN, string CarType, string TimeStart, string TimeEnd, JqGridParam jqgridparam) //===复制时需要修改===
        {
            string sql = $"select a.*,CarType from Q_CarQualityOutput_Pro a left join Q_CarQualityInspection_Pro b on a.CarQualityInspectionId =b.CarQualityInspectionId where a.Enabled = '1' and  a.QualityCheckPointGroupNm  like '%{QualityCheckPointGroupNm}%' and a.QualityCheckPointNm like '%{QualityCheckPointNm}%' and a.CarComponentNm like '%{CarComponentNm}%' and a.DefectNm like '%{DefectNm}%'  and a.OutputResult like '%{OutputResult}%' and a.ReinspectionNumber like '%{ReinspectionNumber}%'  and a.VIN like '%{VIN}%' and CarType like '%{CarType}%' ";
            ////开始时间
            //if (TimeStart != "" && TimeStart != null)
            //{
            //    sql+=$" and DATEDIFF(day,'{TimeStart}',a.QualityInspectTm) >= 0 " ;
            //}
            //else { }

            ////结束时间
            //if (TimeEnd != "" && TimeEnd != null)
            //{
            //    sql += $" and DATEDIFF(day,a.QualityInspectTm,'{TimeEnd}') >= 0 ";
            //}
            //else { }

            List<DbParameter> parameter = new List<DbParameter>();
            //开始时间
            if (!String.IsNullOrEmpty(TimeStart))
            {
                //开始时间把@放在前面
                sql += $" and DateDiff(day,@TimeStart,a.QualityInspectTm) >=0 ";
                parameter.Add(DbFactory.CreateDbParameter("@TimeStart", TimeStart));
            }
            //结束时间
            if (!String.IsNullOrEmpty(TimeEnd))
            {
                //结束时间把@放在后面
                sql += $" and DateDiff(day,a.QualityInspectTm,@TimeEnd) >=0 ";
                parameter.Add(DbFactory.CreateDbParameter("@TimeEnd", TimeEnd));
            }


            sql += $" order by a.QualityInspectTm desc ";
            //return Repository().FindTableBySql(sql,false);
            return DataFactory.Database().FindTableBySql(sql, parameter.ToArray(), false);
        }
        #endregion

        #region 搜索结果信息
        public DataTable GetResultPageListByCondition(string QualityCheckPointGroupNm,  string VIN, string CarType, string TimeStart, string TimeEnd, JqGridParam jqgridparam) //===复制时需要修改===
        {
            string sql = $"select a.*,b.CarColor1,b.CarType from Q_CarQualityResult_Pro a left join P_ProducePlan_Pro b on a.VIN =b.VIN where  a.Enabled = '1' and a.QualityCheckPointGroupNm  like '%{QualityCheckPointGroupNm}%' and a.VIN like '%{VIN}%' and CarType like '%{CarType}%' ";
            ////开始时间
            //if (TimeStart != "" && TimeStart != null)
            //{
            //    sql += $" and DATEDIFF(day,'{TimeStart}',a.QualityInspectTm) >= 0 ";
            //}
            //else { }

            ////结束时间
            //if (TimeEnd != "" && TimeEnd != null)
            //{
            //    sql += $" and DATEDIFF(day,a.QualityInspectTm,'{TimeEnd}') >= 0 ";
            //}
            //else { }
            List<DbParameter> parameter = new List<DbParameter>();
            //开始时间
            if (!String.IsNullOrEmpty(TimeStart))
            {
                //开始时间把@放在前面
                sql += $" and DateDiff(day,@TimeStart,a.QualityInspectTm) >=0 ";
                parameter.Add(DbFactory.CreateDbParameter("@TimeStart", TimeStart));
            }
            //结束时间
            if (!String.IsNullOrEmpty(TimeEnd))
            {
                //结束时间把@放在后面
                sql += $" and DateDiff(day,a.QualityInspectTm,@TimeEnd) >=0 ";
                parameter.Add(DbFactory.CreateDbParameter("@TimeEnd", TimeEnd));
            }

            sql += $" order by a.QualityInspectTm desc ";
            //return Repository().FindTableBySql(sql, false);
            return DataFactory.Database().FindTableBySql(sql, parameter.ToArray(), false);

        }
        #endregion

        #region 导出搜索
        public DataTable GetExcel_Data(string QualityCheckPointGroupNm,string QualityCheckPointNm, string CarComponentNm, string DefectNm, string OutputResult, string ReinspectionNumber, string VIN, string CarType, string TimeStart, string TimeEnd, JqGridParam jqgridparam) //===复制时需要修改===
        {
            string sql = $"select a.QualityCheckPointGroupNm,a.QualityCheckPointNm,a.VIN,CarType,a.CarComponentNm,a.DefectNm,a.DefectAnalysis,a.OutputResult,a.ReinspectionNumber,a.StfCd,a.StfNm,a.QualityInspectTm,a.WStfCd,a.WStfNm,a.WxTm,a.XStfCd,a.XStfNm,a.OutputTime from {tableName} a left join Q_CarQualityInspection_Pro b on a.CarQualityInspectionId =b.CarQualityInspectionId where a.Enabled = '1' and  a.QualityCheckPointGroupNm like '%{QualityCheckPointGroupNm}%' and a.QualityCheckPointNm like '%{QualityCheckPointNm}%' and a.CarComponentNm like '%{CarComponentNm}%' and a.DefectNm like '%{DefectNm}%' and a.OutputResult like '%{OutputResult}%' and a.ReinspectionNumber like '%{ReinspectionNumber}%'  and a.VIN like '%{VIN}%'  and CarType like '%{CarType}%' ";
            ////开始时间
            //if (TimeStart != "" && TimeStart != null)
            //{
            //    sql += $" and DATEDIFF(day,'{TimeStart}',a.QualityInspectTm) >= 0 ";
            //}
            //else { }

            ////结束时间
            //if (TimeEnd != "" && TimeEnd != null)
            //{
            //    sql += $" and DATEDIFF(day,a.QualityInspectTm,'{TimeEnd}') >= 0 ";
            //}
            List<DbParameter> parameter = new List<DbParameter>();
            //开始时间
            if (!String.IsNullOrEmpty(TimeStart))
            {
                //开始时间把@放在前面
                sql += $" and DateDiff(day,@TimeStart,a.QualityInspectTm) >=0 ";
                parameter.Add(DbFactory.CreateDbParameter("@TimeStart", TimeStart));
            }
            //结束时间
            if (!String.IsNullOrEmpty(TimeEnd))
            {
                //结束时间把@放在后面
                sql += $" and DateDiff(day,a.QualityInspectTm,@TimeEnd) >=0 ";
                parameter.Add(DbFactory.CreateDbParameter("@TimeEnd", TimeEnd));
            }
            sql += $" order by a.QualityInspectTm desc ";
            //return Repository().FindTableBySql(sql, false);
            return DataFactory.Database().FindTableBySql(sql, parameter.ToArray(), false);

        }
        #endregion

        #region 2.高级检索
        public DataTable GridPageStockTryList(List<ConditionAndKey> condition, DateTime? starttime, DateTime? endtime, ref JqGridParam jqgridparam)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(@"select CarInspectionOutputId,CarQualityInspectionId,QualityCheckPointId,QualityCheckPointCd,QualityCheckPointNm,VIN,CarComponentId,CarPositionId,CarPositionGroupId,CarComponentCd,CarComponentNm,CarComponentSimpleCd,DefectId,DefectCatgId,DefectCatgGroupId,DefectCd,DefectNm,DefectAnalysis,StfCd,StfNm,QualityInspectTm,WStfCd,WStfNm, WxTm,OutputValue,OutputResult,XStfCd,XStfNm,OutputTime,ReinspectionNumber,HistoryId,isFile,CreTm,CreCd,CreNm,MdfTm,MdfCd,MdfNm,Rem,RsvFld1,RsvFld2 from " + tableName + " where OutputTime<='" + endtime + "' and OutputTime>='" + starttime + "' ");
            foreach (var item in condition)
            {
                switch (item.Condition)
                {

                    case "QualityCheckPointCd":
                        sql.Append(" and QualityCheckPointCd");
                        break;
                    case "QualityCheckPointNm":
                        sql.Append(" and QualityCheckPointNm");
                        break;
                    case "VIN":
                        sql.Append(" and VIN");
                        break;
                    case "CarComponentCd":
                        sql.Append(" and CarComponentCd");
                        break;
                    case "CarComponentNm":
                        sql.Append(" and CarComponentNm");
                        break;
                    case "CarComponentSimpleCd":
                        sql.Append(" and CarComponentSimpleCd");
                        break;
                    case "DefectCd":
                        sql.Append(" and DefectCd");
                        break;
                    case "DefectNm":
                        sql.Append(" and DefectNm");
                        break;
                    case "DefectAnalysis":
                        sql.Append(" and DefectAnalysis");
                        break;
                    case "StfCd":
                        sql.Append(" and StfCd");
                        break;
                    case "StfNm":
                        sql.Append(" and StfNm");
                        break;
                    case "WStfCd":
                        sql.Append(" and WStfCd");
                        break;
                    case "WStfNm":
                        sql.Append(" and WStfNm");
                        break;
                    case "OutputValue":
                        sql.Append(" and OutputValue");
                        break;
                    case "OutputResult":
                        sql.Append(" and OutputResult");
                        break;
                    case "XStfCd":
                        sql.Append(" and XStfCd");
                        break;
                    case "XStfNm":
                        sql.Append(" and XStfNm");
                        break;
                    case "ReinspectionNumber":
                        sql.Append(" and ReinspectionNumber");
                        break;
                }
                switch (item.ExpressExtension)
                {
                    case "like":
                        sql.Append(" like '%" + item.Keywords + "%'");
                        break;
                    case "=":
                        sql.Append(" ='" + item.Keywords + "'");
                        break;
                }
            }
            return Repository().FindTableBySql(sql.ToString(),false);
        }
        #endregion

        #region 下拉框
        public DataTable getQualityCheckPointNm()
        {
            DataTable QualityCheckPointNm = Repository().FindTableBySql($"select wcnm from BBdbR_WcBase A join BBdbR_PlineBase B on A.PlineId=B.PlineId and WorkSectionId ='065ee432-0799-4b8d-82a3-8c4b9ad92fe2' and A.Enabled=1 and B.Enabled=1 order by B.Sort,A.Sort");
            return QualityCheckPointNm;
        }
        #endregion

        #region 获得导出模板
        /// <summary>
        /// 导出模板
        /// </summary>
        /// <param name="ImportId"></param>
        /// <param name="data"></param>
        /// <param name="DataColumn"></param>
        /// <param name="fileName"></param>
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

        #region 根据结果表主键删除主键下的所有缺陷记录
        public int deleteByResult(string CarQualityResultId) //===复制时需要修改===
        {
            string sql = "";
            List<Q_CarQualityOutput_Pro> dtList;
            sql = @"select a.* from Q_CarQualityOutput_Pro a left join Q_CarQualityInspection_Pro b on a.CarQualityInspectionId = b.CarQualityInspectionId where a.Enabled = '1' and b.CarQualityResultId = '" + CarQualityResultId + "' ";
            dtList = Repository().FindListBySql(sql.ToString());
            foreach (var item in dtList)
            {
                item.Enabled = "0";
                //删除
            }
            int num = Repository().Update(dtList);
            return num;//返回影响条数
        }
        #endregion

        #region 根据二级表Q_CarQualityInspection_Pro主键删除主键下的所有缺陷记录
        public int deleteByInspectionId(string CarQualityInspectionId) //===复制时需要修改===
        {
            string sql = "";
            List<Q_CarQualityOutput_Pro> dtList;
            sql = @"select a.* from Q_CarQualityOutput_Pro a left join Q_CarQualityInspection_Pro b on a.CarQualityInspectionId = b.CarQualityInspectionId where a.Enabled = '1' and b.CarQualityInspectionId = '" + CarQualityInspectionId + "' ";
            dtList = Repository().FindListBySql(sql.ToString());
            foreach (var item in dtList)
            {
                item.Enabled = "0";
                //删除
            }
            int num = Repository().Update(dtList);
            return num;//返回影响条数
        }
        #endregion

        #region 根据二级表Q_CarQualityInspection_Pro主键和复检次数删除主键下的所有缺陷记录
        public int deleteByInspectionId_ReinspectionNumber(string CarQualityInspectionId,int ReinspectionNumber) //===复制时需要修改===
        {
            string sql = "";
            List<Q_CarQualityOutput_Pro> dtList;
            sql = @"select a.* from Q_CarQualityOutput_Pro a left join Q_CarQualityInspection_Pro b on a.CarQualityInspectionId = b.CarQualityInspectionId where a.Enabled = '1' and b.CarQualityInspectionId = '" + CarQualityInspectionId + "' and a.ReinspectionNumber > '" + ReinspectionNumber + "' ";
            dtList = Repository().FindListBySql(sql.ToString());
            foreach (var item in dtList)
            {
                item.Enabled = "0";
                //删除
            }
            int num = Repository().Update(dtList);
            return num;//返回影响条数
        }
        #endregion

        #region 删除方法
        //array 需要删除的信息的主键的数组
        //删除表中某一条数据表示将表中该条数据的Enabled设置为0，并不是真的删除该数据
        //返回值为1，或者0
        //1表示操作成功，0表示操作失败
        public int Delete(string KeyValue)
        {

            ////创建一个表格的list，用于存储通过主键查询到的信息
            List<Q_CarQualityOutput_Pro> listEntity = new List<Q_CarQualityOutput_Pro>(); //===复制时需要修改===
            //===复制时需要修改===
            Q_CarQualityOutput_Pro entity = Repository().FindEntity(KeyValue);//根据主键（keyValue）在数据库中查找实体 //===复制时需要修改===
            entity.Enabled = "0";//将该实体的IsAvailable属性改为false
            entity.Modify(KeyValue);
            listEntity.Add(entity);
            return Repository().Update(listEntity);//修改数据库
        }
        #endregion

        #region 根据CarInspectionOutputId获取质量录入信息
        public DataTable GetInfoByOutputId(string CarInspectionOutputId) //===复制时需要修改===
        {
            string sql = $"select c.CarQualityResultId,a.* from Q_CarQualityOutput_Pro a left join Q_CarQualityInspection_Pro b on a.CarQualityInspectionId = b.CarQualityInspectionId left join Q_CarQualityResult_Pro c on b.CarQualityResultId = c.CarQualityResultId where a.Enabled = '1' and b.Enabled = '1' and c.Enabled = '1' and a.CarInspectionOutputId = '{CarInspectionOutputId}'";
            return Repository().FindTableBySql(sql,false);
        }
        #endregion

    }
}