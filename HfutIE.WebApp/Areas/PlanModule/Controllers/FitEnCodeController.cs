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
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace HfutIE.WebApp.Areas.PlanModule.Controllers
{
    /// <summary>
    /// 合装线编码器数值记录表控制器
    /// </summary>
    public class FitEnCodeController : PublicController<P_FitEnCodeInfo>
    {
        #region 全局变量定义区
        //定义本页面主要操作的表的表名，称为主表
        string tableName = "P_FitEnCodeInfo";
        public static DataTable EnCodeList = new DataTable();
        #endregion

        #region 创建数据库操作对象区域
        P_FitEnCodeInfoBll FitEnCodeInfoBll = new P_FitEnCodeInfoBll();
        #endregion
        
        #region 方法区    
        
        #region 2.展示表格
        /// <summary>
        /// 在数据库中查询相应的信息
        /// </summary>
        /// <param name="areaId">点击的节点的主键</param>
        /// <param name="parentId">节点的父级主键</param>
        /// <param name="jqgridparam">分页参数</param>
        /// <returns></returns>
        public ActionResult GetEnCodeListJson(string PlineNm, JqGridParam jqgridparam)
        {
            try
            {
                //Stopwatch watch = CommonHelper.TimerStart();
                //获取点击节点对应的数据（列表）
                EnCodeList = FitEnCodeInfoBll.GetList(PlineNm);//===复制时需要修改===
                //var JsonData = new
                //{
                //    total = jqgridparam.total,
                //    page = jqgridparam.page,
                //    records = jqgridparam.records,
                //    costtime = CommonHelper.TimerEnd(watch),
                //    rows = EnCodeList,
                //};
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "1", "关重件车身队列查询成功");
                return Content(EnCodeList.ToJson());
            }
            catch (Exception ex)
            {
                //this.WriteLog(-1, null, null, null, "操作失败" + ex.Message);
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion

        #region 3.编辑方法(不用了)
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
        public ActionResult SubmitForm1(string KeyValue, P_FitEnCodeInfo entity)//===复制时需要修改===
        {
            try
            {
                int IsOk = 0;//用于判断
                
                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";
                
                if (!string.IsNullOrEmpty(KeyValue))//编辑操作
                {
                    entity.Modify(KeyValue);
                    //===复制时需要修改===
                    P_FitEnCodeInfo Oldentity = repositoryfactory.Repository().FindEntity(KeyValue);//获取没更新之前实体对象
                    
                    IsOk = FitEnCodeInfoBll.Update(entity);//将修改后的实体更新到数据库，插入成功返回1，失败返回0；
                    //this.WriteLog(IsOk, entity, Oldentity, KeyValue, Message);//记录日志
                    
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

        #region VIN校准(不用了)
        public ActionResult VINToOk(string PlineNm)
        {
            try
            {
                //Stopwatch watch = CommonHelper.TimerStart();
                

                //获取点击节点对应的数据（列表）
                string i  = FitEnCodeInfoBll.VINToOk(PlineNm);//===复制时需要修改===
                //var JsonData = new
                //{
                //    total = jqgridparam.total,
                //    page = jqgridparam.page,
                //    records = jqgridparam.records,
                //    costtime = CommonHelper.TimerEnd(watch),
                //    rows = EnCodeList,
                //};
                if (i== "success")
                {
                    Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other, "1", "VIIN校准成功");
                    return Content(new JsonMessage { Success = true, Code = "1", Message = "操作成功" }.ToString());
                }
                else
                {
                    Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other, "-1", "VIIN校准失败");
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = i }.ToString());
                }
                
                
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other, "-1", "操作失败：" + ex.Message);
                //this.WriteLog(-1, null, null, null, "操作失败" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion

        #region 6.检查字段是否唯一
        ///// <summary>
        ///// 根据传入的字段名和字段值判断该字段是否存在
        ///// </summary>
        ///// <param name="KeyName">字段名</param>
        ///// <param name="KeyValue">字段值</param>
        ///// <returns>返回该判断结果</returns>
        //public ActionResult ChectOnlyOne(string KeyName, string KeyValue)
        //{
        //    try
        //    {
        //        int IsOk = 0;
        //        string Message = "该字段已经存在！";

        //        if (!string.IsNullOrEmpty(KeyValue))
        //        {
        //            IsOk = FitEnCodeInfoBll.CheckCount(tableName, KeyName, KeyValue);
        //        }
        //        if (IsOk > 0)
        //        {
        //            return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = Message }.ToString());
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
        //    }
        //}
        #endregion

        #region 7.编辑填充界面数据
        /// <summary>
        /// 填充界面数据
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        //public override ActionResult SetForm(string KeyValue)
        //{
        //    try
        //    {
        //        DataTable dt = FitEnCodeInfoBll.GetDepartmentBase(KeyValue);
        //        P_FitEnCodeInfo entity = new P_FitEnCodeInfo();
        //        entity.DepartmentID = dt.Rows[0]["DepartmentID"].ToString();
        //        entity.CompanyId = dt.Rows[0]["CompanyId"].ToString();
        //        entity.DepartmentCode = dt.Rows[0]["DepartmentCode"].ToString();
        //        entity.DepartmentName = dt.Rows[0]["DepartmentName"].ToString();
        //        entity.ParentDepartmentID = dt.Rows[0]["ParentDepartmentID"].ToString();
        //        entity.DepartmentType = dt.Rows[0]["DepartmentType"].ToString();
        //        entity.StfId = dt.Rows[0]["StfId"].ToString();
        //        entity.StfCd = dt.Rows[0]["StfCd"].ToString();
        //        entity.StfNm = dt.Rows[0]["StfNm"].ToString();
        //        entity.Phn = dt.Rows[0]["Phn"].ToString();
        //        entity.DepartmentDescription = dt.Rows[0]["DepartmentDescription"].ToString();
        //        entity.VersionNumber = dt.Rows[0]["VersionNumber"].ToString();
        //        entity.Enabled = int.Parse(dt.Rows[0]["Enabled"].ToString());
        //        entity.Rem = dt.Rows[0]["Rem"].ToString();
        //        return Content(entity.ToJson());
        //    }
        //    catch (Exception ex)
        //    {
        //        return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
        //    }
        //}
        #endregion

        #region 3.删除方法
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
        public override ActionResult Delete(string KeyValue, string ParentId, string DeleteMark)
        {
            //不管是多个主键还是单个主键，将主键拆分出来，放在数组中
            string[] array = KeyValue.Split(',');
            try
            {
                var Message = "删除失败。";//定义返回信息，该信息将返回到界面上，给用户观看
                int IsOk = 0;//判断删除方法是否成，0表示不成功，大于0表示成功                  
                if (array != null && array.Length > 0)
                {
                    IsOk = FitEnCodeInfoBll.Delete(array);
                }
                if (IsOk > 0)//表示删除成
                {
                    Message = "删除成功。";//修改返回信息
                }
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
        #endregion

    }
}