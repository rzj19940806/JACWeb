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

namespace HfutIE.WebApp.Areas.EquipmentManaModule.Controllers
{
    /// <summary>
    /// E_EOLCheckResult控制器
    /// </summary>
    public class E_EOLCheckResultController : PublicController<E_EOLCheckResult>
    {
        #region 全局变量定义区
        //定义本页面主要操作的表的表名，称为主表
        //string tableName = "P_FitEnCodeInfo";
        //public static DataTable EnCodeList = new DataTable();
        #endregion

        #region 创建数据库操作对象区域
        E_EOLCheckResultBll eolcheckresultbll = new E_EOLCheckResultBll();
        E_EOLDetailResultBll eoldetailresultbll = new E_EOLDetailResultBll();
        ProducePlanBll ProducePlanBll = new ProducePlanBll();
        #endregion

        #region 方法区    

        #region 计算日期与今天日期的天数
        public string DateDiff(string DateTime0)
        {
            try
            {
                DateTime DateTime1 = Convert.ToDateTime(DateTime0);
                DateTime today = DateTime.Now;//获取当前时间
                TimeSpan ts1 = new TimeSpan(DateTime1.Ticks);
                TimeSpan ts2 = new TimeSpan(today.Ticks);
                TimeSpan ts = ts2.Subtract(ts1);
                string dateDiff = ts.Days.ToString() + "天" + ts.Hours.ToString() + "小时" + ts.Minutes.ToString() + "分钟" + ts.Seconds.ToString() + "秒";
                string day = ts.Days.ToString(); //获取datatime类型数据距离当前时间的天数
                return day;
            }
            catch (Exception)
            {
                return null;
            }

        }
        #endregion

        
        #region EOL区域方法

        #region 1.加载EOL上表格数据-查找E_EOLCheckResult表中当天检测数据
        public ActionResult GetEOLCheckResultListJson(JqGridParam jqgridparam)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                DataTable dt = new DataTable();
                strSql.Append(@"select * from E_EOLCheckResult A where exists (select 1 from E_EOLCheckResult B where A.VIN=B.VIN group by vin having  A.TestNum=MAX(B.TestNum)) and DateDiff(dd,Time,getdate())=0 order by Time");
                dt = DataFactory.Database().FindTableBySql(strSql.ToString(), false);
                var JsonData = new
                {
                    rows = dt,
                };
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion

        #region 2.根据搜索条件加载EOL上表格数据-E_EOLCheckResult
        public ActionResult GetEOLCheckResultByCondition( string CarType,string VIN,string Cd,string StartTime,string EndTime, JqGridParam jqgridparam)
        {
            #region 原版本
            //StartTime = StartTime + " 00:00:00";
            //EndTime = EndTime + " 23:59:59";
            //try
            //{
            //    StringBuilder strSql = new StringBuilder();
            //    DataTable dt = new DataTable();
            //    if (Condition == "CarType")
            //    {
            //        if (CarType=="all")
            //        {
            //            strSql.Append(@"select * from E_EOLCheckResult A where exists (select 1 from E_EOLCheckResult B where A.VIN=B.VIN group by vin having  A.TestNum=MAX(B.TestNum))  and Time between '" + StartTime + "' and '" + EndTime + "' order by Time;");
            //            dt = DataFactory.Database().FindTableBySql(strSql.ToString(), false);
            //        }
            //        else
            //        {
            //            strSql.Append(@"select * from E_EOLCheckResult A where exists (select 1 from E_EOLCheckResult B where A.VIN=B.VIN group by vin having  A.TestNum=MAX(B.TestNum))  and CarName like '%" + CarType + "%' and Time between '" + StartTime + "' and '" + EndTime + "' order by Time ;");
            //            dt = DataFactory.Database().FindTableBySql(strSql.ToString(), false);
            //        }
            //    }
            //    else if (Condition == "VIN")
            //    {
            //        strSql.Append(@"select * from E_EOLCheckResult A where exists (select 1 from E_EOLCheckResult B where A.VIN=B.VIN group by vin having  A.TestNum=MAX(B.TestNum))  and VIN like '%" + VIN + "' and Time between '" + StartTime + "' and '" + EndTime + "' order by Time;");
            //        dt = DataFactory.Database().FindTableBySql(strSql.ToString(), false);
            //    }
            //    else
            //    {
            //        strSql.Append(@"select * from E_EOLCheckResult A where exists (select 1 from E_EOLCheckResult B where A.VIN=B.VIN group by vin having  A.TestNum=MAX(B.TestNum))  and Time between '" + StartTime + "' and '" + EndTime + "' order by Time;");
            //        dt = DataFactory.Database().FindTableBySql(strSql.ToString(), false);
            //    }
            //    var JsonData = new
            //    {
            //        rows = dt,
            //    };
            //    return Content(JsonData.ToJson());
            //}
            //catch (Exception ex)
            //{
            //    return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            //}
            #endregion

            #region 查询修改
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                StringBuilder strSql = new StringBuilder();
                DataTable dt = new DataTable();

                //初始语句未加搜索条件
                strSql.Append(@"select A.*,C.MatCd,C.CarType as CarType2 from E_EOLCheckResult A left join P_ProducePlan_Pro C ON A.VIN = C.VIN where exists (select 1 from E_EOLCheckResult B where A.VIN=B.VIN group by vin having A.TestNum=MAX(B.TestNum))  ");

                List<DbParameter> parameter = new List<DbParameter>();
                //模糊搜索车型
                if (CarType!=""&& CarType!=null)
                {
                    //strSql.Append(" and C.CarType like '%" + CarType + "%' ");
                    strSql.Append(" and C.CarType like @CarType ");
                    parameter.Add(DbFactory.CreateDbParameter("@CarType", "%" + CarType + "%"));
                }

                //模糊搜索VIN号
                if (VIN!=""&& VIN!=null)
                {
                    //strSql.Append(" and VIN like '%" + VIN + "%' ");
                    strSql.Append(" and A.VIN like @VIN ");
                    parameter.Add(DbFactory.CreateDbParameter("@VIN", "%" + VIN + "%"));
                }

                //模糊搜索车型编码
                if (Cd != "" && Cd != null)
                {
                    //strSql.Append(" and VIN like '%" + VIN + "%' ");
                    strSql.Append(" and C.MatCd like @Cd ");
                    parameter.Add(DbFactory.CreateDbParameter("@Cd", "%" + Cd + "%"));
                }

                //开始时间
                if (StartTime!=null && StartTime!="")
                {
                    //string StartTime2 = DateDiff(StartTime);    //转换为到今天的天数以减少查询时间
                    //strSql.Append(@" and DateDiff(dd,Time,getdate()) <= '" + StartTime2 + "' ");
                    //开始时间把@放在前面
                    strSql.Append(" and DateDiff(dd,@StartTime,Time) >=0 ");
                    parameter.Add(DbFactory.CreateDbParameter("@StartTime", StartTime));
                }

                //结束时间
                if (EndTime != null && EndTime!="")
                {
                    //string EndTime2 = DateDiff(EndTime);    //转换为到今天的天数以减少查询时间
                    //strSql.Append(@" and DateDiff(dd,Time,getdate()) >= '" + EndTime2 + "' ");
                    //结束时间把@放在后面
                    strSql.Append(" and DateDiff(dd,Time,@EndTime) >=0 ");
                    parameter.Add(DbFactory.CreateDbParameter("@EndTime", EndTime));
                }

                //按照时间排序
                strSql.Append(" order by Time desc");

                dt = DataFactory.Database().FindTableBySql(strSql.ToString(), parameter.ToArray(), false);
                var JsonData = new
                {
                    jqgridparam.total,
                    jqgridparam.page,
                    jqgridparam.records,
                    costtime = CommonHelper.TimerEnd(watch),
                    rows = dt,
                };
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "1", "EOL检测信息查询成功");
                return Content(JsonData.ToJson());

            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "EOL检测信息查询发生异常错误：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
            


