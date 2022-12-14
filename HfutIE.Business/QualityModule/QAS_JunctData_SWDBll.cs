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
    /// 连接点质量结果数据_SWD
    /// <author>
    ///	<name>CHFAS</name>
    ///	<date>2021.07.20 12:00</date>
    /// </author>
    /// </summary>
    public class QAS_JunctData_SWDBll : RepositoryFactory<QAS_JunctData_SWD>
    {
        #region
        #endregion

        #region 全局变量定义区
        //定义本页面主要操作的表的表名，称为主表
        string tableName = "QAS_JunctData_SWD";

        #endregion

        #region 方法区
        public int ImportSWDData(string moduleId, DataTable dt, string BodyNo, string CarType, DateTime CheckTm, out DataTable Result)
        {
            int IsOk = 0;//导入状态
            bool CheckEx = false;
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();

            //构造导入返回结果表
            DataTable Newdt = new DataTable("Result");
            Newdt.Columns.Add("rowid", typeof(System.String));//行号
            Newdt.Columns.Add("locate", typeof(System.String));//位置
            Newdt.Columns.Add("reason", typeof(System.String));//原因
            //Newdt = JudgeOrder(dt, Newdt);//重复性判断
            //bool isExit = Newdt.Rows.Count > 0 ? true : false;
            int errorNum = 1;
            try
            {
                //模板主表
                Base_ExcelImport base_excellimport = DataFactory.Database().FindEntity<Base_ExcelImport>("ModuleId", moduleId);
                if (base_excellimport.ImportId == null)
                {
                    IsOk = 0;
                }
                else
                {
                    string pkName = new Base_DataBaseBll().GetPrimaryKey(base_excellimport.ImportTable);
                    //模板明细表
                    List<Base_ExcelImportDetail> listBase_ExcelImportDetail = DataFactory.Database().FindList<Base_ExcelImportDetail>("ImportId", base_excellimport.ImportId);
                    string tableName = base_excellimport.ImportTable;
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        #region 先删除原始数据
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append("delete QAS_JunctData_SWD where 1=1 and BodyNo = '" + BodyNo + "'");
                        int ext = database.ExecuteBySql(strSql, isOpenTrans);
                        #endregion

                        #region 遍历Excel数据行
                        int rowNum = 1;
                        foreach (DataRow item in dt.Rows)
                        {
                            Hashtable entity = new Hashtable();//最终要插入数据库的hashtable
                            StringBuilder sb = new StringBuilder();
                            entity[pkName] = Guid.NewGuid().ToString();//首先给主键赋值
                            StringBuilder rowSb = new StringBuilder();//累加每一个单元格的值，一行全空就停止插入
                            #region 遍历模板，为每一行中每个字段找到模板列并赋值
                            foreach (Base_ExcelImportDetail excelImportDetail in listBase_ExcelImportDetail)
                            {
                                string value = "";
                                value = item[excelImportDetail.ColumnName].ToString();
                                rowSb.Append(value);//累加每一个单元格的值，一行全空就停止插入
                                DateTime dateTime = DateTime.Now;
                                float num = 0;
                                #region 单个字段赋值
                                switch (excelImportDetail.DataType)
                                {
                                    //字符串
                                    case "0":
                                        entity[excelImportDetail.FieldName] = value;
                                        break;
                                    //数字
                                    case "1":
                                        if (Single.TryParse(value, out num))
                                        {
                                            entity[excelImportDetail.FieldName] = value;
                                        }
                                        else
                                        {
                                            if (base_excellimport.ErrorHanding == "0")
                                            {
                                                CheckEx = true;
                                            }
                                            DataRow dr = Newdt.NewRow();
                                            dr = Newdt.NewRow();
                                            dr[0] = errorNum;
                                            dr[1] = "第[" + rowNum.ToString() + "]行[" + excelImportDetail.ColumnName + "]";
                                            dr[2] = "数字格式不正确";
                                            Newdt.Rows.Add(dr);
                                            errorNum++;
                                            continue;
                                        }
                                        break;
                                    //日期
                                    case "2":
                                        if (DateTime.TryParse(value, out dateTime))
                                        {
                                            entity[excelImportDetail.FieldName] = value;
                                        }
                                        else
                                        {
                                            if (base_excellimport.ErrorHanding == "0")
                                            {
                                                CheckEx = true;
                                            }
                                            DataRow dr = Newdt.NewRow();
                                            dr = Newdt.NewRow();
                                            dr[0] = errorNum;
                                            dr[1] = "第[" + rowNum.ToString() + "]行[" + excelImportDetail.ColumnName + "]";
                                            dr[2] = "日期格式不正确";
                                            Newdt.Rows.Add(dr);
                                            errorNum++;
                                            continue;
                                        }
                                        break;
                                    //外键
                                    case "3":
                                        sb.Clear();
                                        sb.Append(" and ");
                                        sb.Append(excelImportDetail.CompareField);
                                        sb.Append("='");
                                        sb.Append(value);
                                        sb.Append("' ");
                                        Hashtable htf = new Hashtable();
                                        //字段值非空才去找外键
                                        if (!string.IsNullOrEmpty(value))
                                        {
                                            htf = database.FindHashtable(excelImportDetail.ForeignTable, sb);
                                        }
                                        if (htf.Count > 0)
                                        {
                                            entity[excelImportDetail.FieldName] = htf[excelImportDetail.BackField.ToLower()];
                                        }
                                        else
                                        {
                                            if (base_excellimport.ErrorHanding == "0")
                                            {
                                                CheckEx = true;
                                            }
                                            DataRow dr = Newdt.NewRow();
                                            dr = Newdt.NewRow();
                                            dr[0] = errorNum;
                                            dr[1] = "第[" + rowNum.ToString() + "]行[" + excelImportDetail.ColumnName + "]";
                                            dr[2] = excelImportDetail.ColumnName + "在系统中不存在";
                                            Newdt.Rows.Add(dr);
                                            errorNum++;
                                            continue;
                                        }
                                        break;
                                    //唯一识别
                                    case "4":
                                        //判断该值是否在表中已存在
                                        sb.Clear();
                                        sb.Append(" and ");
                                        sb.Append(excelImportDetail.FieldName);
                                        sb.Append("='");
                                        sb.Append(value);
                                        sb.Append("' ");
                                        Hashtable htm = database.FindHashtable(base_excellimport.ImportTable, sb);
                                        if (htm.Count > 0)
                                        {
                                            if (base_excellimport.ErrorHanding == "0")
                                            {
                                                CheckEx = true;
                                            }
                                            DataRow dr = Newdt.NewRow();
                                            dr = Newdt.NewRow();
                                            dr[0] = errorNum;
                                            dr[1] = "第[" + rowNum.ToString() + "]行[" + excelImportDetail.ColumnName + "]";
                                            dr[2] = excelImportDetail.ColumnName + "在系统中已存在不能重复插入";
                                            Newdt.Rows.Add(dr);
                                            errorNum++;
                                            continue;
                                        }
                                        entity[excelImportDetail.FieldName] = value;
                                        break;
                                    default:
                                        break;
                                }
                                #endregion 单字段赋值结束
                            }
                            #endregion 遍历模板结束
                            //如果遇到空行，说明从Excel导入后续的行都是空的，不再导入，清除掉最后一个错误
                            if (string.IsNullOrEmpty(rowSb.ToString()))
                            {
                                Newdt.Rows.RemoveAt(Newdt.Rows.Count - 1);
                                break;
                            }
                            if (CheckEx)  //导入有错误，事务回滚
                            {
                                database.Rollback();
                                break;
                            }
                            #region 自定义字段赋值
                            entity["CarType"] = CarType;
                            entity["BodyNo"] = BodyNo;
                            entity["CheckTm"] = CheckTm;
                            entity["Category"] = "SWD";
                            entity["CreTm"] = DateTime.Now;
                            entity["CreCd"] = ManageProvider.Provider.Current().UserId;
                            entity["CreNm"] = ManageProvider.Provider.Current().UserName;
                            #endregion
                            database.Insert(base_excellimport.ImportTable, entity, isOpenTrans);
                            rowNum++;
                        }
                        #endregion 遍历Excel数据行结束
                        if (!CheckEx) database.Commit();
                        IsOk = 1;
                    }
                }
            }
            catch (System.Exception ex)
            {
                database.Rollback();
                Base_SysLogBll.Instance.WriteLog("", OperationType.Add, "-1", "异常错误：" + ex.Message);
                IsOk = -1;
            }
            Result = Newdt;
            return IsOk;
        }
        /// <summary>
        /// 获取SWD检测结果数据
        /// </summary>
        /// <param name="PropertyName"></param>
        /// <param name="PropertyValue"></param>
        /// <returns></returns>
        public List<QAS_JunctData_SWD> GetPageList(string PropertyName, string PropertyValue)
        {
            List<QAS_JunctData_SWD> dt;
            string sql = "";
            if (PropertyName == "")
            {
                sql = "select * from " + tableName + " where 1=1 order by Code";
            }
            else
            {
                //根据条件查询
                sql = "select * from " + tableName + " where " + PropertyName + " = '" + PropertyValue + "' order by Code";         
            }
            dt = Repository().FindListBySql(sql);
            return dt;
        }
        #endregion

    }
}