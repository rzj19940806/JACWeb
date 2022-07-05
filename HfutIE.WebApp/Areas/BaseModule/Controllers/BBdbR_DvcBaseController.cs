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

namespace HfutIE.WebApp.Areas.BaseModule.Controllers
{
    /// <summary>
    /// _FactoryBaseInformation控制器
    /// </summary>
    public class BBdbR_DvcBaseController : PublicController<BBdbR_DvcBase>
    {
        #region 全局变量定义区
        //定义本页面主要操作的表的表名，称为主表
        string tableName = "BBdbR_DvcBase";
        public static DataTable DvcList = new DataTable();
        #endregion

        #region 创建数据库操作对象区域
        //BBdbR_FacBaseBll，用以访问BBdbR_FacBaseBll中操作数据库的方法
        BBdbR_DvcBaseBll MyBll = new BBdbR_DvcBaseBll(); //===复制时需要修改===
        E_EquipmentMaiPlan_ProBll EquipmentMaiPlan_ProBll = new E_EquipmentMaiPlan_ProBll();
        public readonly RepositoryFactory<BBdbR_DvcBase> repository_dvcbase = new RepositoryFactory<BBdbR_DvcBase>();
        #endregion

        #region 打开视图区域
        public ActionResult Select()//打开选择公司子界面页面
        {
            return View();
        }
        #endregion

        #region 方法区   

