using HfutIE.Business;
using HfutIE.Entity;
using HfutIE.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HfutIE.WebApp.Areas.SystemManaModule.Controllers
{
    /// <summary>
    /// 系统异常日志表控制器
    /// </summary>
    public class AbnorLogManaController : PublicController<S_LogErr>
    {
        #region
        #endregion

        #region 全局变量
        #endregion

        #region 视图
        public ActionResult RemoveLog()
        {
            return View();
        }
        #endregion

        #region 操作表格
        S_LogErrBll MyBll = new S_LogErrBll();
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
        public ActionResult GetAborLogList(string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                DataTable ListData = MyBll.GetLogList(StartTime, EndTime, jqgridparam);
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

        #region 加载表格
        public ActionResult GridPageJsonErr(JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                DataTable ListData = MyBll.GetPageList();
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
            if (KeepTime == "7")//保留近一周
            {
                Remark = "保留近一周";
            }
            else if (KeepTime == "1")//保留近一个月
            {
                Remark = "保留近一个月";
            }
            else if (KeepTime == "3")//保留近三个月
            {
                Remark = "保留近三个月";
            }
            if (KeepTime == "0")//
            {
                Remark = "不保留，全部删除";
            }
            try
            {
                var Message = "清空失败。";
                int IsOk = MyBll.RemoveLog(KeepTime);
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



        #endregion


    }
}