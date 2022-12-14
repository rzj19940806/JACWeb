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
    public class Q_KeyPartsBind_ProController : PublicController<Q_KeyPartsBind_Pro>
    {
        // GET: BaseModule/Q_KeyPartsBind_Pro
        public ActionResult ECIndex()
        {
            return View();
        }

        #region  创建数据库操作对象区域
        Q_KeyPartsBind_ProBll MyBll = new Q_KeyPartsBind_ProBll();
        #endregion

        #region 搜索
        public ActionResult GetPageListByCondition(string VIN, string OrderCd, string CarType, string ProductMatCd, string MatCd, string MatNm, string SupplierCd, string PlineNm, string WcCd,string BarCode, string TimeStart, string TimeEnd, JqGridParam jqgridparam)
        {
            try
            {
                #region 使用bll中搜索方法
                //Stopwatch watch = CommonHelper.TimerStart();
                //DataTable ListData = MyBll.GetPageListByCondition(VIN, OrderCd, CarType, ProductMatCd, MatCd, MatNm, SupplierCd, PlineNm, WcCd, TimeStart, TimeEnd, jqgridparam);
                #endregion

                Stopwatch watch = CommonHelper.TimerStart();
                StringBuilder strSql = new StringBuilder();

                strSql.Append($"select a.*,b.MatNm as bMatNm from Q_KeyPartsBind_Pro a left join BBdbR_ProductBase b on a.ProductMatCd = b.MatCd  where a.VIN like @VIN and a.OrderCd like @OrderCd and a.CarType like @CarType and a.ProductMatCd like @ProductMatCd and a.MatCd like @MatCd and a.MatNm like @MatNm and a.SupplierCd like @SupplierCd and a.PlineNm like @PlineNm and a.WcCd like @WcCd and a.BarCode like @BarCode  ");

                List<DbParameter> parameter = new List<DbParameter>();
                parameter.Add(DbFactory.CreateDbParameter("@VIN", "%" + VIN + "%"));
                parameter.Add(DbFactory.CreateDbParameter("@OrderCd", "%" + OrderCd + "%"));
                parameter.Add(DbFactory.CreateDbParameter("@CarType", "%" + CarType + "%"));
                parameter.Add(DbFactory.CreateDbParameter("@ProductMatCd", "%" + ProductMatCd + "%"));
                parameter.Add(DbFactory.CreateDbParameter("@MatCd", "%" + MatCd + "%"));
                parameter.Add(DbFactory.CreateDbParameter("@MatNm", "%" + MatNm + "%"));
                parameter.Add(DbFactory.CreateDbParameter("@SupplierCd", "%" + SupplierCd + "%"));
                parameter.Add(DbFactory.CreateDbParameter("@PlineNm", "%" + PlineNm + "%"));
                parameter.Add(DbFactory.CreateDbParameter("@WcCd", "%" + WcCd + "%"));
                parameter.Add(DbFactory.CreateDbParameter("@BarCode", "%" + BarCode + "%"));
                //开始时间
                if (TimeStart != "" && TimeStart != null)
                {
                    //string StartTime = DateDiff(TimeStart);    //开始时间距离当天的天数 大
                    //strSql.Append(" and DateDiff(dd,Datetime,getdate()) <=  " + StartTime);
                    //开始时间把@放在前面
                    strSql.Append(" and DateDiff(dd,@TimeStart,Datetime) >=0 ");
                    parameter.Add(DbFactory.CreateDbParameter("@TimeStart", TimeStart));
                }

                //结束时间
                if (TimeEnd != "" && TimeEnd != null)
                {
                    //string EndTime = DateDiff(TimeEnd);        //计划结束时间距离当天的天数 小
                    //strSql.Append(" and DateDiff(dd,Datetime,getdate()) >= " + EndTime);
                    //结束时间把@放在后面
                    strSql.Append(" and DateDiff(dd,Datetime,@TimeEnd) >=0 ");
                    parameter.Add(DbFactory.CreateDbParameter("@TimeEnd", TimeEnd));
                }
                strSql.Append("  order by Datetime desc ");
                DataTable ListData = DataFactory.Database().FindTableBySql(strSql.ToString(), parameter.ToArray(), false);

                var JsonData = new
                {
                    jqgridparam.total,
                    jqgridparam.page,
                    jqgridparam.records,
                    costtime = CommonHelper.TimerEnd(watch),
                    rows = ListData,
                };
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "1", "关重件录入信息查询成功");
                return Content(ListData.ToJson());
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "关重件录入信息查询发生异常错误：" + ex.Message);
                return null;
            }
        }
        #endregion

        #region 重构导出
        public ActionResult GetExcel_Data(string VIN, string OrderCd, string CarType, string ProductMatCd, string MatCd, string MatNm, string SupplierCd, string PlineNm, string WcCd, string TimeStart, string TimeEnd,string BarCode, JqGridParam jqgridparam)
        {
            try
            {

                #region 根据当前搜索条件查出数据并导出

                   StringBuilder strSql = new StringBuilder();

                    strSql.Append($"select a.VIN,b.MatNm as bMatNm,a.OrderCd,a.CarType,a.ProductMatCd,a.CarColor1,a.BarCode,a.RsvFld1,a.SupplierCd,a.SupplierNm,a.MatCd,a.MatNm,a.FacNm,a.WorkshopNm,a.WorkSectionNm,a.PlineNm,a.WcCd,a.StfNm,a.Datetime from Q_KeyPartsBind_Pro a left join BBdbR_ProductBase b on a.ProductMatCd = b.MatCd  where a.VIN like @VIN and a.OrderCd like @OrderCd and a.CarType like @CarType and a.ProductMatCd like @ProductMatCd and a.MatCd like @MatCd and a.MatNm like @MatNm and a.SupplierCd like @SupplierCd and a.PlineNm like @PlineNm and a.WcCd like @WcCd and a.BarCode like @BarCode ");

                    List<DbParameter> parameter = new List<DbParameter>();
                    parameter.Add(DbFactory.CreateDbParameter("@VIN", "%" + VIN + "%"));
                    parameter.Add(DbFactory.CreateDbParameter("@OrderCd", "%" + OrderCd + "%"));
                    parameter.Add(DbFactory.CreateDbParameter("@CarType", "%" + CarType + "%"));
                    parameter.Add(DbFactory.CreateDbParameter("@ProductMatCd", "%" + ProductMatCd + "%"));
                    parameter.Add(DbFactory.CreateDbParameter("@MatCd", "%" + MatCd + "%"));
                    parameter.Add(DbFactory.CreateDbParameter("@MatNm", "%" + MatNm + "%"));
                    parameter.Add(DbFactory.CreateDbParameter("@SupplierCd", "%" + SupplierCd + "%"));
                    parameter.Add(DbFactory.CreateDbParameter("@PlineNm", "%" + PlineNm + "%"));
                    parameter.Add(DbFactory.CreateDbParameter("@WcCd", "%" + WcCd + "%"));
                    parameter.Add(DbFactory.CreateDbParameter("@BarCode", "%" + BarCode + "%"));
                    //开始时间
                    if (TimeStart != "" && TimeStart != null)
                    {
                        strSql.Append(" and DateDiff(dd,@TimeStart,Datetime) >=0 ");
                        parameter.Add(DbFactory.CreateDbParameter("@TimeStart", TimeStart));
                    }

                    //结束时间
                    if (TimeEnd != "" && TimeEnd != null)
                    {
                        strSql.Append(" and DateDiff(dd,Datetime,@TimeEnd) >=0 ");
                        parameter.Add(DbFactory.CreateDbParameter("@TimeEnd", TimeEnd));
                    }
                    strSql.Append("  order by Datetime desc ");
                    DataTable dt = DataFactory.Database().FindTableBySql(strSql.ToString(), parameter.ToArray(), false);
                 
                #endregion
                // DataTable dt = MyBll.GetPageListByCondition(VIN, OrderCd, CarType, ProductMatCd, MatCd, MatNm, SupplierCd, PlineNm, WcCd, TimeStart, TimeEnd, BarCode, jqgridparam);
                //DataTable dt = MyBll.GetPageListByCondition(VIN, OrderCd, CarType, ProductMatCd, MatCd, MatNm, SupplierCd, PlineNm, WcCd, TimeStart, TimeEnd, jqgridparam );
                string fileName = "关重件录入数据";
                string excelType = "xls";
                MemoryStream ms = DeriveExcel.ExportExcel_KeyPartsBind(dt, excelType);
                if (!fileName.EndsWith(".xls"))
                {
                    fileName = fileName + ".xls";
                }
                Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other, "1", "关重件录入入数据导出成功");
                return File(ms, "application/vnd.ms-excel", Url.Encode(fileName));
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other, "-1", "关重件录入数据导出操作失败：" + ex.Message);
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

        public ActionResult GetECPageList(string VIN, string OrderCd, string ProducePlanCd,string MatCd, string CarType, string TimeStart, string TimeEnd, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = new Stopwatch();
                watch.Start();
                string sql = $"select A.VIN,A.Code,A.Tm,B.ProducePlanCd,B.OrderCd,B.MatCd,B.CarType from P_EnvironmentalCode A with(nolock) join P_PublishPlan_Pro B with(nolock) on A.VIN = B.VIN  and A.Enabled<>'0' and B.Enabled='1' where A.VIN like @VIN  and ProducePlanCd like @ProducePlanCd and OrderCd like @OrderCd and MatCd like @MatCd and CarType like @CarType and Tm=(select MAX(Tm) from P_EnvironmentalCode C with(nolock) where C.VIN=A.VIN) ";

                List<DbParameter> parameter = new List<DbParameter>();
                parameter.Add(DbFactory.CreateDbParameter("@VIN", "%" + VIN + "%"));
                parameter.Add(DbFactory.CreateDbParameter("@ProducePlanCd", "%" + ProducePlanCd + "%"));
                parameter.Add(DbFactory.CreateDbParameter("@OrderCd", "%" + OrderCd + "%"));
                parameter.Add(DbFactory.CreateDbParameter("@MatCd", "%" + MatCd + "%"));
                parameter.Add(DbFactory.CreateDbParameter("@CarType", "%" + CarType + "%"));

                //开始时间
                if (TimeStart != "" && TimeStart != null)
                {
                    //sql += $" and DATEDIFF(DD, '{TimeStart}', Tm) >= 0";
                    //开始时间把@放在前面
                    sql += " and DateDiff(dd,@TimeStart,Tm) >=0 ";
                    parameter.Add(DbFactory.CreateDbParameter("@TimeStart", TimeStart));
                }

                //结束时间
                if (TimeEnd != "" && TimeEnd != null)
                {
                    //sql += $" and DATEDIFF(DD, Tm, '{TimeEnd}') >= 0";
                    //结束时间把@放在后面
                    sql += " and DateDiff(dd,Tm,@TimeEnd) >=0 ";
                    parameter.Add(DbFactory.CreateDbParameter("@TimeEnd", TimeEnd));
                }

                sql += $" order by tm desc";
                DataTable ListData = DataFactory.Database().FindTableBySql(sql, parameter.ToArray(), false);
                #region 原方法
                //DataTable ListData = DataFactory.Database().FindTableBySql($"select A.VIN,A.Code,A.Tm,B.ProducePlanCd,B.OrderCd,B.MatCd,B.CarType from P_EnvironmentalCode A with(nolock) join P_PublishPlan_Pro B with(nolock) on A.VIN = B.VIN  and A.Enabled<>'0' and B.Enabled='1' where A.VIN like '%{VIN}%' and DATEDIFF(DD, '{TimeStart}', Tm) >= 0 and DATEDIFF(DD, Tm, '{TimeEnd}') >= 0 and ProducePlanCd like '%{ProducePlanCd}%' and OrderCd like '%{OrderCd}%' and MatCd like '%{MatCd}%' and CarType like '%{CarType}%' and Tm=(select MAX(Tm) from P_EnvironmentalCode C with(nolock) where C.VIN=A.VIN) order by tm desc", false);
                #endregion
                var JsonData = new
                {
                    total = jqgridparam.total,
                    page = jqgridparam.page,
                    records = jqgridparam.records,
                    costtime = CommonHelper.TimerEnd(watch),
                    rows = ListData,
                };
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "1", "环保码信息查询成功");
                return Content(ListData.ToJson());
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "环保码信息查询发生异常错误：" + ex.Message);
                return null;
            }
        }


        #region 环保件导出
        public ActionResult GetExcel_Data2(string VIN, string OrderCd, string ProducePlanCd, string MatCd, string CarType, string TimeStart, string TimeEnd, JqGridParam jqgridparam)
        {
            try
            {

                #region 根据当前搜索条件查出数据并导出
                /*
             

                StringBuilder strSql = new StringBuilder();

                strSql.Append($"select a.VIN,b.MatNm as bMatNm,a.OrderCd,a.CarType,a.ProductMatCd,a.CarColor1,a.BarCode,a.RsvFld1,a.SupplierCd,a.SupplierNm,a.MatCd,a.MatNm,a.FacNm,a.WorkshopNm,a.WorkSectionNm,a.PlineNm,a.WcCd,a.StfNm,a.Datetime from Q_KeyPartsBind_Pro a left join BBdbR_ProductBase b on a.ProductMatCd = b.MatCd  where a.VIN like @VIN and a.OrderCd like @OrderCd and a.CarType like @CarType and a.ProductMatCd like @ProductMatCd and a.MatCd like @MatCd and a.MatNm like @MatNm and a.SupplierCd like @SupplierCd and a.PlineNm like @PlineNm and a.WcCd like @WcCd and a.BarCode like @BarCode ");

                List<DbParameter> parameter = new List<DbParameter>();
                parameter.Add(DbFactory.CreateDbParameter("@VIN", "%" + VIN + "%"));
                parameter.Add(DbFactory.CreateDbParameter("@OrderCd", "%" + OrderCd + "%"));
                parameter.Add(DbFactory.CreateDbParameter("@CarType", "%" + CarType + "%"));
              //  parameter.Add(DbFactory.CreateDbParameter("@ProductMatCd", "%" + ProductMatCd + "%"));
                parameter.Add(DbFactory.CreateDbParameter("@MatCd", "%" + MatCd + "%"));
               // parameter.Add(DbFactory.CreateDbParameter("@MatNm", "%" + MatNm + "%"));
               // parameter.Add(DbFactory.CreateDbParameter("@SupplierCd", "%" + SupplierCd + "%"));
               // parameter.Add(DbFactory.CreateDbParameter("@PlineNm", "%" + PlineNm + "%"));
               // parameter.Add(DbFactory.CreateDbParameter("@WcCd", "%" + WcCd + "%"));
               // parameter.Add(DbFactory.CreateDbParameter("@BarCode", "%" + BarCode + "%"));
                //开始时间
                if (TimeStart != "" && TimeStart != null)
                {
                    strSql.Append(" and DateDiff(dd,@TimeStart,Datetime) >=0 ");
                    parameter.Add(DbFactory.CreateDbParameter("@TimeStart", TimeStart));
                }

                //结束时间
                if (TimeEnd != "" && TimeEnd != null)
                {
                    strSql.Append(" and DateDiff(dd,Datetime,@TimeEnd) >=0 ");
                    parameter.Add(DbFactory.CreateDbParameter("@TimeEnd", TimeEnd));
                }
                strSql.Append("  order by Datetime desc ");
                DataTable dt = DataFactory.Database().FindTableBySql(strSql.ToString(), parameter.ToArray(), false);
                 */
                #endregion

                // DataTable dt = MyBll.GetPageListByCondition(VIN, OrderCd, CarType, ProductMatCd, MatCd, MatNm, SupplierCd, PlineNm, WcCd, TimeStart, TimeEnd, BarCode, jqgridparam);
                //DataTable dt = MyBll.GetPageListByCondition(VIN, OrderCd, CarType, ProductMatCd, MatCd, MatNm, SupplierCd, PlineNm, WcCd, TimeStart, TimeEnd, jqgridparam );
                // DataTable dt = MyBll.GetECPageList(VIN,  OrderCd,  ProducePlanCd,  MatCd,  CarType,  TimeStart,  TimeEnd,  jqgridparam);

                string sql = $"select A.VIN,A.Code,A.Tm,B.ProducePlanCd,B.OrderCd,B.MatCd,B.CarType from P_EnvironmentalCode A with(nolock) join P_PublishPlan_Pro B with(nolock) on A.VIN = B.VIN  and A.Enabled<>'0' and B.Enabled='1' where A.VIN like @VIN  and ProducePlanCd like @ProducePlanCd and OrderCd like @OrderCd and MatCd like @MatCd and CarType like @CarType and Tm=(select MAX(Tm) from P_EnvironmentalCode C with(nolock) where C.VIN=A.VIN) ";

                List<DbParameter> parameter = new List<DbParameter>();
                parameter.Add(DbFactory.CreateDbParameter("@VIN", "%" + VIN + "%"));
                parameter.Add(DbFactory.CreateDbParameter("@ProducePlanCd", "%" + ProducePlanCd + "%"));
                parameter.Add(DbFactory.CreateDbParameter("@OrderCd", "%" + OrderCd + "%"));
                parameter.Add(DbFactory.CreateDbParameter("@MatCd", "%" + MatCd + "%"));
                parameter.Add(DbFactory.CreateDbParameter("@CarType", "%" + CarType + "%"));

                //开始时间
                if (TimeStart != "" && TimeStart != null)
                {
                    //sql += $" and DATEDIFF(DD, '{TimeStart}', Tm) >= 0";
                    //开始时间把@放在前面
                    sql += " and DateDiff(dd,@TimeStart,Tm) >=0 ";
                    parameter.Add(DbFactory.CreateDbParameter("@TimeStart", TimeStart));
                }

                //结束时间
                if (TimeEnd != "" && TimeEnd != null)
                {
                    //sql += $" and DATEDIFF(DD, Tm, '{TimeEnd}') >= 0";
                    //结束时间把@放在后面
                    sql += " and DateDiff(dd,Tm,@TimeEnd) >=0 ";
                    parameter.Add(DbFactory.CreateDbParameter("@TimeEnd", TimeEnd));
                }

                sql += $" order by tm desc";
                DataTable dt = DataFactory.Database().FindTableBySql(sql, parameter.ToArray(), false);

                string fileName = "环保件录入数据";
                string excelType = "xls";
                MemoryStream ms = DeriveExcel.ExportExcel_KeyPartsBind2(dt, excelType);
                if (!fileName.EndsWith(".xls"))
                {
                    fileName = fileName + ".xls";
                }
                Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other, "1", "环保件录入入数据导出成功");
                return File(ms, "application/vnd.ms-excel", Url.Encode(fileName));
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other, "-1", "环保件录入数据导出操作失败：" + ex.Message);
                return null;
            }

        }
        #endregion

    }
}
 