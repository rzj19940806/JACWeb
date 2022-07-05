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

namespace HfutIE.WebApp.Areas.PlanModule.Controllers
{
    /// <summary>
    /// 小时计划接收过程表控制器
    /// </summary>
    public class HourProducePlanController : PublicController<P_HourProducePlan_Pro>
    {
        #region 创建数据库操作对象区域
        HourProducePlanBll HourProducePlanBll = new HourProducePlanBll();
        #endregion

        #region 查询方法
        public ActionResult GetHourProducePlanListJson(string StartTimePlanTime, string EndTimePlanTime, string ConditionVIN, string ConditionProducePlanCd, string ConditionOrderCd, string ConditionMatCd, string StartTimeRecieveTm, string EndTimeRecieveTm, string ConditionCarType, string ConditionPlanStatus, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                #region datatable查询
                StringBuilder strSql = new StringBuilder();
                strSql.Append(@"SELECT * FROM P_HourProducePlan_Pro WHERE Enabled='1' ");

                #region 判断输入框内容添加检索条件
                List<DbParameter> parameter = new List<DbParameter>();
                //是否加VIN号模糊搜索
                if (ConditionVIN != "" && ConditionVIN != null)
                {
                    //strSql.Append(" and VIN like '%" + ConditionVIN + "%'");
                    strSql.Append(" and VIN like @VIN ");
                    parameter.Add(DbFactory.CreateDbParameter("@VIN", "%" + ConditionVIN));
                }
                else { }

                //是否加计划编号模糊搜索
                if (ConditionProducePlanCd != "" && ConditionProducePlanCd != null)
                {
                    //strSql.Append(" and ProducePlanCd like '%" + ConditionProducePlanCd + "%'");
                    strSql.Append(" and ProducePlanCd like @ProducePlanCd ");
                    parameter.Add(DbFactory.CreateDbParameter("@ProducePlanCd", "%" + ConditionProducePlanCd + "%"));
                }
                else { }

                //是否加订单号模糊搜索
                if (ConditionOrderCd != "" && ConditionOrderCd != null)
                {
                    //strSql.Append(" and OrderCd like '%" + ConditionOrderCd + "%'");
                    strSql.Append(" and OrderCd like @ConditionOrderCd ");
                    parameter.Add(DbFactory.CreateDbParameter("@ConditionOrderCd", "%" + ConditionOrderCd + "%"));
                }
                else { }

                //是否加车型编码模糊搜索
                if (ConditionMatCd != "" && ConditionMatCd != null)
                {
                    //strSql.Append(" and MatCd like '%" + ConditionMatCd + "%'");
                    strSql.Append(" and MatCd like @MatCd ");
                    parameter.Add(DbFactory.CreateDbParameter("@MatCd", "%" + ConditionMatCd + "%"));
                }
                else { }

                //是否加车型模糊搜索
                if (ConditionCarType != "" && ConditionCarType != null)
                {
                    if (ConditionCarType == "L5")
                    {
                        ConditionCarType = "5";
                    }
                    else { }
                    //strSql.Append(" and CarType like '%" + ConditionCarType + "%'");
                    strSql.Append(" and CarType like @CarType ");
                    parameter.Add(DbFactory.CreateDbParameter("@CarType", "%" + ConditionCarType + "%"));
                }
                else { }

                //是否加计划状态搜索
                if (ConditionPlanStatus != "" && ConditionPlanStatus != null)
                {
                    //strSql.Append(" and PlanStatus = '" + ConditionPlanStatus + "'");
                    strSql.Append(" and PlanStatus like @PlanStatus ");
                    parameter.Add(DbFactory.CreateDbParameter("@PlanStatus", "%" + ConditionPlanStatus + "%"));
                }
                else { }

                //预计生产开始时间
                if (StartTimePlanTime != "" && StartTimePlanTime != null)
                {
                    //string StartPlanTime = DateDiff(StartTimePlanTime);    //计划开始时间距离当天的天数 大
                    //strSql.Append(" and DateDiff(dd,PlanTime,getdate()) <=  " + StartPlanTime);

                    strSql.Append(" and DateDiff(dd,@StartTimePlanTime,PlanTime) >=0 ");
                    parameter.Add(DbFactory.CreateDbParameter("@StartTimePlanTime", StartTimePlanTime));
                }
                else { }

                //预计生产结束时间
                if (EndTimePlanTime != "" && EndTimePlanTime != null)
                {
                    //string EndPlanTime = DateDiff(EndTimePlanTime);        //计划结束时间距离当天的天数 小
                    //strSql.Append(" and DateDiff(dd,PlanTime,getdate()) >= " + EndPlanTime);

                    strSql.Append(" and DateDiff(dd,PlanTime,@EndTimePlanTime) >=0 ");
                    parameter.Add(DbFactory.CreateDbParameter("@EndTimePlanTime", EndTimePlanTime));
                }
                else { }

                //计划接收开始时间
                if (StartTimeRecieveTm != "" && StartTimeRecieveTm != null)
                {
                    //string StartRecieveTm = DateDiff(StartTimeRecieveTm);  //接收开始时间距离当天的天数 大
                    //strSql.Append(" and DateDiff(dd,RecieveTm,getdate()) <= " + StartRecieveTm);

                    strSql.Append(" and DateDiff(dd,@StartTimeRecieveTm,RecieveTm) >=0 ");
                    parameter.Add(DbFactory.CreateDbParameter("@StartTimeRecieveTm", StartTimeRecieveTm));

                }
                else { }

                //计划接收结束时间
                if (EndTimeRecieveTm != "" && EndTimeRecieveTm != null)
                {
                    //string EndRecieveTm = DateDiff(EndTimeRecieveTm);      //接收结束时间距离当天的天数 小
                    //strSql.Append(" and DateDiff(dd,RecieveTm,getdate()) >= " + EndRecieveTm);

                    strSql.Append(" and DateDiff(dd,RecieveTm,@EndTimeRecieveTm) >=0 ");
                    parameter.Add(DbFactory.CreateDbParameter("@EndTimeRecieveTm", EndTimeRecieveTm));
                }
                else { }
                #endregion

                //按照计划时间排序
                strSql.Append(" order by PlanTime desc");

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
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "1", "小时计划信息查询成功");
                return Content(JsonData.ToJson());
                #endregion
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "查询小时计划序列时发生异常错误：" + ex.Message);
                return null;
            }
        }
        #endregion

        #region 计算日期与今天日期的天数
        public string DateDiff(string DateTime1)
        {
            try
            {
                DateTime today = DateTime.Now;//获取当前时间
                TimeSpan ts1 = new TimeSpan(Convert.ToDateTime(DateTime1).Ticks);
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

        #region 重构导出
        public ActionResult GetExcel_Data(string StartTimePlanTime, string EndTimePlanTime, string ConditionVIN, string ConditionProducePlanCd, string ConditionOrderCd, string ConditionMatCd, string StartTimeRecieveTm, string EndTimeRecieveTm, string ConditionCarType, string ConditionPlanStatus, JqGridParam jqgridparam)
        {
            try
            {
                
                #region 根据当前搜索条件查出数据并导出

                StringBuilder strSql = new StringBuilder();
                strSql.Append(@"SELECT ProducePlanCd,OrderCd,VIN,MatCd,CarType,CarColor1,PlanTime,PlanStatus,RecieveTm FROM P_HourProducePlan_Pro WHERE Enabled='1' ");

                #region 判断输入框内容添加检索条件
                List<DbParameter> parameter = new List<DbParameter>();
                //是否加VIN号模糊搜索
                if (ConditionVIN != "" && ConditionVIN != null)
                {
                    //strSql.Append(" and VIN like '%" + ConditionVIN + "%'");
                    strSql.Append(" and VIN like @VIN ");
                    parameter.Add(DbFactory.CreateDbParameter("@VIN", "%" + ConditionVIN));
                }
                else { }

                //是否加计划编号模糊搜索
                if (ConditionProducePlanCd != "" && ConditionProducePlanCd != null)
                {
                    //strSql.Append(" and ProducePlanCd like '%" + ConditionProducePlanCd + "%'");
                    strSql.Append(" and ProducePlanCd like @ProducePlanCd ");
                    parameter.Add(DbFactory.CreateDbParameter("@ProducePlanCd", "%" + ConditionProducePlanCd + "%"));
                }
                else { }

                //是否加订单号模糊搜索
                if (ConditionOrderCd != "" && ConditionOrderCd != null)
                {
                    //strSql.Append(" and OrderCd like '%" + ConditionOrderCd + "%'");
                    strSql.Append(" and OrderCd like @ConditionOrderCd ");
                    parameter.Add(DbFactory.CreateDbParameter("@ConditionOrderCd", "%" + ConditionOrderCd + "%"));
                }
                else { }

                //是否加车型编码模糊搜索
                if (ConditionMatCd != "" && ConditionMatCd != null)
                {
                    //strSql.Append(" and MatCd like '%" + ConditionMatCd + "%'");
                    strSql.Append(" and MatCd like @MatCd ");
                    parameter.Add(DbFactory.CreateDbParameter("@MatCd", "%" + ConditionMatCd + "%"));
                }
                else { }

                //是否加车型模糊搜索
                if (ConditionCarType != "" && ConditionCarType != null)
                {
                    if (ConditionCarType == "L5")
                    {
                        ConditionCarType = "5";
                    }
                    else { }
                    //strSql.Append(" and CarType like '%" + ConditionCarType + "%'");
                    strSql.Append(" and CarType like @CarType ");
                    parameter.Add(DbFactory.CreateDbParameter("@CarType", "%" + ConditionCarType + "%"));
                }
                else { }

                //是否加计划状态搜索
                if (ConditionPlanStatus != "" && ConditionPlanStatus != null)
                {
                    //strSql.Append(" and PlanStatus = '" + ConditionPlanStatus + "'");
                    strSql.Append(" and PlanStatus like @PlanStatus ");
                    parameter.Add(DbFactory.CreateDbParameter("@PlanStatus", "%" + ConditionPlanStatus + "%"));
                }
                else { }

                //预计生产开始时间
                if (StartTimePlanTime != "" && StartTimePlanTime != null)
                {
                    //string StartPlanTime = DateDiff(StartTimePlanTime);    //计划开始时间距离当天的天数 大
                    //strSql.Append(" and DateDiff(dd,PlanTime,getdate()) <=  " + StartPlanTime);

                    strSql.Append(" and DateDiff(dd,@StartTimePlanTime,PlanTime) >=0 ");
                    parameter.Add(DbFactory.CreateDbParameter("@StartTimePlanTime", StartTimePlanTime));
                }
                else { }

                //预计生产结束时间
                if (EndTimePlanTime != "" && EndTimePlanTime != null)
                {
                    //string EndPlanTime = DateDiff(EndTimePlanTime);        //计划结束时间距离当天的天数 小
                    //strSql.Append(" and DateDiff(dd,PlanTime,getdate()) >= " + EndPlanTime);

                    strSql.Append(" and DateDiff(dd,PlanTime,@EndTimePlanTime) >=0 ");
                    parameter.Add(DbFactory.CreateDbParameter("@EndTimePlanTime", EndTimePlanTime));
                }
                else { }

                //计划接收开始时间
                if (StartTimeRecieveTm != "" && StartTimeRecieveTm != null)
                {
                    //string StartRecieveTm = DateDiff(StartTimeRecieveTm);  //接收开始时间距离当天的天数 大
                    //strSql.Append(" and DateDiff(dd,RecieveTm,getdate()) <= " + StartRecieveTm);

                    strSql.Append(" and DateDiff(dd,@StartTimeRecieveTm,RecieveTm) >=0 ");
                    parameter.Add(DbFactory.CreateDbParameter("@StartTimeRecieveTm", StartTimeRecieveTm));

                }
                else { }

                //计划接收结束时间
                if (EndTimeRecieveTm != "" && EndTimeRecieveTm != null)
                {
                    //string EndRecieveTm = DateDiff(EndTimeRecieveTm);      //接收结束时间距离当天的天数 小
                    //strSql.Append(" and DateDiff(dd,RecieveTm,getdate()) >= " + EndRecieveTm);

                    strSql.Append(" and DateDiff(dd,RecieveTm,@EndTimeRecieveTm) >=0 ");
                    parameter.Add(DbFactory.CreateDbParameter("@EndTimeRecieveTm", EndTimeRecieveTm));
                }
                else { }
                #endregion

                //按照计划时间排序
                strSql.Append(" order by PlanTime desc");

                //DataTable dt = DataFactory.Database().FindTableBySql(strSql.ToString(), false);
                DataTable dt = DataFactory.Database().FindTableBySql(strSql.ToString(), parameter.ToArray(), false);

                #endregion



                string fileName = "总装生产小时计划";
                string excelType = "xls";
                MemoryStream ms = DeriveExcel.ExportExcel_HourProducePlan(dt, excelType);
                if (!fileName.EndsWith(".xls"))
                {
                    fileName = fileName + ".xls";
                }
                Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other, "1".ToString(), "小时计划导出成功");
                return File(ms, "application/vnd.ms-excel", Url.Encode(fileName));
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other, "-1", "小时计划导出操作失败：" + ex.Message);
                return null;
            }

        }
        #endregion

    }
}