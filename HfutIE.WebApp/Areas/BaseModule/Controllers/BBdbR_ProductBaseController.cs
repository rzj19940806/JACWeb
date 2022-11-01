using HfutIE.Business;
using HfutIE.DataAccess;
using HfutIE.Entity;
using HfutIE.Repository;
using HfutIE.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace HfutIE.WebApp.Areas.BaseModule.Controllers
{
    /// <summary>
    /// 产品基础信息表控制器
    /// </summary>
    public class BBdbR_ProductBaseController : PublicController<BBdbR_ProductBase>
    {
        #region 全局变量定义区

        //定义本页面主要操作的表的表名，称为主表
        string tableName = "BBdbR_ProductBase";//===复制时需要修改===
        #endregion

        #region 创建数据库操作对象区域
        BBdbR_ProductBaseBll MyBll = new BBdbR_ProductBaseBll(); //===复制时需要修改===
        #endregion

        #region 打开视图区域
       override public ActionResult Index()//打开产品信息编辑界面
        {
            return View();
        }
        override public ActionResult Form()//打开产品信息编辑界面
        {
            return View();
        }
        #endregion

        #region 方法区   

        #region 1.加载表格数据
        /// <summary>
        /// 加载主页面
        /// </summary>
        /// <returns></returns>
        public ActionResult GridPage()
        {
            try
            {
                #region List接收查询结果
                //List<BBdbR_ProductBase> ListData = MyBll.GetPlanList("");
                //var JsonData = new
                //{
                //    rows = ListData,
                //};
                //string a = JsonData.ToJson();
                //return Content(JsonData.ToJson());
                #endregion

                #region datatable接收查询结果
                StringBuilder strSql = new StringBuilder();
                strSql.Append(@"select a.*,b.RsvFld1 as seq FROM  BBdbR_ProductBase a left join AUEX_NEW_FUELOIL_ID b on a.Oil=b.GID where  a.Enabled=1 order by MatCd asc ");
                DataTable dtExport = DataFactory.Database().FindTableBySql(strSql.ToString(), false);
                var JsonData = new
                {
                    rows = dtExport,
                };
                return Content(JsonData.ToJson());
                #endregion
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion

        #region 2.查询方法
        public ActionResult GridPageByCondition(string MatCd, string MatNm, string CarType, string CarColor1, string TOTAL_WEIGHT, string CAPACITY, JqGridParam jqgridparam)
        {
            try
            {
                #region datatable接收查询结果
                StringBuilder strSql = new StringBuilder();
                List<DbParameter> parameter = new List<DbParameter>();
                strSql.Append(@"SELECT  a.*,b.RsvFld1 as seq FROM  BBdbR_ProductBase a left join AUEX_NEW_FUELOIL_ID b on a.Oil=b.GID  where a.Enabled=1 ");

                #region 判断输入框内容添加检索条件
                //是否加产品编号模糊搜索
                if (MatCd != "" && MatCd != null)
                {
                    //strSql.Append(" and MatCd like '%" + MatCd + "%'");
                    strSql.Append(" and MatCd like @MatCd ");
                    parameter.Add(DbFactory.CreateDbParameter("@MatCd", "%" + MatCd + "%"));
                }
                else { }

                //是否加产品名称模糊搜索
                if (MatNm != "" && MatNm != null)
                {
                    //strSql.Append(" and MatNm like '%" + MatNm + "%'");
                    strSql.Append(" and MatNm like @MatNm ");
                    parameter.Add(DbFactory.CreateDbParameter("@MatNm", "%" + MatNm + "%"));
                }
                else { }

                //是否加车型模糊搜索
                if (CarType != "" && CarType != null)
                {
                    //strSql.Append(" and CarType like '%" + CarType + "%'");
                    strSql.Append(" and CarType like @CarType ");
                    parameter.Add(DbFactory.CreateDbParameter("@CarType", "%" + CarType + "%"));
                }
                else { }

                //是否加颜色模糊搜索
                if (CarColor1 != "" && CarColor1 != null)
                {
                    //strSql.Append(" and CarColor1 like '%" + CarColor1 + "%'");
                    strSql.Append(" and CarColor1 like @CarColor1 ");
                    parameter.Add(DbFactory.CreateDbParameter("@CarColor1", "%" + CarColor1 + "%"));
                }
                else { }

                //是否加最大允许总质量搜索
                if (TOTAL_WEIGHT != "" && TOTAL_WEIGHT != null)
                {
                    //strSql.Append(" and TOTAL_WEIGHT = '" + TOTAL_WEIGHT + "'");
                    strSql.Append(" and TOTAL_WEIGHT = @TOTAL_WEIGHT ");
                    parameter.Add(DbFactory.CreateDbParameter("@TOTAL_WEIGHT", TOTAL_WEIGHT));
                }
                else { }

                //是否加载客人数搜索
                if (CAPACITY != "" && CAPACITY != null)
                {
                    //strSql.Append(" and CAPACITY = '" + CAPACITY + "'");
                    strSql.Append(" and CAPACITY = @CAPACITY ");
                    parameter.Add(DbFactory.CreateDbParameter("@CAPACITY", CAPACITY));
                }
                else { }

                #endregion

                //按照产品编号排序
                strSql.Append(" order by MatCd ");

                DataTable dt = DataFactory.Database().FindTableBySql(strSql.ToString(), parameter.ToArray(), false);
                var JsonData = new
                {
                    rows = dt,
                };
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "1", "产品基本档案查询成功");
                return Content(JsonData.ToJson());
                #endregion
            }
            catch (Exception ex)
            {
                //CCSLog.CCSLogHelper.WriteExLog(ex, CCSLog.LogType.WebSite);
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "产品基本档案查询发生异常错误：" + ex.Message);
                return null;
            }
        }
        #endregion

        #region 3.双击获取实体信息
        /// <summary>
        /// 获取物料信息对象返回JSON
        /// 双击行展示图片
        /// </summary>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SetMatForm(string KeyValue)
        {
            try
            {
                DataTable ListData = MyBll.GetMatInfor(KeyValue);
                var JsonData = new
                {
                    rows = ListData,
                };
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion

        #region 4.重构导出
        public ActionResult GetExcel_Data(string MatCd, string MatNm, string CarType, string CarColor1, string TOTAL_WEIGHT, string CAPACITY, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();

                #region datatable接收查询结果
                StringBuilder strSql = new StringBuilder();
                List<DbParameter> parameter = new List<DbParameter>();
                strSql.Append(@"SELECT  MatCd,MatNm,CarType,CarColor1,Notification,EngineOut,EngineModel,EngineMaxPower,TOTAL_WEIGHT,CAPACITY,BodyWeight,Purpose,SpecialCarNm,MAX_POWER_SPEED,Oil,DIS,Rem FROM  BBdbR_ProductBase where Enabled=1 ");

                #region 判断输入框内容添加检索条件
                //是否加产品编号模糊搜索
                if (MatCd != "" && MatCd != null)
                {
                    //strSql.Append(" and MatCd like '%" + MatCd + "%'");
                    strSql.Append(" and MatCd like @MatCd ");
                    parameter.Add(DbFactory.CreateDbParameter("@MatCd", "%" + MatCd + "%"));
                }
                else { }

                //是否加产品名称模糊搜索
                if (MatNm != "" && MatNm != null)
                {
                    //strSql.Append(" and MatNm like '%" + MatNm + "%'");
                    strSql.Append(" and MatNm like @MatNm ");
                    parameter.Add(DbFactory.CreateDbParameter("@MatNm", "%" + MatNm + "%"));
                }
                else { }

                //是否加车型模糊搜索
                if (CarType != "" && CarType != null)
                {
                    //strSql.Append(" and CarType like '%" + CarType + "%'");
                    strSql.Append(" and CarType like @CarType ");
                    parameter.Add(DbFactory.CreateDbParameter("@CarType", "%" + CarType + "%"));
                }
                else { }

                //是否加颜色模糊搜索
                if (CarColor1 != "" && CarColor1 != null)
                {
                    //strSql.Append(" and CarColor1 like '%" + CarColor1 + "%'");
                    strSql.Append(" and CarColor1 like @CarColor1 ");
                    parameter.Add(DbFactory.CreateDbParameter("@CarColor1", "%" + CarColor1 + "%"));
                }
                else { }

                //是否加最大允许总质量搜索
                if (TOTAL_WEIGHT != "" && TOTAL_WEIGHT != null)
                {
                    //strSql.Append(" and TOTAL_WEIGHT = '" + TOTAL_WEIGHT + "'");
                    strSql.Append(" and TOTAL_WEIGHT = @TOTAL_WEIGHT ");
                    parameter.Add(DbFactory.CreateDbParameter("@TOTAL_WEIGHT", TOTAL_WEIGHT));
                }
                else { }

                //是否加载客人数搜索
                if (CAPACITY != "" && CAPACITY != null)
                {
                    //strSql.Append(" and CAPACITY = '" + CAPACITY + "'");
                    strSql.Append(" and CAPACITY = @CAPACITY ");
                    parameter.Add(DbFactory.CreateDbParameter("@CAPACITY", CAPACITY));
                }
                else { }

                #endregion

                //按照产品编号排序
                strSql.Append(" order by MatCd ");

                DataTable dt = DataFactory.Database().FindTableBySql(strSql.ToString(), parameter.ToArray(), false);

                #endregion



                string fileName = "产品基础信息表";
                string excelType = "xls";
                MemoryStream ms = DeriveExcel.ExportExcel_ProductBase(dt, excelType);
                if (!fileName.EndsWith(".xls"))
                {
                    fileName = fileName + ".xls";
                }

                #region 解决乱码
                //Encoding ec = Encoding.GetEncoding("iso-8859-1");
                //byte[] btArr = ec.GetBytes(fileName);
                //string fileName2 = Encoding.Default.GetString(btArr);
                #endregion
                Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other,"1", "产品基本档案导出成功");
                return File(ms, "application/vnd.ms-excel", fileName);
                //Response.AddHeader("Content-Disposition", "attachment; filename=" + Server.UrlEncode("产品基础信息表.xls".ToString()));
                //return File(ms, "application/vnd.ms-excel");
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other, "-1", "产品基本档案导出操作失败：" + ex.Message);
                return null;
            }

        }
        #endregion

        #endregion
    }
}