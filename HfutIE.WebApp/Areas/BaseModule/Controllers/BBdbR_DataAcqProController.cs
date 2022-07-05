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
    public class BBdbR_DataAcqProController : PublicController<BBdbR_DataAcqPro>
    {
        // GET: BaseModule/BBdbR_DataAcqPro

        #region  创建数据库操作对象区域
        BBdbR_DataAcqProBll MyBll = new BBdbR_DataAcqProBll();
        #endregion

        #region 2.高级检索
        public ActionResult GetPageListByCondition(string PlineNm, string WcNm,string SignalSource, string SignalDetail, string CurValue, string HandleSts, string HandlerCode, string ExceptionType, string TimeStart, string TimeEnd, JqGridParam jqgridparam)
        {
            try
            {
                #region 搜索原方法-多多少少有点问题
                //Stopwatch watch = CommonHelper.TimerStart();
                //DataTable ListData = MyBll.GetPageListByCondition(PlineNm, WcNm, SignalSource, SignalDetail, CurValue, HandleSts, HandlerCode, ExceptionType, TimeStart, TimeEnd, jqgridparam);
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

                #region 搜索修改
                Stopwatch watch = CommonHelper.TimerStart();
                StringBuilder strSql = new StringBuilder();
                //初始语句未加搜索条件
                strSql.Append(@" select *,CONVERT(VARCHAR(8),CONVERT(TIME,DATEADD(ss,convert(int,HandleTm),'1900-01-01'))) as  HandleTm2 from BBdbR_DataAcqPro  where Enabled = 1");

                List<DbParameter> parameter = new List<DbParameter>();
                #region 添加搜索条件
                //产线名称
                if (PlineNm != "" && PlineNm != null)
                {
                    //strSql.Append(" and PlineNm = '" + PlineNm + "' ");
                    strSql.Append(" and PlineNm = @PlineNm ");
                    parameter.Add(DbFactory.CreateDbParameter("@PlineNm", PlineNm));
                }
                else { }
                //工位名称
                if (WcNm != "" && WcNm != null)
                {
                    //strSql.Append(" and WcNm like '%" + WcNm + "%' ");
                    strSql.Append(" and WcNm like @WcNm ");
                    parameter.Add(DbFactory.CreateDbParameter("@WcNm", "%" + WcNm + "%"));
                }
                else { }
                //信号来源
                if (SignalSource != "" && SignalSource != null)
                {
                    //strSql.Append(" and SignalSource like '%" + SignalSource + "%' ");
                    strSql.Append(" and SignalSource like @SignalSource ");
                    parameter.Add(DbFactory.CreateDbParameter("@SignalSource", "%" + SignalSource + "%"));
                }
                else { }
                //信号详情
                if (SignalDetail != "" && SignalDetail != null)
                {
                    //strSql.Append(" and SignalDetail like '%" + SignalDetail + "%' ");
                    strSql.Append(" and SignalDetail like @SignalDetail ");
                    parameter.Add(DbFactory.CreateDbParameter("@SignalDetail", "%" + SignalDetail + "%"));
                }
                else { }
                //补录状态
                if (CurValue != "" && CurValue != null)
                {
                    //strSql.Append(" and CurValue like '%" + CurValue + "%' ");
                    strSql.Append(" and CurValue like @CurValue ");
                    parameter.Add(DbFactory.CreateDbParameter("@CurValue", "%" + CurValue + "%"));
                }
                else { }
                //处理状态
                if (HandleSts != "" && HandleSts != null)
                {
                    //strSql.Append(" and HandleSts like '%" + HandleSts + "%' ");
                    strSql.Append(" and HandleSts like @HandleSts ");
                    parameter.Add(DbFactory.CreateDbParameter("@HandleSts", "%" + HandleSts + "%"));
                }
                else { }
                //处理人编号
                if (HandlerCode != "" && HandlerCode != null)
                {
                    //strSql.Append(" and HandlerCode like '%" + HandlerCode + "%' ");
                    strSql.Append(" and HandlerCode like @HandlerCode ");
                    parameter.Add(DbFactory.CreateDbParameter("@HandlerCode", "%" + HandlerCode + "%"));
                }
                else { }
                //异常类型
                if (ExceptionType != "" && ExceptionType != null)
                {
                    //strSql.Append(" and ExceptionType like '%" + ExceptionType + "%' ");
                    strSql.Append(" and ExceptionType like @ExceptionType ");
                    parameter.Add(DbFactory.CreateDbParameter("@ExceptionType", "%" + ExceptionType + "%"));
                }
                else { }
                //开始时间
                if (TimeStart != "" && TimeStart != null)
                {
                    //strSql.Append(" and DateDiff(dd,AcqTm,getdate()) <= DateDiff(dd,'" + TimeStart + "',getdate()) ");
                    //开始时间把@放在前面
                    strSql.Append(" and DateDiff(dd,@TimeStart,AcqTm) >=0 ");
                    parameter.Add(DbFactory.CreateDbParameter("@TimeStart", TimeStart));
                }
                else { }
                //结束时间
                if (TimeEnd != "" && TimeEnd != null)
                {
                    //strSql.Append(" and DateDiff(dd,AcqTm,getdate()) >= DateDiff(dd,'" + TimeEnd + "',getdate()) ");
                    //结束时间把@放在后面
                    strSql.Append(" and DateDiff(dd,AcqTm,@TimeEnd) >=0 ");
                    parameter.Add(DbFactory.CreateDbParameter("@TimeEnd", TimeEnd));
                }
                else { }
                #endregion

                //排序
                strSql.Append(" order by AcqTm desc");
                DataTable dt = DataFactory.Database().FindTableBySql(strSql.ToString(), parameter.ToArray(), false);
                var JsonData = new
                {
                    jqgridparam.total,
                    jqgridparam.page,
                    jqgridparam.records,
                    costtime = CommonHelper.TimerEnd(watch),
                    rows = dt,
                };
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "1", "Andon展示信息查询成功");
                return Content(JsonData.ToJson());
                #endregion

            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "Andon展示信息查询出现异常错误：" + ex.Message);
                return null;
            }
        }
        #endregion

        #region 3.获取下拉框内容
        /// <summary>
        /// 获取产线名称
        /// </summary>
        /// <param name="jqgridparam"></param>
        /// <returns></returns>
        public ActionResult GetPlineNm(JqGridParam jqgridparam)
        {
            try
            {
                string area_key = "";
                string parentId = "";
                string sort = "";
                Stopwatch watch = CommonHelper.TimerStart();
                BBdbR_PlineBaseBll PlineBll = new BBdbR_PlineBaseBll();
                DataTable Plinedt = PlineBll.GetList(area_key, parentId, sort, ref jqgridparam);
                var JsonData = new
                {
                    total = jqgridparam.total,
                    page = jqgridparam.page,
                    records = jqgridparam.records,
                    costtime = CommonHelper.TimerEnd(watch),
                    rows = Plinedt,
                };
                return Content(Plinedt.ToJson());
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 获取工位名称
        /// </summary>
        /// <param name="jqgridparam"></param>
        /// <returns></returns>
        public ActionResult GetWcNm(JqGridParam jqgridparam)
        {
            try
            {
                string area_key = "";
                string parentId = "";
                string sort = "";
                Stopwatch watch = CommonHelper.TimerStart();
                BBdbR_WcBaseBll WcBll = new BBdbR_WcBaseBll();
                DataTable Wcdt = WcBll.GetList(area_key, parentId, sort, ref jqgridparam);
                var JsonData = new
                {
                    total = jqgridparam.total,
                    page = jqgridparam.page,
                    records = jqgridparam.records,
                    costtime = CommonHelper.TimerEnd(watch),
                    rows = Wcdt,
                };
                return Content(Wcdt.ToJson());
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 获取岗位名称
        /// </summary>
        /// <param name="jqgridparam"></param>
        /// <returns></returns>
        public ActionResult GetPostNm(JqGridParam jqgridparam)
        {
            try
            {
                string area_key = "";
                string parentId = "";
                string sort = "";
                Stopwatch watch = CommonHelper.TimerStart();
                BBdbR_PostBaseBll PostBll = new BBdbR_PostBaseBll();
                DataTable Postdt = PostBll.GetList(area_key, parentId, sort, ref jqgridparam);
                var JsonData = new
                {
                    total = jqgridparam.total,
                    page = jqgridparam.page,
                    records = jqgridparam.records,
                    costtime = CommonHelper.TimerEnd(watch),
                    rows = Postdt,
                };
                return Content(Postdt.ToJson());
            }
            catch (Exception ex)
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
                strSql.Append($"select * from BBdbR_PlineBase where Enabled=1 and PlineTyp = '生产主线' order by Sort");
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
        public ActionResult GetExcel_Data(string PlineNm, string WcNm, string SignalSource, string SignalDetail, string CurValue, string HandleSts, string HandlerCode, string ExceptionType, string TimeStart, string TimeEnd, JqGridParam jqgridparam)
        {
            try
            {
                #region 根据当前搜索条件查出数据并导出
                StringBuilder strSql = new StringBuilder();
                //初始语句未加搜索条件
                strSql.Append(@" select PlineNm,WcNm,SignalSource,SignalDetail,CurValue,HandleSts,AcqTm,EndTm,CONVERT(VARCHAR(8),CONVERT(TIME,DATEADD(ss,convert(int,HandleTm),'1900-01-01'))) as  HandleTm,HandlerCode,HandlerName,ExceptionType,ExceptionDes,HandleResult,MdfTm,MdfNm,Rem from BBdbR_DataAcqPro where Enabled = 1");

                List<DbParameter> parameter = new List<DbParameter>();
                #region 添加搜索条件
                //产线名称
                if (PlineNm != "" && PlineNm != null)
                {
                    //strSql.Append(" and PlineNm = '" + PlineNm + "' ");
                    strSql.Append(" and PlineNm = @PlineNm ");
                    parameter.Add(DbFactory.CreateDbParameter("@PlineNm", PlineNm));
                }
                else { }
                //工位名称
                if (WcNm != "" && WcNm != null)
                {
                    //strSql.Append(" and WcNm like '%" + WcNm + "%' ");
                    strSql.Append(" and WcNm like @WcNm ");
                    parameter.Add(DbFactory.CreateDbParameter("@WcNm", "%" + WcNm + "%"));
                }
                else { }
                //信号来源
                if (SignalSource != "" && SignalSource != null)
                {
                    //strSql.Append(" and SignalSource like '%" + SignalSource + "%' ");
                    strSql.Append(" and SignalSource like @SignalSource ");
                    parameter.Add(DbFactory.CreateDbParameter("@SignalSource", "%" + SignalSource + "%"));
                }
                else { }
                //信号详情
                if (SignalDetail != "" && SignalDetail != null)
                {
                    //strSql.Append(" and SignalDetail like '%" + SignalDetail + "%' ");
                    strSql.Append(" and SignalDetail like @SignalDetail ");
                    parameter.Add(DbFactory.CreateDbParameter("@SignalDetail", "%" + SignalDetail + "%"));
                }
                else { }
                //补录状态
                if (CurValue != "" && CurValue != null)
                {
                    //strSql.Append(" and CurValue like '%" + CurValue + "%' ");
                    strSql.Append(" and CurValue like @CurValue ");
                    parameter.Add(DbFactory.CreateDbParameter("@CurValue", "%" + CurValue + "%"));
                }
                else { }
                //处理状态
                if (HandleSts != "" && HandleSts != null)
                {
                    //strSql.Append(" and HandleSts like '%" + HandleSts + "%' ");
                    strSql.Append(" and HandleSts like @HandleSts ");
                    parameter.Add(DbFactory.CreateDbParameter("@HandleSts", "%" + HandleSts + "%"));
                }
                else { }
                //处理人编号
                if (HandlerCode != "" && HandlerCode != null)
                {
                    //strSql.Append(" and HandlerCode like '%" + HandlerCode + "%' ");
                    strSql.Append(" and HandlerCode like @HandlerCode ");
                    parameter.Add(DbFactory.CreateDbParameter("@HandlerCode", "%" + HandlerCode + "%"));
                }
                else { }
                //异常类型
                if (ExceptionType != "" && ExceptionType != null)
                {
                    //strSql.Append(" and ExceptionType like '%" + ExceptionType + "%' ");
                    strSql.Append(" and ExceptionType like @ExceptionType ");
                    parameter.Add(DbFactory.CreateDbParameter("@ExceptionType", "%" + ExceptionType + "%"));
                }
                else { }
                //开始时间
                if (TimeStart != "" && TimeStart != null)
                {
                    //strSql.Append(" and DateDiff(dd,AcqTm,getdate()) <= DateDiff(dd,'" + TimeStart + "',getdate()) ");
                    //开始时间把@放在前面
                    strSql.Append(" and DateDiff(dd,@TimeStart,AcqTm) >=0 ");
                    parameter.Add(DbFactory.CreateDbParameter("@TimeStart", TimeStart));
                }
                else { }
                //结束时间
                if (TimeEnd != "" && TimeEnd != null)
                {
                    //strSql.Append(" and DateDiff(dd,AcqTm,getdate()) >= DateDiff(dd,'" + TimeEnd + "',getdate()) ");
                    //结束时间把@放在后面
                    strSql.Append(" and DateDiff(dd,AcqTm,@TimeEnd) >=0 ");
                    parameter.Add(DbFactory.CreateDbParameter("@TimeEnd", TimeEnd));
                }
                else { }
                #endregion

                //排序
                strSql.Append(" order by AcqTm desc");
                DataTable dt = DataFactory.Database().FindTableBySql(strSql.ToString(), parameter.ToArray(), false);
                #endregion

                string fileName = "Andon补录数据";
                string excelType = "xls";
                MemoryStream ms = DeriveExcel.ExportExcel_DataAcqPro(dt, excelType);
                if (!fileName.EndsWith(".xls"))
                {
                    fileName = fileName + ".xls";
                }
                Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other, "1", "Andon展示数据导出成功");
                return File(ms, "application/vnd.ms-excel", Url.Encode(fileName));
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other, "-1", "Andon展示数据导出操作失败：" + ex.Message);
                return null;
            }

        }
        #endregion

    }
}