        #region 1.新增编辑方法
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
        public override ActionResult SubmitForm(BBdbR_DvcBase entity, string KeyValue)//===复制时需要修改===
        {
            try
            {
                int IsOk = 0;//用于判断
                string Name = "DvcCd";        //页面中的编号字段名，如：公司编号   //===复制时需要修改===
                string Value = entity.DvcCd;  //页面中的编号字段值                 //===复制时需要修改===
                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";

                if (!string.IsNullOrEmpty(KeyValue))//编辑操作
                {
                    //===复制时需要修改===
                    BBdbR_DvcBase Oldentity = repositoryfactory.Repository().FindEntity(KeyValue);//获取没更新之前实体对象
                    entity.Modify(KeyValue);
                    IsOk = MyBll.Update(entity);//将修改后的实体更新到数据库，插入成功返回1，失败返回0；
                    this.WriteLog(IsOk, entity, Oldentity, KeyValue, Message);//记录日志
                }
                else//新增操作
                {
                    IsOk = MyBll.CheckCount(Name, Value);//判断页面中填写的数据的编号字段的值是否存在
                    if (IsOk > 0)//存在
                    {
                        Message = "该编号已经存在！";
                        return Content(new JsonMessage { Success = false, Code = IsOk.ToString(), Message = Message }.ToString());
                    }
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

        #region 2.删除方法
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
                if (array != null && array.Length > 0) IsOk = MyBll.Delete(array);
                if (IsOk > 0) Message = "删除信息";
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

        #region 3.查询方法
        //查询方法，本方法为单条件查询，即根据一个条件进行查询
        //查询条件为Condition，也是数据库表_CompanyBaseInformation中的一个字段名
        //查询值为keywords，也是数据库表_CompanyBaseInformation中的字段名的字段值
        //本查询采用近似查询（like）

        public ActionResult GridPageByCondition(string DvcCd, string DvcNm, string DvcCatg, string DvcTyp, string DvcLocatn, JqGridParam jqgridparam)
        {
            try
            {
                #region 查询原方法
                //string keyword = keywords.Trim();
                //Stopwatch watch = CommonHelper.TimerStart();
                //DvcList = MyBll.GetPageListByCondition(keyword,Condition, jqgridparam);//===复制时需要修改===
                //var JsonData = new
                //{
                //    total = jqgridparam.total,
                //    page = jqgridparam.page,
                //    records = jqgridparam.records,
                //    costtime = CommonHelper.TimerEnd(watch),
                //    rows = DvcList,
                //};
                //return Content(DvcList.ToJson());
                #endregion

                #region 查询修改
                Stopwatch watch = CommonHelper.TimerStart();
                StringBuilder strSql = new StringBuilder();
                List<DbParameter> parameter = new List<DbParameter>();
                strSql.Append(@"select * from BBdbR_DvcBase where Enabled=1 ");
                #region 搜索条件判定
                //设备编号模糊搜索
                if (DvcCd != "" && DvcCd != null)
                {
                    //strSql.Append(" and DvcCd like '%" + DvcCd + "%' ");
                    strSql.Append(" and DvcCd like @DvcCd ");
                    parameter.Add(DbFactory.CreateDbParameter("@DvcCd", "%" + DvcCd + "%"));
                }
                else { }
                //设备名称模糊搜索
                if (DvcNm != "" && DvcNm != null)
                {
                    //strSql.Append(" and DvcNm like '%" + DvcNm + "%' ");
                    strSql.Append(" and DvcNm like @DvcNm ");
                    parameter.Add(DbFactory.CreateDbParameter("@DvcNm", "%" + DvcNm + "%"));
                }
                else { }
                //设备类别模糊搜索
                if (DvcCatg != "" && DvcCatg != null)
                {
                    //strSql.Append(" and DvcCatg like '%" + DvcCatg + "%' ");
                    strSql.Append(" and DvcCatg like @DvcCatg ");
                    parameter.Add(DbFactory.CreateDbParameter("@DvcCatg", "%" + DvcCatg + "%"));
                }
                else { }
                //设备类型模糊搜索
                if (DvcTyp != "" && DvcTyp != null)
                {
                    //strSql.Append(" and DvcTyp like '%" + DvcTyp + "%' ");
                    strSql.Append(" and DvcTyp like @DvcTyp ");
                    parameter.Add(DbFactory.CreateDbParameter("@DvcTyp", "%" + DvcTyp + "%"));
                }
                else { }
                //设备位置模糊搜索
                if (DvcLocatn != "" && DvcLocatn != null)
                {
                    //strSql.Append(" and DvcLocatn like '%" + DvcLocatn + "%' ");
                    strSql.Append(" and DvcLocatn like @DvcLocatn ");
                    parameter.Add(DbFactory.CreateDbParameter("@DvcLocatn", "%" + DvcLocatn + "%"));
                }
                else { }
                #endregion
                //排序
                strSql.Append(" order by DvcCd asc");
                DvcList = DataFactory.Database().FindTableBySql(strSql.ToString(), parameter.ToArray(), false);


                DvcList.Columns.Add("ClassNm");
                for (int i = 0; i < DvcList.Rows.Count; i++)
                {
                    if (DvcList.Rows[i]["Class"].ToString() == "车间")
                    {
                        StringBuilder sql = new StringBuilder();
                        sql.Append("select WorkshopNm from BBdbR_WorkshopBase where WorkshopId='" + DvcList.Rows[i]["ClassId"].ToString() + "'and Enabled=1 ");
                        DataTable dt1 = DataFactory.Database().FindTableBySql(sql.ToString(), false);
                        if (dt1.Rows.Count > 0)
                        {
                            DvcList.Rows[i]["ClassNm"] = dt1.Rows[0][0];
                        }
                    }
                    else if (DvcList.Rows[i]["Class"].ToString() == "工位")
                    {
                        StringBuilder sql = new StringBuilder();
                        sql.Append("select WcNm from BBdbR_WcBase where WcId='" + DvcList.Rows[i]["ClassId"].ToString() + "' and Enabled=1 ");
                        DataTable dt2 = DataFactory.Database().FindTableBySql(sql.ToString(), false);
                        if (dt2.Rows.Count > 0)
                        {
                            DvcList.Rows[i]["ClassNm"] = dt2.Rows[0][0];
                        }
                    }
                    else if (DvcList.Rows[i]["Class"].ToString() == "产线")
                    {
                        StringBuilder sql = new StringBuilder();
                        sql.Append("select PlineNm from BBdbR_PlineBase where PlineId='" + DvcList.Rows[i]["ClassId"].ToString() + "' and Enabled=1 ");
                        DataTable dt3 = DataFactory.Database().FindTableBySql(sql.ToString(), false);
                        if (dt3.Rows.Count > 0)
                        {
                            DvcList.Rows[i]["ClassNm"] = dt3.Rows[0][0];
                        }
                    }
                    else if (DvcList.Rows[i]["Class"].ToString() == "岗位")
                    {
                        StringBuilder sql = new StringBuilder();
                        sql.Append("select PostNm from BBdbR_PostBase where PostId='" + DvcList.Rows[i]["ClassId"].ToString() + "' and Enabled=1 ");
                        DataTable dt4 = DataFactory.Database().FindTableBySql(sql.ToString(), false);
                        if (dt4.Rows.Count > 0)
                        {
                            DvcList.Rows[i]["ClassNm"] = dt4.Rows[0][0];
                        }
                    }
                    else if (DvcList.Rows[i]["Class"].ToString() == "AVI设备")
                    {
                        StringBuilder sql = new StringBuilder();
                        sql.Append("select AviNm from BBdbR_AVIBase where AviId ='" + DvcList.Rows[i]["ClassId"].ToString() + "' and Enabled=1 ");
                        DataTable dt5 = DataFactory.Database().FindTableBySql(sql.ToString(), false);
                        if (dt5.Rows.Count > 0)
                        {
                            DvcList.Rows[i]["ClassNm"] = dt5.Rows[0][0];
                        }
                    }
                }
                var JsonData = new
                {
                    jqgridparam.total,
                    jqgridparam.page,
                    jqgridparam.records,
                    costtime = CommonHelper.TimerEnd(watch),
                    rows = DvcList,
                };
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "1", "设备管理信息查询成功");
                return Content(JsonData.ToJson());

                #endregion
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "设备管理信息查询异常错误：" + ex.Message);
                return null;
            }
        }
        #endregion

        #region 4.检查字段是否唯一
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

        #region 5.绑定页面表格数据源
        /// <summary>
        /// 加载列表
        /// </summary>
        /// <returns></returns>
        public ActionResult GridPageJson1()
        {
            try
            {
                List<BBdbR_DvcBase> ListData = MyBll.GetPlanList1();
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
        //public ActionResult GridPageJson1()
        //{
        //    try
        //    {
        //        List<BBdbR_DvcBase> ListData = MyBll.GetPlanList();
        //        var JsonData = new
        //        {
        //            rows = ListData,
        //        };
        //        string a = JsonData.ToJson();
        //        return Content(JsonData.ToJson());
        //    }
        //    catch (Exception ex)
        //    {
        //        return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
        //    }
        //}
        #endregion

        #region 6.编辑填充界面数据
        public  ActionResult SetFormcity(string KeyValue)
        {
            try
            {
                BBdbR_DvcBase Deviceentity = MyBll.GetPlanList1(KeyValue);
                return Content(Deviceentity.ToJson());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion

        #region 7.导入
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
            //ViewBag.ID = Request.Params["ID"];
            //ID1 = ViewBag.ID;
            //ViewBag.OrderID = Request.Params["OrderID"];
            //OrderID1 = ViewBag.OrderID;
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
        public ActionResult ImportExel(string AlarmId)
        {
            int IsOk = 0;//导入状态
            int IsCheck = 1;//用作检验重复地址的标识
            DataTable Result = new DataTable();//导入错误记录表
            IDatabase database = DataFactory.Database();
            List<BBdbR_DvcBase> DeviceList = new List<BBdbR_DvcBase>();

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

                    RemoveEmpty(dt);//清除空行
                    dt.Columns.Add("rowid", typeof(System.String));//用来标识excell行ID
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dt.Rows[i]["rowid"] = i + 1;
                    }
                    #region 设备信息导入
                    //校验
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        IsCheck = 1;//重置标识
                        DataRow dr = Newdt.NewRow();
                        if (dt.Rows[i]["机构级别"].ToString().Trim() != "车间" && dt.Rows[i]["机构级别"].ToString().Trim() != "工位"&& dt.Rows[i]["机构级别"].ToString().Trim() != "产线"&& dt.Rows[i]["机构级别"].ToString().Trim() != "岗位")
                        {
                            dr = Newdt.NewRow();
                            dr[0] = errorNum;
                            dr[1] = "第[" + dt.Rows[i]["rowid"].ToString() + "]行[岗位类型]";
                            dr[2] = "岗位类型错误";
                            Newdt.Rows.Add(dr);
                            errorNum++;
                            IsCheck = 0;
                            continue;
                        }
                        if (dt.Rows[i]["设备编号"].ToString().Trim() != "")
                        {
                            int DeviceCount = MyBll.CheckCount("DvcCd", dt.Rows[i]["设备编号"].ToString());//是否有相同设备编码
                            if (DeviceCount > 0)
                            {
                                dr = Newdt.NewRow();
                                dr[0] = errorNum;
                                dr[1] = "第[" + dt.Rows[i]["rowid"].ToString() + "]行[设备编号]";
                                dr[2] = "在系统中已存在不能重复插入";
                                Newdt.Rows.Add(dr);
                                errorNum++;
                                IsCheck = 0;
                                continue;
                            }
                            else
                            {
                                BBdbR_DvcBase Device = new BBdbR_DvcBase();
                                Device.DvcId = System.Guid.NewGuid().ToString();
                                Device.Class = dt.Rows[i]["机构级别"].ToString().Trim();
                                string classCD = dt.Rows[i]["机构编号"].ToString().Trim(); //界面配置的是ClassId====>机构编号
                                Device.DvcCd = dt.Rows[i]["设备编号"].ToString().Trim();
                                Device.DvcNm = dt.Rows[i]["设备名称"].ToString().Trim();
                                Device.DvcCatg = dt.Rows[i]["设备类别"].ToString().Trim();//设备类别
                                Device.DvcTyp = dt.Rows[i]["设备类型"].ToString().Trim();  //设备类型
                                Device.IPAddr = dt.Rows[i]["IP地址"].ToString().Trim();
                                Device.Port = dt.Rows[i]["端口"].ToString().Trim();
                                Device.DvcMaker = dt.Rows[i]["设备型号"].ToString().Trim();
                                Device.DvcMdl = dt.Rows[i]["设备产商"].ToString().Trim();
                                Device.DvcLife = dt.Rows[i]["设备寿命"].ToString().Trim();
                                Device.DvcLocatn = dt.Rows[i]["设备位置"].ToString().Trim();
                                Device.MaintCycle = dt.Rows[i]["维保周期(天)"].ToString().Trim();
                                Device.LeadTm = dt.Rows[i]["提前期（天）"].ToString().Trim();
                                Device.Dsc = dt.Rows[i]["设备描述"].ToString().Trim();
                                Device.Rem = dt.Rows[i]["备注"].ToString().Trim();
                                Device.DvcMDt = dt.Rows[i]["设备制造日期"].ToString().Trim();
                                Device.Enabled = "1";
                                Device.VersionNumber = "V1.0.0";
                                Device.CreTm = DateTime.Now.ToString();  //修改时间
                                Device.CreNm = ManageProvider.Provider.Current().UserId; //修改人编号
                                Device.CreCd = ManageProvider.Provider.Current().UserName;//修改人名称

                                DataTable dataTable = MyBll.searchClass(Device.Class, classCD);//机构级别、机构编号
                                //string idTest = Convert.ToString(dataTable.Rows[0]["id"]);  ///机构ID

                                if(dataTable == null)
                                {
                                    dr = Newdt.NewRow();
                                    dr[0] = errorNum;
                                    dr[1] = "第[" + dt.Rows[i]["rowid"].ToString() + "]行[机构编号]";
                                    dr[2] = "无效的机构编号";
                                    Newdt.Rows.Add(dr);
                                    errorNum++;
                                    IsCheck = 0;
                                    continue;
                                }
                                else
                                {
                                    Device.ClassId = dataTable.Rows[0]["id"].ToString();  ///机构ID
                                }


                                DeviceList.Add(Device);
                                int b = database.Insert(DeviceList);
                                if (b > 0)
                                {
                                    IsOk = IsOk + b;
                                    DeviceList.Clear();
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
                        }
                        else
                        {
                            dr = Newdt.NewRow();
                            dr[0] = errorNum;
                            dr[1] = "第[" + dt.Rows[i]["rowid"].ToString() + "]行";
                            dr[2] = "设备编号不能为空";
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

        #region 8.机构名称下拉框
        public ActionResult GetClassNm(string classv)
        {
            try
            {
                DataTable dataTable = MyBll.GetWorkshopNm(classv);
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

        #region 10.重构导出方法
        /// <summary>
        /// 1.如果是按照条件查询后再进行
        /// </summary>
        /// <param name="type"></param>
        /// <param name="jqgridparam"></param>
        /// <returns></returns>
        public ActionResult GetExcel_Data(string DvcCd, string DvcNm, string DvcCatg, string DvcTyp, string DvcLocatn, JqGridParam jqgridparam)
        {
            try
            {
                #region 获取数据
                DataTable ListData = new DataTable();
                if (DvcList.Rows.Count > 0)
                {
                    ListData = DvcList.DefaultView.ToTable("设备信息", true, "DvcCd", "DvcNm", "Class", "ClassNm", "DvcLocatn", "IPAddr", "DvcCatg", "DvcTyp", "CreTm", "CreNm", "MdfTm", "MdfNm", "Rem");//获取表中特定列
                }
                #endregion


                string fileName = "设备信息表";
                string excelType = "xls";
                MemoryStream ms = DeriveExcel.ExportExcel_Dvc(ListData, excelType);
                if (!fileName.EndsWith(".xls"))
                {
                    fileName = fileName + ".xls";
                }
                Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other, "1", "设备信息导出成功");
                return File(ms, "application/vnd.ms-excel", Url.Encode(fileName));
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other, "-1", "设备信息导出操作失败：" + ex.Message);
                throw;
            }
           
        }
        #endregion

        #endregion

    }
}