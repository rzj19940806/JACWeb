using HfutIE.Business;
using HfutIE.Entity;
using HfutIE.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HfutIE.WebApp.Areas.DefectModule.Controllers
{
    /// <summary>
    /// Andon数据采集过程表控制器
    /// </summary>
    public class EnterDefectController : Controller
    {
        #region 全局变量定义区

        //定义本页面主要操作的表的表名，称为主表
        string tableName = "Q_CarQualityInspection_Pro";//===复制时需要修改===

        #endregion

        #region 创建数据库操作对象区域
        Q_CarQualityInspection_ProBll MyBll = new Q_CarQualityInspection_ProBll();//===复制时需要修改===
        #endregion

        #region 打开视图区域                    
        public ActionResult Q_CarQualityDefectIndex()//打开子界面
        {
            return View();
        }
        #endregion
     
        # region 通过关键词获取车身基本信息
        /// <summary>
        /// 获取车身信息对象返回JSON
        /// </summary>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult GetCarInfor(string keywords)
        {          
                try
                {
                    Q_CarQualityInspection_Pro ListData = MyBll.GetCarInforbykeywords(keywords);
                    var JsonData = new
                    {
                        rows = ListData,
                    };
                    return Content(ListData.ToJson());
                }
                catch (Exception ex)
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
                }
            
        }
        #endregion
                              
        #region 通过主键获取质控点名称     
        public ActionResult GetWcName()
        {
            try
            {
                //Q_CarQualityInspection_Pro ListData = MyBll.GetWcNamebyKeyValue();
            DataTable dataTable = MyBll.GetWcName();
                var JsonData = new
                {
                    rows = dataTable,
                };
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }

        }
        #endregion
        #region 编辑方法

        public ActionResult SubmitForm(BBdbR_DataAcqPro entity, string KeyValue)//===复制时需要修改===
        {
            try
            {
                int IsOk = 0;//用于判断
                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";
                //===复制时需要修改===
                    entity.Modify(KeyValue);
                    entity.CurValue="已补录";
                    //IsOk = MyBll.Update(entity);//将修改后的实体更新到数据库，插入成功返回1，失败返回0；
               
                return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion


        #region Andon大屏
        #region  获取Andon计划信息
        /// <summary>
        ///获取计划信息
        /// </summary>
        /// <returns></returns>
        public ActionResult FindPlan()
        {
            try
            {
                DataTable ListData = MyBll.GetTodayPlan();
                var JsonData = new
                {
                    rows = ListData,
                };
                string a = JsonData.ToJson();
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        #endregion


        #region  获取Andon班组信息
        /// <summary>
        ///获取计划信息
        /// </summary>
        /// <returns></returns>
        public ActionResult FindClass()
        {
            try
            {
                DataTable ListData = MyBll.GetTodayClass();
                var JsonData = new
                {
                    rows = ListData,
                };
                string a = JsonData.ToJson();
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        #endregion

        #endregion





    }
}