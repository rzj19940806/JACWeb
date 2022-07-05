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

namespace HfutIE.WebApp.Areas.BaseModule.Controllers
{
    /// <summary>
    /// 接口管理控制器
    /// </summary>
    public class BBdbR_InterfaceController : PublicController<BBdbR_Interface>
    {
        #region 全局变量定义区
        //定义本页面主要操作的表的表名，称为主表
        string tableName = "BBdbR_Interface";
        public static DataTable InterfaceList = new DataTable();
        #endregion

        #region 创建数据库操作对象区域
        BBdbR_InterfaceBll InterfaceBll = new BBdbR_InterfaceBll();
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
        public ActionResult GetInterfaceListJson(JqGridParam jqgridparam)
        {
            try
            {
                //Stopwatch watch = CommonHelper.TimerStart();
                //获取点击节点对应的数据（列表）
                InterfaceList = InterfaceBll.GetList();//===复制时需要修改===
                //var JsonData = new
                //{
                //    total = jqgridparam.total,
                //    page = jqgridparam.page,
                //    records = jqgridparam.records,
                //    costtime = CommonHelper.TimerEnd(watch),
                //    rows = InterfaceList,
                //};
                return Content(InterfaceList.ToJson());
            }
            catch (Exception ex)
            {
                this.WriteLog(-1, null, null, null, "操作失败" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion

        #region 3.新增编辑方法
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
        public ActionResult SubmitForm1(string KeyValue, BBdbR_Interface entity)//===复制时需要修改===
        {
            try
            {
                int IsOk = 0;//用于判断
                
                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";
                
                if (!string.IsNullOrEmpty(KeyValue))//编辑操作
                {
                    entity.Sql = entity.Sql== "&nbsp;" ? "" : entity.Sql;
                    
                    entity.Modify(KeyValue);
                    //===复制时需要修改===
                    BBdbR_Interface Oldentity = repositoryfactory.Repository().FindEntity(KeyValue);//获取没更新之前实体对象
                    
                    IsOk = InterfaceBll.Update(entity);//将修改后的实体更新到数据库，插入成功返回1，失败返回0；
                    this.WriteLog(IsOk, entity, Oldentity, KeyValue, Message);//记录日志
                    
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

        

        
        #endregion

    }
}