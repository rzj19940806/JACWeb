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

namespace HfutIE.WebApp.Areas.BaseModule.Controllers
{
    public class Q_KeyByPass_ProController : PublicController<Q_KeyByPass_Pro>
    {
        // GET: BaseModule/Q_KeyByPass_Pro
        #region  创建数据库操作对象区域
        Q_KeyByPass_ProBll MyBll = new Q_KeyByPass_ProBll();
        #endregion

        #region 搜索
        public ActionResult GetPageListByCondition(string VIN, string OrderCd, string CarType, string ProductMatCd, string MatCd, string MatNm,string PlineNm, string WcCd, string TimeStart, string TimeEnd, JqGridParam jqgridparam)
        {
            try
            {

                //使用bll中方法
                //DataTable ListData = MyBll.GetPageListByCondition(VIN, OrderCd, CarType, ProductMatCd, MatCd, MatNm, PlineNm, WcCd, TimeStart, TimeEnd, jqgridparam);

                Stopwatch watch = CommonHelper.TimerStart();
                StringBuilder strSql = new StringBuilder();
                strSql.Append($"select a.*,b.MatNm as bMatNm from Q_KeyByPass_Pro a left join BBdbR_ProductBase b on a.ProductMatCd = b.MatCd  where a.VIN like @VIN and a.OrderCd like @OrderCd and a.CarType like @CarType and a.ProductMatCd like @ProductMatCd and a.MatCd like @MatCd and a.MatNm like @MatNm and a.PlineNm like @PlineNm and a.WcCd like @WcCd ");

                List<DbParameter> parameter = new List<DbParameter>();
                parameter.Add(DbFactory.CreateDbParameter("@VIN", "%" + VIN + "%"));
                parameter.Add(DbFactory.CreateDbParameter("@OrderCd", "%" + OrderCd + "%"));
                parameter.Add(DbFactory.CreateDbParameter("@CarType", "%" + CarType + "%"));
                parameter.Add(DbFactory.CreateDbParameter("@ProductMatCd", "%" + ProductMatCd + "%"));
                parameter.Add(DbFactory.CreateDbParameter("@MatCd", "%" + MatCd + "%"));
                parameter.Add(DbFactory.CreateDbParameter("@MatNm", "%" + MatNm + "%"));
                parameter.Add(DbFactory.CreateDbParameter("@PlineNm", "%" + PlineNm + "%"));
                parameter.Add(DbFactory.CreateDbParameter("@WcCd", "%" + WcCd + "%"));

                //开始时间
                if (TimeStart != "" && TimeStart != null)
                {
                    //string StartTime = DateDiff(TimeStart);    //开始时间距离当天的天数 大
                    //strSql.Append(" and DateDiff(dd,Datetime,getdate()) <=  " + StartTime);
                    //开始时间把@放在前面
                    strSql.Append(" and DateDiff(dd,@TimeStart,Datetime) >=0 ");
                    parameter.Add(DbFactory.CreateDbParameter("@TimeStart", TimeStart));
                }
                else { }

                //结束时间
                if (TimeEnd != "" && TimeEnd != null)
                {
                    //string EndTime = DateDiff(TimeEnd);        //结束时间距离当天的天数 小
                    //strSql.Append(" and DateDiff(dd,Datetime,getdate()) >= " + EndTime);
                    //结束时间把@放在后面
                    strSql.Append(" and DateDiff(dd,Datetime,@TimeEnd) >=0 ");
                    parameter.Add(DbFactory.CreateDbParameter("@TimeEnd", TimeEnd));
                }
                else { }
                strSql.Append(" order by Datetime desc ");
                DataTable dt = DataFactory.Database().FindTableBySql(strSql.ToString(), parameter.ToArray(), false);
                var JsonData = new
                {
                    jqgridparam.total,
                    jqgridparam.page,
                    jqgridparam.records,
                    costtime = CommonHelper.TimerEnd(watch),
                    rows = dt,
                };
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "1", "关重件Pass信息查询成功");
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "关重件Pass信息查询发生异常错误：" + ex.Message);
                return null;
            }
        }
        #endregion

        #region 3.获取下拉框内容
        /// <summary>
        /// 获取设备名称
        /// </summary>
        /// <param name="jqgridparam"></param>
        /// <returns></returns>
        //public ActionResult GetDeviceNm(JqGridParam jqgridparam)
        //{
        //    try
        //    {
        //        Stopwatch watch = CommonHelper.TimerStart();
        //        BBdbR_DvcBaseBll DvcBll = new BBdbR_DvcBaseBll();
        //        DataTable Dvcdt = DvcBll.GrtDvcNm();
        //        var JsonData = new
        //        {
        //            total = jqgridparam.total,
        //            page = jqgridparam.page,
        //            records = jqgridparam.records,
        //            costtime = CommonHelper.TimerEnd(watch),
        //            rows = Dvcdt,
        //        };
        //        return Content(Dvcdt.ToJson());
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}
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

        #region 加载产线名称下拉框
        public ActionResult GetPlineList()
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append($"select * from BBdbR_PlineBase where Enabled=1 and (PlineTyp = '生产主线' or PlineTyp = '生产辅线') order by Sort");
                DataTable dt = DataFactory.Database().FindTableBySql(strSql.ToString(), false);
                var JsonData = new
                {
                    rows = dt,
                };
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }
        }
        #endregion

        #region 重构导出
        public ActionResult GetExcel_Data(string VIN, string OrderCd, string CarType, string ProductMatCd, string MatCd, string MatNm, string PlineNm, string WcCd, string TimeStart, string TimeEnd, JqGridParam jqgridparam)
        {
            try
            {
                #region 将搜索时保存的查询到的全列全局变量datatable转为只含导出列的datatable 数据量大时耗时较长
                //Stopwatch watch = CommonHelper.TimerStart();
                //DataTable ListData = new DataTable();
                //if (dtExport.Rows.Count > 0)
                //{
                //    ListData = dtExport.DefaultView.ToTable("总装生产日计划", true, "ProducePlanCd", "OrderCd", "VIN", "MatCd", "CarType", "CarColor1", "PlanTime", "PlanStatus", "RecieveTm");//获取表中特定列
                //}
                #endregion

                #region 根据当前搜索条件查出数据并导出
                StringBuilder strSql = new StringBuilder();
                strSql.Append($"select a.VIN,b.MatNm as bMatNm,a.OrderCd,a.CarType,a.ProductMatCd,a.CarColor1,a.MatCd,a.MatNm,a.FacNm,a.WorkshopNm,a.WorkSectionNm,a.PlineNm,a.WcCd,a.StfNm,a.Datetime from Q_KeyByPass_Pro a left join BBdbR_ProductBase b on a.ProductMatCd = b.MatCd  where a.VIN like @VIN and a.OrderCd like @OrderCd and a.CarType like @CarType and a.ProductMatCd like @ProductMatCd and a.MatCd like @MatCd and a.MatNm like @MatNm and a.PlineNm like @PlineNm and a.WcCd like @WcCd ");

                List<DbParameter> parameter = new List<DbParameter>();
                parameter.Add(DbFactory.CreateDbParameter("@VIN", "%" + VIN + "%"));
                parameter.Add(DbFactory.CreateDbParameter("@OrderCd", "%" + OrderCd + "%"));
                parameter.Add(DbFactory.CreateDbParameter("@CarType", "%" + CarType + "%"));
                parameter.Add(DbFactory.CreateDbParameter("@ProductMatCd", "%" + ProductMatCd + "%"));
                parameter.Add(DbFactory.CreateDbParameter("@MatCd", "%" + MatCd + "%"));
                parameter.Add(DbFactory.CreateDbParameter("@MatNm", "%" + MatNm + "%"));
                parameter.Add(DbFactory.CreateDbParameter("@PlineNm", "%" + PlineNm + "%"));
                parameter.Add(DbFactory.CreateDbParameter("@WcCd", "%" + WcCd + "%"));

                //开始时间
                if (TimeStart != "" && TimeStart != null)
                {
                    //string StartTime = DateDiff(TimeStart);    //开始时间距离当天的天数 大
                    //strSql.Append(" and DateDiff(dd,Datetime,getdate()) <=  " + StartTime);
                    //开始时间把@放在前面
                    strSql.Append(" and DateDiff(dd,@TimeStart,Datetime) >=0 ");
                    parameter.Add(DbFactory.CreateDbParameter("@TimeStart", TimeStart));
                }
                else { }

                //结束时间
                if (TimeEnd != "" && TimeEnd != null)
                {
                    //string EndTime = DateDiff(TimeEnd);        //结束时间距离当天的天数 小
                    //strSql.Append(" and DateDiff(dd,Datetime,getdate()) >= " + EndTime);
                    //结束时间把@放在后面
                    strSql.Append(" and DateDiff(dd,Datetime,@TimeEnd) >=0 ");
                    parameter.Add(DbFactory.CreateDbParameter("@TimeEnd", TimeEnd));
                }
                else { }
                strSql.Append(" order by Datetime desc ");
                DataTable dt = DataFactory.Database().FindTableBySql(strSql.ToString(), parameter.ToArray(), false);
                #endregion



                string fileName = "关重件ByPass数据";
                string excelType = "xls";
                MemoryStream ms = DeriveExcel.ExportExcel_KeyByPass(dt, excelType);
                if (!fileName.EndsWith(".xls"))
                {
                    fileName = fileName + ".xls";
                }
                Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other, "-1", "关重件ByPass数据导出成功");
                return File(ms, "application/vnd.ms-excel", Url.Encode(fileName));
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other, "-1", "关重件ByPass数据导出操作失败：" + ex.Message);
                return null;
            }

        }
        #endregion

    }


}