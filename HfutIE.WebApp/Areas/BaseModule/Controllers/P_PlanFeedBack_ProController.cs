using HfutIE.Business;
using HfutIE.DataAccess;
using HfutIE.Entity;
using HfutIE.Repository;
using HfutIE.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace HfutIE.WebApp.Areas.BaseModule.Controllers
{
    /// <summary>
    /// P_PlanFeedBack_Pro控制器_ckl
    /// </summary>
    public class P_PlanFeedBack_ProController : PublicController<P_PlanFeedBack_Pro>
    {
        #region 全局变量定义区
        //定义本页面主要操作的表的表名，称为主表
        string tableName = "P_PlanFeedBack_Pro";
        //public static DataTable PlanFeedBackList = new DataTable();
        #endregion

        #region 创建数据库操作对象区域
        //创建数据库访问对象，用以访问其中操作数据库的方法
        P_PlanFeedBack_ProBll MyBll = new P_PlanFeedBack_ProBll(); //===复制时需要修改===
        public readonly RepositoryFactory<P_PlanFeedBack_Pro> repository_feedbackbase = new RepositoryFactory<P_PlanFeedBack_Pro>();
        BBdbR_AVIBaseBll avibll = new BBdbR_AVIBaseBll();
        #endregion

        public virtual ActionResult Index_repair()
        {
            return View();
        }

        #region 方法区  

        #region 1.查询未反馈数据
        //查询方法，本方法为单条件查询，即根据一个条件进行查询
        //查询条件为Condition，也是数据库表_CompanyBaseInformation中的一个字段名
        //查询值为keywords，也是数据库表_CompanyBaseInformation中的字段名的字段值
        //本查询采用近似查询（like）

        public ActionResult GridPageByCondition1(string AviCd, string VIN,string MatCd, string ProducePlanCd, string ColletionTimeStart, string ColletionTimeEnd, JqGridParam jqgridparam)
        {
            try
            {
                
                Stopwatch watch = CommonHelper.TimerStart();
                DataTable PlanFeedBackList = MyBll.GetPageListByCondition1(AviCd, VIN, MatCd, ProducePlanCd, ColletionTimeStart, ColletionTimeEnd, jqgridparam);//===复制时需要修改===
                var JsonData = new
                {
                    total = jqgridparam.total,
                    page = jqgridparam.page,
                    records = jqgridparam.records,
                    costtime = CommonHelper.TimerEnd(watch),
                    rows = PlanFeedBackList,
                };
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "1", "未反馈信息查询成功");
                return Content(PlanFeedBackList.ToJson());
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "未反馈信息查询出现异常错误：" + ex.Message);
                return null;
            }
        }
        #endregion

        #region 2.查询已反馈数据
        //查询方法，本方法为单条件查询，即根据一个条件进行查询
        //查询条件为Condition，也是数据库表_CompanyBaseInformation中的一个字段名
        //查询值为keywords，也是数据库表_CompanyBaseInformation中的字段名的字段值
        //本查询采用近似查询（like）

        public ActionResult GridPageByCondition2(string AviCd, string VIN, string MatCd, string ProducePlanCd, string ColletionTimeStart, string ColletionTimeEnd, JqGridParam jqgridparam)
        {
            try
            {

                Stopwatch watch = CommonHelper.TimerStart();
                DataTable PlanFeedBackList = MyBll.GetPageListByCondition2(AviCd, VIN, MatCd, ProducePlanCd, ColletionTimeStart, ColletionTimeEnd, jqgridparam);//===复制时需要修改===
                var JsonData = new
                {
                    total = jqgridparam.total,
                    page = jqgridparam.page,
                    records = jqgridparam.records,
                    costtime = CommonHelper.TimerEnd(watch),
                    rows = PlanFeedBackList,
                };
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "1", "报工反馈信息查询成功");
                return Content(PlanFeedBackList.ToJson());
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "已反馈信息查询出现异常错误：" + ex.Message);
                return null;
            }
        }
        #endregion

        #region 2.展示待补录
        //查询方法，本方法为单条件查询，即根据一个条件进行查询
        //查询条件为Condition，也是数据库表_CompanyBaseInformation中的一个字段名
        //查询值为keywords，也是数据库表_CompanyBaseInformation中的字段名的字段值
        //本查询采用近似查询（like）

        public ActionResult GridPageAddRecord(string AviCd, string VIN, string MatCd, string ProducePlanCd, string ColletionTimeStart, string ColletionTimeEnd, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                DataTable PlanFeedBackList = MyBll.GetPageAddRecord(AviCd, VIN, MatCd, ProducePlanCd, ColletionTimeStart, ColletionTimeEnd, jqgridparam);//===复制时需要修改===
                var JsonData = new
                {
                    total = jqgridparam.total,
                    page = jqgridparam.page,
                    records = jqgridparam.records,
                    costtime = CommonHelper.TimerEnd(watch),
                    rows = PlanFeedBackList,
                };
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "1", "采集补录信息查询成功");
                return Content(PlanFeedBackList.ToJson());
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "采集补录信息查询发生异常错误：" + ex.Message);
                return null;
            }
        }
        #endregion



        #region 3.编辑填充界面数据
        //public ActionResult SetFeedbackForm(string KeyValue)
        //{
        //    try
        //    {
        //        P_PlanFeedBack_Pro ListData = MyBll.SetFeedbackInfor(KeyValue);
        //        //var JsonData = new
        //        //{
        //        //    rows = ListData,
        //        //};
        //        return Content(ListData.ToJson());
        //    }
        //    catch (Exception ex)
        //    {
        //        return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
        //    }
        //}
        #endregion

        #region 4.新增或编辑反馈时间
        
        //public override ActionResult SubmitForm(P_PlanFeedBack_Pro entity, string KeyValue)//===复制时需要修改===
        //{
        //    try
        //    {
        //        int IsOk = 0;//用于判断
        //        string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";

        //            //===复制时需要修改===
        //            P_PlanFeedBack_Pro Oldentity = MyBll.SetFeedbackInfor(KeyValue);
        //        if (!string.IsNullOrEmpty(Oldentity.FeedbackTime.ToString()))//编辑操作
        //        {
        //            entity.PlanFeedBackProId = KeyValue;//编辑保持主键不变
        //            entity.Modify(KeyValue);
        //            IsOk = MyBll.Update(entity);//将修改后的实体更新到数据库，插入成功返回1，失败返回0；
        //            if (IsOk > 0)
        //            {
        //                Message = "编辑并更新报工反馈时间成功！";
        //            }
        //            else
        //            {
        //                Message = "编辑并更新报工反馈时间失败！";
        //            }
        //            this.WriteLog(IsOk, entity, Oldentity, KeyValue, Message);//记录日志
        //        }
        //        else//新增过点反馈时间操作
        //        {
        //            entity.PlanFeedBackProId = KeyValue;//编辑保持主键不变
        //            IsOk = MyBll.addTime(entity);//将实体插入数据库，插入成功返回1，失败返回0；
        //            if (IsOk > 0)
        //            {
        //                Message = "新增并插入报工反馈时间成功！";
        //            }
        //            else
        //            {
        //                Message = "新增并插入报工反馈时间失败！";
        //            }
        //            this.WriteLog(IsOk, entity, null, KeyValue, Message);//记录日志
        //        }
        //        return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = Message }.ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        this.WriteLog(-1, entity, null, KeyValue, "操作失败：" + ex.Message);//记录日志
        //        return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
        //    }
        //}
        #endregion

        #region 5.展示工序编号下拉框
        
        //public ActionResult GetOp_CodeList()
        //{
        //    try
        //    {
                
        //        DataTable opcodetable = avibll.Getopcode();//===复制时需要修改===
                
        //        return Content(opcodetable.ToJson());
        //    }
        //    catch (Exception ex)
        //    {
        //        Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
        //        return null;
        //    }
        //}
        #endregion

        #region 1.获取AVI下拉框内容
        /// <summary>
        /// 获取AVI点名称
        /// </summary>
        /// <param name="jqgridparam"></param>
        /// <returns></returns>
        public ActionResult getAllAviNm()
        {
            try
            {

                DataTable avidt = avibll.GetAviNm();

                return Content(avidt.ToJson());
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region 6.一键补录数据
        /// <summary>
        /// 首先判断需要删除的信息是否绑定了其他信息
        ///     如：删除一条工厂信息先要判断该条公司信息下面是否绑定了车间信息
        ///         如果绑定了信息，则提示“当前所选有子节点数据，不能删除。”并结束
        ///  在确定没绑定数据的情况下，删除该数据
        ///     删除表示将该数据的Enable属性设为false，并不真的删除该记录
        /// </summary>
        /// <param name="KeyValue">页面中提供的需要删除的数据的主键,可能是多条数据的主键，即多个主键</param>
        /// <param name="ParentId">不需要</param>
        /// <param name="DeleteMark">不需要</param>
        /// <returns></returns>
        public ActionResult OneKeyRecord(string KeyValue,string Type, P_PlanFeedBack_Pro entity)
        {
            //不管是多个主键还是单个主键，将主键拆分出来，放在数组中
            string[] array = idarray;
            try
            {
                var Message = "补录失败。";//定义返回信息，该信息将返回到界面上，给用户观看
                int IsOk = 0;//判断删除方法是否成，0表示不成功，大于0表示成功  
                if (array != null && array.Length > 0)
                {
                    foreach (string item in array)
                    {
                        
                        entity.PlanFeedBackProId = item;//编辑保持主键不变
                        P_PlanFeedBack_Pro Oldentity = repository_feedbackbase.Repository().FindEntity(item);//获取没更新之前实体对象
                        entity.OP_CODE = Oldentity.OP_CODE;
                        if (Type == "0")
                        {
                            entity.FeedbackTime = DateTime.Now;
                        }
                        
                        IsOk += MyBll.addTime(entity);//更新时间且插入车身过点记录表

                    }
                }
                

                if (IsOk == array.Length) Message = "补录信息成功";
                //WriteLog(IsOk, array, Message);
                Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other, IsOk.ToString(), Message);
                return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                //WriteLog(-1, array, "操作失败：" + ex.Message);
                Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other, "-1", "采集补录信息补录操作失败：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion

        #region 批量补录时赋值
        public static string[] idarray;
        public ActionResult SetWorkId(string KeyValue)
        {
            idarray =  KeyValue.Split(',');
            return Content(new JsonMessage { Success = true, Code = 1.ToString(), Message = "" }.ToString());
        }
        #endregion

        #endregion

    }
}