//=====================================================================================
// All Rights Reserved , Copyright @ HfutIE 2021
// Software Developers @ HfutIE 2021
//=====================================================================================

using HfutIE.DataAccess;
using HfutIE.Entity;
using HfutIE.Repository;
using HfutIE.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace HfutIE.Business
{
    /// <summary>
    /// 小时计划接收过程表
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.08.29 11:12</date>
    /// </author>
    /// </summary>
    public class HourProducePlanBll : RepositoryFactory<P_HourProducePlan_Pro>
    {
        public List<P_HourProducePlan_Pro> GridPageJson(string Condition, string StartTime, string EndTime, string State, string Keywords, JqGridParam jqgridparam)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT * FROM P_HourProducePlan_Pro WHERE Enabled='1' AND " + Condition);

            if (Condition == "PlanTime" || Condition == "PlanPublishTime")
            {
                parameter.Add(DbFactory.CreateDbParameter("@StartTime", CommonHelper.GetDateTime(StartTime + " 00:00")));
                parameter.Add(DbFactory.CreateDbParameter("@EndTime", CommonHelper.GetDateTime(EndTime + " 23:59")));
                strSql.Append(" Between @StartTime AND @EndTime ");
            }
            else if (Condition == "OrderStatus" || Condition == "PlanStatus")
            {
                strSql.Append(" = '" + State + "'");
            }
            else
            {
                strSql.Append(" Like '%" + Keywords + "%'");
            }

            return Repository().FindListPageBySql(strSql.ToString(), parameter.ToArray(), ref jqgridparam);
        }
    }
}