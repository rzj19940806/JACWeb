//=====================================================================================
// All Rights Reserved , Copyright @ HfutIE 2017
// Software Developers @ HfutIE 2017
//=====================================================================================

using HfutIE.Entity;
using HfutIE.Repository;
using HfutIE.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using HfutIE.DataAccess;
using System.Data.SqlClient;
using System;

namespace HfutIE.Business
{
    /// <summary>
    /// INTERFACE_C
    /// <author>
    ///		<name>she</name>
    ///		<date>2017.06.05 08:47</date>
    /// </author>
    /// </summary>
    public class INTERFACE_CBll : RepositoryFactory<INTERFACE_C>
    {
        public DataTable GetList()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@" SELECT  [interface_key]
      ,[interface_code]
      ,[interface_name]
      ,[interface_type]
      ,[interface_ip]
      ,[database_name]
      ,[user_name]
      ,[password]
      ,[start_time]
      ,[frequency]
      ,[remarks]
      ,[CreateDate]
      ,[CreateUserId]
      ,[CreateUserName]
      ,[ModifyDate]
      ,[ModifyUserId]
      ,[ModifyUserName]
  FROM [JAC_FrontAxle].[dbo].[INTERFACE_C]
                                     ");
            List<DbParameter> parameter = new List<DbParameter>();           
            return Repository().FindTableBySql(strSql.ToString(), parameter.ToArray());
        }
        public DataTable GetPageList(string keyword, string Condition, string ParameterJson, ref JqGridParam jqgridparam)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();

            strSql.Append(@" SELECT   [interface_key]
                                     ,[interface_code]
                                     ,[interface_name]
                                     ,[interface_type]
                                     ,[interface_ip]
                                     ,[database_name]
                                     ,[user_name]
                                     ,[password]
                                     ,[start_time]
                                     ,[frequency]
                                     ,[remarks]
                                     ,[CreateDate]
                                     ,[CreateUserId]
                                     ,[CreateUserName]
                                     ,[ModifyDate]
                                     ,[ModifyUserId]
                                     ,[ModifyUserName]
                                     ,[from_table_name]
                                     ,[rx_mode]
                                     ,[period]
                                     ,[period_unit]
                                     ,[to_table_name]
                                     FROM [JAC_FrontAxle].[dbo].[INTERFACE_C]");
            if (!string.IsNullOrEmpty(keyword))
            {
                strSql.Append(@" where  " + Condition + " LIKE  @keyword  ");

                parameter.Add(DbFactory.CreateDbParameter("@keyword", '%' + keyword + '%'));
            }
            if (!string.IsNullOrEmpty(ParameterJson))
            {
                List<DbParameter> outparameter = new List<DbParameter>();
                strSql.Append(ConditionBuilder.GetWhereSql(ParameterJson.JonsToList<Condition>(), out outparameter));
                parameter.AddRange(outparameter);
            }
            return Repository().FindTablePageBySql(strSql.ToString(), parameter.ToArray(), ref jqgridparam);
        }
        public DataTable SqlTableList()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@" select name from sysobjects where xtype='u' order by name ");//查出数据库中所有的表的名称
            return Repository().FindTableBySql(strSql.ToString());
        }
        public int CheckTableExist(INTERFACE_C entity)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select count(*) From sysobjects where name='" + entity.from_table_name + "'");//查出数据库中所有的表的名称
            int count = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection("server=" + entity.interface_ip + ";database=" + entity.database_name + ";user id=" + entity.user_name + ";password=" + entity.password + ";Connection Timeout=2;"))
                {
                    int a = connection.ConnectionTimeout;
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand(strSql.ToString(), connection))
                    {
                        count =Convert.ToInt16(cmd.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("未找到相应服务器，请重试。");
            }
            return count;
        }
    }
}