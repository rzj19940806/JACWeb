﻿using HfutIE.Business;
using HfutIE.Entity;
using HfutIE.Repository;
using HfutIE.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HfutIE.WebApp.Areas.BaseModule.Controllers
{
    /// <summary>
    /// 调度规则基本信息表控制器
    /// </summary>
    public class BBdbR_SchedulRuleBaseController : PublicController<BBdbR_SchedulRuleBase>
    {
        #region 全局变量定义区

        //定义本页面主要操作的表的表名，称为主表
        string tableName = "BBdbR_SchedulRuleBase";//===复制时需要修改===
        public readonly RepositoryFactory<BBdbR_SchedulRuleBase> repository_schedulrule = new RepositoryFactory<BBdbR_SchedulRuleBase>();//?
        #endregion

        #region 创建数据库操作对象区域
        BBdbR_SchedulRuleBaseBll MyBll = new BBdbR_SchedulRuleBaseBll(); //===复制时需要修改===
        #endregion

        #region 打开视图区域

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
        public override ActionResult SubmitForm(BBdbR_SchedulRuleBase entity, string KeyValue)//===复制时需要修改===
        {
            try
            {
                int IsOk = 0;//用于判断
                string Name = "SchedulRuleCd";        //页面中的编号字段名，如：公司编号   //===复制时需要修改===
                string Value = entity.SchedulRuleCd;  //页面中的编号字段值                 //===复制时需要修改===
                string Name1 = "SchedulRuleNm";
                string Value1 = entity.SchedulRuleNm;
                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";//keyValue=空吗？是，返回新增；否，返回编辑。

                if (!string.IsNullOrEmpty(KeyValue))//编辑操作
                {
                    //===复制时需要修改===
                    BBdbR_SchedulRuleBase Oldentity = repositoryfactory.Repository().FindEntity(KeyValue);//获取没更新之前实体对象
                    entity.SchedulRuleId = KeyValue;
                    entity.Modify(KeyValue);
                    IsOk = MyBll.Update(entity);//将修改后的实体更新到数据库，插入成功返回1，失败返回0；
                    this.WriteLog(IsOk, entity, Oldentity, KeyValue, Message);//记录日志
                }
                else//新增操作
                {
                    IsOk = MyBll.CheckCount(Name, Value,Name1,Value1);//判断页面中填写的数据的编号字段的值是否存在
                    if (IsOk ==1)//编号存在
                    {
                        Message = "该编号已经存在！";
                        return Content(new JsonMessage { Success = false, Code = IsOk.ToString(), Message = Message }.ToString());
                    }
                    else if(IsOk == 2)
                    {
                        Message = "该名称已经存在！";
                        return Content(new JsonMessage { Success = false, Code = IsOk.ToString(), Message = Message }.ToString());
                    }
                    entity.SchedulRuleId = System.Guid.NewGuid().ToString();
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
        /// 
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
                //直接删除
                if (array != null && array.Length > 0)
                {            
                    IsOk = MyBll.Delete(array);//删// MyBll.Delete(array[i]);
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

        #region 3.查询方法
        //查询方法，本方法为单条件查询，即根据一个条件进行查询
        //查询条件为Condition，也是数据库表_CompanyBaseInformation中的一个字段名
        //查询值为keywords，也是数据库表_CompanyBaseInformation中的字段名的字段值
        //本查询采用近似查询（like）

        public ActionResult GridPageByCondition(string keywords, string Condition, JqGridParam jqgridparam)
        {
            try
            {
                string keyword = keywords.Trim();
                Stopwatch watch = CommonHelper.TimerStart();
                List<BBdbR_SchedulRuleBase> ListData = MyBll.GetPageListByCondition(keyword, Condition, jqgridparam);//===复制时需要修改===
                var JsonData = new
                {
                    total = jqgridparam.total,
                    page = jqgridparam.page,
                    records = jqgridparam.records,
                    costtime = CommonHelper.TimerEnd(watch),
                    rows = ListData,
                };
                return Content(ListData.ToJson());
            }
            catch (Exception ex)
            {
                //CCSLog.CCSLogHelper.WriteExLog(ex, CCSLog.LogType.WebSite);
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
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
        //public ActionResult ChectOnlyOne(string KeyName, string KeyValue)
        //{
        //    try
        //    {
        //        int IsOk = 0;
        //        string Message = "该字段已经存在！";

        //        if (!string.IsNullOrEmpty(KeyValue))
        //        {
        //            IsOk = MyBll.CheckCount(KeyName, KeyValue);
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
        //#endregion
        //#region 8.编辑填充界面数据
        //public override ActionResult SetForm(string KeyValue)
        //{
        //    try
        //    {
        //        BBdbR_SchedulRuleBase Deviceentity = MyBll.GetPlanList(KeyValue);
        //        return Content(Deviceentity.ToJson());
        //    }
        //    catch (Exception ex)
        //    {
        //        return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
        //    }
        //}
        #endregion

        #endregion
    }
}