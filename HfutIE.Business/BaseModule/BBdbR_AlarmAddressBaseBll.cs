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
    /// 设备报警地址管理表
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.08.17 19:38</date>
    /// </author>
    /// </summary>
    public class BBdbR_AlarmAddressBaseBll : RepositoryFactory<BBdbR_AlarmAddressBase>
    {
        #region 全局变量定义区
        //定义本页面主要操作的表的表名，称为主表
        string tableName = "BBdbR_AlarmAddressBase";//===复制时需要修改===
        #endregion
        #region 1.页面表格
        /// <summary>
        /// 联合查询，展示页面表格
        /// </summary>
        /// <param name="CheckId"></param>
        /// <returns></returns>
        public DataTable GetPlanList()
        {
            StringBuilder strSql = new StringBuilder();
            DataTable dt;
            strSql.Append(@"SELECT a.*,b.DvcNm as DvcNm FROM  " + tableName + " a join BBdbR_DvcBase b on a.DvcId=b.DvcId where a.Enabled=1 ");
            dt = Repository().FindTableBySql(strSql.ToString(),false);
            return dt;
        }
        #endregion
        #region 2.展示表格
        /// <summary>
        /// 搜索表格中所有Enabled = true的数据, 即为有效的工厂信息
        /// </summary>
        /// <param name="jqgridparam">分页参数</param>
        /// <returns>返回搜索到的数据</returns>
        public List<BBdbR_AlarmAddressBase> GetPageList(JqGridParam jqgridparam) //===复制时需要修改===
        {
            return Repository().FindList("Enabled", "1");
        }
        #endregion
        #region 3.编辑方法
        //将修改后的实体跟新到数据库中
        //返回值为1，或者0
        //1表示操作成功，0表示操作失败
        public int Update(BBdbR_AlarmAddressBase entity) //===复制时需要修改===
        {
            //entity .AndonRuleTm= ConvertHelper . entity.AndonRuleTm
            return Repository().Update(entity); //将修改后的实体跟新到数据库中
        }
        public int CheckCount(string KeyName, string KeyValue)
        {
            string sql = @"select * from " + tableName + " where Enabled = '1' and " + KeyName + " = '" + KeyValue + "'";
            DataTable count = Repository().FindTableBySql(sql);
            int a = count.Rows.Count;
            return a;
        }
        #endregion
        #region 4.检查某个字段的字段值是否存在
        /// <summary>
        ///   Enabled = 1的数据中某个字段（KeyName）的字段值（KeyValue）是否存在
        /// </summary>
        /// <param name="KeyName">字段名</param>
        /// <param name="KeyValue">字段值</param>
        /// <returns>返回的是搜索的表中包含该字段值的记录条数</returns>
        public int CheckCount(string KeyName1, string KeyValue1, string KeyName2, string KeyValue2)
        {
            string sql = @"select * from " + tableName + " where Enabled = '1' and'" + KeyName1 + "' = '" + KeyValue1 + "'and'" + KeyName2 + "' = '" + KeyValue2 + "'";
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
        public int Insert(BBdbR_AlarmAddressBase entity) //===复制时需要修改===
        {
            return Repository().Insert(entity);
        }
        #endregion
        #region 7.删除方法
        //array 需要删除的信息的主键的数组
        //删除表中某一条数据表示将表中该条数据的Enabled设置为0，并不是真的删除该数据
        //返回值为1，或者0
        //1表示操作成功，0表示操作失败
        public int Delete(string[] array)
        {
            //创建一个表格的list，用于存储通过主键查询到的信息
            List<BBdbR_AlarmAddressBase> listEntity = new List<BBdbR_AlarmAddressBase>(); //===复制时需要修改===
            foreach (string keyValue in array)
            {
                //===复制时需要修改===
                BBdbR_AlarmAddressBase entity = Repository().FindEntity(keyValue);//根据主键（keyValue）在数据库中查找实体 //===复制时需要修改===
                entity.Enabled = "0";//将该实体的IsAvailable属性改为false
                listEntity.Add(entity);
            }
            return Repository().Update(listEntity);//修改数据库
        }
        #endregion
        #region 8.查询方法，需要修改sql语句
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
            DataTable dt;
            if (Condition == "all")
            { 
                sql = @"select a.*,b.DvcNm as DvcNm from " + tableName + " a join BBdbR_DvcBase b on a.DvcId = b.DvcId where a.Enabled = '1'";
                dt = Repository().FindTableBySql(sql.ToString(), false);
            }
            else if (Condition == "Class")
            {
                sql = @"select a.*,b.DvcNm as DvcNm from " + tableName + " a join BBdbR_DvcBase b on a.DvcId = b.DvcId where a.Enabled = '1' and a." + Condition + " like  '%" + keywords + "%'";
                dt = Repository().FindTableBySql(sql.ToString(), false);
            }
            else
            {
                //根据条件查询
                //string sql1 = @"select ";
                sql = @"select a.*,b.DvcNm as DvcNm from " + tableName + " a join BBdbR_DvcBase b on a.DvcId = b.DvcId where a.Enabled = '1' and b." + Condition + " like  '%" + keywords + "%'";
                dt = Repository().FindTableBySql(sql.ToString(), false);
            }
            return dt;
        }
        #endregion
        #region 10.下拉框设备
        public DataTable GetDvcNm()
        {
            string sql = @"select DvcId as id, DvcNm as dvcnm from BBdbR_DvcBase where Enabled='1'";
            return Repository().FindTableBySql(sql);
        }
        
        #endregion
        #region 10.编辑界面填充
        //编辑界面填充
        public DataTable SetAlarmInfor(string keywords) //===复制时需要修改===
        {
            string strSql = "";
            DataTable dt;

            strSql = @"select * from " + tableName + " where Enabled = '1'";
            dt = Repository().FindTableBySql(strSql.ToString());
            dt.Columns.Add(new DataColumn("PlineNm", typeof(string)));
            dt.Columns.Add(new DataColumn("WcNm", typeof(string)));
            dt.Columns.Add(new DataColumn("AviNm", typeof(string)));
            dt.Columns.Add(new DataColumn("DvcNm", typeof(string)));

            string PlineId = dt.Rows[0]["PlineId"].ToString();
            string WcId = dt.Rows[0]["PlineId"].ToString();
            string AviId = dt.Rows[0]["PlineId"].ToString();
            string DvcId = dt.Rows[0]["PlineId"].ToString();

           string  sql1= @"select * from BBdbR_PlineBase where Enabled = '1' and PlineId='" + PlineId + "'";
            DataTable dt1 = Repository().FindTableBySql(strSql.ToString());
            dt.Rows [0]["PlineNm"]  = dt1.Rows[0]["PlineNm"];

            string sql2 = @"select * from BBdbR_WcBase where Enabled = '1' and PlineId='" + PlineId + "'";
            DataTable dt2 = Repository().FindTableBySql(strSql.ToString());
            dt.Rows[0]["WcNm"] = dt1.Rows[0]["WcNm"];

            string sql3 = @"select * from BBdbR_AVIBase where Enabled = '1' and PlineId='" + PlineId + "'";
            DataTable dt3 = Repository().FindTableBySql(strSql.ToString());
            dt.Rows[0]["AviNm"] = dt1.Rows[0]["AviNm"];

            string sql4 = @"select * from BBdbR_DvcBase where Enabled = '1' and PlineId='" + PlineId + "'";
            DataTable dt4 = Repository().FindTableBySql(strSql.ToString());
            dt.Rows[0]["DvcNm"] = dt1.Rows[0]["DvcNm"];

            return dt;
        }
        #endregion
        
        
        #region 9.加载全部故障列表
        /// <summary>
        /// 联合查询
        /// </summary>
        /// <returns></returns>
        public List<BBdbR_AlarmAddressBase> GetFaultList()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT  * FROM  BBdbR_AlarmAddressBase where Enabled=1 ");
            List<BBdbR_AlarmAddressBase> dt = Repository().FindListBySql(strSql.ToString());
            return dt;
        }
        #endregion
    
        #region 10.导出模板
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

       
        #region 导出得到设备ID
        public DataTable searchID(string DvcCd)
        {
            try
            {
                string sql = @"select DvcId as id from BBdbR_DvcBase where DvcCd = '" + DvcCd + "' and Enabled = 1";
                DataTable dtID = Repository().FindTableBySql(sql.ToString(), false);
                return dtID != null ? dtID : null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion
    }
}