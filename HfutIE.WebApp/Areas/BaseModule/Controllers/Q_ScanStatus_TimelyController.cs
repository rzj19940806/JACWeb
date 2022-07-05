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
    /// _FactoryBaseInformation控制器
    /// </summary>
    public class Q_ScanStatus_TimelyController : PublicController<BBdbR_MatBase>
    {
        #region 全局变量定义区

        //定义本页面主要操作的表的表名，称为主表
        string tableName = "Q_ScanStatus_Timely";//===复制时需要修改===

        #endregion
        
        #region 打开视图区域
        
        public ActionResult ScanStatusIndex()//打开车身扫码实时状态界面
        {
            return View();
        }


        #endregion

        #region 方法区   

        #region 1.加载表格数据-查找Q_ScanStatus_Timely表中所有数据
        /// <summary>
        /// 加载列表 主页面
        /// </summary>
        /// <returns></returns>
        public ActionResult GridPageJson1(JqGridParam jqgridparam)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                DataTable dt = new DataTable();
                strSql.Append(@"select a.*,b.PlineNm,b.PlineCd,c.WcNm,d.CarColor1,d.CarType,ISNULL(e.MatCd,PMC.MatCd) MatCd,ISNULL(e.MatNm,ISNULL(PMC.MatNm,a.MatId)) MatNm 
                    from Q_ScanStatus_Timely a left join BBdbR_PlineBase b on a.PlineId =b.PlineId 
                    left join BBdbR_WcBase c on a.WcId =c.WcId 
                    left join P_ProducePlan_Pro d on a.VIN = d.VIN 
                    left join BBdbR_MatBase e on a.MatId = e.MatId 
                    Left join BBdbR_PartMatConfig PMC on A.MatId=PMC.Id
                    order by c.WcCd");
                dt = DataFactory.Database().FindTableBySql(strSql.ToString(), false);
                var JsonData = new
                {
                    rows = dt,
                };
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        

        #region 查询方法-按照条件查询

        public ActionResult GridPageByCondition(string PlineNm, string ScanStatus,string RemainStationNo, JqGridParam jqgridparam)
        {
            try
            {

                var sql = $@"select a.*,b.PlineNm,b.PlineCd,c.WcNm,d.CarColor1,d.CarType,e.MatCd,e.MatNm 
                    from Q_ScanStatus_Timely a left join BBdbR_PlineBase b on a.PlineId =b.PlineId 
                    left join BBdbR_WcBase c on a.WcId =c.WcId left join P_ProducePlan_Pro d on a.VIN = d.VIN 
                    left join BBdbR_MatBase e on a.MatId = e.MatId 
                    where b.PlineNm like @PlineNm 
                    and (ScanStatus like @ScanStatus or @ScanStatus='%已扫码%' and ScanStatus != '未扫码' and ScanStatus != 'ByPass') 
                    and RemainStationNo like @RemainStationNo order by c.WcCd";

                List<DbParameter> parameter = new List<DbParameter>();
                parameter.Add(DbFactory.CreateDbParameter("@PlineNm", "%" + PlineNm + "%"));
                parameter.Add(DbFactory.CreateDbParameter("@ScanStatus", "%" + ScanStatus + "%"));
                parameter.Add(DbFactory.CreateDbParameter("@RemainStationNo", "%" + RemainStationNo + "%"));

                DataTable dt = DataFactory.Database().FindTableBySql(sql, parameter.ToArray(), false);

                var JsonData = new
                {
                    rows = dt,
                };
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "1", "扫码状态监控信息查询成功");
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "扫码状态监控信息查询发生异常错误：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion



        #endregion


        #region 更改已选行ScanStatus状态从未扫码为ByPass
        public ActionResult ChangeScanStatus(string WcId,string VIN,string MatId,string MatNo,string ScanStatus)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"UPDATE Q_ScanStatus_Timely SET ScanStatus = 'ByPass' WHERE WcId = '" + WcId + "' and VIN = '" + VIN + "' and MatId = '" + MatId + "' and MatNo ='" + MatNo + "' and ScanStatus ='" + ScanStatus + "' ");
            
            try
            {
                int r = DbHelperSQL.ExecuteSql(strSql.ToString());
                return Content("\"" + r + "\"");
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }

        }
        #endregion

        #region 查询工位未扫码记录条数
        public ActionResult FindWcScandNumber(string WcId , JqGridParam jqgridparam)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                DataTable dt = new DataTable();
                strSql.Append(@"select * from Q_ScanStatus_Timely  WHERE WcId = '" + WcId + "' and ScanStatus = '未扫码'");
                dt = DataFactory.Database().FindTableBySql(strSql.ToString(), false);
                var JsonData = new
                {
                    rows = dt,
                };
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