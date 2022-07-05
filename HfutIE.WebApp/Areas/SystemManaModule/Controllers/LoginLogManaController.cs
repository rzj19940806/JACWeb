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

namespace HfutIE.WebApp.Areas.SystemManaModule.Controllers
{
    /// <summary>
    /// 操作日志表控制器
    /// </summary>
    public class LoginLogManaController : PublicController<S_SysLog>
    {
        #region
        #endregion

        #region 操作表格
        S_SysLogBll MyBll = new S_SysLogBll();
        Base_SysLogBll base_syslogbll = new Base_SysLogBll();
        #endregion

        #region 全局变量

        #endregion

        #region 打开视图
        public ActionResult LoginDetail()
        {
            return View();
        }

        public ActionResult RemoveLog()
        {
            return View();
        }
        public ActionResult DeriveDialog()
        {
            return View();
        }
        #endregion

        #region 方法区

        #region 搜索日志列表（返回Json）
        /// <summary>
        /// 订单列表（返回Json）
        /// </summary>
        /// <param name="BillNo">制单编号</param>
        /// <param name="StartTime">制单开始时间</param>
        /// <param name="EndTime">制单结束时间</param>
        /// <param name="jqgridparam">分页参数</param>
        /// <returns></returns>
        public ActionResult GetLogList(string CreateUserName ,string IPAddress,string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                DataTable ListData = base_syslogbll.GetLogList(CreateUserName, IPAddress,StartTime, EndTime,ref jqgridparam);                
                var JsonData = new
                {
                    total = jqgridparam.total,
                    page = jqgridparam.page,
                    records = jqgridparam.records,
                    costtime = CommonHelper.TimerEnd(watch),
                    rows = ListData,
                };
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "1", "登录日志信息查询成功");
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "登录日志信息查询发生异常错误：" + ex.Message);
                return null;
            }
        }

        public ActionResult GridLogInfor(string condition, string keywords, string ParameterJson, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                DataTable ListData = MyBll.SelectLogIn(condition, keywords, ParameterJson, jqgridparam);             
                var JsonData = new
                {
                    total = jqgridparam.total,
                    page = jqgridparam.page,
                    records = jqgridparam.records,
                    costtime = CommonHelper.TimerEnd(watch),
                    rows = ListData,
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

        #region 加载日志
        ///// <summary>
        ///// 【系统日志】返回系统日志列表JSON
        ///// </summary>
        ///// <param name="ModuleId">模块ID</param>
        ///// <param name="ParameterJson">搜索条件</param>
        ///// <param name="jqgridparam">表格参数</param>
        ///// <returns></returns>
        //public ActionResult GridPageListJson(string ModuleId, string ParameterJson, JqGridParam jqgridparam)
        //{
        //    try
        //    {
        //        Stopwatch watch = CommonHelper.TimerStart();
        //        DataTable ListData = base_syslogbll.GetLoginList(ref jqgridparam);
        //        var JsonData = new
        //        {
        //            total = jqgridparam.total,
        //            page = jqgridparam.page,
        //            records = jqgridparam.records,
        //            costtime = CommonHelper.TimerEnd(watch),
        //            rows = ListData,
        //        };
        //        return Content(JsonData.ToJson());
        //    }
        //    catch (Exception ex)
        //    {
        //        Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
        //        return null;
        //    }
        //}
        #endregion

        #region 加载日志详细信息
        /// <summary>
        /// 【系统日志明细】返回表格JSON
        /// </summary>
        /// <param name="SysLogId">主键值</param>
        /// <returns></returns>
        public ActionResult GetSysLogDetailJson(string SysLogId, JqGridParam jqgridparam)
        {          
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                DataTable ListData = MyBll.GetSysLogDetailList(SysLogId);
                var JsonData = new
                {
                    total = jqgridparam.total,
                    page = jqgridparam.page,
                    records = jqgridparam.records,
                    costtime = CommonHelper.TimerEnd(watch),
                    rows = ListData,
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

        #region 清空日志
        /// <summary>
        /// 【清空日志】 表单提交事件
        /// </summary>
        /// <param name="KeepTime">日志保留时间</param>
        public ActionResult SubmitRemoveLog(string KeepTime)
        {
            string Remark = "";
            if (KeepTime == "6")//保留近半年
            {
                Remark = "保留近半年";
            }
            else if (KeepTime == "12")//保留近一年
            {
                Remark = "保留近一年";
            }
            else if (KeepTime == "36")//保留近三年
            {
                Remark = "保留近三年";
            }
            else if (KeepTime == "60")//保留近五年
            {
                Remark = "保留近五年";
            }
            else if (KeepTime == "0")//不保留，全部删除
            {
                Remark = "不保留，全部删除";
            }

            try
            {
                var Message = "清空失败。";
                int IsOk = base_syslogbll.RemoveLoginLog(KeepTime);
                if (IsOk >= 0)
                {
                    IsOk = 1;
                    Message = "删除成功。";
                }
                Base_SysLogBll.Instance.WriteLog("", OperationType.Other, IsOk.ToString(), Message + Remark);
                return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Other, "-1", Remark + "," + "错误：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败，错误：" + ex.Message }.ToString());
            }
        }
        #endregion

        #region 重构导出
        public ActionResult GetExcel_Data(string CreateUserName, string IPAddress, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                #region 根据当前搜索条件查出数据并导出

                StringBuilder strSql = new StringBuilder();
                strSql.Append(@"SELECT CreateDate,LogType,IPAddress,CreateUserName,Status,Remark FROM Base_SysLog WHERE LogType = '0' ");

                #region 判断输入框内容添加检索条件
                List<DbParameter> parameter = new List<DbParameter>();
                if (!string.IsNullOrEmpty(CreateUserName))
                {
                    strSql.Append(" AND CreateUserName LIKE @CreateUserName");
                    parameter.Add(DbFactory.CreateDbParameter("@CreateUserName", "%" + CreateUserName + "%"));
                }
                if (!string.IsNullOrEmpty(IPAddress))
                {
                    strSql.Append(" AND IPAddress LIKE @IPAddress");
                    parameter.Add(DbFactory.CreateDbParameter("@IPAddress", "%" + IPAddress + "%"));
                }
                if (!string.IsNullOrEmpty(StartTime))
                {
                    parameter.Add(DbFactory.CreateDbParameter("@StartTime", CommonHelper.GetDateTime(StartTime)));
                    strSql.Append(" AND DATEDIFF(ss, @StartTime,CreateDate)>=0 ");
                }
                if (!string.IsNullOrEmpty(EndTime))
                {

                    parameter.Add(DbFactory.CreateDbParameter("@EndTime", CommonHelper.GetDateTime(EndTime)));
                    strSql.Append(" AND DATEDIFF(ss, @EndTime,CreateDate)<=0 ");
                }

                //if (!string.IsNullOrEmpty(CreateUserName))
                //{
                //    strSql.Append(" AND CreateUserName LIKE '%" + CreateUserName + "%'");
                //}
                //if (!string.IsNullOrEmpty(IPAddress))
                //{
                //    strSql.Append(" AND IPAddress LIKE '%" + IPAddress + "%'");
                //}
                //if (!string.IsNullOrEmpty(StartTime))
                //{
                //    strSql.Append(" AND CreateDate >= '" + StartTime + "' ");
                //}
                //if (!string.IsNullOrEmpty(EndTime))
                //{
                //    strSql.Append(" AND CreateDate <=  '" + EndTime + "' ");
                //}
                #endregion

                //按照计划时间排序
                strSql.Append(" order by CreateDate desc");

                //DataTable dt = DataFactory.Database().FindTableBySql(strSql.ToString(), false);
                DataTable dt = DataFactory.Database().FindTableBySql(strSql.ToString(), parameter.ToArray(), false);
                #endregion



                string fileName = "登录日志";
                string excelType = "xls";
                MemoryStream ms = DeriveExcel.ExportExcel_LoginLogMana(dt, excelType);
                if (!fileName.EndsWith(".xls"))
                {
                    fileName = fileName + ".xls";
                }
                Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other, "1", "登录日志导出成功");
                return File(ms, "application/vnd.ms-excel", Url.Encode(fileName));
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other, "-1", "登录日志导出操作失败：" + ex.Message);
                return null;
            }

        }
        #endregion

        #endregion
    }
}