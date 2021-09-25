using HfutIE.Business;
using HfutIE.DataAccess;
using HfutIE.Entity;
using HfutIE.Repository;
using HfutIE.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace HfutIE.WebApp.Areas.QualityModule.Controllers
{
    /// <summary>
    /// 质量检测项表控制器
    /// </summary>
    public class QualityEntryController : PublicController<QAS_JunctionDataMid>
    {
        #region 列表视图表单
        public ActionResult IndexImpt()
        {
            string moduleId = DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId"));
            //模板主表
            Base_ExcelImport base_excellimport = DataFactory.Database().FindEntity<Base_ExcelImport>("ModuleId", moduleId);
            if (base_excellimport.ModuleId != null)
            {
                ViewBag.ModuleId = moduleId;
                ViewBag.ImportFileName = base_excellimport.ImportFileName;
                ViewBag.ImportName = base_excellimport.ImportName;
                ViewBag.ImportId = base_excellimport.ImportId;
            }
            else
            {
                ViewBag.ModuleId = "0";
            }
            return View();
        }
        public ActionResult DataImport(string BodyNo, string CarType, string CheckTm)
        {
            string moduleId = DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId"));
            //模板主表
            Base_ExcelImport base_excellimport = DataFactory.Database().FindEntity<Base_ExcelImport>("ModuleId", moduleId);
            if (base_excellimport.ModuleId != null)
            {
                ViewBag.ModuleId = moduleId;
                ViewBag.ImportFileName = base_excellimport.ImportFileName;
                ViewBag.ImportName = base_excellimport.ImportName;
                ViewBag.ImportId = base_excellimport.ImportId;
                ViewBag.BodyNo = BodyNo;
                ViewBag.CarType = CarType;
                ViewBag.CheckTm = CheckTm;
            }
            else
            {
                ViewBag.ModuleId = "0";
            }
            return View();
        }
        #endregion

        #region 全局变量区域
        public readonly RepositoryFactory<QAS_JunctionDataMid> repository_JunDataMid = new RepositoryFactory<QAS_JunctionDataMid>();
        public readonly RepositoryFactory<QAS_JunctionDataDoc> repository_JunDataDoc = new RepositoryFactory<QAS_JunctionDataDoc>();
        public readonly RepositoryFactory<QAS_JunctionItemBase> repository_JunItemBase = new RepositoryFactory<QAS_JunctionItemBase>();
        public readonly RepositoryFactory<QAS_JunctData_SWD> repository_JunSWDBase = new RepositoryFactory<QAS_JunctData_SWD>();
        //public JavaScriptSerializer js = new JavaScriptSerializer();//反序列化    
        #endregion

        #region 创建数据库操作对象区域
        QAS_JunctionDataMidBll JunMidBll = new QAS_JunctionDataMidBll();
        QAS_JunctionDataDocBll JunDocBll = new QAS_JunctionDataDocBll();
        QAS_JunctData_SWDBll JunSWDBll = new QAS_JunctData_SWDBll();
        #endregion

        #region 方法区
        #region 主方法区
        /// <summary>
        /// 加载点连接待录入列表(多条件)
        /// </summary>
        /// <param name="PropNm1"></param>
        /// <param name="PropValue1"></param>
        /// <param name="PropNm2"></param>
        /// <param name="PropValue2"></param>
        /// <param name="PropNm3"></param>
        /// <param name="PropValue3"></param>
        /// <returns></returns>
        public ActionResult GetJunDateMidList(string PropNm1, string PropValue1, string PropNm2, string PropValue2, string PropNm3, string PropValue3, JqGridParam jqgridparam)
        {
            Stopwatch watch = CommonHelper.TimerStart();
            List<QAS_JunctionDataMid> ListData = JunMidBll.GetPageList(PropNm1, PropValue1, PropNm2, PropValue2, PropNm3, PropValue3);
            var JsonData = new
            {
                total = jqgridparam.total,
                page = jqgridparam.page,
                records = jqgridparam.records,
                costtime = CommonHelper.TimerEnd(watch),
                rows = ListData,
            };
            return Content(JsonData.ToJson());
        }
        /// <summary>
        /// 生成点连接待检测项列表
        /// </summary>
        /// <param name="Category"></param>
        /// <returns></returns>
        public ActionResult GenerateEntry(string Category, string BodyNo, string CarType, string CheckTm)
        {
            try
            {
                var Message = "载入失败";
                //先判断是否已存在该车身下该类别的记录
                string selectmidsql = "select * from QAS_JunctionDataMid where Category ='"+ Category + "' and BodyNo='"+ BodyNo + "'";  
                List<QAS_JunctionDataMid> datamid = repository_JunDataMid.Repository().FindListBySql(selectmidsql);
                if (datamid.Count > 0)
                {
                    //录入中间表存在数据=>加载未完成录入的列表
                    return Content(new JsonMessage { Success = true, Code = "1", Message = "加载成功【该车身存在未完成的录入项】" }.ToString());
                }
                else
                {
                    string selectdocsql = "select * from QAS_JunctionDataDoc where Category ='" + Category + "' and BodyNo='" + BodyNo + "'";
                    List<QAS_JunctionDataDoc> datadoc = repository_JunDataDoc.Repository().FindListBySql(selectdocsql);
                    if (datadoc.Count > 0)
                    {
                        return Content(new JsonMessage { Success = false, Code = "-1", Message = "车身【" + BodyNo + "】已存在完整的录入数据！" }.ToString());
                    }
                    else
                    {
                        //无该车身录入数据=>根据检测项生成待录入列表
                        string selectjunitemsql = "select * from QAS_JunctionItemBase where Category ='" + Category + "' and CarType='" + CarType + "'";
                        List<QAS_JunctionItemBase> dataitem = repository_JunItemBase.Repository().FindListBySql(selectjunitemsql);
                        if (dataitem.Count == 0 || dataitem == null)
                        {
                            return Content(new JsonMessage { Success = false, Code = "-1", Message = "未找到该类别【" + Category + "/" + CarType + "】下的检测项！" }.ToString());
                        }
                        #region 根据不同类别检测初步填写检测值
                        string checkstartvalue = "";
                        if (Category != "SWD")
                        {
                            checkstartvalue = "OK";//除SWD检测外，默认检测值为ok
                        }
                        #endregion
                        List<QAS_JunctionDataMid> junmidlist = new List<QAS_JunctionDataMid>();
                        for (int i = 0; i < dataitem.Count; i++)
                        {
                            QAS_JunctionDataMid junmid = new QAS_JunctionDataMid();
                            junmid.RecordId = Guid.NewGuid().ToString();
                            junmid.CarType = dataitem[i].CarType;
                            junmid.Code = dataitem[i].Code;
                            junmid.WcNm = dataitem[i].WcNm;
                            junmid.ItemNm = dataitem[i].ItemNm;
                            junmid.PartNm = dataitem[i].PartNm;
                            junmid.CCSC = dataitem[i].CCSC;
                            junmid.HeadHeghitSta = dataitem[i].HeadHeghitSta;
                            junmid.HeadGapSta = dataitem[i].HeadGapSta;
                            junmid.LengthSta = dataitem[i].LengthSta;
                            junmid.TightenTOR = dataitem[i].TightenTOR;
                            junmid.SpotLocation = dataitem[i].SpotLocation;
                            junmid.SpotStaValue = dataitem[i].SpotStaValue;
                            junmid.Unit = dataitem[i].Unit;
                            junmid.Category = dataitem[i].Category;
                            junmid.BodyNo = BodyNo;
                            junmid.CheckTm = Convert.ToDateTime(CheckTm);
                            junmid.CheckValue = checkstartvalue;//检测初步默认值
                            junmidlist.Add(junmid);
                        }
                        int extresult = repository_JunDataMid.Repository().Insert(junmidlist);//批量插入(执行事务)
                        Message = "载入成功";
                    }
                }
                return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        /// <summary>
        /// 重新载入待录入列表=>删除已有数据(未提交过)
        /// </summary>
        /// <param name="Category"></param>
        /// <returns></returns>
        public ActionResult ReloadEntry(string Category, string BodyNo, string CarType)
        {
            try
            {
                var Message = "重载失败";
                bool result = false;
                string IsOk = "-1";
                //先判断是否已提交过该车身、类别、车型下的检测数据
                string selectdocsql = "select * from QAS_JunctionDataDoc where Category ='" + Category + "' and BodyNo='" + BodyNo + "' and CarType='" + CarType + "'";
                List<QAS_JunctionDataDoc> datadoc = repository_JunDataDoc.Repository().FindListBySql(selectdocsql);
                if (datadoc.Count > 0)
                {
                    return Content(new JsonMessage { Success = result, Code = IsOk, Message = "车身【" + BodyNo + "】已提交过检测数据，无法重载！" }.ToString());
                }
                string selectmidsql = "select * from QAS_JunctionDataMid where Category ='" + Category + "' and BodyNo='" + BodyNo + "' and CarType='" + CarType + "'";
                List<QAS_JunctionDataMid> datamid = repository_JunDataMid.Repository().FindListBySql(selectmidsql);
                if (datamid.Count > 0)
                {
                    //录入中间表存在数据且未提交过=>重载(删除数据)
                    StringBuilder deletemidsql = new StringBuilder();
                    deletemidsql.Append("delete QAS_JunctionDataMid where Category ='" + Category + "' and BodyNo='" + BodyNo + "' and CarType='" + CarType + "'");
                    int extrow = repository_JunDataMid.Repository().ExecuteBySql(deletemidsql);
                    if (extrow > 0)
                    {
                        result = true;
                        IsOk = "1";
                        Message = "重载成功";                       
                    }                                      
                }
                else
                {
                    return Content(new JsonMessage { Success = result, Code = IsOk, Message = "重载失败：未找到需重载的数据！" }.ToString());
                }
                return Content(new JsonMessage { Success = result, Code = IsOk, Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        /// <summary>
        /// 点连接质量录入结果数据提交
        /// </summary>
        /// <param name="entityList">提交List数据</param>
        /// <param name="KeyValue">未用</param>
        /// <returns></returns>
        public ActionResult SubmitJunEntry(List<QAS_JunctionDataMid> entityList, string KeyValue)
        {
            int Entryrow = 0;
            int noEntryrow = 0;
            int IsOk = 0;
            string Message = "提交失败";
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();           
            try
            {
                foreach (QAS_JunctionDataMid entity in entityList)
                {
                    if (entity.CheckValue == "" || entity.CheckValue == null)
                    {
                        noEntryrow++;
                        continue;
                    }
                    QAS_JunctionDataDoc entityListdoc = new QAS_JunctionDataDoc();
                    entityListdoc.RecordId = entity.RecordId;
                    entityListdoc.CarType = entity.CarType;
                    entityListdoc.Code = entity.Code;
                    entityListdoc.WcNm = entity.WcNm;
                    entityListdoc.ItemNm = entity.ItemNm;
                    entityListdoc.PartNm = entity.PartNm;
                    entityListdoc.CCSC = entity.CCSC;
                    entityListdoc.HeadHeghitSta = entity.HeadHeghitSta;
                    entityListdoc.HeadGapSta = entity.HeadGapSta;
                    entityListdoc.LengthSta = entity.LengthSta;
                    entityListdoc.TightenTOR = entity.TightenTOR;
                    entityListdoc.SpotLocation = entity.SpotLocation;
                    entityListdoc.SpotStaValue = entity.SpotStaValue;
                    entityListdoc.Unit = entity.Unit;
                    entityListdoc.Category = entity.Category;
                    entityListdoc.BodyNo = entity.BodyNo;
                    entityListdoc.CheckTm = entity.CheckTm;
                    entityListdoc.CheckValue = entity.CheckValue;
                    entityListdoc.Rem = entity.Rem;
                    entityListdoc.CreTm = DateTime.Now;
                    entityListdoc.CreCd = ManageProvider.Provider.Current().Code;
                    entityListdoc.CreNm = ManageProvider.Provider.Current().UserName;
                    int insetresult = repository_JunDataDoc.Repository().Insert(entityListdoc,isOpenTrans);
                    if (insetresult > 0)//插入Doc成功
                    {
                        int deleteresult = repository_JunDataMid.Repository().Delete(entity.RecordId, isOpenTrans);//删除Mid数据
                    }
                    Entryrow++;
                }
                database.Commit();//提交事务                  
                IsOk = 1;
                Message = "" + Entryrow + "条数据提交成功";
                string selectsql = "select * from QAS_JunctionDataMid where Category ='" + entityList[0].Category + "' and BodyNo='" + entityList[0].BodyNo + "'";
                List<QAS_JunctionDataMid> datarow = repository_JunDataMid.Repository().FindListBySql(selectsql);
                if (datarow.Count > 0)//中间表中存在未完成录入数据=>界面工具栏继续锁定
                {
                    IsOk = 9;//工具栏锁定标识:9
                    if (noEntryrow > 0)//提交数据中CheckValue为空判断
                    {
                        Message = "" + Entryrow + "条数据提交成功，" + noEntryrow + "条未完成录入！";
                    }                                      
                }
                return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        /// <summary>
        /// 获取需导入的SWD检测数据模板
        /// </summary>
        /// <param name="ImportId"></param>
        /// <returns></returns>
        public ActionResult GetExcelModule(string ImportId)
        {
            if (!string.IsNullOrEmpty(ImportId))
            {
                DataSet ds = new DataSet();
                DataTable data = new DataTable();
                string DataColumn = "";
                string fileName = "";
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
                            rows[i] = 0;
                            break;
                        //日期
                        case "2":
                            data.Columns.Add(excelImportDetail.ColumnName, typeof(DateTime));
                            rows[i] = DateTime.Now.Date;
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
                ds.Tables.Add(data);
                MemoryStream ms = DeriveExcel.ExportToExcel(ds, "xls", DataColumn.Split('|'));
                if (!fileName.EndsWith(".xls"))
                {
                    fileName = fileName + ".xls";
                }
                return File(ms, "application/vnd.ms-excel", Url.Encode(fileName));
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// SWD检测结果数据导入
        /// </summary>
        /// <returns></returns>
        public ActionResult ImportDate(string BodyNo, string CarType, DateTime CheckTm)
        {
            int IsOk = 1;//导入状态
            System.Data.DataTable Result = new System.Data.DataTable();//导入错误记录表
            try
            {
                string moduleId = Request["moduleId"]; //表名
                StringBuilder sb_table = new StringBuilder();
                HttpFileCollectionBase files = Request.Files;
                HttpPostedFileBase file = files["filePath"];//获取上传的文件
                string fullname = file.FileName;
                string IsXls = System.IO.Path.GetExtension(fullname).ToString().ToLower();//System.IO.Path.GetExtension获得文件的扩展名
                if (!IsXls.EndsWith(".xls") && !IsXls.EndsWith(".xlsx"))
                {
                    IsOk = 0;
                }
                else
                {
                    string filename = Guid.NewGuid().ToString() + ".xls";
                    if (fullname.EndsWith(".xlsx"))
                    {
                        filename = Guid.NewGuid().ToString() + ".xlsx";
                    }
                    if (file != null && file.FileName != "")
                    {
                        string msg = UploadHelper.FileUpload(file, Server.MapPath("~/Resource/UploadFile/ImportExcel/"), filename);
                    }
                    System.Data.DataTable dt = ImportExcel.ExcelToDataTable(Server.MapPath("~/Resource/UploadFile/ImportExcel/") + filename);
                    DataTable dtnew = RemoveEmpty(dt);//清除空行                 
                    DataView dv = dtnew.DefaultView; //取Excel表中的车型(去重)
                    IsOk = JunSWDBll.ImportSWDData(moduleId, dtnew, BodyNo, CarType, CheckTm, out Result);//数据导入
                }
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Add, "-1", "异常错误：" + ex.Message);
                IsOk = 0;
            }
            if (Result.Rows.Count > 0)
            {
                IsOk = 0;
            }
            var JsonData = new
            {
                Status = IsOk > 0 ? "true" : "false",
                ResultData = Result
            };
            return Content(JsonData.ToJson());
        }
        /// <summary>
        /// 判断车身是否重复录入
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        public ActionResult JudgeRepeat(string KeyValue)
        {
            try
            {
                var Message = "重复提交";
                bool result = false;
                string IsOk = "0";
                //判断是否已提交过该车身的检测数据
                string selectdocsql = "select * from QAS_JunctData_SWD where BodyNo ='" + KeyValue + "'";
                List<QAS_JunctData_SWD> data = repository_JunSWDBase.Repository().FindListBySql(selectdocsql);
                if (data.Count > 0)
                {
                    return Content(new JsonMessage { Success = result, Code = IsOk, Message = Message }.ToString());
                }
                Message = "正常提交";
                result = true;
                IsOk = "1";
                return Content(new JsonMessage { Success = result, Code = IsOk, Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        /// <summary>
        /// 获取SWD检测结果数据
        /// </summary>
        /// <returns></returns>
        public ActionResult GetJunDateSWDList(string PropertyName, string PropertyValue, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<QAS_JunctData_SWD> ListData = JunSWDBll.GetPageList(PropertyName, PropertyValue);
                var JsonData = new
                {
                    total = jqgridparam.total,
                    page = jqgridparam.page,
                    records = jqgridparam.records,
                    costtime = CommonHelper.TimerEnd(watch),
                    rows = ListData,
                };
                return Content(JsonData.ToJson());
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion

        #region 辅助方法        
        /// <summary>
        /// 清除Datatable空行
        /// </summary>
        /// <param name="dt"></param>
        public DataTable RemoveEmpty(DataTable dt)
        {
            List<DataRow> removelist = new List<DataRow>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                bool rowdataisnull = true;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (!string.IsNullOrEmpty(dt.Rows[i][j].ToString().Trim()))
                    {
                        rowdataisnull = false;
                    }
                }
                if (rowdataisnull)
                {
                    removelist.Add(dt.Rows[i]);
                }
            }
            for (int i = 0; i < removelist.Count; i++)
            {
                dt.Rows.Remove(removelist[i]);
            }
            return dt;
        }
        #endregion

        #endregion

    }
}