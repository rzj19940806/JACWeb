using HfutIE.Business;
using HfutIE.DataAccess;
using HfutIE.Entity;
using HfutIE.Repository;
using HfutIE.Utilities;
using Newtonsoft.Json.Linq;
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
    /// 车身质量检查销项过程表控制器
    /// </summary>
    public class Q_CarQualityOutput_ProController : PublicController<Q_CarQualityOutput_Pro>
    {
        
        #region 全局变量定义区
        //定义本页面主要操作的表的表名，称为主表
        string tableName = "Q_CarQualityOutput_Pro";
        public static DataTable CarQualityOutputList = new DataTable();
        #endregion

        #region  创建数据库操作对象区域
        Q_CarQualityOutput_ProBll MyBll = new Q_CarQualityOutput_ProBll();
        Q_CarQualityInspection_ProBll MyBll2 = new Q_CarQualityInspection_ProBll();
        Q_CarQualityResult_ProBll MyResultBll = new Q_CarQualityResult_ProBll();
        public readonly RepositoryFactory<Q_CarQualityOutput_Pro> repository_Q_CarQualityResult_Pro = new RepositoryFactory<Q_CarQualityOutput_Pro>();
        #endregion

        #region 1.查询
        /// <summary>
        /// 
        /// </summary>
        /// <param name="keywords">查询值</param>
        /// <param name="Condition">搜索条件</param>
        /// <param name="TimeStart">开始时间</param>
        /// <param name="TimeEnd">结束时间</param>
        /// <param name="jqgridparam">分页参数</param>
        /// <returns></returns>
        public ActionResult GetPageListByCondition(string QualityCheckPointGroupNm, string QualityCheckPointNm, string CarComponentNm, string DefectNm,  string OutputResult, string ReinspectionNumber,  string VIN, string CarType, string TimeStart, string TimeEnd, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                DataTable dt = MyBll.GetPageListByCondition(QualityCheckPointGroupNm, QualityCheckPointNm, CarComponentNm, DefectNm, OutputResult, ReinspectionNumber, VIN, CarType, TimeStart, TimeEnd, jqgridparam);
                var JsonData = new
                {
                    total = jqgridparam.total,
                    page = jqgridparam.page,
                    records = jqgridparam.records,
                    costtime = CommonHelper.TimerEnd(watch),
                    rows = dt,
                };
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "1", "质量录入信息查询成功");
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "质量录入信息查询发生异常错误：" + ex.Message);
                return null;
            }
        }
        #endregion

        #region 1.查询结果信息
        /// <summary>
        /// 
        /// </summary>
        /// <param name="keywords">查询值</param>
        /// <param name="Condition">搜索条件</param>
        /// <param name="TimeStart">开始时间</param>
        /// <param name="TimeEnd">结束时间</param>
        /// <param name="jqgridparam">分页参数</param>
        /// <returns></returns>
        public ActionResult GetResultPageListByCondition(string QualityCheckPointGroupNm, string CarComponentNm, string DefectNm, string OutputResult, string ReinspectionNumber,  string VIN, string CarType, string TimeStart, string TimeEnd, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                DataTable dt = MyBll.GetResultPageListByCondition(QualityCheckPointGroupNm, VIN, CarType, TimeStart, TimeEnd, jqgridparam);
                var JsonData = new
                {
                    total = jqgridparam.total,
                    page = jqgridparam.page,
                    records = jqgridparam.records,
                    costtime = CommonHelper.TimerEnd(watch),
                    rows = dt,
                };
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "1", "质量结果信息查询成功");
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "质量结果信息查询发生异常错误：" + ex.Message);
                return null;
            }
        }
        #endregion

        #region 2.重构导出方法
        /// <summary>
        /// 1.如果是按照条件查询后再进行
        /// </summary>
        /// <param name="type"></param>
        /// <param name="jqgridparam"></param>
        /// <returns></returns>
        public ActionResult GetExcel_Data(string QualityCheckPointGroupNm, string QualityCheckPointNm, string CarComponentNm, string DefectNm, string OutputResult, string ReinspectionNumber, string VIN,string CarType, string TimeStart, string TimeEnd, JqGridParam jqgridparam)
        {
            try
            {
                #region 获取数据
                DataTable dt = MyBll.GetExcel_Data(QualityCheckPointGroupNm, QualityCheckPointNm,CarComponentNm, DefectNm, OutputResult, ReinspectionNumber, VIN, CarType, TimeStart, TimeEnd, jqgridparam);
                #endregion
                string fileName = "质量录入数据";
                string excelType = "xls";
                MemoryStream ms = DeriveExcel.ExportExcel_CarQualityOutput(dt, excelType);
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

        #region 3.高级检索
        //public ActionResult GridListJson2(string keyvalue, string StartTime, string EndTime, JqGridParam jqgridparam)
        //{
        //    DateTime? starttime = Convert.ToDateTime(StartTime);
        //    DateTime? endtime = Convert.ToDateTime(EndTime).AddDays(1);
        //    List<ConditionAndKey> list = new List<ConditionAndKey>();
        //    if (!string.IsNullOrEmpty(keyvalue))//存在输入的检索条件
        //    {
        //        JObject tempobj = JObject.Parse(keyvalue);
        //        foreach (var item in tempobj)
        //        {
        //            ConditionAndKey cond_key = new ConditionAndKey();
        //            cond_key.Condition = tempobj[item.Key]["Condition"].ToString();
        //            cond_key.Keywords = tempobj[item.Key]["Keywords"].ToString();
        //            cond_key.AndOr = tempobj[item.Key]["AndOr"].ToString();
        //            cond_key.ExpressExtension = tempobj[item.Key]["ExpressExtension"].ToString();
        //            list.Add(cond_key);
        //        }
        //    }
        //    Stopwatch watch = CommonHelper.TimerStart();
        //    CarQualityOutputList = MyBll.GridPageStockTryList(list, starttime, endtime, ref jqgridparam);
        //    var JsonData = new
        //    {
        //        total = jqgridparam.total,
        //        page = jqgridparam.page,
        //        records = jqgridparam.records,
        //        costtime = CommonHelper.TimerEnd(watch),
        //        rows = CarQualityOutputList,
        //    };
        //    return Content(JsonData.ToJson());
        //}

        //public ActionResult GridPageJsonNew2(string ParameterJson, JqGridParam jqgridparam)
        //{
        //    try
        //    {
        //        Condition condition = new Condition();
        //        condition.ParamName = "Enabled";
        //        condition.Operation = ConditionOperate.Equal;
        //        condition.ParamValue = "1";
        //        condition.Logic = "AND";
        //        Stopwatch watch = CommonHelper.TimerStart();
        //        List<Condition> conditions = new List<Condition>();
        //        conditions.Add(condition);
        //        List<DbParameter> parameter = new List<DbParameter>();

        //        if (!string.IsNullOrEmpty(ParameterJson))
        //        {
        //            conditions.AddRange(ParameterJson.JonsToList<Condition>());
        //        }
        //        string WhereSql = ConditionBuilder.GetWhereSql(conditions, out parameter);
        //        CarQualityOutputList = repositoryfactory.Repository().FindTablePage(WhereSql, parameter.ToArray(), ref jqgridparam);
               
        //        var JsonData = new
        //        {
        //            total = jqgridparam.total,
        //            page = jqgridparam.page,
        //            records = jqgridparam.records,
        //            costtime = CommonHelper.TimerEnd(watch),
        //            rows = CarQualityOutputList,
        //        };
        //        return Content(JsonData.ToJson());
        //    }
        //    catch (Exception ex)
        //    {
        //        Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message + "\r\n条件：" + ParameterJson);
        //        return null;
        //    }
        //}
        #endregion

        #region 下拉框
        public ActionResult getQualityCheckPointNm()
        {
            try
            {
                CarQualityOutputList = MyBll.getQualityCheckPointNm();
                return Content(CarQualityOutputList.ToJson());
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }
        }
        #endregion
        
        #region 导入-第二版

        public class ChassisNumberinfo
        {
            public string ChassisNumber { get; set; } //底盘号
            public string VIN { get; set; }           //VIN
            public string Tm { get; set; }            //录入时间
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

            //用来存储各VIN号已有数据
            List<P_ProducePlan_Pro> ProducePlanList0 = new List<P_ProducePlan_Pro>();                               //产品信息List
            List<BBdbR_PlineBase> QCPGroupList0 = new List<BBdbR_PlineBase>();                                          //质控点组信息
            List<Q_CarQualityResult_Pro> CarQualityResultList0 = new List<Q_CarQualityResult_Pro>();                //结果表List
            List<Q_CarQualityInspection_Pro> CarQualityInspectionList0 = new List<Q_CarQualityInspection_Pro>();    //过程表List
            List<Q_CarQualityOutput_Pro> CarQualityOutputList0 = new List<Q_CarQualityOutput_Pro>();                //销项表List

            //用来存储导入数据
            List<Q_CarQualityResult_Pro> CarQualityResultList = new List<Q_CarQualityResult_Pro>();         //结果表List
            List<Q_CarQualityInspection_Pro> CarQualityInspectionList = new List<Q_CarQualityInspection_Pro>(); //过程表List
            List<Q_CarQualityOutput_Pro> CarQualityOutputList = new List<Q_CarQualityOutput_Pro>(); //销项表List

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

                    #region 导入前获取所有质量质控点组信息（表中工段信息）并存入QCPGroupList0
                    StringBuilder strSqlQCPG = new StringBuilder();
                    DataTable dtCheckQCPG = new DataTable();
                    strSqlQCPG.Append(@"select a.* from  BBdbR_PlineBase a left join BBdbR_WorkSectionBase b on a.WorkSectionId = b.WorkSectionId where b.WorkSectionNm = '质量工段' and a.Enabled = 1 and b.Enabled =1 ");
                    dtCheckQCPG = DataFactory.Database().FindTableBySql(strSqlQCPG.ToString(), false);
                    foreach (DataRow item in dtCheckQCPG.Rows)
                    {
                        BBdbR_PlineBase NewQCPG = new BBdbR_PlineBase();
                        NewQCPG.PlineId = item["PlineId"].ToString();//质控点组Id
                        NewQCPG.PlineCd = item["PlineCd"].ToString();//质控点组Cd
                        NewQCPG.PlineNm = item["PlineNm"].ToString();//质控点组Nm
                        QCPGroupList0.Add(NewQCPG);
                    }
                    #endregion

                    #region 表中数据循环录入
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        IsCheck = 1;//重置标识
                        DataRow dr = Newdt.NewRow();

                        if (dt.Rows[i]["录入时间"].ToString().Trim() != "")
                        {
                            #region 判断底盘号是不是空
                            if (dt.Rows[i]["底盘号"].ToString().Trim() != "")
                            {
                                ChassisNumber = dt.Rows[i]["底盘号"].ToString().Trim();//赋值给变量

                                //检查ChassisNumberList中是否已经存在此底盘号
                                int ifInList = 0;
                                foreach (ChassisNumberinfo s in ChassisNumberList)
                                {
                                    if (s.ChassisNumber == ChassisNumber)
                                    {
                                        ifInList = ifInList + 1;
                                    }
                                    else { }
                                }

                                if (ifInList == 0)//这是一个新的底盘号
                                {
                                    string ThisVIN = "";//根据底盘号查到VIN号再用此VIN号查询三个表中数据
                                    #region 根据底盘号获取此车辆信息并存入ProducePlanList0
                                    //检查VIN是否存在
                                    StringBuilder strSqlVIN = new StringBuilder();
                                    DataTable dtCheckVIN = new DataTable();
                                    strSqlVIN.Append(@"select * from P_ProducePlan_Pro where VIN like '%" + ChassisNumber + "%'  and Enabled ='1' order by PlanTime desc");
                                    dtCheckVIN = DataFactory.Database().FindTableBySql(strSqlVIN.ToString(), false);
                                    if (dtCheckVIN.Rows.Count > 0)
                                    {

                                        ThisVIN = dtCheckVIN.Rows[0]["VIN"].ToString();
                                        //给ProducePlanList0添加一条数据
                                        P_ProducePlan_Pro prodecePlan = new P_ProducePlan_Pro();
                                        prodecePlan.VIN = dtCheckVIN.Rows[0]["VIN"].ToString();
                                        prodecePlan.CarType = dtCheckVIN.Rows[0]["CarType"].ToString();
                                        prodecePlan.MatCd = dtCheckVIN.Rows[0]["MatCd"].ToString();
                                        prodecePlan.CarColor1 = dtCheckVIN.Rows[0]["CarColor1"].ToString();
                                        prodecePlan.OrderCd = dtCheckVIN.Rows[0]["OrderCd"].ToString();
                                        ProducePlanList0.Add(prodecePlan);
                                    }
                                    else
                                    {
                                        //无法通过底盘号查询到车辆信息
                                        ThisVIN = "";
                                    }
                                    #endregion

                                    #region 根据底盘号获得的VIN号查询结果表数据并存入CarQualityResultList0
                                    if (ThisVIN != "")
                                    {
                                        StringBuilder strSqlFindTable1ByVIN = new StringBuilder();
                                        DataTable dtFindTable1ByVIN = new DataTable();
                                        strSqlFindTable1ByVIN.Append(@"select * from Q_CarQualityResult_Pro where VIN = '" + ThisVIN + "'  and Enabled ='1' ");
                                        dtFindTable1ByVIN = DataFactory.Database().FindTableBySql(strSqlFindTable1ByVIN.ToString(), false);
                                        if (dtFindTable1ByVIN.Rows.Count > 0)
                                        {
                                            foreach (DataRow item in dtFindTable1ByVIN.Rows)
                                            {
                                                Q_CarQualityResult_Pro newResult = new Q_CarQualityResult_Pro();
                                                newResult.CarQualityResultId = item["CarQualityResultId"].ToString();             //结果表主键
                                                newResult.QualityCheckPointGroupId = item["QualityCheckPointGroupId"].ToString();//结果表质控点组主键
                                                newResult.QualityCheckPointGroupCd = item["QualityCheckPointGroupCd"].ToString();//结果表质控点组编号
                                                newResult.QualityCheckPointGroupNm = item["QualityCheckPointGroupNm"].ToString();//结果表质控点组名称
                                                newResult.QualityCheckPointId = item["QualityCheckPointId"].ToString();          //结果表质控点主键
                                                newResult.QualityCheckPointCd = item["QualityCheckPointCd"].ToString();          //结果表质控点编号
                                                newResult.QualityCheckPointNm = item["QualityCheckPointNm"].ToString();          //结果表质控点名称
                                                newResult.StfCd = item["StfCd"].ToString();                                      //结果表录入人员编号
                                                newResult.StfNm = item["StfNm"].ToString();                                      //结果表录入人员姓名
                                                newResult.QualityInspectTm = item["QualityInspectTm"].ToString();                //结果表录入时间
                                                newResult.VIN = item["VIN"].ToString();                                          //结果表VIN
                                                newResult.QualityResult = item["QualityResult"].ToString();                      //结果表质检结果
                                                newResult.Enabled = item["Enabled"].ToString();                                  //结果表有效性
                                                newResult.CreCd = item["CreCd"].ToString();                                      //结果表创建人编号
                                                newResult.CreNm = item["CreNm"].ToString();                                      //结果表创建人姓名
                                                newResult.CreTm = item["CreTm"].ToString();                                      //结果表创建时间
                                                CarQualityResultList0.Add(newResult);
                                            }
                                        }
                                        else
                                        {

                                        }
                                    }
                                    else { }
                                    #endregion

                                    #region 根据底盘号获得的VIN号查询录入过程表数据并存入CarQualityInspectionList0
                                    if (ThisVIN != "")
                                    {
                                        StringBuilder strSqlFindTable2ByVIN = new StringBuilder();
                                        DataTable dtFindTable2ByVIN = new DataTable();
                                        strSqlFindTable2ByVIN.Append(@"select * from Q_CarQualityInspection_Pro where VIN = '" + ThisVIN + "'  and Enabled ='1' ");
                                        dtFindTable2ByVIN = DataFactory.Database().FindTableBySql(strSqlFindTable2ByVIN.ToString(), false);
                                        if (dtFindTable2ByVIN.Rows.Count > 0)
                                        {
                                            foreach (DataRow item in dtFindTable2ByVIN.Rows)
                                            {
                                                Q_CarQualityInspection_Pro newInspection = new Q_CarQualityInspection_Pro();
                                                newInspection.CarQualityInspectionId = item["CarQualityInspectionId"].ToString();       //过程表主键
                                                newInspection.CarQualityResultId = item["CarQualityResultId"].ToString();               //过程表结果表主键
                                                newInspection.QualityCheckPointGroupId = item["QualityCheckPointGroupId"].ToString();   //过程表质控点组主键
                                                newInspection.QualityCheckPointGroupCd = item["QualityCheckPointGroupCd"].ToString();   //过程表质控点组编号
                                                newInspection.QualityCheckPointGroupNm = item["QualityCheckPointGroupNm"].ToString();   //过程表质控点组名称
                                                newInspection.QualityCheckPointId = item["QualityCheckPointId"].ToString();             //过程表质控点主键
                                                newInspection.QualityCheckPointCd = item["QualityCheckPointCd"].ToString();             //过程表质控点编号
                                                newInspection.QualityCheckPointNm = item["QualityCheckPointNm"].ToString();             //过程表质控点名称
                                                newInspection.CarType = item["CarType"].ToString();                                     //过程表车型
                                                newInspection.MatCd = item["MatCd"].ToString();                                         //过程表车身物料号
                                                newInspection.CarColor1 = item["CarColor1"].ToString();                                 //过程表车身面漆颜色
                                                newInspection.VIN = item["VIN"].ToString();                                             //过程表VIN
                                                newInspection.OrderCd = item["OrderCd"].ToString();                                     //过程表订单号
                                                newInspection.CarComponentId = item["CarComponentId"].ToString();                       //过程表检验部件名称-车身组件主键
                                                newInspection.CarPositionId = item["CarPositionId"].ToString();                         //过程表检验岗主键-车身方位主键
                                                newInspection.CarPositionGroupId = item["CarPositionGroupId"].ToString();               //过程表检验项目主键-车身方位分组主键
                                                newInspection.CarComponentCd = item["CarComponentCd"].ToString();                       //过程表检验部件编号-车身组件最终编码
                                                newInspection.CarComponentNm = item["CarComponentNm"].ToString();                       //过程表检验部件名称-车身组件名称
                                                newInspection.CarComponentSimpleCd = item["CarComponentSimpleCd"].ToString();           //过程表检验部件简码-车身组件简码
                                                newInspection.DefectId = item["DefectId"].ToString();                                   //过程表缺陷主键
                                                newInspection.DefectCatgId = item["DefectCatgId"].ToString();                           //过程表缺陷类型主键
                                                newInspection.DefectCatgGroupId = item["DefectCatgGroupId"].ToString();                 //过程表缺陷分组主键
                                                newInspection.DefectCd = item["DefectCd"].ToString();                                   //过程表缺陷编号
                                                newInspection.DefectNm = item["DefectNm"].ToString();                                   //过程表缺陷名称
                                                newInspection.DefectAnalysis = item["DefectAnalysis"].ToString();                       //过程表缺陷分析
                                                newInspection.QualityInspectTm = item["QualityInspectTm"].ToString();                   //过程表录入时间
                                                newInspection.StfCd = item["StfCd"].ToString();                                         //过程表录入人员编号
                                                newInspection.StfNm = item["StfNm"].ToString();                                         //过程表录入人员姓名
                                                newInspection.Enabled = item["Enabled"].ToString();                                     //过程表有效性
                                                newInspection.CreCd = item["CreCd"].ToString();                                         //过程表创建人编号
                                                newInspection.CreNm = item["CreNm"].ToString();                                         //过程表创建人姓名
                                                newInspection.CreTm = item["CreTm"].ToString();                                         //过程表创建时间
                                                newInspection.Rem = item["Rem"].ToString();                                             //过程表备注
                                                CarQualityInspectionList0.Add(newInspection);
                                            }
                                        }
                                        else
                                        {

                                        }
                                    }
                                    else { }
                                    #endregion

                                    #region 根据底盘号获得的VIN号查询销项表数据并存入CarQualityOutputList0
                                    if (ThisVIN != "")
                                    {
                                        StringBuilder strSqlFindTable3ByVIN = new StringBuilder();
                                        DataTable dtFindTable3ByVIN = new DataTable();
                                        strSqlFindTable3ByVIN.Append(@"select * from Q_CarQualityOutput_Pro where VIN = '" + ThisVIN + "'  and Enabled ='1' ");
                                        dtFindTable3ByVIN = DataFactory.Database().FindTableBySql(strSqlFindTable3ByVIN.ToString(), false);
                                        if (dtFindTable3ByVIN.Rows.Count > 0)
                                        {
                                            foreach (DataRow item in dtFindTable3ByVIN.Rows)
                                            {
                                                Q_CarQualityOutput_Pro newOutput = new Q_CarQualityOutput_Pro();
                                                newOutput.CarInspectionOutputId = item["CarInspectionOutputId"].ToString();         //销项表主键
                                                newOutput.CarQualityInspectionId = item["CarQualityInspectionId"].ToString();       //销项表过程表主键
                                                newOutput.QualityCheckPointGroupId = item["QualityCheckPointGroupId"].ToString();   //销项表质控点组主键
                                                newOutput.QualityCheckPointGroupCd = item["QualityCheckPointGroupCd"].ToString();   //销项表质控点组编号
                                                newOutput.QualityCheckPointGroupNm = item["QualityCheckPointGroupNm"].ToString();   //销项表质控点组名称
                                                newOutput.QualityCheckPointId = item["QualityCheckPointId"].ToString();             //销项表质控点主键
                                                newOutput.QualityCheckPointCd = item["QualityCheckPointCd"].ToString();             //销项表质控点编号
                                                newOutput.QualityCheckPointNm = item["QualityCheckPointNm"].ToString();             //销项表质控点名称
                                                newOutput.VIN = item["VIN"].ToString();                                             //销项表VIN
                                                newOutput.CarComponentId = item["CarComponentId"].ToString();                       //销项表检验部件名称-车身组件主键
                                                newOutput.CarPositionId = item["CarPositionId"].ToString();                         //销项表检验岗主键-车身方位主键
                                                newOutput.CarPositionGroupId = item["CarPositionGroupId"].ToString();               //销项表检验项目主键-车身方位分组主键
                                                newOutput.CarComponentCd = item["CarComponentCd"].ToString();                       //销项表检验部件编号-车身组件最终编码
                                                newOutput.CarComponentNm = item["CarComponentNm"].ToString();                       //销项表检验部件名称-车身组件名称
                                                newOutput.CarComponentSimpleCd = item["CarComponentSimpleCd"].ToString();           //销项表检验部件简码-车身组件简码
                                                newOutput.DefectId = item["DefectId"].ToString();                                   //销项表缺陷主键
                                                newOutput.DefectCatgId = item["DefectCatgId"].ToString();                           //销项表缺陷类型主键
                                                newOutput.DefectCatgGroupId = item["DefectCatgGroupId"].ToString();                 //销项表缺陷分组主键
                                                newOutput.DefectCd = item["DefectCd"].ToString();                                   //销项表缺陷编号
                                                newOutput.DefectNm = item["DefectNm"].ToString();                                   //销项表缺陷名称
                                                newOutput.DefectAnalysis = item["DefectAnalysis"].ToString();                       //销项表缺陷分析
                                                newOutput.StfCd = item["StfCd"].ToString();                                         //销项表质检录入人员编号
                                                newOutput.StfNm = item["StfNm"].ToString();                                         //销项表质检录入人员姓名
                                                newOutput.QualityInspectTm = item["QualityInspectTm"].ToString();                   //销项表质检录入时间
                                                newOutput.WStfCd = item["WStfCd"].ToString();                                       //销项表维修人员编号
                                                newOutput.WStfNm = item["WStfNm"].ToString();                                       //销项表维修人员姓名
                                                newOutput.WxTm = item["WxTm"].ToString();                                           //销项表维修时间
                                                newOutput.OutputResult = item["OutputResult"].ToString();                           //销项表检查结果
                                                newOutput.XStfCd = item["XStfCd"].ToString();                                       //销项人员编号
                                                newOutput.XStfNm = item["XStfNm"].ToString();                                       //销项人员编号
                                                newOutput.OutputTime = item["OutputTime"].ToString();                               //销项时间
                                                newOutput.ReinspectionNumber = Convert.ToInt32(item["ReinspectionNumber"].ToString());//销项表复检次数
                                                newOutput.Enabled = item["Enabled"].ToString();                                     //销项表有效性
                                                newOutput.CreCd = item["CreCd"].ToString();                                         //销项表创建人编号
                                                newOutput.CreNm = item["CreNm"].ToString();                                         //销项表创建人姓名
                                                newOutput.CreTm = item["CreTm"].ToString();                                         //销项表创建时间
                                                CarQualityOutputList0.Add(newOutput);
                                            }
                                        }
                                        else
                                        {

                                        }
                                    }
                                    else { }
                                    #endregion

                                    #region 根据底盘号获得的VIN号存入ChassisNumberList
                                    if (ThisVIN != "")
                                    {
                                        ChassisNumberinfo s = new ChassisNumberinfo();
                                        s.ChassisNumber = ChassisNumber;
                                        s.VIN = ThisVIN;
                                        s.Tm = dt.Rows[i]["录入时间"].ToString().Trim();
                                        ChassisNumberList.Add(s);//如果底盘list中没有该底盘号，添加此底盘号
                                    }
                                    else { }
                                    #endregion

                                }
                                else { }

                            }
                            else
                            {
                                //底盘号为空，使用上一次底盘号
                            }
                            #endregion

                            #region 校验底盘号
                            int ifInChassisNumberList = 0;                                     //用于判断底盘号是否在ChassisNumberList中
                            string VIN = "";                                                   //VIN
                            string CarType = "";                                               //车型
                            string MatCd = "";                                                 //车身物料号
                            string CarColor1 = "";                                             //车身面漆颜色
                            string OrderCd = "";                                               //订单号
                            //找出此底盘号在ProducePlanList0中的VIN号
                            foreach (P_ProducePlan_Pro carinfo in ProducePlanList0)
                            {
                                if (carinfo.VIN.Contains(ChassisNumber))
                                {
                                    ifInChassisNumberList = 1;
                                    VIN = carinfo.VIN;
                                    CarType = carinfo.CarType;
                                    MatCd = carinfo.MatCd;
                                    CarColor1 = carinfo.CarColor1;
                                    OrderCd = carinfo.OrderCd;
                                }
                                else { }
                            }
                            if (ifInChassisNumberList == 0)
                            {
                                dr = Newdt.NewRow();
                                dr[0] = errorNum;
                                dr[1] = "第[" + dt.Rows[i]["rowid"].ToString() + "]行[底盘号]";
                                dr[2] = "未查到车辆信息";
                                Newdt.Rows.Add(dr);
                                errorNum++;
                                IsCheck = 0;
                                continue;
                            }
                            else { }

                            #endregion

                            #region 校验质控点组（工段）名称
                            int ifInQCPGList = 0;                                                   //用于判定表中质控点组是否在系统中存在
                            string QualityCheckPointNm = "导入";                                    //质控点名称
                            string QualityCheckPointId = "";                                        //质控点Id
                            string QualityCheckPointCd = "";                                        //质控点编号
                            string QualityCheckPointGroupId = "";                                   //质控点组主键
                            string QualityCheckPointGroupCd = "";                                   //质控点组编号
                            string QualityCheckPointGroupNm = dt.Rows[i]["工段"].ToString().Trim(); //质控点组名称
                            if (QualityCheckPointGroupNm != "")
                            {
                                //检查表中工段（质控点组）是否存在于QCPGroupList0中
                                foreach (BBdbR_PlineBase item in QCPGroupList0)
                                {
                                    if (QualityCheckPointGroupNm == item.PlineNm)
                                    {
                                        ifInQCPGList = 1;
                                        QualityCheckPointGroupId = item.PlineId;
                                        QualityCheckPointGroupCd = item.PlineCd;
                                    }
                                    else
                                    {

                                    }
                                }
                                if (ifInQCPGList == 0)
                                {
                                    dr = Newdt.NewRow();
                                    dr[0] = errorNum;
                                    dr[1] = "第[" + dt.Rows[i]["rowid"].ToString() + "]行[工段]";
                                    dr[2] = "无效的工段";
                                    Newdt.Rows.Add(dr);
                                    errorNum++;
                                    IsCheck = 0;
                                    continue;
                                }
                                else
                                {

                                }
                            }
                            else
                            {
                                continue;
                                //质控点组名称（表中工段名称）为空，此车车辆在各质控点组全部合格
                            }

                            #endregion

                            //缺陷是否有数据
                            if (dt.Rows[i]["问题"].ToString().Trim() != "")
                            {
                                //检查重复导入-销项表
                                //检查是否有相同车辆异常信息
                                int ifCheckOutPut = 0;//用于判断是否在销项表中有数据
                                //先判断已有数据是否重复
                                foreach (var OutputInfo in CarQualityOutputList0)
                                {
                                    if (VIN == OutputInfo.VIN && QualityCheckPointGroupNm == OutputInfo.QualityCheckPointGroupNm && dt.Rows[i]["问题"].ToString().Trim() == OutputInfo.CarComponentNm && dt.Rows[i]["备注"].ToString().Trim() == OutputInfo.DefectAnalysis)
                                    {
                                        ifCheckOutPut = 1;
                                    }
                                    else { }
                                }
                                //再判断此导入表中之前的数据有无重复
                                foreach (var OutputInfo in CarQualityOutputList)
                                {
                                    if (VIN == OutputInfo.VIN && QualityCheckPointGroupNm == OutputInfo.QualityCheckPointGroupNm && dt.Rows[i]["问题"].ToString().Trim() == OutputInfo.CarComponentNm && dt.Rows[i]["备注"].ToString().Trim() == OutputInfo.DefectAnalysis)
                                    {
                                        ifCheckOutPut = 2;
                                    }
                                    else { }
                                }

                                if (ifCheckOutPut == 1)
                                {
                                    dr = Newdt.NewRow();
                                    dr[0] = errorNum;
                                    dr[1] = "第[" + dt.Rows[i]["rowid"].ToString() + "]行[车辆异常信息]";
                                    dr[2] = "在系统中已存在不能重复插入";
                                    Newdt.Rows.Add(dr);
                                    errorNum++;
                                    IsCheck = 0;
                                    continue;
                                }
                                else if (ifCheckOutPut == 2)
                                {
                                    dr = Newdt.NewRow();
                                    dr[0] = errorNum;
                                    dr[1] = "第[" + dt.Rows[i]["rowid"].ToString() + "]行[车辆异常信息]";
                                    dr[2] = "此表中已存在此条数据，无法重复插入";
                                    Newdt.Rows.Add(dr);
                                    errorNum++;
                                    IsCheck = 0;
                                    continue;
                                }
                                else //该条缺陷在销项表中没有重复数据且此表中也无重复数据
                                {
                                    int ifCheckResult = 0;//用于判断是否在此表中和数据库有重复数据
                                    //检验结果表中是否有重复数据，如果无数据添加一条不合格数据，如果有数据，把该数据修改时间改为当前时间，合格状态改为不合格并获取主键
                                    string CarQualityResultId = "";//结果表主键
                                    //先检验数据库中数据
                                    foreach (Q_CarQualityResult_Pro ResultInfo in CarQualityResultList0)
                                    {
                                        if (VIN == ResultInfo.VIN && QualityCheckPointGroupNm == ResultInfo.QualityCheckPointGroupNm && QualityCheckPointNm == "导入")
                                        {
                                            ifCheckResult = 1;
                                            CarQualityResultId = ResultInfo.CarQualityResultId;
                                        }
                                        else { }
                                    }
                                    //再检验已添加数据
                                    foreach (Q_CarQualityResult_Pro ResultInfo in CarQualityResultList)
                                    {
                                        if (VIN == ResultInfo.VIN && QualityCheckPointGroupNm == ResultInfo.QualityCheckPointGroupNm && QualityCheckPointNm == "导入")
                                        {
                                            ifCheckResult = 1;
                                            CarQualityResultId = ResultInfo.CarQualityResultId;
                                        }
                                        else { }
                                    }

                                    if (ifCheckResult == 0)//无数据
                                    {
                                        #region 插入结果表数据
                                        Q_CarQualityResult_Pro CarQualityResult = new Q_CarQualityResult_Pro();
                                        CarQualityResult.CarQualityResultId = System.Guid.NewGuid().ToString();  //结果表主键
                                        CarQualityResultId = CarQualityResult.CarQualityResultId;               //自动生成主键并赋值
                                        CarQualityResult.QualityCheckPointGroupId = QualityCheckPointGroupId;   //结果表质控点组主键
                                        CarQualityResult.QualityCheckPointGroupCd = QualityCheckPointGroupCd;//结果表质控点组编号
                                        CarQualityResult.QualityCheckPointGroupNm = QualityCheckPointGroupNm;//结果表质控点组名称
                                        CarQualityResult.QualityCheckPointId = QualityCheckPointId;             //结果表质控点主键
                                        CarQualityResult.QualityCheckPointCd = QualityCheckPointCd;             //结果表质控点编号
                                        CarQualityResult.QualityCheckPointNm = QualityCheckPointNm;             //结果表质控点名称
                                        CarQualityResult.StfCd = dt.Rows[i]["录入人员工号"].ToString().Trim();  //结果表录入人员编号
                                        CarQualityResult.StfNm = dt.Rows[i]["录入人员姓名"].ToString().Trim();  //结果表录入人员姓名
                                        CarQualityResult.QualityInspectTm = dt.Rows[i]["录入时间"].ToString().Trim();//结果表录入时间
                                        CarQualityResult.VIN = VIN;                                             //结果表VIN
                                        CarQualityResult.QualityResult = "不合格";                              //结果表质检结果
                                        CarQualityResult.isFile = 0;                                            //结果表是否转档
                                        CarQualityResult.Enabled = "1";                                         //结果表有效性
                                        CarQualityResult.CreCd = ManageProvider.Provider.Current().UserId;      //结果表创建人编号
                                        CarQualityResult.CreNm = ManageProvider.Provider.Current().UserName;    //结果表创建人姓名
                                        CarQualityResult.CreTm = DateTime.Now.ToString();                       //结果表创建时间
                                        CarQualityResultList.Add(CarQualityResult);
                                        #endregion
                                    }
                                    else
                                    {
                                        //已有数据，一定是不合格（除非第一次导入时是合格，又导入一次，两次数据不一致），不需要对其修改,foreach时已拿到其主键并赋值
                                    }

                                    #region 插入质检过程表数据
                                    Q_CarQualityInspection_Pro CarQualityInspection = new Q_CarQualityInspection_Pro();
                                    CarQualityInspection.CarQualityInspectionId = System.Guid.NewGuid().ToString();  //过程表主键
                                    CarQualityInspection.CarQualityResultId = CarQualityResultId;                    //过程表结果表主键
                                    CarQualityInspection.QualityCheckPointGroupId = QualityCheckPointGroupId;   //过程表质控点组主键
                                    CarQualityInspection.QualityCheckPointGroupCd = QualityCheckPointGroupCd;   //过程表质控点组编号
                                    CarQualityInspection.QualityCheckPointGroupNm = QualityCheckPointGroupNm;   //过程表质控点组名称
                                    CarQualityInspection.QualityCheckPointId = QualityCheckPointId;             //过程表质控点主键
                                    CarQualityInspection.QualityCheckPointCd = QualityCheckPointCd;             //过程表质控点编号
                                    CarQualityInspection.QualityCheckPointNm = QualityCheckPointNm;             //过程表质控点名称
                                    CarQualityInspection.CarType = CarType;                                     //过程表车型
                                    CarQualityInspection.MatCd = MatCd;                                         //过程表车身物料号
                                    CarQualityInspection.CarColor1 = CarColor1;                                 //过程表车身面漆颜色
                                    CarQualityInspection.VIN = VIN;                                             //过程表VIN
                                    CarQualityInspection.OrderCd = OrderCd;                                     //过程表订单号
                                    CarQualityInspection.CarComponentId = "";                                   //过程表检验部件名称-车身组件主键
                                    CarQualityInspection.CarPositionId = "";                                    //过程表检验岗主键-车身方位主键
                                    CarQualityInspection.CarPositionGroupId = "";                               //过程表检验项目主键-车身方位分组主键
                                    CarQualityInspection.CarComponentCd = "";                                   //过程表检验部件编号-车身组件最终编码
                                    CarQualityInspection.CarComponentNm = dt.Rows[i]["问题"].ToString().Trim(); //过程表检验部件名称-车身组件名称
                                    CarQualityInspection.CarComponentSimpleCd = "";                             //过程表检验部件简码-车身组件简码
                                    CarQualityInspection.DefectId = "";                                         //过程表缺陷主键
                                    CarQualityInspection.DefectCatgId = "";                                     //过程表缺陷类型主键
                                    CarQualityInspection.DefectCatgGroupId = "";                                //过程表缺陷分组主键
                                    CarQualityInspection.DefectCd = "";                                         //过程表缺陷编号
                                    CarQualityInspection.DefectNm = "";                                         //过程表缺陷名称
                                    CarQualityInspection.DefectAnalysis = dt.Rows[i]["备注"].ToString().Trim(); //过程表缺陷分析
                                    CarQualityInspection.QualityInspectTm = dt.Rows[i]["录入时间"].ToString().Trim();//过程表录入时间
                                    CarQualityInspection.StfCd = dt.Rows[i]["录入人员工号"].ToString().Trim();  //过程表录入人员编号
                                    CarQualityInspection.StfNm = dt.Rows[i]["录入人员姓名"].ToString().Trim();  //过程表录入人员姓名
                                    CarQualityInspection.isFile = 0;                                            //过程表是否转档
                                    CarQualityInspection.Enabled = "1";                                         //过程表有效性
                                    CarQualityInspection.CreCd = ManageProvider.Provider.Current().UserId;      //过程表创建人编号
                                    CarQualityInspection.CreNm = ManageProvider.Provider.Current().UserName;    //过程表创建人姓名
                                    CarQualityInspection.CreTm = DateTime.Now.ToString();                       //过程表创建时间
                                    CarQualityInspection.Rem = dt.Rows[i]["备注"].ToString().Trim();            //过程表备注
                                    CarQualityInspectionList.Add(CarQualityInspection);
                                    #endregion

                                    #region 插入质检销项表数据
                                    Q_CarQualityOutput_Pro CarQualityOutput = new Q_CarQualityOutput_Pro();
                                    CarQualityOutput.CarInspectionOutputId = System.Guid.NewGuid().ToString();  //销项表主键
                                    CarQualityOutput.CarQualityInspectionId = CarQualityInspection.CarQualityInspectionId;//销项表过程表主键
                                    CarQualityOutput.QualityCheckPointGroupId = QualityCheckPointGroupId;   //销项表质控点组主键
                                    CarQualityOutput.QualityCheckPointGroupCd = QualityCheckPointGroupCd;   //销项表质控点组编号
                                    CarQualityOutput.QualityCheckPointGroupNm = QualityCheckPointGroupNm;   //销项表质控点组名称
                                    CarQualityOutput.QualityCheckPointId = QualityCheckPointId;             //销项表质控点主键
                                    CarQualityOutput.QualityCheckPointCd = QualityCheckPointCd;             //销项表质控点编号
                                    CarQualityOutput.QualityCheckPointNm = QualityCheckPointNm;             //销项表质控点名称
                                    CarQualityOutput.VIN = VIN;                                             //销项表VIN
                                    CarQualityOutput.CarComponentId = "";                                   //销项表检验部件名称-车身组件主键
                                    CarQualityOutput.CarPositionId = "";                                    //销项表检验岗主键-车身方位主键
                                    CarQualityOutput.CarPositionGroupId = "";                               //销项表检验项目主键-车身方位分组主键
                                    CarQualityOutput.CarComponentCd = "";                                   //销项表检验部件编号-车身组件最终编码
                                    CarQualityOutput.CarComponentNm = dt.Rows[i]["问题"].ToString().Trim(); //销项表检验部件名称-车身组件名称
                                    CarQualityOutput.CarComponentSimpleCd = "";                             //销项表检验部件简码-车身组件简码
                                    CarQualityOutput.DefectId = "";                                         //销项表缺陷主键
                                    CarQualityOutput.DefectCatgId = "";                                     //销项表缺陷类型主键
                                    CarQualityOutput.DefectCatgGroupId = "";                                //销项表缺陷分组主键
                                    CarQualityOutput.DefectCd = "";                                         //销项表缺陷编号
                                    CarQualityOutput.DefectNm = "";                                         //销项表缺陷名称
                                    CarQualityOutput.DefectAnalysis = dt.Rows[i]["备注"].ToString().Trim(); //销项表缺陷分析
                                    CarQualityOutput.StfCd = dt.Rows[i]["录入人员工号"].ToString().Trim();  //销项表质检录入人员编号
                                    CarQualityOutput.StfNm = dt.Rows[i]["录入人员姓名"].ToString().Trim();  //销项表质检录入人员姓名
                                    CarQualityOutput.QualityInspectTm = dt.Rows[i]["录入时间"].ToString().Trim();//销项表质检录入时间
                                    CarQualityOutput.WStfCd = dt.Rows[i]["维修人员工号"].ToString().Trim(); //销项表维修人员编号
                                    CarQualityOutput.WStfNm = dt.Rows[i]["维修人员"].ToString().Trim();     //销项表维修人员姓名
                                    CarQualityOutput.WxTm = dt.Rows[i]["维修时间"].ToString().Trim();       //销项表维修时间
                                    CarQualityOutput.OutputResult = "已检查";                               //销项表检查结果
                                    CarQualityOutput.XStfCd = dt.Rows[i]["复检人员工号"].ToString().Trim(); //销项人员编号
                                    CarQualityOutput.XStfNm = dt.Rows[i]["复检人员"].ToString().Trim();     //销项人员编号
                                    CarQualityOutput.OutputTime = dt.Rows[i]["复检时间"].ToString().Trim(); //销项时间
                                    CarQualityOutput.ReinspectionNumber = 0;                                //销项表复检次数
                                    CarQualityOutput.isFile = 0;                                            //销项表是否转档
                                    CarQualityOutput.Enabled = "1";                                         //销项表有效性
                                    CarQualityOutput.CreCd = ManageProvider.Provider.Current().UserId;      //销项表创建人编号
                                    CarQualityOutput.CreNm = ManageProvider.Provider.Current().UserName;    //销项表创建人姓名
                                    CarQualityOutput.CreTm = DateTime.Now.ToString();                       //销项表创建时间
                                                                                                            //CarQualityOutput.Rem = dt.Rows[i]["备注"].ToString().Trim();            //销项表备注

                                    if (CarQualityOutput.WxTm == "" || CarQualityOutput.WxTm == null)
                                    {
                                        CarQualityOutput.WxTm = CarQualityOutput.QualityInspectTm;  //如果导入表中没有时间，导入时间变为录入时间
                                    }
                                    else
                                    {

                                    }
                                    if (CarQualityOutput.OutputTime == "" || CarQualityOutput.OutputTime == null)
                                    {
                                        CarQualityOutput.OutputTime = CarQualityOutput.QualityInspectTm;  //如果导入表中没有时间，导入时间变为录入时间
                                    }
                                    else
                                    {

                                    }

                                    CarQualityOutputList.Add(CarQualityOutput);
                                    #endregion

                                }
                            }
                            else
                            {
                                dr = Newdt.NewRow();
                                dr[0] = errorNum;
                                dr[1] = "第[" + dt.Rows[i]["rowid"].ToString() + "]行[问题]";
                                dr[2] = "该工段问题不能为空";
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
                            dr[2] = "录入时间不能为空";
                            Newdt.Rows.Add(dr);
                            errorNum++;
                            IsCheck = 0;
                            continue;
                        }
                    }
                    #endregion

                    #region 表中数据省略的合格数据导入

                    foreach (ChassisNumberinfo everyChassisinfo in ChassisNumberList)
                    {
                        string VIN = everyChassisinfo.VIN;//获取此表中的各个VIN号

                        //循环已有质控点组，检查此VIN号在各质控点组是否有数据，没有数据加一条合格数据
                        foreach (var everyQCPG in QCPGroupList0)
                        {
                            //循环两个结果表list，查找重复记录
                            int ifCheckResult = 0;
                            //先查数据库
                            foreach (var ResultInfo in CarQualityResultList0)
                            {
                                if (VIN == ResultInfo.VIN && everyQCPG.PlineNm == ResultInfo.QualityCheckPointGroupNm)
                                {
                                    ifCheckResult = 1;
                                }
                                else { }
                            }
                            //再查此表已添加数据
                            foreach (var ResultInfo in CarQualityResultList)
                            {
                                if (VIN == ResultInfo.VIN && everyQCPG.PlineNm == ResultInfo.QualityCheckPointGroupNm)
                                {
                                    ifCheckResult = 1;
                                }
                                else { }
                            }
                            if (ifCheckResult == 0)
                            {
                                #region 结果表插入合格数据
                                Q_CarQualityResult_Pro CarQualityResult = new Q_CarQualityResult_Pro();
                                CarQualityResult.CarQualityResultId = System.Guid.NewGuid().ToString(); //结果表主键
                                CarQualityResult.QualityCheckPointGroupId = everyQCPG.PlineId;          //结果表质控点组主键
                                CarQualityResult.QualityCheckPointGroupCd = everyQCPG.PlineCd;          //结果表质控点组编号
                                CarQualityResult.QualityCheckPointGroupNm = everyQCPG.PlineNm;          //结果表质控点组名称
                                CarQualityResult.QualityCheckPointId = "";                              //结果表质控点主键
                                CarQualityResult.QualityCheckPointCd = "";                              //结果表质控点编号
                                CarQualityResult.QualityCheckPointNm = "导入";                          //结果表质控点名称
                                CarQualityResult.StfCd = "";                                            //结果表录入人员编号
                                CarQualityResult.StfNm = "";                                            //结果表录入人员姓名
                                CarQualityResult.QualityInspectTm = everyChassisinfo.Tm;                //结果表录入时间
                                CarQualityResult.VIN = VIN;                                             //结果表VIN
                                CarQualityResult.QualityResult = "合格";                                //结果表质检结果
                                CarQualityResult.isFile = 0;                                            //结果表是否转档
                                CarQualityResult.Enabled = "1";                                         //结果表有效性
                                CarQualityResult.CreCd = ManageProvider.Provider.Current().UserId;      //结果表创建人编号
                                CarQualityResult.CreNm = ManageProvider.Provider.Current().UserName;    //结果表创建人姓名
                                CarQualityResult.CreTm = DateTime.Now.ToString();                       //结果表创建时间
                                CarQualityResultList.Add(CarQualityResult);
                                #endregion
                            }
                            else { }
                        }
                    }
                    #endregion

                    #region 向三个表中插入数据
                    //结果表
                    int b0 = database.Insert(CarQualityResultList);
                    if (b0 > 0)
                    {
                        IsOk = IsOk + b0;
                        CarQualityResultList.Clear();
                        CarQualityResultList0.Clear();
                    }
                    else
                    {

                    }

                    //质检过程表
                    int b1 = database.Insert(CarQualityInspectionList);
                    if (b1 > 0)
                    {
                        IsOk = IsOk + b1;
                        CarQualityInspectionList.Clear();
                        CarQualityInspectionList0.Clear();
                    }
                    else
                    {

                    }

                    //销项表
                    int b2 = database.Insert(CarQualityOutputList);
                    if (b2 > 0)
                    {
                        IsOk = IsOk + b2;
                        CarQualityOutputList.Clear();
                        CarQualityOutputList0.Clear();
                        ProducePlanList0.Clear();
                        QCPGroupList0.Clear();
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

        #region 删除结果表数据

        public ActionResult DeleteResult(string KeyValue)
        {
            //不管是多个主键还是单个主键，将主键拆分出来，放在数组中
            string[] array = KeyValue.Split(',');
            try
            {
                var Message = "删除失败。";//定义返回信息，该信息将返回到界面上，给用户观看
                int IsOk = 0;//判断删除方法是否成，0表示不成功，大于0表示成功  

                for (int i = 0; i < array.Length; i++) 
                {
                    //Q_HTCarQualityResult_Pro Oldentity = repository_Q_HTCarQualityResult_Pro.Repository().FindEntity(array[i]);
                    //删除此条结果记录下的所有Q_CarQualityInspection记录
                    int num1 = MyBll2.deleteByResult(array[i]);
                    //删除此条结果记录下的所有Q_CarQualityOutput_Pro记录
                    int num2 = MyBll.deleteByResult(array[i]);
                    //删除此条结果记录
                    IsOk = MyResultBll.Delete(array[i]);
                    if (IsOk > 0) Message = "删除成功。";
                }
                WriteLog(IsOk, array, Message);//记录日志
                return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = Message + "同时删除缺陷记录" }.ToString());
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
                   

                    DataTable dt = MyBll.GetInfoByOutputId(array[i]);
                    if (dt.Rows.Count > 0)
                    {
                        int ReinspectionNumber = Convert.ToInt32(dt.Rows[0]["ReinspectionNumber"].ToString());
                        if (ReinspectionNumber == 0)//如果删除的是复检次数为0的数据
                        {
                            //删除Q_CarQualityInspection_Pro表中数据
                            int num1 = MyBll2.delete(dt.Rows[0]["CarQualityInspectionId"].ToString());
                            //删除Q_CarQualityOutput_Pro表中数据
                            int num2 = MyBll.deleteByInspectionId(dt.Rows[0]["CarQualityInspectionId"].ToString());
                        }
                        else
                        {
                            int num = MyBll.deleteByInspectionId_ReinspectionNumber(dt.Rows[0]["CarQualityInspectionId"].ToString(), ReinspectionNumber);
                        }
                    }
                    else { }

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

        #region 根据form中提供的时间重新计算DPU
        public  ActionResult SubmitForm1(string date)//===复制时需要修改===
        {
            try
            {
                int IsOk = 0;//用于判断
                string Message = "更新成功";
                //删除指定日期的DPU
                StringBuilder deleteSql = new StringBuilder();
                deleteSql.Append($@"delete from Q_DPUResult where TimeType = '日' and datediff(day, StartTime,'{date}') = 0");
                int deleteResult = DataFactory.Database().ExecuteBySql(deleteSql);

                //更新DPU
                StringBuilder strSql = new StringBuilder();
                strSql.Append ($@"insert into Q_DPUResult (Id,TimeType,Cartype,StartTime,EndTime,ZZDPUResult,ZZQualityNumber,ZZCarNumber,AZDPUResult,AZQualityNumber,AZCarNumber,
                    DPDPUResult,DPQualityNumber,DPCarNumber,LYDPUResult,LYQualityNumber,LYCarNumber,
                    BZDPUResult,BZQualityNumber,BZCarNumber,CZDPUResult,CZQualityNumber,CZCarNumber,CheckDPUResult)
                    select newid(),'日',CarType,'{date}', '{date}',
                    IIF(总装problem is null and 总装vin is not null,0,1.0*总装problem/总装vin),总装problem,总装vin,
                    IIF(A章problem is null and A章vin is not null,0,1.0*A章problem/A章vin),A章problem,A章vin,
                    IIF(底盘problem is null and 底盘vin is not null,0,1.0*底盘problem/底盘vin),底盘problem,底盘vin,
                    IIF(淋雨problem is null and 淋雨vin is not null,0,1.0*淋雨problem/淋雨vin),淋雨problem,淋雨vin,
                    IIF(B章problem is null and B章vin is not null,0,1.0*B章problem/B章vin),B章problem,B章vin,
                    IIF(C章problem is null and C章vin is not null,0,1.0*C章problem/C章vin),C章problem,C章vin ,
                    IIF(A章vin is null or A章problem is null,0,1.0*A章problem/A章vin)+
                    IIF(B章problem is null or B章vin is null,0,1.0*B章problem/B章vin)+
                    IIF(C章problem is null or C章vin is null,0,1.0*C章problem/C章vin)+
                    IIF(底盘problem is null or 底盘vin is null,0,1.0*底盘problem/底盘vin)+
                    IIF(淋雨problem is null or 淋雨vin is null,0,1.0*淋雨problem/淋雨vin)
                    from (select * from 
                    (SELECT  PlineNm + 'problem' PlineNm, IIF(CarType='M3L','M3',IIF(CarType='M5','L5',CarType)) CarType, COUNT(problem) AS Num 
                    FROM dbo.FR_Quality('{date}', '{date}')
                    WHERE (problem IS NOT NULL) GROUP BY PlineNm, IIF(CarType='M3L','M3',IIF(CarType='M5','L5',CarType))
                    UNION ALL
                    SELECT  PlineNm + 'vin' PlineNm, IIF(CarType='M3L','M3',IIF(CarType='M5','L5',CarType)) CarType, COUNT(DISTINCT vin) AS Num
                    FROM dbo.FR_Quality('{date}','{date}')
                    GROUP BY PlineNm, IIF(CarType='M3L','M3',IIF(CarType='M5','L5',CarType))) T ) C
                    PIVOT(sum(Num) FOR plineNm IN(总装problem,A章problem,B章problem,C章problem,底盘problem,淋雨problem,总装vin,A章vin,B章vin,C章vin,底盘vin,淋雨vin)) AS T ");
                int insertResult = DataFactory.Database().ExecuteBySql(strSql);
                if (insertResult != 0)
                {
                    IsOk = 1;
                    Message = "更新成功";
                    Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other, IsOk.ToString(), Message);
                    return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = Message }.ToString());
                }
                else
                {
                    IsOk = 1;
                    Message = "更新成功";
                    Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other, IsOk.ToString(), Message);
                    return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = Message }.ToString());
                }
                
                
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other, "-1", "更新" + date + "DPU操作失败：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion



    }
}