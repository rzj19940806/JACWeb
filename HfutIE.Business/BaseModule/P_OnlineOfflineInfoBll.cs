//=====================================================================================
// All Rights Reserved , Copyright @ HfutIE 2021
// Software Developers @ HfutIE 2021
//=====================================================================================

using HfutIE.DataAccess;
using HfutIE.Entity;
using HfutIE.Repository;
using HfutIE.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace HfutIE.Business
{
    /// <summary>
    /// P_OnlineOfflineInfo
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.12.20 10:52</date>
    /// </author>
    /// </summary>
    public class P_OnlineOfflineInfoBll : RepositoryFactory<P_PlanFeedBack_Pro>
    {
        #region 全局变量定义区

        //定义本页面主要操作的表的表名，称为主表
        string tableName = "P_PlanFeedBack_Pro";//===复制时需要修改===
        public readonly RepositoryFactory<P_PlanFeedBack_Pro> repository_feedbackbase = new RepositoryFactory<P_PlanFeedBack_Pro>();
        #endregion

        #region 方法区

        
        
        public DataTable GetCollectTime(string AviCd, string VIN, string MatCd, string ProducePlanCd, string ColletionTimeStart, string ColletionTimeEnd, JqGridParam jqgridparam) //===复制时需要修改===
        {
            #region 原方法
            //IDatabase database = DataFactory.Database();
            //string sql = "";
            //sql = $@"select SUBSTRING(A.VIN,10,8)as ChassisCd,A.VIN,A.MatCd,B.MatNm,OrderCd,ProducePlanCd,C.AviCd,C.AviNm,FeedbackTime  from P_PlanFeedBack_Pro A LEFT JOIN BBdbR_ProductBase B ON A.MatCd = B.MatCd LEFT JOIN BBdbR_AVIBase C ON A.OP_CODE = C.OP_CODE WHERE  A.VIN IN (select VIN from P_PlanFeedBack_Pro D LEFT JOIN BBdbR_AVIBase E ON D.OP_CODE = E.OP_CODE where VIN like '%{VIN}%'  and E.AviCd like '%{AviCd}%' and D.MatCd LIKE  '%{MatCd}%' and D.ProducePlanCd LIKE  '%{ProducePlanCd}%')  ";

            //if (!String.IsNullOrEmpty(ColletionTimeStart)&& !String.IsNullOrEmpty(ColletionTimeEnd))
            //{
            //    sql = $@"select SUBSTRING(A.VIN,10,8)as ChassisCd,A.VIN,A.MatCd,B.MatNm,OrderCd,ProducePlanCd,C.AviCd,C.AviNm,FeedbackTime  from P_PlanFeedBack_Pro A LEFT JOIN BBdbR_ProductBase B ON A.MatCd = B.MatCd LEFT JOIN BBdbR_AVIBase C ON A.OP_CODE = C.OP_CODE WHERE  A.VIN IN (select VIN from P_PlanFeedBack_Pro D LEFT JOIN BBdbR_AVIBase E ON D.OP_CODE = E.OP_CODE where VIN like '%{VIN}%'  and E.AviCd like '%{AviCd}%' and D.MatCd LIKE  '%{MatCd}%' and D.ProducePlanCd LIKE  '%{ProducePlanCd}%' and FeedbackTime between '{ColletionTimeStart} 00:00:00' and '{ColletionTimeEnd}  23:59:59')   ";
            //}
            //else
            //{
            //    //预计生产开始时间
            //    if (ColletionTimeStart != "" && ColletionTimeStart != null)
            //    {
            //        string StartPlanTime = DateDiff(ColletionTimeStart);    //计划开始时间距离当天的天数 大
            //        sql += $@" and DateDiff(dd,FeedbackTime,getdate()) <=  " + StartPlanTime;
            //    }
            //    else { }

            //    //预计生产结束时间
            //    if (ColletionTimeEnd != "" && ColletionTimeEnd != null)
            //    {
            //        string EndPlanTime = DateDiff(ColletionTimeEnd);        //计划结束时间距离当天的天数 小
            //        sql += $@" and DateDiff(dd,FeedbackTime,getdate()) >=  " + EndPlanTime;
            //    }
            //    else { }
            //}
            
            //sql += $@" order by VIN DESC ";
            //return database.FindTableBySql(sql, false);
            #endregion

            #region 修改-SQL注入
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            if (!String.IsNullOrEmpty(ColletionTimeStart) && !String.IsNullOrEmpty(ColletionTimeEnd))
            {
                strSql.Append($@"select SUBSTRING(A.VIN,10,8)as ChassisCd,A.VIN,A.MatCd,B.MatNm,OrderCd,ProducePlanCd,C.AviCd,C.AviNm,FeedbackTime  from P_PlanFeedBack_Pro A LEFT JOIN BBdbR_ProductBase B ON A.MatCd = B.MatCd LEFT JOIN BBdbR_AVIBase C ON A.OP_CODE = C.OP_CODE WHERE  A.VIN IN (select VIN from P_PlanFeedBack_Pro D LEFT JOIN BBdbR_AVIBase E ON D.OP_CODE = E.OP_CODE where VIN like @VIN  and E.AviCd like @AviCd and D.MatCd LIKE @MatCd and D.ProducePlanCd LIKE @ProducePlanCd and FeedbackTime between @ColletionTimeStart and @ColletionTimeEnd)  ");
                parameter.Add(DbFactory.CreateDbParameter("@AviCd", "%" + AviCd + "%"));
                parameter.Add(DbFactory.CreateDbParameter("@VIN", "%" + VIN ));
                parameter.Add(DbFactory.CreateDbParameter("@MatCd", "%" + MatCd + "%"));
                parameter.Add(DbFactory.CreateDbParameter("@ProducePlanCd", "%" + ProducePlanCd + "%"));
                parameter.Add(DbFactory.CreateDbParameter("@ColletionTimeStart",  ColletionTimeStart + " 00:00:00"));
                parameter.Add(DbFactory.CreateDbParameter("@ColletionTimeEnd", ColletionTimeEnd + " 23:59:59"));
            }
            else
            {
                strSql.Append($@"select SUBSTRING(A.VIN,10,8)as ChassisCd,A.VIN,A.MatCd,B.MatNm,OrderCd,ProducePlanCd,C.AviCd,C.AviNm,FeedbackTime  from P_PlanFeedBack_Pro A LEFT JOIN BBdbR_ProductBase B ON A.MatCd = B.MatCd LEFT JOIN BBdbR_AVIBase C ON A.OP_CODE = C.OP_CODE WHERE  A.VIN IN (select VIN from P_PlanFeedBack_Pro D LEFT JOIN BBdbR_AVIBase E ON D.OP_CODE = E.OP_CODE where VIN like @VIN  and E.AviCd like @AviCd and D.MatCd LIKE  @MatCd and D.ProducePlanCd LIKE  @ProducePlanCd)  ");
                parameter.Add(DbFactory.CreateDbParameter("@AviCd", "%" + AviCd + "%"));
                parameter.Add(DbFactory.CreateDbParameter("@VIN", "%" + VIN));
                parameter.Add(DbFactory.CreateDbParameter("@MatCd", "%" + MatCd + "%"));
                parameter.Add(DbFactory.CreateDbParameter("@ProducePlanCd", "%" + ProducePlanCd + "%"));
                //预计生产开始时间
                if (ColletionTimeStart != "" && ColletionTimeStart != null)
                {
                    //string StartPlanTime = DateDiff(ColletionTimeStart);    //计划开始时间距离当天的天数 大
                    //sql += $@" and DateDiff(dd,FeedbackTime,getdate()) <=  " + StartPlanTime;

                    strSql.Append(" and DateDiff(dd,@ColletionTimeStart,FeedbackTime) >=0 ");
                    parameter.Add(DbFactory.CreateDbParameter("@ColletionTimeStart", ColletionTimeStart));
                }
                else { }

                //预计生产结束时间
                if (ColletionTimeEnd != "" && ColletionTimeEnd != null)
                {
                    //string EndPlanTime = DateDiff(ColletionTimeEnd);        //计划结束时间距离当天的天数 小
                    //sql += $@" and DateDiff(dd,FeedbackTime,getdate()) >=  " + EndPlanTime;

                    strSql.Append(" and DateDiff(dd,FeedbackTime,@ColletionTimeEnd) >=0 ");
                    parameter.Add(DbFactory.CreateDbParameter("@ColletionTimeEnd", ColletionTimeEnd));
                }
                else { }
            }
            strSql.Append(" order by VIN desc");
            DataTable dt = DataFactory.Database().FindTableBySql(strSql.ToString(), parameter.ToArray(), false);
            return dt;
            #endregion
        }

        #region 计算日期与今天日期的天数
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