using HfutIE.Business;
using HfutIE.DataAccess;
using HfutIE.Entity;
using HfutIE.Repository;
using HfutIE.Utilities;
using System;
using System.Collections;
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
    /// <summary>
    /// 班次基本信息表控制器
    /// </summary>
    public class BFacR_ShiftBaseController : PublicController<BFacR_ShiftBase>
    {
        #region 全局变量定义区

        //定义本页面主要操作的表的表名，称为主表
        string tableName = "BFacR_ShiftBase";//===复制时需要修改===
        public readonly RepositoryFactory<BFacR_ShiftBase> repository_shiftbase = new RepositoryFactory<BFacR_ShiftBase>();
        public static DataTable ShiftList = new DataTable();
        #endregion

        #region 创建数据库操作对象区域
        //BBdbR_PushRuleBll，用以访问BBdbR_PushRuleBll中操作数据库的方法
        BFacR_ShiftBaseBll MyBll = new BFacR_ShiftBaseBll(); //===复制时需要修改===
        #endregion

        #region 打开视图区域
        public ActionResult TeamConfigForm()//打开班组配置子界面页面
        {
            return View();
        }
        public ActionResult ShiftTimeConfig()//打开班组配置子界面页面
        {
            return View();
        }
        #endregion

        #region 方法区

        #region 1.搜索
        //查询方法，本方法为单条件查询，即根据一个条件进行查询
        //查询条件为Condition，也是数据库表_CompanyBaseInformation中的一个字段名
        //查询值为keywords，也是数据库表_CompanyBaseInformation中的字段名的字段值
        //本查询采用近似查询（like）

        public ActionResult GridPageByCondition1(string keywords, string Condition, JqGridParam jqgridparam)
        {
            try
            {
                if(keywords == null)
                {
                    keywords = "";
                    Condition = "all";
                }
                string keyword = keywords.Trim();
                Stopwatch watch = CommonHelper.TimerStart();
                ShiftList = MyBll.GetPageListByCondition(keyword, Condition, jqgridparam);//===复制时需要修改===
                var JsonData = new
                {
                    total = jqgridparam.total,
                    page = jqgridparam.page,
                    records = jqgridparam.records,
                    costtime = CommonHelper.TimerEnd(watch),
                    rows = ShiftList,
                };
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "1", "班次管理信息查询成功");
                return Content(ShiftList.ToJson());
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "班次管理信息查询发生异常错误：" + ex.Message);
                return null;
            }
        }

        //查询方法，本方法为单条件查询，即根据一个条件进行查询
        //查询条件为Condition，也是数据库表_CompanyBaseInformation中的一个字段名
        //查询值为keywords，也是数据库表_CompanyBaseInformation中的字段名的字段值
        //本查询采用近似查询（like）

        public ActionResult GridListJson_TimeConfig(string KeyValue, string Condition, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                ShiftList = MyBll.GetListJson_TimeConfig(KeyValue, jqgridparam);//===复制时需要修改===
                var JsonData = new
                {
                    total = jqgridparam.total,
                    page = jqgridparam.page,
                    records = jqgridparam.records,
                    costtime = CommonHelper.TimerEnd(watch),
                    rows = ShiftList,
                };
                return Content(ShiftList.ToJson());
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }
        }
        #endregion

        #region 2.新增、编辑
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
        public ActionResult SubmitShiftBase(BFacR_ShiftBase entity, string KeyValue)//===复制时需要修改===
        {
            try
            {
                int IsOk = 0;//用于判断
                string Name = "ShiftCd";        //页面中的编号字段名，如：公司编号   //===复制时需要修改===
                string Value = entity.ShiftCd;  //页面中的编号字段值                 //===复制时需要修改===
                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";

                if (!string.IsNullOrEmpty(KeyValue))//编辑操作
                {
                    //===复制时需要修改===
                    BFacR_ShiftBase Oldentity = repository_shiftbase.Repository().FindEntity(KeyValue);//获取没更新之前实体对象
                    entity.ShiftId = KeyValue;//编辑保持主键不变
                    entity.Modify(KeyValue);                  
                    IsOk = MyBll.Update(entity);//将修改后的实体更新到数据库，插入成功返回1，失败返回0；
                    this.WriteLog(IsOk, entity, Oldentity, KeyValue, Message);//记录日志
                }
                else//新增操作
                {
                    //一个班次出现多次，不可判断重复
                    //IsOk = MyBll.CheckCount(tableName, Name, Value);//判断页面中填写的数据的编号字段的值是否存在
                    //if (IsOk > 0)//存在
                    //{
                    //    Message = "该班次编号已经存在！";
                    //    return Content(new JsonMessage { Success = false, Code = "-1", Message = Message }.ToString());
                    //}

                    entity.Create();                   
                    IsOk = MyBll.Insert(entity);//将实体插入数据库，插入成功返回1，失败返回0；
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

        #region 3.删除
        /// <summary>
        /// 首先判断需要删除的信息是否绑定了其他信息
        ///     如：删除一条工厂信息先要判断该条公司信息下面是否绑定了车间信息
        ///         如果绑定了信息，则提示“当前所选有子节点数据，不能删除。”并结束
        ///  在确定没绑定数据的情况下，删除该数据
        ///     删除表示将该数据的Enable属性设为false，并不真的删除该记录
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
                for (int i = 0; i < array.Length; i++)
                {
                    //班次下面设有班组时不可删除
                    //班次属于某个班制时不可删除
                    ClassConfigViewBll ClassBll = new ClassConfigViewBll();
                    ClassConfigView Oldentity = ClassBll.GetByCondition(array[i]);
                    BFacR_ClassTeamConfigBll ClassTeamBll = new BFacR_ClassTeamConfigBll();
                    List<BFacR_ClassTeamConfig> Classentity = ClassTeamBll.GetPageListByCondition(array[i]);
                    if (Oldentity.ShiftId!="" && Classentity.Count>0)
                    {
                        throw new Exception("当前所选有子节点数据，不能删除。");
                    }
                    else
                    {
                        IsOk = MyBll.Delete(array[i]);//软删除-Enabled=0
                        Message = "删除成功。";//修改返回信息
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
            List<BFacR_ShiftBase> BFacRShiftBaseList = new List<BFacR_ShiftBase>();

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
                        #region 班次基本信息导入
                        //校验
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {

                            IsCheck = 1;//重置标识
                            DataRow dr = Newdt.NewRow();

                            string RestTm = "";

                            switch (dt.Rows[i]["时间类型"].ToString())
                            {
                                case "工作":
                                    RestTm = "1"; break;
                                case "休息":
                                    RestTm = "0"; break;
                                default:
                                    dr = Newdt.NewRow();
                                    dr[0] = errorNum;
                                    dr[1] = "第[" + dt.Rows[i]["rowid"].ToString() + "]行[时间类型]";
                                    dr[2] = "数字格式不正确请重新输入";
                                    Newdt.Rows.Add(dr);
                                    errorNum++;
                                    IsCheck = 0;
                                    break;
                            }


                            if (dt.Rows[i]["班次编号"].ToString().Trim() != "" && dt.Rows[i]["班次名称"].ToString().Trim() != "" && dt.Rows[i]["开始时间"].ToString().Trim() != "" && dt.Rows[i]["结束时间"].ToString().Trim() != "")
                            {
                                BFacR_ShiftBase ShiftBaseEntity = new BFacR_ShiftBase();

                                ShiftBaseEntity.ShiftId = System.Guid.NewGuid().ToString();//班次ID
                                ShiftBaseEntity.ShiftCd = dt.Rows[i]["班次编号"].ToString().Trim();
                                ShiftBaseEntity.ShiftNm = dt.Rows[i]["班次名称"].ToString().Trim();
                                ShiftBaseEntity.RestTm = RestTm; //时间类型
                                ShiftBaseEntity.StrtRestTm = dt.Rows[i]["开始时间"].ToString().Substring(dt.Rows[i]["开始时间"].ToString().Length - 8);
                                ShiftBaseEntity.EndRestTm = dt.Rows[i]["结束时间"].ToString().Substring(dt.Rows[i]["结束时间"].ToString().Length - 8);

                                ShiftBaseEntity.VersionNumber = "V1.0";
                                ShiftBaseEntity.Enabled = "1";
                                ShiftBaseEntity.CreTm = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                ShiftBaseEntity.CreCd = ManageProvider.Provider.Current().UserId;
                                ShiftBaseEntity.CreNm = ManageProvider.Provider.Current().UserName;

                                BFacRShiftBaseList.Add(ShiftBaseEntity);

                                int b = database.Insert(BFacRShiftBaseList);
                                if (b > 0)
                                {
                                    IsOk = IsOk + b;
                                    BFacRShiftBaseList.Clear();
                                }
                                else
                                {
                                    dr = Newdt.NewRow();
                                    dr[0] = errorNum;
                                    dr[1] = "第[" + dt.Rows[i]["rowid"].ToString() + "]行";
                                    dr[2] = "班次信息插入失败";
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
                                dr[2] = "信息不能为空";
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

        #region 6.班次班组配置
        /// <summary>
        /// 加载配置弹框中上半个表格
        /// 加载的数据是已有班组信息但无配置
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        public ActionResult GridListJson_Up(string KeyValue, JqGridParam jqgridparam)
        {
            try
            {
                BFacR_TeamBaseBll TeamBaseBll = new BFacR_TeamBaseBll();
                if (KeyValue != "")
                {
                    string keyvalue = KeyValue.Trim();//删除字符串头和尾的空白字符
                    Stopwatch watch = CommonHelper.TimerStart();//测量时间
                    List<BFacR_TeamBase> ListData = TeamBaseBll.GetReConfigList(keyvalue, jqgridparam);//===复制时需要修改===
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
                else
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "请确保选中班制信息" }.ToString());
                }
            }
            catch (Exception ex)
            {
                WriteLog(-1, KeyValue, "操作失败：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        /// <summary>
        /// 加载配置弹框中下半个表格
        /// 加载的数据是已有班组信息也有配置
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        public ActionResult GridListJson_Down(string KeyValue, JqGridParam jqgridparam)
        {
            try
            {
                BFacR_TeamBaseBll ShiftBaseBll = new BFacR_TeamBaseBll();
                if (KeyValue != "")
                {
                    string keyvalue = KeyValue.Trim();//删除字符串头和尾的空白字符
                    Stopwatch watch = CommonHelper.TimerStart();//测量时间
                    List<BFacR_TeamBase> ListData = ShiftBaseBll.GetConfigList(keyvalue, jqgridparam);//===复制时需要修改===
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
                else
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "请确保选中班制信息" }.ToString());
                }
            }
            catch (Exception ex)
            {
                WriteLog(-1, KeyValue, "操作失败：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        /// <summary>
        /// 提交班制班次配置信息
        /// </summary>
        /// <returns></returns>
        public ActionResult SubmitStaffConfig(List<BFacR_TeamBase> listdto, string ShiftId)
        {
            try
            {
                BFacR_ClassTeamConfigBll ClassBll = new BFacR_ClassTeamConfigBll();
                var Message = "配置失败。";//定义返回信息，该信息将返回到界面上，给用户观看
                int IsOk = 0;//用于判断          
                if (ShiftId != "")//先把对应ClassId中所有班组均删除，然后插入已配置班组
                {
                    List<BFacR_ClassTeamConfig> ListData1 = ClassBll.GetClassList(ShiftId, "");
                    if (ListData1.Count > 0)
                    {
                        ClassBll.Delete(ListData1);//执行删除操作
                    }
                }
                if (listdto != null)
                {
                    for (int i = 0; i < listdto.Count; i++)
                    {
                        string a = listdto[i].TeamId.ToString();
                        List<BFacR_ClassTeamConfig> ListData = ClassBll.GetClassList(ShiftId, listdto[i].TeamId.ToString());
                        if (ListData.Count == 0)
                        {
                            IsOk = ClassBll.InsertClassConfig(ShiftId, listdto[i]);//将实体插入数据库，插入成功返回1，失败返回0；
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
                IsOk = 1;
                Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other, IsOk.ToString(), "班次班组配置成功");
                return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = "配置成功!" }.ToString());
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other, "-1", "班次班组配置操作失败：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion

        #region 7.重构导出方法
        /// <summary>
        /// 1.如果是按照条件查询后再进行
        /// </summary>
        /// <param name="type"></param>
        /// <param name="jqgridparam"></param>
        /// <returns></returns>
        public ActionResult GetExcel_Data(string type, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                DataTable ListData = new DataTable();
                ListData = ShiftList.DefaultView.ToTable("班次信息表", true, "ShiftCd", "ShiftNm", "CreTm", "CreCd", "CreNm");//获取AVI中特定列
                if (ListData.Rows.Count > 0)
                {
                    string fileName = "班次信息";
                    string excelType = "xls";
                    MemoryStream ms = DeriveExcel.ExportExcel_Shift(ListData, excelType);
                    if (!fileName.EndsWith(".xls"))
                    {
                        fileName = fileName + ".xls";
                    }
                    Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other, "1", "班次信息导出成功");
                    return File(ms, "application/vnd.ms-excel", Url.Encode(fileName));
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other, "-1", "班次信息导出操作失败：" + ex.Message);
                return null;
            }
            
        }
        #endregion

        #region 2.新增、编辑
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
        ///// <summary>
        ///// 表单赋值
        ///// </summary>
        ///// <param name="KeyValue">主键值</param>
        ///// <returns></returns>
        //[HttpPost]
        //[ValidateInput(false)]
        //[LoginAuthorize]
        //public ActionResult SetForm1(string KeyValue)
        //{
        //    BFacR_ShiftTimeConfig entity = repository_shiftbase1.Repository().FindEntity(KeyValue);
        //    return Content(entity.ToJson());
        //}
        public ActionResult SubmitForm_TmConfig(BFacR_ShiftBase entity, string KeyValue, string FatherKey)//===复制时需要修改===
        {
            try
            {
                int IsOk = 0;//用于判断
                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";
                if (!string.IsNullOrEmpty(KeyValue))//编辑操作
                {
                    //===复制时需要修改===
                    BFacR_ShiftBase Oldentity = repository_shiftbase.Repository().FindEntity(KeyValue);//获取没更新之前实体对象
                    entity.ShiftId = KeyValue;
                    //entity.ShiftTimeConfigId = KeyValue;//编辑保持主键不变
                    entity.Modify(KeyValue);
                    IsOk = MyBll.Update(entity);//将修改后的实体更新到数据库，插入成功返回1，失败返回0；
                    this.WriteLog(IsOk, entity, Oldentity, KeyValue, Message);//记录日志
                }
                else//新增操作
                {
                    //entity.ShiftId = FatherKey;
                    BFacR_ShiftBase Oldentity = repository_shiftbase.Repository().FindEntity(FatherKey);//获取没更新之前实体对象
                    
                    entity.ShiftCd = Oldentity.ShiftCd;
                    entity.ShiftNm = Oldentity.ShiftNm;
                    entity.Create();
                    IsOk = MyBll.Insert(entity);//将实体插入数据库，插入成功返回1，失败返回0；
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
        
        #endregion
    }
}