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
    /// 班次详细时间配置信息表
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.08.28 15:35</date>
    /// </author>
    /// </summary>
    public class BFacR_ShiftBaseBll : RepositoryFactory<BFacR_ShiftBase>
    {
        #region 全局变量定义区
        //定义本页面主要操作的表的表名，称为主表
        string tableName = "BFacR_ShiftBase";//===复制时需要修改===
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
        public DataTable GetPageListByCondition(string keywords, string Condition, JqGridParam jqgridparam) //===复制时需要修改===
        {
            string sql = "";
            if (Condition == "all")
            {
                sql = @"select * from " + tableName + " where RestTm = '总时间' and Enabled=1 order by ShiftCd asc";
                return Repository().FindTableBySql(sql.ToString(),false);
            }
            else
            {
                if (keywords != "all")
                {
                    //根据条件查询
                    sql = @"select * from " + tableName + " where  " + Condition + " like  '%" + keywords + "%' and RestTm = '总时间' and Enabled=1 order by ShiftCd asc";
                    return Repository().FindTableBySql(sql.ToString(), false);
                }
                else
                {
                    sql = @"select * from " + tableName + " where RestTm = '总时间' and Enabled=1  order by ShiftCd asc";
                    return Repository().FindTableBySql(sql.ToString(), false);
                }
            }
        }
        /// <summary>
        /// 拿到班次时间配置信息
        /// </summary>
        /// <param name="keywords"></param>
        /// <param name="Condition"></param>
        /// <param name="jqgridparam"></param>
        /// <returns></returns>
        public DataTable GetListJson_TimeConfig(string KeyValue, JqGridParam jqgridparam) //===复制时需要修改===
        {
            
             //string sql = @"select * from BFacR_ShiftTimeConfig where ShiftId = '"+ KeyValue + "' and Enabled=1";
             string sql = @"select * from BFacR_ShiftBase where ShiftCd in (select ShiftCd from BFacR_ShiftBase where ShiftId = '" + KeyValue + "' and Enabled = 1)and Enabled = 1 order by StrtRestTm ";
             return Repository().FindTableBySql(sql.ToString(), false);
        }
        #endregion

        #region 2.编辑
        //将修改后的实体跟新到数据库中
        //返回值为1，或者0
        //1表示操作成功，0表示操作失败
        public int Update(BFacR_ShiftBase entity) //===复制时需要修改===
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
            string sql = @"select * from " + tableName + " where  " + KeyName + " = '" + KeyValue + "' and Enabled=1";
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
        public int Insert(BFacR_ShiftBase entity) //===复制时需要修改===
        {
            return Repository().Insert(entity);
        }
        #endregion

        #region 5.删除
        //array 需要删除的信息的主键的数组
        //删除表中某一条数据表示将表中该条数据的Enabled设置为0，并不是真的删除该数据
        //返回值为1，或者0
        //1表示操作成功，0表示操作失败
        public int Delete(string id)
        {
            //创建一个表格的list，用于存储通过主键查询到的信息
            List<BFacR_ShiftBase> listEntity = new List<BFacR_ShiftBase>(); //===复制时需要修改===
            //===复制时需要修改===
            BFacR_ShiftBase entity = Repository().FindEntity(id);//根据主键（keyValue）在数据库中查找实体 //===复制时需要修改===
            entity.Enabled = "0";//将该实体的IsAvailable属性改为false
            listEntity.Add(entity);
            return Repository().Update(listEntity);//修改数据库
        }
        #endregion

        #region 6.导出模板
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

        #region 7.导入
        /// <summary>
        /// 导入数据
        /// </summary>
        /// <param name="dt">Excel数据</param>
        /// <returns></returns>
        public int ImportExcel(string moduleId, DataTable dt, out DataTable Result)
        {
            //构造导入返回结果表
            DataTable Newdt = new DataTable("Result");
            Newdt.Columns.Add("rowid", typeof(System.String));                 //行号
            Newdt.Columns.Add("locate", typeof(System.String));                 //位置
            Newdt.Columns.Add("reason", typeof(System.String));                 //原因
            int IsOk = 0;
            //获得导入模板
            //模板主表
            Base_ExcelImport base_excellimport = DataFactory.Database().FindEntity<Base_ExcelImport>("ModuleId", moduleId);
            if (base_excellimport.ImportId == null)
            {
                IsOk = 0;
            }
            else
            {
                //string pkName = new Base_DataBaseBll().GetPrimaryKey(base_excellimport.ImportTable);//主键列名
                //模板明细表
                List<Base_ExcelImportDetail> listBase_ExcelImportDetail = DataFactory.Database().FindList<Base_ExcelImportDetail>("ImportId", base_excellimport.ImportId);
                //取出要插入的表名
                string tableName = base_excellimport.ImportTable;
                if (dt != null && dt.Rows.Count > 0)
                {
                    bool isExit = false;
                    IDatabase database = DataFactory.Database();
                    DbTransaction isOpenTrans = database.BeginTrans();
                    try
                    {
                        #region 遍历Excel数据行
                        int rowNum = 1;
                        int errorNum = 1;
                        foreach (DataRow item in dt.Rows)
                        {
                            Hashtable entity = new Hashtable();//最终要插入数据库的hashtable
                            StringBuilder sb = new StringBuilder();
                            StringBuilder rowSb = new StringBuilder();//累加每一个单元格的值，一行全空就停止插入
                            #region 遍历模板，为每一行中每个字段找到模板列并赋值
                            foreach (Base_ExcelImportDetail excelImportDetail in listBase_ExcelImportDetail)
                            {
                                string value = "";
                                value = item[excelImportDetail.ColumnName].ToString();
                                rowSb.Append(value);//累加每一个单元格的值，一行全空就停止插入
                                DateTime dateTime = DateTime.Now;
                                decimal num = 0;
                                #region 单个字段赋值
                                switch (excelImportDetail.DataType)
                                {
                                    //字符串
                                    case "0":

                                        entity[excelImportDetail.FieldName] = value;
                                        break;
                                    //数字
                                    case "1":
                                        if (decimal.TryParse(value, out num))
                                        {
                                            entity[excelImportDetail.FieldName] = value;
                                        }
                                        else
                                        {
                                            if (base_excellimport.ErrorHanding == "0")
                                            {
                                                isExit = true;
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
                                                isExit = true;
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
                                                isExit = true;
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
                                                isExit = true;
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
                            if (isExit)
                            {
                                break;
                            }

                            database.Insert(base_excellimport.ImportTable, entity, isOpenTrans);
                            rowNum++;
                        }
                        #endregion 遍历Excel数据行结束
                        database.Commit();
                        IsOk = 1;
                    }
                    catch (System.Exception ex)
                    {
                        database.Rollback();
                        Base_SysLogBll.Instance.WriteLog("", OperationType.Add, "-1", "异常错误：" + ex.Message);
                        IsOk = -1;
                    }
                }
            }
            Result = Newdt;
            return IsOk;
        }
        #endregion

        #region 8.班制班次配置
        /// <summary>
        /// 查询班次基本信息表里存在，但未存在配置的班次列表
        /// 查询的时候传递了keywords
        /// </summary>
        /// <param name="keywords">查询值</param>
        /// <param name="jqgridparam">分页参数</param>
        /// <returns></returns>
        public List<BFacR_ShiftBase> GetReConfigList(string keywords, JqGridParam jqgridparam) //===复制时需要修改===
        {
            string sql = "";
            if (keywords != "")
            {
                sql = @"select * from " + tableName + " where Enabled=1 and RestTm = '总时间' and ShiftId not in (select Distinct(ShiftId) from BFacR_ClassConfig where Enabled=1 and ClassId='" + keywords + "')";
                return Repository().FindListPageBySql(sql.ToString(), ref jqgridparam);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 查询班次基本信息表里存在，也存在配置的班次列表
        /// 查询的时候传递了keywords
        /// </summary>
        /// <param name="keywords">查询值</param>
        /// <param name="jqgridparam">分页参数</param>
        /// <returns></returns>
        public List<BFacR_ShiftBase> GetConfigList(string keywords,string Type, JqGridParam jqgridparam) //===复制时需要修改===
        {
            string sql = "";
            if (keywords != "")
            {
                if (Type == "1")//班组班次配置时展示信息不同
                {
                    sql = @"select a.* from " + tableName + " a join BFacR_ClassConfig b on a.ShiftId=b.ShiftId where a.Enabled=1 and b.Enabled=1 and b.ClassId='" + keywords+"'";
                }
                else
                {
                    sql = @"select * from " + tableName + "  where ShiftCd in(select a.ShiftCd from " + tableName + " a join BFacR_ClassConfig b on a.ShiftId=b.ShiftId where a.Enabled=1 and b.Enabled=1 and b.ClassId='" + keywords + "') order by StrtRestTm";
                }
                return Repository().FindListBySql(sql.ToString());
            }
            else
            {
                return null;
            }
        }
        #endregion
    }
}