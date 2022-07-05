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
    /// 物料加注配置表控制器
    /// </summary>
    public class BBDBR_MatJZConfigController : PublicController<BBDBR_MatJZConfig>
    {
        #region 创建数据库操作对象区域
        BBDBR_MatJZConfigBll MyBll = new BBDBR_MatJZConfigBll(); //===复制时需要修改===
        #endregion

        #region 搜索
        public ActionResult GridPageByCondition(string MatCd,string JZType, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                StringBuilder strSql = new StringBuilder();
                strSql.Append($"select * from BBDBR_MatJZConfig where Enabled = 1   ");
                if (!String.IsNullOrEmpty(MatCd))
                {
                    strSql.Append($" and MatCd like '%" + MatCd + "%'  ");
                }
                else { }
                if (!String.IsNullOrEmpty(JZType))
                {
                    strSql.Append($" and JZType like '%" + JZType + "%'  ");
                }
                else { }
                strSql.Append("  order by MatCd asc ");
                DataTable ListData = DataFactory.Database().FindTableBySql(strSql.ToString(), false);

                var JsonData = new
                {
                    jqgridparam.total,
                    jqgridparam.page,
                    jqgridparam.records,
                    costtime = CommonHelper.TimerEnd(watch),
                    rows = ListData,
                };
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "1", "物料加注配置信息查询成功");
                return Content(ListData.ToJson());
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "物料加注配置信息查询发生异常错误：" + ex.Message);
                return null;
            }
        }
        #endregion

        #region 新增编辑方法
        public override ActionResult SubmitForm(BBDBR_MatJZConfig entity, string KeyValue)//===复制时需要修改===
        {
            try
            {
                int IsOk = 0;//用于判断
                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";

                if (!string.IsNullOrEmpty(KeyValue))//编辑操作
                {
                    //===复制时需要修改===
                    BBDBR_MatJZConfig oldentity = repositoryfactory.Repository().FindEntity(KeyValue);//获取没更新之前实体对象
                    entity.Modify(KeyValue);
                    IsOk = MyBll.Update(entity);//将修改后的实体更新到数据库，插入成功返回1，失败返回0；
                    this.WriteLog(IsOk, entity, oldentity, KeyValue, Message);//记录日志
                }
                else//新增操作
                {
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

        #region 删除方法
        /// <summary>
        /// <param name="KeyValue">页面中提供的需要删除的数据的主键,可能是多条数据的主键，即多个主键</param>
        /// <param name="ParentId">不需要</param>
        /// <param name="DeleteMark">不需要</param>
        /// <returns></returns>
        public ActionResult DeleteRule(string KeyValue, string ParentId, string DeleteMark)
        {
            //不管是多个主键还是单个主键，将主键拆分出来，放在数组中
            string[] array = KeyValue.Split(',');
            try
            {
                var Message = "删除失败。";//定义返回信息，该信息将返回到界面上，给用户观看
                int IsOk = 0;//判断删除方法是否成，0表示不成功，大于0表示成功                
                //假删除
                IsOk = MyBll.Delete(array);//执行删除操作
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

        #region 导出
        public ActionResult GetExcel_Data(string MatCd, string JZType, JqGridParam jqgridparam)
        {
            try
            {

                #region 根据当前搜索条件查出数据并导出
                StringBuilder strSql = new StringBuilder();
                strSql.Append($"select MatCd,JZType,JZNumber,CreTm,CreCd,CreNm,MdfTm,MdfCd,MdfNm,Rem from BBDBR_MatJZConfig where Enabled = 1   ");
                if (!String.IsNullOrEmpty(MatCd))
                {
                    strSql.Append($" and MatCd like '%" + MatCd + "%'  ");
                }
                else { }
                if (!String.IsNullOrEmpty(JZType))
                {
                    strSql.Append($" and JZType like '%" + JZType + "%'  ");
                }
                else { }
                strSql.Append("  order by MatCd asc ");
                DataTable ListData = DataFactory.Database().FindTableBySql(strSql.ToString(), false);

                #endregion
                
                string fileName = "物料加注配置";
                string excelType = "xls";
                MemoryStream ms = DeriveExcel.ExportExcel_MatJZConfig(ListData, excelType);
                if (!fileName.EndsWith(".xls"))
                {
                    fileName = fileName + ".xls";
                }
                Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other, "1", "物料加注配置信息导出成功");
                return File(ms, "application/vnd.ms-excel", Url.Encode(fileName));
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other, "-1", "物料加注配置信息导出操作失败：" + ex.Message);
                return null;
            }

        }
        #endregion

        #region 导入
        
        #region 导入Excel弹出框页面
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
        #endregion
        
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

        #region 清除空行
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

        #region 导入Excel数据
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
            
            //用来存储导入数据
            List<BBDBR_MatJZConfig> BBDBR_MatJZConfigList = new List<BBDBR_MatJZConfig>();         //结果表List
            List<BBDBR_MatJZConfig> BBDBR_MatJZConfigUpdateList = new List<BBDBR_MatJZConfig>();   //结果表List

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
                    #region 物料加注配置信息导入
                    
                    #region 表中数据循环录入
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        IsCheck = 1;//重置标识
                        DataRow dr = Newdt.NewRow();

                        string MatId = "";//物料Id
                        #region 校验物料编号
                        if (dt.Rows[i]["物料编号"].ToString().Trim() != "")
                        {
                            //判断物料编号是否存在
                            StringBuilder strSqlCheckMatCd = new StringBuilder();
                            DataTable dtCheckMatCd = new DataTable();
                            strSqlCheckMatCd.Append(@" select * from BBdbR_MatBase where MatCd = '" + dt.Rows[i]["物料编号"].ToString().Trim() + "' and Enabled = '1' ");
                            dtCheckMatCd = DataFactory.Database().FindTableBySql(strSqlCheckMatCd.ToString(), false);
                            if (dtCheckMatCd.Rows.Count > 0)
                            {
                                MatId = dtCheckMatCd.Rows[0]["MatId"].ToString();
                            }
                            else
                            {
                                dr = Newdt.NewRow();
                                dr[0] = errorNum;
                                dr[1] = "第[" + dt.Rows[i]["rowid"].ToString() + "]行";
                                dr[2] = "物料编号不存在";
                                Newdt.Rows.Add(dr);
                                errorNum++;
                                IsCheck = 0;
                                continue;
                            }

                        }
                        else
                        {
                            dr = Newdt.NewRow();
                            dr[0] = errorNum;
                            dr[1] = "第[" + dt.Rows[i]["rowid"].ToString() + "]行";
                            dr[2] = "物料编号不能为空";
                            Newdt.Rows.Add(dr);
                            errorNum++;
                            IsCheck = 0;
                            continue;
                        }
                        #endregion

                        #region 校验加注类型
                        if (dt.Rows[i]["加注类型"].ToString().Trim() != "")
                        {

                        }
                        else
                        {
                            dr = Newdt.NewRow();
                            dr[0] = errorNum;
                            dr[1] = "第[" + dt.Rows[i]["rowid"].ToString() + "]行";
                            dr[2] = "加注类型不能为空";
                            Newdt.Rows.Add(dr);
                            errorNum++;
                            IsCheck = 0;
                            continue;
                        }
                        #endregion

                        #region 判断物料加注配置信息在Excel中是否存在
                        bool IfExitInExcel1 = BBDBR_MatJZConfigList.Exists(t => t.MatCd == dt.Rows[i]["物料编号"].ToString().Trim());
                        bool IfExitInExcel2 = BBDBR_MatJZConfigUpdateList.Exists(t => t.MatCd == dt.Rows[i]["物料编号"].ToString().Trim());
                        if (IfExitInExcel1 == true || IfExitInExcel2 == true)
                        {
                            dr = Newdt.NewRow();
                            dr[0] = errorNum;
                            dr[1] = "第[" + dt.Rows[i]["rowid"].ToString() + "]行";
                            dr[2] = "此物料加注配置信息在此表中已存在";
                            Newdt.Rows.Add(dr);
                            errorNum++;
                            IsCheck = 0;
                            continue;
                        }
                        else { }
                        #endregion

                        #region 判断物料加注信息在系统中是否存在
                        StringBuilder strSqlCheckExit = new StringBuilder();
                        DataTable dtCheckExit = new DataTable();
                        strSqlCheckExit.Append(@" select * from BBDBR_MatJZConfig where MatCd = '" + dt.Rows[i]["物料编号"].ToString().Trim() + "' and Enabled = '1' ");
                        dtCheckExit = DataFactory.Database().FindTableBySql(strSqlCheckExit.ToString(), false);
                        if (dtCheckExit.Rows.Count>0)
                        {
                            dr = Newdt.NewRow();
                            dr[0] = errorNum;
                            dr[1] = "第[" + dt.Rows[i]["rowid"].ToString() + "]行";
                            dr[2] = "此物料加注配置信息在系统中已存在，原数据已被覆盖";
                            Newdt.Rows.Add(dr);
                            errorNum++;
                            IsCheck = 0;

                            BBDBR_MatJZConfig newentity = new BBDBR_MatJZConfig();
                            newentity.Id = MatId;
                            newentity.MatCd = dt.Rows[i]["物料编号"].ToString().Trim();
                            newentity.JZType = dt.Rows[i]["加注类型"].ToString().Trim();
                            newentity.JZNumber = Convert.ToDecimal(dt.Rows[i]["加注量"].ToString().Trim());
                            newentity.Enabled = "1";
                            newentity.CreTm = System.DateTime.Now;
                            newentity.CreCd = ManageProvider.Provider.Current().UserId;
                            newentity.CreNm = ManageProvider.Provider.Current().UserName;
                            BBDBR_MatJZConfigUpdateList.Add(newentity);

                            //StringBuilder strSqlupdate = new StringBuilder();
                            //strSqlupdate.Append(@"update BBDBR_MatJZConfig set JZType = '" + newentity.JZType + "' , JZNumber = '" + newentity.JZNumber + "', CreTm = '" + newentity.CreTm + "' ,CreCd = '" + newentity.CreCd + "',CreNm = '" + newentity.CreNm + "'  where MatCd = '" + dt.Rows[i]["物料编号"].ToString().Trim() + "' and Enabled = '1' ");
                            //int a = DataFactory.Database().ExecuteBySql(strSqlupdate);
                        }
                        else
                        {
                            BBDBR_MatJZConfig newentity = new BBDBR_MatJZConfig();
                            newentity.Id = MatId;
                            newentity.MatCd = dt.Rows[i]["物料编号"].ToString().Trim();
                            newentity.JZType = dt.Rows[i]["加注类型"].ToString().Trim();
                            newentity.JZNumber = Convert.ToDecimal(dt.Rows[i]["加注量"].ToString().Trim());
                            newentity.Enabled = "1";
                            newentity.CreTm = System.DateTime.Now;
                            newentity.CreCd = ManageProvider.Provider.Current().UserId;
                            newentity.CreNm = ManageProvider.Provider.Current().UserName;
                            BBDBR_MatJZConfigList.Add(newentity); 
                        }
                        #endregion
                        
                    }
                    #endregion
                    
                    #region 插入和更新数据
                    //新增数据
                    int b0 = database.Insert(BBDBR_MatJZConfigList);
                    if (b0 > 0)
                    {
                        IsOk = IsOk + b0;
                        BBDBR_MatJZConfigList.Clear();
                    }
                    else
                    {

                    }

                    //更新数据
                    int b1 = database.Update(BBDBR_MatJZConfigUpdateList);
                    if (b1 > 0)
                    {
                        IsOk = IsOk + b1;
                        BBDBR_MatJZConfigUpdateList.Clear();
                    }
                    else
                    {

                    }

                    #endregion



                    //if (IsCheck == 0)
                    //{
                    //    IsOk = 0;
                    //}
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



    }
}