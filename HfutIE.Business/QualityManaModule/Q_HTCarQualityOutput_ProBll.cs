//=====================================================================================
// All Rights Reserved , Copyright @ HfutIE 2022
// Software Developers @ HfutIE 2022
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
    /// 冲焊涂装质量检查销项过程表
    /// <author>
    ///		<name>she</name>
    ///		<date>2022.04.25 09:55</date>
    /// </author>
    /// </summary>
    public class Q_HTCarQualityOutput_ProBll : RepositoryFactory<Q_HTCarQualityOutput_Pro>
    {
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

        #region 按VIN号和工段查询
        public List<Q_HTCarQualityOutput_Pro> GetListByVIN(string VIN, string QualityCheckPointGroupNm) //===复制时需要修改===
        {
            string sql = "";
            List<Q_HTCarQualityOutput_Pro> dt;
            sql = @"select * from Q_HTCarQualityOutput_Pro where Enabled = '1' and VIN = '" + VIN + "' and QualityCheckPointGroupNm = '" + QualityCheckPointGroupNm + "'";
            dt = Repository().FindListBySql(sql.ToString());
            return dt;
        }

        #endregion

        #region 根据结果表主键删除主键下的所有缺陷记录
        public int deleteByResult(string CarQualityResultId) //===复制时需要修改===
        {
            string sql = "";
            List<Q_HTCarQualityOutput_Pro> dtList;
            sql = @"select * from Q_HTCarQualityOutput_Pro where Enabled = '1' and CarQualityResultId = '" + CarQualityResultId + "' ";
            dtList = Repository().FindListBySql(sql.ToString());
            foreach (var item in dtList)
            {
                item.Enabled = "0";
            }
            int num = Repository().Update(dtList);
            return num;//返回结果
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
            List<Q_HTCarQualityOutput_Pro> listEntity = new List<Q_HTCarQualityOutput_Pro>(); //===复制时需要修改===
            //===复制时需要修改===
            Q_HTCarQualityOutput_Pro entity = Repository().FindEntity(KeyValue);//根据主键（keyValue）在数据库中查找实体 //===复制时需要修改===
            entity.Enabled = "0";//将该实体的IsAvailable属性改为false
            entity.Modify(KeyValue);
            listEntity.Add(entity);
            return Repository().Update(listEntity);//修改数据库
        }
        #endregion



    }
}