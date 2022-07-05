//=====================================================================================
// All Rights Reserved , Copyright @ HfutIE 2022
// Software Developers @ HfutIE 2022
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
using System.Text;

namespace HfutIE.Business
{
    /// <summary>
    /// P_ProductionLog
    /// <author>
    ///		<name>she</name>
    ///		<date>2022.03.01 13:03</date>
    /// </author>
    /// </summary>
    public class P_ProductionLogBll : RepositoryFactory<P_ProductionLog>
    {

        public P_ProductionLog ProductionLog = new P_ProductionLog();
        public DataTable GetPageListByCondition(string OPModule, string OPAction, string OPType, string OPResult, string TimeStart, string TimeEnd, JqGridParam jqgridparam) //===复制时需要修改===
        {
            string sql = "";
            DataTable dt;
            sql = $"select * from P_ProductionLog where Enabled = 1  ";
            List<DbParameter> parameter = new List<DbParameter>();
            
            if (!String.IsNullOrEmpty(OPModule))
            {
                sql += " and OPModule like @OPModule ";
                parameter.Add(DbFactory.CreateDbParameter("@OPModule", "%" + OPModule + "%"));
            }
            if (!String.IsNullOrEmpty(OPAction))
            {
                sql += " and OPAction like @OPAction ";
                parameter.Add(DbFactory.CreateDbParameter("@OPAction", "%" + OPAction + "%"));
            }
            if (!String.IsNullOrEmpty(OPType))
            {
                sql += " and OPType like @OPType ";
                parameter.Add(DbFactory.CreateDbParameter("@OPType", "%" + OPType + "%"));
            }
            if (!String.IsNullOrEmpty(OPResult))
            {
                sql += " and OPResult like @OPResult ";
                parameter.Add(DbFactory.CreateDbParameter("@OPResult", "%" + OPResult + "%"));
            }


            //开始时间
            //if (!String.IsNullOrEmpty(TimeStart))
            //{
            //    sql += $" and DATEDIFF(day,'{TimeStart}',OPTime) >= 0 ";
            //}

            ////结束时间
            //if (!String.IsNullOrEmpty(TimeEnd))
            //{
            //    sql += $" and DATEDIFF(day,OPTime,'{TimeEnd}') >= 0 ";
            //}
            //开始时间
            if (!String.IsNullOrEmpty(TimeStart))
            {
                //strSql.Append(" and DateDiff(dd,AcqTm,getdate()) <= DateDiff(dd,'" + TimeStart + "',getdate()) ");
                //开始时间把@放在前面
                sql += " and DateDiff(day,@TimeStart,OPTime) >=0 ";
                parameter.Add(DbFactory.CreateDbParameter("@TimeStart", TimeStart));
            }
            //结束时间
            if (!String.IsNullOrEmpty(TimeEnd))
            {
                //strSql.Append(" and DateDiff(dd,AcqTm,getdate()) >= DateDiff(dd,'" + TimeEnd + "',getdate()) ");
                //结束时间把@放在后面
                sql += " and DateDiff(day,OPTime,@TimeEnd) >=0 ";
                parameter.Add(DbFactory.CreateDbParameter("@TimeEnd", TimeEnd));
            }
            sql += $" order by {jqgridparam.sidx} {jqgridparam.sord} ";
            //dt = Repository().FindTableBySql(sql.ToString(), false);
            dt = DataFactory.Database().FindTableBySql(sql, parameter.ToArray(), false);
            return dt;
        }

        public int RemoveLog(string KeepTime)
        {
            StringBuilder strSql = new StringBuilder();
            DateTime OPTime = DateTime.Now;

            OPTime = DateTime.Now.AddMonths(-(Convert.ToInt32(KeepTime)));
            strSql.Append($"DELETE P_ProductionLog where OPTime < '{OPTime}'");
            return DataFactory.Database().ExecuteBySql(strSql);
        }
    }
}