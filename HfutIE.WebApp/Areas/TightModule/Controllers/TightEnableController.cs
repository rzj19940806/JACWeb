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

namespace HfutIE.WebApp.Areas.TightModule.Controllers
{
    /// <summary>
    /// 工位基础信息表控制器
    /// </summary>
    public class TightEnableController : PublicController<Tg_JobEnableConfig>
    {
        #region 全局变量定义区
        //定义本页面主要操作的表的表名，称为主表
        public static DataTable WcList = new DataTable();
        //public static DataTable QPointList = new DataTable();
        #endregion

        #region 创建数据库操作对象区域
        //创建数据库访问对象，用以访问其中操作数据库的方法
        TightEnableBll MyBll = new TightEnableBll(); //===复制时需要修改===
        //BBdbR_QualityCheckPointCarComponentConfigBll MyBll1 = new BBdbR_QualityCheckPointCarComponentConfigBll(); //===复制时需要修改===
        public readonly RepositoryFactory<Tg_JobEnableConfig> repository_plinebase = new RepositoryFactory<Tg_JobEnableConfig>();
        #endregion

        #region 打开视图区域

        public ActionResult QualityIndex()
        {
            return View();
        }
        public ActionResult QForm()
        {
            return View();
        }
        public ActionResult ConfigComponent()
        {
            return View();
        }

        #endregion

        #region 方法区   




