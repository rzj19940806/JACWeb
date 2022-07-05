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
    public class P_CarPastRecordInfoController : PublicController<P_CarPastRecordInfo>
    {
        // GET: BaseModule/P_CarPastRecordInfo
        #region  创建数据库操作对象区域
        P_CarPastRecordInfoBll MyBll = new P_CarPastRecordInfoBll();
        #endregion

        #region 搜索
        /// <summary>
        /// 
        /// </summary>
        /// <param name="keywords">查询值</param>
        /// <param name="Condition">搜索条件</param>
        /// <param name="TimeStart">开始时间</param>
        /// <param name="TimeEnd">结束时间</param>
        /// <param name="jqgridparam">分页参数</param>
        /// <returns></returns>
        public ActionResult GridPageByCondition(string AviCd, string VIN, string CarType, string MatCd, string ProducePlanCd, string TimeStart, string TimeEnd, JqGridParam jqgridparam)
        {
            try
            {
                #region List接收查询内容
                //Stopwatch watch = CommonHelper.TimerStart();
                //List<P_CarPastRecordInfo> ListData = MyBll.GetPageListByCondition(AviCd, VIN, CarType, MatCd, ProducePlanCd, TimeStart, TimeEnd, jqgridparam);
                //var JsonData = new
                //{
                //    total = jqgridparam.total,
                //    page = jqgridparam.page,
                //    records = jqgridparam.records,
                //    costtime = CommonHelper.TimerEnd(watch),
                //    rows = ListData,
                //};
                //return Content(ListData.ToJson());
                #endregion

                #region datatable接收查询内容
                Stopwatch watch = CommonHelper.TimerStart();
                StringBuilder strSql = new StringBuilder();
                List<DbParameter> parameter = new List<DbParameter>();
                strSql.Append($"select * from P_CarPastRecordInfo where AviCd  like @AviCd and VIN like @VIN and CarType like  @CarType and MatCd like @MatCd and ProducePlanCd like @ProducePlanCd  and VIN !='9999' and (AviCd not like'PBS%' or AviCd = 'PBS-1') ");
                parameter.Add(DbFactory.CreateDbParameter("@AviCd", "%" + AviCd));
                parameter.Add(DbFactory.CreateDbParameter("@VIN", "%" + VIN));
                parameter.Add(DbFactory.CreateDbParameter("@CarType", "%" + CarType + "%"));
                parameter.Add(DbFactory.CreateDbParameter("@MatCd", "%" + MatCd + "%"));
                parameter.Add(DbFactory.CreateDbParameter("@ProducePlanCd", "%" + ProducePlanCd + "%"));

                if (TimeStart != "" && TimeStart != null)
                {
                    //strSql.Append($" and  PastTime > '{TimeStart}  00:00:00'");
                    strSql.Append(" and DateDiff(dd,@TimeStart,PastTime) >=0 ");
                    parameter.Add(DbFactory.CreateDbParameter("@TimeStart", TimeStart));
                }
                if (TimeEnd != "" && TimeEnd != null)
                {
                    //strSql.Append($" and  PastTime < '{TimeEnd} 23:59:59'");
                    strSql.Append(" and DateDiff(dd,PastTime,@TimeEnd) >=0 ");
                    parameter.Add(DbFactory.CreateDbParameter("@TimeEnd", TimeEnd));
                }
                strSql.Append($" order by {jqgridparam.sidx} {jqgridparam.sord}");
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
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "1", "AVI过点信息查询成功");
                return Content(JsonData.ToJson());
                #endregion
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "AVI过点信息查询异常错误：" + ex.Message);
                return null;
            }
        }
        #endregion

        #region 获取下拉框内容
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

        #region 重构导出
        public ActionResult GetExcel_Data(string AviCd, string VIN, string CarType, string MatCd, string ProducePlanCd, string TimeStart, string TimeEnd, JqGridParam jqgridparam)
        {
            try
            {
                #region 根据当前搜索条件查出数据并导出
                StringBuilder strSql = new StringBuilder();
                List<DbParameter> parameter = new List<DbParameter>();
                strSql.Append($"select AviCd,AviNm,CarType,MatCd,CarColor1,VIN,SequenceNo,ProducePlanCd,CarRoute,PlineCd,PlineNm,PastType,PastTime,PastNo,CreStaff,Rem  from P_CarPastRecordInfo where AviCd  like @AviCd and VIN like @VIN and CarType like @CarType and MatCd like @MatCd and ProducePlanCd like @ProducePlanCd and VIN !='9999' and (AviCd not like'PBS%' or AviCd = 'PBS-1')");
                parameter.Add(DbFactory.CreateDbParameter("@AviCd", "%" + AviCd));
                parameter.Add(DbFactory.CreateDbParameter("@VIN", "%" + VIN));
                parameter.Add(DbFactory.CreateDbParameter("@CarType", "%" + CarType + "%"));
                parameter.Add(DbFactory.CreateDbParameter("@MatCd", "%" + MatCd + "%"));
                parameter.Add(DbFactory.CreateDbParameter("@ProducePlanCd", "%" + ProducePlanCd + "%"));

                if (TimeStart != "" && TimeStart != null)
                {
                    //strSql.Append($" and  PastTime > '{TimeStart}  00:00:00'");
                    strSql.Append(" and DateDiff(dd,@TimeStart,PastTime) >=0 ");
                    parameter.Add(DbFactory.CreateDbParameter("@TimeStart", TimeStart));
                }
                if (TimeEnd != "" && TimeEnd != null)
                {
                    //strSql.Append($" and  PastTime < '{TimeEnd} 23:59:59'");
                    strSql.Append(" and DateDiff(dd,PastTime,@TimeEnd) >=0 ");
                    parameter.Add(DbFactory.CreateDbParameter("@TimeEnd", TimeEnd));
                }
                strSql.Append($"  order by  PastTime desc");
                //DataTable dt = DataFactory.Database().FindTableBySql(strSql.ToString(), false);
                DataTable dt = DataFactory.Database().FindTableBySql(strSql.ToString(), parameter.ToArray(), false);

                #endregion
                
                string fileName = "AVI过点信息";
                string excelType = "xls";
                MemoryStream ms = DeriveExcel.ExportExcel_CarPastRecord(dt, excelType);
                if (!fileName.EndsWith(".xls"))
                {
                    fileName = fileName + ".xls";
                }
                Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other,"1", "AVI过点信息导出成功");
                return File(ms, "application/vnd.ms-excel", Url.Encode(fileName));
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other, "-1", "AVI过点信息导出操作失败：" + ex.Message);
                return null;
            }

        }
        #endregion

    }
}