            #endregion




        }
        #endregion

        #region 3.点击EOL上表格一行，根据VIN号自动加载EOL下表格展示E_EOLDetailResult数据
        public ActionResult GetEOLDetailResult( string VIN, JqGridParam jqgridparam)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                DataTable dt = new DataTable();
                strSql.Append(@"select * from E_EOLDetailResult  where VIN=@VIN");

                List<DbParameter> parameter = new List<DbParameter>();
                parameter.Add(DbFactory.CreateDbParameter("@VIN", VIN));
                dt = DataFactory.Database().FindTableBySql(strSql.ToString(), parameter.ToArray(), false);
                var JsonData = new
                {
                    rows = dt,
                };
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion

        #endregion



        #region 检测线区域方法

        #region 根据搜索条件加载检测线上表格数据-计划表车身信息+8个表的检测结果
        public ActionResult GetTestLineResultByCondition( string CarType, string VIN,string Cd, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            Stopwatch watch = CommonHelper.TimerStart();
            //开始时间
            if (StartTime==""|| StartTime==null)
            {
                StartTime = "2021-01-01";
            }

            //结束时间
            if (EndTime == "" || EndTime == null)
            {
                EndTime = DateTime.Now.ToString("yyyy-MM-dd");
            }
            
            try
            {
                StringBuilder strSql = new StringBuilder();
                DataTable dt = new DataTable();
                //开始时间，结束时间，车型，VIN号-（模糊查询）
                strSql.Append($"select * from TestLineResult ('{StartTime}','{EndTime}','{CarType}','{VIN}','{Cd}')");
                dt = DataFactory.Database().FindTableBySql(strSql.ToString(), false);

                var JsonData = new
                {
                    jqgridparam.total,
                    jqgridparam.page,
                    jqgridparam.records,
                    costtime = CommonHelper.TimerEnd(watch),
                    rows = dt,
                };
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "1", "检测线检测信息查询成功");
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "检测线检测信息查询发生异常错误：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion

        #region 1.点击检测线上表格一行，根据VIN号自动加载检测线下表格展示E_CSpeedResult速度表数据
        public ActionResult GetGridTestLineResultSpeed(string VIN, JqGridParam jqgridparam)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                DataTable dt = new DataTable();
                strSql.Append(@"select * from E_CSpeedResult  where VinCode = @VIN");
                List<DbParameter> parameter = new List<DbParameter>();
                parameter.Add(DbFactory.CreateDbParameter("@VIN", VIN));
                dt = DataFactory.Database().FindTableBySql(strSql.ToString(), parameter.ToArray(), false);
                //dt = DataFactory.Database().FindTableBySql(strSql.ToString(), false);
                var JsonData = new
                {
                    rows = dt,
                };
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion

        #region 2.点击检测线上表格一行，根据VIN号自动加载检测线下表格展示E_CSlipResult侧滑表数据
        public ActionResult GetGridTestLineResultSlip(string VIN, JqGridParam jqgridparam)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                DataTable dt = new DataTable();
                strSql.Append(@"select * from E_CSlipResult  where VinCode =@VIN");
                List<DbParameter> parameter = new List<DbParameter>();
                parameter.Add(DbFactory.CreateDbParameter("@VIN", VIN));
                dt = DataFactory.Database().FindTableBySql(strSql.ToString(), parameter.ToArray(), false);
                var JsonData = new
                {
                    rows = dt,
                };
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion

        #region 3.点击检测线上表格一行，根据VIN号自动加载检测线下表格展示E_CHornResult喇叭声级表数据
        public ActionResult GetGridTestLineResultHorn(string VIN, JqGridParam jqgridparam)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                DataTable dt = new DataTable();
                strSql.Append(@"select * from E_CHornResult  where VinCode = @VIN");
                List<DbParameter> parameter = new List<DbParameter>();
                parameter.Add(DbFactory.CreateDbParameter("@VIN", VIN));
                dt = DataFactory.Database().FindTableBySql(strSql.ToString(), parameter.ToArray(), false);
                var JsonData = new
                {
                    rows = dt,
                };
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion

        #region 4.点击检测线上表格一行，根据VIN号自动加载检测线下表格展示E_CHeadResult灯光检测表数据
        public ActionResult GetGridTestLineResultHead(string VIN, JqGridParam jqgridparam)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                DataTable dt = new DataTable();
                strSql.Append(@"select * from E_CHeadResult  where VinCode = @VIN");
                List<DbParameter> parameter = new List<DbParameter>();
                parameter.Add(DbFactory.CreateDbParameter("@VIN", VIN));
                dt = DataFactory.Database().FindTableBySql(strSql.ToString(), parameter.ToArray(), false);
                var JsonData = new
                {
                    rows = dt,
                };
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion

        #region 5.点击检测线上表格一行，根据VIN号自动加载检测线下表格展示E_CBrakeResult制动检测表数据
        public ActionResult GetGridTestLineResultBrake(string VIN, JqGridParam jqgridparam)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                DataTable dt = new DataTable();
                strSql.Append(@"select *,F_L_MaxSumForce+F_R_MaxSumForce F_MaxSumForce,R_L_MaxSumForce+R_R_MaxSumForce R_MaxSumForce,F_L_MaxSumForce+F_R_MaxSumForce+R_L_MaxSumForce+R_R_MaxSumForce MaxSumForce,P_L_MaxSumForce+P_R_MaxSumForce P_MaxSumForce from E_CBrakeResult where VinCode =@VIN");
                List<DbParameter> parameter = new List<DbParameter>();
                parameter.Add(DbFactory.CreateDbParameter("@VIN", VIN));
                dt = DataFactory.Database().FindTableBySql(strSql.ToString(), parameter.ToArray(), false);
                var JsonData = new
                {
                    rows = dt,
                };
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion

        #region 6.点击检测线上表格一行，根据VIN号自动加载检测线下表格展示E_CAngleResult转角检测表数据
        public ActionResult GetGridTestLineResultAngle(string VIN, JqGridParam jqgridparam)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                DataTable dt = new DataTable();
                strSql.Append(@"select * from E_CAngleResult  where VinCode =@VIN");
                List<DbParameter> parameter = new List<DbParameter>();
                parameter.Add(DbFactory.CreateDbParameter("@VIN", VIN));
                dt = DataFactory.Database().FindTableBySql(strSql.ToString(), parameter.ToArray(), false);
                var JsonData = new
                {
                    rows = dt,
                };
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion

        #region 7.点击检测线上表格一行，根据VIN号自动加载检测线下表格展示E_CAlignmentResult四轮定位数据
        public ActionResult GetGridTestLineResultAlignment(string VIN, JqGridParam jqgridparam)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                DataTable dt = new DataTable();
                strSql.Append(@"select * from E_CAlignmentResult  where VinCode =@VIN");
                List<DbParameter> parameter = new List<DbParameter>();
                parameter.Add(DbFactory.CreateDbParameter("@VIN", VIN));
                dt = DataFactory.Database().FindTableBySql(strSql.ToString(), parameter.ToArray(), false);
                var JsonData = new
                {
                    rows = dt,
                };
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion

        #region 8.点击检测线上表格一行，根据VIN号自动加载检测线下表格展示E_CABSResult ABS数据
        public ActionResult GetGridTestLineResultABS(string VIN, JqGridParam jqgridparam)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                DataTable dt = new DataTable();
                strSql.Append(@"select * from E_CABSResult  where VinCode = @VIN");
                List<DbParameter> parameter = new List<DbParameter>();
                parameter.Add(DbFactory.CreateDbParameter("@VIN", VIN));
                dt = DataFactory.Database().FindTableBySql(strSql.ToString(), parameter.ToArray(), false);
                var JsonData = new
                {
                    rows = dt,
                };
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion

        #endregion


        #region 加注区域方法

        #region 根据搜索条件加载加注上表格数据-计划表车身信息+8个表的检测结果
        public ActionResult GetJZResultByCondition(string CarType, string VIN, string Cd, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            Stopwatch watch = CommonHelper.TimerStart();
            //开始时间
            if (StartTime == "" || StartTime == null)
            {
                StartTime = "2021-01-01";
            }

            //结束时间
            if (EndTime == "" || EndTime == null)
            {
                EndTime = DateTime.Now.ToString("yyyy-MM-dd");
            }

            try
            {
                StringBuilder strSql = new StringBuilder();
                DataTable dt = new DataTable();
                //开始时间，结束时间，车型，VIN号-（模糊查询）
                strSql.Append($"select * from JZresult ('{StartTime}','{EndTime}','{CarType}','{VIN}','{Cd}')");
                dt = DataFactory.Database().FindTableBySql(strSql.ToString(), false);
                var JsonData = new
                {
                    jqgridparam.total,
                    jqgridparam.page,
                    jqgridparam.records,
                    costtime = CommonHelper.TimerEnd(watch),
                    rows = dt,
                };
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "1", "加注检测信息查询成功");
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "加注检测信息查询发生异常错误：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion

        #region 1.点击加注上表格一行，根据VIN号自动加载加注下表格展示E_JZEHY_FDY加注二合一防冻液数据
        public ActionResult GetGridJZResultEHYFDY(string VIN, JqGridParam jqgridparam)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                DataTable dt = new DataTable();
                strSql.Append(@"select * from E_JZEHY_FDY  where VIN = @VIN");
                List<DbParameter> parameter = new List<DbParameter>();
                parameter.Add(DbFactory.CreateDbParameter("@VIN", VIN));
                dt = DataFactory.Database().FindTableBySql(strSql.ToString(), parameter.ToArray(), false);
                var JsonData = new
                {
                    rows = dt,
                };
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion

        #region 2.点击加注上表格一行，根据VIN号自动加载加注下表格展示E_JZEHY_ZXY加注二合一转向液数据
        public ActionResult GetGridJZResultEHYZXY(string VIN, JqGridParam jqgridparam)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                DataTable dt = new DataTable();
                strSql.Append(@"select * from E_JZEHY_ZXY  where VIN = @VIN");
                List<DbParameter> parameter = new List<DbParameter>();
                parameter.Add(DbFactory.CreateDbParameter("@VIN", VIN));
                dt = DataFactory.Database().FindTableBySql(strSql.ToString(), parameter.ToArray(), false);
                var JsonData = new
                {
                    rows = dt,
                };
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion

        #region 3.点击加注上表格一行，根据VIN号自动加载加注下表格展示E_JZSHY_ZDY加注三合一制动液数据
        public ActionResult GetGridJZResultSHYZDY(string VIN, JqGridParam jqgridparam)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                DataTable dt = new DataTable();
                strSql.Append(@"select * from E_JZSHY_ZDY  where VIN =@VIN");
                List<DbParameter> parameter = new List<DbParameter>();
                parameter.Add(DbFactory.CreateDbParameter("@VIN", VIN));
                dt = DataFactory.Database().FindTableBySql(strSql.ToString(), parameter.ToArray(), false);
                var JsonData = new
                {
                    rows = dt,
                };
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion

        #region 4.点击加注上表格一行，根据VIN号自动加载加注下表格展示E_JZSHY_LM加注三合一冷媒数据
        public ActionResult GetGridJZResultSHYLM(string VIN, JqGridParam jqgridparam)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                DataTable dt = new DataTable();
                strSql.Append(@"select * from E_JZSHY_LM  where VIN = @VIN");
                List<DbParameter> parameter = new List<DbParameter>();
                parameter.Add(DbFactory.CreateDbParameter("@VIN", VIN));
                dt = DataFactory.Database().FindTableBySql(strSql.ToString(), parameter.ToArray(), false);
                var JsonData = new
                {
                    rows = dt,
                };
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion

        #region 5.点击加注上表格一行，根据VIN号自动加载加注下表格展示E_JZSHY_XDY加注三合一洗涤液数据
        public ActionResult GetGridJZResultSHYXDY(string VIN, JqGridParam jqgridparam)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                DataTable dt = new DataTable();
                strSql.Append(@"select * from E_JZSHY_XDY  where VIN = @VIN");
                List<DbParameter> parameter = new List<DbParameter>();
                parameter.Add(DbFactory.CreateDbParameter("@VIN", VIN));
                dt = DataFactory.Database().FindTableBySql(strSql.ToString(), parameter.ToArray(), false);
                var JsonData = new
                {
                    rows = dt,
                };
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion

        #region 6.点击加注上表格一行，根据VIN号自动加载加注下表格展示E_JZZY_CL正压CL数据
        public ActionResult GetGridJZResultZYCL(string VIN, JqGridParam jqgridparam)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                DataTable dt = new DataTable();
                strSql.Append(@"select * from E_JZZY_CL  where VIN = @VIN");
                List<DbParameter> parameter = new List<DbParameter>();
                parameter.Add(DbFactory.CreateDbParameter("@VIN", VIN));
                dt = DataFactory.Database().FindTableBySql(strSql.ToString(), parameter.ToArray(), false);
                var JsonData = new
                {
                    rows = dt,
                };
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion

        #region 7.点击加注上表格一行，根据VIN号自动加载加注下表格展示E_JZZY_BR正压BR数据
        public ActionResult GetGridJZResultZYBR(string VIN, JqGridParam jqgridparam)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                DataTable dt = new DataTable();
                strSql.Append(@"select * from E_JZZY_BR  where VIN = @VIN");
                List<DbParameter> parameter = new List<DbParameter>();
                parameter.Add(DbFactory.CreateDbParameter("@VIN", VIN));
                dt = DataFactory.Database().FindTableBySql(strSql.ToString(), parameter.ToArray(), false);
                var JsonData = new
                {
                    rows = dt,
                };
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion

        #region 8.点击加注上表格一行，根据VIN号自动加载加注下表格展示E_JZZY_AC正压AC数据
        public ActionResult GetGridJZResultZYAC(string VIN, JqGridParam jqgridparam)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                DataTable dt = new DataTable();
                strSql.Append(@"select * from E_JZZY_AC  where VIN =@VIN");
                List<DbParameter> parameter = new List<DbParameter>();
                parameter.Add(DbFactory.CreateDbParameter("@VIN", VIN));
                dt = DataFactory.Database().FindTableBySql(strSql.ToString(), parameter.ToArray(), false);
                var JsonData = new
                {
                    rows = dt,
                };
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion


        #endregion


        #region 胎压区域方法

        #region 1.加载胎压上表格数据-查找E_TireCheck表中当天检测数据
        public ActionResult GetTYCheckResultListJson(JqGridParam jqgridparam)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                DataTable dt = new DataTable();
                strSql.Append(@"select * from E_TireCheck  where  DateDiff(dd,DetectionTime,getdate())=0 order by DetectionTime desc");
                dt = DataFactory.Database().FindTableBySql(strSql.ToString(), false);
                var JsonData = new
                {
                    rows = dt,
                };
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion

        #region 2.根据搜索条件加载胎压上表格数据-E_TireCheck
        public ActionResult GetTYCheckResultByCondition( string CarType, string VIN, string Cd, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            #region 查询原方法
            //StartTime = StartTime + " 00:00:00";
            //EndTime = EndTime + " 23:59:59";
            //try
            //{
            //    StringBuilder strSql = new StringBuilder();
            //    DataTable dt = new DataTable();
            //    if (Condition == "CarType")
            //    {
            //        if (CarType == "all")
            //        {
            //            strSql.Append(@"select * from E_TireCheck  where  DetectionTime between '"+ StartTime + "' and '"+ EndTime + "' order by DetectionTime desc");
            //            dt = DataFactory.Database().FindTableBySql(strSql.ToString(), false);
            //        }
            //        else
            //        {
            //            strSql.Append(@"select * from E_TireCheck  where CarType like '%" + CarType + "%' DetectionTime between '" + StartTime + "' and '" + EndTime + "' order by DetectionTime desc");
            //            dt = DataFactory.Database().FindTableBySql(strSql.ToString(), false);
            //        }
            //    }
            //    else if (Condition == "VIN")
            //    {
            //        strSql.Append(@"select * from E_TireCheck  where VIN like '%" + VIN + "' DetectionTime between '" + StartTime + "' and '" + EndTime + "' order by DetectionTime desc");
            //        dt = DataFactory.Database().FindTableBySql(strSql.ToString(), false);
            //    }
            //    else
            //    {
            //        strSql.Append(@"select * from E_TireCheck  where  DetectionTime between '" + StartTime + "' and '" + EndTime + "' order by DetectionTime desc");
            //        dt = DataFactory.Database().FindTableBySql(strSql.ToString(), false);
            //    }
            //    var JsonData = new
            //    {
            //        rows = dt,
            //    };
            //    return Content(JsonData.ToJson());
            //}
            //catch (Exception ex)
            //{
            //    return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            //}
            #endregion

            #region 查询修改
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                StringBuilder strSql = new StringBuilder();
                DataTable dt = new DataTable();

                //初始语句未加搜索条件
                strSql.Append(@"select A.*,P.MatCd from E_TireCheck A join P_PublishPlan_Pro P on A.VIN=P.VIN and exists (select 1 from E_TireCheck B where A.VIN=B.VIN group by vin having  A.DetectionTime=MAX(B.DetectionTime))  ");

                List<DbParameter> parameter = new List<DbParameter>();
                //模糊搜索车型
                if (CarType != "" && CarType != null)
                {
                    //strSql.Append(" and CarType like '%" + CarType + "%' ");
                    strSql.Append(" and A.CarType like @CarType ");
                    parameter.Add(DbFactory.CreateDbParameter("@CarType", "%" + CarType + "%"));
                }

                //模糊搜索VIN号
                if (VIN != "" && VIN != null)
                {
                    //strSql.Append(" and VIN like '%" + VIN + "%' ");
                    strSql.Append(" and A.VIN like @VIN ");
                    parameter.Add(DbFactory.CreateDbParameter("@VIN", "%" + VIN + "%"));
                }

                //模糊搜索cd
                if (Cd != "" && Cd != null)
                {
                    //strSql.Append(" and VIN like '%" + VIN + "%' ");
                    strSql.Append(" and P.MatCd like @Cd ");
                    parameter.Add(DbFactory.CreateDbParameter("@Cd", "%" + Cd + "%"));
                }

                //开始时间
                if (StartTime != null && StartTime != "")
                {
                    //string StartTime2 = DateDiff(StartTime);    //转换为到今天的天数以减少查询时间
                    //strSql.Append(@" and DateDiff(dd,DetectionTime,getdate()) <= '" + StartTime2 + "' ");
                    //开始时间把@放在前面
                    strSql.Append(" and DateDiff(dd,@StartTime,DetectionTime) >=0 ");
                    parameter.Add(DbFactory.CreateDbParameter("@StartTime", StartTime));
                }

                //结束时间
                if (EndTime != null && EndTime != "")
                {
                    //string EndTime2 = DateDiff(EndTime);    //转换为到今天的天数以减少查询时间
                    //strSql.Append(@" and DateDiff(dd,DetectionTime,getdate()) >= '" + EndTime2 + "' ");
                    //结束时间把@放在后面
                    strSql.Append(" and DateDiff(dd,DetectionTime,@EndTime) >=0 ");
                    parameter.Add(DbFactory.CreateDbParameter("@EndTime", EndTime));
                }

                //按照时间排序
                strSql.Append(" order by DetectionTime desc");

                dt = DataFactory.Database().FindTableBySql(strSql.ToString(), parameter.ToArray(), false);
                var JsonData = new
                {
                    jqgridparam.total,
                    jqgridparam.page,
                    jqgridparam.records,
                    costtime = CommonHelper.TimerEnd(watch),
                    rows = dt,
                };
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "1", "胎压检测信息查询成功");
                return Content(JsonData.ToJson());

            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "胎压检测信息查询发生异常错误：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }


            
            #endregion



        }
        #endregion



        #endregion


        #region 拧紧区域方法
        public ActionResult GetNJCheckResultByCondition(string CarType, string VIN, string StartTime, string EndTime,string Cd ,JqGridParam jqgridparam)
        {
            #region 查询修改
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                StringBuilder strSql = new StringBuilder();
                DataTable dt = new DataTable();

                //初始语句未加搜索条件
                strSql.Append(@"select a.*,b.TorqueUL,b.TorqueLL,b.TorqueSL,b.AngleUL,AngleLL,AngleSL,Ord,case when IsOK='1'then '合格'else '不合格'end  as tg_result 
from Tg_TightenDataDoc a left join Tg_JobTorqueConfig b on  
a.WcJobCd=b.WcJobCd and a.RsvFld2=b.Ord  where 1=1   ");

                List<DbParameter> parameter = new List<DbParameter>();
                //模糊搜索车型
                if (CarType != "" && CarType != null)
                {
                    //strSql.Append(" and C.CarType like '%" + CarType + "%' ");
                    strSql.Append(" and (a.Code like @CarType and (a.Type='车辆类型'or a.type='车型+图号') )");
                    parameter.Add(DbFactory.CreateDbParameter("@CarType", "%" + CarType + "%"));
                }

                //模糊搜索VIN号
                if (VIN != "" && VIN != null)
                {
                    //strSql.Append(" and IDENTIFY like '%" + VIN + "%' ");
                    strSql.Append(" and a.Vin like @VIN ");
                    parameter.Add(DbFactory.CreateDbParameter("@VIN", "%" + VIN + "%"));
                }

                //模糊搜索cd
                if (Cd != "" && Cd != null)
                {
                    //strSql.Append(" and VIN like '%" + VIN + "%' ");
                    strSql.Append(" and( (a.Code like @Cd and a.Type='图号') or(a.RsvFld1 like @Cd and a.Type='车型+图号'))");
                    parameter.Add(DbFactory.CreateDbParameter("@Cd", "%" + Cd + "%"));
                }

                //开始时间
                if (StartTime != null && StartTime != "")
                {
                    //string StartTime2 = DateDiff(StartTime);    //转换为到今天的天数以减少查询时间
                    //strSql.Append(@" and DateDiff(dd,TIME_STAMP,getdate()) <= '" + StartTime2 + "' ");
                    //开始时间把@放在前面
                    strSql.Append(" and DateDiff(dd,@StartTime,CollectionTime) >=0 ");
                    parameter.Add(DbFactory.CreateDbParameter("@StartTime", StartTime));
                }

                //结束时间
                if (EndTime != null && EndTime != "")
                {
                    //string EndTime2 = DateDiff(EndTime);    //转换为到今天的天数以减少查询时间
                    //strSql.Append(@" and DateDiff(dd,TIME_STAMP,getdate()) >= '" + EndTime2 + "' ");
                    //结束时间把@放在后面
                    strSql.Append(" and DateDiff(dd,CollectionTime,@EndTime) >=0 ");
                    parameter.Add(DbFactory.CreateDbParameter("@EndTime", EndTime));
                }
                else { }

                //按照时间排序
                strSql.Append(" order by CollectionTime desc");

                dt = DataFactory.Database().FindTableBySql(strSql.ToString(), parameter.ToArray(), false);
                var JsonData = new
                {
                    jqgridparam.total,
                    jqgridparam.page,
                    jqgridparam.records,
                    costtime = CommonHelper.TimerEnd(watch),
                    rows = dt,
                };
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "1", "拧紧检测信息查询成功");
                return Content(JsonData.ToJson());

            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "拧紧检测信息查询发生异常错误：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }



            #endregion



        }

        #endregion


        #region 导出

        #region EOL数据导出
        public ActionResult GetExcel_DataEOL(string StartTime, string EndTime, string VIN, string Cd, string CarType)
        {
            try
            {
                #region 根据当前搜索条件查出数据并导出
                StringBuilder strSql = new StringBuilder();
                DataTable dt = new DataTable();

                //初始语句未加搜索条件
                strSql.Append(@"select A.VIN,C.MatCd,C.CarType as CarType2,A.CarName,A.Station,A.Time,A.TestNum,A.Result from E_EOLCheckResult A left join P_ProducePlan_Pro C ON A.VIN = C.VIN where  exists (select 1 from E_EOLCheckResult B where A.VIN=B.VIN group by vin having  A.TestNum=MAX(B.TestNum)) ");

                List<DbParameter> parameter = new List<DbParameter>();
                //模糊搜索车型
                if (CarType != "" && CarType != null)
                {
                    //strSql.Append(" and C.CarType like '%" + CarType + "%' ");
                    strSql.Append(" and C.CarType like @CarType ");
                    parameter.Add(DbFactory.CreateDbParameter("@CarType", "%" + CarType + "%"));
                }

                //模糊搜索VIN号
                if (VIN != "" && VIN != null)
                {
                    //strSql.Append(" and VIN like '%" + VIN + "%' ");
                    strSql.Append(" and A.VIN like @VIN ");
                    parameter.Add(DbFactory.CreateDbParameter("@VIN", "%" + VIN + "%"));
                }
                //模糊搜索车型编码
                if (Cd != "" && Cd != null)
                {
                    //strSql.Append(" and VIN like '%" + VIN + "%' ");
                    strSql.Append(" and C.MatCd like @Cd ");
                    parameter.Add(DbFactory.CreateDbParameter("@Cd", "%" + Cd + "%"));
                }

                //开始时间
                if (StartTime != null && StartTime != "")
                {
                    //string StartTime2 = DateDiff(StartTime);    //转换为到今天的天数以减少查询时间
                    //strSql.Append(@" and DateDiff(dd,Time,getdate()) <= '" + StartTime2 + "' ");
                    //开始时间把@放在前面
                    strSql.Append(" and DateDiff(dd,@StartTime,Time) >=0 ");
                    parameter.Add(DbFactory.CreateDbParameter("@StartTime", StartTime));
                }

                //结束时间
                if (EndTime != null && EndTime != "")
                {
                    //string EndTime2 = DateDiff(EndTime);    //转换为到今天的天数以减少查询时间
                    //strSql.Append(@" and DateDiff(dd,Time,getdate()) >= '" + EndTime2 + "' ");
                    //结束时间把@放在后面
                    strSql.Append(" and DateDiff(dd,Time,@EndTime) >=0 ");
                    parameter.Add(DbFactory.CreateDbParameter("@EndTime", EndTime));
                }

                //按照时间排序
                strSql.Append(" order by Time desc");

                dt = DataFactory.Database().FindTableBySql(strSql.ToString(), parameter.ToArray(), false);

                #endregion



                string fileName = "EOL检测数据导出";
                string excelType = "xls";
                MemoryStream ms = DeriveExcel.ExportExcel_EOL(dt, excelType);
                if (!fileName.EndsWith(".xls"))
                {
                    fileName = fileName + ".xls";
                }
                #region 解决乱码
                //Encoding ec = Encoding.GetEncoding("iso-8859-1");
                //byte[] btArr = ec.GetBytes(fileName);
                //string fileName2 = Encoding.Default.GetString(btArr);
                #endregion
                Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other, "1", "EOL检测数据导出成功");
                return File(ms, "application/vnd.ms-excel", Url.Encode(fileName));
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other, "-1", "EOL检测数据导出操作失败：" + ex.Message);
                return null;
            }

        }
        #endregion

        #region 检测线数据导出
        public ActionResult GetExcel_DataTestLine(string StartTime, string EndTime, string VIN, string CarType, string Cd)
        {
            try
            {
                #region 根据当前搜索条件查出数据并导出
                if (StartTime != "" && StartTime != null)
                {
                    StartTime = StartTime + " 00:00:00";
                }
                else
                {
                    StartTime = "2021-01-01 00:00:00";
                }

                //结束时间
                if (EndTime != "" && EndTime != null)
                {
                    EndTime = EndTime + " 23:59:59";
                }
                else
                {
                    EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                    EndTime = EndTime + " 23:59:59";
                }

                StringBuilder strSql = new StringBuilder();
                DataTable dt = new DataTable();
                //开始时间，结束时间，车型，VIN号-（模糊查询）
                strSql.Append(@"select VIN,MatCd,CarType,CarColor,ABSResult,AlignmentResult,AngleResult,BrakeResult,Head_Result,HornResult,SpeedResult from TestLineResult ('" + StartTime + "','" + EndTime + "','" + CarType + "','" + VIN + "','" + Cd + "')");
                dt = DataFactory.Database().FindTableBySql(strSql.ToString(), false);

                #endregion



                string fileName = "检测线检测数据导出";
                string excelType = "xls";
                MemoryStream ms = DeriveExcel.ExportExcel_TestLine(dt, excelType);
                if (!fileName.EndsWith(".xls"))
                {
                    fileName = fileName + ".xls";
                }
                #region 解决乱码
                //Encoding ec = Encoding.GetEncoding("iso-8859-1");
                //byte[] btArr = ec.GetBytes(fileName);
                //string fileName2 = Encoding.Default.GetString(btArr);
                #endregion
                Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other, "1", "检测线检测数据导出成功");
                return File(ms, "application/vnd.ms-excel", Url.Encode(fileName));
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other, "-1", "检测线检测数据导出操作失败：" + ex.Message);
                return null;
            }

        }
        #endregion

        #region 加注数据导出
        public ActionResult GetExcel_DataJZ(string StartTime, string EndTime, string VIN, string CarType, string Cd)
        {
            try
            {
                #region 根据当前搜索条件查出数据并导出
                if (StartTime != "" && StartTime != null)
                {
                    StartTime = StartTime + " 00:00:00";
                }
                else
                {
                    StartTime = "2021-01-01 00:00:00";
                }

                //结束时间
                if (EndTime != "" && EndTime != null)
                {
                    EndTime = EndTime + " 23:59:59";
                }
                else
                {
                    EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                    EndTime = EndTime + " 23:59:59";
                }

                StringBuilder strSql = new StringBuilder();
                DataTable dt = new DataTable();
                //开始时间，结束时间，车型，VIN号-（模糊查询）
                strSql.Append(@"select VIN,MatCd,CarType,CarColor," + "防冻液加注结果" + ",转向液加注结果,制动液加注结果,冷媒加注结果,洗涤液加注结果,正压CL检测结果,正压BR检测结果,正压AC检测结果 from JZresult ('" + StartTime + "','" + EndTime + "','" + CarType + "','" + VIN + "','" + Cd + "')");

                #region 解决乱码
                //Encoding ec = Encoding.GetEncoding("iso-8859-1");
                //byte[] btArr = ec.GetBytes(strSql.ToString());
                //string strSql2 = Encoding.Default.GetString(btArr);
                #endregion

                dt = DataFactory.Database().FindTableBySql(strSql.ToString(), false);

                #endregion



                string fileName = "加注检测数据导出";
                string excelType = "xls";
                MemoryStream ms = DeriveExcel.ExportExcel_JZ(dt, excelType);
                if (!fileName.EndsWith(".xls"))
                {
                    fileName = fileName + ".xls";
                }
                #region 解决乱码
                //Encoding ec2 = Encoding.GetEncoding("iso-8859-1");
                //byte[] btArr2 = ec2.GetBytes(fileName);
                //string fileName2 = Encoding.Default.GetString(btArr2);
                #endregion
                Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other, "1", "加注检测数据导出成功");
                return File(ms, "application/vnd.ms-excel", Url.Encode(fileName));
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other, "-1", "加注检测数据导出操作失败：" + ex.Message);
                return null;
            }

        }
        #endregion

        #region 胎压数据导出
        public ActionResult GetExcel_DataTY(string StartTime, string EndTime, string VIN, string CarType, string Cd)
        {
            try
            {
                #region 根据当前搜索条件查出数据并导出
                StringBuilder strSql = new StringBuilder();
                DataTable dt = new DataTable();

                //初始语句未加搜索条件
                strSql.Append(@"select A.VIN,P.MatCd,A.CarType,A.SensorNm,A.RFTireID,A.RFTirePressureLimit,A.RFTirePressureLowerLimit,A.RFTirePressureValue,A.RFTirePressureResult,A.LFTireID,A.LFTirePressureLimit,A.LFTirePressureLowerLimit,A.LFTirePressureValue,A.LFTirePressureResult,A.RBTireID,A.RBTirePressureLimit,A.RBTirePressureLowerLimit,A.RBTirePressureValue,A.RBTirePressureResult,A.LBTireID,A.LBTirePressureLimit,A.LBTirePressureLowerLimit,A.LBTirePressureValue,A.LBTirePressureResult,A.TirePressureUnit,A.DetectionSource,A.DetectionTime,A.TireResult from E_TireCheck A join P_PublishPlan_Pro P on A.VIN=P.VIN and exists (select 1 from E_TireCheck B where A.VIN=B.VIN group by vin having  A.DetectionTime=MAX(B.DetectionTime))  ");

                List<DbParameter> parameter = new List<DbParameter>();
                //模糊搜索车型
                if (CarType != "" && CarType != null)
                {
                    //strSql.Append(" and CarType like '%" + CarType + "%' ");
                    strSql.Append(" and CarType like @CarType ");
                    parameter.Add(DbFactory.CreateDbParameter("@CarType", "%" + CarType + "%"));
                }

                //模糊搜索VIN号
                if (VIN != "" && VIN != null)
                {
                    //strSql.Append(" and VIN like '%" + VIN + "%' ");
                    strSql.Append(" and VIN like @VIN ");
                    parameter.Add(DbFactory.CreateDbParameter("@VIN", "%" + VIN + "%"));
                }

                //模糊搜索车型编码
                if (Cd != "" && Cd != null)
                {
                    //strSql.Append(" and VIN like '%" + VIN + "%' ");
                    strSql.Append(" and P.MatCd like @Cd ");
                    parameter.Add(DbFactory.CreateDbParameter("@Cd", "%" + Cd + "%"));
                }

                //开始时间
                if (StartTime != null && StartTime != "")
                {
                    //string StartTime2 = DateDiff(StartTime);    //转换为到今天的天数以减少查询时间
                    //strSql.Append(@" and DateDiff(dd,DetectionTime,getdate()) <= '" + StartTime2 + "' ");
                    //开始时间把@放在前面
                    strSql.Append(" and DateDiff(dd,@StartTime,DetectionTime) >=0 ");
                    parameter.Add(DbFactory.CreateDbParameter("@StartTime", StartTime));
                }

                //结束时间
                if (EndTime != null && EndTime != "")
                {
                    //string EndTime2 = DateDiff(EndTime);    //转换为到今天的天数以减少查询时间
                    //strSql.Append(@" and DateDiff(dd,DetectionTime,getdate()) >= '" + EndTime2 + "' ");
                    //结束时间把@放在后面
                    strSql.Append(" and DateDiff(dd,DetectionTime,@EndTime) >=0 ");
                    parameter.Add(DbFactory.CreateDbParameter("@EndTime", EndTime));
                }

                //按照时间排序
                strSql.Append(" order by DetectionTime desc");

                dt = DataFactory.Database().FindTableBySql(strSql.ToString(), parameter.ToArray(), false);
                #endregion



                string fileName = "胎压检测数据导出";
                string excelType = "xls";
                MemoryStream ms = DeriveExcel.ExportExcel_TY(dt, excelType);
                if (!fileName.EndsWith(".xls"))
                {
                    fileName = fileName + ".xls";
                }
                #region 解决乱码
                //Encoding ec = Encoding.GetEncoding("iso-8859-1");
                //byte[] btArr = ec.GetBytes(fileName);
                //string fileName2 = Encoding.Default.GetString(btArr);
                #endregion
                Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other, "1", "胎压检测数据导出成功");
                return File(ms, "application/vnd.ms-excel", Url.Encode(fileName));
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other, "-1", "胎压检测数据导出操作失败：" + ex.Message);
                return null;
            }

        }
        #endregion

        #region 拧紧数据导出
        public ActionResult GetExcel_DataNJ(string StartTime, string EndTime, string VIN, string CarType, string Cd)
        {
            try
            {
                #region 根据当前搜索条件查出数据并导出
                StringBuilder strSql = new StringBuilder();
                DataTable dt = new DataTable();

                ////初始语句未加搜索条件
                //strSql.Append(@"select A.IDENTIFY,P.MatCd as Cd,C.CarType,A.JOB_NO,A.PSET_NO,A.BATCH_SIZE,A.BATCH_COUNT,A.BATCH_STATUS,A.TORQUE_STATUS,A.TORQUE_MIN,A.TORQUE_MAX,A.TORQUE_TARGET,A.TORQUE,A.ANGLE_STATUS,A.ANGLE_MIN,A.ANGLE_MAX,A.ANGLE_TARGET,A.ANGLE,A.TIGHTENING_ID,A.TIGHTENING_STATUS,A.TIME_STAMP from E_SCREW A left join P_ProducePlan_Pro C ON A.IDENTIFY = C.VIN left join P_PublishPlan_Pro P on A.IDENTIFY=P.VIN where exists (select 1 from E_SCREW B where A.IDENTIFY=B.IDENTIFY group by IDENTIFY having  A.TIME_STAMP=MAX(B.TIME_STAMP))  ");

                //List<DbParameter> parameter = new List<DbParameter>();
                ////模糊搜索车型
                //if (CarType != "" && CarType != null)
                //{
                //    //strSql.Append(" and C.CarType like '%" + CarType + "%' ");
                //    strSql.Append(" and C.CarType like @CarType ");
                //    parameter.Add(DbFactory.CreateDbParameter("@CarType", "%" + CarType + "%"));
                //}

                ////模糊搜索VIN号
                //if (VIN != "" && VIN != null)
                //{
                //    //strSql.Append(" and IDENTIFY like '%" + VIN + "%' ");
                //    strSql.Append(" and IDENTIFY like @VIN ");
                //    parameter.Add(DbFactory.CreateDbParameter("@VIN", "%" + VIN + "%"));
                //}

                ////模糊搜索cd
                //if (Cd != "" && Cd != null)
                //{
                //    //strSql.Append(" and VIN like '%" + VIN + "%' ");
                //    strSql.Append(" and P.MatCd like @Cd ");
                //    parameter.Add(DbFactory.CreateDbParameter("@Cd", "%" + Cd + "%"));
                //}

                ////开始时间
                //if (StartTime != null && StartTime != "")
                //{
                //    //string StartTime2 = DateDiff(StartTime);    //转换为到今天的天数以减少查询时间
                //    //strSql.Append(@" and DateDiff(dd,TIME_STAMP,getdate()) <= '" + StartTime2 + "' ");
                //    //开始时间把@放在前面
                //    strSql.Append(" and DateDiff(dd,@StartTime,TIME_STAMP) >=0 ");
                //    parameter.Add(DbFactory.CreateDbParameter("@StartTime", StartTime));
                //}

                ////结束时间
                //if (EndTime != null && EndTime != "")
                //{
                //    //string EndTime2 = DateDiff(EndTime);    //转换为到今天的天数以减少查询时间
                //    //strSql.Append(@" and DateDiff(dd,TIME_STAMP,getdate()) >= '" + EndTime2 + "' ");
                //    //结束时间把@放在后面
                //    strSql.Append(" and DateDiff(dd,TIME_STAMP,@EndTime) >=0 ");
                //    parameter.Add(DbFactory.CreateDbParameter("@EndTime", EndTime));
                //}
                //else { }

                ////按照时间排序
                //strSql.Append(" order by TIME_STAMP desc");
                //初始语句未加搜索条件
                strSql.Append(@"select a.*,b.TorqueUL,b.TorqueLL,b.TorqueSL,b.AngleUL,AngleLL,AngleSL,Serial,Ord,'合格' from Tg_TightenDataDoc a join Tg_JobTorqueConfig b on  
a.WcJobCd=b.WcJobCd and a.RsvFld2=b.Ord ");

                List<DbParameter> parameter = new List<DbParameter>();
                //模糊搜索车型
                if (CarType != "" && CarType != null)
                {
                    //strSql.Append(" and C.CarType like '%" + CarType + "%' ");
                    strSql.Append(" and (a.Code like @CarType and (a.Type='车辆类型'or a.type='车型+图号') )");
                    parameter.Add(DbFactory.CreateDbParameter("@CarType", "%" + CarType + "%"));
                }

                //模糊搜索VIN号
                if (VIN != "" && VIN != null)
                {
                    //strSql.Append(" and IDENTIFY like '%" + VIN + "%' ");
                    strSql.Append(" and a.Vin like @VIN ");
                    parameter.Add(DbFactory.CreateDbParameter("@VIN", "%" + VIN + "%"));
                }

                //模糊搜索cd
                if (Cd != "" && Cd != null)
                {
                    //strSql.Append(" and VIN like '%" + VIN + "%' ");
                    strSql.Append(" and( (a.Code like @Cd and a.Type='图号') or(a.RsvFld1 like @Cd and a.Type='车型+图号'))");
                    parameter.Add(DbFactory.CreateDbParameter("@Cd", "%" + Cd + "%"));
                }

                //开始时间
                if (StartTime != null && StartTime != "")
                {
                    //string StartTime2 = DateDiff(StartTime);    //转换为到今天的天数以减少查询时间
                    //strSql.Append(@" and DateDiff(dd,TIME_STAMP,getdate()) <= '" + StartTime2 + "' ");
                    //开始时间把@放在前面
                    strSql.Append(" and DateDiff(dd,@StartTime,CollectionTime) >=0 ");
                    parameter.Add(DbFactory.CreateDbParameter("@StartTime", StartTime));
                }

                //结束时间
                if (EndTime != null && EndTime != "")
                {
                    //string EndTime2 = DateDiff(EndTime);    //转换为到今天的天数以减少查询时间
                    //strSql.Append(@" and DateDiff(dd,TIME_STAMP,getdate()) >= '" + EndTime2 + "' ");
                    //结束时间把@放在后面
                    strSql.Append(" and DateDiff(dd,CollectionTime,@EndTime) >=0 ");
                    parameter.Add(DbFactory.CreateDbParameter("@EndTime", EndTime));
                }
                else { }

                //按照时间排序
                strSql.Append(" order by CollectionTime desc");
                dt = DataFactory.Database().FindTableBySql(strSql.ToString(), parameter.ToArray(), false);
                dt.Columns.Remove("ScrewID");
                #endregion



                string fileName = "拧紧数据导出";
                string excelType = "xls";
                MemoryStream ms = DeriveExcel.ExportExcel_NJ(dt, excelType);
                if (!fileName.EndsWith(".xls"))
                {
                    fileName = fileName + ".xls";
                }
                #region 解决乱码
                //Encoding ec = Encoding.GetEncoding("iso-8859-1");
                //byte[] btArr = ec.GetBytes(fileName);
                //string fileName2 = Encoding.Default.GetString(btArr);
                #endregion
                Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other, "1", "拧紧检测数据导出成功");
                return File(ms, "application/vnd.ms-excel", Url.Encode(fileName));
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other, "-1", "拧紧检测数据导出操作失败：" + ex.Message);
                return null;
            }

        }
        #endregion

        #endregion





        #endregion

    }
}