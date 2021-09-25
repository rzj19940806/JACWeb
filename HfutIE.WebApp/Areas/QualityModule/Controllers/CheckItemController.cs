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
    public class CheckItemController : PublicController<QAS_DTSItemBase>
    {
        #region 列表视图表单
        public ActionResult IndexJunct()
        {
            return View();
        }
        public ActionResult IndexTorsn()
        {
            return View();
        }
        public ActionResult FormJunct()
        {
            return View();
        }
        public ActionResult FormTorsn()
        {
            return View();
        }
        /// <summary>
        /// 检测项导入Excel弹出框页面
        /// </summary>
        /// <returns></returns>
        public ActionResult ExcelImport(string Category)
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
                ViewBag.Category = Category;
            }
            else
            {
                ViewBag.ModuleId = "0";
            }
            return View();
        }
        #endregion

        #region 全局变量区域
        public readonly RepositoryFactory<QAS_DTSItemBase> repository_DTSItemBase = new RepositoryFactory<QAS_DTSItemBase>();
        public readonly RepositoryFactory<QAS_DTSItemStaBase> repository_DTSItemStaBase = new RepositoryFactory<QAS_DTSItemStaBase>();
        public readonly RepositoryFactory<QAS_TorsionItemBase> repository_TorsionItemBase = new RepositoryFactory<QAS_TorsionItemBase>();
        public readonly RepositoryFactory<QAS_JunctionItemBase> repository_JunctionItemBase = new RepositoryFactory<QAS_JunctionItemBase>();
        public JavaScriptSerializer js = new JavaScriptSerializer();//反序列化    
        #endregion

        #region 创建数据库操作对象区域
        QAS_DTSItemBaseBll DTSItemBll = new QAS_DTSItemBaseBll();
        QAS_DTSItemStaBaseBll DTSItemStaBll = new QAS_DTSItemStaBaseBll();
        QAS_TorsionItemBaseBll TorsionItemBll = new QAS_TorsionItemBaseBll();
        QAS_JunctionItemBaseBll JunctionItemBll = new QAS_JunctionItemBaseBll();
        #endregion

        #region 方法区

        #region 公共方法区
        /// <summary>
        /// 获取导出模板
        /// </summary>
        /// <param name="ImportId"></param>
        /// <param name="data"></param>
        /// <param name="DataColumn"></param>
        /// <param name="fileName"></param>
        public ActionResult GetExcellTemperature(string ImportId, string Category)
        {
            if (!string.IsNullOrEmpty(ImportId))
            {
                DataSet ds = new DataSet();
                DataTable data = new DataTable();
                string DataColumn = "";
                string fileName = "";
                if (Category == "Tor")
                {
                    TorsionItemBll.GetExcellTemperature(ImportId, out data, out DataColumn, out fileName);
                }
                else
                {
                    JunctionItemBll.GetExcellTemperature(ImportId, Category, out data, out DataColumn, out fileName);
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
        /// 导入Excell数据
        /// </summary>
        /// <param name="Category"></param>
        /// <returns></returns>
        public ActionResult ImportExcell(string Category)
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
                    DataTable dtcartype = dv.ToTable(true, "车型");
                    string[] cartypes = new string[dtcartype.Rows.Count];
                    for (int i = 0; i < cartypes.Length;i++)
                    {
                        cartypes[i] = dtcartype.Rows[i][0].ToString().Trim();
                    }
                    if (Category == "Tor")
                    {
                        IsOk = TorsionItemBll.ImportExcelTor(moduleId, dtnew, cartypes, out Result);//Tor数据导入
                    }
                    else
                    {
                        IsOk = JunctionItemBll.ImportExcelJun(moduleId, Category, cartypes, dtnew, out Result);//Jun数据导入
                    }               
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

        #endregion

        #region 1-DTS检测项维护
        /// <summary>
        /// 新增/编辑DTS检测项
        /// </summary>
        /// <param name="KeyValue">DTS主键</param>
        /// <param name="entity">DTS检测项基本信息</param>
        /// <param name="ConfigInfoJson">配置信息</param>
        /// <returns></returns>
        public ActionResult SubmitFormDTS(string KeyValue, QAS_DTSItemBase entity, string ConfigInfoJson)
        {
            try
            {
                float result;                
                string Message = KeyValue == "" ? "新增成功！" : "编辑成功！";
                bool isrepeat = true;//判断重复
                List<QAS_DTSItemBase> list = repository_DTSItemBase.Repository().FindListBySql("select * from QAS_DTSItemBase");//获取DTSItemBase数据
                //list空行=>直接判定未重复；list不为空=>存在该值，可能是编辑的这条数据
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    QAS_DTSItemBase itementity = list.Where(s => s.RecordId == KeyValue).FirstOrDefault();
                    list.Remove(itementity);
                }
                isrepeat = list.Where(s => s.DTSItemID == entity.DTSItemID).Count() == 0 ? false : isrepeat;
                if (isrepeat)
                {
                    Message = "该编号已经存在！";
                    return Content(new JsonMessage { Success = true, Code = "0", Message = Message }.ToString());
                }
                List<QAS_DTSItemStaDto> dtsconfigdto = js.Deserialize<List<QAS_DTSItemStaDto>>(ConfigInfoJson);//反序列化=>有float字段会报错，从index输入防错移到controller处理
                List<QAS_DTSItemStaBase> dtsconfig = new List<QAS_DTSItemStaBase>();
                if (dtsconfigdto.Count != 0)
                {
                    foreach (var item in dtsconfigdto)
                    {
                        if (!string.IsNullOrEmpty(item.SerialNumber))
                        {
                            if (!Single.TryParse(item.BaseValue, out result) || !Single.TryParse(item.TolValue, out result) || !Single.TryParse(item.TOLUp, out result) || !Single.TryParse(item.TOLDown, out result))
                            {
                                return Content(new JsonMessage { Success = true, Code = "0", Message = "数据格式异常" }.ToString());
                            }
                            else
                            {
                                QAS_DTSItemStaBase itemsta = new QAS_DTSItemStaBase();
                                itemsta.BaseValue = float.Parse(item.BaseValue);
                                itemsta.TolValue = float.Parse(item.TolValue);
                                itemsta.TOLUp = float.Parse(item.TOLUp);
                                itemsta.TOLDown = float.Parse(item.TOLDown);
                                itemsta.SerialNumber = item.SerialNumber;
                                itemsta.Direction = item.Direction;
                                dtsconfig.Add(itemsta);
                            }
                        }
                    }
                }
                //List<QAS_DTSItemStaBase> dtsconfig = js.Deserialize<List<QAS_DTSItemStaBase>>(ConfigInfoJson);//反序列化
                if (!string.IsNullOrEmpty(KeyValue))//编辑
                {
                    entity.MdfTm = DateTime.Now;
                    entity.MdfCd = ManageProvider.Provider.Current().Code;
                    entity.MdfNm = ManageProvider.Provider.Current().UserName;
                    int extdts = repository_DTSItemBase.Repository().Update(entity);
                    if (extdts > 0)
                    {
                        //string strSql = "select * from QAS_DTSItemStaBase where ParId = '" + KeyValue + "'";
                        DataTable data = repository_DTSItemStaBase.Repository().FindTable("ParId", KeyValue);
                        if (data.Rows.Count > 0)
                        {
                            //先删除
                            //string[] idlist = new string[data.Rows.Count];                       
                            for (int i = 0; i < data.Rows.Count; i++)
                            {
                                //idlist[i] = data.Rows[i]["RecordId"].ToString();
                                repository_DTSItemStaBase.Repository().Delete(data.Rows[i]["RecordId"]);
                            }
                        }
                        if (dtsconfig.Count != 0)
                        {
                            foreach (var item in dtsconfig)
                            {
                                if (!string.IsNullOrEmpty(item.SerialNumber))//以上数据防错处理后，此判断无意义
                                {
                                    QAS_DTSItemStaBase staentity = new QAS_DTSItemStaBase();
                                    staentity.RecordId = Guid.NewGuid().ToString();
                                    staentity.ParId = KeyValue;
                                    staentity.SerialNumber = item.SerialNumber;
                                    staentity.BaseValue = item.BaseValue;
                                    staentity.TolValue = item.TolValue;
                                    staentity.Direction = item.Direction;
                                    staentity.TOLUp = item.TOLUp;
                                    staentity.TOLDown = item.TOLDown;
                                    staentity.Enabled = "1";
                                    staentity.MdfTm = DateTime.Now;
                                    repository_DTSItemStaBase.Repository().Insert(staentity);
                                }
                            }
                        }
                    }
                    else
                    {
                        WriteLog(-1, entity, null, KeyValue, "编辑操作失败：" + "0");
                        return Content(new JsonMessage { Success = false, Code = "-1", Message = "编辑失败" }.ToString());
                    }                   
                }
                else
                {
                    string ID = Guid.NewGuid().ToString();
                    entity.RecordId = ID;
                    entity.CreTm = DateTime.Now;
                    entity.CreCd = ManageProvider.Provider.Current().Code;
                    entity.CreNm = ManageProvider.Provider.Current().UserName;
                    int extdts = repository_DTSItemBase.Repository().Insert(entity);
                    if (extdts > 0)
                    {
                        if (dtsconfig.Count() != 0)
                        {
                            //List<QAS_DTSItemStaBase> dtssta = new List<QAS_DTSItemStaBase>();
                            foreach (var item in dtsconfig)
                            {
                                if (!string.IsNullOrEmpty(item.SerialNumber))
                                {
                                    QAS_DTSItemStaBase staentity = new QAS_DTSItemStaBase();
                                    staentity.RecordId = Guid.NewGuid().ToString();
                                    staentity.ParId = ID;
                                    staentity.SerialNumber = item.SerialNumber;
                                    staentity.BaseValue = item.BaseValue;
                                    staentity.TolValue = item.TolValue;
                                    staentity.Direction = item.Direction;
                                    staentity.TOLUp = item.TOLUp;
                                    staentity.TOLDown = item.TOLDown;
                                    staentity.Enabled = "1";
                                    staentity.CreTm = DateTime.Now;
                                    repository_DTSItemStaBase.Repository().Insert(staentity);
                                }
                            }
                        }                     
                    }
                    else
                    {
                        WriteLog(-1, entity,null, KeyValue, "新增操作失败：" + "0");
                        return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败" }.ToString());
                    }   
                }
                return Content(new JsonMessage { Success = true, Code = 1.ToString(), Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                WriteLog(-1, entity, null, KeyValue, "操作失败：" + ex.Message);//记录日志
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        /// <summary>
        /// 查询DTS检测项
        /// </summary>
        /// <param name="PropertyName"></param>
        /// <param name="PropertyValue"></param>
        /// <param name="jqgridparam"></param>
        /// <returns></returns>
        public ActionResult GetDTSItemListPage(string PropertyName, string PropertyValue, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<QAS_DTSItemBase> ListData = DTSItemBll.GetPageList(PropertyName, PropertyValue);
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
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 单条件查询DTS检测项配置
        /// </summary>
        /// <param name="PropertyName"></param>
        /// <param name="PropertyValue"></param>
        /// <param name="jqgridparam"></param>
        /// <returns></returns>
        public ActionResult GetItemStaList(string PropertyName, string PropertyValue, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<QAS_DTSItemStaBase> ListData = DTSItemStaBll.GetPageList(PropertyName, PropertyValue);
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
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 根据主键获取DTS检测项实体JSON
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        public  ActionResult GridItemJson(string KeyValue)
        {
            try
            {
                QAS_DTSItemBase entity = repository_DTSItemBase.Repository().FindEntity(KeyValue);
                return Content(entity.ToJson());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        public ActionResult DTSItemDelete(string KeyValue)
        {
            string[] array = KeyValue.Split(',');
            try
            {
                var Message = "删除失败。";            
                int IsOk = 0;
                //直接删除，先删除检测项配置，再删除检测项
                if (array != null && array.Length > 0)
                {
                    for (int i = 0; i < array.Length; i++)
                    {
                        int extrow1 = repository_DTSItemStaBase.Repository().Delete("ParId", array[i]);//删配置项
                        int extrow2 = repository_DTSItemBase.Repository().Delete(array[i]);//删除检测项        
                    }
                    Message = "删除成功。";
                }
                WriteLog(IsOk, array, Message);
                return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                WriteLog(-1, array, "操作失败：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        /// <summary>
        /// DTS新增编号获取
        /// </summary>
        /// <param name="PropValue1"></param>
        /// <param name="PropValue2"></param>
        /// <returns></returns>
        public ActionResult DTSCodeGet(string PropValue1, string PropValue2)
        {
            try
            {
                string DTSCode = "";
                string Location = "";
                if (PropValue2 == "右侧")
                {
                    Location = "R";
                }
                else
                {
                    Location = "L";
                }
                string selectsql = "select top 1 DTSItemID from QAS_DTSItemBase where CarType='" + PropValue1 + "' and Location = '" + PropValue2 + "' order by DTSItemID desc";
                List<QAS_DTSItemBase> ListData = repository_DTSItemBase.Repository().FindListBySql(selectsql);
                if (ListData.Count > 0)
                {
                    int num = Convert.ToInt16(ListData[0].DTSItemID.Substring(4)) + 1;//去掉前4个字符=>得到序号
                    if (num<10 && num >0)
                    {
                        DTSCode = PropValue1 + Location + "00" + num.ToString();
                    }
                    else if (num<100 && num >= 10)
                    {
                        DTSCode = PropValue1 + Location + "0" + num.ToString();
                    }
                    else
                    {
                        //DTS检测项不超过999项
                        DTSCode = PropValue1 + Location + num.ToString();
                    }                    
                }
                else
                {
                    DTSCode = PropValue1 + Location + "001";
                }
                var JsonData = new
                {
                    Code = DTSCode
                };
                return Content(JsonData.ToJson());
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion

        #region 2-扭力检测项维护
        /// <summary>
        /// 查询扭力检测项
        /// </summary>
        /// <param name="PropertyName"></param>
        /// <param name="PropertyValue"></param>
        /// <param name="jqgridparam"></param>
        /// <returns></returns>
        public ActionResult GetTorItemList(string PropertyName, string PropertyValue, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<QAS_TorsionItemBase> ListData = TorsionItemBll.GetPageList(PropertyName, PropertyValue);
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
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 删除Torsn检测项
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        public ActionResult TorItemDelete(string KeyValue)
        {
            string[] array = KeyValue.Split(',');
            try
            {
                var Message = "删除失败。";
                int IsOk = 0;
                //直接删除Tor检测项
                if (array != null && array.Length > 0)
                {
                    for (int i = 0; i < array.Length; i++)
                    {
                        int extrow2 = repository_TorsionItemBase.Repository().Delete(array[i]);//删除检测项  
                    }
                    Message = "删除成功。";
                }
                WriteLog(IsOk, array, Message);
                return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                WriteLog(-1, array, "操作失败：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        /// <summary>
        /// 根据主键获取Tor扭力检测项实体JSON
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        public ActionResult GridTorItemJson(string KeyValue)
        {
            try
            {
                QAS_TorsionItemBase entity = repository_TorsionItemBase.Repository().FindEntity(KeyValue);
                return Content(entity.ToJson());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        /// <summary>
        /// 新增/编辑Tor检测项
        /// </summary>
        /// <param name="KeyValue">主键</param>
        /// <param name="entity">Tor扭力检测项信息</param>
        /// <returns></returns>
        public ActionResult SubmitFormTor(string KeyValue, QAS_TorsionItemBase entity)
        {
            try
            {
                int extresult;
                string Message = KeyValue == "" ? "新增成功！" : "编辑成功！";
                bool isrepeat = true;//判断重复
                List<QAS_TorsionItemBase> list = new List<QAS_TorsionItemBase>();
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    list = repository_TorsionItemBase.Repository().FindListBySql("select * from QAS_TorsionItemBase where RecordId != '" + KeyValue + "' and TORItemID ='" + entity.TORItemID + "'");
                }
                else
                {
                    list = repository_TorsionItemBase.Repository().FindListBySql("select * from QAS_TorsionItemBase where TORItemID ='" + entity.TORItemID + "'");
                }
                isrepeat = list.Count() == 0 ? false : isrepeat;
                if (isrepeat)
                {
                    Message = "该编号已经存在！";
                    return Content(new JsonMessage { Success = true, Code = "0", Message = Message }.ToString());
                }

                if (!string.IsNullOrEmpty(KeyValue))//编辑
                {
                    entity.MdfTm = DateTime.Now;
                    entity.MdfCd = ManageProvider.Provider.Current().Code;
                    entity.MdfNm = ManageProvider.Provider.Current().UserName;
                    extresult = repository_TorsionItemBase.Repository().Update(entity);        
                }
                else
                {
                    string ID = Guid.NewGuid().ToString();
                    entity.RecordId = ID;
                    entity.CreTm = DateTime.Now;
                    entity.CreCd = ManageProvider.Provider.Current().Code;
                    entity.CreNm = ManageProvider.Provider.Current().UserName;
                    extresult = repository_TorsionItemBase.Repository().Insert(entity);
                }
                if (extresult < 1)
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败！" }.ToString());
                }
                return Content(new JsonMessage { Success = true, Code = 1.ToString(), Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        #endregion

        #region 3-点连接检测项维护
        /// <summary>
        /// 查询点连接检测项(单条件)
        /// </summary>
        /// <param name="PropertyName"></param>
        /// <param name="PropertyValue"></param>
        /// <param name="jqgridparam"></param>
        /// <returns></returns>
        public ActionResult GetJunItemList(string PropertyName, string PropertyValue, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<QAS_JunctionItemBase> ListData = JunctionItemBll.GetPageList(PropertyName, PropertyValue);
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
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 查询点连接检测项(双条件)
        /// </summary>
        /// <param name="PropertyName1">condition值</param>
        /// <param name="PropertyValue1"></param>
        /// <param name="PropertyName2"></param>
        /// <param name="PropertyValue2"></param>
        /// <param name="jqgridparam"></param>
        /// <returns></returns>
        public ActionResult GetJunItemList2(string PropertyName1, string PropertyValue1, string PropertyName2, string PropertyValue2, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<QAS_JunctionItemBase> ListData = JunctionItemBll.GetPageList(PropertyName1, PropertyValue1, PropertyName2, PropertyValue2);
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
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 根据主键获取点连接检测项实体JSON
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        public ActionResult GridJunItemJson(string KeyValue)
        {
            try
            {
                QAS_JunctionItemBase entity = repository_JunctionItemBase.Repository().FindEntity(KeyValue);
                return Content(entity.ToJson());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        /// <summary>
        /// 新增/编辑点连接检测项
        /// </summary>
        /// <param name="KeyValue">主键</param>
        /// <param name="entity">点连接检测项信息</param>
        /// <returns></returns>
        public ActionResult SubmitFormJun(string KeyValue, QAS_JunctionItemBase entity)
        {
            try
            {
                int extresult;
                string Message = KeyValue == "" ? "新增成功！" : "编辑成功！";
                if (!string.IsNullOrEmpty(KeyValue))//编辑
                {
                    entity.MdfTm = DateTime.Now;
                    entity.MdfCd = ManageProvider.Provider.Current().Code;
                    entity.MdfNm = ManageProvider.Provider.Current().UserName;
                    extresult = repository_JunctionItemBase.Repository().Update(entity);
                }
                else
                {
                    string ID = Guid.NewGuid().ToString();
                    entity.RecordId = ID;
                    entity.CreTm = DateTime.Now;
                    entity.CreCd = ManageProvider.Provider.Current().Code;
                    entity.CreNm = ManageProvider.Provider.Current().UserName;
                    extresult = repository_JunctionItemBase.Repository().Insert(entity);
                }
                if (extresult < 1)
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败！" }.ToString());
                }
                return Content(new JsonMessage { Success = true, Code = 1.ToString(), Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        /// <summary>
        /// 删除Torsn检测项
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        public ActionResult JunItemDelete(string KeyValue)
        {
            string[] array = KeyValue.Split(',');
            try
            {
                var Message = "删除失败。";
                int IsOk = 0;
                //直接删除Jun点连接检测项
                if (array != null && array.Length > 0)
                {
                    for (int i = 0; i < array.Length; i++)
                    {
                        int extrow = repository_JunctionItemBase.Repository().Delete(array[i]);//删除检测项  
                    }
                    Message = "删除成功。";
                }
                WriteLog(IsOk, array, Message);
                return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                WriteLog(-1, array, "操作失败：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
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