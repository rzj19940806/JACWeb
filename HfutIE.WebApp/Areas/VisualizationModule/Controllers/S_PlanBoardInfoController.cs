using HfutIE.Business;
using HfutIE.DataAccess;
using HfutIE.Entity;
using HfutIE.Repository;
using HfutIE.Utilities;
using HfutIE.VisualizationModule;
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

namespace HfutIE.WebApp.Areas.VisualizationModule.Controllers
{
    /// <summary>
    /// _FactoryBaseInformation控制器
    /// </summary>
    public class S_PlanBoardInfoController : PublicController<S_PlanBoardInfo>
    {
        #region 创建数据库操作对象区域
        //创建数据库访问对象，用以访问其中操作数据库的方法
        S_PlanBoardInfoBll MyBll = new S_PlanBoardInfoBll(); //===复制时需要修改===
        public readonly RepositoryFactory<S_PlanBoardInfo> repository_StfBase = new RepositoryFactory<S_PlanBoardInfo>();
        #endregion
        

        #region 方法区   

        #region 1.新增编辑方法
        //entity表示页面中传入的实体，KeyValue表示传入的主键
        //不管是新增还是编辑，页面中都会传入实体（entity）
        //新增时实体是一个全新的实体
        //编辑时实体是修改后的实体
        //只有在编辑时页面中才会传入主键entity（KeyValue），该主键是需要编辑那个实体的主键
        //
        //编辑方法首先根据KeyValue是否有值判断是需要编辑还是需要新增
        //如果是新增就将该实体新增到数据库中
        //如果是编辑就将该实体更新到数据中
        //
        //不管是新增还是编辑首先判断页面输入的编号是否已经存在
        //如果已经存在就直接返回“该编号已经存在！”的信息
        //不存在再进行下一步
        public override ActionResult SubmitForm(S_PlanBoardInfo entity, string KeyValue)//===复制时需要修改===
        {
            try
            {
                #region form为null时转为空
                if (entity.Label == null)
                {
                    entity.Label = "";
                }
                else { }
                if (entity.WelcomeLabel == null)
                {
                    entity.WelcomeLabel = "";
                }
                else { }
                if (entity.fontfamily == null)
                {
                    entity.fontfamily = "";
                }
                else { }
                #endregion

                int IsOk = 0;//用于判断
                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";

                if (!string.IsNullOrEmpty(KeyValue))//编辑操作
                {
                    //===复制时需要修改===
                    S_PlanBoardInfo Oldentity = repository_StfBase.Repository().FindEntity(KeyValue);//获取没更新之前实体对象
                    entity.Modify(KeyValue);
                    IsOk = MyBll.Update(entity);//将修改后的实体更新到数据库，插入成功返回1，失败返回0；
                    this.WriteLog(IsOk, entity, Oldentity, KeyValue, Message);//记录日志                
                }
                else//新增操作
                {
                    entity.Create();
                    IsOk = MyBll.Insert(entity);//将实体插入数据库，插入成功返回1，失败返回0；
                    this.WriteLog(IsOk, entity, null, KeyValue, Message);//记录日志
                }
                return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                this.WriteLog(-1, entity, null, KeyValue, "操作失败：" + ex.Message);//记录日志
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion

        #region 2.删除方法
        /// <summary>
        /// 首先判断需要删除的信息是否绑定了其他信息
        ///     如：删除一条公司信息先要判断该条公司信息下面是否绑定了工厂信息
        ///         如果绑定了信息，则提示“当前所选有子节点数据，不能删除。”并结束
        ///  在确定没绑定数据的情况下，删除该数据
        ///     删除表示将该数据的IsAvailable属性设为false，并不真的删除该记录
        /// </summary>
        /// <param name="KeyValue">页面中提供的需要删除的数据的主键,可能是多条数据的主键，即多个主键</param>
        /// <param name="ParentId">不需要</param>
        /// <param name="DeleteMark">不需要</param>
        /// <returns></returns>
        public ActionResult DeleteWc(string KeyValue)
        {
            //不管是多个主键还是单个主键，将主键拆分出来，放在数组中
            string[] array = KeyValue.Split(',');
            try
            {
                var Message = "删除失败。";//定义返回信息，该信息将返回到界面上，给用户观看
                int IsOk = 0;//判断删除方法是否成，0表示不成功，大于0表示成功
                IsOk = MyBll.Delete(array);
                if (IsOk > 0) Message = "删除成功。";
                WriteLog(IsOk, array, Message);
                return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                WriteLog(-1, array, "操作失败：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion

        #region 3.查询方法
        //查询方法，本方法为单条件查询，即根据一个条件进行查询
        //查询条件为Condition，也是数据库表_CompanyBaseInformation中的一个字段名
        //查询值为keywords，也是数据库表_CompanyBaseInformation中的字段名的字段值
        //本查询采用近似查询（like）

        public ActionResult GridPageByCondition(string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                #region 原方法
                //string keyword = keywords.Trim();
                //Stopwatch watch = CommonHelper.TimerStart();
                //DataTable ListData = MyBll.GetPageListByCondition(keyword, Condition, jqgridparam);//===复制时需要修改===
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

                #region 查询修改
                Stopwatch watch = CommonHelper.TimerStart();
                StringBuilder strSql = new StringBuilder();
                List<DbParameter> parameter = new List<DbParameter>();
                strSql.Append(@"SELECT * FROM S_PlanBoardInfo WHERE Enabled='1' ");
                //开始时间
                if (StartTime != "" && StartTime != null)
                {
                    //string StartTime2 = DateDiff(StartTime);    //开始时间距离当天的天数 大
                    //strSql.Append(" and DateDiff(dd,Date,getdate()) <=  " + StartTime2);

                    strSql.Append(" and DateDiff(dd,@StartTime,Date) >=0 ");
                    parameter.Add(DbFactory.CreateDbParameter("@StartTime", StartTime));
                }
                else { }

                //结束时间
                if (EndTime != "" && EndTime != null)
                {
                    //string EndTime2 = DateDiff(EndTime);        //计划结束时间距离当天的天数 小
                    //strSql.Append(" and DateDiff(dd,Date,getdate()) >= " + EndTime2);

                    strSql.Append(" and DateDiff(dd,Date,@EndTime) >=0 ");
                    parameter.Add(DbFactory.CreateDbParameter("@EndTime", EndTime));
                }
                else { }
                strSql.Append(" order by Date desc,CreTm desc " );
                DataTable dt = DataFactory.Database().FindTableBySql(strSql.ToString(), parameter.ToArray(), false);
                var JsonData = new
                {
                    jqgridparam.total,
                    jqgridparam.page,
                    jqgridparam.records,
                    costtime = CommonHelper.TimerEnd(watch),
                    rows = dt,
                };
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "1", "大屏维护信息查询成功");
                return Content(JsonData.ToJson());
                #endregion


            }
            catch (Exception ex)
            {
                //CCSLog.CCSLogHelper.WriteExLog(ex, CCSLog.LogType.WebSite);
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "大屏维护信息查询发生异常错误：" + ex.Message);
                return null;
            }
        }
        #endregion

        #region 4.检查字段是否唯一
        /// <summary>
        /// 根据传入的字段名和字段值判断该字段是否存在
        /// </summary>
        /// <param name="KeyName">字段名</param>
        /// <param name="KeyValue">字段值</param>
        /// <returns>返回该判断结果</returns>
        public ActionResult ChectOnlyOne(string KeyName, string KeyValue)
        {
            try
            {
                int IsOk = 0;
                string Message = "该字段已经存在！";

                if (!string.IsNullOrEmpty(KeyValue))
                {
                    IsOk = MyBll.CheckCount(KeyName, KeyValue);
                }
                if (IsOk > 0)
                {
                    return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = Message }.ToString());
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        #endregion

        #region 5.复制一条已有记录并更新为当天信息
        public ActionResult btn_copyrecord(string KeyValue)//===复制时需要修改===
        {
            int IsOk = 0;//用于判断
            string Message = "新增成功。";
            S_PlanBoardInfo entity = repositoryfactory.Repository().FindEntity(KeyValue);
            S_PlanBoardInfo entity2 = new S_PlanBoardInfo();
            entity2 = entity;
            entity2.ID = System.Guid.NewGuid().ToString();
            entity2.CreTm = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            entity2.Date = DateTime.Now.ToString("yyyy-MM-dd");
            entity2.CreCd = ManageProvider.Provider.Current().UserId;
            entity2.CreNm = ManageProvider.Provider.Current().UserName;
            entity2.MdfTm = "";
            entity2.MdfCd = "";
            entity2.MdfNm = "";
            try
            {
                IsOk = MyBll.Insert(entity);//将实体插入数据库，插入成功返回1，失败返回0；
                this.WriteLog(IsOk, entity, null, KeyValue, Message);//记录日志
                return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                this.WriteLog(-1, entity, null, KeyValue, "操作失败：" + ex.Message);//记录日志
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion

        #region 6.加载表格
        
        public ActionResult GridPageJson1(JqGridParam jqgridparam)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append(@"select * from S_PlanBoardInfo where Enabled = 1 order by Date desc,CreTm desc ");
                DataTable dt = DataFactory.Database().FindTableBySql(strSql.ToString(), false);
                if (dt.Rows.Count > 0)
                {
                    var JsonData = new
                    {
                        rows = dt,
                    };
                    return Content(JsonData.ToJson());
                }
                return null;
            }
            catch (Exception ex)
            {
                //CCSLog.CCSLogHelper.WriteExLog(ex, CCSLog.LogType.WebSite);
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, " - 1", "异常错误：" + ex.Message);
                return null;
            }
        }
        #endregion
        
        #region 7.重构导出
        public ActionResult GetExcel_Data(string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {


                #region 根据当前搜索条件查出数据并导出
                
                StringBuilder strSql = new StringBuilder();
                List<DbParameter> parameter = new List<DbParameter>();
                strSql.Append(@"SELECT Date,DayPlanUph,DayPlanNo,MouthPlanNo,Label,Type,DayActualNo,DayOutNo,DayWherehouseNo,MouthActualNo,MouthOutNo,MouthWherehouseNo,CreTm,CreNm,MdfTm,MdfNm,Rem FROM S_PlanBoardInfo WHERE Enabled='1' ");
                //开始时间
                if (StartTime != "" && StartTime != null)
                {
                    //string StartTime2 = DateDiff(StartTime);    //开始时间距离当天的天数 大
                    //strSql.Append(" and DateDiff(dd,Date,getdate()) <=  " + StartTime2);

                    strSql.Append(" and DateDiff(dd,@StartTime,Date) >=0 ");
                    parameter.Add(DbFactory.CreateDbParameter("@StartTime", StartTime));
                }
                else { }

                //结束时间
                if (EndTime != "" && EndTime != null)
                {
                    //string EndTime2 = DateDiff(EndTime);        //计划结束时间距离当天的天数 小
                    //strSql.Append(" and DateDiff(dd,Date,getdate()) >= " + EndTime2);

                    strSql.Append(" and DateDiff(dd,Date,@EndTime) >=0 ");
                    parameter.Add(DbFactory.CreateDbParameter("@EndTime", EndTime));
                }
                else { }
                strSql.Append(" order by Date desc,CreTm desc ");
                DataTable dt = DataFactory.Database().FindTableBySql(strSql.ToString(), parameter.ToArray(), false);

                #endregion



                string fileName = "大屏生产计划维护信息";
                string excelType = "xls";
                MemoryStream ms = DeriveExcel.ExportExcel_PlanBoard(dt, excelType);
                if (!fileName.EndsWith(".xls"))
                {
                    fileName = fileName + ".xls";
                }
                Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other, "1", "大屏维护信息导出成功");
                return File(ms, "application/vnd.ms-excel", Url.Encode(fileName));
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other, "-1", "大屏维护信息导出操作失败：" + ex.Message);
                return null;
            }

        }
        #endregion

        #region 8.计算日期与今天日期的天数
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


        #endregion
    }
}