        #region 4.删除方法
        /// <summary>
        /// 首先判断需要删除的信息是否绑定了其他信息
        ///     如：删除一条公司信息先要判断该条公司信息下面是否绑定了工厂信息
        ///         如果绑定了信息，则提示“当前所选有子节点数据，不能删除。”并结束
        ///  在确定没绑定数据的情况下，删除该数据
        ///     删除表示将该数据的IsAvailable属性设为false，并不真的删除该记录
        /// </summary>
        /// <param name="KeyValue">页面中提供的需要删除的数据的主键,可能是多条数据的主键，即多个主键</param>
        /// <param name="ParentId">不需要</param>
        /// <param name="DeleteMark">不需要</param>
        /// <returns></returns>
        public ActionResult DeleteWc(string KeyValue)
        {
            //不管是多个主键还是单个主键，将主键拆分出来，放在数组中
            string[] array = KeyValue.Split(',');
            try
            {
                var Message = "删除失败。";//定义返回信息，该信息将返回到界面上，给用户观看
                int IsOk = 0;//判断删除方法是否成，0表示不成功，大于0表示成功
                for (int i = 0; i < array.Length; i++)
                {
                    Tg_JobEnableConfig Oldentity = repositoryfactory.Repository().FindEntity(array[i]);
                    int nums = MyBll.hasChildNode(Oldentity.ID);
                    if (nums > 0)
                    {
                        throw new Exception("当前所选有子节点数据，不能删除。");
                    }
                    else
                    {
                        IsOk = MyBll.DeleteUseEnabled(array[i]);
                        if (IsOk > 0) Message = "删除成功。";
                    }

                }
                WriteLog(IsOk, array, Message);
                return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                WriteLog(-1, array, "操作失败：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion

        #region 6.检查字段是否唯一
        /// <summary>
        /// 根据传入的字段名和字段值判断该字段是否存在
        /// </summary>
        /// <param name="KeyName">字段名</param>
        /// <param name="KeyValue">字段值</param>
        /// <returns>返回该判断结果</returns>
        public ActionResult ChectOnlyOne(string KeyName, string KeyValue)
        {
            try
            {
                int IsOk = 0;
                string Message = "该字段已经存在！";

                if (!string.IsNullOrEmpty(KeyValue))
                {
                    IsOk = MyBll.CheckCount(KeyName, KeyValue);
                }
                if (IsOk > 0)
                {
                    return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = Message }.ToString());
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        #endregion

        #region 7.编辑填充界面数据
        public override ActionResult SetForm(string KeyValue)
        {
            try
            {
                Tg_JobEnableConfig Plineentity = MyBll.GetPlanList1(KeyValue);

                return Content(Plineentity.ToJson());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion

        #region 8.导入
        /// <summary>
        /// 导入Excel弹出框页面
        /// </summary>
        /// <returns></returns>
        [ManagerPermission(PermissionMode.Enforce)]
        public ActionResult ExcelImportDialog(string area_key)
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
                ViewBag.area_key = area_key;
            }
            else
            {
                ViewBag.ModuleId = "0";
                ViewBag.area_key = area_key;
            }
            return View();
        }

        #region 导出模板
        /// <summary>
        /// 下载Excell模板
        /// </summary>
        /// <returns></returns>
        public ActionResult GetExcellTemperature(string ImportId)
        {
            if (!string.IsNullOrEmpty(ImportId))
            {
                DataSet ds = new DataSet();
                DataTable data = new DataTable(); string DataColumn = ""; string fileName;
                MyBll.GetExcellTemperature(ImportId, out data, out DataColumn, out fileName);
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
        #endregion

        /// <summary>
        /// 清除Datatable空行
        /// </summary>
        /// <param name="dt"></param>
        public void RemoveEmpty(DataTable dt)
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
        }

        /// <summary>
        /// 导入Excell数据
        /// </summary>
        /// <returns></returns>
        public ActionResult ImportExel(string area_key)
        {
            int IsOk = 0;//导入状态
            int IsCheck = 1;//用作检验重复地址的标识
            DataTable Result = new DataTable();//导入错误记录表
            IDatabase database = DataFactory.Database();
            List<Tg_JobEnableConfig> CntlAddrList = new List<Tg_JobEnableConfig>();

            //构造导入返回结果表
            DataTable Newdt = new DataTable("Result");
            Newdt.Columns.Add("rowid", typeof(System.String));                 //行号
            Newdt.Columns.Add("locate", typeof(System.String));                 //位置
            Newdt.Columns.Add("reason", typeof(System.String));                 //原因
            int errorNum = 1;
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

                    DataTable dt = ImportExcel.ExcelToDataTable(Server.MapPath("~/Resource/UploadFile/ImportExcel/") + filename);

                    RemoveEmpty(dt);//清除空行。???=>20210712注：方法是否真的有用？void返回对dt未生效
                    dt.Columns.Add("rowid", typeof(System.String));//用来标识excell行ID
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dt.Rows[i]["rowid"] = i + 1;
                    }
                    #region JOB使能配置信息导入
                    //校验
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        IsCheck = 1;//重置标识
                        DataRow dr = Newdt.NewRow();
                        if (dt.Rows[i]["类型"].ToString().Trim() != "物料" && dt.Rows[i]["类型"].ToString().Trim() != "产品")
                        {
                            dr = Newdt.NewRow();
                            dr[0] = errorNum;
                            dr[1] = "第[" + dt.Rows[i]["rowid"].ToString() + "]行[类型]";
                            dr[2] = "类型错误";
                            Newdt.Rows.Add(dr);
                            errorNum++;
                            IsCheck = 0;
                            continue;
                        }
                        if (dt.Rows[i]["编号"].ToString().Trim() != ""|| dt.Rows[i]["工位JOB编号"].ToString().Trim() != "")
                        {
                            int DeviceCount = MyBll.CheckCount( dt.Rows[i]["编号"].ToString(), dt.Rows[i]["工位JOB编号"].ToString());//是否有相同的JOB配置
                            int DeviceCount2 = MyBll.CheckCount2("tg_wcjobconfig", dt.Rows[i]["工位JOB编号"].ToString());//是否有该JOB编号
                            int DeviceCount3 = MyBll.CheckCount2(dt.Rows[i]["类型"].ToString(), dt.Rows[i]["编号"].ToString());//是否有该实体编号

                            if (DeviceCount > 0)
                            {
                                dr = Newdt.NewRow();
                                dr[0] = errorNum;
                                dr[1] = "第[" + dt.Rows[i]["rowid"].ToString() + "]行[编号]";
                                dr[2] = "JOB使能配置在系统中已存在不能重复插入";
                                Newdt.Rows.Add(dr);
                                errorNum++;
                                IsCheck = 0;
                                continue;
                            }
                            else if(DeviceCount2 < 1)
                            {
                                dr = Newdt.NewRow();
                                dr[0] = errorNum;
                                dr[1] = "第[" + dt.Rows[i]["rowid"].ToString() + "]行[工位JOB编号]";
                                dr[2] = "系统内未找到该工位JOB编号";
                                Newdt.Rows.Add(dr);
                                errorNum++;
                                IsCheck = 0;
                                continue;
                            }
                            else if (DeviceCount3 < 1)
                            {
                                dr = Newdt.NewRow();
                                dr[0] = errorNum;
                                dr[1] = "第[" + dt.Rows[i]["rowid"].ToString() + "]行[编号]";
                                dr[2] = "系统内未找到该" + dt.Rows[i]["类型"].ToString() + "编号";
                                Newdt.Rows.Add(dr);
                                errorNum++;
                                IsCheck = 0;
                                continue;
                            }
                            else
                            {


                                Tg_JobEnableConfig pushGetItem = new Tg_JobEnableConfig();
                                pushGetItem.WJCId = dt.Rows[i]["工位JOB编号"].ToString().Trim();
                                pushGetItem.Code = dt.Rows[i]["编号"].ToString().Trim();
                                pushGetItem.Type = dt.Rows[i]["类型"].ToString().Trim();
                                pushGetItem.Rem = dt.Rows[i]["备注"].ToString().Trim();
                                pushGetItem.CreTm = DateTime.Now;  //修改时间
                                pushGetItem.CreCd = ManageProvider.Provider.Current().UserId; //修改人编号
                                pushGetItem.CreNm = ManageProvider.Provider.Current().UserName;//修改人名称

                                CntlAddrList.Add(pushGetItem);

                                int b = database.Insert(CntlAddrList);
                                if (b > 0)
                                {
                                    IsOk = IsOk + b;
                                    CntlAddrList.Clear();
                                }
                                else
                                {
                                    dr = Newdt.NewRow();
                                    dr[0] = errorNum;
                                    dr[1] = "第[" + dt.Rows[i]["rowid"].ToString() + "]行";
                                    dr[2] = "工位信息导入失败";
                                    Newdt.Rows.Add(dr);
                                    IsCheck = 0;
                                    continue;
                                }
                            }
                        }
                        else
                        {
                            dr = Newdt.NewRow();
                            dr[0] = errorNum;
                            dr[1] = "第[" + dt.Rows[i]["rowid"].ToString() + "]行";
                            dr[2] = "编号及工位JOB编号不能为空";
                            Newdt.Rows.Add(dr);
                            errorNum++;
                            IsCheck = 0;
                            continue;
                        }
                    }
                    if (IsCheck == 0)
                    {
                        IsOk = 0;
                    }
                    #endregion

                }
                Result = Newdt;
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

        #endregion

        #region 9.质控点方法区

        #region 5.查询方法
        //查询方法，本方法为单条件查询，即根据一个条件进行查询
        //查询条件为Condition，也是数据库表_CompanyBaseInformation中的一个字段名
        //查询值为keywords，也是数据库表_CompanyBaseInformation中的字段名的字段值
        //本查询采用近似查询（like）

        public ActionResult GridPageByCondition(string type, string KeyValue,string WcCode, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                DataTable dt = MyBll.GetPageListByCondition(type, KeyValue, WcCode, jqgridparam);
                var JsonData = new
                {
                    total = jqgridparam.total,
                    page = jqgridparam.page,
                    records = jqgridparam.records,
                    costtime = CommonHelper.TimerEnd(watch),
                    rows = dt,
                };
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "1", "JOB信息查询成功");
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                //CCSLog.CCSLogHelper.WriteExLog(ex, CCSLog.LogType.WebSite);
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "JOB信息查询发生异常错误：" + ex.Message);
                return null;
            }
        }

