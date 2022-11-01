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
    /// P_OnlineOfflineInfo控制器
    /// </summary>
    public class P_OnlineOfflineInfoController : PublicController<P_PlanFeedBack_Pro>
    {
        #region 全局变量定义区
        //定义本页面主要操作的表的表名，称为主表
        string tableName = "P_PlanFeedBack_Pro";
        public static DataTable PlanFeedBackList = new DataTable();
        public static DataTable alltable = new DataTable();
        public static DataTable NamesTable = new DataTable();
        #endregion

        #region 创建数据库操作对象区域
        //创建数据库访问对象，用以访问其中操作数据库的方法
        P_PlanFeedBack_ProBll MyBll = new P_PlanFeedBack_ProBll(); //===复制时需要修改===
        P_OnlineOfflineInfoBll OnlineBll = new P_OnlineOfflineInfoBll(); //===复制时需要修改===
        public readonly RepositoryFactory<P_PlanFeedBack_Pro> repository_feedbackbase = new RepositoryFactory<P_PlanFeedBack_Pro>();
        BBdbR_AVIBaseBll avibll = new BBdbR_AVIBaseBll();

        #endregion
        

        #region 方法区  

        

        #region 1.获取下拉框内容
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


        #region 获取列名
        public ActionResult GridNamesList(string AviCd, string VIN, string MatCd, string ProducePlanCd, DateTime ColletionTimeStart, DateTime ColletionTimeEnd, JqGridParam jqgridparam)
        {
            try
            {
                
                //DataTable ListYear = OnlineBll.GetPageList_Year(ProductionLineName, jqgridparam);
                DataTable avidt = avibll.GetAviNm();

                #region 构造表中/英文列名
                NamesTable = new DataTable();
                NamesTable.Columns.Add("names");
                NamesTable.Columns.Add("englishnames");

                string[] ChineseName = { "底盘号", "VIN码", "车型编码", "车型名称","订单号","工单号" };
                string[] EnglishName = { "ChassisCd", "VIN", "MatCd", "MatNm", "OrderCd", "ProducePlanCd" };

                for (int i = 0; i < ChineseName.Length; i++)
                {
                    DataRow row1 = NamesTable.NewRow();
                    row1["names"] = ChineseName[i];
                    row1["englishnames"] = EnglishName[i];
                    NamesTable.Rows.Add(row1);
                }
                

                for (int i = 0; i < avidt.Rows.Count; i++)
                {
                    DataRow namerow1 = NamesTable.NewRow();
                    namerow1["names"] = avidt.Rows[i]["AviNm"];
                    namerow1["englishnames"] = avidt.Rows[i]["AviCd"];
                    NamesTable.Rows.Add(namerow1);

                    
                }
                
                #endregion
                

                var JsonData = new
                {

                    //rows = stringBuilder,
                    rows = NamesTable,
                };
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion
        

        #region 加载列表数据

        public ActionResult GridList(string AviCd, string VIN, string MatCd, string ProducePlanCd, string ColletionTimeStart, string ColletionTimeEnd, JqGridParam jqgridparam)
        {
            try
            {
                

                Stopwatch watch = CommonHelper.TimerStart();
                DataTable listTable = OnlineBll.GetCollectTime(AviCd, VIN, MatCd, ProducePlanCd, ColletionTimeStart, ColletionTimeEnd, jqgridparam);
                DataTable avidt = avibll.GetAviNm();
                
                alltable = listTable.Clone();
                alltable.Columns.Remove("AviCd");
                alltable.Columns.Remove("AviNm");
                alltable.Columns.Remove("FeedbackTime");
                for (int i = 0; i < avidt.Rows.Count; i++)
                {

                    alltable.Columns.Add(avidt.Rows[i]["AviCd"].ToString());
                }
                string nowvin = "";
                int index = -1;
                for (int i = 0; i < listTable.Rows.Count; i++)
                {
                    if (nowvin != listTable.Rows[i]["VIN"].ToString())
                    {
                        DataRow row = alltable.NewRow();
                        row["ChassisCd"] = listTable.Rows[i]["ChassisCd"].ToString();
                        row["VIN"] = listTable.Rows[i]["VIN"].ToString();
                        row["MatCd"] = listTable.Rows[i]["MatCd"].ToString();
                        row["MatNm"] = listTable.Rows[i]["MatNm"].ToString();
                        row["OrderCd"] = listTable.Rows[i]["OrderCd"].ToString();
                        row["ProducePlanCd"] = listTable.Rows[i]["ProducePlanCd"].ToString();
                        row[listTable.Rows[i]["AviCd"].ToString()] = listTable.Rows[i]["FeedbackTime"].ToString();
                        ++index;
                        nowvin = listTable.Rows[i]["VIN"].ToString();
                        alltable.Rows.Add(row);
                    }
                    else//同一个VIN码
                    {
                        alltable.Rows[index][listTable.Rows[i]["AviCd"].ToString()] = listTable.Rows[i]["FeedbackTime"].ToString();
                    }
                }


                var JsonData = new
                {
                    total = jqgridparam.total,
                    page = jqgridparam.page,
                    records = jqgridparam.records,
                    costtime = CommonHelper.TimerEnd(watch),
                    rows = alltable,
                    
                };
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "1", "产品上下线查询成功");
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "产品上下线查询发生异常错误：" + ex.Message);
                return null;
            }
        }

        #endregion


        #region 10.重构导出方法
        /// <summary>
        /// 1.如果是按照条件查询后再进行
        /// </summary>
        /// <param name="type"></param>
        /// <param name="jqgridparam"></param>
        /// <returns></returns>
        public ActionResult GetExcel_Data(JqGridParam jqgridparam)
        {
            try
            {
                DataTable ListData = new DataTable();

                string[] array = new string[alltable.Columns.Count];

                for (int i = 0; i < alltable.Columns.Count; i++)
                {
                    array[i] = alltable.Columns[i].ColumnName;

                }

                ListData = alltable.DefaultView.ToTable("产品上下线表", true, array);//获取设备信息中特定列

                if (ListData.Rows.Count > 0)
                {
                    string fileName = "产品上下线信息";
                    string excelType = "xls";
                    MemoryStream ms = DeriveExcel.ExportExcel_Online(ListData, NamesTable, excelType);
                    if (!fileName.EndsWith(".xls"))
                    {
                        fileName = fileName + ".xls";
                    }
                    Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other, "1", "产品上下线信息导出成功");
                    return File(ms, "application/vnd.ms-excel", Url.Encode(fileName));
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other, "-1", "产品上下线信息导出操作失败：" + ex.Message);
                return null;
            }
            
        }
        #endregion

        #endregion

    }
}