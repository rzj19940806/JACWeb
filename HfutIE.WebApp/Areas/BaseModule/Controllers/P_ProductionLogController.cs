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
    /// P_ProductionLog控制器
    /// </summary>
    public class P_ProductionLogController : PublicController<P_ProductionLog>
    {
        #region 创建数据库操作对象区域
        //创建数据库访问对象，用以访问其中操作数据库的方法
        P_ProductionLogBll MyBll = new P_ProductionLogBll(); //===复制时需要修改===
        public readonly RepositoryFactory<P_ProductionLog> repository_dt = new RepositoryFactory<P_ProductionLog>();
        #endregion

        #region 方法区   

        #region 8.查询方法
        //查询方法，本方法为单条件查询，即根据一个条件进行查询
        //查询条件为Condition，也是数据库表_CompanyBaseInformation中的一个字段名
        //查询值为keywords，也是数据库表_CompanyBaseInformation中的字段名的字段值
        //本查询采用近似查询（like）

        public ActionResult GridPageByCondition(string OPModule, string OPAction, string OPType, string OPResult, string TimeStart, string TimeEnd, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                DataTable ListData = MyBll.GetPageListByCondition(OPModule, OPAction, OPType, OPResult, TimeStart, TimeEnd, jqgridparam);//===复制时需要修改===
                var JsonData = new
                {
                    total = jqgridparam.total,
                    page = jqgridparam.page,
                    records = jqgridparam.records,
                    costtime = CommonHelper.TimerEnd(watch),
                    rows = ListData,
                };
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "1", "接口日志信息查询成功");
                return Content(ListData.ToJson());
            }
            catch (Exception ex)
            {
                //CCSLog.CCSLogHelper.WriteExLog(ex, CCSLog.LogType.WebSite);
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "接口日志信息查询发生异常错误：" + ex.Message);
                return null;
            }
        }
        #endregion
        public ActionResult RemoveLog()
        {
            return View();
        }
        public ActionResult SubmitRemoveLog(string KeepTime)
        {
            try
            {
                var Message = "清空失败";
                int IsOk = MyBll.RemoveLog(KeepTime);
                if (IsOk >= 0)
                {
                    IsOk = 1;
                    Message = "删除成功";
                }
                Base_SysLogBll.Instance.WriteLog("", OperationType.Other, IsOk.ToString(), Message + "保留"+KeepTime+"个月内数据");
                return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Other, "-1", "保留" + KeepTime + "个月内数据" + "," + "错误：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败，错误：" + ex.Message }.ToString());
            }
        }

        #region 重构导出
        public ActionResult GetExcel_Data(string OPModule, string OPAction, string OPType, string OPResult, string TimeStart, string TimeEnd, JqGridParam jqgridparam)
        {
            try
            {
                #region 根据当前搜索条件查出数据并导出

                StringBuilder strSql = new StringBuilder();
                strSql.Append(@"select OPModule,OPAction,OPObject,OPAssistObject,OPType,OPResult,OPRem,OPTime from P_ProductionLog where Enabled = 1   ");
                List<DbParameter> parameter = new List<DbParameter>();
                #region 判断输入框内容添加检索条件

                if (!String.IsNullOrEmpty(OPModule))
                {
                    strSql.Append(" and OPModule like @OPModule ");
                    parameter.Add(DbFactory.CreateDbParameter("@OPModule", "%" + OPModule + "%"));
                }
                if (!String.IsNullOrEmpty(OPAction))
                {
                    strSql.Append(" and OPAction like @OPAction ");
                    parameter.Add(DbFactory.CreateDbParameter("@OPAction", "%" + OPAction + "%"));
                }
                if (!String.IsNullOrEmpty(OPType))
                {
                    strSql.Append(" and OPType like @OPType ");
                    parameter.Add(DbFactory.CreateDbParameter("@OPType", "%" + OPType + "%"));
                }
                if (!String.IsNullOrEmpty(OPResult))
                {
                    strSql.Append(" and OPResult like @OPResult ");
                    parameter.Add(DbFactory.CreateDbParameter("@OPResult", "%" + OPResult + "%"));
                }
                //开始时间
                if (!String.IsNullOrEmpty(TimeStart))
                {
                    //开始时间把@放在前面
                    strSql.Append(" and DateDiff(day,@TimeStart,OPTime) >=0 ");
                    parameter.Add(DbFactory.CreateDbParameter("@TimeStart", TimeStart));
                }
                //结束时间
                if (!String.IsNullOrEmpty(TimeEnd))
                {
                    //结束时间把@放在后面
                    strSql.Append(" and DateDiff(day,OPTime,@TimeEnd) >=0 ");
                    parameter.Add(DbFactory.CreateDbParameter("@TimeEnd", TimeEnd));
                }


                ////预计生产开始时间
                ////开始时间
                //if (!String.IsNullOrEmpty(TimeStart))
                //{
                //    strSql.Append(@" and DATEDIFF(day,'" + TimeStart + "',OPTime) >= 0 ");
                //}
                //else { }

                ////结束时间
                //if (!String.IsNullOrEmpty(TimeEnd))
                //{
                //    strSql.Append(@" and DATEDIFF(day,OPTime,'" + TimeEnd + "') >= 0 ");
                //}
                #endregion

                //按照计划时间排序
                strSql.Append(" order by OPTime desc");

                //DataTable dt = DataFactory.Database().FindTableBySql(strSql.ToString(), false);
                DataTable dt = DataFactory.Database().FindTableBySql(strSql.ToString(), parameter.ToArray(), false);

                #endregion



                string fileName = "接口日志信息";
                string excelType = "xls";
                MemoryStream ms = DeriveExcel.ExportExcel_P_ProductionLog(dt, excelType);
                if (!fileName.EndsWith(".xls"))
                {
                    fileName = fileName + ".xls";
                }
                Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other, "1", "接口日志导出成功");
                return File(ms, "application/vnd.ms-excel", Url.Encode(fileName));
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other, "-1", "接口日志导出操作失败：" + ex.Message);
                return null;
            }

        }
        #endregion


        #endregion
    }
}