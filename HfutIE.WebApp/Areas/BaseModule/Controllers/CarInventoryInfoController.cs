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
    public class CarInventoryInfoController : PublicController<P_CarPastRecordInfo>
    {
        // GET: BaseModule/P_CarPastRecordInfo
        #region  创建数据库操作对象区域
        P_CarPastRecordInfoBll MyBll = new P_CarPastRecordInfoBll();
        BBdbR_AVIBaseBll avibasebll = new BBdbR_AVIBaseBll();
        #endregion

        #region 定义全局变量datatable用于存放搜索结果和导出
        public static DataTable dtExport = new DataTable();
        #endregion

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
                
                DataTable avidt = avibasebll.GetAviNm();
                
                return Content(avidt.ToJson());
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region 获取数据

        /// <summary>
        /// 检索区间库存
        /// </summary>
        /// <param name="productLineName"></param>
        /// <param name="Condition"></param>
        /// <param name="keywords"></param>
        /// <param name="jqgridparam"></param>
        /// <returns></returns>
        //[LoginAuthorize]
        public ActionResult GridPageListJsonCarRecord(string crossAviCd, string nocrossAviCd, string StartTime, string EndTime, string CarType, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                IDatabase database = DataFactory.Database();
                //List<Pro_MaterialConsumeRecord> materailConsumeList = materailConsumeRecordBll.GetPageListMaterialConsume(productLineName, Condition, keywords, ColletionTimeStart, ColletionTimeEnd, WorkGroupType, Batch, ref jqgridparam);
                #region 查询版本1.0
                //StringBuilder sql = new StringBuilder();

                //sql.Append($@"select C.OrderCd,C.ProducePlanCd,SUBSTRING(A.VIN,10,8) ChassisCd,A.VIN,E.CarType,C.MatCd,E.MatNm,E.CarColor1,D.AviNm,B.FeedbackTime PastTime from 
                //  (select VIN,max(OP_CODE) OP_CODE from P_PlanFeedBack_Pro with(nolock) where FeedbackTime is not null group by VIN) A
                //  join P_PlanFeedBack_Pro B on A.VIN=B.VIN and A.OP_CODE=B.OP_CODE 
                //  and B.FeedbackTime between '{StartTime.ToString("yyyy-MM-dd")}' and '{EndTime.ToString("yyyy-MM-dd")} 23:59:59' ");
                //if (nocrossAviCd != "")
                //{
                //    sql.Append($@" and A.OP_CODE >='{crossAviCd}' and A.OP_CODE<'{nocrossAviCd}'");
                //}
                //else
                //{
                //    sql.Append($@" and A.OP_CODE >='{crossAviCd}' ");
                //}
                //sql.Append($@" left join P_ProducePlan_Pro C with(nolock) on A.VIN=C.VIN
                //  left join BBdbR_AVIBase D on A.OP_CODE=D.OP_CODE 
                //  left join BBdbR_ProductBase E on C.MatCd=E.MatCd where E.CarType like '%{CarType}%'
                //  order by B.FeedbackTime  ");
                #endregion

                #region 查询版本2.0
                //string sql = "";
                //if (nocrossAviCd != "")
                //{
                //    sql = $@"select SUBSTRING(D.VIN,10,8)as ChassisCd,D.VIN,D.ProducePlanCd,D.CarType,D.CarColor1,D.AviNm,D.PastTime,E.OrderCd,E.MatCd,F.MatNm from (select A.VIN from (select VIN, PastTime from P_CarPastRecordInfo where VIN <> '9999' and PastTime between '{StartTime.ToString("yyyy-MM-dd")}' and '{EndTime.ToString("yyyy-MM-dd")}  23:59:59' and AviCd = '{crossAviCd}') A left join (select VIN from P_CarPastRecordInfo where VIN <> '9999'  and AviCd = '{nocrossAviCd}' and PastTime > '{StartTime.ToString("yyyy-MM-dd")}') B on A.VIN = B.VIN where B.VIN is null) C 
                //  join (select * from P_CarPastRecordInfo A where PastTime > '{StartTime.ToString("yyyy-MM-dd")}'  and exists(select 1 from P_CarPastRecordInfo B where A.VIN = B.VIN  group by VIN having  A.PastTime = MAX(B.PastTime))) D
                // on C.VIN = D.VIN LEFT JOIN P_ProducePlan_Pro E ON D.VIN = E.VIN LEFT JOIN BBdbR_ProductBase F ON E.MatCd = F.MatCd WHERE D.CarType LIKE '{CarType}%'";
                //}
                //else//未修改好（目前查询速度慢）
                //{
                //    sql = $@"select SUBSTRING(D.VIN,10,8)as ChassisCd,D.VIN,D.ProducePlanCd,D.CarType,D.CarColor1,D.AviNm,D.PastTime,E.OrderCd,E.MatCd,F.MatNm from (select A.VIN from (select VIN, PastTime from P_CarPastRecordInfo where VIN <> '9999' and PastTime between '{StartTime.ToString("yyyy-MM-dd")}' and '{EndTime.ToString("yyyy-MM-dd")}  23:59:59' and AviCd = '{crossAviCd}') A left join (select VIN from P_CarPastRecordInfo where VIN <> '9999'  and AviCd = '{nocrossAviCd}' and AviCd<>'' and PastTime > '{StartTime.ToString("yyyy-MM-dd")}') B on A.VIN = B.VIN where B.VIN is null) C 
                //  join (select * from P_CarPastRecordInfo A where PastTime > '{StartTime.ToString("yyyy-MM-dd")}'  and exists(select 1 from P_CarPastRecordInfo B where A.VIN = B.VIN  group by VIN having  A.PastTime = MAX(B.PastTime))) D
                // on C.VIN = D.VIN LEFT JOIN P_ProducePlan_Pro E ON D.VIN = E.VIN LEFT JOIN BBdbR_ProductBase F ON E.MatCd = F.MatCd WHERE D.CarType LIKE '{CarType}%'";
                //}
                #endregion

                #region 查询版本3.0
                string sql = "";
                sql = $@" 
                    select B.OrderCd,B.ProducePlanCd,A.VIN,SUBSTRING(A.VIN,10,8)as ChassisCd,A.CarType,A.CarColor1,A.MatCd,A.OP_CODE,B.FeedbackTime,MatNm,AviNm from (
                    select  A.VIN,A.CarType,A.CarColor1,A.MatCd,B.OP_CODE,C.MatNm,D.AviNm--B.ProducePlanCd,B.OrderCd,
                    from P_LineProductionQueue_Pro A with(nolock)
                    join (select VIN,MAX(OP_CODE) OP_CODE from P_PlanFeedBack_Pro with(nolock) where FeedbackTime is not null group by VIN ) B 
                    on A.VIN=B.VIN and A.CarType like '%{CarType}%' --车型筛选
                    LEFT JOIN BBdbR_MatBase C ON A.MatCd= C.MatCd LEFT JOIN  BBdbR_AVIBase D ON B.OP_CODE = D.OP_CODE 
                    ) A 
                    join P_PlanFeedBack_Pro B on A.VIN=B.VIN and A.OP_CODE=B.OP_CODE
                    and exists (select 1 from P_PlanFeedBack_Pro PI where PI.VIN=A.VIN and (PI.OP_CODE='{crossAviCd}' or '{crossAviCd}'='') --经过AVI点，允许为空
                    and DATEDIFF(DD,IIF('{StartTime}'='','2022-01-01','{StartTime}'),PI.FeedbackTime)>=0 --'2022-03-15' 过点开始时间
                    and DATEDIFF(DD,PI.FeedbackTime,IIF('{EndTime}'='',getdate(),'{EndTime}'))>=0 ) --'2022-04-15'=过点结束时间
                    and not exists (select 1 from P_PlanFeedBack_Pro PI where PI.VIN=A.VIN and (PI.OP_CODE='{nocrossAviCd}' and LEN(PI.FeedbackTime)>0  and '{nocrossAviCd}'<>''))--未经过AVI点，允许为空   ";

                #endregion





                dtExport = database.FindTableBySql(sql.ToString(), false);
                var JsonData = new
                {
                    total = jqgridparam.total,
                    page = jqgridparam.page,
                    records = jqgridparam.records,
                    costtime = CommonHelper.TimerEnd(watch),
                    rows = dtExport,
                };
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "1", "区间查询成功");
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "区间查询发生异常错误：" + ex.Message);
                return null;
            }
        }

        #endregion

        #region 重构导出
        public ActionResult GetExcel_Data(JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                DataTable ListData = new DataTable();

                if (dtExport.Rows.Count > 0)
                {
                    ListData = dtExport.DefaultView.ToTable("区间查询数据", true, "OrderCd", "ProducePlanCd", "ChassisCd", "VIN", "CarType", "MatCd", "MatNm", "CarColor1", "AviNm", "FeedbackTime");//获取表中特定列
                }

                string fileName = "区间查询数据";
                string excelType = "xls";
                MemoryStream ms = DeriveExcel.ExportExcel_CarInventoryInfo(ListData, excelType);
                if (!fileName.EndsWith(".xls"))
                {
                    fileName = fileName + ".xls";
                }
                Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other, "1", "区间查询数据导出成功");
                return File(ms, "application/vnd.ms-excel", Url.Encode(fileName));
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other, "-1", "区间查询数据导出操作失败：" + ex.Message);
                return null;
            }

        }
        #endregion

    }
}