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
    /// Tg_WcJobConfig控制器
    /// </summary>
    public class WcJobController : PublicController<Tg_WcJobConfig>
    {
        public readonly RepositoryFactory<Tg_JobTorqueConfig> repository_Tg_JobTorqueConfig = new RepositoryFactory<Tg_JobTorqueConfig>();
        public ActionResult Form_Torque()
        {
            return View();
        }
        public ActionResult GridPageByCondition(string WcCd, string WcNm, string WcJobCd, string JobCd, string ControllerID, JqGridParam jqgridparam)
        {
            try
            {
                #region 查询修改
                Stopwatch watch = CommonHelper.TimerStart();
                StringBuilder strSql = new StringBuilder();
                strSql.Append(@"SELECT ID,WcJobCd,
                                   WcId,
                                   WcCd,
                                   WcNm,
                                   Position,
                                   JobCd,
                                   ControllerID,
                                   ControllerPort,
                                   TorqueUL,TorqueLL,TorqueSL,AngleUL,AngleLL,AngleSL,Num,
                                   CreTm,
                                   CreCd,
                                   CreNm,
                                   MdfTm,
                                   MdfCd,
                                   MdfNm,
                                   Rem,
                                   RsvFld1,
                                   RsvFld2 FROM dbo.Tg_WcJobConfig WHERE ID>0 ");
                List<DbParameter> parameter = new List<DbParameter>();
                if (!String.IsNullOrEmpty(WcCd))
                {
                    strSql.Append(" and WcCd like @WcCd ");
                    parameter.Add(DbFactory.CreateDbParameter("@WcCd", "%" + WcCd + "%"));
                }
                if (!String.IsNullOrEmpty(WcJobCd))
                {
                    strSql.Append(" and WcCd like @WcJobCd ");
                    parameter.Add(DbFactory.CreateDbParameter("@WcJobCd", "%" + WcJobCd + "%"));
                }
                if (!String.IsNullOrEmpty(WcNm))
                {
                    strSql.Append(" and WcNm like @WcNm ");
                    parameter.Add(DbFactory.CreateDbParameter("@WcNm", "%" + WcNm + "%"));
                }
                if (!String.IsNullOrEmpty(JobCd))
                {
                    strSql.Append(" and JobCd like @JobCd ");
                    parameter.Add(DbFactory.CreateDbParameter("@JobCd", "%" + JobCd + "%"));
                }
                if (!String.IsNullOrEmpty(ControllerID))
                {
                    strSql.Append(" and ControllerID like @ControllerID ");
                    parameter.Add(DbFactory.CreateDbParameter("@ControllerID", "%" + ControllerID + "%"));
                }
                #endregion
                //按照检验岗编号排序
                strSql.Append(" order by WcCd desc");
                DataTable dt = DataFactory.Database().FindTableBySql(strSql.ToString(), parameter.ToArray(), false);
                var JsonData = new
                {
                    total = jqgridparam.total,
                    page = jqgridparam.page,
                    records = jqgridparam.records,
                    costtime = CommonHelper.TimerEnd(watch),
                    rows = dt,
                };
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "1", "工位Job信息查询成功");
                return Content(JsonData.ToJson());

            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "工位Job信息查询发生异常错误：" + ex.Message);
                return null;
            }
        }
        public ActionResult GridPageByCondition1(string KeyValue, JqGridParam jqgridparam)
        {
            try
            {
                #region 查询修改
                Stopwatch watch = CommonHelper.TimerStart();
                StringBuilder strSql = new StringBuilder();
                strSql.Append(@"SELECT * FROM dbo.Tg_JobTorqueConfig WHERE ID>0 and WcJobCd='" + KeyValue + "'");

                #endregion
                //按照检验岗编号排序
                strSql.Append(" order by Ord ");
                DataTable dt = DataFactory.Database().FindTableBySql(strSql.ToString(), false);
                var JsonData = new
                {
                    total = jqgridparam.total,
                    page = jqgridparam.page,
                    records = jqgridparam.records,
                    costtime = CommonHelper.TimerEnd(watch),
                    rows = dt,
                };
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "1", "工位Job扭矩转角信息查询成功");
                return Content(JsonData.ToJson());

            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "工位Job扭矩转角信息查询发生异常错误：" + ex.Message);
                return null;
            }
        }

        public override ActionResult SubmitForm(Tg_WcJobConfig entity, string KeyValue)//===复制时需要修改===
        {
            try
            {
                IDatabase database = DataFactory.Database();
                int IsOk = 0;//用于判断
                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";//keyValue=空吗？是，返回新增；否，返回编辑。
                if (!string.IsNullOrEmpty(KeyValue))//编辑操作
                {
                    //===复制时需要修改===
                    Tg_WcJobConfig Oldentity = repositoryfactory.Repository().FindEntity(KeyValue);//获取没更新之前实体对象
                    entity.ID = KeyValue;
                    entity.MdfTm = DateTime.Now;
                    entity.MdfCd = ManageProvider.Provider.Current().UserId;
                    entity.MdfNm = ManageProvider.Provider.Current().UserName;

                    IsOk = database.Update(entity);//将修改后的实体更新到数据库，插入成功返回1，失败返回0；
                    //this.WriteLog(IsOk, entity, Oldentity, KeyValue, Message);//记录日志                 
                }
                else//新增操作
                {
                    string WcNm = entity.WcNm;
                    string Position = entity.Position;
                    string JobCd = entity.JobCd;
                    string WcJobCd = entity.WcJobCd;
                    string ControllerID = entity.ControllerID;
                    string ControllerPort = entity.ControllerPort;
                    StringBuilder sb1 = new StringBuilder();
                    sb1.Append(@"SELECT WcId,WcCd FROM dbo.BBdbR_WcBase WHERE WcNm='" + WcNm + "'");
                    DataTable dt = database.FindTableBySql(sb1.ToString());
                    if (dt.Rows.Count == 0)//存在
                    {
                        Message = "工位不存在！";
                        return Content(new JsonMessage { Success = false, Code = IsOk.ToString(), Message = Message }.ToString());
                    }
                    StringBuilder sb2 = new StringBuilder();
                    sb2.Append(@"SELECT WcJobCd FROM dbo.Tg_WcJobConfig WHERE WcJobCd='" + WcJobCd + "'");
                    DataTable dt2 = database.FindTableBySql(sb2.ToString());
                    if (dt2.Rows.Count > 0)//存在同样的工位JOB号
                    {
                        Message = "工位JOB编号已存在！";
                        return Content(new JsonMessage { Success = false, Code = IsOk.ToString(), Message = Message }.ToString());
                    }
                    StringBuilder sb = new StringBuilder();
                    sb.Append(@"SELECT ID FROM dbo.Tg_WcJobConfig WHERE WcNm='" + WcNm + "' AND Position='" + Position + "' " +
                        "AND JobCd='" + JobCd + "' AND ControllerID='" + ControllerID + "' AND ControllerPort='" + ControllerPort + "'");
                    DataTable dt1 = database.FindTableBySql(sb.ToString());//判断页面中填写的数据的编号字段的值是否存在
                    if (dt1.Rows.Count > 0)//存在
                    {
                        Message = "该记录已经存在！";
                        return Content(new JsonMessage { Success = false, Code = "0", Message = Message }.ToString());
                    }
                    entity.CreTm = DateTime.Now;
                    entity.CreCd = ManageProvider.Provider.Current().UserId;
                    entity.CreNm = ManageProvider.Provider.Current().UserName;
                    entity.WcId = dt.Rows[0]["WcId"].ToString();
                    entity.WcCd = dt.Rows[0]["WcCd"].ToString();
                    IsOk = database.Insert(entity);//将实体插入数据库，插入成功返回1，失败返回0；
                    //this.WriteLog(IsOk, entity, null, KeyValue, Message);//记录日志
                }
                return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                this.WriteLog(-1, entity, null, KeyValue, "操作失败：" + ex.Message);//记录日志
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        public ActionResult SubmitForm1(Tg_JobTorqueConfig entity, string KeyValue, string WcJobCd)//===复制时需要修改===
        {
            try
            {
                IDatabase database = DataFactory.Database();
                int IsOk = 0;//用于判断
                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";//keyValue=空吗？是，返回新增；否，返回编辑。
                if (!string.IsNullOrEmpty(KeyValue))//编辑操作
                {
                    //===复制时需要修改===
                    Tg_JobTorqueConfig Oldentity = repository_Tg_JobTorqueConfig.Repository().FindEntity(KeyValue);//获取没更新之前实体对象
                    entity.ID = KeyValue;
                    entity.MdfTm = DateTime.Now;
                    entity.MdfCd = ManageProvider.Provider.Current().UserId;
                    entity.MdfNm = ManageProvider.Provider.Current().UserName;

                    IsOk = database.Update(entity);//将修改后的实体更新到数据库，插入成功返回1，失败返回0；
                    //this.WriteLog(IsOk, entity, Oldentity, KeyValue, Message);//记录日志                 
                }
                else//新增操作
                {

                    entity.WcJobCd = WcJobCd;
                    StringBuilder sb1 = new StringBuilder();
                    sb1.Append(@"SELECT WcJobCd FROM dbo.Tg_WcJobConfig WHERE WcJobCd='" + WcJobCd + "'");
                    DataTable dt = database.FindTableBySql(sb1.ToString());
                    if (dt.Rows.Count == 0)//存在
                    {
                        Message = "工位JOB信息不存在！";
                        return Content(new JsonMessage { Success = false, Code = IsOk.ToString(), Message = Message }.ToString());
                    }
                    entity.CreTm = DateTime.Now;
                    entity.CreCd = ManageProvider.Provider.Current().UserId;
                    entity.CreNm = ManageProvider.Provider.Current().UserName;

                    IsOk = database.Insert(entity);//将实体插入数据库，插入成功返回1，失败返回0；
                    //this.WriteLog(IsOk, entity, null, KeyValue, Message);//记录日志
                }
                return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        [LoginAuthorize]
        public virtual ActionResult SetForm1(string KeyValue)
        {
            Tg_JobTorqueConfig entity = repository_Tg_JobTorqueConfig.Repository().FindEntity(KeyValue);
            return Content(entity.ToJson());
        }

        public ActionResult Delete(string KeyValue)
        {
            //不管是多个主键还是单个主键，将主键拆分出来，放在数组中
            string[] array = KeyValue.Split(',');
            try
            {
                var Message = "删除失败。";//定义返回信息，该信息将返回到界面上，给用户观看
                int IsOk = 0;//判断删除方法是否成，0表示不成功，大于0表示成功  
                IDatabase database = DataFactory.Database();
                for (int i = 0; i < array.Length; i++) //先把对应CarPartId中所有检查项目均删除
                {
                    Tg_WcJobConfig Oldentity = database.FindEntity<Tg_WcJobConfig>(array[i]);
                    IsOk = database.Delete(Oldentity);
                    if (IsOk > 0) Message = "删除成功。";
                }
                WriteLog(IsOk, array, Message);//记录日志
                return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                WriteLog(-1, array, "操作失败：" + ex.Message);//记录日志
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        public ActionResult Delete1(string KeyValue)
        {
            //不管是多个主键还是单个主键，将主键拆分出来，放在数组中
            string[] array = KeyValue.Split(',');
            try
            {
                var Message = "删除失败。";//定义返回信息，该信息将返回到界面上，给用户观看
                int IsOk = 0;//判断删除方法是否成，0表示不成功，大于0表示成功  
                IDatabase database = DataFactory.Database();
                for (int i = 0; i < array.Length; i++) //先把对应CarPartId中所有检查项目均删除
                {
                    Tg_JobTorqueConfig Oldentity = database.FindEntity<Tg_JobTorqueConfig>(array[i]);
                    IsOk = database.Delete(Oldentity);
                    if (IsOk > 0) Message = "删除成功。";
                }
                WriteLog(IsOk, array, Message);//记录日志
                return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                WriteLog(-1, array, "操作失败：" + ex.Message);//记录日志
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

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
        /// 下载Excell模板
        /// </summary>
        /// <returns></returns>
        public ActionResult GetExcellTemplate(string ImportId)
        {
            if (!string.IsNullOrEmpty(ImportId))
            {
                DataSet ds = new DataSet();
                DataTable data = new DataTable();
                string DataColumn = "";
                string fileName;
                Base_ExcelImportBll bll = new Base_ExcelImportBll();
                bll.GetExcellTemperature(ImportId, out data, out DataColumn, out fileName);
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
        /// <summary>
        /// Datatable去重
        /// </summary>
        /// <param name="table"></param>
        private static DataTable RemoveDuplicateRows(DataTable table)
        {
            ArrayList UniqueRecords = new ArrayList();
            ArrayList DuplicateRecords = new ArrayList();

            // Check if records is already added to UniqueRecords otherwise,
            // Add the records to DuplicateRecords
            foreach (DataRow dRow in table.Rows)
            {
                //这里是需要比较是否为重复行的四个列。如果只有一个列需要比较，那么就写一个字段就可以了。
                string compareValue = dRow[0].ToString() + dRow[1] + dRow[2] + dRow[3] + dRow[4];
                if (UniqueRecords.Contains(compareValue))
                    DuplicateRecords.Add(dRow);
                else
                    UniqueRecords.Add(compareValue);
            }

            // Remove duplicate rows from DataTable added to DuplicateRecords
            foreach (DataRow dRow in DuplicateRecords)
            {
                table.Rows.Remove(dRow);
            }

            // Return the clean DataTable which contains unique records.
            return table;
        }
        /// <summary>
        /// 导入Excell数据
        /// </summary>
        /// <returns></returns>
        public ActionResult ImportExel(string AlarmId)
        {
            int IsOk = 0;//导入状态
            IDatabase database = DataFactory.Database();
            //构造导入返回结果表
            DataTable Newdt = new DataTable();
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
                    dt.Columns.Add("rowid", typeof(System.String));//用来标识excell行ID
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dt.Rows[i]["rowid"] = i + 1;
                    }
                    RemoveEmpty(dt);//清除空行
                    //RemoveDuplicateRows(dt);
                    StringBuilder sb = new StringBuilder();
                    sb.Append(@"SELECT WcId,
                                       PlineId,
                                       WcCd,
                                       WcNm,
                                       WcTyp,
                                       WcQueue,
                                       WcLength,
                                       StartPoint,
                                       PreAlarmPoint,
                                       StopPoint,
                                       EndPoint,
                                       Seq,
                                       Dsc,
                                       VersionNumber,
                                       Enabled,
                                       CreTm,
                                       CreCd,
                                       CreNm,
                                       MdfTm,
                                       MdfCd,
                                       MdfNm,
                                       Rem,
                                       RsvFld1,
                                       RsvFld2,
                                       Sort FROM dbo.BBdbR_WcBase");

                    List<BBdbR_WcBase> wcList = database.FindListBySql<BBdbR_WcBase>(sb.ToString());
                    StringBuilder sb1 = new StringBuilder();
                    sb1.Append(@"SELECT ID,WcJobCd,
                                        WcId,
                                        WcCd,
                                        WcNm,
                                        Position,
                                        JobCd,
                                        ControllerID,
                                        ControllerPort,
                                        CreTm,
                                        CreCd,
                                        CreNm,
                                        MdfTm,
                                        MdfCd,
                                        MdfNm,
                                        Rem,
                                        RsvFld1,
                                        RsvFld2 FROM dbo.Tg_WcJobConfig");
                    List<Tg_WcJobConfig> wcjList = database.FindListBySql<Tg_WcJobConfig>(sb1.ToString());
                    List<Tg_WcJobConfig> istList = new List<Tg_WcJobConfig>();
                    List<Tg_JobTorqueConfig> istList1 = new List<Tg_JobTorqueConfig>();
                    //第一遍遍历，插入JOB信息
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = Newdt.NewRow();
                        string WcJobCd = dt.Rows[i][0].ToString().Trim();
                        string WcNm = dt.Rows[i][1].ToString().Trim();
                        string Position = dt.Rows[i][2].ToString().Trim();
                        string JobCd = dt.Rows[i][3].ToString().Trim();
                        string ControllerID = dt.Rows[i][4].ToString().Trim();
                        string ControllerPort = dt.Rows[i][5].ToString().Trim();

                        //decimal TorqueUL =Convert.ToDecimal(dt.Rows[i][6].ToString().Trim());
                        //decimal TorqueLL = Convert.ToDecimal(dt.Rows[i][7].ToString().Trim());
                        //decimal TorqueSL = Convert.ToDecimal(dt.Rows[i][8].ToString().Trim());
                        //decimal AngleUL = Convert.ToDecimal(dt.Rows[i][9].ToString().Trim());
                        //decimal AngleLL = Convert.ToDecimal(dt.Rows[i][10].ToString().Trim());
                        //decimal AngleSL = Convert.ToDecimal(dt.Rows[i][11].ToString().Trim());

                        //int Num = Convert.ToInt32(dt.Rows[i][12].ToString().Trim());
                        //int Ord = Convert.ToInt32(dt.Rows[i][13].ToString().Trim());
                        //检验和数据库是否重复
                        if (!wcjList.Exists(s => s.WcJobCd == WcJobCd))
                        {//检验关键项是否为空
                            if (!string.IsNullOrEmpty(WcJobCd) && !string.IsNullOrEmpty(WcNm) && !string.IsNullOrEmpty(Position) && !string.IsNullOrEmpty(ControllerID) && !string.IsNullOrEmpty(ControllerPort) && !string.IsNullOrEmpty(JobCd))
                            {
                                //检验工位是否存在
                                BBdbR_WcBase wc = wcList.FirstOrDefault(s => s.WcNm == WcNm);
                                if (wc != null)
                                {

                                    //检验和数据库是否重复
                                    if (!wcjList.Exists(s => s.WcNm == WcNm && s.Position == Position && s.JobCd == JobCd && s.ControllerID == ControllerID && s.ControllerPort == ControllerPort))
                                    {
                                        //如果数据库不存在相同的JOB项，则插入JOB表+拧紧配置表
                                        Tg_WcJobConfig ety = new Tg_WcJobConfig()
                                        {
                                            WcJobCd = WcJobCd,
                                            WcId = wc.WcId,
                                            WcCd = wc.WcCd,
                                            WcNm = WcNm,
                                            Position = Position,
                                            JobCd = JobCd,
                                            ControllerID = ControllerID,
                                            ControllerPort = ControllerPort,
                                            CreCd = ManageProvider.Provider.Current().UserId,
                                            CreNm = ManageProvider.Provider.Current().UserName,
                                            CreTm = DateTime.Now
                                        };
                                        istList.Add(ety);
                                        //Tg_JobTorqueConfig ety1 = new Tg_JobTorqueConfig()
                                        //{
                                        //    WcJobCd = WcJobCd,


                                        //    TorqueUL = TorqueUL,
                                        //    TorqueLL = TorqueLL,
                                        //    TorqueSL = TorqueSL,

                                        //    AngleUL = AngleUL,
                                        //    AngleLL = AngleLL,
                                        //    AngleSL = AngleSL,

                                        //    Num = Num,
                                        //    Ord = Ord,
                                        //    CreCd = ManageProvider.Provider.Current().UserId,
                                        //    CreNm = ManageProvider.Provider.Current().UserName,
                                        //    CreTm = DateTime.Now
                                        //};
                                        //istList1.Add(ety1);
                                    }
                                    else
                                    {
                                        //Tg_JobTorqueConfig ety1 = new Tg_JobTorqueConfig()
                                        //{
                                        //    WcJobCd = WcJobCd,
                                        //    TorqueUL = TorqueUL,
                                        //    TorqueLL = TorqueLL,
                                        //    TorqueSL = TorqueSL,
                                        //    AngleUL = AngleUL,
                                        //    AngleLL = AngleLL,
                                        //    AngleSL = AngleSL,
                                        //    Num = Num,
                                        //    Ord = Ord,
                                        //    CreCd = ManageProvider.Provider.Current().UserId,
                                        //    CreNm = ManageProvider.Provider.Current().UserName,
                                        //    CreTm = DateTime.Now
                                        //};
                                        //istList1.Add(ety1);
                                        dr = Newdt.NewRow();
                                        dr[0] = errorNum;
                                        dr[1] = "第[" + dt.Rows[i]["rowid"].ToString() + "]行仅导入拧紧数据";
                                        dr[2] = "数据库中已存在相同配置项";
                                        Newdt.Rows.Add(dr);
                                        continue;
                                    }



                                }
                                else
                                {
                                    dr = Newdt.NewRow();
                                    dr[0] = errorNum;
                                    dr[1] = "第[" + dt.Rows[i]["rowid"].ToString() + "]行导入失败";
                                    dr[2] = "工位不存在";
                                    Newdt.Rows.Add(dr);
                                    errorNum++;
                                    continue;
                                }
                            }
                            else
                            {
                                dr = Newdt.NewRow();
                                dr[0] = errorNum;
                                dr[1] = "第[" + dt.Rows[i]["rowid"].ToString() + "]行导入失败";
                                dr[2] = "数据不完整";
                                Newdt.Rows.Add(dr);
                                errorNum++;
                                continue;
                            }
                        }
                        else
                        {
                            dr = Newdt.NewRow();
                            dr[0] = errorNum;
                            dr[1] = "第[" + dt.Rows[i]["rowid"].ToString() + "]行跳过JOB信息";
                            dr[2] = "数据库中已存在相同工位JOB编号";
                            Newdt.Rows.Add(dr);
                            errorNum++;
                            continue;
                        }
                    }
                    //数据导入数据库
                    if (istList.Count > 0)
                    {
                        IsOk = database.Insert(istList);
                    }
                    List<Tg_WcJobConfig> wcjList2 = database.FindListBySql<Tg_WcJobConfig>(sb1.ToString());

                    //第二遍遍历，插入扭矩转角信息
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = Newdt.NewRow();
                        string WcJobCd = dt.Rows[i][0].ToString().Trim();

                        decimal TorqueUL = Convert.ToDecimal(dt.Rows[i][6].ToString().Trim());
                        decimal TorqueLL = Convert.ToDecimal(dt.Rows[i][7].ToString().Trim());
                        decimal TorqueSL = Convert.ToDecimal(dt.Rows[i][8].ToString().Trim());
                        decimal AngleUL = Convert.ToDecimal(dt.Rows[i][9].ToString().Trim());
                        decimal AngleLL = Convert.ToDecimal(dt.Rows[i][10].ToString().Trim());
                        decimal AngleSL = Convert.ToDecimal(dt.Rows[i][11].ToString().Trim());

                        int Num = Convert.ToInt32(dt.Rows[i][12].ToString().Trim());
                        int Ord = Convert.ToInt32(dt.Rows[i][13].ToString().Trim());

                        //检验和数据库是否重复
                        if (wcjList2.Exists(s => s.WcJobCd == WcJobCd))
                        {
                            Tg_JobTorqueConfig ety1 = new Tg_JobTorqueConfig()
                            {
                                WcJobCd = WcJobCd,


                                TorqueUL = TorqueUL,
                                TorqueLL = TorqueLL,
                                TorqueSL = TorqueSL,

                                AngleUL = AngleUL,
                                AngleLL = AngleLL,
                                AngleSL = AngleSL,

                                Num = Num,
                                Ord = Ord,
                                CreCd = ManageProvider.Provider.Current().UserId,
                                CreNm = ManageProvider.Provider.Current().UserName,
                                CreTm = DateTime.Now
                            };
                            istList1.Add(ety1);


                        }
                        else
                        {
                            dr = Newdt.NewRow();
                            dr[0] = errorNum;
                            dr[1] = "第[" + dt.Rows[i]["rowid"].ToString() + "]行扭矩信息导入失败";
                            dr[2] = "数据库中不存在该工位JOB编号";
                            Newdt.Rows.Add(dr);
                            errorNum++;
                            continue;
                        }
                    }
                    if (istList1.Count > 0)
                    {
                        IsOk = database.Insert(istList1);
                    }
                }
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Add, "-1", "异常错误：" + ex.Message);
                IsOk = 0;
            }
            if (Newdt.Rows.Count > 0)
            {
                IsOk = 0;
            }
            var JsonData = new
            {
                Status = IsOk > 0 ? "true" : "false",
                ResultData = Newdt
            };
            return Content(JsonData.ToJson());
        }
    }
}