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
    /// 完整计划接收过程表
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.08.29 10:48</date>
    /// </author>
    /// </summary>
    public class ProducePlanBll : RepositoryFactory<P_ProducePlan_Pro>
    {
        public List<P_ProducePlan_Pro> GridPageJson(string Condition, string StartTime, string EndTime, string State, string Keywords, JqGridParam jqgridparam)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT * FROM P_ProducePlan_Pro WHERE Enabled='1' AND " + Condition);

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
            var s = Repository().FindListPageBySql(strSql.ToString(), parameter.ToArray(), ref jqgridparam);
            return Repository().FindListPageBySql(strSql.ToString(), parameter.ToArray(), ref jqgridparam);
        }



        #region 删除方法
        //array 需要删除的信息的主键的数组
        //删除表中某一条数据表示将表中该条数据的Enabled设置为0，并不是真的删除该数据
        //返回值为1，或者0
        //1表示操作成功，0表示操作失败
        public int Delete(string[] array)
        {
            //创建一个表格的list，用于存储通过主键查询到的信息

            foreach (string keyValue in array)
            {
                ArrayList sqllist = new ArrayList();
                //===复制时需要修改===
                string delete_plan = $"DELETE from P_ProducePlan_Pro where VIN = '{keyValue}'";
                string delete_pline = $"DELETE from P_LineProductionQueue_Pro where VIN = '{keyValue}'";
                string delete_pfeedback = $"DELETE from P_PlanFeedBack_Pro where VIN = '{keyValue}'";
                string delete_precord = $"DELETE from P_CarPastRecordInfo where VIN = '{keyValue}'";
                string delete_ppastinfo = $"DELETE from P_CarPastInfo_Pro where VIN = '{keyValue}'";
                sqllist.Add(delete_plan);
                sqllist.Add(delete_pline);
                sqllist.Add(delete_pfeedback);
                sqllist.Add(delete_precord);
                sqllist.Add(delete_ppastinfo);
                string isok = DbHelperSQL.ExecuteSqlTran1(sqllist);
                if (isok != "success")
                {
                    return 0;
                }

            }
            return 1;
        }
        #endregion



    }
}