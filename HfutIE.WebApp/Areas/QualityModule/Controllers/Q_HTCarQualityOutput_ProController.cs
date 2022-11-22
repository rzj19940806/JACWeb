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

namespace HfutIE.WebApp.Areas.QualityModule.Controllers
{
    /// <summary>
    /// 冲焊涂装质量检查销项过程表控制器
    /// </summary>
    public class Q_HTCarQualityOutput_ProController : PublicController<Q_HTCarQualityOutput_Pro>
    {

        #region  创建数据库操作对象区域
        Q_HTCarQualityOutput_ProBll MyBll = new Q_HTCarQualityOutput_ProBll();
        Q_HTCarQualityResult_ProBll ResultBll = new Q_HTCarQualityResult_ProBll();
        public readonly RepositoryFactory<Q_HTCarQualityResult_Pro> repository_Q_HTCarQualityResult_Pro = new RepositoryFactory<Q_HTCarQualityResult_Pro>();
        public readonly RepositoryFactory<Q_HTCarQualityOutput_Pro> repository_Q_HTCarQualityOutput_Pro = new RepositoryFactory<Q_HTCarQualityOutput_Pro>();
        #endregion

        #region 查询方法
        public ActionResult GetListByCondition( string VIN, string CarType,string TimeStart,string TimeEnd,string GN , JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                #region datatable查询
                StringBuilder strSql = new StringBuilder();
                strSql.Append(@"select * from Q_HTCarQualityOutput_Pro where Enabled = 1 ");

                List<DbParameter> parameter = new List<DbParameter>();
                #region 判断输入框内容添加检索条件
                //是否加VIN号模糊搜索
                if (VIN != "" && VIN != null)
                {
                    //strSql.Append(" and VIN like '%" + VIN + "%' ");
                    strSql.Append(" and VIN like @VIN ");
                    parameter.Add(DbFactory.CreateDbParameter("@VIN", "%" + VIN + "%"));
                }

                //是否加VIN号模糊搜索
                if (CarType != "" && CarType != null)
                {
                    //strSql.Append(" and CarType like '%" + CarType + "%' ");
                    strSql.Append(" and CarType like @CarType ");
                    parameter.Add(DbFactory.CreateDbParameter("@CarType", "%" + CarType + "%"));
                }
                else { }

                if (TimeStart != "" && TimeStart != null)
                {
                    //strSql.Append(" and DATEDIFF(day,'" + TimeStart + "',QualityInspectTm) >= 0");
                    //开始时间把@放在前面
                    strSql.Append(" and DateDiff(dd,@TimeStart,QualityInspectTm) >=0 ");
                    parameter.Add(DbFactory.CreateDbParameter("@TimeStart", TimeStart));
                }
                else { }
                if (TimeEnd != "" && TimeEnd != null)
                {
                    //strSql.Append(" and DATEDIFF(day,QualityInspectTm,'" + TimeEnd + "') >= 0 ");
                    strSql.Append(" and DateDiff(dd,QualityInspectTm,@TimeEnd) >=0 ");
                    parameter.Add(DbFactory.CreateDbParameter("@TimeEnd", TimeEnd));
                }
                else { }
                if (GN != "" && GN != null)
                {
                    //strSql.Append(" and DATEDIFF(day,QualityInspectTm,'" + TimeEnd + "') >= 0 ");
                    strSql.Append(" and RsvFld1 ='" + GN + "' ");
                }
                else { }

                #endregion

                //按照录入时间排序
                strSql.Append(" order by QualityInspectTm desc");

                //DataTable dt = DataFactory.Database().FindTableBySql(strSql.ToString(), false);
                DataTable dt = DataFactory.Database().FindTableBySql(strSql.ToString(), parameter.ToArray(), false);
                var JsonData = new
                {
                    jqgridparam.total,
                    jqgridparam.page,
                    jqgridparam.records,
                    costtime = CommonHelper.TimerEnd(watch),
                    rows = dt,
                };
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "1", "焊涂质量信息查询成功");
                return Content(JsonData.ToJson());
                #endregion
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "焊涂质量信息查询发生异常错误：" + ex.Message);
                return null;
            }
        }
        #endregion

        #region 查询方法
        public ActionResult GetResultByCondition(string VIN, string CarType, string TimeStart, string TimeEnd, string GN, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                #region datatable查询
                StringBuilder strSql = new StringBuilder();
                strSql.Append(@"select * from Q_HTCarQualityResult_Pro where Enabled = 1 ");

                #region 判断输入框内容添加检索条件
                //是否加VIN号模糊搜索
                if (VIN != "" && VIN != null)
                {
                    strSql.Append(" and VIN like '%" + VIN + "%' ");
                }
                else { }

                //是否加VIN号模糊搜索
                if (CarType != "" && CarType != null)
                {
                    strSql.Append(" and CarType like '%" + CarType + "%' ");
                }
                else { }

                if (TimeStart != "" && TimeStart != null)
                {
                    strSql.Append(" and DATEDIFF(day,'" + TimeStart + "',QualityInspectTm) >= 0");
                }
                else { }
                if (TimeEnd != "" && TimeEnd != null)
                {
                    strSql.Append(" and DATEDIFF(day,QualityInspectTm,'" + TimeEnd + "') >= 0 ");
                }
                else { }
                if (GN != "" && GN != null)
                {
                    strSql.Append(" and RsvFld1 ='" + GN + "' ");
                }
                else { }
                #endregion

                //按照录入时间排序
                strSql.Append(" order by QualityInspectTm desc");

                DataTable dt = DataFactory.Database().FindTableBySql(strSql.ToString(), false);
                var JsonData = new
                {
                    jqgridparam.total,
                    jqgridparam.page,
                    jqgridparam.records,
                    costtime = CommonHelper.TimerEnd(watch),
                    rows = dt,
                };
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "1", "焊涂质量结果信息查询成功");
                return Content(JsonData.ToJson());
                #endregion
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "焊涂质量结果信息查询发生异常错误：" + ex.Message);
                return null;
            }
        }
        #endregion

        #region 导入

        #region 建立底盘信息类
        public class ChassisNumberinfo
        {
            public string ChassisNumber { get; set; } //底盘号
            public string VIN { get; set; }           //VIN
            public string CarType { get; set; }      //车型
            public string QualityCheckPointGroupNm { get; set; }      //工段
            public DateTime? TransitionTm { get; set; }      //转序时间
        }
        #endregion


        #region 导入弹窗界面
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

        #region 导入Excell数据
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

            //用来存储各VIN号已有数据
            //List<Q_HTCarQualityResult_Pro> Carinformation = new List<Q_HTCarQualityResult_Pro>();                   //产品信息List

            List<Q_HTCarQualityResult_Pro> HTCarQualityResultList0 = new List<Q_HTCarQualityResult_Pro>();         //结果表List
            List<Q_HTCarQualityOutput_Pro> HTCarQualityOutputList0 = new List<Q_HTCarQualityOutput_Pro>();          //销项表List

            //用来存储车辆信息导入数据
            List<Q_HTCarQualityResult_Pro> HTCarQualityResultInforList = new List<Q_HTCarQualityResult_Pro>();    //结果表List
            //用来存储异常问题导入数据
            //List<Q_HTCarQualityResult_Pro> HTCarQualityResultList = new List<Q_HTCarQualityResult_Pro>();         //结果表List
            List<Q_HTCarQualityOutput_Pro> HTCarQualityOutputList = new List<Q_HTCarQualityOutput_Pro>();          //销项表List


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
                    #region 质量录入数据导入

                    string ChassisNumber = "";                                         //储存临时底盘号
                    List<ChassisNumberinfo> ChassisNumberList = new List<ChassisNumberinfo>();
                    #region 表中数据循环录入
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        IsCheck = 1;//重置标识
                        DataRow dr = Newdt.NewRow();

                        if (dt.Rows[i]["VIN"].ToString().Trim() != "")
                        {
                            //VIN不为空，为导入车辆信息
                            //不允许工段为空且只能输入“涂装”和“冲焊”
                            string qcpg = "工段";
                            if (dt.Rows[i]["工段"].ToString().Trim() != "涂装" && dt.Rows[i]["工段"].ToString().Trim() != "冲焊")
                            {
                                dr = Newdt.NewRow();
                                dr[0] = errorNum;
                                dr[1] = "第[" + dt.Rows[i]["rowid"].ToString() + "]行[VIN号]";
                                dr[2] = "请填写“涂装”或“冲焊”！";
                                Newdt.Rows.Add(dr);
                                errorNum++;
                                IsCheck = 0;
                                continue;
                            }
                            else { }

                            //车型不能为空
                            if (dt.Rows[i]["车型"].ToString().Trim() == "")
                            {
                                dr = Newdt.NewRow();
                                dr[0] = errorNum;
                                dr[1] = "第[" + dt.Rows[i]["rowid"].ToString() + "]行[VIN号]";
                                dr[2] = "车型不能为空！";
                                Newdt.Rows.Add(dr);
                                errorNum++;
                                IsCheck = 0;
                                continue;
                            }
                            else { }

                            //判断录入时间是否为空
                            if (dt.Rows[i]["录入时间"].ToString().Trim() == "")
                            {
                                dr = Newdt.NewRow();
                                dr[0] = errorNum;
                                dr[1] = "第[" + dt.Rows[i]["rowid"].ToString() + "]行[录入时间]";
                                dr[2] = "不能为空！";
                                Newdt.Rows.Add(dr);
                                errorNum++;
                                IsCheck = 0;
                                continue;
                            }
                            else { }

                            //先检测是否表中存在此VIN号数据
                            StringBuilder strSqlVIN = new StringBuilder();
                            DataTable dtCheckVIN = new DataTable();
                            strSqlVIN.Append(@"select * from Q_HTCarQualityResult_Pro where Enabled = 1  and VIN = '" + dt.Rows[i]["VIN"].ToString().Trim() + "' and QualityCheckPointGroupNm = '" + dt.Rows[i]["工段"].ToString().Trim() + "'");
                            dtCheckVIN = DataFactory.Database().FindTableBySql(strSqlVIN.ToString(), false);
                            if (dtCheckVIN.Rows.Count != 0)
                            {
                                dr = Newdt.NewRow();
                                dr[0] = errorNum;
                                dr[1] = "第[" + dt.Rows[i]["rowid"].ToString() + "]行[VIN号]";
                                dr[2] = "车辆信息在系统中已存在";
                                Newdt.Rows.Add(dr);
                                errorNum++;
                                IsCheck = 0;
                                continue;
                            }
                            else { }

                            //检测是否在HTCarQualityResultInforList中
                            bool ifexist = HTCarQualityResultInforList.Exists(t => t.VIN == dt.Rows[i]["VIN"].ToString().Trim()&& t.VIN == dt.Rows[i]["工段"].ToString().Trim());
                            if (ifexist)
                            {
                                dr = Newdt.NewRow();
                                dr[0] = errorNum;
                                dr[1] = "第[" + dt.Rows[i]["rowid"].ToString() + "]行[VIN号]";
                                dr[2] = "车辆信息在此表中已存在";
                                Newdt.Rows.Add(dr);
                                errorNum++;
                                IsCheck = 0;
                                continue;
                            }
                            else { }

                            //均不存在时，加入HTCarQualityResultInforList
                            Q_HTCarQualityResult_Pro newQ_HTCarQualityResult_Pro = new Q_HTCarQualityResult_Pro();
                            newQ_HTCarQualityResult_Pro.CarQualityResultId = Guid.NewGuid().ToString();         //结果表主键
                            newQ_HTCarQualityResult_Pro.VIN = dt.Rows[i]["VIN"].ToString().Trim();              //结果表VIN号
                            newQ_HTCarQualityResult_Pro.CarType = dt.Rows[i]["车型"].ToString().Trim();         //结果表车型
                            newQ_HTCarQualityResult_Pro.TransitionTm = Convert.ToDateTime(dt.Rows[i]["转序日期"].ToString().Trim());    //结果表转序日期
                            newQ_HTCarQualityResult_Pro.QualityCheckPointGroupNm = dt.Rows[i]["工段"].ToString().Trim();                //结果表工段

                            newQ_HTCarQualityResult_Pro.QualityResult = "合格";                                 //结果表检测结果默认合格
                            newQ_HTCarQualityResult_Pro.QualityInspectTm = Convert.ToDateTime(dt.Rows[i]["录入时间"].ToString().Trim());                        
                            newQ_HTCarQualityResult_Pro.Enabled = "1";                                          //结果表有效性
                            newQ_HTCarQualityResult_Pro.CreCd = ManageProvider.Provider.Current().UserId;       //结果表创建人编号
                            newQ_HTCarQualityResult_Pro.CreNm = ManageProvider.Provider.Current().UserName;     //结果表创建人姓名
                            newQ_HTCarQualityResult_Pro.CreTm = DateTime.Now;                                   //结果表创建时间
                            HTCarQualityResultInforList.Add(newQ_HTCarQualityResult_Pro);
                        }
                        else
                        {
                            //VIN为空，为导入异常信息

                            
                            //不允许工段为空且只能输入“涂装”和“冲焊”
                            if (dt.Rows[i]["工段"].ToString().Trim() != "涂装" && dt.Rows[i]["工段"].ToString().Trim() != "冲焊")
                            {
                                dr = Newdt.NewRow();
                                dr[0] = errorNum;
                                dr[1] = "第[" + dt.Rows[i]["rowid"].ToString() + "]行[VIN号]";
                                dr[2] = "请填写“涂装”或“冲焊”！";
                                Newdt.Rows.Add(dr);
                                errorNum++;
                                IsCheck = 0;
                                continue;
                            }
                            else { }
                            //判断录入时间是否为空
                            if (dt.Rows[i]["录入时间"].ToString().Trim() == "")
                            {
                                dr = Newdt.NewRow();
                                dr[0] = errorNum;
                                dr[1] = "第[" + dt.Rows[i]["rowid"].ToString() + "]行[录入时间]";
                                dr[2] = "不能为空！";
                                Newdt.Rows.Add(dr);
                                errorNum++;
                                IsCheck = 0;
                                continue;
                            }
                            else { }
                            //判断录入问题是否为空
                            if (dt.Rows[i]["问题"].ToString().Trim() == "")
                            {
                                dr = Newdt.NewRow();
                                dr[0] = errorNum;
                                dr[1] = "第[" + dt.Rows[i]["rowid"].ToString() + "]行[问题]";
                                dr[2] = "不能为空！";
                                Newdt.Rows.Add(dr);
                                errorNum++;
                                IsCheck = 0;
                                continue;
                            }
                            else { }

                            //判断底盘号是否为空
                            if (dt.Rows[i]["底盘号"].ToString().Trim() != "")
                            {
                                ChassisNumber = dt.Rows[i]["底盘号"].ToString().Trim();
                                //检查ChassisNumberList中是否已经存在此底盘号
                                bool isChassisNumberExist = ChassisNumberList.Exists(t => t.ChassisNumber == dt.Rows[i]["底盘号"].ToString().Trim()&& t.QualityCheckPointGroupNm == dt.Rows[i]["工段"].ToString().Trim());
                                if (!isChassisNumberExist)
                                {
                                    //这是一个新的底盘号
                                    string ThisVIN = "";
                                    #region 根据底盘号获取结果表数据并获取车辆信息，存入HTCarQualityResultList0和ChassisNumberList
                                    List<Q_HTCarQualityResult_Pro> newList = ResultBll.GetListByChassisNumber(ChassisNumber, dt.Rows[i]["工段"].ToString().Trim());
                                    if (newList.Count>0)
                                    {
                                        Q_HTCarQualityResult_Pro newResult_Pro = newList[0];
                                        newResult_Pro.QualityResult = "不合格";
                                        newResult_Pro.QualityInspectTm = Convert.ToDateTime(dt.Rows[i]["录入时间"].ToString().Trim());
                                        HTCarQualityResultList0.Add(newResult_Pro);     //添加到HTCarQualityResultList0

                                        ThisVIN = newResult_Pro.VIN;
                                        ChassisNumberinfo s = new ChassisNumberinfo();
                                        s.ChassisNumber = ChassisNumber;
                                        s.VIN = newResult_Pro.VIN;
                                        s.CarType = newResult_Pro.CarType;
                                        s.QualityCheckPointGroupNm = dt.Rows[i]["工段"].ToString().Trim();
                                        s.TransitionTm = newResult_Pro.TransitionTm;
                                        ChassisNumberList.Add(s);                       //添加到ChassisNumberList
                                    }
                                    else
                                    {
                                        ChassisNumberinfo s = new ChassisNumberinfo();
                                        s.ChassisNumber = ChassisNumber;
                                        s.VIN = "";
                                        s.CarType = "";
                                        s.QualityCheckPointGroupNm = dt.Rows[i]["工段"].ToString().Trim();
                                        s.TransitionTm = null;
                                        ChassisNumberList.Add(s);                       //添加到ChassisNumberList
                                    }
                                    #endregion

                                    #region 根据底盘号查到的VIN号查找表中已存在的异常信息并存入HTCarQualityOutputList0
                                    List<Q_HTCarQualityOutput_Pro> newOutputList = MyBll.GetListByVIN(ThisVIN, dt.Rows[i]["工段"].ToString().Trim());
                                    foreach (Q_HTCarQualityOutput_Pro newoutput in newOutputList)
                                    {
                                        HTCarQualityOutputList0.Add(newoutput);         //添加到HTCarQualityOutputList0
                                    }
                                    #endregion


                                }
                                else { }
                            }
                            else { }

                            //校验底盘号
                            ChassisNumberinfo thisChassnumberinfo = ChassisNumberList.Find(t => t.ChassisNumber == ChassisNumber && t.QualityCheckPointGroupNm == dt.Rows[i]["工段"].ToString().Trim());
                            if (thisChassnumberinfo.VIN == "" || thisChassnumberinfo.VIN == null)
                            {
                                dr = Newdt.NewRow();
                                dr[0] = errorNum;
                                dr[1] = "第[" + dt.Rows[i]["rowid"].ToString() + "]行[底盘号]";
                                dr[2] = "未查找到车辆信息，请先导入车辆信息！";
                                Newdt.Rows.Add(dr);
                                errorNum++;
                                IsCheck = 0;
                                continue;
                            }
                            else { }

                            #region 校验销项表系统中是否有重复信息
                            bool isOutput0Exist = HTCarQualityOutputList0.Exists(t => t.VIN == thisChassnumberinfo.VIN && t.QualityCheckPointGroupNm == dt.Rows[i]["工段"].ToString().Trim() && t.DefectNm == dt.Rows[i]["问题"].ToString().Trim( )&& t.DefectAnalysis == dt.Rows[i]["备注"].ToString().Trim());
                            if (isOutput0Exist)
                            {
                                dr = Newdt.NewRow();
                                dr[0] = errorNum;
                                dr[1] = "第[" + dt.Rows[i]["rowid"].ToString() + "]行[车辆异常信息]";
                                dr[2] = "在系统中已存在！";
                                Newdt.Rows.Add(dr);
                                errorNum++;
                                IsCheck = 0;
                                continue;
                            }
                            else { }
                            #endregion

                            #region 校验销项表EXCEL中是否有重复信息
                            bool isOutputExist = HTCarQualityOutputList.Exists(t => t.VIN == thisChassnumberinfo.VIN && t.QualityCheckPointGroupNm == dt.Rows[i]["工段"].ToString().Trim() && t.DefectNm == dt.Rows[i]["问题"].ToString().Trim() && t.DefectAnalysis == dt.Rows[i]["备注"].ToString().Trim());
                            if (isOutputExist)
                            {
                                dr = Newdt.NewRow();
                                dr[0] = errorNum;
                                dr[1] = "第[" + dt.Rows[i]["rowid"].ToString() + "]行[车辆异常信息]";
                                dr[2] = "在此表中已存在！";
                                Newdt.Rows.Add(dr);
                                errorNum++;
                                IsCheck = 0;
                                continue;
                            }
                            else { }

                            #endregion

                            #region 更新结果表中的车辆信息
                            //将HTCarQualityResultList0检测结果更新为不合格即可,上方已修改
                            #endregion

                            #region 添加销项表中的车辆信息
                            //获取结果表主键
                            Q_HTCarQualityResult_Pro resultpro = HTCarQualityResultList0.Find(t => t.VIN == thisChassnumberinfo.VIN && t.QualityCheckPointGroupNm == dt.Rows[i]["工段"].ToString().Trim());
                            Q_HTCarQualityOutput_Pro newoutputpro = new Q_HTCarQualityOutput_Pro();
                            newoutputpro.CarInspectionOutputId = Guid.NewGuid().ToString();         //销项表主键
                            newoutputpro.CarQualityResultId = resultpro.CarQualityResultId;         //对应结果表主键
                            newoutputpro.VIN = resultpro.VIN;                                       //销项表VIN号
                            newoutputpro.CarType = resultpro.CarType;                               //销项表车型
                            newoutputpro.TransitionTm = resultpro.TransitionTm;                     //销项表转序时间
                            newoutputpro.QualityCheckPointGroupNm = dt.Rows[i]["工段"].ToString().Trim();//销项表工段
                            newoutputpro.QualityCheckPointNm = "导入";
                            newoutputpro.DefectNm = dt.Rows[i]["问题"].ToString().Trim();           //销项表问题
                            newoutputpro.DefectAnalysis = dt.Rows[i]["备注"].ToString().Trim();     //销项表问题分析
                            newoutputpro.StfCd = dt.Rows[i]["录入人员工号"].ToString().Trim();      //销项表录入人员工号
                            newoutputpro.StfNm = dt.Rows[i]["录入人员姓名"].ToString().Trim();      //销项表录入人员姓名
                            newoutputpro.QualityInspectTm = Convert.ToDateTime(dt.Rows[i]["录入时间"].ToString().Trim());//销项表录入时间
                            newoutputpro.Enabled = "1";                                             //销项表有效性
                            newoutputpro.CreCd = ManageProvider.Provider.Current().UserId;
                            newoutputpro.CreNm = ManageProvider.Provider.Current().UserName;
                            newoutputpro.CreTm = DateTime.Now;
                            HTCarQualityOutputList.Add(newoutputpro);                               //将实体添加到HTCarQualityOutputList中
                            #endregion

                        }
                        
                    }
                    #endregion

                   

                    #region 向三个表中插入数据
                    //向结果表中插入车辆基础信息
                    int b0 = database.Insert(HTCarQualityResultInforList);
                    if (b0 > 0)
                    {
                        IsOk = IsOk + b0;
                        HTCarQualityResultInforList.Clear();
                        ChassisNumberList.Clear();
                    }
                    else
                    {

                    }


                    //向结果表中更新结果为不合格
                    int b1 = database.Update(HTCarQualityResultList0);
                    if (b1 > 0)
                    {
                        IsOk = IsOk + b0;
                        HTCarQualityResultList0.Clear();
                        ChassisNumberList.Clear();
                    }
                    else
                    {

                    }

                    //销项表
                    int b2 = database.Insert(HTCarQualityOutputList);
                    if (b2 > 0)
                    {
                        IsOk = IsOk + b2;
                        HTCarQualityOutputList.Clear();
                        HTCarQualityOutputList0.Clear();
                    }
                    else
                    {

                    }
                    Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other, "1", "焊涂质量数据导入成功");
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
                Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other, "-1", "焊涂质量数据导入操作失败：" + ex.Message);
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

        #region 删除结果表数据
       
        public  ActionResult DeleteResult(string KeyValue)
        {
            //不管是多个主键还是单个主键，将主键拆分出来，放在数组中
            string[] array = KeyValue.Split(',');
            try
            {
                var Message = "删除失败。";//定义返回信息，该信息将返回到界面上，给用户观看
                int IsOk = 0;//判断删除方法是否成，0表示不成功，大于0表示成功  

                for (int i = 0; i < array.Length; i++) //先把对应CarPartId中所有检查项目均删除
                {
                    //Q_HTCarQualityResult_Pro Oldentity = repository_Q_HTCarQualityResult_Pro.Repository().FindEntity(array[i]);
                    //删除此条结果记录下的所有缺陷记录
                    int num = MyBll.deleteByResult(array[i]);
                    //删除此条结果记录
                    IsOk = ResultBll.Delete(array[i]);
                    if (IsOk > 0) Message = "删除成功。";
                }
                WriteLog(IsOk, array, Message);//记录日志
                return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = Message + "，同时删除缺陷记录" }.ToString());
            }
            catch (Exception ex)
            {
                WriteLog(-1, array, "操作失败：" + ex.Message);//记录日志
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion

        #region 删除缺陷过程表数据

        public ActionResult DeleteOutput(string KeyValue)
        {
            //不管是多个主键还是单个主键，将主键拆分出来，放在数组中
            string[] array = KeyValue.Split(',');
            try
            {
                var Message = "删除失败。";//定义返回信息，该信息将返回到界面上，给用户观看
                int IsOk = 0;//判断删除方法是否成，0表示不成功，大于0表示成功  

                for (int i = 0; i < array.Length; i++) //先把对应CarPartId中所有检查项目均删除
                {
                    //Q_HTCarQualityOutput_Pro Oldentity = repository_Q_HTCarQualityOutput_Pro.Repository().FindEntity(array[i]);
                   
                    //删除此条结果记录
                    IsOk = MyBll.Delete(array[i]);
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
        #endregion

        #endregion

        #region 2.重构导出方法
        /// <summary>
        /// 1.如果是按照条件查询后再进行
        /// </summary>
        /// <param name="type"></param>
        /// <param name="jqgridparam"></param>
        /// <returns></returns>
        public ActionResult GetExcel_Data(string QualityCheckPointGroupNm,  string VIN, string CarType, string TimeStart, string TimeEnd, JqGridParam jqgridparam)
        {
            try
            {
                #region 获取数据---需要修改
                StringBuilder strSql = new StringBuilder();
                strSql.Append(@"select QualityCheckPointGroupNm,VIN,CarType,CarComponentNm,DefectNm,DefectAnalysis,CreCd,CreNm,CreTm	 from Q_HTCarQualityOutput_Pro where Enabled = 1 ");

                List<DbParameter> parameter = new List<DbParameter>();
                #region 判断输入框内容添加检索条件
                //是否加VIN号模糊搜索
                if (VIN != "" && VIN != null)
                {
                    //strSql.Append(" and VIN like '%" + VIN + "%' ");
                    strSql.Append(" and VIN like @VIN ");
                    parameter.Add(DbFactory.CreateDbParameter("@VIN", "%" + VIN + "%"));
                }

                //是否加VIN号模糊搜索
                if (CarType != "" && CarType != null)
                {
                    //strSql.Append(" and CarType like '%" + CarType + "%' ");
                    strSql.Append(" and CarType like @CarType ");
                    parameter.Add(DbFactory.CreateDbParameter("@CarType", "%" + CarType + "%"));
                }
                else { }

                if (TimeStart != "" && TimeStart != null)
                {
                    //strSql.Append(" and DATEDIFF(day,'" + TimeStart + "',QualityInspectTm) >= 0");
                    //开始时间把@放在前面
                    strSql.Append(" and DateDiff(dd,@TimeStart,QualityInspectTm) >=0 ");
                    parameter.Add(DbFactory.CreateDbParameter("@TimeStart", TimeStart));
                }
                else { }
                if (TimeEnd != "" && TimeEnd != null)
                {
                    //strSql.Append(" and DATEDIFF(day,QualityInspectTm,'" + TimeEnd + "') >= 0 ");
                    strSql.Append(" and DateDiff(dd,QualityInspectTm,@TimeEnd) >=0 ");
                    parameter.Add(DbFactory.CreateDbParameter("@TimeEnd", TimeEnd));
                }
                else { }
                if (QualityCheckPointGroupNm != "" && QualityCheckPointGroupNm != null)
                {
                    //strSql.Append(" and DATEDIFF(day,QualityInspectTm,'" + TimeEnd + "') >= 0 ");
                    strSql.Append(" and RsvFld1 ='" + QualityCheckPointGroupNm + "' ");
                }
                else { }

                #endregion

                //按照录入时间排序
                strSql.Append(" order by QualityInspectTm desc");

                //DataTable dt = DataFactory.Database().FindTableBySql(strSql.ToString(), false);
                DataTable dt = DataFactory.Database().FindTableBySql(strSql.ToString(), parameter.ToArray(), false);
                #endregion
                string fileName = "冲涂质量录入数据";
                string excelType = "xls";
                MemoryStream ms = DeriveExcel.ExportExcel_HTCarQualityOutput(dt, excelType);//需要修改
                if (!fileName.EndsWith(".xls"))
                {
                    fileName = fileName + ".xls";
                }
                Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other, "1", "质量录入数据导出成功");
                return File(ms, "application/vnd.ms-excel", Url.Encode(fileName));
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other, "-1", "质量录入数据导出操作失败：" + ex.Message);
                return null;
            }


        }
        #endregion




    }
}