using HfutIE.Business;
using HfutIE.DataAccess;
using HfutIE.Entity;
using HfutIE.Repository;
using HfutIE.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace HfutIE.WebApp.Areas.BaseModule.Controllers
{
    public class BBdbR_CntlAddrConfController : PublicController<BBdbR_CntlAddrConf>
    {
        #region 创建数据库操作对象区域
        //创建数据库访问对象，用以访问其中操作数据库的方法
        BBdbR_CntlAddrConfBll MyBll = new BBdbR_CntlAddrConfBll(); //===复制时需要修改===
        public readonly RepositoryFactory<BBdbR_CntlAddrConf> repository_cntladdrconf = new RepositoryFactory<BBdbR_CntlAddrConf>();

        #endregion

        #region 方法区  

        #region 1.加载表格数据
        /// <summary>
        /// 加载列表
        /// </summary>
        /// <returns></returns>
        public ActionResult GridPageJson1()
        {
            try
            {
                DataTable ListData = MyBll.GetPlanList();
                var JsonData = new
                {
                    rows = ListData,
                };
                string a = JsonData.ToJson();

                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        #endregion

        #region 2.新增编辑方法
        //entity表示页面中传入的实体，KeyValue表示传入的主键
        //不管是新增还是编辑，页面中都会传入实体（entity）
        //新增时实体是一个全新的实体
        //编辑时实体是修改后的实体
        //只有在编辑时页面中才会传入主键entity（KeyValue），该主键是需要编辑那个实体的主键
        //
        //编辑方法首先根据KeyValue是否有值判断是需要编辑还是需要新增
        //如果是新增就将该实体新增到数据库中
        //如果是编辑就将该实体更新到数据中
        //
        //不管是新增还是编辑首先判断页面输入的编号是否已经存在
        //如果已经存在就直接返回“该编号已经存在！”的信息
        //不存在再进行下一步
        public override ActionResult SubmitForm(BBdbR_CntlAddrConf entity, string KeyValue)//===复制时需要修改===
        {
            try
            {
                int IsOk = 0;//用于判断
                string Name = "AviCd";        //页面中的编号字段名，如：公司编号   //===复制时需要修改===
                string Name2 = "AviNm";
                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";

                if (!string.IsNullOrEmpty(KeyValue))//编辑操作
                {
                    //===复制时需要修改===
                    BBdbR_CntlAddrConf Oldentity = repositoryfactory.Repository().FindEntity(KeyValue);//获取没更新之前实体对象
                    entity.RecId = KeyValue;//编辑保持主键不变
                    if(entity.IsMonitoring == "0")
                    {
                        entity.MonitorRate = 0;
                    }
                    entity.Modify(KeyValue);
                    IsOk = MyBll.Update(entity);//将修改后的实体更新到数据库，插入成功返回1，失败返回0；
                    if (IsOk > 0)
                    {
                        Message = "编辑数采地址信息成功！";
                    }
                    else
                    {
                        Message = "编辑数采地址信息失败！";
                    }
                    this.WriteLog(IsOk, entity, Oldentity, KeyValue, Message);//记录日志
                }
                else//新增操作
                {
                    
                    entity.Create();
                    IsOk = MyBll.Insert(entity);//将实体插入数据库，插入成功返回1，失败返回0；
                    if (IsOk > 0)
                    {
                        Message = "新增数采地址信息成功！";
                    }
                    else
                    {
                        Message = "新增数采地址信息失败！";
                    }
                    this.WriteLog(IsOk, entity, null, KeyValue, Message);//记录日志
                }
                return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                this.WriteLog(-1, entity, null, KeyValue, "操作失败：" + ex.Message);//记录日志
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion

        #region 3.删除方法
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
        public override ActionResult Delete(string KeyValue, string ParentId, string DeleteMark)
        {
            //不管是多个主键还是单个主键，将主键拆分出来，放在数组中
            string[] array = KeyValue.Split(',');
            try
            {
                var Message = "删除失败。";//定义返回信息，该信息将返回到界面上，给用户观看
                int IsOk = 0;//判断删除方法是否成，0表示不成功，大于0表示成功                  
                //IsOk = MyBll.Delete(array);//执行删除操作

                //直接删除
                if (array != null && array.Length > 0)
                {
                    IsOk = MyBll.Delete(array);

                }
                if (IsOk > 0)//表示删除成
                {
                    Message = "删除成功。";//修改返回信息
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

        #region 4.查询方法
        //查询方法，本方法为单条件查询，即根据一个条件进行查询
        //查询条件为Condition，也是数据库表_CompanyBaseInformation中的一个字段名
        //查询值为keywords，也是数据库表_CompanyBaseInformation中的字段名的字段值
        //本查询采用近似查询（like）

        public ActionResult GridPageByCondition(string keywords, string Condition, JqGridParam jqgridparam)
        {
            try
            {
                string keyword = keywords.Trim();
                Stopwatch watch = CommonHelper.TimerStart();
                DataTable ListData = MyBll.GetPageListByCondition(keyword, Condition, jqgridparam);//===复制时需要修改===
                var JsonData = new
                {
                    total = jqgridparam.total,
                    page = jqgridparam.page,
                    records = jqgridparam.records,
                    costtime = CommonHelper.TimerEnd(watch),
                    rows = ListData,
                };
                return Content(ListData.ToJson());
            }
            catch (Exception ex)
            {
                //CCSLog.CCSLogHelper.WriteExLog(ex, CCSLog.LogType.WebSite);
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }
        }
        #endregion

        #region 5.下拉框设备信息
        /// <summary>
        /// 人员下拉框
        /// </summary>
        /// <returns></returns>
        public ActionResult GetDvcNm()
        {
            try
            {
                DataTable dataTable = MyBll.GetDvcNm();
                var JsonData = new
                {
                    rows = dataTable,
                };
                return Content(JsonData.ToJson());
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region 4.导入
        /// <summary>
        /// 导入Excel弹出框页面
        /// </summary>
        /// <returns></returns>
        [ManagerPermission(PermissionMode.Enforce)]
        public ActionResult ExcelImportDialog()
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
        /// <summary>
        /// 导入Excell数据
        /// </summary>
        /// <returns></returns>
        public ActionResult ImportExel()
        {
            int IsOk = 0;//导入状态
            int IsCheck = 1;//用作检验重复地址的标识
            DataTable Result = new DataTable();//导入错误记录表
            IDatabase database = DataFactory.Database();
            List<BBdbR_CntlAddrConf> BBdbR_CntlAddrConfList = new List<BBdbR_CntlAddrConf>();

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
                if (file != null)
                {
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
                        #region 数采地址信息导入
                        //校验
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {

                            IsCheck = 1;//重置标识
                            DataRow dr = Newdt.NewRow();
                            string IsMonitoring = "";
                            switch (dt.Rows[i]["是否监控"].ToString())
                            {
                                case "是":
                                    IsMonitoring = "1"; break;
                                case "否":
                                    IsMonitoring = "0"; break;
                                default:
                                    dr = Newdt.NewRow();
                                    dr[0] = errorNum;
                                    dr[1] = "第[" + dt.Rows[i]["rowid"].ToString() + "]行[是否监控]";
                                    dr[2] = "数字格式不正确请重新输入";
                                    Newdt.Rows.Add(dr);
                                    errorNum++;
                                    IsCheck = 0;
                                    break;
                            }

                            if (dt.Rows[i]["采集分组"].ToString().Trim() != "" && dt.Rows[i]["数采类型"].ToString().Trim() != "" && dt.Rows[i]["地址名称"].ToString().Trim() != "" && dt.Rows[i]["地址值"].ToString().Trim() != "")
                            {
                                BBdbR_CntlAddrConf entity = new BBdbR_CntlAddrConf();
                                entity.RecId = System.Guid.NewGuid().ToString();
                                //entity.DvcId = dt.Rows[i]["设备"].ToString().Trim();
                                entity.CntlType = dt.Rows[i]["数采类型"].ToString().Trim();
                                entity.CntlGroup = dt.Rows[i]["采集分组"].ToString().Trim();
                                entity.SingnalNm = dt.Rows[i]["地址名称"].ToString().Trim();
                                entity.CntlAddr = dt.Rows[i]["地址值"].ToString();
                                entity.CntlAddrDsc = dt.Rows[i]["地址描述"].ToString();
                                entity.CntlDateType = dt.Rows[i]["地址数据类型"].ToString();
                                entity.SglSource = dt.Rows[i]["地址来源"].ToString();
                                entity.IsMonitoring = IsMonitoring;
                                entity.MonitorRate = int.Parse(dt.Rows[i]["监控频率"].ToString().Trim());
                                entity.Rem = dt.Rows[i]["备注"].ToString().Trim();


                                entity.Enabled = "1";
                                entity.CreTm = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                entity.CreCd = ManageProvider.Provider.Current().UserId;
                                entity.CreNm = ManageProvider.Provider.Current().UserName;

                                string dvcCd = dt.Rows[i]["设备编号"].ToString();
                                DataTable dtCd = MyBll.searchID(dvcCd);
                                entity.DvcId = Convert.ToString(dtCd.Rows[0]["id"]);

                                BBdbR_CntlAddrConfList.Add(entity);


                                int b = database.Insert(BBdbR_CntlAddrConfList);
                                if (b > 0)
                                {
                                    IsOk = IsOk + b;
                                    BBdbR_CntlAddrConfList.Clear();
                                }
                                else
                                {
                                    dr = Newdt.NewRow();
                                    dr[0] = errorNum;
                                    dr[1] = "第[" + dt.Rows[i]["rowid"].ToString() + "]行";
                                    dr[2] = "设备信息插入失败";
                                    Newdt.Rows.Add(dr);
                                    IsCheck = 0;
                                    continue;
                                }
                            }
                            else
                            {
                                dr = Newdt.NewRow();
                                dr[0] = errorNum;
                                dr[1] = "第[" + dt.Rows[i]["rowid"].ToString() + "]行";
                                dr[2] = "班次信息不能为空";
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

        #region 5.导出模板
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
        #endregion

        #endregion
    }
}