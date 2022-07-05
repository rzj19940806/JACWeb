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
    /// 设备基础信息表
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.03.25 21:56</date>
    /// </author>
    /// </summary>
    public class BBdbR_DvcBaseBll : RepositoryFactory<BBdbR_DvcBase>
    {
        #region 全局变量定义区

        //定义本页面主要操作的表的表名，称为主表
        string tableName = "BBdbR_DvcBase";//===复制时需要修改===
        #endregion

        #region 1.获取树，需要修改sql语句
        public DataTable GetTree()
        {
            StringBuilder strSql = new StringBuilder();    
            //===复制时需要修改===
       
            strSql.Append(@"select    
                             '10' AS keys,
                             '10'AS code,
                             '江淮蔚来乘用车制造公司' AS name,
                             '1' As IsAvailable,
                             '0' AS parentId,
                             '1' as sort 
                          from BBdbR_FacBase where 1=1");
            strSql.Append(@"union select  FacId AS keys,     
                             FacCd AS code,
                             FacNm AS name,
                             Enabled As IsAvailable,
                             '10' as parentId,  
                             '1' as sort    
                         from BBdbR_FacBase where Enabled = '1' ");
            strSql.Append(@" union select    
                             a.WorkshopId AS keys,
                             a.WorkshopCd AS code,
                             a.WorkshopNm AS name,
                             a.Enabled As IsAvailable,
                             a.FacId AS parentId,
                              '1' as sort 
                          from  BBdbR_WorkshopBase a,BBdbR_FacBase b 
                          where a.FacId=b.FacId and a.Enabled = '1' order by code asc");
            return Repository().FindTableBySql(strSql.ToString());
        }
        #endregion

        #region 3.展示表格
        /// <summary>
        /// 搜索表格中所有IsAvailable = true的数据
        /// </summary>
        /// <param name="jqgridparam">分页参数</param>
        /// <returns>返回搜索到的数据</returns>
        public List<BBdbR_DvcBase> GetPageList(JqGridParam jqgridparam) //===复制时需要修改===
        {
            return Repository().FindList("Enabled", "1");
        }
        #endregion

        #region 4.编辑方法
        //将修改后的实体跟新到数据库中
        //返回值为1，或者0
        //1表示操作成功，0表示操作失败
        public int Update(BBdbR_DvcBase entity) //===复制时需要修改===
        {
            return Repository().Update(entity); //将修改后的实体跟新到数据库中
        }
        #endregion

        #region 5.检查某个字段的字段值是否存在
        /// <summary>
        ///   Enabled = 1的数据中某个字段（KeyName）的字段值（KeyValue）是否存在
        /// </summary>
        /// <param name="KeyName">字段名</param>
        /// <param name="KeyValue">字段值</param>
        /// <returns>返回的是搜索的表中包含该字段值的记录条数</returns>
        public int CheckCount(string KeyName, string KeyValue)
        {
            string sql = @"select * from " + tableName + " where Enabled = 1 and " + KeyName + " = '" + KeyValue + "'";
            DataTable count = Repository().FindTableBySql(sql);
            int a = count.Rows.Count;
            return a;
        }
        #endregion

        #region 6.新增方法
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
        public int Insert(BBdbR_DvcBase entity) //===复制时需要修改===
        {
            return Repository().Insert(entity);
        }
        #endregion

        #region 6.下拉框

        //获取所有车间
        public DataTable GetWorkshopNm(string classv)
        {
            string sql = "";
            if (classv == "车间")
            {
                 sql = @"select WorkshopId as id, WorkshopNm as name from BBdbR_WorkshopBase where Enabled=1 order by sort asc";
                
            } else if(classv == "工位")
            {
                sql = @"select WcId as id, WcNm as name from BBdbR_WcBase where Enabled=1 order by WcCd asc";
            } else if(classv == "产线")
            {
                sql = @"select PlineId as id, PlineNm as name from BBdbR_PlineBase where Enabled=1 order by PlineCd asc";
            } 
            else if (classv == "AVI设备")
            {
                sql = @"select AviId as id, AviNm as name from BBdbR_AVIBase where Enabled=1 and OP_CODE is not null  order by OP_CODE,AviCd asc";
            }
            return Repository().FindTableBySql(sql);
        }
        //获取所有产线
        
        //获取机构信息
        public DataTable GetWorkshopNm2(string ClassId)
        {
            string sql = @"select WorkshopNm as classnm from BBdbR_WorkshopBase where Enabled='1' and WorkshopId=" + "'" + ClassId + "'";
            return Repository().FindTableBySql(sql);
        }
        public DataTable GetPlineNm2(string ClassId)
        {
            string sql = @"select PlineNm as classnm from BBdbR_PlineBase where Enabled='1' and PlineId=" + "'" + ClassId + "'";
            return Repository().FindTableBySql(sql);
        }
        public DataTable GetWcNm2(string ClassId)
        {
            string sql = @"select WcNm as classnm from BBdbR_WcBase where Enabled='1' and WcId=" + "'" + ClassId + "'";
            return Repository().FindTableBySql(sql);
        }
        public DataTable GetPostNm2(string ClassId)
        {
            string sql = @"select PostNm as classnm from BBdbR_PostBase where Enabled='1' and PostId=" + "'" + ClassId + "'";
            return Repository().FindTableBySql(sql);
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
            List<BBdbR_DvcBase> listEntity = new List<BBdbR_DvcBase>(); //===复制时需要修改===
            foreach (string keyValue in array)
            {
                //===复制时需要修改===
                BBdbR_DvcBase entity = Repository().FindEntity(keyValue);//根据主键（keyValue）在数据库中查找实体 //===复制时需要修改===
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
            DataTable dt = new DataTable();
            if (Condition == "all" || Condition == null)
            {
                sql = @"select * from " + tableName +" where Enabled=1 order by "+ jqgridparam.sidx+" "+ jqgridparam.sord;
                dt = Repository().FindTableBySql(sql.ToString(), false);
                dt.Columns.Add("ClassNm");
                for(int i = 0; i < dt.Rows.Count; i++)
                {
                    if(dt.Rows[i]["Class"].ToString() == "车间")
                    {
                        sql = @"select WorkshopNm from BBdbR_WorkshopBase where WorkshopId='" + dt.Rows[i]["ClassId"].ToString() + "'and Enabled=1 ";
                        DataTable dt1 = Repository().FindTableBySql(sql.ToString(), false);
                        if (dt1.Rows.Count>0)
                        {
                            dt.Rows[i]["ClassNm"] = dt1.Rows[0][0];
                        }
                    }
                    else if(dt.Rows[i]["Class"].ToString() == "工位")
                    {
                        sql = @"select WcNm from BBdbR_WcBase where WcId='" + dt.Rows[i]["ClassId"].ToString() + "' and Enabled=1 ";
                        DataTable dt2 = Repository().FindTableBySql(sql.ToString(), false);
                        if (dt2.Rows.Count > 0)
                        {
                            dt.Rows[i]["ClassNm"] = dt2.Rows[0][0];
                        }
                    }
                    else if(dt.Rows[i]["Class"].ToString() == "产线")
                    {
                        sql = @"select PlineNm from BBdbR_PlineBase where PlineId='" + dt.Rows[i]["ClassId"].ToString() + "' and Enabled=1 ";
                        DataTable dt3 = Repository().FindTableBySql(sql.ToString(), false);
                        if (dt3.Rows.Count > 0)
                        {
                            dt.Rows[i]["ClassNm"] = dt3.Rows[0][0];
                        }
                    }
                    else if(dt.Rows[i]["Class"].ToString() == "岗位")
                    {
                        sql = @"select PostNm from BBdbR_PostBase where PostId='" + dt.Rows[i]["ClassId"].ToString() + "' and Enabled=1 ";
                        DataTable dt4 = Repository().FindTableBySql(sql.ToString(), false);
                        if (dt4.Rows.Count > 0)
                        {
                            dt.Rows[i]["ClassNm"] = dt4.Rows[0][0];
                        }
                    }
                    else if (dt.Rows[i]["Class"].ToString() == "AVI设备")
                    {
                        sql = @"select AviNm from BBdbR_AVIBase where AviId ='" + dt.Rows[i]["ClassId"].ToString() + "' and Enabled=1 ";
                        DataTable dt5 = Repository().FindTableBySql(sql.ToString(), false);
                        if (dt5.Rows.Count > 0)
                        {
                            dt.Rows[i]["ClassNm"] = dt5.Rows[0][0];
                        }
                    }
                }
            }
            else
            {
                sql = @"select * from " + tableName + " where Enabled = 1 and " + Condition + " like  '%" + keywords + "%'" + jqgridparam.sidx + " " + jqgridparam.sord;
                dt = Repository().FindTableBySql(sql.ToString(), false);
                dt.Columns.Add("ClassNm");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["Class"].ToString() == "车间")
                    {
                        sql = @"select WorkshopNm from BBdbR_WorkshopBase where WorkshopId='" + dt.Rows[i]["ClassId"].ToString() + "'and Enabled=1 ";
                        DataTable dt1 = Repository().FindTableBySql(sql.ToString(), false);
                        dt.Rows[i]["ClassNm"] = dt1.Rows[0][0];
                    }
                    else if (dt.Rows[i]["Class"].ToString() == "工位")
                    {
                        sql = @"select WcNm from BBdbR_WcBase where WcId='" + dt.Rows[i]["ClassId"].ToString() + "' and Enabled=1 ";
                        DataTable dt2 = Repository().FindTableBySql(sql.ToString(), false);
                        dt.Rows[i]["ClassNm"] = dt2.Rows[0][0];
                    }
                    else if (dt.Rows[i]["Class"].ToString() == "产线")
                    {
                        sql = @"select PlineNm from BBdbR_PlineBase where PlineId='" + dt.Rows[i]["ClassId"].ToString() + "' and Enabled=1 ";
                        DataTable dt3 = Repository().FindTableBySql(sql.ToString(), false);
                        dt.Rows[i]["ClassNm"] = dt3.Rows[0][0];
                    }
                    else if (dt.Rows[i]["Class"].ToString() == "岗位")
                    {
                        sql = @"select PostNm from BBdbR_PostBase where PostId='" + dt.Rows[i]["ClassId"].ToString() + "' and Enabled=1 ";
                        DataTable dt4 = Repository().FindTableBySql(sql.ToString(), false);
                        dt.Rows[i]["ClassNm"] = dt4.Rows[0][0];
                    }
                    else if (dt.Rows[i]["Class"].ToString() == "AVI设备")
                    {
                        sql = @"select AviNm from BBdbR_AVIBase where AviId ='" + dt.Rows[i]["ClassId"].ToString() + "' and Enabled=1 ";
                        DataTable dt5 = Repository().FindTableBySql(sql.ToString(), false);
                        if (dt5.Rows.Count > 0)
                        {
                            dt.Rows[i]["ClassNm"] = dt5.Rows[0][0];
                        }
                    }
                }
            }
            return dt;
        }
        #endregion

        #region 9.页面表格
        /// <summary>
        /// 联合查询，展示页面表格
        /// </summary>
        /// <returns></returns>
        public List<BBdbR_DvcBase> GetPlanList1()
        {
            return Repository().FindList("Enabled", "1");
        }
      
        /// <summary>
        /// 编辑弹框中数据源
        /// </summary>
        /// <returns></returns>
        public BBdbR_DvcBase GetPlanList1(string KeyValue)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT  * FROM  BBdbR_DvcBase where  DvcId='"+ KeyValue + "'");
            List<BBdbR_DvcBase> dt = Repository().FindListBySql(strSql.ToString());
            BBdbR_DvcBase Dvcentity = new BBdbR_DvcBase();
            Dvcentity = dt[0];
            return Dvcentity;
        }
        #endregion

        #region 10.获得导出模板
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

        #region 11.导入数据
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

                            //if (tableName == "A_OrderBase")
                            //{
                            //    entity[pkName] = int.Parse(GetNextKey(tableName, pkName.ToString()).ToString());
                            //}
                            //else
                            //{
                            //    entity[pkName] = Guid.NewGuid().ToString();//首先给主键赋值
                            //}



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

        #region 导出时的机构信息
        public DataTable searchClass(string dvcClass, string devClassICd)
        {
            try
            {
                string sql = "";
                DataTable dt = new DataTable();
                if (dvcClass == "车间")
                {
                    sql = @"select WorkshopId as id from BBdbR_WorkshopBase where WorkshopCd = '" + devClassICd + "'Enabled=1";
                    dt = Repository().FindTableBySql(sql.ToString(), false);
                }
                else if (dvcClass == "工位")
                {
                    sql = @"select WcId  as id from BBdbR_WcBase where WcCd = '" + devClassICd + "'Enabled=1";
                    dt = Repository().FindTableBySql(sql.ToString(), false);
                }
                else if (dvcClass == "产线")
                {
                    sql = @"select PlineId as id from BBdbR_PlineBase wherre PlineCd = '" + devClassICd + "'Enabled=1";
                    dt = Repository().FindTableBySql(sql.ToString(), false);
                }
                else if (dvcClass == "岗位")
                {
                    sql = @"select PostId as id from BBdbR_PostBase where PostCd = '" + devClassICd + "'Enabled=1";
                    dt = Repository().FindTableBySql(sql.ToString(), false);
                }
                else if (dvcClass == "AVI设备")
                {
                    sql = @"select AviId as id from BBdbR_AVIBase where AviCd = '" + devClassICd + "'Enabled=1";
                    dt = Repository().FindTableBySql(sql.ToString(), false);
                }
                return dt != null?dt:null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

    }
}