        public ActionResult GridPageByConditionEnable(string type, string KeyValue,string WcCode, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                DataTable dt = MyBll.GetPageListByConditionEnable(type, KeyValue, WcCode, jqgridparam);
                var JsonData = new
                {
                    total = jqgridparam.total,
                    page = jqgridparam.page,
                    records = jqgridparam.records,
                    costtime = CommonHelper.TimerEnd(watch),
                    rows = dt,
                };
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "1", "JOB使能信息查询成功");
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                //CCSLog.CCSLogHelper.WriteExLog(ex, CCSLog.LogType.WebSite);
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "JOB使能信息查询发生异常错误：" + ex.Message);
                return null;
            }
        }
        public ActionResult GridPageByConditionTorque(string type, string KeyValue, string WcCode, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                string sql = "";
                DataTable dt = new DataTable();
                sql = @"SELECT * FROM dbo.Tg_JobTorqueConfig WHERE ID>0 and WcJobCd='" + KeyValue + "' order by Ord ";
                dt= DbHelperSQL.OpenTable(sql);
                var JsonData = new
                {
                    total = jqgridparam.total,
                    page = jqgridparam.page,
                    records = jqgridparam.records,
                    costtime = CommonHelper.TimerEnd(watch),
                    rows = dt,
                };
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "1", "JOB使能信息查询成功");
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                //CCSLog.CCSLogHelper.WriteExLog(ex, CCSLog.LogType.WebSite);
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "JOB使能信息查询发生异常错误：" + ex.Message);
                return null;
            }
        }

        #endregion

        #region 9.1 加载未配置物料或产品
        public ActionResult GridNotConfigJson(string JOBId, string Condition, string keywords, JqGridParam jqgridparam)//加载未配置组件
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                DataTable dt = MyBll.GetNotConfig(JOBId, Condition, keywords, ref jqgridparam);
                var JsonData = new
                {
                    total = jqgridparam.total,
                    page = jqgridparam.page,
                    records = jqgridparam.records,
                    costtime = CommonHelper.TimerEnd(watch),
                    rows = dt,
                };
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                this.WriteLog(-1, null, null, null, "操作失败" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion
        #region 9.2组件配置提交
        public ActionResult JobEnableSubmit(string JOBId, List<Tg_JobEnableConfig> tabledata)//组件配置提交
        {
            try
            {
                int IsOk = 0;//用于判断提交是否成功，成功为1，失败为0
                //DataTable dt = MyBll1.GetPlineId(WcId);
                //string PlineId = dt.Rows[0]["PlineId"].ToString();

                for (int i = 0; i < tabledata.Count; i++)
                {
                    //tabledata[i].ConfigId = System.Guid.NewGuid().ToString();
                    tabledata[i].WJCId = JOBId;
                    tabledata[i].CreTm = DateTime.Now;
                    tabledata[i].CreCd = ManageProvider.Provider.Current().UserId;
                    tabledata[i].CreNm = ManageProvider.Provider.Current().UserName;
                    IsOk = MyBll.JOBEnableInsert(tabledata[i]);
                }
                Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other, "1", "JOB使能配置操作成功");
                return Content(new JsonMessage { Success = true, Code = IsOk.ToString() }.ToString());
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other, "-1", "JOB使能配置操作失败：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion


        #region 9.4.删除配置
        public ActionResult ConfigDelete(string KeyValue)
        {
            //不管是多个主键还是单个主键，将主键拆分出来，放在数组中
            string[] array = KeyValue.Split(',');
            try
            {
                var Message = "删除JOB使能配置失败。";//定义返回信息，该信息将返回到界面上，给用户观看
                int IsOk = 0;//判断删除方法是否成，0表示不成功，大于0表示成功
                for (int i = 0; i < array.Length; i++)
                {

                    IsOk = MyBll.ConfigDelete(array[i]);
                    if (IsOk > 0) Message = "删除JOB使能配置成功。";

                }
                WriteLog(IsOk, array, Message);
                return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                WriteLog(-1, array, "操作失败：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion

        #endregion



        #region 11.重构导出方法-质控点
        /// <summary>
        /// 1.如果是按照条件查询后再进行
        /// </summary>
        /// <param name="type"></param>
        /// <param name="jqgridparam"></param>
        /// <returns></returns>
        public ActionResult GetExcel_DataQuality(string area_key, string parentId, string sort, string WcCd, string WcNm, JqGridParam jqgridparam)
        {
            #region 查询
            Stopwatch watch = CommonHelper.TimerStart();
            StringBuilder strSql = new StringBuilder();
            if (area_key == "" && parentId == "")
            {
                strSql.Append(@"select a.*,b.PlineCd as PlineCd,b.PlineNm as PlineNm from BBdbR_WcBase a join BBdbR_PlineBase b on a.PlineId=b.PlineId join BBdbR_WorkSectionBase c on b.WorkSectionId=c.WorkSectionId join BBdbR_WorkshopBase d on d.WorkshopId=c.WorkshopId where a.Enabled=1 and b.Enabled=1 and d.WorkshopCd='ZLCJXN01' ");
            }
            else
            {
                if (parentId != "0")
                {
                    if (sort == "0")
                    {
                        strSql.Append(@"select a.*,b.PlineCd as PlineCd,b.PlineNm as PlineNm from BBdbR_WcBase a join BBdbR_PlineBase b on a.PlineId=b.PlineId join BBdbR_WorkSectionBase c on b.WorkSectionId=c.WorkSectionId where a.Enabled=1 and b.Enabled=1 and c.Enabled=1 and c.WorkSectionId='" + area_key + "' ");     //===复制时需要修改===
                    }
                    else if (sort == "1")
                    {
                        strSql.Append(@"select a.*,b.PlineCd as PlineCd,b.PlineNm as PlineNm from BBdbR_WcBase a join BBdbR_PlineBase b on a.PlineId=b.PlineId join BBdbR_WorkSectionBase c on b.WorkSectionId=c.WorkSectionId where a.Enabled=1 and b.Enabled=1 and c.Enabled=1 and c.WorkSectionId='" + area_key + "' ");     //===复制时需要修改===
                    }
                    else
                    {
                        strSql.Append(@"select a.*,b.PlineCd as PlineCd,b.PlineNm as PlineNm from BBdbR_WcBase a join BBdbR_PlineBase b on a.PlineId=b.PlineId where a.Enabled=1 and b.Enabled=1 and a.PlineId='" + area_key + "' ");     //===复制时需要修改===要修改===
                    }
                }
                else
                {
                    strSql.Append(@"select a.*,b.PlineCd as PlineCd,b.PlineNm as PlineNm from BBdbR_WcBase a join BBdbR_PlineBase b on a.PlineId=b.PlineId join BBdbR_WorkSectionBase c on b.WorkSectionId=c.WorkSectionId join BBdbR_WorkshopBase d on c.WorkshopId=d.WorkshopId where a.Enabled=1 and b.Enabled=1 and c.Enabled=1 and d.Enabled=1 and d.WorkshopId='" + area_key + "' ");     //===复制时需要修改===
                }
            }
            //模糊搜索质控点编号
            if (WcCd != null && WcCd != "")
            {
                strSql.Append(" and a.WcCd like '%" + WcCd + "%'");
            }
            else { }
            //模糊搜索质控点名称
            if (WcNm != null && WcNm != "")
            {
                strSql.Append(" and a.WcNm like '%" + WcNm + "%'");
            }
            else { }

            //排序
            strSql.Append(" order by a.sort ");
            DataTable dt = DataFactory.Database().FindTableBySql(strSql.ToString(), false);

            #endregion

            try
            {
                DataTable ListData = new DataTable();
                ListData = dt.DefaultView.ToTable("质控点信息表", true, "WcCd", "WcNm", "PlineCd", "PlineNm", "Dsc", "CreTm", "CreCd", "CreNm", "MdfTm", "MdfCd", "MdfNm", "Rem");//获取工位信息表中特定列

                if (ListData.Rows.Count > 0)
                {
                    string fileName = "质控点信息表";
                    string excelType = "xls";
                    MemoryStream ms = DeriveExcel.ExportExcel_Wc2(ListData, excelType);
                    if (!fileName.EndsWith(".xls"))
                    {
                        fileName = fileName + ".xls";
                    }
                    Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other, "1", "质控点信息导出成功");
                    return File(ms, "application/vnd.ms-excel", Url.Encode(fileName));
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other, "-1", "质控点信息导出操作失败：" + ex.Message);
                return null;
            }

        }
        #endregion
    }
}